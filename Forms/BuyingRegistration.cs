using QRCoder;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class BuyingRegistration : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }

        public string RegNumber { get; set; }

        public DateTime currentDate { get; set; }
        public UserModel Userlog { get; set; }
        private int lastIncrementValue = 0;
        private DataTable dtFarmer;
        private DataTable dtPO;

        private string tempVendorID;
        private string tempVendorName;
        private string tempPO;
        private string tempPOType;
        private string tempContract;

        private bool ScannerdgvPO=false;
        private string orderNbrScanPO = "";
        private string InventoryIDScanPO = "";

        public BuyingRegistration()
        {
            InitializeComponent();
        }

        private void BuyingRegistration_Load(object sender, EventArgs e)
        {
            if (RegNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadRegistration();
            }
        }

        private void loadRegistration()
        {
            tbRegDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbRegDoc.Text = RegNumber;
            tbQueue.Text = RegNumber.Split('-')[1];

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Registration [{RegNumber}]";

            loadComboFarmer();

            var tempPO1 = "";

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from BuyingRegistration where RegistrationNumber = '{RegNumber}' and RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tempVendorID = reader.GetValue(1).ToString();
                        tempPO1 = reader.GetValue(2).ToString();
                        tempPOType = reader.GetValue(6).ToString();
                        tempContract = reader.GetValue(7).ToString();

                        tbFarmer.Text = tempVendorID;
                        cbFarmer.SelectedIndex = cbFarmer.FindStringExact(tempVendorID);

                        tbPO.Text = tempPO1;
                        tbEstWeight.Text = Convert.ToDecimal(reader.GetValue(8).ToString()).ToString("N2");
                        tbEstLot.Text = reader.GetValue(9).ToString();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            if (tempPO1 != "")
            {
                for (int i = 0; i < dgvPO.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvPO.Rows[i];
                    if (row.Cells[1].FormattedValue.ToString() == tempPO1)
                    {
                        dgvPO.CurrentCell = dgvPO.Rows[i].Cells[1];
                    }
                }
            }
        }

        private void loadComboFarmer()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbFarmer.SelectedIndex = -1;

                dtFarmer = new DataTable();
                try
                {
                    string query = "select * from VendorData where Status = 'Active'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtFarmer);
                    string[] arrray = dtFarmer.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbFarmer);
                    cbFarmer.Items.Clear();
                    cbFarmer.Items.AddRange(arrray);
                    //cbFarmer.Items.AddRange(new object[] { "John", "Tina", "Doctor", "Alaska" });

                    //cbFarmer.DataSource = dtFarmer;
                    //cbFarmer.DisplayMember = "VendorID";
                    //cbFarmer.ValueMember = "VendorID";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadQueue()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                tbQueue.Text = "";

                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from NumberingSetting where NumberingID = 'REG'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        DateTime dbDate = Convert.ToDateTime(reader.GetValue(2).ToString());
                        int dbIncrement = Convert.ToInt32(reader.GetValue(1).ToString());
                        if (currentDate.Date == dbDate.Date)
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
                    MessageBox.Show(e.ToString());
                }
            }
            var currentQueue = lastIncrementValue + 1;
            tbQueue.Text = currentQueue.ToString().PadLeft(4, '0');

        }

        private void loadQueueByDate()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                tbQueue.Text = "";

                DataTable dt = new DataTable();
                try
                {
                    string query = $@"SELECT TOP 1
	                                    *
                                    FROM
	                                    BuyingRegistration
                                    WHERE
	                                    RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'
                                    Order BY RegistrationNumber DESC";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        int dbIncrement = Convert.ToInt32(reader.GetValue(0).ToString().Split('-')[1]);
                        lastIncrementValue = dbIncrement;
                    }
                    else
                    {
                        lastIncrementValue = 0;
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                    MessageBox.Show($"Cannot connect database server for queue checking!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Dispose();
                }
            }
            var currentQueue = lastIncrementValue + 1;
            tbQueue.Text = currentQueue.ToString().PadLeft(4, '0');
        }

        private void loadPO()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtPO = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT
	                                        VendorPO.VendorID,
	                                        VendorPO.OrderNbr,
	                                        VendorPO.OrderType,
	                                        VendorPO.NoKontrak,
	                                        VendorContract.FarmerID,
	                                        VendorContract.NoKTP,
	                                        VendorContract.Area,
	                                        VendorContract.SubArea,
	                                        VendorContract.Seri,
	                                        VendorContract.InventoryID,
	                                        VendorPODetail.OrderQty,
	                                        VendorPODetail.QtyOnReceipts,
	                                        (VendorPODetail.OrderQty - VendorPODetail.QtyOnReceipts) as UnfulfilledQty
                                        FROM
	                                        dbo.VendorPO
	                                        INNER JOIN
	                                        dbo.VendorContract
	                                        ON
		                                        VendorPO.NoKontrak = VendorContract.NoKontrak
	                                        INNER JOIN
	                                        dbo.VendorPODetail
	                                        ON
		                                        VendorPO.OrderNbr = VendorPODetail.OrderNbr
                                        WHERE
                                            VendorPO.VendorID = '{tempVendorID}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtPO);
                    dgvPO.DataSource = dtPO;

                    //Header Text
                    dgvPO.Columns[0].HeaderText = "Vendor ID";
                    dgvPO.Columns[0].Visible = false;
                    dgvPO.Columns[1].HeaderText = "order Number";
                    dgvPO.Columns[2].HeaderText = "order Type";
                    dgvPO.Columns[3].HeaderText = "Contract Number";
                    dgvPO.Columns[4].HeaderText = "Farmer ID";
                    dgvPO.Columns[5].HeaderText = "ID Number";
                    dgvPO.Columns[6].HeaderText = "Area";
                    dgvPO.Columns[7].HeaderText = "Sub-Area";
                    dgvPO.Columns[8].HeaderText = "Series";
                    dgvPO.Columns[9].HeaderText = "Inventory ID";
                    dgvPO.Columns[10].HeaderText = "Order Qty";
                    dgvPO.Columns[10].DefaultCellStyle.Format = "N2";
                    dgvPO.Columns[11].HeaderText = "On Receipt Qty";
                    dgvPO.Columns[11].DefaultCellStyle.Format = "N2";
                    dgvPO.Columns[12].HeaderText = "Unfulfilled Qty";
                    dgvPO.Columns[12].DefaultCellStyle.Format = "N2";

                    dgvPO.ClearSelection();
                    tempPO = "";
                    tbPO.Text = "";


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void tbQueue_TextChanged(object sender, EventArgs e)
        {
            tbRegDoc.Text = $"REG-{tbQueue.Text}";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RegNumber = "<NEW>";
            resetScreen();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool createregistrasi = true;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
               
                try
                {
                    string query = $@"SELECT
	                                    BuyingUse
                                    FROM
	                                    BuyingRegistration
                                    WHERE
	                                    RegistrationDate = '{tbRegDate.Text}'
                                        AND RegistrationNumber = '{tbRegDoc.Text}'
                                    Order BY RegistrationNumber DESC";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                         reader.Read();

                        Console.WriteLine(reader.GetValue(0).ToString());
                        if (reader.GetValue(0).ToString().Trim().Length > 5)
                        {
                            MessageBox.Show($"No Registrasi telah terpakai oleh buying {reader.GetValue(0).ToString()} !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            createregistrasi = false;
                        }
                    }
                }
                catch (Exception f)
                {
                    //MessageBox.Show(e.ToString());
                    MessageBox.Show($"Masalah jaringan, koneksi ke database putus", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Dispose();
                }
            }

            //enable this line to enable checking for multi terminal
            loadQueueByDate();

            if (createregistrasi) {
                var orderNbr = "";
                var InventoryID = "";
                if (dgvPO.SelectedRows.Count > 0)
                {
                    orderNbr = dgvPO.Rows[dgvPO.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                    InventoryID = dgvPO.Rows[dgvPO.SelectedRows[0].Index].Cells[9].FormattedValue.ToString();
                }
                else if(ScannerdgvPO) {
                    orderNbr = orderNbrScanPO;
                    InventoryID = InventoryIDScanPO;
                }

                if (cbFarmer.SelectedItem != null)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_BuyingRegistration", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@RegistrationNumber", tbRegDoc.Text);
                                command.Parameters.AddWithValue("@VendorID", tempVendorID);
                                command.Parameters.AddWithValue("@OrderNbr", orderNbr);
                                command.Parameters.AddWithValue("@InventoryID", InventoryID);
                                command.Parameters.AddWithValue("@RegistrationDate", tbRegDate.Text);
                                command.Parameters.AddWithValue("@WarehouseID", Warehouse.WarehouseID);
                                command.Parameters.AddWithValue("@OrderType", tempPOType);
                                command.Parameters.AddWithValue("@NoKontrak", tempContract);
                                command.Parameters.AddWithValue("@EstWeight", Convert.ToDecimal(tbEstWeight.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@EstLot", Convert.ToInt32(tbEstLot.Text));
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
                            lastIncrementValue = Convert.ToInt32(tbQueue.Text);
                            RegNumber = tbRegDoc.Text;
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Registration [{RegNumber}]";
                            MessageBox.Show("Registration complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

            }
         

            //loadPO();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (RegNumber != "<NEW>")
            {
                QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode($"{RegNumber}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                string QRImage = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);

                BuyingRegistrationPrint registrationPrint = new BuyingRegistrationPrint
                {
                    Company = Warehouse.Company,
                    Address = GetBranch(Warehouse.WarehouseID, 3),
                    Phone = GetBranch(Warehouse.WarehouseID, 4),
                    Warehouse = Warehouse.Descr,
                    RegDate = tbRegDate.Text,
                    RegNumber = RegNumber,
                    VendorID = tempVendorID,
                    VendorName = tempVendorName,
                    Contract = $"{tempPO}/{tempContract}",
                    QRImage = QRImage
                };
                registrationPrint.ShowDialog();
            }
            else
            {
                MessageBox.Show("Create a registration first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void resetScreen()
        {
            //currentDate = DateTime.Now;
            tbRegDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            RegNumber = "<NEW>";

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Registration [{RegNumber}]";

            loadComboFarmer();
            //loadQueue();
            loadQueueByDate();

            tbVendorClass.Text = "";
            tbVendorDetails.Text = "";

            tempVendorID = "";
            tempVendorName = "";
            tempPO = "";
            tempPOType = "";
            tempContract = "";

            tbFarmer.Text = tempVendorID;

            loadPO();

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

        private void dgvPO_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPO.SelectedRows.Count > 0 && dgvPO.SelectedRows[0].Index >= 0)
            {
                tempPO = dgvPO.Rows[dgvPO.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                tbPO.Text = tempPO;

                tempPOType = dgvPO.Rows[dgvPO.SelectedRows[0].Index].Cells[2].FormattedValue.ToString();
                tempContract = dgvPO.Rows[dgvPO.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
            }
        }



        private void cbFarmer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFarmer.SelectedIndex >= 0)
            {
                tempVendorID = cbFarmer.SelectedItem.ToString().Split('|')[0].Trim();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        string query = $"select * from VendorData where VendorID = '{tempVendorID}'";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            string details = "";
                            details = $@"{reader.GetValue(1)}{Environment.NewLine}{reader.GetValue(6) ?? ""}, {reader.GetValue(7) ?? ""}{Environment.NewLine}{reader.GetValue(8) ?? ""}, {reader.GetValue(9) ?? ""}";

                            tempVendorName = reader.GetValue(1).ToString();
                            tbVendorDetails.Text = details ?? "";
                            tbVendorClass.Text = reader.GetValue(12).ToString() ?? "";
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                }
            }

            loadPO();
        }

        private void tbFarmer_KeyPress(object sender, KeyPressEventArgs e)
        {

            if(e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                int index = cbFarmer.FindStringExact(tbFarmer.Text);
                if (index >= 0)
                {
                    cbFarmer.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Farmer Code not available !");
                }
            }

        }

        private void btnToogle_Click(object sender, EventArgs e)
        {
            if (tbFarmer.Visible)
            {
                tbFarmer.Visible = false;
                cbFarmer.Visible = true;
            }
            else
            {
                tbFarmer.Visible = true;
                cbFarmer.Visible = false;
            }
        }

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

        private void tbPO_TextChanged(object sender, EventArgs e)
        {
            if (tbPO.Text != "")
            {
                //disable change reg
                cbFarmer.Enabled = false;
                tbFarmer.Enabled = false;

            }
            else
            {
                //enable change reg
                cbFarmer.Enabled = true;
                tbFarmer.Enabled = true;

            }
        }



        private void Find_PO(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        string query = $@"SELECT
                                         VendorPO.VendorID,
                                         VendorPO.OrderNbr,
                                         VendorPO.OrderType,
                                         VendorPO.NoKontrak,
                                         VendorPODetail.InventoryID
                                        FROM
                                         dbo.VendorPO
                                         INNER JOIN
                                         dbo.VendorContract
                                         ON
                                          VendorPO.NoKontrak = VendorContract.NoKontrak
                                         INNER JOIN
                                         dbo.VendorPODetail
                                         ON
                                          VendorPO.OrderNbr = VendorPODetail.OrderNbr
                                        WHERE
                                            VendorPO.OrderNbr = '{FarmPO.Text}'";

                     

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            tempVendorID = reader.GetValue(0).ToString();
                            tempPO = reader.GetValue(1).ToString();
                            tbPO.Text = tempPO;
                            tempPOType = reader.GetValue(2).ToString();
                            tempContract = reader.GetValue(3).ToString();
                            ScannerdgvPO = true;
                            orderNbrScanPO = tempPO;
                            InventoryIDScanPO = reader.GetValue(4).ToString();
                        }
                    }
                    catch (Exception ea)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

            int index = cbFarmer.FindStringExact(tempVendorID);
          
            if (index >= 0)
            {
                cbFarmer.SelectedIndex = index;
            }


            tbPO.Text = FarmPO.Text;
            tbFarmer.Text = tempVendorID;

         

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from VendorData where VendorID = '{tempVendorID}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        string details = "";
                        details = $@"{reader.GetValue(1)}{Environment.NewLine}{reader.GetValue(6) ?? ""}, {reader.GetValue(7) ?? ""}{Environment.NewLine}{reader.GetValue(8) ?? ""}, {reader.GetValue(9) ?? ""}";

                        tempVendorName = reader.GetValue(1).ToString();
                        tbVendorDetails.Text = details ?? "";
                        tbVendorClass.Text = reader.GetValue(12).ToString() ?? "";
                    }
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            //loadPO();

        }


        //end of file

    }
}