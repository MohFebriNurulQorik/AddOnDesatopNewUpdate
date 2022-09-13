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
using System.Linq;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
using ExcelDataReader;

namespace ScaleAddon.Forms
{
    public partial class BuyingProcessV2 : Form
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
        private int lastLotIncrementValue = 0;
        private DataTable dtRegistration;
        private DataTable dtInventory;
        private DataTable dtEntryStage;
        private DataTable dtEntrySource;
        private DataTable dtEntryForm;
        private DataTable dtEntryCropYear;
        private DataTable dtEntryGrade;
        private DataTable dtEntryArea;
        private DataTable dtEntryStalk;
        private DataTable dtDetail;
        private DataTable dtQC;
        
        private DataTableCollection dtImport;
        private DataTable dtDetailImport;

        private string tempRegNumber;
        private string tempVendorID;
        private string tempVendorName;
        private string tempPO;
        private string tempWarehouse;
        private string tempPOType;
        private decimal tempPOOpenQty;
        private bool allowOverPO = false;
        private string tempKontrak;
        private string tempInventoryID;
        private string tempQCDocNumber;

        private string tempSource;
        private string tempStage;
        private string tempArea;
        private string tempStalkPosition;
        private string tempGrade;
        private string tempGradeDraft;
        private string tempForm;
        private string tempCropYear;

        private bool tempQCExist = false;
        private bool tempQCLock = false;

        private string tempUnitPrice;

        private SerialPort port;

        public BuyingProcessV2()
        {
            InitializeComponent();
        }

        private void BuyingProcess_Load(object sender, EventArgs e)
        {
            if (Userlog.UserRoles.Contains("BY-IPORT")|| Userlog.UserRoles.Contains("SUPERVISOR"))
            {
                label43.Visible = true;
                label44.Visible = true;
                textFilename.Visible = true;
                cbosheet.Visible = true;
                ImportButton.Visible = true;
                SaveImport.Visible = true;
            }
            else
            {
                label43.Visible = false;
                label44.Visible = false;
                textFilename.Visible = false;
                cbosheet.Visible = false;
                ImportButton.Visible = false;
                SaveImport.Visible = false;
            }

            tempCropYear = $"{FiscalInfo.CurrentFiscalYear.ToString().Substring(2, 2)}";

            //startSerial();
            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadBuyingProcess();
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
            chkLblPrint.Checked = true;
            optLblThermal.Checked = true;
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

                try
                {
                    port.Close();
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        #endregion serialPort

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DocNumber = "<NEW>";
            resetScreen();
        }

        private void loadBuyingProcess()
        {
            
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");

            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}]";

            loadComboReg();
            loadComboInv();

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from BuyingLine where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(9).ToString();
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(10).ToString();
                        tempRegNumber = reader.GetValue(5).ToString();
                        cbRegistration.SelectedIndex = cbRegistration.Items.IndexOf(tempRegNumber);
                        tbRegistration.Text = tempRegNumber;
                        tbBuyerName.Text = reader.GetValue(12).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(9).ToString() == "HOLD" ? 1 : 0);

                        tempInventoryID = reader.GetValue(7).ToString();
                        cbInventory.SelectedIndex = cbInventory.FindString(tempInventoryID);
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
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";
            checkHold.Checked = true;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}]";

            loadComboReg();
            loadComboInv();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbVendorID.Text = "";
            tbVendorClass.Text = "";
            tbVendorDetails.Text = "";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaRefNbr.Text = "";
            tbBuyerName.Text = "";

            tempVendorID = "";
            tempInventoryID = "";
            tempPO = "";
            tempPOType = "";
            tempKontrak = "";
            tempInventoryID = "";


            btnGenerateLotQC.Enabled = false;

            resetEntry();
            loadDetail();
        }

        private void loadComboReg()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbRegistration.SelectedIndex = -1;

                dtRegistration = new DataTable();
                try
                {
                    //string query = $@"SELECT *
                    //                    FROM
                    //                        dbo.BuyingRegistration
                    //                    WHERE
                    //                        RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'
                    //                    AND
                    //                        WarehouseID = '{Warehouse.WarehouseID}'";
                    string query = $@"SELECT
	                                        dbo.BuyingRegistration.*, 
	                                        dbo.BuyingLine.DocumentID, 
	                                        dbo.BuyingLine.DocumentDate
                                        FROM
	                                        dbo.BuyingRegistration
	                                        LEFT JOIN
	                                        dbo.BuyingLine
	                                        ON 
		                                        dbo.BuyingRegistration.RegistrationNumber = dbo.BuyingLine.RegistrationNumber AND
		                                        dbo.BuyingRegistration.RegistrationDate = dbo.BuyingLine.DocumentDate
                                        WHERE
                                            dbo.BuyingRegistration.RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'
                                        AND
                                            dbo.BuyingRegistration.WarehouseID = '{Warehouse.WarehouseID}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtRegistration);
                    string[] arrray;
                    if (DocNumber.Contains("NEW"))
                    {
                        arrray = dtRegistration.Rows.OfType<DataRow>().Where(j => j[13].ToString().Length == 0).Select(k => k[0].ToString()).ToArray();
                    }
                    else
                    {
                        arrray = dtRegistration.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                    }


                    //new AutoCompleteBehavior(cbRegistration);
                    cbRegistration.Items.Clear();
                    cbRegistration.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboInv()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbInventory.SelectedIndex = -1;

                dtInventory = new DataTable();
                try
                {
                    string query = "select * from InventoryItem";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtInventory);
                    string[] arrray = dtInventory.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[1].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbInventory);
                    cbInventory.Items.Clear();
                    cbInventory.Items.AddRange(arrray);
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
                    string query = "select * from NumberingSetting where NumberingID = 'BY/IN'";
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
                tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-BY/IN-{currentIncrement.ToString().PadLeft(4, '0')}";

                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-BY/IN-{currentIncrement.ToString().PadLeft(4, '0')}";

                if (!checkDocNbrExist(docNbr))
                {
                    tbDocNumber.Text = docNbr;
                }
                else
                {
                    tbDocNumber.Text = "<NEW>";
                    MessageBox.Show($"Document number {docNbr} already exist, \n or unable to check existing document number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string query = $"IF EXISTS ( SELECT * FROM BuyingLine WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
            resetEntry();
        }

        private void saveDocument()
        {
            bool RegistrasiUse = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                
                try
                {
                    

                    string query = $"SELECT BuyingUse FROM  BuyingRegistration where RegistrationNumber='{cbRegistration.SelectedItem.ToString()}' AND VendorID='{tbVendorID.Text}' AND RegistrationDate = '{tbBuyingDate.Text}'";

                    connection.Open();
                    SqlDataReader reader;
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        reader = command.ExecuteReader();
                    }

                    if (reader.HasRows)
                    {
                        reader.Read();
                        try
                        {
                            if (reader.GetValue(0).ToString() == tbDocNumber.Text)
                            {
                                RegistrasiUse = true;
                            }
                            else if (reader.GetValue(0).ToString() == "" || reader.GetValue(0) == null)
                            {
                                RegistrasiUse = true;
                            }
                            else 
                            {
                                RegistrasiUse = false;
                            }
                        }
                        catch (Exception q) { }

                    }
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString());
                }
            }


            if (cbRegistration.SelectedItem != null && cbInventory.SelectedItem != null)
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

                if (!incrementError && RegistrasiUse)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_BuyingLine_v2_C", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                                command.Parameters.AddWithValue("@DocumentDate", tbBuyingDate.Text);
                                command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                                command.Parameters.AddWithValue("@VendorID", tbVendorID.Text);
                                command.Parameters.AddWithValue("@VendorDetails", tbVendorDetails.Text);
                                command.Parameters.AddWithValue("@RegistrationNumber", cbRegistration.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@OrderNbr", tempPO);
                                command.Parameters.AddWithValue("@InventoryID", cbInventory.SelectedItem.ToString().Split('|')[0].Trim());
                                command.Parameters.AddWithValue("@VendorClass", tbVendorClass.Text);
                                command.Parameters.AddWithValue("@Status", tbStatus.Text);
                                command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                                command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text);
                                command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                                command.Parameters.AddWithValue("@OperationType", OperationType);

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            insertError = true;
                            tbDocNumber.Text = "<NEW>";
                            loadComboReg();
                        }


                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            if (!insertError && this.Text.Contains("<NEW>"))
                            {
                                this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}]";

                                using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@NumberingID", "BY/IN");
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
                    MessageBox.Show($"Nomer registrasi telah terpakai di Dokument lain \n Silahkan buat nomer registrasi baru", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show($"Please choose registration number and/or inventory !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void cbRegistration_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (cbRegistration.SelectedIndex >= 0)
            {
                
                tempRegNumber = cbRegistration.SelectedItem.ToString();

                var results = from myRow in dtRegistration.AsEnumerable()
                              where myRow.Field<string>("RegistrationNumber") == tempRegNumber
                              select myRow;
               
                foreach (var result in results)
                {
                    // do something with it
                    tempVendorID = result.ItemArray[1].ToString();
                    tempPO = result.ItemArray[2].ToString();
                    tempInventoryID = result.ItemArray[3].ToString();
                    tempWarehouse = result.ItemArray[5].ToString();
                    tempPOType = result.ItemArray[6].ToString();
                    tempKontrak = result.ItemArray[7].ToString();
                }
                if (tempPO != "")
                {
                    bool buyingget = false;
                    decimal OrderQty = 0;
                    decimal QtyOnReceipts = 0;
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {

                        //clear
                        DataTable dt = new DataTable();
                        try
                        {
                            string query = $"select * from vendorPODetail where OrderNbr = '{tempPO}' ";
                           
                            connection.Open();
                            SqlDataReader reader;
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                reader = command.ExecuteReader();
                            }

                            if (reader.HasRows)
                            {
                                reader.Read();

                                OrderQty = Convert.ToDecimal(reader.GetValue(5).ToString());
                                QtyOnReceipts = Convert.ToDecimal(reader.GetValue(6).ToString());


                            }

                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show(e2.ToString());
                        }
                    }

                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        //clear
                        DataTable dt = new DataTable();
                        try
                        {
                            string query = $"SELECT SUM(BuyingLineDetail.WeightNetto) FROM  BuyingLine JOIN BuyingLineDetail ON BuyingLine.DocumentID = BuyingLineDetail.DocumentID WHERE BuyingLine.OrderNbr  = '{tempPO}'";

                            connection.Open();
                            SqlDataReader reader;
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                reader = command.ExecuteReader();
                            }

                            if (reader.HasRows)
                            {
                                reader.Read();
                                try { 
                                QtyOnReceipts = Convert.ToDecimal(reader.GetValue(0).ToString());
                                }
                                catch(Exception q){ }

                            }
                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show(e2.ToString());
                        }
                    }

                    tempPOOpenQty = OrderQty - QtyOnReceipts;

                }
                else
                {
                    tempPOOpenQty = 0M;
                }

                if (tempInventoryID.Length > 0)
                {
                    cbInventory.SelectedIndex = cbInventory.FindString(tempInventoryID);
                    cbInventory.Enabled = false;
                    resetEntry();
                }
                else
                {
                    cbInventory.SelectedIndex = -1;
                    cbInventory.Enabled = true;
                }

                tbVendorID.Text = tempVendorID;
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

                //reset QC status
                tempQCExist = false;
                tempQCDocNumber = "";


                //check if QC available
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //clear
                    DataTable dt = new DataTable();
                    try
                    {
                        string query = $@"SELECT
                                                BuyingRegistration.RegistrationNumber, 
	                                            BuyingRegistration.RegistrationDate, 
	                                            BuyingRegistration.WarehouseID, 
	                                            BuyingQC.TotalLot, 
	                                            BuyingQC.SamplingRange,
                                                BuyingQC.DocumentID
                                            FROM
                                                dbo.BuyingRegistration
                                                INNER JOIN
                                                dbo.BuyingQC
                                                ON
                                                    BuyingRegistration.RegistrationNumber = BuyingQC.RegistrationNumber AND
                                                    BuyingRegistration.RegistrationDate = BuyingQC.DocumentDate
                                            WHERE
                                                BuyingRegistration.RegistrationNumber = '{tempRegNumber}' AND
                                                BuyingRegistration.RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";


                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            tempQCExist = true;
                            tempQCDocNumber = reader.GetValue(5).ToString();

                        }
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                }

                if (tempQCExist)
                {
                    tbQCA.BackColor = System.Drawing.Color.LimeGreen;
                    tbQCA.Text = "QC AVAILABLE";
                    btnGenerateLotQC.Enabled = true;
                    btnAddEntry.Enabled = false;

                }
                else
                {
                    tbQCA.BackColor = System.Drawing.Color.LightCoral;
                    tbQCA.Text = "QC NOT AVAILABLE";
                    btnGenerateLotQC.Enabled = false;

                }

                setSource();
                setCropYear();
            }
        }

        private void cbInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInventory.SelectedIndex >= 0)
            {
                tempInventoryID = cbInventory.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                tempInventoryID = "";
            }

            resetEntry();
        }

        #region Entry

        private void resetEntry()
        {
            //loadDetail();

            groupEntry.Text = "Lot Entry [<NEW>]";
            groupEntry.BackgroundImage = null;

            //tempSource = "";
            //tempCropYear = DateTime.Now.Year.ToString().Substring(2);
            //sudah ada method sendiri

            if (DocNumber == "<NEW>")
            {
                tempStage = "";
                tempArea = "";
                tempStalkPosition = "";
                tempGrade = "";
                tempForm = "";
            }

            tempUnitPrice = "0";

            tbEntryInv.Text = tempInventoryID;


            loadComboSource();
            loadComboStage();
            loadComboForm();
            loadComboCropYear();
            loadComboArea();
            loadComboStalk();

            //loadComboGrade(); 

            //move to tbEntryInv
            setLotNbr();
            //setSource();
            setCropYear();

            //tbEntryLot.Text = "<NEW>";
            tbEntryWeightRope.Text = "";
            tbEntryWeightShipping.Text = "";
            tbEntryWeightReceive.Text = "";
            //tbEntryWeightTare.Text = "";
            tbEntryWeightNetto.Text = "";
            tbEntryUnitPrice.Text = "0";
            tbEntryGrossValue.Text = "0";
            tbEntryRemark.Text = "";
            tbEntryNTRM.Text = "0";
            tbEntryNettValue.Text = "0";
            checkNTRM.Checked = false;
            checkResidue.Checked = false;
            checkMC.Checked = false;
            //tbEntryGradeDraft.Text = "";

            tbPO.Text = tempPO;
            tbPOOpenQty.Text = tempPOOpenQty.ToString("N2");

            cbEntrySource.SelectedIndex = cbEntrySource.FindString(tempSource);
            if (dtDetail != null && dtDetail.Rows.Count > 0)
            {
                cbEntryStage.SelectedIndex = cbEntryStage.FindString(tempStage);
            }
            else
            {
                cbEntryStage.SelectedIndex = cbEntryStage.FindString("GR");
            }

            //Console.WriteLine(tempCropYear);
            //Console.WriteLine(tempForm);

            cbEntryForm.SelectedIndex = cbEntryForm.FindString(tempForm);
            cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(tempCropYear);
            cbEntryGrade.SelectedIndex = cbEntryGrade.FindStringExact(tempGrade);
            cbEntryArea.SelectedIndex = cbEntryArea.FindString(tempArea);
            cbEntryStalk.SelectedIndex = cbEntryStalk.FindString(tempStalkPosition);


        //    Console.WriteLine(tempPOOpenQty);
        //    Console.WriteLine(tempPO);
            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = true;
                    btnSave.Enabled = true;
                    btnAddEntry.Enabled = false;
                    btnSaveLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = true;
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = false;
                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Window;
                        tbEntryUnitPrice.ReadOnly = true;
                    }
                    else
                    {
                        unsyncing.Visible = false;
                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Info;
                        tbEntryUnitPrice.ReadOnly = true;
                    }
                    break;

                case "SYNCED":
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintDoc.Enabled = true;
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = true;
                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Window;
                        tbEntryUnitPrice.ReadOnly = true;
                    }
                    else
                    {
                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Info;
                        tbEntryUnitPrice.ReadOnly = true;
                        unsyncing.Visible = false;
                    }
                    break;
                     
                default:
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = true;
                    btnAddEntry.Enabled = true;
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = false;

                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Window;
                        tbEntryUnitPrice.ReadOnly = false;
                    }
                    else
                    {
                        unsyncing.Visible = false;
                        tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Info;
                        tbEntryUnitPrice.ReadOnly = true;
                    }
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; btnAddEntry.Enabled = true; } else { 
                        btnSaveLot.Enabled = false; btnAddEntry.Enabled =false;
                        if (tempPOOpenQty <= 0 && tempPO != "")
                        {
                            MessageBox.Show("No registrasi ini memiliki PO yang telah melebihi order", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnSave.Enabled = false;
                            btnAddEntry.Enabled = false;
                        }
                        else
                        {

                            btnSave.Enabled = true;
                            btnAddEntry.Enabled = false;
                        }
                    }
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = false;
                    break;
            }

            tempQCLock = false;
            checkReject.Enabled = true;

            btnPrintLot.Enabled = false;
        }

        private void setSource()
        {
            //tempSource = "";
            switch (tbVendorClass.Text)
            {
                case "FARMERCORP":
                    cbEntrySource.SelectedIndex = cbEntrySource.FindString("A");
                    break;

                case "FARMERIPS":
                    cbEntrySource.SelectedIndex = cbEntrySource.FindString("C");
                    break;

                case "FARMERUMUM":
                    cbEntrySource.SelectedIndex = cbEntrySource.FindString("D");
                    break;

                case "FARMEROM":
                    cbEntrySource.SelectedIndex = cbEntrySource.FindString("F");
                    break;

                default:
                    cbEntrySource.SelectedIndex = -1;
                    break;
            }
        }

        private void setCropYear()
        {
            //tempCropYear = "";
            if (tempKontrak != "" && tempKontrak != null)
            {
                tempCropYear = tempKontrak.Substring(0, 2);
                cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(tempCropYear);
            }
        }

        private void loadComboSource()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntrySource.SelectedIndex = -1;

                dtEntrySource = new DataTable();
                try
                {
                    string query = "select * from ItemAttribute where CodeType = 'SOURCE' and Active=1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntrySource);
                    string[] arrray = dtEntrySource.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    cbEntrySource.Items.Clear();
                    cbEntrySource.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboStage()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryStage.SelectedIndex = -1;

                dtEntryStage = new DataTable();
                try
                {
                    string query = "select * from SegmentValue where SegmentID = 1 and Active=1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryStage);
                    string[] arrray = dtEntryStage.Rows.OfType<DataRow>().Select(k => k[3].ToString() + " | " + k[4].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryStage);
                    cbEntryStage.Items.Clear();
                    cbEntryStage.Items.AddRange(arrray);

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboForm()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryForm.SelectedIndex = -1;

                dtEntryForm = new DataTable();
                try
                {
                    string query = "select * from SegmentValue where SegmentID = 2 and Active=1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryForm);
                    string[] arrray = dtEntryForm.Rows.OfType<DataRow>().Select(k => k[3].ToString() + " | " + k[4].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryForm);
                    cbEntryForm.Items.Clear();
                    cbEntryForm.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboCropYear()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryCropYear.SelectedIndex = -1;

                dtEntryCropYear = new DataTable();
                try
                {
                    string query = "select * from SegmentValue where SegmentID = 3 and Active=1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryCropYear);
                    string[] arrray = dtEntryCropYear.Rows.OfType<DataRow>().Select(k => k[3].ToString() + " | " + k[4].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryCropYear);
                    cbEntryCropYear.Items.Clear();
                    cbEntryCropYear.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboGrade()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryGrade.SelectedIndex = -1;

                dtEntryGrade = new DataTable();
                try
                {
                    string query = $"select * from TobaccoGrade where InventoryID = '{tbEntryInv.Text}' and ProcessID = 'BY' and WarehouseID = '{tbWarehouse.Text}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryGrade);
                    string[] arrray = dtEntryGrade.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryGrade);
                    cbEntryGrade.Items.Clear();
                    cbEntryGrade.Items.Add("-NOT GRADED-");
                    cbEntryGrade.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboArea()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryArea.SelectedIndex = -1;

                dtEntryArea = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'AREA' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryArea);
                    string[] arrray = dtEntryArea.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryArea);
                    cbEntryArea.Items.Clear();
                    cbEntryArea.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboStalk()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryStalk.SelectedIndex = -1;

                dtEntryStalk = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'STALK POSITION' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryStalk);
                    string[] arrray = dtEntryStalk.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryStalk);
                    cbEntryStalk.Items.Clear();
                    cbEntryStalk.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void setLotNbr()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from NumberingSetting where NumberingID = 'BY/LOT'";
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
                            lastLotIncrementValue = dbIncrement;
                        }
                        else
                        {
                            lastLotIncrementValue = 0;
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    //lastLotIncrementValue = -1;
                }
            }
            var currentLotIncrement = lastLotIncrementValue + 1;

            string lotnbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempInventoryID}{tempSource ?? "X"}BY{tempForm ?? "XX"}{currentLotIncrement.ToString().PadLeft(5, '0')}";
            tbEntryLot.Text = lotnbr.Trim();

        }



        private void loadUnitPrice()
        {
           
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                //clear
                tempUnitPrice = "";

                DataTable dt = new DataTable();
                try
                {
                    string query = $@"select * from TobaccoPrice
                                        where InventoryID = '{tempInventoryID ?? ""}'
                                           and Source = '{tempSource ?? ""}'
                                           and Area = '{tempArea ?? ""}'
                                           and Grade = '{tempGrade ?? ""}'
                                           and Form = '{tempForm ?? ""}'
                                           and CropYear = '{tempCropYear ?? ""}'
                                         order by EffectiveDate Asc";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    //Console.WriteLine(query);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                          
                            DateTime effectiveDate = DateTime.Parse(reader.GetValue(7).ToString());

                            if (currentDate >= effectiveDate)
                            {
                                Console.WriteLine(tempUnitPrice);
                                tempUnitPrice = Math.Round(Convert.ToDouble(reader.GetValue(6)), 2).ToString("N2");
                            }
                        }

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            tbEntryUnitPrice.Text = tempUnitPrice;
        }

        private void setValue()
        {
            var weightNetto = "0";
            var unitPrice = "0";

            if (tbEntryWeightNetto.Text != "")
            {
                weightNetto = tbEntryWeightNetto.Text.Replace(",", "");
            }
            if (tbEntryUnitPrice.Text != "")
            {
                unitPrice = tbEntryUnitPrice.Text.Replace(",", "");
            }

            tbEntryGrossValue.Text = Math.Round(Convert.ToDouble(weightNetto) * Convert.ToDouble(unitPrice), 2).ToString("N2");

            if (checkNTRM.Checked && tbEntryNTRM.Text != "")
            {
                tbEntryNettValue.Text = Math.Round(Convert.ToDouble(tbEntryGrossValue.Text) - Convert.ToDouble(tbEntryNTRM.Text), 2).ToString("N2");
            }
            else
            {
                tbEntryNettValue.Text = tbEntryGrossValue.Text;
            }

            if (Convert.ToDouble(tbEntryNettValue.Text) > 0)
            {
                tbEntryNettValue.BackColor = System.Drawing.SystemColors.Info;
            }
            else
            {
                if (checkReject.Checked)
                {
                    tbEntryNettValue.BackColor = System.Drawing.SystemColors.Info;
                }
                else
                {
                    tbEntryNettValue.BackColor = System.Drawing.Color.PaleVioletRed;
                }
            }
        }

        private void tbEntryInv_TextChanged(object sender, EventArgs e)
        {
            loadComboGrade();
            setLotNbr();
            loadUnitPrice();
        }

        private void cbEntrySource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntrySource.SelectedItem != null) tempSource = cbEntrySource.SelectedItem.ToString().Split('|')[0].Trim();
            setLotNbr();
            loadUnitPrice();

            //if (tempSource == "B" || tempSource == "F" || tempSource == "A")
            //{
            //    tbEntryUnitPrice.ReadOnly = false;
            //    tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Window;
            //}
            //else
            //{
            //    tbEntryUnitPrice.ReadOnly = true;
            //    tbEntryUnitPrice.BackColor = System.Drawing.SystemColors.Info;
            //}
        }

        private void cbEntryStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryStage.SelectedItem != null) tempStage = cbEntryStage.SelectedItem.ToString().Split('|')[0].Trim();
        }

        private void cbEntryForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryForm.SelectedItem != null) tempForm = cbEntryForm.SelectedItem.ToString().Split('|')[0].Trim();
            setLotNbr();
            loadUnitPrice();
        }

        private void cbEntryCropYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryCropYear.SelectedItem != null) tempCropYear = cbEntryCropYear.SelectedItem.ToString().Split('|')[0].Trim();
            loadUnitPrice();
        }

        private void cbEntryGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryGrade.SelectedItem != null) tempGrade = cbEntryGrade.SelectedItem.ToString().Split('|')[0].Trim();
            if (groupEntry.Text.Contains("<NEW>") && cbEntryGrade.SelectedIndex >= 0) tbEntryGradeDraft.Text = tempGrade;
            loadUnitPrice();
        }

        private void cbEntryArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryArea.SelectedItem != null) tempArea = cbEntryArea.SelectedItem.ToString().Split('|')[0].Trim();
            loadUnitPrice();
        }

        private void cbEntryStalk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryStalk.SelectedItem != null) tempStalkPosition = cbEntryStalk.SelectedItem.ToString().Split('|')[0].Trim();
        }

        private void tbEntryWeightReceive_TextChanged(object sender, EventArgs e)
        {
            Decimal rope = 0;
            Decimal weightnetto = 0;
            Decimal weightshipping = 0;
            Decimal received = 0;
            Decimal tare = 0;

            if (tbEntryWeightTare.TextLength > 0)
            {
                tare = Convert.ToDecimal(tbEntryWeightTare.Text);
            }

            if (tbEntryWeightReceive.TextLength > 0)
            {
                received = Convert.ToDecimal(tbEntryWeightReceive.Text);
            }

            tbEntryWeightNetto.Text = (received - tare).ToString();

            if (tbEntryWeightRope.TextLength > 0)
            {
                rope = Convert.ToDecimal(tbEntryWeightRope.Text);
                weightnetto = Convert.ToDecimal(tbEntryWeightTare.Text);
                weightshipping = weightnetto * (rope / 100);
                tbEntryWeightNetto.Text = (weightnetto - weightshipping).ToString();
            }

            //tbEntryWeightNetto.Text = (received - rope - tare).ToString();

        }

        public void perhitungan()
        {
            Decimal rope = 0;
            Decimal received = 0;
            Decimal tare = 0;
            Decimal weightnetto = 0;

            try
            {

                if (Convert.ToInt32(tbEntryWeightRope.Text) > 100)
                {
                    tbEntryWeightRope.Text = (100).ToString();
                }

                if (Convert.ToInt32(tbEntryWeightRope.Text) < 0)
                {
                    tbEntryWeightRope.Text = (0).ToString();
                }
            }
            catch (Exception e)
            {


            }


            if (tbEntryWeightTare.TextLength > 0)
            {
                tare = Convert.ToDecimal(tbEntryWeightTare.Text);
            }

            if (tbEntryWeightReceive.TextLength > 0)
            {
                received = Convert.ToDecimal(tbEntryWeightReceive.Text);
            }

            tbEntryWeightNetto.Text = (received - tare).ToString();

            if (tbEntryWeightRope.TextLength > 0)
            {
                rope = Convert.ToDecimal(tbEntryWeightRope.Text);
                tbEntryWeightShipping.Text = (Convert.ToDecimal(tbEntryWeightNetto.Text) * rope / 100).ToString();
                tbEntryWeightNetto.Text = (Convert.ToDecimal(tbEntryWeightNetto.Text) - Convert.ToDecimal(tbEntryWeightShipping.Text)).ToString();
            }
        }

        private void tbEntryWeightRope_TextChanged(object sender, EventArgs e)
        {

            perhitungan();

            //tbEntryWeightNetto.Text = (received - rope - tare).ToString();

        }

        private void tbEntryWeightTare_TextChanged(object sender, EventArgs e)
        {
            perhitungan();
        }

        private void tbEntryWeightNetto_TextChanged(object sender, EventArgs e)
        {
            setValue();
        }

        private void tbEntryUnitPrice_TextChanged(object sender, EventArgs e)
        {
            setValue();
        }

        private void checkNTRM_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                tbEntryNTRM.Text = "0";
                tbEntryNTRM.ReadOnly = false;
                tbEntryNTRM.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                tbEntryNTRM.Text = "0";
                tbEntryNTRM.ReadOnly = true;
                tbEntryNTRM.BackColor = System.Drawing.SystemColors.Info;
            }

            setValue();
        }

        private void tbEntryNTRM_TextChanged(object sender, EventArgs e)
        {
            setValue();
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            //resetEntry();
            tbEntryWeightReceive.Text = tbScale.Text;
        }

        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            resetEntry();
        }

        private void btnSaveLot_Click(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                saveDocument();
            }

            if (DocNumber != "<NEW>")
            {

                if (groupEntry.Text.Contains("<NEW>"))
                {
                    setLotNbr();

                    string lotnbr = tbEntryLot.Text;

                    if (!checkLotNbrExist(lotnbr))
                    {
                        tbEntryLot.Text = lotnbr.Trim();

                        if (Convert.ToDouble(tbEntryNettValue.Text) > 0)
                        {

                            saveLot(0);
                        }
                        else
                        {
                            if (checkReject.Checked || tempSource == "B" || tempSource == "F")
                            {
                                saveLot(0);
                            }
                            else
                            {
                                MessageBox.Show($"Nett value is zero, please check entry!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Lot number {lotnbr} already exist, or unable to check existing lot number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }


                }
                else
                {
                    if (Convert.ToDouble(tbEntryNettValue.Text) > 0)
                    {
                        saveLot(1);
                    }
                    else
                    {
                        if (checkReject.Checked || tempSource == "B" || tempSource == "F")
                        {
                            MessageBox.Show($"This lot will be saved with zero value!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            saveLot(1);
                        }
                        else
                        {
                            MessageBox.Show($"Nett value is zero, please check entry!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }


            }
        }

        private bool checkLotNbrExist(string lotnbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM StockItem WHERE LotNbr = '{lotnbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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
                }
            }
        }

        private bool checkmobilegriding(string lotnbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM BuyingRecord WHERE LotNum = '{lotnbr}') BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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
                            return false;
                        }
                        else
                        {
                            return true;
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

                }
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

        private void saveLot(int OperationType)
        {

            if ((double.Parse(tbEntryWeightReceive.Text != "" ? tbEntryWeightReceive.Text : "0") > 0 && double.Parse(tbEntryWeightNetto.Text != "" ? tbEntryWeightNetto.Text : "0") > 0) || checkReject.Checked)
            {
                // katanya pak is permintaan manajemen harus di turuti FL- itu grade dengan price 0
                if ((tbEntryNettValue.Text != "0" && tbEntryNettValue.Text != "" && tbEntryNettValue.Text != "0.00") || checkReject.Checked || cbEntryGrade.Text == "-NOT GRADED-"|| tbEntryUnitPrice.Text == "0.00")
                {

                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        bool insertError = false;
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_BuyingLineDetail_v3", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                command.Parameters.AddWithValue("@InventoryID", tbEntryInv.Text);
                                command.Parameters.AddWithValue("@SubItem", $"{tempStage}.{tempForm}.{tempCropYear}.{tempGrade}");
                                command.Parameters.AddWithValue("@LotNbr", tbEntryLot.Text);
                                command.Parameters.AddWithValue("@Source", tempSource);
                                command.Parameters.AddWithValue("@Stage", tempStage);
                                command.Parameters.AddWithValue("@tForm", tempForm);
                                command.Parameters.AddWithValue("@CropYear", tempCropYear);
                                command.Parameters.AddWithValue("@Grade", tempGrade);
                                command.Parameters.AddWithValue("@Area", tempArea);
                                command.Parameters.AddWithValue("@StalkPosition", tempStalkPosition);
                                command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(tbEntryWeightRope.Text == "" ? "0" : tbEntryWeightRope.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(tbEntryWeightShipping.Text == "" ? "0" : tbEntryWeightShipping.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(tbEntryWeightReceive.Text == "" ? "0" : tbEntryWeightReceive.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(tbEntryWeightTare.Text == "" ? "0" : tbEntryWeightTare.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(tbEntryWeightNetto.Text == "" ? "0" : tbEntryWeightNetto.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@UoM", tbEntryWeightUoM.Text);
                                command.Parameters.AddWithValue("@CostUnit", Convert.ToDecimal(tbEntryUnitPrice.Text == "" ? "0" : tbEntryUnitPrice.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@CostGross", Convert.ToDecimal(tbEntryGrossValue.Text == "" ? "0" : tbEntryGrossValue.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@NTRM", checkNTRM.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@CostNTRM", Convert.ToDecimal(tbEntryNTRM.Text == "" ? "0" : tbEntryNTRM.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@CostNett", Convert.ToDecimal(tbEntryNettValue.Text == "" ? "0" : tbEntryNettValue.Text.Replace(",", "")));
                                command.Parameters.AddWithValue("@MC", checkMC.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@Remark", tbEntryRemark.Text);
                                command.Parameters.AddWithValue("@StatusReject", checkReject.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@SyncDetail", 0);
                                command.Parameters.AddWithValue("@OrderNbr", tempPO);
                                command.Parameters.AddWithValue("@NoKontrak", tempKontrak ?? "");
                                command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text);
                                command.Parameters.AddWithValue("@GradeDraft", tempGradeDraft);
                                command.Parameters.AddWithValue("@OperationType", OperationType);
                                command.Parameters.AddWithValue("@Residue", checkResidue.Checked ? 1 : 0);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            insertError = true;
                        }
                        finally
                        {
                            

                        }

                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            if (!insertError)
                            {

                                if (!groupEntry.Text.Contains(tbEntryLot.Text))
                                {
                                    groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                                    lastLotIncrementValue = lastLotIncrementValue + 1;

                                    using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@NumberingID", "BY/LOT");
                                        command.Parameters.AddWithValue("@LastIncrementValue", lastLotIncrementValue);
                                        command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                        command.ExecuteNonQuery();
                                    }
                                }

                                //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                //resetEntry();

                                var lotNbr = tbEntryLot.Text;
                                loadDetail();
                                //fix selected row
                                foreach (DataGridViewRow row in dgvDetail.Rows)
                                {
                                    if (row.Cells[3].Value.ToString() == lotNbr)
                                        row.Selected = true;
                                }

                                if (!groupEntry.Text.Contains("<NEW>"))
                                {
                                    //groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                                    btnPrintLot.Enabled = true;

                                    //trigger print and new lot
                                    btnPrintLot.PerformClick();
                                    btnAddEntry.PerformClick();
                                    cbEntryGrade.Focus();
                                    //resetEntry();
                                }
                            }

                            //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //resetEntry();

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
                    MessageBox.Show("Entry data tidak lengkap, \nCheck entry data buying anda kembali", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Entry berat belum di masukkan, \nCheck entry data berat anda kembali", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool saveLotQC(int statusReject, string remark)
        {
            bool myReturn = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                bool insertError = false;
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Insert_BuyingLineDetailQC_v2", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@InventoryID", tbEntryInv.Text);
                        command.Parameters.AddWithValue("@SubItem", $"{tempStage}.{tempForm}.{tempCropYear}.{tempGrade}");
                        command.Parameters.AddWithValue("@LotNbr", tbEntryLot.Text);
                        command.Parameters.AddWithValue("@Source", tempSource);
                        command.Parameters.AddWithValue("@Stage", tempStage);
                        command.Parameters.AddWithValue("@tForm", tempForm);
                        command.Parameters.AddWithValue("@CropYear", tempCropYear);
                        command.Parameters.AddWithValue("@Grade", tempGrade);
                        command.Parameters.AddWithValue("@Area", tempArea);
                        command.Parameters.AddWithValue("@StalkPosition", tempStalkPosition);
                        command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(tbEntryWeightRope.Text == "" ? "0" : tbEntryWeightRope.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(tbEntryWeightShipping.Text == "" ? "0" : tbEntryWeightShipping.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(tbEntryWeightReceive.Text == "" ? "0" : tbEntryWeightReceive.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(tbEntryWeightTare.Text == "" ? "0" : tbEntryWeightTare.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(tbEntryWeightNetto.Text == "" ? "0" : tbEntryWeightNetto.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@UoM", tbEntryWeightUoM.Text);
                        command.Parameters.AddWithValue("@CostUnit", Convert.ToDecimal(tbEntryUnitPrice.Text == "" ? "0" : tbEntryUnitPrice.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@CostGross", Convert.ToDecimal(tbEntryGrossValue.Text == "" ? "0" : tbEntryGrossValue.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@NTRM", checkNTRM.Checked ? 1 : 0);
                        command.Parameters.AddWithValue("@CostNTRM", Convert.ToDecimal(tbEntryNTRM.Text == "" ? "0" : tbEntryNTRM.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@CostNett", Convert.ToDecimal(tbEntryNettValue.Text == "" ? "0" : tbEntryNettValue.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@MC", checkMC.Checked ? 1 : 0);
                        command.Parameters.AddWithValue("@Remark", remark);
                        command.Parameters.AddWithValue("@StatusReject", statusReject);
                        command.Parameters.AddWithValue("@SyncDetail", 0);
                        command.Parameters.AddWithValue("@OrderNbr", tempPO);
                        command.Parameters.AddWithValue("@NoKontrak", tempKontrak ?? "");
                        command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text);
                        command.Parameters.AddWithValue("@GradeDraft", tempGradeDraft);
                        command.Parameters.AddWithValue("@QCLock", statusReject);
                        command.Parameters.AddWithValue("@OperationType", 0);
                        command.Parameters.AddWithValue("@Residue", checkResidue.Checked ? 1 : 0);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    insertError = true;
                }
                finally
                {
                    //if (!groupEntry.Text.Contains(tbEntryLot.Text))
                    //{
                    //    lastLotIncrementValue = lastLotIncrementValue + 1;

                    //    using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                    //    {
                    //        command.CommandType = CommandType.StoredProcedure;
                    //        command.Parameters.AddWithValue("@NumberingID", "BY/LOT");
                    //        command.Parameters.AddWithValue("@LastIncrementValue", lastLotIncrementValue);
                    //        command.Parameters.AddWithValue("@NumberingDate", currentDate);

                    //        command.ExecuteNonQuery();
                    //    }
                    //}

                    ////MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ////resetEntry();

                    //var lotNbr = tbEntryLot.Text;
                    //loadDetail();
                    ////fix selected row
                    //foreach (DataGridViewRow row in dgvDetail.Rows)
                    //{
                    //    if (row.Cells[3].Value.ToString() == lotNbr)
                    //        row.Selected = true;
                    //}

                    ////groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                    //btnPrintLot.Enabled = true;

                    ////trigger print and new lot
                    ////btnPrintLot.PerformClick();
                    ////btnAddEntry.PerformClick();
                    //resetEntry();
                }


                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    if (!insertError)
                    {

                        if (!groupEntry.Text.Contains(tbEntryLot.Text))
                        {
                            lastLotIncrementValue = lastLotIncrementValue + 1;

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", "BY/LOT");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastLotIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }
                        }

                        //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //resetEntry();

                        var lotNbr = tbEntryLot.Text;
                        loadDetail();
                        //fix selected row
                        foreach (DataGridViewRow row in dgvDetail.Rows)
                        {
                            if (row.Cells[3].Value.ToString() == lotNbr)
                                row.Selected = true;
                        }

                        //groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                        btnPrintLot.Enabled = true;

                        //trigger print and new lot
                        //btnPrintLot.PerformClick();
                        //btnAddEntry.PerformClick();
                        resetEntry();

                        myReturn = true;
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"Update numbering setting failed, please contact IT", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    insertError = true;
                    myReturn = false;
                }


                return myReturn;
            }
        }

        #endregion Entry

        #region Details

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
	                                        BuyingLineDetail
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
                    dgvDetail.Columns[5].HeaderText = "Stage";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Form";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.Columns[7].HeaderText = "Crop Year";
                    dgvDetail.Columns[7].Visible = false;
                    dgvDetail.Columns[8].HeaderText = "Grade";
                    dgvDetail.Columns[8].Visible = false;
                    dgvDetail.Columns[9].HeaderText = "Area";
                    dgvDetail.Columns[10].HeaderText = "Stalk Position";
                    dgvDetail.Columns[11].HeaderText = "MC%";
                    dgvDetail.Columns[8].Visible = false;
                    dgvDetail.Columns[11].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[12].HeaderText = "Shipping";
                    dgvDetail.Columns[8].Visible = false;
                    dgvDetail.Columns[12].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[13].HeaderText = "Receive";
                    dgvDetail.Columns[13].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[14].HeaderText = "Tare";
                    dgvDetail.Columns[14].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[15].HeaderText = "Netto";
                    dgvDetail.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[16].HeaderText = "UoM";
                    dgvDetail.Columns[17].HeaderText = "Unit Price";
                    dgvDetail.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[17].Visible = false;
                    dgvDetail.Columns[18].HeaderText = "Gross Value";
                    dgvDetail.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[18].Visible = false;
                    dgvDetail.Columns[19].HeaderText = "NTRM";
                    dgvDetail.Columns[19].Visible = false;
                    dgvDetail.Columns[20].HeaderText = "NTRM (Deduct)";
                    dgvDetail.Columns[20].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[21].HeaderText = "Nett Value";
                    dgvDetail.Columns[21].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[21].Visible = false;
                    dgvDetail.Columns[22].HeaderText = "MC";
                    //dgvDetail.Columns[22].Visible = false;
                    dgvDetail.Columns[23].HeaderText = "Remark";
                    dgvDetail.Columns[24].HeaderText = "Rejected";
                    dgvDetail.Columns[25].HeaderText = "Synced";
                    dgvDetail.Columns[25].Visible = true;
                    dgvDetail.Columns[26].HeaderText = "Order Number";
                    dgvDetail.Columns[26].Visible = false;
                    dgvDetail.Columns[27].HeaderText = "Contract";
                    dgvDetail.Columns[27].Visible = false;
                    dgvDetail.Columns[28].HeaderText = "Buyer Name";
                    dgvDetail.Columns[28].Visible = false;
                    dgvDetail.Columns[29].HeaderText = "Grade Draft";
                    dgvDetail.Columns[29].Visible = false;
                    dgvDetail.Columns[30].HeaderText = "QCLock";
                    dgvDetail.Columns[31].HeaderText = "Residue";

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        RejectLot.Text = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", "StatusReject=1")).ToString();
                        AllLot.Text = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", string.Empty)).ToString();
                        tbDetailLot.Text = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", "StatusReject=0")).ToString();
                        try { allWN.Text = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty)).ToString("N2"); } catch (Exception s) { allWN.Text = "0"; }
                        try { tbDetailWNetto.Text = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", "StatusReject=0")).ToString("N2"); } catch (Exception s) { tbDetailWNetto.Text = "0"; }
                        try { tbRejectDetailWNetto.Text = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", "StatusReject=1")).ToString("N2"); } catch (Exception s) { tbRejectDetailWNetto.Text = "0"; }
                        try { tbDetailNettValue.Text = Convert.ToDecimal(dtDetail.Compute("SUM(CostNett)", "StatusReject=0")).ToString("N2"); } catch (Exception s) { tbDetailNettValue.Text = "0"; }

                        btnGenerateLotQC.Enabled = false;
                    }
                    else
                    {
                        ////enable change reg
                        //cbRegistration.Enabled = true;
                        //tbRegistration.Enabled = true;
                        //tbBuyerName.ReadOnly = false;
                        //tbBuyerName.BackColor = System.Drawing.SystemColors.Window;

                        if (tempQCExist)
                        {
                            btnGenerateLotQC.Enabled = true;
                        }
                        else
                        {

                            btnGenerateLotQC.Enabled = false;
                        }
                    }

                    if (tbDocNumber.Text != "<NEW>")
                    {
                        //disable change reg
                        cbRegistration.Enabled = false;
                        tbRegistration.Enabled = false;
                        tbBuyerName.ReadOnly = true;
                        tbBuyerName.BackColor = System.Drawing.SystemColors.Info;
                    }
                    else
                    {
                        //enable change reg
                        cbRegistration.Enabled = true;
                        tbRegistration.Enabled = true;
                        tbBuyerName.ReadOnly = false;
                        tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                var lotNbr = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].Value.ToString();

                tbEntryInv.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                //loadComboGrade();

                cbEntrySource.SelectedIndex = cbEntrySource.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[4].Value.ToString());
                cbEntryStage.SelectedIndex = cbEntryStage.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].Value.ToString());
                cbEntryForm.SelectedIndex = cbEntryForm.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[6].Value.ToString());
                cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[7].Value.ToString());
                cbEntryGrade.SelectedIndex = cbEntryGrade.FindStringExact(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].Value.ToString());
                cbEntryArea.SelectedIndex = cbEntryArea.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[9].Value.ToString());
                cbEntryStalk.SelectedIndex = cbEntryStalk.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].Value.ToString());

                tbEntryWeightRope.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[11].Value.ToString();
                tbEntryWeightShipping.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[12].Value.ToString();
                tbEntryWeightReceive.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[13].Value.ToString();
                tbEntryWeightTare.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[14].Value.ToString();
                tbEntryWeightNetto.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[15].Value.ToString();
                tbEntryWeightUoM.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[16].Value.ToString();
                tbEntryUnitPrice.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[17].Value.ToString();
                //tbEntryGrossValue.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[17].Value.ToString();
                checkNTRM.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].Value.ToString() == "1" ? true : false;
                tbEntryNTRM.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[20].Value.ToString();
                checkMC.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[22].Value.ToString() == "1" ? true : false;
                //tbEntryNettValue.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[20].Value.ToString();
                tbEntryRemark.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[23].Value.ToString();
                checkReject.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[24].Value.ToString() == "1" ? true : false;

                tbPO.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[26].Value.ToString();

                tbEntryGradeDraft.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[29].Value.ToString();
                tempQCLock = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[30].Value.ToString() == "1" ? true : false;
                checkResidue.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[31].Value.ToString() == "1" ? true : false;


                if (tempQCLock)
                {
                    checkReject.Enabled = false;
                }
                else
                {
                    checkReject.Enabled = true;
                }


                //var lotNbr = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].Value.ToString();
                groupEntry.Text = $"Lot Entry [{lotNbr}]";
                groupEntry.BackgroundImage = Properties.Resources.editMode;
                tbEntryLot.Text = lotNbr;
                btnPrintLot.Enabled = true;

                if (checkLotNbrInSTock(lotNbr) && checkmobilegriding(lotNbr))
                {


                    btnSaveLot.Enabled = true;
                }
                else
                {
                    groupEntry.BackgroundImage = Properties.Resources.viewMode;
                    btnSaveLot.Enabled = false;
                    if (!checkLotNbrInSTock(lotNbr))
                    {

                    }
                    else if (!checkmobilegriding(lotNbr))
                    {
                        if (tbStatus.Text == "HOLD")
                        {
                            MessageBox.Show("Lot ini telah Grade dari Mobile Aplikasi, Lot tidak dapat edit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                }
                if (tbStatus.Text == "OPEN" || tbStatus.Text == "SYNCED")
                {
                    groupEntry.BackgroundImage = Properties.Resources.viewMode;
                    btnAddEntry.Enabled = false;
                    btnSaveLot.Enabled = false;
                }

            }
        }
        #endregion Details

        private void tbN2_Validated(object sender, EventArgs e)
        {
            ((TextBox)sender).Text = Convert.ToDecimal(((TextBox)sender).Text).ToString("N2");
        }

        private void btnPrintLot_Click(object sender, EventArgs e)
        {
            //if (!groupEntry.Text.Contains("<NEW>") && !tbDocNumber.Text.Contains("<NEW>"))
            if (!tbDocNumber.Text.Contains("<NEW>"))
            {
                if (chkLblPrint.Checked)
                {
                    QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
                    QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode($"{tbEntryLot.Text}", QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    string QRImage = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);

                    if (optLblThermal.Checked)
                    {
                        BuyingProcessLotPrint lotPrint = new BuyingProcessLotPrint
                        {
                            Company = Warehouse.Company,
                            Address = GetBranch(Warehouse.WarehouseID, 3),
                            Phone = GetBranch(Warehouse.WarehouseID, 4),
                            DocNumber = tbDocNumber.Text,
                            BuyDate = tbBuyingDate.Text,
                            LotNumber = tbEntryLot.Text,
                            VendorID = tempVendorID,
                            VendorName = tempVendorName,
                            VendorClass = tbVendorClass.Text,
                            Grade = cbEntryGrade.SelectedItem.ToString(),
                            Mutu = cbEntryForm.SelectedItem.ToString(),
                            Netto = tbEntryWeightNetto.Text,
                            Reject = checkReject.Checked ? "REJECTED" : "",
                            NTRM = checkNTRM.Checked ? "Yes" : "No",
                            MC = checkMC.Checked ? "Yes" : "No",
                            Remark = tbEntryRemark.Text,
                            QRImage = QRImage,
                            Warehouse = Warehouse.Descr,
                            InventoryID = cbInventory.Text,
                            Area = cbEntryArea.SelectedItem.ToString().Substring(0,2),
                            Residue = checkResidue.Checked ? "Yes" : "No",
                            Rope = tbEntryWeightRope.Text
                        };
                        lotPrint.ShowDialog();
                    }
                    else
                    {
                        BuyingProcessLotPrintLbl lotPrint = new BuyingProcessLotPrintLbl
                        {
                            Company = Warehouse.Company,
                            Address = GetBranch(Warehouse.WarehouseID, 3),
                            Phone = GetBranch(Warehouse.WarehouseID, 4),
                            DocNumber = tbDocNumber.Text,
                            BuyDate = tbBuyingDate.Text,
                            LotNumber = tbEntryLot.Text,
                            VendorID = tempVendorID,
                            VendorName = tempVendorName,
                            VendorClass = tbVendorClass.Text,
                            Grade = cbEntryGrade.SelectedItem.ToString(),
                            Mutu = cbEntryForm.SelectedItem.ToString(),
                            Netto = tbEntryWeightNetto.Text,
                            Reject = checkReject.Checked ? "REJECTED" : "",
                            NTRM = checkNTRM.Checked ? "Yes" : "No",
                            MC = checkMC.Checked ? "Yes" : "No",
                            Remark = tbEntryRemark.Text,
                            QRImage = QRImage,
                            Warehouse = Warehouse.Descr,
                            Source = cbEntrySource.SelectedItem.ToString(),
                            Length = "",
                            Process = "BY",
                            StalkPos = cbEntryStalk.SelectedItem.ToString(),
                            Color = "",
                            InventoryID = tbEntryInv.Text,
                            Buyer = tbBuyerName.Text,
                            Ferment = "",
                            Area = cbEntryArea.SelectedItem.ToString().Substring(0,2),
                            Residue = checkResidue.Checked ? "Yes" : "No",
                            Rope = tbEntryWeightRope.Text
                        };
                        lotPrint.ShowDialog();
                    }
                }
            }
            else
            {
                MessageBox.Show("Create a bale/lot first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in dgvDetail.Rows)
            {
                if (Convert.ToInt32(Myrow.Cells[24].Value) == 1)
                {
                    Myrow.DefaultCellStyle.BackColor = System.Drawing.Color.PaleVioletRed;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
            }
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            string referenceNbr = "";
            var docBranch = GetBranch(tbWarehouse.Text, 2);


            loadDetail();
            DataView dv_filter = new DataView(dtDetail, $"StatusReject = 0 and SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

            PurchaseReceipt purchaseReceipt = new PurchaseReceipt();
            //header receipt
            //purchaseReceipt.Branch = Warehouse.WarehouseID;
            purchaseReceipt.Branch = docBranch;
            purchaseReceipt.Type = "Receipt";
            purchaseReceipt.VendorID = tempVendorID;
            purchaseReceipt.VendorRef = DocNumber;
            purchaseReceipt.Date = Convert.ToDateTime(tbBuyingDate.Text);
            purchaseReceipt.Hold = false;
            

            List<PurchaseReceiptDetail> listDetail = new List<PurchaseReceiptDetail>();

            //document details
            foreach (DataRowView rowView in dv_filter)
            {
                
                PurchaseReceiptDetail detail = new PurchaseReceiptDetail();

                detail.Branch = Warehouse.Branch;
                detail.Branch = docBranch;
                detail.InventoryID = rowView[1].ToString();
                detail.Location = AcumaticaCred.AcumaticaInvLocation;
                //detail.LotSerialNbr = rowView[3].ToString();
                detail.ReceiptQty = Convert.ToDecimal(rowView[15]);
                detail.Subitem = rowView[2].ToString().Replace(".", "");
                //detail.TransactionDescription = rowView[0].ToString();
                detail.TransactionDescription = rowView[3].ToString();
                detail.UnitCost = Convert.ToDecimal(rowView[17]);
                detail.ExtendedCost = Convert.ToDecimal(rowView[21]);
                detail.UOM = rowView[16].ToString();
                detail.Warehouse = tbWarehouse.Text;
               

                if (rowView[26].ToString() != "")
                {
                    detail.POOrderNbr = rowView[26].ToString();
                
                    detail.POOrderType = tempPOType;
                    detail.POLineNbr = 1;
                    detail.NoKontrak = rowView[27].ToString();
                }
                listDetail.Add(detail);
            }
            purchaseReceipt.Details = listDetail;

            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
            try
            {
                Console.WriteLine(purchaseReceipt);
                var purchaseReceiptApi = new PurchaseReceiptApi(configuration);
                var response = purchaseReceiptApi.PutEntity(purchaseReceipt);

                ReleasePurchaseReceipt releasePurchaseReceipt = new ReleasePurchaseReceipt((PurchaseReceipt)response);
                purchaseReceiptApi.InvokeAction(releasePurchaseReceipt);

                referenceNbr = response.ReceiptNbr.ToString();
                tbAcumaticaRefNbr.Text = referenceNbr;
            }
            catch (Exception ePut)
            {
                MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                syncError = true;
            }
            finally
            {
                authApi.AuthLogout();
                if (!syncError)
                {
                    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}]";
                    tbStatus.Text = "SYNCED";
                    saveDocument();
                    MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

                                using (SqlCommand command = new SqlCommand("Update_BuyingLineDetail_Sync", connection))
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


        #region TBInputCheck

        private void textbox_check(object sender, KeyPressEventArgs e)
        {
            var oldText = ((TextBox)sender).Text;
            var newChar = e.KeyChar.ToString();
            string newValueString = $"{oldText}{newChar}";
            decimal newValueDecimal;

            if (decimal.TryParse(newValueString, out newValueDecimal))
            {
                e.Handled = false;
            }
            else
            {
                if (e.KeyChar == (char)8)
                {
                    e.Handled = false;//Allow backspace
                }
                else
                {
                    e.Handled = true;
                }
            }

            //if (!char.IsDigit(e.KeyChar)) e.Handled = true;         //Just Digits
            //if (e.KeyChar == (char)44 || e.KeyChar == (char)46) e.Handled = false;            //Allow comma and period
        }

        private void tbEntryUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        private void tbScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        private void tbEntryNTRM_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        private void tbEntryWeightRope_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        private void tbEntryWeightShipping_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        private void tbEntryWeightTare_KeyPress(object sender, KeyPressEventArgs e)
        {
            textbox_check(sender, e);
        }

        #endregion TBInputCheck

        private void checkReject_CheckedChanged(object sender, EventArgs e)
        {
            if (checkReject.Checked)
            {
                tbEntryUnitPrice.Text = "0";
            }
            setValue();
        }

        private void checkAllowOverPO_CheckedChanged(object sender, EventArgs e)
        {
            allowOverPO = ((CheckBox)sender).Checked;

            if (allowOverPO)
            {
                if (!tempQCExist) { btnAddEntry.Enabled = true; } else { btnAddEntry.Enabled = false; }

            }
            else
            {
                if (isOverPO())
                {
                    btnAddEntry.Enabled = false;
                }
                else
                {
                    if (!tempQCExist) { btnAddEntry.Enabled = true; } else { btnAddEntry.Enabled = false; }
                }
            }
        }

        private bool isOverPO()
        {
            decimal totNett = Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));

            if ((totNett > tempPOOpenQty) && tempPO != "")
            {
                return true;
            }
            else { return false; }
        }

        private void tbDetailWNetto_TextChanged(object sender, EventArgs e)
        {
            if (isOverPO())
            {
                tbPOStatus.Text = "WARNING - Over PO Qty!!!";
                tbPOStatus.BackColor = System.Drawing.Color.LightCoral;

                if (!allowOverPO)
                {
                    btnAddEntry.Enabled = false;
                }
                else
                {
                    if (!tempQCExist) { btnAddEntry.Enabled = true; } else { btnAddEntry.Enabled = false; }
                }
            }
            else
            {
                tbPOStatus.Text = "Qty safe / within PO limit";
                tbPOStatus.BackColor = System.Drawing.SystemColors.Info;
                if (!tempQCExist) { btnAddEntry.Enabled = true; } else { btnAddEntry.Enabled = false; }
            }
        }

        private void tbEntryGradeDraft_TextChanged(object sender, EventArgs e)
        {
            tempGradeDraft = ((TextBox)sender).Text;
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
	                                        BuyingLineDetail
	                                    WHERE
		                                    BuyingLineDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        BuyingLineDetail.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.BuyingLineDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            BuyingProcessDocPrint buyingDocPrint = new BuyingProcessDocPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "BUYING PROCESS",
                DocDate = tbBuyingDate.Text,
                NoReg = tempRegNumber,
                VendorID = tempVendorID,
                VendorDetails = tbVendorDetails.Text,
                OrderNbr = tempPO,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc
            };
            buyingDocPrint.ShowDialog();

        }
        
        

        private void tbRegistration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                int index = cbRegistration.FindStringExact(tbRegistration.Text);
                if (index >= 0)
                {
                    cbRegistration.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Registration Code not available !");
                }
            }
        }

        private void btnToogle_Click(object sender, EventArgs e)
        {
            if (tbRegistration.Visible)
            {
                tbRegistration.Visible = false;
                cbRegistration.Visible = true;
            }
            else
            {
                tbRegistration.Visible = true;
                cbRegistration.Visible = false;
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

        private void btnGenerateLotQC_Click(object sender, EventArgs e)
        {

            if (DocNumber == "<NEW>")
            {
                saveDocument();
            }

            if (DocNumber != "<NEW>")
            {

                if (cbEntrySource.SelectedIndex >= 0 && cbEntryForm.SelectedIndex >= 0 && cbEntryCropYear.SelectedIndex >= 0 && cbEntryGrade.SelectedIndex >= 0 && cbEntryArea.SelectedIndex >= 0 && cbEntryStalk.SelectedIndex >= 0)
                {

                    //load data untuk grid
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        //create new dt
                        dtQC = new DataTable();
                        try
                        {
                            string query = $@"SELECT *
                                        FROM
	                                        BuyingQCDetail
                                        WHERE
	                                        DocumentID = '{tempQCDocNumber}' ";

                            SqlCommand command = new SqlCommand(query, connection);
                            connection.Open();

                            SqlDataAdapter da = new SqlDataAdapter(command);
                            da.Fill(dtQC);

                            if (dtQC.Rows.Count > 0)
                            {
                                int countLot = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", string.Empty));
                                foreach (DataRow row in dtQC.Rows)
                                {
                                    var LotRange = row["LotNbr"].ToString().Split('-');
                                    var range = Convert.ToInt32(LotRange[1]) - Convert.ToInt32(LotRange[0]) + 1;
                                    //for (int i = 0; i < range; i++)
                                    //{

                                    //    if (groupEntry.Text.Contains("<NEW>"))
                                    //    {
                                    //        setLotNbr();
                                    //    }

                                    //    saveLotQC(Convert.ToInt32(row["StatusReject"].ToString()), row["Remark"].ToString());
                                    //}

                                    int i = 0;
                                    while (i < range)
                                    {
                                        if (groupEntry.Text.Contains("<NEW>"))
                                        {
                                            setLotNbr();
                                        }

                                        bool success = saveLotQC(Convert.ToInt32(row["StatusReject"].ToString()), row["Remark"].ToString());

                                        if (success)
                                        {
                                            i++;
                                        }
                                    }


                                }
                            }
                            else
                            {
                                MessageBox.Show($"QC details not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }


                        }
                        catch (Exception e3)
                        {
                            MessageBox.Show(e3.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Please set basic lot parameter first!\n[Source, Form, Crop year, Grade, Area, Stalk position]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }




            }


        }

        private void tbScale_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbEntrySource_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryStage_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryForm_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryCropYear_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryGrade_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryArea_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryStalk_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbBuyerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                int index = cbRegistration.FindStringExact(tbRegistration.Text);
                if (index >= 0)
                {
                    cbRegistration.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Registration Code not available !");
                }
            }
        }

        private void unsyncing_Click(object sender, EventArgs e) 
        {
            if (dtDetail.Rows.Count > 0)
            {
                loadDetail();
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 1", "LotNbr Asc", DataViewRowState.CurrentRows);

            
                    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";
                    tbStatus.Text = "OPEN";
                    tbAcumaticaRefNbr.Text = "";
                    saveDocument();
                    //Update Item Sync status
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
                                using (SqlCommand command = new SqlCommand("Update_BuyingLineDetail_Sync", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
                                    command.Parameters.AddWithValue("@LotNbr", rowLotNumber);
                                    command.Parameters.AddWithValue("@SyncDetail", 0);

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
                    loadDetail();
                    // unsync
            }
            else
            {
                MessageBox.Show($"Tidak Singkron Salah", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls|Excel CSV |*.csv" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textFilename.Text = openFileDialog.FileName;
                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                            {

                                DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                                    {
                                        UseHeaderRow = true
                                    }
                                });


                                dtImport = result.Tables;
                                cbosheet.Items.Clear();
                                foreach (DataTable dt in dtImport)
                                {
                                    cbosheet.Items.Add(dt.TableName);
                                }

                            }
                        }
                        SaveImport.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message + " ( Excel Jangan Dibuka OKE )");
                    }


                }
            }
        }


        private void cbosheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDetailImport = dtImport[cbosheet.SelectedItem.ToString()];
            SaveImport.Enabled = true;
        }

        private void SaveImport_Click(object sender, EventArgs e)
        {
            SqlTransaction objTrans = null;
            try
            {
                if (((tempPO.ToString().Trim() != null || tempPO.ToString().Trim() != "") && Convert.ToDecimal(dtDetailImport.Compute("SUM(WeightNetto)", string.Empty)) <= Convert.ToDecimal(tbPOOpenQty.Text))|| (tempPO.ToString().Trim()==null || tempPO.ToString().Trim() == ""))
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        bool insertError = false;
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                                objTrans = connection.BeginTransaction();
                            }


                            foreach (DataRow row in dtDetailImport.Rows)
                            {

                                if (row["InventoryID"].ToString().Trim() != null &&
                                    row["Source"].ToString().Trim() != null &&
                                    row["Stage"].ToString().Trim() != null &&
                                    row["Form"].ToString().Trim() != null &&
                                    row["CropYear"].ToString().Trim() != null &&
                                    row["Grade"].ToString().Trim() != null &&
                                    row["Area"].ToString().Trim() != null &&
                                    row["StalkPosition"].ToString().Trim() != null &&
                                    row["WeightRope"].ToString().Trim() != null &&
                                    row["WeightShipping"].ToString().Trim() != null &&
                                    row["WeightReceive"].ToString().Trim() != null &&
                                    row["WeightTare"].ToString().Trim() != null &&
                                    row["WeightNetto"].ToString().Trim() != null &&
                                    row["UoM"].ToString().Trim() != null &&
                                    row["BuyerName"].ToString().Trim() != null &&
                                    row["CostUnit"].ToString().Trim() != null &&
                                    row["CostGross"].ToString().Trim() != null &&
                                    row["NTRM"].ToString().Trim() != null &&
                                    row["CostNett"].ToString().Trim() != null &&
                                    row["MC"].ToString().Trim() != null &&
                                    row["GradeDraft"].ToString().Trim() != null &&
                                    row["Residue"].ToString().Trim() != null 
                                    )
                                {

                                    string InventoryID = row["InventoryID"].ToString().Trim();
                                    string Source = row["Source"].ToString().Trim();
                                    string Stage = row["Stage"].ToString().Trim();
                                    string Form = row["Form"].ToString().Trim();
                                    string CropYear = row["CropYear"].ToString().Trim();
                                    string Grade = row["Grade"].ToString().Trim();
                                    string Area = row["Area"].ToString().Trim();
                                    string StalkPosition = row["StalkPosition"].ToString().Trim();
                                    string WeightRope = row["WeightRope"].ToString().Trim();
                                    string WeightShipping = row["WeightShipping"].ToString().Trim();
                                    string WeightReceive = row["WeightReceive"].ToString().Trim();
                                    string WeightTare = row["WeightTare"].ToString().Trim();
                                    string WeightNetto = row["WeightNetto"].ToString().Trim();
                                    string UoM = row["UoM"].ToString().Trim();
                                    string Remark = "IMPORT OUT";
                                    string BuyerName = row["BuyerName"].ToString().Trim();
                                    string CostUnit = row["CostUnit"].ToString().Trim();
                                    string CostGross = row["CostGross"].ToString().Trim();
                                    string NTRM = row["NTRM"].ToString().Trim();
                                    string CostNTRM = row["CostNTRM"].ToString().Trim();
                                    string CostNett = row["CostNett"].ToString().Trim();
                                    string MC = row["MC"].ToString().Trim();
                                    string GradeDraft = row["GradeDraft"].ToString().Trim();
                                    string SubItem = $"{Stage}.{Form}.{CropYear}.{Grade}";
                                    string Residue = row["Residue"].ToString().Trim();

                                    using (SqlCommand command = new SqlCommand("Insert_BuyingLineDetail_v3_import", connection, objTrans))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;

                                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                        command.Parameters.AddWithValue("@InventoryID", InventoryID);
                                        command.Parameters.AddWithValue("@SubItem", SubItem);
                                        command.Parameters.AddWithValue("@LotNbr", $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-".ToString());
                                        command.Parameters.AddWithValue("@Source", Source);
                                        command.Parameters.AddWithValue("@Stage", Stage);
                                        command.Parameters.AddWithValue("@Form", Form);
                                        command.Parameters.AddWithValue("@CropYear",CropYear);
                                        command.Parameters.AddWithValue("@Grade", Grade);
                                        command.Parameters.AddWithValue("@Area", Area);
                                        command.Parameters.AddWithValue("@StalkPosition", StalkPosition);
                                        command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(WeightRope));
                                        command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(WeightShipping));
                                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(WeightReceive));
                                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(WeightTare));
                                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(WeightNetto));
                                        command.Parameters.AddWithValue("@UoM", UoM);
                                        command.Parameters.AddWithValue("@CostUnit", CostUnit);
                                        command.Parameters.AddWithValue("@CostGross", CostGross);
                                        command.Parameters.AddWithValue("@NTRM", NTRM);
                                        command.Parameters.AddWithValue("@CostNTRM", CostNTRM);
                                        command.Parameters.AddWithValue("@CostNett", CostNett);
                                        command.Parameters.AddWithValue("@MC", MC);
                                        command.Parameters.AddWithValue("@Remark", "Import Buying");
                                        command.Parameters.AddWithValue("@StatusReject",0);
                                        command.Parameters.AddWithValue("@SyncDetail", 0);
                                        command.Parameters.AddWithValue("@OrderNbr", tempPO);
                                        command.Parameters.AddWithValue("@NoKontrak", tempKontrak ?? "");
                                        command.Parameters.AddWithValue("@BuyerName", BuyerName);
                                        command.Parameters.AddWithValue("@GradeDraft", GradeDraft);
                                        command.Parameters.AddWithValue("@OperationType", 0);
                                        command.Parameters.AddWithValue("@Residue", Residue);

                                        command.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Check Imput Excel !", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            
                            objTrans.Commit();
                            loadBuyingProcess();
                            loadDetail();
                            resetEntry();
                            dtImport.Clear();
                            cbosheet.Items.Clear();
                            cbosheet.Text = "";
                            dtDetailImport.Clear();
                            textFilename.Clear();
                            SaveImport.Enabled = false;
                            MessageBox.Show("Import Success !");
                        }
                        catch (Exception e_update)
                        {
                            objTrans.Rollback();
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                        }
                    }


                }
                else
                {
                    MessageBox.Show($"Total WeightNetto melebihi PO Open Qty !", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"--{ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void checkMC_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                tbEntryWeightRope.Text = "0";
                tbEntryWeightShipping.Text = "0";
                tbEntryWeightRope.ReadOnly = false;
                tbEntryWeightRope.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                tbEntryWeightRope.Text = "0";
                tbEntryWeightShipping.Text = "0";
                tbEntryWeightRope.ReadOnly = true;
                tbEntryWeightRope.BackColor = System.Drawing.SystemColors.Info;
            }

        }

        //end of file
    }
}