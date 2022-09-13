using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Ports;

namespace ScaleAddon.Forms
{
    public partial class ScaleCalibration2 : Form
    {
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public UserModel Userlog { get; set; }
        public string ClientID { get; set; }
        public ScaleComModel ScaleCom { get; set; }

        private DataTable dtDetail;

        private DateTime currentDate;

        private bool curIsActive;

        private SerialPort port;

        private DateTime s_tgl;
        //add new
        private DateTime s_tgl2;
        public ScaleCalibration2()
        {
            InitializeComponent();
           
        }




       
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == System.IO.Ports.SerialData.Eof) return;
            if (!port.IsOpen) return;
            // Show all the incoming data in the port's buffer
            string readMsg = port.ReadLine();

            string prefix = ScaleCom.Prefix;
            string postfix = ScaleCom.Postfix;

            if (readMsg.Contains(prefix))
            {
                if (prefix.Length > 0) { readMsg = readMsg.Replace(prefix, ""); }
                if (postfix.Length > 0) { readMsg = readMsg.Replace(postfix, ""); }
                setText(readMsg, tbScale);
            }
        }

        public delegate void setTextCallback(string someText, Control tb);
        
        public void setText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new setTextCallback(setText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text = someText;
            }
        }

        private void startSerial()
        {
            string comPort = ScaleCom.Port;
            int baudrate = Convert.ToInt32(ScaleCom.Baudrate);
            Parity parity = Parity.None;
            string sParity = ScaleCom.Parity;
            switch (sParity)
            {
                case "Even":
                    parity = Parity.Even;
                    break;

                case "mark":
                    parity = Parity.Mark;
                    break;

                case "Odd":
                    parity = Parity.Odd;
                    break;

                case "Space":
                    parity = Parity.Space;
                    break;

                default:
                    parity = Parity.None;
                    break;
            }
            int databit = Convert.ToInt32(ScaleCom.Databits);
            StopBits stopBits = StopBits.One;
            string sStopBits = ScaleCom.Stopbits;
            switch (sStopBits)
            {
                case "":
                    stopBits = StopBits.None;
                    break;

                case "0":
                    stopBits = StopBits.None;
                    break;

                case "1.5":
                    stopBits = StopBits.OnePointFive;
                    break;

                case "2":
                    stopBits = StopBits.Two;
                    break;

                default:
                    stopBits = StopBits.One;
                    break;
            }

            port = new SerialPort(comPort, baudrate, parity, databit, stopBits);
            port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            try
            {
                port.Open();
            }
            catch (Exception ee)
            {
                //do nothing
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
         /*   if (!aa(tbDate.Text))
            {*/
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_ScaleCalibration", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", $"{Warehouse.WarehouseID}-{ClientID}-{currentDate.ToString("yyMMdd-HHmm")}");
                            command.Parameters.AddWithValue("@DocumentDate", currentDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            command.Parameters.AddWithValue("@WarehouseID", Warehouse.WarehouseID);
                            command.Parameters.AddWithValue("@ClientID", ClientID);
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        loadHistory();
                        loadCalibration();
                    }
                }
            /*}
            else
            {
                MessageBox.Show($"Document {tbDocNumber.Text} already exists to input weighing test items", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
        }
        private void loadHistory()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtDetail = new DataTable();
                try
                {
                    string query = $@"SELECT
                                            *
                                        FROM
	                                        ScaleCalibration
                                        WHERE ClientID = '{ClientID}'
                                            AND WarehouseID = '{Warehouse.WarehouseID}'
                                            AND
                                        DocumentDate BETWEEN '{s_tgl2.Date.ToString("yyyy-MM-dd")}' AND '{s_tgl.Date.ToString("yyyy-MM-dd 23:59:59")}'
                                            AND DocumentID LIKE '%{tbFilter.Text}%'
                                        ORDER BY DocumentDate Desc";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                    dgvDetail.DataSource = dtDetail;

                    //Header Text
                    dgvDetail.Columns[0].HeaderText = "Document ID";
                    dgvDetail.Columns[1].HeaderText = "Date-Time";
                    dgvDetail.Columns[2].HeaderText = "Warehouse ID";
                    dgvDetail.Columns[2].Visible = false;
                    dgvDetail.Columns[3].HeaderText = "Client ID";
                    dgvDetail.Columns[3].Visible = false;
                    dgvDetail.Columns[4].HeaderText = "Creator ID";
                    dgvDetail.Columns[5].HeaderText = "Created Date";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Modified Date";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.ClearSelection();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
          
        }

        private void loadCalibration()
        {
            ScaleCalibration.ClearCalibration();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from ScaleCalibration WHERE WarehouseID='{Warehouse.WarehouseID}' " +
                        $"AND ClientID = '{ClientID}' ORDER BY DocumentDate Desc";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        ScaleCalibration.DocumentID = reader.GetValue(0).ToString();
                        ScaleCalibration.DocumentDate = reader.GetValue(1).ToString();
                        ScaleCalibration.WarehouseID = reader.GetValue(2).ToString();
                        ScaleCalibration.ClientID = reader.GetValue(3).ToString();
                        ScaleCalibration.CreatorID = reader.GetValue(4).ToString();

                        tbDocNumber.Text = ScaleCalibration.DocumentID;
                        tbDate.Text = ScaleCalibration.DocumentDate;
                        tbWarehouse.Text = ScaleCalibration.WarehouseID;
                        tbClientID.Text = ScaleCalibration.ClientID;
                        tbCreatorID.Text = ScaleCalibration.CreatorID;
                        tbStatus.Text = ScaleCalibration.isActive() == true ? "Active" : "Expired";

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        MessageBox.Show("Tidak dapat menemukan info kalibrasi", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }//end using connection
        }

        private void ScaleCalibration2_Load(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Scale Calibration";
            currentDate = DateTime.Now;
            s_tgl  = DateTime.Now;
            s_tgl2 = DateTime.Now.AddDays(-1);
            dtpListDate2.Value = DateTime.Now.AddDays(-1);
            loadCalibration();
            loadHistory();
        }
        public void OpenPort()
        {

            /* if (ScaleCalibration.isActive())
                {*/
            /*  if (port != null && port.IsOpen)
              {
                  port.Close();
                  tbScale.BackColor = System.Drawing.SystemColors.ActiveBorder;
                  tbScale.Text = "0.00";
              }
              else
              {*/
            // close
            try
            {
                port.Close();
            }
            catch (System.Exception ex)
            {

            }
            finally {

                MessageBox.Show("Konek ke timbangan","Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (port != null) port.Dispose();
                startSerial();
                if (port.IsOpen) tbScale.BackColor = System.Drawing.SystemColors.ActiveCaption;
            }

                   
               /* }*/
         /*   }
            else
            {
                MessageBox.Show($"Scale calibration is expired!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
        }

     
        public int  getitem(string nodoc) {
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                connection.Open();
                string query = $@"SELECT
                                            count(id) as a
                                        FROM
	                                        ScaleCalibrationDetail
                                

                                WHERE DocumentID='{nodoc}'
                                        ";
                SqlCommand command = new SqlCommand(query, connection);


                Int32 count = (Int32)command.ExecuteScalar();


                if (count < 1) {
                    textBox2.Text = $"1";
                } else if (count < 2) {
                    textBox2.Text = $"2";
                }
                else if (count < 3) {
                    textBox2.Text = $"3";
                }
                else if (count < 4) {
                    textBox2.Text = $"4";
                }
                else if (count < 5) {
                    textBox2.Text = $"5";

                } else if (count < 6) {
                    textBox2.Text = $"4";
                } else if (count < 7) {
                    textBox2.Text = $"3";
                } else if (count < 8)
                {
                    textBox2.Text = $"2";
                } else if (count < 9)
                {
                    textBox2.Text = $"1";
                }

                return count;

                connection.Close();
            }

        }

        int indexdetail;
        private void getindexheader(object sender, DataGridViewCellEventArgs e)
        {
            indexdetail = e.RowIndex;
            getdetail(indexdetail);
        }
        string nodoc2;
        public void getdetail(int index) {

            if (index != -1)
            {

                var nodoc = dgvDetail.Rows[index].Cells[0].Value.ToString();
                 nodoc2=nodoc;
                textBox4.Text = dgvDetail.Rows[index].Cells[4].Value.ToString();
                textBox6.Text = dgvDetail.Rows[index].Cells[0].Value.ToString();
                textBox7.Text = dgvDetail.Rows[index].Cells[1].Value.ToString();
                textBox8.Text = aa(dgvDetail.Rows[index].Cells[1].Value.ToString()) == true ? "Active" : "View";
               int jumlah_pemberat = getitem(textBox6.Text);
                if (aa(dgvDetail.Rows[index].Cells[1].Value.ToString()) == true)
                {
                    button1.Enabled = true;
                }
                else
                {

                    button1.Enabled = false;
                }



                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //create new dt
                    dtDetail = new DataTable();
                    try
                    {
                        string query = $@"SELECT
                                            *
                                        FROM
	                                        ScaleCalibrationDetail
                                        WHERE
                                            DocumentID = '{nodoc}'";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtDetail);
                        
                        dataGridView1.DataSource = dtDetail;

                        //Header Text
                        dataGridView1.Columns[0].HeaderText = "ID";
                        dataGridView1.Columns[0].Visible = false;
                        dataGridView1.Columns[1].HeaderText = "Document ID";
                        dataGridView1.Columns[2].HeaderText = "Number Of Ballast";
                        dataGridView1.Columns[3].HeaderText = "Weight Value";
                        dataGridView1.Columns[4].HeaderText = "Total Weight";
                        dataGridView1.Columns[4].HeaderText = "Total Weight";
                        dataGridView1.Columns[5].HeaderText = "Real Total Weight";
                        dataGridView1.Columns[6].HeaderText = "Difference";
                       
                        dataGridView1.ClearSelection();

                      
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.ToString());
                    }
                }
            }
        }

        private void dgvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupDetail_Enter(object sender, EventArgs e)
        {

        }

        public bool aa(String DocumentDate) {

            DateTime docDate = DateTime.Parse(DocumentDate);
            DateTime curDate = DateTime.Now;
            DateTime curDatelatter = DateTime.Now.AddDays(-1);
         

                var curDate1 = DateTime.Parse(curDate.ToString("yyyy MM dd 22:00:00"));
                var curDate2 = DateTime.Parse(curDate.ToString("yyyy MM dd 11:00:00"));
                var curDate3 = DateTime.Parse(curDatelatter.ToString("yyyy MM dd 21:59:59"));
                var curDateNow = DateTime.Parse(curDate.ToString("yyyy MM dd HH:mm:ss"));
                var docDate1 = DateTime.Parse(docDate.ToString("yyyy MM dd HH:mm:ss"));

                Console.WriteLine(curDate.ToString("yyyy MM dd 14:00:00"));
                Console.WriteLine(docDate.ToString("yyyy MM dd HH:mm:ss"));
                TimeSpan duration = curDate.Subtract(docDate);


                if (docDate1 < curDate1 && docDate1 > curDate2)
                {
                    Console.WriteLine(5);
                    if (curDateNow < curDate1)
                    {
                        Console.WriteLine(3);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(4);
                        return false;
                    }
                }
                else if (docDate1 < curDate2 && docDate1 > curDate3)
                {
                    Console.WriteLine(6);
                    if (curDateNow < curDate2)
                    {
                        Console.WriteLine(1);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine(2);
                        return false;
                    }

                }
                else
                {
                    Console.WriteLine(7);
                    return false;
                }
            

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int jumlah_pemberat = getitem(textBox6.Text);
            if (jumlah_pemberat < 9)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string a = textBox7.Text;
                    if (textBox6.Text != $"<Add Item>" && textBox7.Text != null && textBox2.Text != $"" && textBox3.Text != $"")
                    {
                        try
                        {
                            decimal selisih = decimal.Parse($"{tbScale.Text}") - decimal.Parse($"{textBox1.Text}");
                            decimal selisih1 = Math.Abs(selisih);
                            decimal maxselisih = decimal.Parse("0.2");
                            if (selisih1 <= maxselisih)
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }


                                using (SqlCommand command = new SqlCommand("Insert_ScaleCalibrationDetail", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", $"{textBox6.Text}");
                                    command.Parameters.AddWithValue("@NumberOfBallast", decimal.Parse($"{textBox2.Text}"));
                                    command.Parameters.AddWithValue("@WeightValue", decimal.Parse($"{textBox3.Text}"));
                                    command.Parameters.AddWithValue("@TotalWeight", decimal.Parse($"{textBox1.Text}"));
                                    command.Parameters.AddWithValue("@RealTotalWeight", decimal.Parse($"{tbScale.Text}"));
                                    command.Parameters.AddWithValue("@Difference", selisih1);

                                    command.ExecuteNonQuery();


                                    MessageBox.Show($"Success Save", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    getdetail(indexdetail);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Perbedaan skala lebih dari 0,2 KG. Tolong perbaiki timbangannya", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        finally
                        {

                            loadHistory();
                            loadCalibration();

                        }
                    }
                    else
                    {


                        MessageBox.Show($"Silahkan isi inputan atau pilih dokumen", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }
            }
            else {
                MessageBox.Show($"uji penimbangan telah mencapai 9 kali", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void Setbullets(object sender, EventArgs e)
        {
            decimal a = 0;
            decimal b = 0;

            try
            {
                a = decimal.Parse(textBox2.Text);

            }catch (Exception erw) {

                a = 0;
            }

            try
            {
                b = decimal.Parse(textBox3.Text);
            }
            catch (Exception qwe)
            {
                a = 0;

            }

            decimal c = a * b;
            textBox1.Text = c.ToString("N2");
        }

        private void SetWeight(object sender, EventArgs e)
        {

            decimal a = 0;
            decimal b = 0;

            try
            {
                a = decimal.Parse(textBox2.Text);

            }
            catch (Exception erw)
            {

                a = 0;
            }

            try
            {
                b = decimal.Parse(textBox3.Text);
            }
            catch (Exception qwe)
            {
                a = 0;

            }

            decimal c = a * b;
            textBox1.Text = c.ToString("N2");
        }

        private void tbCreatorID_TextChanged(object sender, EventArgs e)
        {

        }

        private void ScaleCalibration2_Close(object sender, FormClosingEventArgs e)
        {
            /*port.Close();*/ // tutup port
        }

        private void s_search(object sender, EventArgs e)
        {
            s_tgl = dtpListDate.Value;
            if (dtpListDate2.Value > dtpListDate.Value)
            {
                dtpListDate2.Value = dtpListDate.Value;
            }
            s_tgl2 = dtpListDate2.Value;
            loadHistory();
        }

        private void s_search2(object sender, EventArgs e)
        {

        }

        private void s_search_klik(object sender, EventArgs e)
        {
            loadHistory();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox6.Text != $"<Add Item>" && textBox7.Text != null  && textBox3.Text != $"")
            {
                DataSetAddon myDoc = new DataSetAddon();
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {

                        string query = $@"SELECT
                                            *
                                        FROM
	                                        ScaleCalibrationDetail
                                        WHERE
                                            DocumentID = '{nodoc2}'";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(myDoc.ScaleCalibrationDetail);
                        Console.WriteLine(1);
                        Console.WriteLine(myDoc.ScaleCalibrationDetail);


                        ScaleCalibrationPrint2 ScaleCalibrationPrint2 = new ScaleCalibrationPrint2
                        {
                            
                            Company = Warehouse.Company,
                            WarehouseDescr = Warehouse.Descr,
                            Address = Warehouse.AddressLine1,
                            Phone = Warehouse.Phone1,
                            docNum = textBox6.Text,
                            docdate = textBox7.Text,
                            ClientID = ClientID,
                            Warehouse = Warehouse.WarehouseID,
                            DocType = "SCALE CALIBRATION",
                            CreatorID = $"{textBox4.Text}",
                            DocDetails = myDoc
                        };
                        ScaleCalibrationPrint2.ShowDialog();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                }

            }
            else {
                MessageBox.Show($"Silahkan isi inputan atau pilih dokumen", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenPort();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }
    }
}
