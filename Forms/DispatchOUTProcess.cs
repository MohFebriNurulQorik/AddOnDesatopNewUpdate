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
using ExcelDataReader;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ScaleAddon.Forms
{
    public partial class DispatchOUTProcess : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public UserModel Userlog { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }

        private int lastIncrementValue = -1;

        private DataTable dtLot;
        private DataTable dtWarehouse;
        private DataTable dtDetail;
        private DataTable dtEntry;

        private SerialPort port;
        private string LotStockItem = null;

        private DataTableCollection dtImport;
        private DataTable dtDetailImport;
        
        List<dynamic> MapArray = new List<dynamic>();




        public DispatchOUTProcess()
        {
            InitializeComponent();
        }

        private void DispatchOUTProcess_Load(object sender, EventArgs e)
        {
            if (Userlog.UserRoles.Contains("DOT-IMPORT") || Userlog.UserRoles.Contains("SUPERVISOR"))
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

            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
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

        private void resetScreen()
        {
            currentDate = DateTime.Now;
            tbDisptachDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";
            //AcumaticaRefNbr = "";
            checkHold.Checked = true;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";

            loadComboLot();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbTotalCost.Text = "0";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaRefNbr.Text = "";
            tbDispatchNote.Text = "";
            tbLogisticService.Text = "";
            tbLisencePlate.Text = "";

            loadComboWarehouse();

            resetEntry();
        }

        private void resetEntry()
        {
            loadDetail();
         
            groupEntry.Text = "Lot Entry [<NEW>]";

            tbEntryLot.Text = "";
            tbEntryInv.Text = "";
            tbEntryWeightNettoOld.Text = "0";
            tbEntryGrade.Text = "";
            tbEntrySubItem.Text = "";
            tbEntryWeightReceive.Text = "0";
            tbEntryWeightTare.Text = "0";
            tbEntryWeightNetto.Text = "0";

            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = true;
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintWL.Enabled = true;
                    btnPrintSJ.Enabled = false;
                    btnPrintSUM.Enabled = true;
                    cbWarehouseTo.Enabled = false;
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = false;
                    }
                    else
                    {
                        unsyncing.Visible = false;
                    }
                    break;

                case "SYNCED":
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintWL.Enabled = true;
                    btnPrintSJ.Enabled = true;
                    btnPrintSUM.Enabled = true;
                    cbWarehouseTo.Enabled = false;
                    
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = true;
                    }
                    else
                    {
                        unsyncing.Visible = false;
                    }
                    break;

                default:
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintWL.Enabled = false;
                    cbWarehouseTo.Enabled = true;
                    btnPrintSJ.Enabled = false;
                    btnPrintSUM.Enabled = false;
                    if (Userlog.UserRoles.Contains("SUPERVISOR"))
                    {
                        unsyncing.Visible = true;
                        unsyncing.Enabled = false;
                    }
                    else
                    {
                        unsyncing.Visible = false;
                    }
                    break;
            }
        }

        private void loadProcess()
        {
            tbDisptachDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";

            loadComboLot();
            loadComboWarehouse();
            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from DispatchOUTLine where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(4).ToString();
                        tbTotalCost.Text = Convert.ToDecimal(reader.GetValue(5)).ToString("N2");
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(7).ToString();
                        tbDispatchNote.Text = reader.GetValue(8).ToString();
                        cbWarehouseTo.SelectedIndex = cbWarehouseTo.FindString(reader.GetValue(3).ToString());
                        //AcumaticaRefNbr = reader.GetValue(8).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(4).ToString() == "HOLD" ? 1 : 0);
                        tbLogisticService.Text = reader.GetValue(12).ToString();
                        tbLisencePlate.Text = reader.GetValue(13).ToString();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
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
                  
                    string query = $@"SELECT *
                                        FROM
	                                        DispatchOUTLineDetail
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
                    dgvDetail.Columns[6].HeaderText = "Form";
                    dgvDetail.Columns[7].HeaderText = "Crop Year";
                    dgvDetail.Columns[8].HeaderText = "Grade";
                    dgvDetail.Columns[9].HeaderText = "Area";
                    dgvDetail.Columns[10].HeaderText = "Color";
                    dgvDetail.Columns[11].HeaderText = "Fermentation";
                    dgvDetail.Columns[12].HeaderText = "Length";
                    dgvDetail.Columns[13].HeaderText = "Process";
                    dgvDetail.Columns[14].HeaderText = "Stalk Position";
                    dgvDetail.Columns[15].HeaderText = "Rope";
                    dgvDetail.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[16].HeaderText = "Shipping";
                    dgvDetail.Columns[16].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[17].HeaderText = "Receive";
                    dgvDetail.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[18].HeaderText = "Tare";
                    dgvDetail.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[19].HeaderText = "Netto";
                    dgvDetail.Columns[19].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[20].HeaderText = "UoM";
                    dgvDetail.Columns[21].HeaderText = "Remark";
                    dgvDetail.Columns[22].HeaderText = "Old Document ID";
                    dgvDetail.Columns[23].HeaderText = "Synced";
                    dgvDetail.Columns[24].HeaderText = "Unit Cost";
                    dgvDetail.Columns[24].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[24].Visible = false;
                    dgvDetail.Columns[25].HeaderText = "Ext. Cost";
                    dgvDetail.Columns[25].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[25].Visible = false;
                    dgvDetail.Columns[26].HeaderText = "Buyer Name";

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        int countLot = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", string.Empty));
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWShipping = Convert.ToDecimal(dtDetail.Compute("SUM(WeightShipping)", string.Empty));
                        tbDetailWShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = Convert.ToDecimal(dtDetail.Compute("SUM(WeightReceive)", string.Empty));
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = Convert.ToDecimal(dtDetail.Compute("SUM(WeightTare)", string.Empty));
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty));
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                        
                    }
                    else
                    {
                        int countLot = 0;
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWShipping = 0;
                        tbDetailWShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = 0;
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = 0;
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = 0;
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadEntry(string LotNbr)
        {
            resetEntry();

            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtEntry = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT 	DocumentID,
	                                            InventoryID,
	                                            SubItem,
	                                            LotNbr,
	                                            Source,
	                                            Stage,
	                                            Form,
	                                            CropYear,
	                                            Grade,
	                                            Area,
	                                            Color,
	                                            Fermentation,
	                                            Length,
	                                            Process,
	                                            StalkPosition,
	                                            WeightRope,
	                                            WeightShipping,
	                                            WeightReceive,
	                                            WeightTare,
	                                            WeightNetto,
	                                            UoM,
	                                            Remark,
	                                            BuyerName
                                        FROM
	                                        StockItem
                                        WHERE
	                                        LotNbr = '{LotNbr}' AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);

                    tbEntryLot.Text = dtEntry.Rows[0].ItemArray[3].ToString();
                    tbEntryInv.Text = dtEntry.Rows[0].ItemArray[1].ToString();
                    tbEntrySubItem.Text = dtEntry.Rows[0].ItemArray[2].ToString();
                    tbEntryGrade.Text = dtEntry.Rows[0].ItemArray[8].ToString();
                    tbEntryWeightNettoOld.Text = dtEntry.Rows[0].ItemArray[19].ToString();
                    tbEntryWeightReceive.Text = dtEntry.Rows[0].ItemArray[17].ToString();
                    tbEntryWeightTare.Text = dtEntry.Rows[0].ItemArray[18].ToString();
                    tbEntryWeightNetto.Text = dtEntry.Rows[0].ItemArray[19].ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadEntryDetail(string LotNbr)
        {
            resetEntry();

            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtEntry = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT OldDocumentID,
	                                            InventoryID,
	                                            SubItem,
	                                            LotNbr,
	                                            Source,
	                                            Stage,
	                                            Form,
	                                            CropYear,
	                                            Grade,
	                                            Area,
	                                            Color,
	                                            Fermentation,
	                                            Length,
	                                            Process,
	                                            StalkPosition,
	                                            WeightRope,
	                                            WeightShipping,
	                                            WeightReceive,
	                                            WeightTare,
	                                            WeightNetto,
	                                            UoM,
	                                            Remark,
	                                            BuyerName
                                        FROM
	                                        DispatchOUTLineDetail
                                        WHERE
                                            DocumentID = '{DocNumber}'
                                        AND
	                                        LotNbr = '{LotNbr}'";
                    //AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);

                    tbEntryLot.Text = dtEntry.Rows[0].ItemArray[3].ToString();
                    tbEntryInv.Text = dtEntry.Rows[0].ItemArray[1].ToString();
                    tbEntrySubItem.Text = dtEntry.Rows[0].ItemArray[2].ToString();
                    tbEntryGrade.Text = dtEntry.Rows[0].ItemArray[8].ToString();
                    tbEntryWeightNettoOld.Text = dtEntry.Rows[0].ItemArray[16].ToString();
                    tbEntryWeightReceive.Text = dtEntry.Rows[0].ItemArray[17].ToString();
                    tbEntryWeightTare.Text = dtEntry.Rows[0].ItemArray[18].ToString();
                    tbEntryWeightNetto.Text = dtEntry.Rows[0].ItemArray[19].ToString();
                }
                catch (Exception e)
                {
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
                //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-IO/OUT-{currentIncrement.ToString().PadLeft(4, '0')}";
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-IO/OUT-{currentIncrement.ToString().PadLeft(4, '0')}";
                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-IO/OUT-{currentIncrement.ToString().PadLeft(4, '0')}";

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
                    string query = $"IF EXISTS ( SELECT * FROM DispatchOUTLine WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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

        private void getDocLastIncrement()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from NumberingSetting where NumberingID = 'IO/OUT'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        DateTime dbDate = Convert.ToDateTime(reader.GetValue(2).ToString());
                        int dbIncrement = Convert.ToInt32(reader.GetValue(1).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        //var cur = currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year;
                        //var last = dbDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year;
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

        private void loadComboLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtLot = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    StockItem.LotNbr
                                    FROM
	                                    dbo.StockItem
                                    WHERE
	                                    StockItem.StatusStock = 1";

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

        private void loadComboWarehouse()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtWarehouse = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    *
                                    FROM
	                                    WarehouseSite
                                    WHERE
	                                    WarehouseID != '{tbWarehouse.Text}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtWarehouse);
                    string[] arrray = dtWarehouse.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbLot);
                    cbWarehouseTo.Items.Clear();
                    cbWarehouseTo.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void saveDocument()
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

            if (!incrementError)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_DispatchOUTLine_v2", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbDisptachDate.Text);
                            command.Parameters.AddWithValue("@WarehouseIDFrom", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@WarehouseIDTo", cbWarehouseTo.SelectedItem != null ? cbWarehouseTo.SelectedItem.ToString() : "");
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@TotalCost", tbTotalCost.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalWeight", tbDetailWNetto.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                            command.Parameters.AddWithValue("@Note", tbDispatchNote.Text);
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                            command.Parameters.AddWithValue("@LogisticService", tbLogisticService.Text);
                            command.Parameters.AddWithValue("@LisencePlate", tbLisencePlate.Text);
                            command.Parameters.AddWithValue("@OperationType", OperationType);

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
                    //        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";

                    //        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                    //        {
                    //            command.CommandType = CommandType.StoredProcedure;
                    //            command.Parameters.AddWithValue("@NumberingID", $"IO/OUT");
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
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"IO/OUT");
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
                btnPrintWL.Enabled = true;
                btnPrintSUM.Enabled = true;
            }
            else
            {
                btnPrintWL.Enabled = false;
                btnPrintSUM.Enabled = false;
            }

            if (tbStatus.Text == "SYNCED")
            {
                btnPrintSJ.Enabled = true;
            }
            else
            {
                btnPrintSJ.Enabled = false;
            }


            btnPrintSJ.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
            resetEntry();
        }

        private void cbLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLot.SelectedIndex >= 0)
            {
                loadEntry(cbLot.SelectedItem.ToString());
            }
        }

        private void saveLot()
        {
            if(tbEntryWeightNetto.Text !="" && tbEntryWeightNetto.Text != "0" && Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", "")) > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_DispatchOUTLineDetail", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", DocNumber);
                            command.Parameters.AddWithValue("@InventoryID", dtEntry.Rows[0].ItemArray[1].ToString());
                            command.Parameters.AddWithValue("@SubItem", dtEntry.Rows[0].ItemArray[2].ToString());
                            command.Parameters.AddWithValue("@LotNbr", dtEntry.Rows[0].ItemArray[3].ToString());
                            command.Parameters.AddWithValue("@Source", dtEntry.Rows[0].ItemArray[4].ToString());
                            command.Parameters.AddWithValue("@Stage", dtEntry.Rows[0].ItemArray[5].ToString());
                            command.Parameters.AddWithValue("@tForm", dtEntry.Rows[0].ItemArray[6].ToString());
                            command.Parameters.AddWithValue("@CropYear", dtEntry.Rows[0].ItemArray[7].ToString());
                            command.Parameters.AddWithValue("@Grade", dtEntry.Rows[0].ItemArray[8].ToString());
                            command.Parameters.AddWithValue("@Area", dtEntry.Rows[0].ItemArray[9].ToString());
                            command.Parameters.AddWithValue("@Color", dtEntry.Rows[0].ItemArray[10].ToString());
                            command.Parameters.AddWithValue("@Fermentation", dtEntry.Rows[0].ItemArray[11].ToString());
                            command.Parameters.AddWithValue("@Length", dtEntry.Rows[0].ItemArray[12].ToString());
                            command.Parameters.AddWithValue("@Process", dtEntry.Rows[0].ItemArray[13].ToString());
                            command.Parameters.AddWithValue("@StalkPosition", dtEntry.Rows[0].ItemArray[14].ToString());
                            command.Parameters.AddWithValue("@WeightRope", dtEntry.Rows[0].ItemArray[15].ToString());
                            command.Parameters.AddWithValue("@WeightShipping", dtEntry.Rows[0].ItemArray[19].ToString());
                            //command.Parameters.AddWithValue("@WeightReceive", dtEntry.Rows[0].ItemArray[17].ToString());
                            //command.Parameters.AddWithValue("@WeightTare", dtEntry.Rows[0].ItemArray[18].ToString());
                            //command.Parameters.AddWithValue("@WeightNetto", dtEntry.Rows[0].ItemArray[19].ToString());
                            //command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", "")));
                            command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(tbEntryWeightReceive.Text.Replace(",", "")));
                            command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(tbEntryWeightTare.Text.Replace(",", "")));
                            command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", "")));
                            command.Parameters.AddWithValue("@UoM", dtEntry.Rows[0].ItemArray[20].ToString());
                            command.Parameters.AddWithValue("@Remark", dtEntry.Rows[0].ItemArray[21].ToString());
                            command.Parameters.AddWithValue("@OldDocumentID", dtEntry.Rows[0].ItemArray[0].ToString());
                            command.Parameters.AddWithValue("@SyncDetail", 0);
                            command.Parameters.AddWithValue("@UnitCost", 0);
                            command.Parameters.AddWithValue("@ExtCost", 0);
                            command.Parameters.AddWithValue("@BuyerName", dtEntry.Rows[0].ItemArray[22].ToString());

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //resetEntry();
                        loadDetail();
                        groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                        cbLot.SelectedIndex = -1;
                        loadComboLot();
                    }
                }
            }
            else
            {
                MessageBox.Show($"Empty lot or null value, please check entry and try to save again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void removeLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Delete_DispatchOUTLineDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@LotNbr", tbEntryLot.Text);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Delete error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    //MessageBox.Show("Remove lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //resetEntry();
                    loadDetail();
                    resetEntry();
                    cbLot.SelectedIndex = -1;
                    loadComboLot();
                }
            }
        }

        private void removeAllLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Delete_DispatchOUTLineDetail_All", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Delete error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    //MessageBox.Show("Remove lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //resetEntry();
                    loadDetail();
                    resetEntry();
                    cbLot.SelectedIndex = -1;
                    loadComboLot();
                }
            }
        }

        private void btnSaveLot_Click(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                saveDocument();
            }

            if (DocNumber != "<NEW>")
            {
                saveLot();
                tbLot.Text = "";
                tbLot.Focus();
                saveDocument();

                if (tbLot.Visible)
                {
                    tbLot.Focus();
                }
                resetEntry();
            }
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void btnDelLot_Click(object sender, EventArgs e)
        {
            removeLot();
            saveDocument();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetScreen();
            tbLot.Focus();
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            if (cbWarehouseTo.SelectedIndex < 0)
            {
                MessageBox.Show($"Please check warehouse selection!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                acumaticaSync();
            }
        }
        private void acumaticaSync()
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            bool allSynced = true;
            string msalahdocNbr = "";
            bool issueSynced = false;
            string docNbr = "";
            string referenceNbr = tbAcumaticaRefNbr.Text ?? "";
            var docBranch = GetBranch(tbWarehouse.Text, 2);

            if (tbAcumaticaRefNbr.Text != "")
            {
                issueSynced = true;
                referenceNbr = tbAcumaticaRefNbr.Text;
            }

            loadDetail();
            DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

            //issue
            ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
            inventoryIssue.Date = Convert.ToDateTime(tbDisptachDate.Text);
            inventoryIssue.Description = $"{tbWarehouse.Text} Dispatch OUT Transaction Issue";
            inventoryIssue.ExternalRef = tbDocNumber.Text;
            //inventoryIssue.Hold = false;

            List<ULTInventoryIssueDetail> issueDetails = new List<ULTInventoryIssueDetail>();
            foreach (DataRowView rowView in dv_filter)
            {
                //check if doc buying already synced
                if (docNbr != rowView[22].ToString())
                {
                    docNbr = rowView[22].ToString();
                    if (!checkDocumentSync(docNbr))
                    {
                        msalahdocNbr = rowView[22].ToString();
                        allSynced = false;
                    }
                }
                ULTInventoryIssueDetail issueDetail = new ULTInventoryIssueDetail();
                issueDetail.InventoryID = rowView[1].ToString();
                issueDetail.Branch = docBranch;
                issueDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                issueDetail.Subitem = rowView[2].ToString().Replace(".", "");
                issueDetail.Quantity = Convert.ToDecimal(rowView[16]);
                Console.WriteLine("----------");
                Console.WriteLine(rowView[19]);
                Console.WriteLine("----------");
                issueDetail.Description = rowView[3].ToString();

                issueDetail.Warehouse = tbWarehouse.Text;
                issueDetail.UOM = rowView[20].ToString();
                issueDetail.ReasonCode = "ISSUEDISPATCH";
                //issueDetail.LotSerialNbr = "";

                issueDetails.Add(issueDetail);
            }
            inventoryIssue.Details = issueDetails;

            if (!allSynced)
            {
                MessageBox.Show($"Documents {msalahdocNbr} for bale contained in this process is not synced to Acumatica!\nPlease sync required documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                if (!issueSynced)
                {
                    var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                    var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                    try
                    {
                        var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
                        //var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);
                        //ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
                        //inventoryIssueApi.InvokeAction(releaseInventoryIssue);

                        //referenceNbr = responseIssue.ReferenceNbr.ToString();
                        //tbAcumaticaRefNbr.Text = referenceNbr;

                        if (referenceNbr == "")
                        {
                            var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);

                            referenceNbr = responseIssue.ReferenceNbr.ToString();
                            tbAcumaticaRefNbr.Text = referenceNbr;
                            saveDocument();


                            ReleaseFromHoldInventoryIssue releaseFromHoldInventoryIssue = new ReleaseFromHoldInventoryIssue((ULTInventoryIssue)responseIssue);
                            inventoryIssueApi.InvokeAction(releaseFromHoldInventoryIssue);


                            //throw new InvalidOperationException("error");
                            ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
                            inventoryIssueApi.InvokeAction(releaseInventoryIssue);

                        }
                        else
                        {
                            var responseIssue = inventoryIssueApi.GetList(select: "ReferenceNbr", filter: $"ReferenceNbr eq '{referenceNbr}'");


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
                        if (!syncError)
                        {
                            checkReleasedIssue(referenceNbr, configuration);
                            issueSynced = true;
                        }

                        authApi.AuthLogout();

                        if (!syncError)
                        {
                            saveDocument();
                            //MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

                                        using (SqlCommand command = new SqlCommand("Update_DispatchOUTLineDetail_Sync", connection))
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
                        }
                    }
                }
            }

            //go to transfer screen
            if (!issueSynced)
            {
                MessageBox.Show($"Issue document for bale contained in this process is not synced to Acumatica!\nPlease sync required documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                syncError = false;
                //transfer screen
                ULTTobaccoTransfer tobaccoTransfer = new ULTTobaccoTransfer();
                tobaccoTransfer.DispatchDate = inventoryIssue.Date;
                tobaccoTransfer.DispatchFrom = tbWarehouse.Text;
                tobaccoTransfer.DispatchTo = cbWarehouseTo.SelectedItem.ToString();
                tobaccoTransfer.AddOnNbr = DocNumber;
                tobaccoTransfer.DispatchNote = tbDispatchNote.Text;
                tobaccoTransfer.IssueRefNbr = tbAcumaticaRefNbr.Text;
                tobaccoTransfer.TotalCost = Convert.ToDecimal(tbTotalCost.Text.ToString().Replace(",", ""));
                tobaccoTransfer.TotalQty = Convert.ToDecimal(tbDetailWNetto.Text.ToString().Replace(",", ""));
                tobaccoTransfer.LogisticService = tbLogisticService.Text;
                tobaccoTransfer.LisencePlate = tbLisencePlate.Text;

                List<ULTTobaccoTransferDetail> transferDetails = new List<ULTTobaccoTransferDetail>();

                loadDetail();
                dv_filter = new DataView(dtDetail, $"SyncDetail = 1", "LotNbr Asc", DataViewRowState.CurrentRows);

                foreach (DataRowView rowView in dv_filter)
                {
                    ULTTobaccoTransferDetail transferDetail = new ULTTobaccoTransferDetail();
                    transferDetail.InventoryID = rowView[1].ToString();
                    transferDetail.Subitem = rowView[2].ToString();
                    transferDetail.LotNo = rowView[3].ToString();
                    transferDetail.Source = rowView[4].ToString();
                    transferDetail.Stage = rowView[5].ToString();
                    transferDetail.Form = rowView[6].ToString();
                    transferDetail.CropYear = rowView[7].ToString();
                    transferDetail.Grade = rowView[8].ToString();
                    transferDetail.Area = rowView[9].ToString();
                    transferDetail.Color = rowView[10].ToString();
                    transferDetail.Fermentation = rowView[11].ToString();
                    transferDetail.Length = rowView[12].ToString();
                    transferDetail.Process = rowView[13].ToString();
                    transferDetail.StalkPositions = rowView[14].ToString();
                    transferDetail.QtyRope = Convert.ToDecimal(rowView[15]);
                    transferDetail.QtyShip = Convert.ToDecimal(rowView[16]);
                    transferDetail.QtyGross = Convert.ToDecimal(rowView[17]);
                    transferDetail.QtyTare = Convert.ToDecimal(rowView[18]);
                    transferDetail.QtyNetto = Convert.ToDecimal(rowView[19]);
                    transferDetail.UOM = rowView[20].ToString();
                    transferDetail.UnitCost = Convert.ToDecimal(rowView[24]);
                    transferDetail.ExtCost = Convert.ToDecimal(rowView[25]);
                    transferDetail.BuyerName = rowView[26].ToString();

                    transferDetails.Add(transferDetail);
                }
                tobaccoTransfer.Details = transferDetails;

                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                try
                {
                    var tobaccoTransferApi = new ULTTobaccoTransferApi(configuration);
                    var responseTransfer = tobaccoTransferApi.PutEntity(tobaccoTransfer);
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
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";
                        tbStatus.Text = "SYNCED";

                        saveDocument();
                        MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //Update Item Sync status
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
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
            } // end if else
        }

        //private void acumaticaSync()
        //{
        //    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}] - Syncing with Acumatica, please wait!";
        //    bool syncError = false;
        //    bool allSynced = true;
        //    bool issueSynced = false;
        //    string docNbr = "";
        //    string referenceNbr = tbAcumaticaRefNbr.Text ?? "";
        //    var docBranch = GetBranch(tbWarehouse.Text, 2);

        //    if (tbAcumaticaRefNbr.Text != "")
        //    {
        //        issueSynced = true;
        //        referenceNbr = tbAcumaticaRefNbr.Text;
        //    }

        //    loadDetail();
        //    DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

        //    //issue
        //    ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
        //    inventoryIssue.Date = Convert.ToDateTime(tbDisptachDate.Text);
        //    inventoryIssue.Description = $"{tbWarehouse.Text} Dispatch OUT Transaction Issue";
        //    inventoryIssue.ExternalRef = tbDocNumber.Text;
        //    //inventoryIssue.Hold = false;

        //    List<ULTInventoryIssueDetail> issueDetails = new List<ULTInventoryIssueDetail>();
        //    foreach (DataRowView rowView in dv_filter)
        //    {
        //        //check if doc buying already synced
        //        if (docNbr != rowView[22].ToString())
        //        {
        //            docNbr = rowView[22].ToString();
        //            if (!checkDocumentSync(docNbr))
        //            {
        //                allSynced = false;
        //            }
        //        }
        //        ULTInventoryIssueDetail issueDetail = new ULTInventoryIssueDetail();
        //        issueDetail.InventoryID = rowView[1].ToString();
        //        issueDetail.Branch = docBranch;
        //        issueDetail.Location = AcumaticaCred.AcumaticaInvLocation;
        //        issueDetail.Subitem = rowView[2].ToString().Replace(".", "");
        //        issueDetail.Quantity = Convert.ToDecimal(rowView[19]);
        //        issueDetail.Description = rowView[3].ToString();
        //        issueDetail.Warehouse = tbWarehouse.Text;
        //        issueDetail.UOM = rowView[20].ToString();
        //        issueDetail.ReasonCode = "ISSUEDISPATCH";
        //        //issueDetail.LotSerialNbr = "";

        //        issueDetails.Add(issueDetail);
        //    }
        //    inventoryIssue.Details = issueDetails;

        //    if (!allSynced)
        //    {
        //        MessageBox.Show($"Documents for bale contained in this process is not synced to Acumatica!\nPlease sync required documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    else
        //    {
        //        if (!issueSynced)
        //        {
        //            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
        //            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
        //            try
        //            {
        //                var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
        //                var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);
        //                ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
        //                inventoryIssueApi.InvokeAction(releaseInventoryIssue);

        //                referenceNbr = responseIssue.ReferenceNbr.ToString();
        //                tbAcumaticaRefNbr.Text = referenceNbr;
        //            }
        //            catch (Exception ePut)
        //            {
        //                MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                syncError = true;
        //            }
        //            finally
        //            {
        //                checkReleasedIssue(referenceNbr, configuration);
        //                issueSynced = true;
        //                authApi.AuthLogout();

        //                if (!syncError)
        //                {
        //                    saveDocument();
        //                    //MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                    using (SqlConnection connection = new SqlConnection(ConnectionString))
        //                    {
        //                        try
        //                        {
        //                            if (connection.State != ConnectionState.Open)
        //                            {
        //                                connection.Open();
        //                            }

        //                            foreach (DataRowView rowView in dv_filter)
        //                            {
        //                                var rowDocNumber = rowView[0].ToString();
        //                                var rowLotNumber = rowView[3].ToString();

        //                                using (SqlCommand command = new SqlCommand("Update_DispatchOUTLineDetail_Sync", connection))
        //                                {
        //                                    command.CommandType = CommandType.StoredProcedure;
        //                                    command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
        //                                    command.Parameters.AddWithValue("@LotNbr", rowLotNumber);
        //                                    command.Parameters.AddWithValue("@SyncDetail", 1);

        //                                    command.ExecuteNonQuery();
        //                                }
        //                            }
        //                        }
        //                        catch (Exception e_update)
        //                        {
        //                            MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    //go to transfer screen
        //    if (!issueSynced)
        //    {
        //        MessageBox.Show($"Issue document for bale contained in this process is not synced to Acumatica!\nPlease sync required documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //    else
        //    {
        //        syncError = false;
        //        //transfer screen
        //        ULTTobaccoTransfer tobaccoTransfer = new ULTTobaccoTransfer();
        //        tobaccoTransfer.DispatchDate = inventoryIssue.Date;
        //        tobaccoTransfer.DispatchFrom = tbWarehouse.Text;
        //        tobaccoTransfer.DispatchTo = cbWarehouseTo.SelectedItem.ToString();
        //        tobaccoTransfer.AddOnNbr = DocNumber;
        //        tobaccoTransfer.DispatchNote = tbDispatchNote.Text;
        //        tobaccoTransfer.IssueRefNbr = tbAcumaticaRefNbr.Text;
        //        tobaccoTransfer.TotalCost = Convert.ToDecimal(tbTotalCost.Text.ToString().Replace(",", ""));
        //        tobaccoTransfer.TotalQty = Convert.ToDecimal(tbDetailWNetto.Text.ToString().Replace(",", ""));
        //        tobaccoTransfer.LogisticService = tbLogisticService.Text;
        //        tobaccoTransfer.LisencePlate = tbLisencePlate.Text;

        //        List<ULTTobaccoTransferDetail> transferDetails = new List<ULTTobaccoTransferDetail>();

        //        loadDetail();
        //        dv_filter = new DataView(dtDetail, $"SyncDetail = 1", "LotNbr Asc", DataViewRowState.CurrentRows);

        //        foreach (DataRowView rowView in dv_filter)
        //        {
        //            ULTTobaccoTransferDetail transferDetail = new ULTTobaccoTransferDetail();
        //            transferDetail.InventoryID = rowView[1].ToString();
        //            transferDetail.Subitem = rowView[2].ToString();
        //            transferDetail.LotNo = rowView[3].ToString();
        //            transferDetail.Source = rowView[4].ToString();
        //            transferDetail.Stage = rowView[5].ToString();
        //            transferDetail.Form = rowView[6].ToString();
        //            transferDetail.CropYear = rowView[7].ToString();
        //            transferDetail.Grade = rowView[8].ToString();
        //            transferDetail.Area = rowView[9].ToString();
        //            transferDetail.Color = rowView[10].ToString();
        //            transferDetail.Fermentation = rowView[11].ToString();
        //            transferDetail.Length = rowView[12].ToString();
        //            transferDetail.Process = rowView[13].ToString();
        //            transferDetail.StalkPositions = rowView[14].ToString();
        //            transferDetail.QtyRope = Convert.ToDecimal(rowView[15]);
        //            transferDetail.QtyShip = Convert.ToDecimal(rowView[16]);
        //            transferDetail.QtyGross = Convert.ToDecimal(rowView[17]);
        //            transferDetail.QtyTare = Convert.ToDecimal(rowView[18]);
        //            //transferDetail.QtyNetto = Convert.ToDecimal(rowView[19]);
        //            transferDetail.QtyNetto = Convert.ToDecimal(rowView[16]);
        //            transferDetail.UOM = rowView[20].ToString();
        //            transferDetail.UnitCost = Convert.ToDecimal(rowView[24]);
        //            transferDetail.ExtCost = Convert.ToDecimal(rowView[25]);
        //            transferDetail.BuyerName = rowView[26].ToString();

        //            transferDetails.Add(transferDetail);
        //        }
        //        tobaccoTransfer.Details = transferDetails;

        //        var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
        //        var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
        //        try
        //        {
        //            var tobaccoTransferApi = new ULTTobaccoTransferApi(configuration);
        //            var responseTransfer = tobaccoTransferApi.PutEntity(tobaccoTransfer);
        //        }
        //        catch (Exception ePut)
        //        {
        //            MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            syncError = true;
        //        }
        //        finally
        //        {
        //            authApi.AuthLogout();
        //            if (!syncError)
        //            {
        //                this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT Process [{DocNumber}]";
        //                tbStatus.Text = "SYNCED";

        //                saveDocument();
        //                MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                //Update Item Sync status
        //                using (SqlConnection connection = new SqlConnection(ConnectionString))
        //                {
        //                    try
        //                    {
        //                        if (connection.State != ConnectionState.Open)
        //                        {
        //                            connection.Open();
        //                        }
        //                    }
        //                    catch (Exception e_update)
        //                    {
        //                        MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                    }
        //                }

        //                resetEntry();
        //                saveDocument();
        //            }
        //        }
        //    } // end if else
        //}

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

        private void checkReleasedIssue(string referenceNbr, Configuration configuration)
        {
            var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
        /*    while (true)
            {*/
                var response = inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr }, expand: "Details");

                if (response.Status == "Released")
                {
                    var TotalCost = (decimal)response.TotalCost.Value;
                    tbTotalCost.Text = TotalCost.ToString("N2");

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
                                using (SqlCommand command = new SqlCommand("Update_DispatchOUTLineDetail_Cost", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                    command.Parameters.AddWithValue("@LotNbr", detail.Description.Value.ToString());
                                    command.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(detail.UnitCost.Value.ToString()));
                                    command.Parameters.AddWithValue("@ExtCost", Convert.ToDecimal(detail.ExtCost.Value.ToString()));

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                  /*  break;*/
                }
          /*  }*/
        }

        private bool checkDocumentSync(string docNbr)
        {
            var docStatus = "";
            //load buying registration
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
            tbEntryWeightReceive.Text = tbScale.Text;
        }

        private void tbEntryWeightReceive_TextChanged(object sender, EventArgs e)
        {
            var gross = Convert.ToDecimal(tbEntryWeightReceive.Text.Replace(",", ""));
            var tare = Convert.ToDecimal(tbEntryWeightTare.Text.Replace(",", ""));

            var netto = gross - tare;
            tbEntryWeightNetto.Text = netto.ToString("N2");
        }

        private void tbEntryWeightTare_TextChanged(object sender, EventArgs e)
        {
            var gross = Convert.ToDecimal(tbEntryWeightReceive.Text.Replace(",", ""));
            var tare = Convert.ToDecimal(tbEntryWeightTare.Text.Replace(",", ""));

            var netto = gross - tare;
            tbEntryWeightNetto.Text = netto.ToString("N2");
        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                var index = dgvDetail.SelectedRows[0].Index;
                loadEntryDetail(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString());

                groupEntry.Text = $"Lot Entry [{dgvDetail.Rows[index].Cells[3].FormattedValue.ToString()}]";
                dgvDetail.Rows[index].Selected = true;
                if (tbStatus.Text == "SYNCED" || tbStatus.Text == "OPEN")
                {
                    btnDelLot.Enabled = false;
                }
                else
                {
                    btnDelLot.Enabled = true;
                }
            }
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
                    Buyer = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[26].FormattedValue.ToString(),
                    InventoryID = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString(),
                    Process = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[13].FormattedValue.ToString(),
                    Stage = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].FormattedValue.ToString(),
                    Grade = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].FormattedValue.ToString(),
                    Color = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].FormattedValue.ToString(),
                    Weight = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].FormattedValue.ToString(),
                    Length = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[12].FormattedValue.ToString(),
                    Warehouse = tbWarehouse.Text,
                    Date = tbDisptachDate.Text,
                    Remark = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[21].FormattedValue.ToString(),
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

        private void btnPrintWL_Click(object sender, EventArgs e)
        {
            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        DispatchOUTLineDetail.DocumentID, 
	                                        DispatchOUTLineDetail.InventoryID, 
	                                        DispatchOUTLineDetail.SubItem, 
	                                        DispatchOUTLineDetail.LotNbr, 
	                                        SegmentValue.Descr AS Stage, 
	                                        DispatchOUTLineDetail.WeightNetto as OldNetto, 
	                                        DispatchOUTLineDetail.WeightReceive, 
	                                        DispatchOUTLineDetail.WeightTare, 
	                                        DispatchOUTLineDetail.WeightShipping as NewNetto, 
	                                        DispatchOUTLineDetail.UoM, 
	                                        DispatchOUTLineDetail.Remark, 
	                                        CONCAT(DispatchOUTLine.LogisticService,'/',DispatchOUTLine.LisencePlate) as Note
                                        FROM
	                                        dbo.DispatchOUTLineDetail
	                                        INNER JOIN
	                                        dbo.DispatchOUTLine
	                                        ON 
		                                        DispatchOUTLineDetail.DocumentID = DispatchOUTLine.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        DispatchOUTLineDetail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE DispatchOUTLineDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        DispatchOUTLineDetail.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDispatch.WeightListLineDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode($"{tbDocNumber.Text}", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            string QRImage = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);
            WeightListPrint dispatchOUTWLPrint = new WeightListPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "DISPATCH OUT",
                DocStatus = tbStatus.Text,
                DispatchDate = tbDisptachDate.Text,
                DispatchDetails = myDispatch,
                QRImage = QRImage,
                FromTo = "To " + GetBranch(cbWarehouseTo.Text, 1),
                LogisticService = tbLogisticService.Text,
                LisencePlate = tbLisencePlate.Text
            };
            dispatchOUTWLPrint.ShowDialog();


        }

        private void btnPrintSJ_Click(object sender, EventArgs e)
        {

            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        DispatchOUTLineDetail.InventoryID, 
	                                        InventoryItem.Descr,
	                                        COUNT(DispatchOUTLineDetail.LotNbr) AS PackQty, 
	                                        SUM(DispatchOUTLineDetail.WeightNetto) AS OldWeightNetto,
	                                        SUM(DispatchOUTLineDetail.WeightShipping) AS WeightNetto
                                        FROM
	                                        dbo.DispatchOUTLineDetail
	                                        INNER JOIN
	                                        dbo.InventoryItem
	                                        ON 
		                                        DispatchOUTLineDetail.InventoryID = InventoryItem.InventoryID
                                        WHERE DispatchOUTLineDetail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        DispatchOUTLineDetail.InventoryID,
	                                        InventoryItem.Descr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDispatch.SuratJalanDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            SuratJalanDocPrint suratJalanDocPrint = new SuratJalanDocPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Warehouse2 = GetBranch( cbWarehouseTo.SelectedItem.ToString(),1),
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Address2 = GetBranch(cbWarehouseTo.SelectedItem.ToString(), 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "DISPATCH OUT",
                DocDate = tbDisptachDate.Text,
                Notes = tbDispatchNote.Text,
                Logistic = tbLogisticService.Text,
                LisencePlate = tbLisencePlate.Text,
                DispatchDetails = myDispatch
            };
            suratJalanDocPrint.ShowDialog();
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

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            removeAllLot();
            saveDocument();
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

        private void dgvDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnPrintSUM_Click(object sender, EventArgs e)
        {
            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        DispatchOUTLineDetail.DocumentID, 
	                                        DispatchOUTLineDetail.InventoryID, 
	                                        DispatchOUTLineDetail.SubItem, 
	                                        SegmentValue.Descr AS Stage, 
                                            COUNT(DispatchOUTLineDetail.LotNbr) as WeightTare,
	                                        SUM(DispatchOUTLineDetail.WeightNetto) as OldNetto, 
	                                        SUM(DispatchOUTLineDetail.WeightReceive) as WeightReceive, 
	                                        SUM(DispatchOUTLineDetail.WeightShipping) as NewNetto, 
	                                        DispatchOUTLineDetail.UoM, 
	                                        CONCAT(DispatchOUTLine.LogisticService,'/',DispatchOUTLine.LisencePlate) as Note
                                        FROM
	                                        dbo.DispatchOUTLineDetail
	                                        INNER JOIN
	                                        dbo.DispatchOUTLine
	                                        ON 
		                                        DispatchOUTLineDetail.DocumentID = DispatchOUTLine.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        DispatchOUTLineDetail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE DispatchOUTLineDetail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        DispatchOUTLineDetail.DocumentID, 
	                                        DispatchOUTLineDetail.InventoryID, 
	                                        DispatchOUTLineDetail.SubItem,
                                            SegmentValue.Descr,
	                                        DispatchOUTLineDetail.UoM, 
	                                        CONCAT(DispatchOUTLine.LogisticService,'/',DispatchOUTLine.LisencePlate)
                                        ORDER BY
	                                        DispatchOUTLineDetail.DocumentID, 
	                                        DispatchOUTLineDetail.InventoryID, 
	                                        DispatchOUTLineDetail.SubItem";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDispatch.WeightListLineDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            WeightListSumPrint dispatchOUTWLSUMPrint = new WeightListSumPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "Summary DISPATCH OUT",
                DocStatus = tbStatus.Text,
                DispatchDate = tbDisptachDate.Text,
                DispatchDetails = myDispatch,
                FromTo = "To " + GetBranch(cbWarehouseTo.Text, 1),
                LogisticService = tbLogisticService.Text,
                LisencePlate = tbLisencePlate.Text

            };
            dispatchOUTWLSUMPrint.ShowDialog();

        }

        private void tbDispatchNote_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbWarehouseTo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbLogisticService_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbLisencePlate_KeyPress(object sender, KeyPressEventArgs e)
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

        private void unsyncing_Click(object sender, EventArgs e)
        {
            if (dtDetail.Rows.Count > 0)
            {
                loadDetail();
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 1", "LotNbr Asc", DataViewRowState.CurrentRows);

                if (true)
                {
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

                                using (SqlCommand command = new SqlCommand("Update_DispatchOUTLineDetail_Sync", connection))
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
                }
            }
            else
            {
                MessageBox.Show($"Bale not match with acumatica issue, try again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cbosheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDetailImport = dtImport[cbosheet.SelectedItem.ToString()];
            SaveImport.Enabled = true;
        }


        private async void SaveImport_Click(object sender, EventArgs e)
        {
            MapArray.Clear();
            foreach (DataRow row in dtDetailImport.Rows)
            {
                string LotNbrCheck = row["LotNbr"].ToString().Trim();

                //Console.WriteLine(LotNbrCheck);
                checklotnumber(LotNbrCheck);
            }
            Console.WriteLine(JsonSerializer.Serialize(MapArray));

            SqlTransaction objTrans = null;
            try
            {  
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        bool insertError = false;
                    try
                    {
                        connection.Close();
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                            objTrans = connection.BeginTransaction();
                        }

                        int indexku = 0;
                            
                            foreach (DataRow row in dtDetailImport.Rows)
                            {
                                
                                
                                
                                if (row["LotNbr"].ToString().Trim() != null)
                                    {
                                        string LotNbr = row["LotNbr"].ToString().Trim();
                                        string WeightNetto = row["WeightNetto"].ToString().Trim();



                                if (MapArray[indexku] == null)
                                {
                                    Console.WriteLine("ListArray is null"+ MapArray[indexku]);
                                    objTrans.Rollback();

                                    break;
                                }

                                using (SqlCommand command = new SqlCommand("Insert_DispatchOUTLineDetail", connection, objTrans))
                                    {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                    command.Parameters.AddWithValue("@InventoryID", MapArray[indexku][1]);
                                    command.Parameters.AddWithValue("@SubItem", MapArray[indexku][2]);
                                    command.Parameters.AddWithValue("@LotNbr", MapArray[indexku][3]);
                                    command.Parameters.AddWithValue("@Source", MapArray[indexku][4]);
                                    command.Parameters.AddWithValue("@Stage", MapArray[indexku][5]);
                                    command.Parameters.AddWithValue("@tForm", MapArray[indexku][6]);
                                    command.Parameters.AddWithValue("@CropYear", MapArray[indexku][7]);
                                    command.Parameters.AddWithValue("@Grade", MapArray[indexku][8]);
                                    command.Parameters.AddWithValue("@Area", MapArray[indexku][9]);
                                    command.Parameters.AddWithValue("@Color", MapArray[indexku][10]);
                                    command.Parameters.AddWithValue("@Fermentation", MapArray[indexku][11]);
                                    command.Parameters.AddWithValue("@Length", MapArray[indexku][12]);
                                    command.Parameters.AddWithValue("@Process", MapArray[indexku][13]);
                                    command.Parameters.AddWithValue("@StalkPosition", MapArray[indexku][14]);
                                    command.Parameters.AddWithValue("@WeightRope", MapArray[indexku][15]);
                                    //Console.WriteLine(LotNbr);
                                    //Console.WriteLine(JsonSerializer.Serialize(MapArray[indexku]));
                                    command.Parameters.AddWithValue("@WeightShipping", MapArray[indexku][19]);
                                    command.Parameters.AddWithValue("@WeightReceive", WeightNetto.Trim() == "" ? MapArray[indexku][19] : Convert.ToDecimal(WeightNetto));
                                    command.Parameters.AddWithValue("@WeightTare", 0);
                                    command.Parameters.AddWithValue("@WeightNetto", WeightNetto.Trim() == "" ? MapArray[indexku][19] : Convert.ToDecimal(WeightNetto));
                                    command.Parameters.AddWithValue("@UoM", MapArray[indexku][20]);
                                    command.Parameters.AddWithValue("@Remark", "Import DispetOut");
                                    command.Parameters.AddWithValue("@OldDocumentID", MapArray[indexku][0]);
                                    command.Parameters.AddWithValue("@SyncDetail", 0);
                                    command.Parameters.AddWithValue("@UnitCost", 0);
                                    command.Parameters.AddWithValue("@ExtCost", 0);
                                    command.Parameters.AddWithValue("@BuyerName", MapArray[indexku][22]);
                                    command.ExecuteNonQuery();
                                }

                            }
                                else{
                                            MessageBox.Show($"Check Imput Excel !", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                            indexku++;
                            }
                         
                            objTrans.Commit();
                            loadProcess();
                            loadDetail();
                            resetEntry();
                            cbosheet.Text = "";
                           
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
                            
                            dtDetailImport.Clear();
                            cbosheet.Items.Clear();
                            textFilename.Clear();
                            MapArray.Clear();
                            dtImport.Clear();
                        
                            if (connection.State == ConnectionState.Open)
                                {
                                     connection.Close();
                                }
                        }
                    }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"--{ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

         private void checklotnumber(string lot) {
            //var a = 0;
            string query = $"SELECT *  FROM StockItem  WHERE LotNbr = '{lot}' AND StatusStock = 1";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    List<dynamic> ListArray = new List<dynamic>();
                    reader.Read();
                        ListArray.Add(reader[0]);
                        ListArray.Add(reader[1]);
                        ListArray.Add(reader[2]);
                        ListArray.Add(reader[3]);
                        ListArray.Add(reader[4]);
                        ListArray.Add(reader[5]);
                        ListArray.Add(reader[6]);
                        ListArray.Add(reader[7]);
                        ListArray.Add(reader[8]);
                        ListArray.Add(reader[9]);
                        ListArray.Add(reader[10]);
                        ListArray.Add(reader[11]);
                        ListArray.Add(reader[12]);
                        ListArray.Add(reader[13]);
                        ListArray.Add(reader[14]);
                        ListArray.Add(reader[15]);
                        ListArray.Add(reader[16]);
                        ListArray.Add(reader[17]);
                        ListArray.Add(reader[18]);
                        ListArray.Add(reader[19]);
                        ListArray.Add(reader[20]);
                        ListArray.Add(reader[21]);
                        ListArray.Add(reader[22]);
                        ListArray.Add(reader[23]);
                        ListArray.Add(reader[24]);
                        ListArray.Add(reader[25]);
                        ListArray.Add(reader[26]);
                    MapArray.Add(ListArray);
                    //ListArray.Clear();
                    //Console.WriteLine(lot);

                }
                catch (Exception e){
                    connection.Close();
                }
                finally
                {
                   
                    //Console.WriteLine(JsonSerializer.Serialize(MapArray));
                   
                    connection.Close();
                }
            }
        }

      
        //end of file
    }
}