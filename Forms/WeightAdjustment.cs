using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using Acumatica.ULT_18_200_001.Api;
using Acumatica.ULT_18_200_001.Model;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class WeightAdjustment : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }
        private int lastIncrementValue = -1;

        private DataTable dtLot;
        private DataTable dtDetail;
        private DataTable dtEntry;

        private string LotStockItem = null;

        private SerialPort port;

        public WeightAdjustment()
        {
            InitializeComponent();
        }

        private void WeightAdjustment_Load(object sender, EventArgs e)
        {
            dtEntry = GetTable();

            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
                loadEntryGroup();
                loadDetail();
                resetEntry();
            }

            //enable scale override
            if (ScaleCom.Manual)
            {
                btnScaleOverride.Enabled = true;
                btnScaleOverride.Visible = true;
            }
            else
            {
                btnScaleOverride.Enabled = false;
                btnScaleOverride.Visible = false;
            }
        }

        #region ThreadSafe

        public delegate void addTextCallback(string someText, Control tb);

        public delegate void setTextCallback(string someText, Control tb);

        public void addText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new addTextCallback(addText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text += someText;
            }
        }

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

        #endregion ThreadSafe

        #region serialPort

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

        private void BuyingProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                //port.Close();
                port.Dispose();
            }
        }

        #endregion serialPort

        private static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("DocumentID", typeof(string));
            table.Columns.Add("InventoryID", typeof(string));
            table.Columns.Add("SubItem", typeof(string));
            table.Columns.Add("LotNbr", typeof(string));
            table.Columns.Add("Source", typeof(string));
            table.Columns.Add("Stage", typeof(string));
            table.Columns.Add("Form", typeof(string));
            table.Columns.Add("CropYear", typeof(string));
            table.Columns.Add("Grade", typeof(string));
            table.Columns.Add("Area", typeof(string));
            table.Columns.Add("Color", typeof(string));
            table.Columns.Add("Fermentation", typeof(string));
            table.Columns.Add("Length", typeof(string));
            table.Columns.Add("Process", typeof(string));
            table.Columns.Add("StalkPosition", typeof(string));
            table.Columns.Add("WeightRope", typeof(decimal));
            table.Columns.Add("WeightShipping", typeof(decimal));
            table.Columns.Add("WeightReceive", typeof(decimal));
            table.Columns.Add("WeightTare", typeof(decimal));
            table.Columns.Add("WeightNetto", typeof(decimal));
            table.Columns.Add("WeightReceiveNew", typeof(decimal));
            table.Columns.Add("WeightTareNew", typeof(decimal));
            table.Columns.Add("WeightNettoNew", typeof(decimal));
            table.Columns.Add("UoM", typeof(string));
            table.Columns.Add("CostUnit", typeof(decimal));
            table.Columns.Add("Remark", typeof(string));
            table.Columns.Add("OldDocumentID", typeof(string));
            table.Columns.Add("BuyerName", typeof(string));

            return table;
        }

        private void loadProcess()
        {
            tbDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}]";

            loadComboLot();

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from WeightAdjustLine where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(3).ToString();
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaIssue.Text = reader.GetValue(4).ToString();
                        tbAcumaticaReceipt.Text = reader.GetValue(5).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(3).ToString() == "HOLD" ? 1 : 0);
                        remark.Text = reader.GetValue(9).ToString();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }


        }

        private void resetScreen()
        {
            currentDate = DateTime.Now;
            tbDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";
            checkHold.Checked = true;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}]";

            loadComboLot();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaIssue.Text = "";
            tbAcumaticaReceipt.Text = "";

            loadEntryGroup();
            loadDetail();
        }

        private void resetEntry()
        {
            dtEntry = GetTable();

            loadComboLot();

            loadEntryGroup();

            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = true;
                    btnSave.Enabled = true;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = true;
                    break;

                case "SYNCED":
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintDoc.Enabled = true;
                    break;

                default:
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = true;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = false;
                    break;
            }
        }

        private void loadEntryGroup()
        {
            dgvEntry.DataSource = dtEntry;

            //Header Text
            dgvEntry.Columns[0].HeaderText = "Document ID";
            dgvEntry.Columns[0].Visible = false;
            dgvEntry.Columns[1].HeaderText = "Inventory ID";
            dgvEntry.Columns[2].HeaderText = "Sub Item";
            dgvEntry.Columns[3].HeaderText = "Lot Number";
            dgvEntry.Columns[4].HeaderText = "Source";
            dgvEntry.Columns[4].Visible = false;
            dgvEntry.Columns[5].HeaderText = "Stage";
            dgvEntry.Columns[5].Visible = false;
            dgvEntry.Columns[6].HeaderText = "Form";
            dgvEntry.Columns[6].Visible = false;
            dgvEntry.Columns[7].HeaderText = "Crop Year";
            dgvEntry.Columns[7].Visible = false;
            dgvEntry.Columns[8].HeaderText = "Grade";
            dgvEntry.Columns[8].Visible = false;
            dgvEntry.Columns[9].HeaderText = "Area";
            dgvEntry.Columns[9].Visible = false;
            dgvEntry.Columns[10].HeaderText = "Color";
            dgvEntry.Columns[10].Visible = false;
            dgvEntry.Columns[11].HeaderText = "Fermentation";
            dgvEntry.Columns[11].Visible = false;
            dgvEntry.Columns[12].HeaderText = "Length";
            dgvEntry.Columns[12].Visible = false;
            dgvEntry.Columns[13].HeaderText = "Process";
            dgvEntry.Columns[13].Visible = false;
            dgvEntry.Columns[14].HeaderText = "Stalk Position";
            dgvEntry.Columns[14].Visible = false;
            dgvEntry.Columns[15].HeaderText = "Rope";
            dgvEntry.Columns[15].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[15].Visible = false;
            dgvEntry.Columns[16].HeaderText = "Shipping";
            dgvEntry.Columns[16].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[16].Visible = false;
            dgvEntry.Columns[17].HeaderText = "Receive";
            dgvEntry.Columns[17].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[18].HeaderText = "Tare";
            dgvEntry.Columns[18].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[19].HeaderText = "Netto";
            dgvEntry.Columns[19].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[20].HeaderText = "Receive (NEW)";
            dgvEntry.Columns[20].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[21].HeaderText = "Tare (NEW)";
            dgvEntry.Columns[21].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[22].HeaderText = "Netto (NEW)";
            dgvEntry.Columns[22].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[23].HeaderText = "UOM";
            dgvEntry.Columns[23].Visible = false;
            dgvEntry.Columns[24].HeaderText = "CostUnit";
            dgvEntry.Columns[24].DefaultCellStyle.Format = "N2";
            dgvEntry.Columns[24].Visible = false;
            dgvEntry.Columns[25].HeaderText = "Remark";
            dgvEntry.Columns[25].Visible = false;
            dgvEntry.Columns[26].HeaderText = "Old Document ID";
            dgvEntry.Columns[26].Visible = false;
            dgvEntry.Columns[27].HeaderText = "Buyer Name";
            dgvEntry.Columns[27].Visible = false;

            dgvEntry.ClearSelection();

            if (dtEntry.Rows.Count > 0)
            {
                decimal sumWReceive = Convert.ToDecimal(dtEntry.Compute("SUM(WeightReceive)", string.Empty));
                tbEntryWReceive.Text = sumWReceive.ToString("N2");
                decimal sumWTare = Convert.ToDecimal(dtEntry.Compute("SUM(WeightTare)", string.Empty));
                tbEntryWTare.Text = sumWTare.ToString("N2");
                decimal sumWNetto = Convert.ToDecimal(dtEntry.Compute("SUM(WeightNetto)", string.Empty));
                tbEntryWNetto.Text = sumWNetto.ToString("N2");

                //disable change reg
            }
            else
            {
                decimal sumWReceive = 0M;
                tbEntryWReceive.Text = sumWReceive.ToString("N2");
                decimal sumWTare = 0M;
                tbEntryWTare.Text = sumWTare.ToString("N2");
                decimal sumWNetto = 0M;
                tbEntryWNetto.Text = sumWNetto.ToString("N2");

                tbEntryWNetto.Text = sumWNetto.ToString("N2");
                //enable change reg
            }
        }

        private void loadDetail()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtDetail = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        WeightAdjustLineDetail
                                        WHERE
	                                        DocumentID = '{DocNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                    dgvDetail.DataSource = dtDetail;

                    //Header Text
                    dgvDetail.Columns[0].HeaderText = "Document ID";
                    dgvDetail.Columns[0].Visible = false;
                    dgvDetail.Columns[1].HeaderText = "Inventory ID";
                    dgvDetail.Columns[2].HeaderText = "Sub Item";
                    dgvDetail.Columns[3].HeaderText = "Lot Number";
                    dgvDetail.Columns[4].HeaderText = "Source";
                    dgvDetail.Columns[4].Visible = false;
                    dgvDetail.Columns[5].HeaderText = "Stage";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Form";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.Columns[7].HeaderText = "Crop Year";
                    dgvDetail.Columns[7].Visible = false;
                    dgvDetail.Columns[8].HeaderText = "Grade";
                    //dgvDetail.Columns[8].Visible = false;
                    dgvDetail.Columns[9].HeaderText = "Area";
                    dgvDetail.Columns[9].Visible = false;
                    dgvDetail.Columns[10].HeaderText = "Color";
                    dgvDetail.Columns[10].Visible = false;
                    dgvDetail.Columns[11].HeaderText = "Fermentation";
                    dgvDetail.Columns[11].Visible = false;
                    dgvDetail.Columns[12].HeaderText = "Length";
                    dgvDetail.Columns[12].Visible = false;
                    dgvDetail.Columns[13].HeaderText = "Process";
                    dgvDetail.Columns[13].Visible = false;
                    dgvDetail.Columns[14].HeaderText = "Stalk";
                    dgvDetail.Columns[14].Visible = false;
                    dgvDetail.Columns[15].HeaderText = "Rope";
                    dgvDetail.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[15].Visible = false;
                    dgvDetail.Columns[16].HeaderText = "Shipping";
                    dgvDetail.Columns[16].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[16].Visible = false;
                    dgvDetail.Columns[17].HeaderText = "Receive";
                    dgvDetail.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[18].HeaderText = "Tare";
                    dgvDetail.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[19].HeaderText = "Netto";
                    dgvDetail.Columns[19].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[20].HeaderText = "UoM";
                    dgvDetail.Columns[21].HeaderText = "Unit Price";
                    dgvDetail.Columns[21].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[21].Visible = false;
                    dgvDetail.Columns[22].HeaderText = "Remark";
                    dgvDetail.Columns[23].HeaderText = "Synced";
                    dgvDetail.Columns[24].HeaderText = "Old Document ID";

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        int countLot = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", string.Empty));
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWReceive = Convert.ToDecimal(dtDetail.Compute("SUM(WeightReceive)", string.Empty));
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = Convert.ToDecimal(dtDetail.Compute("SUM(WeightTare)", string.Empty));
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty));
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                    }
                    else
                    {
                        int countLot = 0;
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWReceive = 0M;
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = 0M;
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = 0M;
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbLot.SelectedIndex = -1;
                dtLot = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    StockItem.LotNbr
                                    FROM
	                                    dbo.StockItem
                                    WHERE
	                                    StockItem.StatusStock = 1
                                    AND DocumentID != '{DocNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtLot);
                    string[] arrray = dtLot.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbLot);
                    cbLot.Items.Clear();
                    cbLot.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void getDocLastIncrement()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from NumberingSetting where NumberingID = 'ADJUST'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        DateTime dbDate = Convert.ToDateTime(reader.GetValue(2).ToString());
                        int dbIncrement = Convert.ToInt32(reader.GetValue(1).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        if (currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year == dbDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year)
                        {
                            lastIncrementValue = dbIncrement;
                        }
                        else
                        {
                            lastIncrementValue = 0;
                        }
                    }
                    else
                    {
                        lastIncrementValue = 0;
                    }
                }
                catch (Exception e)
                {
                    lastIncrementValue = -1;
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void setDocNumber()
        {
            getDocLastIncrement();

            if (lastIncrementValue >= 0)
            {

                var currentIncrement = lastIncrementValue + 1;
                //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-BY/IN-{currentIncrement.ToString().PadLeft(4, '0')}";
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-ADJUST-{currentIncrement.ToString().PadLeft(4, '0')}";
                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-ADJUST-{currentIncrement.ToString().PadLeft(4, '0')}";

                if (!checkDocNbrExist(docNbr))
                {
                    tbDocNumber.Text = docNbr;
                }
                else
                {
                    tbDocNumber.Text = "<NEW>";
                    MessageBox.Show($"Document number {docNbr} already exist, or unable to check existing document number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }

        }

        private bool checkDocNbrExist(string docNbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM WeightAdjustLine WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int exist = Convert.ToInt32(reader.GetValue(0).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        if (exist == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return true;
                    //MessageBox.Show(e.ToString());
                }
            }
        }

        private void saveDocument()
        {
            if (remark.Text.Trim() != null && remark.Text.Trim() != "")
            {
                
          
        bool incrementError = false;
            int OperationType = 1;

            if (DocNumber == "<NEW>")
            {
                incrementError = true;
                OperationType = 0;
                setDocNumber();
                //tbStatus.Text = "OPEN";

                if (DocNumber != "<NEW>" && lastIncrementValue >= 0)
                {
                    incrementError = false;

                    if (checkHold.Checked)
                    {
                        tbStatus.Text = "HOLD";
                    }
                    else
                    {
                        tbStatus.Text = "OPEN";
                    }
                    lastIncrementValue = lastIncrementValue + 1;
                    //DocNumber = tbDocNumber.Text;

                }


            }
            bool insertError = false;

            if (!incrementError )
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_WeightAdjustLine_v2", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbDate.Text);
                            command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@AcumaticaIssueRefNbr", tbAcumaticaIssue.Text);
                            command.Parameters.AddWithValue("@AcumaticaReceiptRefNbr", tbAcumaticaReceipt.Text);
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                            command.Parameters.AddWithValue("@OperationType", OperationType);
                            command.Parameters.AddWithValue("@Remark", remark.Text);
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        insertError = true;
                        tbDocNumber.Text = "<NEW>";
                    }
                    //finally
                    //{
                    //    if (!insertError)
                    //    {
                    //        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}]";

                    //        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                    //        {
                    //            command.CommandType = CommandType.StoredProcedure;
                    //            command.Parameters.AddWithValue("@NumberingID", "ADJUSt");
                    //            command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                    //            command.Parameters.AddWithValue("@NumberingDate", currentDate);

                    //            command.ExecuteNonQuery();
                    //        }

                    //        //MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }
                    //}

                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        if (!insertError && this.Text.Contains("<NEW>"))
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", "ADJUSt");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }

                            MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"Update numbering setting failed, please contact IT", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        insertError = true;
                    }

                }
            }
           else
            {

                MessageBox.Show($"Failed to get numbering setting, please check database connection and try to save again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }  



            if (tbStatus.Text == "OPEN" && !checkHold.Checked)
            {
                btnAcumatica.Enabled = true;
            }
            else
            {
                btnAcumatica.Enabled = false;
            }
            if (tbStatus.Text == "OPEN" || tbStatus.Text == "SYNCED")
            {
                btnPrintDoc.Enabled = true;
            }
            else
            {
                btnPrintDoc.Enabled = false;
            }
            }
            else
            {
                MessageBox.Show($"Mandatory Remark", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void loadEntry(string LotNbr)
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        StockItem
                                        WHERE
	                                        LotNbr = '{LotNbr}'
                                        AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        DataRow dr = dtEntry.NewRow();
                        bool alreadyExist = false;

                        dr["DocumentID"] = DocNumber;
                        dr["InventoryID"] = reader.GetValue(1).ToString();
                        dr["SubItem"] = reader.GetValue(2).ToString();
                        dr["LotNbr"] = reader.GetValue(3).ToString();

                        dr["Source"] = reader.GetValue(4).ToString();
                        dr["Stage"] = reader.GetValue(5).ToString();
                        dr["Form"] = reader.GetValue(6).ToString();
                        dr["CropYear"] = reader.GetValue(7).ToString();
                        dr["Grade"] = reader.GetValue(8).ToString();
                        dr["Area"] = reader.GetValue(9).ToString();
                        dr["Color"] = reader.GetValue(10).ToString();
                        dr["Fermentation"] = reader.GetValue(11).ToString();
                        dr["Length"] = reader.GetValue(12).ToString();
                        dr["Process"] = reader.GetValue(13).ToString();
                        dr["StalkPosition"] = reader.GetValue(14).ToString();

                        dr["WeightRope"] = Convert.ToDecimal(reader.GetValue(15).ToString());
                        dr["WeightShipping"] = Convert.ToDecimal(reader.GetValue(16).ToString());
                        dr["WeightReceive"] = Convert.ToDecimal(reader.GetValue(17).ToString());
                        dr["WeightTare"] = Convert.ToDecimal(reader.GetValue(18).ToString());
                        dr["WeightNetto"] = Convert.ToDecimal(reader.GetValue(19).ToString());
                        dr["WeightReceiveNew"] = 0M;
                        dr["WeightTareNew"] = 0M;
                        dr["WeightNettoNew"] = 0M;

                        dr["UOM"] = reader.GetValue(20).ToString();
                        dr["CostUnit"] = 0;
                        dr["Remark"] = reader.GetValue(21).ToString();
                        dr["OldDocumentID"] = reader.GetValue(0).ToString();
                        dr["BuyerName"] = reader.GetValue(24).ToString();

                        for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow drOld = dtEntry.Rows[i];
                            if (drOld["LotNbr"].ToString() == dr["LotNbr"].ToString()) { alreadyExist = true; }
                        }

                        if (!alreadyExist) { dtEntry.Rows.Add(dr); }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void saveEntry()
        {
            if (dtEntry.Rows.Count > 0 )
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtEntry.Rows[i];

                            using (SqlCommand command = new SqlCommand("Insert_WeightAdjustLineDetail", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                command.Parameters.AddWithValue("@InventoryID", dr["InventoryID"].ToString());
                                command.Parameters.AddWithValue("@SubItem", dr["SubItem"].ToString());
                                command.Parameters.AddWithValue("@LotNbr", dr["LotNbr"].ToString());
                                command.Parameters.AddWithValue("@Source", dr["Source"].ToString());
                                command.Parameters.AddWithValue("@Stage", dr["Stage"].ToString());
                                command.Parameters.AddWithValue("@tForm", dr["Form"].ToString());
                                command.Parameters.AddWithValue("@CropYear", dr["CropYear"].ToString());
                                command.Parameters.AddWithValue("@Grade", dr["Grade"].ToString());
                                command.Parameters.AddWithValue("@Area", dr["Area"].ToString());
                                command.Parameters.AddWithValue("@Color", dr["Color"].ToString());
                                command.Parameters.AddWithValue("@Fermentation", dr["Fermentation"].ToString());
                                command.Parameters.AddWithValue("@Length", dr["Length"].ToString());
                                command.Parameters.AddWithValue("@Process", dr["Process"].ToString());
                                command.Parameters.AddWithValue("@StalkPosition", dr["StalkPosition"].ToString());
                                command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(dr["WeightRope"].ToString()));
                                command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(dr["WeightShipping"].ToString()));
                                command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(dr["WeightReceiveNew"].ToString()));
                                command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(dr["WeightTareNew"].ToString()));
                                command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(dr["WeightNettoNew"].ToString()));
                                command.Parameters.AddWithValue("@UoM", dr["UoM"].ToString());
                                command.Parameters.AddWithValue("@CostUnit", Convert.ToDecimal(dr["CostUnit"].ToString()));
                                command.Parameters.AddWithValue("@Remark", dr["Remark"].ToString());
                                command.Parameters.AddWithValue("@SyncDetail", 0);
                                command.Parameters.AddWithValue("@OldDocumentID", dr["OldDocumentID"].ToString());
                                command.Parameters.AddWithValue("@OldWeightReceive", Convert.ToDecimal(dr["WeightReceive"].ToString()));
                                command.Parameters.AddWithValue("@OldWeightTare", Convert.ToDecimal(dr["WeightTare"].ToString()));
                                command.Parameters.AddWithValue("@OldWeightNetto", Convert.ToDecimal(dr["WeightNetto"].ToString()));
                                command.Parameters.AddWithValue("@BuyerName", dr["BuyerName"].ToString());

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        resetEntry();

                        loadDetail();
                        ////fix selected row
                        //foreach (DataGridViewRow row in dgvDetail.Rows)
                        //{
                        //    if (row.Cells[3].Value.ToString() == lotNbr)
                        //        row.Selected = true;
                        //}

                        ////groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                        //btnPrintLot.Enabled = true;
                    }
                }
            }
         
        }

        private void removeLot()
        {
            if (dgvEntry.SelectedRows.Count > 0)
            {
                var lotNbr = dgvEntry.Rows[dgvEntry.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();

                for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtEntry.Rows[i];
                    if (dr["LotNbr"].ToString() == lotNbr) { dr.Delete(); }
                }
                dtEntry.AcceptChanges();

                loadEntryGroup();
            }
        }

        private void calculateWeight()
        {
            //old data
            decimal ReceiveOld = Convert.ToDecimal(tbEntryWReceive.Text);
            decimal TareOld = Convert.ToDecimal(tbEntryWTare.Text);
            decimal NettoOld = Convert.ToDecimal(tbEntryWNetto.Text);

            //new data
            decimal ReceiveNew = Convert.ToDecimal(tbEntryWReceiveNew.Text);
            decimal TareNew = Convert.ToDecimal(tbEntryWTareNew.Text != "" ? tbEntryWTareNew.Text : "0");
            decimal NettoNew = ReceiveNew - TareNew;
            tbEntryWNettoNew.Text = NettoNew.ToString("N2");

            int totalLot = dtEntry.Rows.Count;

            if (totalLot > 1)
            {
                decimal TareDiff = TareNew / totalLot;
                decimal NettoDiff = (NettoOld - NettoNew) / totalLot;

                foreach (DataRow row in dtEntry.Rows)
                {
                    decimal Netto = (decimal)row["WeightNetto"];

                    row["WeightNettoNew"] = Netto - NettoDiff;
                    row["WeightTareNew"] = TareDiff;
                    row["WeightReceiveNew"] = Netto + TareDiff;

                    row.EndEdit();
                    dtEntry.AcceptChanges();
                }
            }
            else if (totalLot == 1)
            {
                foreach (DataRow row in dtEntry.Rows)
                {
                    row["WeightNettoNew"] = NettoNew;
                    row["WeightTareNew"] = TareNew;
                    row["WeightReceiveNew"] = ReceiveNew;

                    row.EndEdit();
                    dtEntry.AcceptChanges();
                }
            }
            else
            {
                //do nothing
            }

            loadEntryGroup();
        }

        private bool checkDocumentSync(string docNbr)
        {
            var docStatus = "";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Select_Status", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", docNbr);

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            docStatus = reader.GetValue(0).ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            if (docStatus == "SYNCED")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Configuration LogIn(AuthApi authApi, string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
        {
            var cookieContainer = new CookieContainer();
            authApi.Configuration.ApiClient.RestClient.CookieContainer = cookieContainer;

            authApi.AuthLogin(new Credentials(username, password, tenant, branch, locale));
            Console.WriteLine("Logged In...");
            var configuration = new Configuration($"{AcumaticaCred.AcumaticaSiteURL}/entity/{AcumaticaCred.AcumaticaEndpointName}/{AcumaticaCred.AcumaticaEndpointVersion}/");

            //share cookie container between API clients because we use different client for authentication and interaction with endpoint
            configuration.ApiClient.RestClient.CookieContainer = authApi.Configuration.ApiClient.RestClient.CookieContainer;
            return configuration;
        }

        private void checkTotalReleaseCost(string referenceNbr, Configuration configuration)
        {
            var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
            decimal totalCost = 0;
            while (true)
            {
                var response = inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr }, expand: "Details");

                if (response.Status == "Released")
                {
                    totalCost = (decimal)response.TotalCost.Value;

                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            foreach (var detail in response.Details)
                            {
                                using (SqlCommand command = new SqlCommand("Update_WeightAdjustLineDetail_Cost", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                    command.Parameters.AddWithValue("@LotNbr", detail.Description.Value.ToString());
                                    //command.Parameters.AddWithValue("@CostUnit", Convert.ToDecimal(detail.UnitCost.Value.ToString()));
                                    command.Parameters.AddWithValue("@CostUnit", totalCost/Convert.ToDecimal(tbDetailWNetto.Text));

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    break;
                }
            }

            //return totalCost;
        }

        #region button

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DocNumber = "<NEW>";
            resetScreen();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
            resetEntry();
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            bool allSynced = true;
            string docNbr = "";
            string referenceIssueNbr = tbAcumaticaIssue.Text ?? "";
            string referenceReceiptNbr = tbAcumaticaReceipt.Text ?? "";
            var docBranch = GetBranch(tbWarehouse.Text, 2);

            loadDetail();
            DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);
            //issue
            ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
            inventoryIssue.Date = Convert.ToDateTime(tbDate.Text);
            inventoryIssue.Description = $"{tbWarehouse.Text} Weight Adjustment Transaction Issue";
            inventoryIssue.ExternalRef = tbDocNumber.Text;
            //inventoryIssue.Hold = false;

            List<ULTInventoryIssueDetail> issueDetails = new List<ULTInventoryIssueDetail>();

            //receipt
            InventoryReceipt inventoryReceipt = new InventoryReceipt();
            inventoryReceipt.Date = Convert.ToDateTime(tbDate.Text);
            inventoryReceipt.Branch = docBranch;
            inventoryReceipt.Description = $"{tbWarehouse.Text} Weight Adjustment Transaction Receipt";
            inventoryReceipt.ExternalRef = tbDocNumber.Text;
            inventoryReceipt.Hold = false;

            List<InventoryReceiptDetail> receiptDetails = new List<InventoryReceiptDetail>();

            ////document details
            foreach (DataRowView rowView in dv_filter)
            {
                //check if doc buying already synced
                if (docNbr != rowView[24].ToString())
                {
                    docNbr = rowView[24].ToString();
                    if (!checkDocumentSync(docNbr))
                    {
                        allSynced = false;
                    }
                }

                ULTInventoryIssueDetail issueDetail = new ULTInventoryIssueDetail();
                issueDetail.InventoryID = rowView[1].ToString();
                issueDetail.Branch = docBranch;
                issueDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                issueDetail.Subitem = rowView[2].ToString().Replace(".", "");
                issueDetail.Quantity = Convert.ToDecimal(rowView[27]);
                issueDetail.Description = rowView[3].ToString();
                issueDetail.Warehouse = tbWarehouse.Text;
                issueDetail.UOM = rowView[20].ToString();
                issueDetail.ReasonCode = "ISSUEADJUST";

                issueDetails.Add(issueDetail);
            }

            inventoryIssue.Details = issueDetails;

            if (!allSynced && tbAcumaticaIssue.Text == "")
            {
                MessageBox.Show($"Documents for bale is not synced to Acumatica!\nPlease sync Buying documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                try
                {

                    var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
                    if (referenceIssueNbr == "")
                    {
                        var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);

                        referenceIssueNbr = responseIssue.ReferenceNbr.ToString();
                        tbAcumaticaIssue.Text = referenceIssueNbr;
                        saveDocument();

                        ReleaseFromHoldInventoryIssue releaseFromHoldInventoryIssue = new ReleaseFromHoldInventoryIssue((ULTInventoryIssue)responseIssue);
                        inventoryIssueApi.InvokeAction(releaseFromHoldInventoryIssue);

                        //throw new InvalidOperationException("error");
                        ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
                        inventoryIssueApi.InvokeAction(releaseInventoryIssue);

                    }
                    else
                    {
                        var responseIssue = inventoryIssueApi.GetList(select: "ReferenceNbr", filter: $"ReferenceNbr eq '{referenceIssueNbr}'");



                        if (responseIssue[0].Status.Value == "Balanced")
                        {

                            ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue[0]);
                            inventoryIssueApi.InvokeAction(releaseInventoryIssue);
                        }

                        if (responseIssue[0].Status.Value == "On Hold")
                        {

                            ReleaseFromHoldInventoryIssue releaseFromHoldInventoryIssue = new ReleaseFromHoldInventoryIssue((ULTInventoryIssue)responseIssue[0]);
                            inventoryIssueApi.InvokeAction(releaseFromHoldInventoryIssue);

                            ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue[0]);
                            inventoryIssueApi.InvokeAction(releaseInventoryIssue);
                        }
                    }
                }
                catch (Exception ePut)
                {
                    MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    syncError = true;
                }
                finally
                {
                    //_ = checkTotalReleaseCost(referenceIssueNbr, configuration).ToString("N2");
                    //checkTotalReleaseCost(referenceIssueNbr, configuration);
                    if (!syncError)
                    {
                        checkTotalReleaseCost(referenceIssueNbr, configuration);
                    }

                    authApi.AuthLogout();
                    if (!syncError)
                    {
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Reclass Process [{DocNumber}]";

                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                foreach (DataRowView rowView in dv_filter)
                                {
                                    var rowDocNumber = rowView[0].ToString();
                                    var rowLotNumber = rowView[3].ToString();

                                    using (SqlCommand command = new SqlCommand("Update_WeightAdjustLineDetail_Sync", connection))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
                                        command.Parameters.AddWithValue("@LotNbr", rowLotNumber);
                                        command.Parameters.AddWithValue("@SyncDetail", 1);

                                        command.ExecuteNonQuery();
                                    }
                                }
                            }
                            catch (Exception e_update)
                            {
                                MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        resetEntry();
                        saveDocument();
                    }
                }
            }//end if

            if (tbAcumaticaIssue.Text != "")
            {
                this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}] - Syncing with Acumatica, please wait!";

                loadDetail();

                foreach (DataRow row in dtDetail.Rows)
                {
                    InventoryReceiptDetail receiptDetail = new InventoryReceiptDetail();
                    receiptDetail.InventoryID = row[1].ToString();
                    receiptDetail.Branch = docBranch;
                    receiptDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                    receiptDetail.Subitem = row[2].ToString().Replace(".", "");
                    receiptDetail.Qty = Convert.ToDecimal(row[19]);
                    receiptDetail.Description = row[3].ToString();
                    receiptDetail.WarehouseID = tbWarehouse.Text;
                    receiptDetail.UnitCost = Convert.ToDecimal(row[21]);
                    receiptDetail.UOM = row[20].ToString();
                    receiptDetail.ReasonCode = "RECEIPTADJUST";
                    //receiptDetail.LotSerialNbr = "";

                    receiptDetails.Add(receiptDetail);
                }

                inventoryReceipt.Details = receiptDetails;

                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                try
                {
                    //var inventoryReceiptApi = new InventoryReceiptApi(configuration);
                    //var responseReceipt = inventoryReceiptApi.PutEntity(inventoryReceipt);
                    //ReleaseInventoryReceipt releaseInventoryReceipt = new ReleaseInventoryReceipt((InventoryReceipt)responseReceipt);
                    //inventoryReceiptApi.InvokeAction(releaseInventoryReceipt);

                    //referenceReceiptNbr = responseReceipt.ReferenceNbr.ToString();
                    //tbAcumaticaReceipt.Text = referenceReceiptNbr;


                    var inventoryReceiptApi = new InventoryReceiptApi(configuration);
                    if (referenceReceiptNbr == "")
                    {
                        var responseReceipt = inventoryReceiptApi.PutEntity(inventoryReceipt);

                        referenceReceiptNbr = responseReceipt.ReferenceNbr.ToString();
                        tbAcumaticaReceipt.Text = referenceReceiptNbr;
                        saveDocument();

                        //throw new InvalidOperationException("error");
                        ReleaseInventoryReceipt releaseInventoryReceipt = new ReleaseInventoryReceipt((InventoryReceipt)responseReceipt);
                        inventoryReceiptApi.InvokeAction(releaseInventoryReceipt);

                    }
                    else
                    {
                        var responseReceipt = inventoryReceiptApi.GetList(select: "ReferenceNbr", filter: $"ReferenceNbr eq '{referenceReceiptNbr}'");
                        ReleaseInventoryReceipt releaseInventoryReceipt = new ReleaseInventoryReceipt((InventoryReceipt)responseReceipt[0]);
                        inventoryReceiptApi.InvokeAction(releaseInventoryReceipt);
                    }

                }
                catch (Exception ePut)
                {
                    MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    syncError = true;
                }
                finally
                {
                    authApi.AuthLogout();
                    tbStatus.Text = "SYNCED";
                    saveDocument();

                    if (!syncError)
                    {
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Weight Adjustment Process [{DocNumber}]";
                        tbStatus.Text = "SYNCED";
                        saveDocument();
                        MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        resetEntry();
                        saveDocument();
                    }
                }
            }
        }


        private void btnScaleComm_Click(object sender, EventArgs e)
        {
            if (ScaleCalibration.isActive())
            {
                if (port != null && port.IsOpen)
                {
                    port.Close();
                    tbScale.BackColor = System.Drawing.SystemColors.ActiveBorder;
                    tbScale.Text = "0.00";
                }
                else
                {
                    if (port != null) port.Dispose();
                    startSerial();
                    if (port.IsOpen) tbScale.BackColor = System.Drawing.SystemColors.ActiveCaption;
                }
            }
            else
            {
                MessageBox.Show($"Scale calibration is expired!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnScaleOverride_Click(object sender, EventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                MessageBox.Show($"Please DISABLE Scale communication first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (tbScale.ReadOnly)
                {
                    tbScale.BackColor = System.Drawing.SystemColors.ActiveCaption;
                    tbScale.ReadOnly = false;
                }
                else
                {
                    tbScale.BackColor = System.Drawing.SystemColors.ActiveBorder;
                    tbScale.Text = "0.00";
                    tbScale.ReadOnly = true;
                }
            }
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            tbEntryWReceiveNew.Text = tbScale.Text;
        }

        private void btnRemoveEntry_Click(object sender, EventArgs e)
        {
            removeLot();

            calculateWeight();
        }

        private void btnSaveEntry_Click(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                saveDocument();
            }

            if (DocNumber != "<NEW>")
            {
                saveEntry();
            }
            tbLot.Text = "";
            tbLot.Focus();
        }

        private void cbLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLot.SelectedIndex >= 0)
            {
                loadEntry(cbLot.SelectedItem.ToString());
                loadEntryGroup();

                calculateWeight();
            }
        }

        private void tbEntryWReceiveNew_TextChanged(object sender, EventArgs e)
        {
            calculateWeight();
        }

        private void tbEntryWTareNew_TextChanged(object sender, EventArgs e)
        {
            calculateWeight();
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void checkHold_CheckedChanged(object sender, EventArgs e)
        {
            if (checkHold.Checked)
            {
                tbStatus.Text = "HOLD";
            }
            else
            {
                tbStatus.Text = "OPEN";
            }
        }


        private void btnPrintLot_Click(object sender, EventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                var lotNbr = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();

                QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode($"{lotNbr}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                string QRImage = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);

                GenericLotPrint lotPrint = new GenericLotPrint
                {
                    LotNumber = lotNbr,
                    Source = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[4].FormattedValue.ToString(),
                    StalkPos = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[14].FormattedValue.ToString(),
                    Ferment = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[11].FormattedValue.ToString(),
                    Buyer = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[28].FormattedValue.ToString(),
                    InventoryID = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString(),
                    Process = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[13].FormattedValue.ToString(),
                    Stage = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].FormattedValue.ToString(),
                    Grade = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].FormattedValue.ToString(),
                    Color = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].FormattedValue.ToString(),
                    Weight = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].FormattedValue.ToString(),
                    Length = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[12].FormattedValue.ToString(),
                    Warehouse = tbWarehouse.Text,
                    Date = tbDate.Text,
                    Remark = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[22].FormattedValue.ToString(),
                    Area = "",
                    QRImage = QRImage
                };
                lotPrint.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select bale/lot first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private void btnPrintDoc_Click(object sender, EventArgs e)
        {

            DataSetAddon myDoc = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
                                            *
                                        FROM
	                                        WeightAdjustLineDetail
	                                    WHERE
		                                    WeightAdjustLineDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        WeightAdjustLineDetail.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.WeightAdjustLineDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            WeightAdjustDocPrint weightAdjustDocPrint = new WeightAdjustDocPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "WEIGHT ADJUST",
                DocDate = tbDate.Text,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc
            };
            weightAdjustDocPrint.ShowDialog();
        }

        private void tbLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                LotStockItem = tbLot.Text;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //clear
                    DataTable dt = new DataTable();
                    try
                    {
                        string query = $"SELECT LotNbr, StatusStock  FROM StockItem  WHERE LotNbr = '{tbLot.Text}' AND StatusStock = 1";

                        connection.Open();
                        SqlDataReader reader;
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            reader = command.ExecuteReader();
                        }

                        if (reader.HasRows)
                        {
                            reader.Read();

                            LotStockItem = reader.GetValue(0).ToString();
                            if (Convert.ToInt32(reader.GetValue(1)) == 1)
                            {
                                int index = cbLot.FindStringExact(tbLot.Text);

                                if (index >= 0)
                                {
                                    cbLot.SelectedIndex = index;
                                    if (e.KeyChar == '\r')
                                    {
                                        if (this.ActiveControl != null)
                                        {
                                            this.SelectNextControl(this.ActiveControl, true, true, true, true);
                                        }
                                        e.Handled = true; // Mark the event as handled
                                    }

                                }
                                else
                                {
                                    MessageBox.Show($"Lot number tidak tersedia !");
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show($"Lot number {LotStockItem} tidak tersedia !", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show($"Lot number {LotStockItem} tidak tersedia !", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnToogle_Click(object sender, EventArgs e)
        {
            if (tbLot.Visible)
            {
                tbLot.Visible = false;
                cbLot.Visible = true;
            }
            else
            {
                tbLot.Visible = true;
                cbLot.Visible = false;
            }
        }

        #endregion button


        private string GetBranch(string warehouseID, int wData)
        {
            var warehouseName = "";
            var branch = "";
            var address = "";
            var phone = "Telp : ";
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                DataTable dtWarehouse = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        WarehouseSite
                                        WHERE
	                                        WarehouseID = '{warehouseID}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtWarehouse);

                    warehouseName = dtWarehouse.Rows[0].ItemArray[1].ToString();
                    branch = dtWarehouse.Rows[0].ItemArray[8].ToString();
                    if (dtWarehouse.Rows[0].ItemArray[4].ToString().Length > 0)
                    {
                        address = dtWarehouse.Rows[0].ItemArray[4].ToString();
                    }
                    if (dtWarehouse.Rows[0].ItemArray[5].ToString().Length > 0)
                    {
                        address = address + Environment.NewLine + dtWarehouse.Rows[0].ItemArray[5].ToString();
                    }
                    if (dtWarehouse.Rows[0].ItemArray[6].ToString().Length > 0)
                    {
                        phone = phone + dtWarehouse.Rows[0].ItemArray[6].ToString();
                    }

                    if (dtWarehouse.Rows[0].ItemArray[6].ToString().Length > 0 && dtWarehouse.Rows[0].ItemArray[7].ToString().Length > 0)
                    {
                        phone = phone + " ," + dtWarehouse.Rows[0].ItemArray[7].ToString();
                    }
                    else
                    {
                        phone = phone + dtWarehouse.Rows[0].ItemArray[7].ToString();
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            switch (wData)
            {
                case 1:
                    return warehouseName;
                    break;
                case 2:
                    return branch;
                    break;
                case 3:
                    return address;
                    break;
                case 4:
                    return phone;
                    break;
                default:
                    return warehouseName;
                    break;
            }

        }


        private bool checkLotNbrInSTock(string lotnbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM StockItem WHERE LotNbr = '{lotnbr}' and StatusStock = 1 ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int exist = Convert.ToInt32(reader.GetValue(0).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        if (exist == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return true;
                    //MessageBox.Show(e.ToString());
                }
            }
        }

        private void tbEntryWTareNew_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (this.ActiveControl != null)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                e.Handled = true; // Mark the event as handled
            }
        }

        private void Closing(object sender, FormClosingEventArgs e)
        {
            if (port != null && port.IsOpen)
            {

                try
                {
                    port.Close();
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        private void tbLot_TextChanged(object sender, EventArgs e)
        {

        }


        //end of file
    }
}