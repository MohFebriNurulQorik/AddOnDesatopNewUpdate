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

namespace ScaleAddon.Forms
{
    public partial class DispatchINProcess : Form
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
        private int transferBale = 0;

        private DataTable dtDetail;
        private DataTable dtLot;
        private DataTable dtEntry;

        private SerialPort port;
        
        private DataTableCollection dtImport;
        private DataTable dtDetailImport;

        public DispatchINProcess()
        {
            InitializeComponent();
        }

        private void DispatchINProcess_Load(object sender, EventArgs e)
        {
            if (Userlog.UserRoles.Contains("DIS-IMPORT") || Userlog.UserRoles.Contains("SUPERVISOR"))
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
            
            if (Userlog.UserRoles.Contains("LOAD-RECALL") || Userlog.UserRoles.Contains("SUPERVISOR"))
            {
                ReceiptAll.Visible = true;
            }
            else
            {
                ReceiptAll.Visible = false;
            }

            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
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

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbTotalCost.Text = "0";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbWarehouseFrom.Text = "";
            tbAcumaticaRefNbr.Text = "";
            tbDispatchNote.Text = "";
            tbAddonNbr.Text = "";

            tbBaleReceived.Text = "0";

            resetEntry();
        }

        private void resetEntry()
        {
            loadDetail();
            loadComboLot();

            groupEntry.Text = "Lot Entry";

            tbEntryLot.Text = "";
            tbEntryInv.Text = "";
            tbEntryWeightShipping.Text = "0";
            tbEntryGrade.Text = "";
            tbEntrySubItem.Text = "";
            tbEntryWeightReceive.Text = "0";
            tbEntryWeightTare.Text = "0";
            tbEntryWeightNetto.Text = "0";

            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = false;
                    btnAcumaticaReceipt.Enabled = true;
                    btnSave.Enabled = true;
                    
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    if (tbAddonNbr.Text != "") { btnPrintWL.Enabled = true; btnPrintSUM.Enabled = true; } else { btnPrintWL.Enabled = false; btnPrintSUM.Enabled = false; };
                    btnPrintSJ.Enabled = false;
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
                    btnAcumaticaReceipt.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintWL.Enabled = true;
                    btnPrintSUM.Enabled = true;
                    btnPrintSJ.Enabled = true;
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
                    btnAcumatica.Enabled = true;
                    btnAcumaticaReceipt.Enabled = false;
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintWL.Enabled = false;
                    btnPrintSUM.Enabled = false;
                    btnPrintSJ.Enabled = false;
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

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from DispatchINLine where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(4).ToString();
                        tbTotalCost.Text = Convert.ToDecimal(reader.GetValue(5)).ToString("N2");
                        tbWarehouseFrom.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(7).ToString();
                        tbDispatchNote.Text = reader.GetValue(8).ToString();
                        tbWarehouse.Text = reader.GetValue(3).ToString();
                        tbAddonNbr.Text = reader.GetValue(10).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(4).ToString() == "HOLD" ? 1 : 0);
                        tbLogisticService.Text = reader.GetValue(14).ToString();
                        tbLisencePlate.Text = reader.GetValue(15).ToString();
                        loadComboLot();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            resetEntry();
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
	                                    *
                                    FROM
	                                    dbo.DispatchINLineData
                                        
                                    WHERE
	                                    DispatchINLineData.SyncDetail = 0
                                    AND DocumentID = '{tbAddonNbr.Text}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtLot);
                    string[] arrray = dtLot.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbLot);
                    cbLot.Items.Clear();
                    cbLot.Items.AddRange(arrray);

                    tbBaleReceived.Text = dtLot.Rows.Count.ToString(); 
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
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        DispatchINLineDetail
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
                    dgvDetail.Columns[23].Visible = false;
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
                        decimal sumValidated = Convert.ToDecimal(dtDetail.Compute("COUNT(WeightShipping)", string.Empty));
                        tbBaleValidated.Text = sumValidated.ToString();
                        decimal sumWShipping = Convert.ToDecimal(dtDetail.Compute("SUM(WeightShipping)", string.Empty));
                        tbDetailWShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = Convert.ToDecimal(dtDetail.Compute("SUM(WeightReceive)", string.Empty));
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = Convert.ToDecimal(dtDetail.Compute("SUM(WeightTare)", string.Empty));
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty));
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                        tbAddonNbr.ReadOnly = true;
                        tbAddonNbr.BackColor = System.Drawing.SystemColors.Info;
                        btnAcumatica.Enabled = false;
                        btnAcumaticaReceipt.Enabled = true;
                    }
                    else
                    {
                        decimal sumValidated = 0;
                        tbBaleValidated.Text = sumValidated.ToString();
                        decimal sumWShipping = 0;
                        tbDetailWShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = 0;
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = 0;
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = 0;
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                        tbAddonNbr.ReadOnly = false;
                        tbAddonNbr.BackColor = System.Drawing.SystemColors.Window;
                        btnAcumatica.Enabled = true;
                        btnAcumaticaReceipt.Enabled = false;
                    }
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
                //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-IO/IN-{currentIncrement.ToString().PadLeft(4, '0')}";
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-IO/IN-{currentIncrement.ToString().PadLeft(4, '0')}";
                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-IO/IN-{currentIncrement.ToString().PadLeft(4, '0')}";

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
                    string query = $"IF EXISTS ( SELECT * FROM DispatchINLine WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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
                    string query = $"select * from NumberingSetting where NumberingID = 'IO/IN'";
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

                        using (SqlCommand command = new SqlCommand("Insert_DispatchINLine_v2", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbDisptachDate.Text);
                            command.Parameters.AddWithValue("@WarehouseIDFrom", tbWarehouseFrom.Text);
                            command.Parameters.AddWithValue("@WarehouseIDTo", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@TotalCost", tbTotalCost.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalWeight", tbDetailWNetto.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                            command.Parameters.AddWithValue("@Note", tbDispatchNote.Text);
                            command.Parameters.AddWithValue("@DispatchOUTNbr", tbAddonNbr.Text);
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
                    //        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

                    //        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                    //        {
                    //            command.CommandType = CommandType.StoredProcedure;
                    //            command.Parameters.AddWithValue("@NumberingID", $"IO/IN");
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
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"IO/IN");
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



            if (tbStatus.Text == "OPEN" && tbAddonNbr.Text != "" && !checkHold.Checked)
            {
                //btnAcumatica.Enabled = true;
                btnAcumaticaReceipt.Enabled = true;
            }
            else
            {
                //btnAcumatica.Enabled = false;
                btnAcumaticaReceipt.Enabled = false;
            }

            if ((tbStatus.Text == "OPEN" && tbAddonNbr.Text != "") || tbStatus.Text == "SYNCED")
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                try
                {
                    string query = $@"SELECT
                                        TOP 1
	                                    DispatchOUTNbr,
                                        *
                                    FROM
	                                    DispatchINLine
                                    WHERE
	                                    DispatchOUTNbr = '{tbAddonNbr.Text}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim().Length > 5)
                        {
                            tbWarehouseFrom.Text = reader.GetValue(3).ToString();
                            tbWarehouse.Text = reader.GetValue(4).ToString();
                            tbLogisticService.Text = reader.GetValue(15).ToString();
                            tbLisencePlate.Text = reader.GetValue(16).ToString();
                            
                            saveDocument();
                            loadDetail();
                            resetEntry();

                        }
                    }
                    else
                    {
                        acumaticaSync();
                        
                    }
                }
                catch (Exception f)
                {

                }
            }

            
            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetScreen();
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

        //private void acumaticaSync()
        //{
        //    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}] - Syncing with Acumatica, please wait!";
        //    bool syncError = false;
        //    bool ReceiptSynced = false;
        //    string docNbr = "";
        //    string referenceNbr = "";
        //    string addonNbr = tbAddonNbr.Text;
        //    ULTTobaccoTransfer tobaccoTransfer;

        //    //get transfer DATA
        //    var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
        //    var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
        //    try
        //    {
        //        var tobaccoTransferApi = new ULTTobaccoTransferApi(configuration);
        //        var responseTransfers = tobaccoTransferApi.GetList(expand: "Details", filter: $"addonNbr eq '{addonNbr}'");

        //        if (responseTransfers.Count > 0)
        //        {
        //            tobaccoTransfer = responseTransfers[0];

        //            if (tobaccoTransfer.ReceiptRefNbr == "" || tobaccoTransfer.ReceiptRefNbr == null)
        //            {
        //                MessageBox.Show($"Transfer {addonNbr} is not available.\nTransfer already received based on Receipt {tobaccoTransfer.ReceiptRefNbr}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            }
        //            else
        //            {
        //                if (tobaccoTransfer.DispatchTo == tbWarehouse.Text)
        //                {
        //                    tbWarehouseFrom.Text = tobaccoTransfer.DispatchFrom.Value;
        //                    tbTotalCost.Text = Convert.ToDecimal(tobaccoTransfer.TotalCost.Value).ToString("N2");
        //                    tbLisencePlate.Text = tobaccoTransfer.LisencePlate.Value;
        //                    tbLogisticService.Text = tobaccoTransfer.LogisticService.Value;
        //                    //transferBale = tobaccoTransfer.Details.Count;
        //                    tbBaleReceived.Text = tobaccoTransfer.Details.Count.ToString();
        //                    saveLot2Data(tobaccoTransfer.Details);
        //                }
        //                else
        //                {
        //                    MessageBox.Show($"Transfer {addonNbr} is not available for this warehouse!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show($"Transfer {addonNbr} is not available", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        }
        //    }
        //    catch (Exception eGet)
        //    {
        //        MessageBox.Show($"--Sync error! {eGet.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        syncError = true;
        //    }
        //    finally
        //    {
        //        authApi.AuthLogout();
        //        saveDocument();
        //        if (!syncError)
        //        {
        //            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

        //            //saveDocument();
        //            MessageBox.Show($"--Sync data Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //            loadComboLot();
        //        }
        //    }
        //}

        private void acumaticaSync()
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            bool ReceiptSynced = false;
            string docNbr = "";
            string referenceNbr = "";
            string addonNbr = tbAddonNbr.Text;
            ULTTobaccoTransfer tobaccoTransfer;

            //get transfer DATA
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
            try
            {
                var tobaccoTransferApi = new ULTTobaccoTransferApi(configuration);
                var responseTransfers = tobaccoTransferApi.GetList(expand: "Details", filter: $"addonNbr eq '{addonNbr}'");

                if (responseTransfers.Count > 0)
                {
                    tobaccoTransfer = responseTransfers[0];

                    if (tobaccoTransfer.ReceiptRefNbr == "" || tobaccoTransfer.ReceiptRefNbr == null)
                    {
                        MessageBox.Show($"Transfer {addonNbr} is not available.\nTransfer already received based on Receipt {tobaccoTransfer.ReceiptRefNbr}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (tobaccoTransfer.DispatchTo == tbWarehouse.Text)
                        {
                            tbWarehouseFrom.Text = tobaccoTransfer.DispatchFrom.Value;
                            tbTotalCost.Text = Convert.ToDecimal(tobaccoTransfer.TotalCost.Value).ToString("N2");
                            tbLisencePlate.Text = tobaccoTransfer.LisencePlate.Value;
                            tbLogisticService.Text = tobaccoTransfer.LogisticService.Value;
                            //transferBale = tobaccoTransfer.Details.Count;
                            tbBaleReceived.Text = tobaccoTransfer.Details.Count.ToString();
                            
                            saveLot2Data(tobaccoTransfer.Details);

                            saveDocument();
                            loadDetail();
                            resetEntry();
                            if (ReceiptAll.Checked == true && tbAcumaticaRefNbr.Text.Trim() == "")
                            {
                                saveLot2DataToDetail(tobaccoTransfer.Details);
                            }

                        
                           
                        }
                        else
                        {
                            MessageBox.Show($"Transfer {addonNbr} is not available for this warehouse!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Transfer {addonNbr} is not available", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception eGet)
            {
                MessageBox.Show($"--Sync error! {eGet.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                syncError = true;
            }
            finally
            {
                authApi.AuthLogout();
                if (!syncError)
                {
                    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";

                    //saveDocument();


                    loadComboLot();
                }
            }
        }

        private void saveLot2Data(List<ULTTobaccoTransferDetail> transferDetails)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    foreach (ULTTobaccoTransferDetail transferDetail in transferDetails)
                    {
                        using (SqlCommand command = new SqlCommand("Insert_DispatchINLineData", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbAddonNbr.Text);
                            command.Parameters.AddWithValue("@InventoryID", transferDetail.InventoryID.Value);
                            command.Parameters.AddWithValue("@SubItem", transferDetail.Subitem.Value);
                            command.Parameters.AddWithValue("@LotNbr", transferDetail.LotNo.Value);
                            command.Parameters.AddWithValue("@Source", transferDetail.Source.Value);
                            command.Parameters.AddWithValue("@Stage", transferDetail.Stage.Value);
                            command.Parameters.AddWithValue("@tForm", transferDetail.Form.Value);
                            command.Parameters.AddWithValue("@CropYear", transferDetail.CropYear.Value);
                            command.Parameters.AddWithValue("@Grade", transferDetail.Grade.Value);
                            command.Parameters.AddWithValue("@Area", transferDetail.Area.Value);
                            command.Parameters.AddWithValue("@Color", transferDetail.Color.Value);
                            command.Parameters.AddWithValue("@Fermentation", transferDetail.Fermentation.Value);
                            command.Parameters.AddWithValue("@Length", transferDetail.Length.Value);
                            command.Parameters.AddWithValue("@Process", transferDetail.Process.Value);
                            command.Parameters.AddWithValue("@StalkPosition", transferDetail.StalkPositions.Value);
                            command.Parameters.AddWithValue("@WeightRope", transferDetail.QtyRope.Value);
                            command.Parameters.AddWithValue("@WeightShipping", transferDetail.QtyShip.Value);
                            command.Parameters.AddWithValue("@WeightReceive", transferDetail.QtyGross.Value);
                            command.Parameters.AddWithValue("@WeightTare", transferDetail.QtyTare.Value);
                            //command.Parameters.AddWithValue("@WeightNetto", transferDetail.QtyNetto.Value);
                            command.Parameters.AddWithValue("@WeightNetto", transferDetail.QtyShip.Value);
                            command.Parameters.AddWithValue("@UoM", transferDetail.UOM.Value);
                            command.Parameters.AddWithValue("@Remark", "");
                            command.Parameters.AddWithValue("@SyncDetail", 0);
                            command.Parameters.AddWithValue("@UnitCost", transferDetail.UnitCost.Value);
                            command.Parameters.AddWithValue("@ExtCost", transferDetail.ExtCost.Value);
                            command.Parameters.AddWithValue("@BuyerName", transferDetail.BuyerName.Value);

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
                    connection.Close();
                    loadDetail();
                }
            }
        }

        private void saveLot2DataToDetail(List<ULTTobaccoTransferDetail> transferDetails)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    foreach (ULTTobaccoTransferDetail transferDetail in transferDetails)
                    {
                        using (SqlCommand command = new SqlCommand("Insert_DispatchINLineDetail", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", DocNumber);
                            command.Parameters.AddWithValue("@InventoryID", transferDetail.InventoryID.Value);
                            command.Parameters.AddWithValue("@SubItem", transferDetail.Subitem.Value);
                            command.Parameters.AddWithValue("@LotNbr", transferDetail.LotNo.Value);
                            command.Parameters.AddWithValue("@Source", transferDetail.Source.Value);
                            command.Parameters.AddWithValue("@Stage", transferDetail.Stage.Value);
                            command.Parameters.AddWithValue("@tForm", transferDetail.Form.Value);
                            command.Parameters.AddWithValue("@CropYear", transferDetail.CropYear.Value);
                            command.Parameters.AddWithValue("@Grade", transferDetail.Grade.Value);
                            command.Parameters.AddWithValue("@Area", transferDetail.Area.Value);
                            command.Parameters.AddWithValue("@Color", transferDetail.Color.Value);
                            command.Parameters.AddWithValue("@Fermentation", transferDetail.Fermentation.Value);
                            command.Parameters.AddWithValue("@Length", transferDetail.Length.Value);
                            command.Parameters.AddWithValue("@Process", transferDetail.Process.Value);
                            command.Parameters.AddWithValue("@StalkPosition", transferDetail.StalkPositions.Value);
                            command.Parameters.AddWithValue("@WeightRope", transferDetail.QtyRope.Value);
                            command.Parameters.AddWithValue("@WeightShipping", transferDetail.QtyShip.Value);
                            command.Parameters.AddWithValue("@WeightReceive", transferDetail.QtyGross.Value);
                            command.Parameters.AddWithValue("@WeightTare", transferDetail.QtyTare.Value);
                            command.Parameters.AddWithValue("@WeightNetto", transferDetail.QtyNetto.Value);
                            command.Parameters.AddWithValue("@UoM", transferDetail.UOM.Value);
                            command.Parameters.AddWithValue("@Remark", "auto load all");
                            command.Parameters.AddWithValue("@SyncDetail", 0);
                            command.Parameters.AddWithValue("@UnitCost", transferDetail.UnitCost.Value);
                            command.Parameters.AddWithValue("@ExtCost", transferDetail.ExtCost.Value);
                            command.Parameters.AddWithValue("@BuyerName", transferDetail.BuyerName.Value);
                            command.Parameters.AddWithValue("@OldDocumentID", tbAddonNbr.Text);
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
                    connection.Close();
                    loadDetail();
                }
            }
        }

        private void saveLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Insert_DispatchINLineDetail", connection))
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
                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(tbEntryWeightReceive.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(tbEntryWeightTare.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@UoM", dtEntry.Rows[0].ItemArray[20].ToString());
                        command.Parameters.AddWithValue("@Remark", dtEntry.Rows[0].ItemArray[21].ToString());
                        command.Parameters.AddWithValue("@OldDocumentID", dtEntry.Rows[0].ItemArray[0].ToString());
                        command.Parameters.AddWithValue("@SyncDetail", 0);
                        command.Parameters.AddWithValue("@UnitCost", Convert.ToDecimal(dtEntry.Rows[0].ItemArray[24].ToString().Replace(",", "")) / Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@ExtCost", Convert.ToDecimal(dtEntry.Rows[0].ItemArray[24].ToString().Replace(",", "")));
                        command.Parameters.AddWithValue("@BuyerName", dtEntry.Rows[0].ItemArray[25].ToString());

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
                    groupEntry.BackgroundImage = Properties.Resources.editMode;
                    cbLot.SelectedIndex = -1;
                    loadComboLot();
                }
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

                    using (SqlCommand command = new SqlCommand("Delete_DispatchINLineDetail", connection))
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

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
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

        private void cbLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLot.SelectedIndex >= 0)
            {
                loadEntry(cbLot.SelectedItem.ToString());
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

                    string query = $@"SELECT
	                                        DocumentID,
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
	                                        SyncDetail,
	                                        UnitCost,
	                                        ExtCost,
	                                        BuyerName
                                        FROM
	                                        DispatchINLineData
                                        WHERE
	                                        LotNbr = '{LotNbr}' AND
                                         DocumentID = '{tbAddonNbr.Text}'

                                        ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);

                    groupEntry.Text = $"Lot Entry [{dtEntry.Rows[0].ItemArray[3].ToString()}]";
                    tbEntryLot.Text = dtEntry.Rows[0].ItemArray[3].ToString();
                    tbEntryInv.Text = dtEntry.Rows[0].ItemArray[1].ToString();
                    tbEntrySubItem.Text = dtEntry.Rows[0].ItemArray[2].ToString();
                    tbEntryGrade.Text = dtEntry.Rows[0].ItemArray[8].ToString();
                    tbEntryWeightShipping.Text = dtEntry.Rows[0].ItemArray[16].ToString();
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
            Console.WriteLine(LotNbr);
            resetEntry();

            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtEntry = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT
	                                        OldDocumentID,
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
	                                        SyncDetail,
	                                        UnitCost,
	                                        ExtCost,
	                                        BuyerName
                                        FROM
	                                        DispatchINLineDetail
                                        WHERE
	                                        DocumentID = '{DocNumber}'
                                        AND
                                            LotNbr = '{LotNbr}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);

                    groupEntry.Text = $"Lot Entry [{dtEntry.Rows[0].ItemArray[3].ToString()}]";
                    tbEntryLot.Text = dtEntry.Rows[0].ItemArray[3].ToString();
                    tbEntryInv.Text = dtEntry.Rows[0].ItemArray[1].ToString();
                    tbEntrySubItem.Text = dtEntry.Rows[0].ItemArray[2].ToString();
                    tbEntryGrade.Text = dtEntry.Rows[0].ItemArray[8].ToString();
                    tbEntryWeightShipping.Text = dtEntry.Rows[0].ItemArray[16].ToString();
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
                saveDocument();
                tbLot.Focus();
            }
        }

        private void btnDelLot_Click(object sender, EventArgs e)
        {
            removeLot();
            saveDocument();
        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                var lotNbr = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();

                Console.WriteLine(lotNbr);
                if (checkLotNbrInSTock(lotNbr))
                {
                    var index = dgvDetail.SelectedRows[0].Index;
                    
                    loadEntryDetail(lotNbr);

                    groupEntry.Text = $"Lot Entry [{dgvDetail.Rows[index].Cells[3].FormattedValue.ToString()}]";
                    dgvDetail.Rows[index].Selected = true;

                    if (tbStatus.Text == "SYNCED")
                    {
                        btnDelLot.Enabled = false;
                    }
                    else
                    {
                        btnDelLot.Enabled = true;
                    }

                }
                else
                {
                    MessageBox.Show("Lot already used in other process, cannot edit lot", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }
        }

        private void btnAcumaticaReceipt_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show($"There are {transferBale} unverified lot, continue sync?", "Sync Receipt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                AcumaticaSyncReceipt();
            }
            else
            {
                // Do something
            }
        }

        private void AcumaticaSyncReceipt()
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            bool ReceiptSynced = false;
            string docNbr = "";
            string referenceNbr = "";
            string addonNbr = tbAddonNbr.Text;
            var docBranch = GetBranch(tbWarehouse.Text,2);
            ULTTobaccoTransfer tobaccoTransfer;

            //get transfer DATA
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
            //receipt
            if (dtDetail.Rows.Count > 0)
            {
                loadDetail();
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

                //issue
                InventoryReceipt inventoryReceipt = new InventoryReceipt();
                inventoryReceipt.Date = Convert.ToDateTime(tbDisptachDate.Text);
                inventoryReceipt.Branch = docBranch;
                inventoryReceipt.Description = $"{tbWarehouse.Text} Dispatch IN Transaction Receipt";
                inventoryReceipt.ExternalRef = tbDocNumber.Text;
                inventoryReceipt.Hold = false;

                List<InventoryReceiptDetail> receiptDetails = new List<InventoryReceiptDetail>();

                foreach (DataRowView rowView in dv_filter)
                {
                    InventoryReceiptDetail receiptDetail = new InventoryReceiptDetail();
                    receiptDetail.InventoryID = rowView[1].ToString();
                    receiptDetail.Branch = docBranch;
                    receiptDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                    receiptDetail.Subitem = rowView[2].ToString().Replace(".", "");
                    receiptDetail.Qty = Convert.ToDecimal(rowView[19]);
                    receiptDetail.Description = rowView[3].ToString();
                    receiptDetail.WarehouseID = tbWarehouse.Text;
                    receiptDetail.UOM = rowView[20].ToString();
                    receiptDetail.ReasonCode = "RECEIPTDISPATCH";

                    receiptDetail.UnitCost = Convert.ToDecimal(tbTotalCost.Text.Replace(",", "")) / Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));

                    //if (rowView[24].ToString() == "0")
                    //{
                    //    receiptDetail.UnitCost = Convert.ToDecimal(tbTotalCost.Text.Replace(",","")) / Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));
                    //}
                    //else
                    //{
                    //    receiptDetail.UnitCost = 0;
                    //}

                    //issueDetail.LotSerialNbr = "";

                    receiptDetails.Add(receiptDetail);
                }
                inventoryReceipt.Details = receiptDetails;

                syncError = false;
                configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                try
                {
                    var inventoryReceiptApi = new InventoryReceiptApi(configuration);
                    var responseReceipt = inventoryReceiptApi.PutEntity(inventoryReceipt);
                    ReleaseInventoryReceipt releaseInventoryReceipt = new ReleaseInventoryReceipt((InventoryReceipt)responseReceipt);
                    inventoryReceiptApi.InvokeAction(releaseInventoryReceipt);

                    referenceNbr = responseReceipt.ReferenceNbr.ToString();
                    tbAcumaticaRefNbr.Text = referenceNbr;
                    //AcumaticaRefNbr = referenceNbr;
                }
                catch (Exception ePut)
                {
                    MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    syncError = true;
                }
                finally
                {
                    //authApi.AuthLogout();
                    if (!syncError)
                    {
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch IN Process [{DocNumber}]";
                        tbStatus.Text = "SYNCED";

                        saveDocument();
                        MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        //update tobacco transfer
                        ULTTobaccoTransfer tobaccoTransferUpdate = new ULTTobaccoTransfer();
                        tobaccoTransferUpdate.AddOnNbr = tbAddonNbr.Text;
                        tobaccoTransferUpdate.ReceiptRefNbr = tbAcumaticaRefNbr.Text;

                        try
                        {
                            var tobaccoTransferApi = new ULTTobaccoTransferApi(configuration);
                            var responseTransfer = tobaccoTransferApi.PutEntity(tobaccoTransferUpdate);
                        }
                        catch (Exception ePut)
                        {
                            MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            syncError = true;
                        }

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

                                    using (SqlCommand command = new SqlCommand("Update_ProcessingLineINDetail_Sync", connection))
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
                    authApi.AuthLogout();
                }//end try catch finally
            }
            else
            {
                MessageBox.Show($"Bale not match with acumatica issue, try again", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbBaleReceived_TextChanged(object sender, EventArgs e)
        {
            transferBale = Convert.ToInt32(((TextBox)sender).Text);
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
	                                        DispatchINLineDetail.DocumentID, 
	                                        DispatchINLineDetail.InventoryID, 
	                                        DispatchINLineDetail.SubItem, 
	                                        DispatchINLineDetail.LotNbr, 
	                                        SegmentValue.Descr AS Stage, 
	                                        DispatchINLineDetail.WeightShipping as OldNetto, 
	                                        DispatchINLineDetail.WeightReceive, 
	                                        DispatchINLineDetail.WeightTare, 
	                                        DispatchINLineDetail.WeightNetto as NewNetto, 
	                                        DispatchINLineDetail.UoM, 
	                                        DispatchINLineDetail.Remark, 
	                                        ' ' as Note
                                        FROM
	                                        dbo.DispatchINLineDetail
	                                        INNER JOIN
	                                        dbo.DispatchINLine
	                                        ON 
		                                        DispatchINLineDetail.DocumentID = DispatchINLine.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        DispatchINLineDetail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE DispatchINLineDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        DispatchINLineDetail.LotNbr";

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
                DocType = "DISPATCH IN",
                DocStatus = tbStatus.Text,
                DispatchDate = tbDisptachDate.Text,
                DispatchDetails = myDispatch,
                QRImage = QRImage,
                FromTo = "From " + GetBranch(tbWarehouseFrom.Text, 1),
                LogisticService = tbLogisticService.Text,
                LisencePlate = tbLisencePlate.Text
            };
            dispatchOUTWLPrint.ShowDialog();
        }

        private void tbLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
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
                    MessageBox.Show($"Lot number not available !");
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

        private void btnPrintSJ_Click(object sender, EventArgs e)
        {

            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        DispatchINLineDetail.InventoryID, 
	                                        InventoryItem.Descr,
	                                        COUNT(DispatchINLineDetail.LotNbr) AS PackQty, 
	                                        SUM(DispatchINLineDetail.WeightShipping) AS OldWeightNetto,
	                                        SUM(DispatchINLineDetail.WeightNetto) AS WeightNetto
                                        FROM
	                                        dbo.DispatchINLineDetail
	                                        INNER JOIN
	                                        dbo.InventoryItem
	                                        ON 
		                                        DispatchINLineDetail.InventoryID = InventoryItem.InventoryID
                                        WHERE DispatchINLineDetail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        DispatchINLineDetail.InventoryID,
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
                Warehouse2 = GetBranch(tbWarehouseFrom.Text, 1),
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Address2 = GetBranch(tbWarehouseFrom.Text, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "DISPATCH IN",
                DocDate = tbDisptachDate.Text,
                Notes = tbDispatchNote.Text,
                Logistic = tbLogisticService.Text,
                LisencePlate = tbLisencePlate.Text,
                DispatchDetails = myDispatch
            };
            suratJalanDocPrint.ShowDialog();
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

        private void btnPrintSUM_Click(object sender, EventArgs e)
        {
            DataSetAddon myDispatch = new DataSetAddon();
         
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $@"SELECT
	                                        DispatchInLineDetail.DocumentID, 
	                                        DispatchInLineDetail.InventoryID, 
	                                        DispatchInLineDetail.SubItem, 
	                                        SegmentValue.Descr AS Stage, 
                                            COUNT(DispatchInLineDetail.LotNbr) as WeightTare,
	                                        SUM(DispatchInLineDetail.WeightNetto) as NewNetto, 
	                                        SUM(DispatchInLineDetail.WeightReceive) as WeightReceive, 
	                                        SUM(DispatchInLineDetail.WeightTare) as WeightTare1, 
	                                        SUM(DispatchInLineDetail.WeightShipping) as OldNetto, 
	                                        DispatchInLineDetail.UoM, 
	                                        CONCAT(DispatchInLine.LogisticService,'/',DispatchInLine.LisencePlate) as Note
                                        FROM
	                                        dbo.DispatchInLineDetail
	                                        INNER JOIN
	                                        dbo.DispatchInLine
	                                        ON 
		                                        DispatchInLineDetail.DocumentID = DispatchInLine.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        DispatchInLineDetail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE DispatchInLineDetail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        DispatchInLineDetail.DocumentID, 
	                                        DispatchInLineDetail.InventoryID, 
	                                        DispatchInLineDetail.SubItem,
                                            SegmentValue.Descr,
	                                        DispatchInLineDetail.UoM, 
	                                        CONCAT(DispatchInLine.LogisticService,'/',DispatchInLine.LisencePlate)
                                        ORDER BY
	                                        DispatchInLineDetail.DocumentID, 
	                                        DispatchInLineDetail.InventoryID, 
	                                        DispatchInLineDetail.SubItem";

                    SqlCommand command = new SqlCommand(query, connection);
                    
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    
                    da.Fill(myDispatch.WeightListLineDetail);
                    

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
             
                WeightListSumPrint dispatchOUTWLSUMPrint = new WeightListSumPrint
                {
                    Company = Warehouse.Company,
                    Warehouse = Warehouse.Descr,
                    Address = GetBranch(Warehouse.WarehouseID, 3),
                    Phone = GetBranch(Warehouse.WarehouseID, 4),
                    DocNumber = tbDocNumber.Text,
                    DocType = "Sumary DISPATCH IN",
                    DocStatus = tbStatus.Text,
                    DispatchDate = tbDisptachDate.Text,
                    DispatchDetails = myDispatch,
                    FromTo = "From " + GetBranch(tbWarehouseFrom.Text, 1),
                    LogisticService = tbLogisticService.Text,
                    LisencePlate = tbLisencePlate.Text

                };
                dispatchOUTWLSUMPrint.ShowDialog();
            }
            //end of file
        }

        private void tbAddonNbr_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnScale_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbEntryWeightTare_KeyPress(object sender, KeyPressEventArgs e)
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

        private void ChangeDispetOot(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                try
                {
                    string query = $@"SELECT
                                        TOP 1
	                                    DispatchOUTNbr,
                                        *
                                    FROM
	                                    DispatchINLine
                                    WHERE
	                                    DispatchOUTNbr = '{tbAddonNbr.Text}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        if (reader.GetValue(0).ToString().Trim().Length > 5)
                        {
                            tbWarehouseFrom.Text = reader.GetValue(3).ToString();
                            tbWarehouse.Text = reader.GetValue(4).ToString();
                            tbLogisticService.Text = reader.GetValue(15).ToString();
                            tbLisencePlate.Text = reader.GetValue(16).ToString();
                           
                            saveDocument();
                            loadDetail();
                            resetEntry();

                        }
                    }
                    else
                    {
                        acumaticaSync();
                    }
                }
                catch (Exception f)
                {

                }
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

                                using (SqlCommand command = new SqlCommand("Update_ProcessingLineINDetail_Sync", connection))
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

                if (Convert.ToInt16(dtImport.Count) <= Convert.ToInt16(tbBaleReceived.Text))
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
                                    row["BuyerName"].ToString().Trim() != null
                                    )
                                {
                                    string OldDocumentID = row["DocumentID"].ToString().Trim();
                                    string InventoryID = row["InventoryID"].ToString().Trim();
                                    string SubItem = row["SubItem"].ToString().Trim();
                                    string LotNbr = row["LotNbr"].ToString().Trim();
                                    string Source = row["Source"].ToString().Trim();
                                    string Stage = row["Stage"].ToString().Trim();
                                    string Form = row["Form"].ToString().Trim();
                                    string CropYear = row["CropYear"].ToString().Trim();
                                    string Grade = row["Grade"].ToString().Trim();
                                    string Area = row["Area"].ToString().Trim();
                                    string Color = row["Color"].ToString().Trim();
                                    string Fermentation = row["Fermentation"].ToString().Trim();
                                    string Length = row["Length"].ToString().Trim();
                                    string Process = row["Process"].ToString().Trim();
                                    string StalkPosition = row["StalkPosition"].ToString().Trim();
                                    string WeightRope = row["WeightRope"].ToString().Trim();
                                    string WeightShipping = row["WeightShipping"].ToString().Trim();
                                    string WeightReceive = row["WeightReceive"].ToString().Trim();
                                    string WeightTare = row["WeightTare"].ToString().Trim();
                                    string WeightNetto = row["WeightNetto"].ToString().Trim();
                                    string UoM = row["UoM"].ToString().Trim();
                                    string Remark = "IMPORT Dispet IN";
                                    string BuyerName = row["BuyerName"].ToString().Trim();
                                    string UnitCost = row["UnitCost"].ToString().Trim();
                                    string ExtCost = row["ExtCost"].ToString().Trim();
                                  

                                    using (SqlCommand command = new SqlCommand("Insert_DispatchINLineDetail_v2_import", connection, objTrans))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;

                                        command.Parameters.AddWithValue("@LotNbr", LotNbr);
                                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                        command.Parameters.AddWithValue("@InventoryID", InventoryID);
                                        command.Parameters.AddWithValue("@SubItem", SubItem);
                                        command.Parameters.AddWithValue("@Source", Source);
                                        command.Parameters.AddWithValue("@Stage", Stage);
                                        command.Parameters.AddWithValue("@Form", Form);
                                        command.Parameters.AddWithValue("@CropYear", CropYear);
                                        command.Parameters.AddWithValue("@Grade", Grade);
                                        command.Parameters.AddWithValue("@Area", Area);
                                        command.Parameters.AddWithValue("@Color",Color);
                                        command.Parameters.AddWithValue("@Fermentation",Fermentation);
                                        command.Parameters.AddWithValue("@Length",Length);
                                        command.Parameters.AddWithValue("@Process",Process);
                                        command.Parameters.AddWithValue("@StalkPosition", StalkPosition);
                                        command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(WeightRope));
                                        command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(WeightShipping));
                                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(WeightReceive));
                                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(WeightTare));
                                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(WeightNetto));
                                        command.Parameters.AddWithValue("@UoM", UoM);
                                        command.Parameters.AddWithValue("@Remark", Remark);
                                        command.Parameters.AddWithValue("@SyncDetail", 0);
                                        command.Parameters.AddWithValue("@OldDocumentID", OldDocumentID);
                                        command.Parameters.AddWithValue("@BuyerName", BuyerName);
                                        command.Parameters.AddWithValue("@UnitCost", UnitCost);
                                        command.Parameters.AddWithValue("@ExtCost", ExtCost);
                                        command.Parameters.AddWithValue("@OperationType", 0);

                                        command.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show($"Check Imput Excel !", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            objTrans.Commit();
                            loadProcess();
                            loadDetail();
                            resetEntry();
                            dtImport.Clear();
                            cbosheet.Items.Clear();
                            cbosheet.Text = "";
                            dtDetailImport.Clear();
                            textFilename.Clear();
                            SaveImport.Enabled = false;
                            MessageBox.Show("Import Success !");
                        }catch (Exception e_update){
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        //end of file
    }
    //end of file
} //class
