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
    public partial class GenericOUTProcess : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }

        public string tempProcess { get; set; }
        public string tempProcessDescr { get; set; }
        public int PriceScenario { get; set; }
        public String AcumaticaReasonCode { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }

        //private string tempProcess = "BT";
        //private string tempProcessDescr = "Butted";
        //private int PriceScenario = 1;

        private int lastIncrementValue = -1;
        private int lastLotIncrementValue = 0;
        private DataTable dtRefIN;
        private DataTable dtInventory;
        private DataTable dtEntryStage;
        private DataTable dtEntrySource;
        private DataTable dtEntryForm;
        private DataTable dtEntryCropYear;
        private DataTable dtEntryGrade;
        private DataTable dtEntryArea;
        private DataTable dtEntryColor;
        private DataTable dtEntryFerment;
        private DataTable dtEntryLength;
        private DataTable dtEntryStalk;
        private DataTable dtDetail;
        private DataTableCollection dtImport;
        private DataTable dtDetailImport;

        private string tempSource;
        private string tempStage;
        private string tempArea;
        private string tempGrade;
        private string tempForm;
        private string tempCropYear;
        private string tempColor;
        private string tempFermentation;
        private string tempLength;
        private string tempStalk;
        private string tempInventoryID;

        private Decimal tempUnitPrice;
        private Decimal tempMaterialIN;
        private Decimal tempUnappliedBalance;
        private Decimal tempMaterialUse;

        //private string AcumaticaRefNbr;
        private string tempBuyerName;

        private SerialPort port;

        public GenericOUTProcess()
        {
            InitializeComponent();
        }

        private void Process_Load(object sender, EventArgs e)
        {
            if (Userlog.UserRoles.Contains("POUT-IPORT") || Userlog.UserRoles.Contains("SUPERVISOR"))
            {
                label39.Visible = true;
                label40.Visible = true;
                textFilename.Visible = true;
                cbosheet.Visible = true;
                ImportButton.Visible = true;
                SaveImport.Visible = true;
            }
            else
            {
                label39.Visible = false;
                label40.Visible = false;
                textFilename.Visible = false;
                cbosheet.Visible = false;
                ImportButton.Visible = false;
                SaveImport.Visible = false;
            }
            
            //startSerial();
            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcessOUT();
               loadDetail();
                resetEntry();
            }

            if (Userlog.UserRoles.Contains("POUT-IPORT"))
            {
                label39.Visible = true;
                label40.Visible = true;
                textFilename.Visible = true;
                cbosheet.Visible = true;
                ImportButton.Visible = true;
                SaveImport.Visible = true;
            }
            else
            {
                label39.Visible = false;
                label40.Visible = false;
                textFilename.Visible = false;
                cbosheet.Visible = false;
                ImportButton.Visible = false;
                SaveImport.Visible = false;
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
            optLblSticker.Checked = true;
            if (tempProcess == "PC") 
            {
                optLblStickerSimple.Enabled = true;
                optLblStickerSimple.Checked = true;
            }
            else 
            {
                optLblStickerSimple.Enabled = false;
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
            //// Show all the incoming data in the port's buffer
            ////addText(port.ReadExisting(), tbOutput);
            //string readMsg = port.ReadExisting();
            //string prefix = ScaleCom.Prefix;
            //string postfix = ScaleCom.Postfix;
            //if (prefix.Length > 0) { readMsg = readMsg.Replace(prefix, ""); }
            //if (postfix.Length > 0) { readMsg = readMsg.Replace(postfix, ""); }
            //setText(readMsg, tbScale);

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

        private void Process_FormClosing(object sender, FormClosingEventArgs e)
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

        private void loadProcessOUT()
        {
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;
            var refku = "";
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}]";

            //loadComboIN();

            getDocLastIncrement();
            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from ProcessingLineOUT where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    loadComboIN();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbStatus.Text = reader.GetValue(3).ToString();
                        tbINCost.Text = Convert.ToDecimal(reader.GetValue(5)).ToString("N2");
                        //tempMaterialIN = Convert.ToDecimal(reader.GetValue(6));
                        //tbINWeight.Text = tempMaterialIN.ToString("N2");
                        refku=reader.GetValue(4).ToString();
                        cbRefIN.Items.Add(reader.GetValue(4).ToString());
                        cbRefIN.SelectedIndex = cbRefIN.FindString(reader.GetValue(4).ToString());
                        tbAcumaticaRefNbr.Text = reader.GetValue(8).ToString();
                        //AcumaticaRefNbr = reader.GetValue(8).ToString();
                        tbBuyerName.Text = reader.GetValue(9).ToString();
                        tbNotes.Text = reader.GetValue(13).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(3).ToString() == "HOLD" ? 1 : 0);
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                  

                    string query2 = $"select TotalWeight from ProcessingLineIN where DocumentID = '{refku}'";
                    SqlCommand command2 = new SqlCommand(query2, connection);
                    connection.Open();
                    SqlDataReader reader2 = command2.ExecuteReader();


                    if (reader2.HasRows)
                    {
                        reader2.Read();
                        tempMaterialIN = Convert.ToDecimal(reader2.GetValue(0));
                        tbINWeight.Text = tempMaterialIN.ToString("N2");

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
	                                        ProcessingLineOUTDetail
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
                    dgvDetail.Columns[22].HeaderText = "Ref IN";
                    dgvDetail.Columns[23].HeaderText = "Synced";
                    dgvDetail.Columns[24].HeaderText = "Zero Cost";
                    dgvDetail.Columns[25].HeaderText = "Buyer Name";
                    dgvDetail.Columns[25].Visible = false;

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
                        
                        
                        if (dtDetail.Compute("SUM(WeightNetto)", "ZeroCost = 0") != DBNull.Value)
                        {
                            decimal sumWNonZeroCost = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", "ZeroCost = 0"));
                            tbDetailWNonZeroCost.Text = sumWNonZeroCost.ToString("N2");
                        }
                        else
                        {
                            decimal sumWNonZeroCost = 0;
                            tbDetailWNonZeroCost.Text = sumWNonZeroCost.ToString("N2");
                        }

                        //disable change reg
                        cbRefIN.Enabled = false;

                        //tbBuyerName.ReadOnly = true;
                        //tbBuyerName.BackColor = System.Drawing.SystemColors.Info;
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

                        decimal sumWNonZeroCost = 0;
                        tbDetailWNonZeroCost.Text = sumWNonZeroCost.ToString("N2");

                        //enable change reg
                        cbRefIN.Enabled = true;

                        //tbBuyerName.ReadOnly = false;
                        //tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            setUnitCost();

            if (cbRefIN.SelectedIndex >= 0)
            {
                calcMaterialUse();
            }
        }

        private void resetScreen()
        {
            currentDate = DateTime.Now;
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";

            checkHold.Checked = true;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}]";

            loadComboIN();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbINCost.Text = "0";
            tbINWeight.Text = "0";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaRefNbr.Text = "";
            tbBuyerName.Text = "";
            tempInventoryID = "";
            tbNotes.Text = "";

            resetEntry();
            loadDetail();
        }

        private void resetEntry()
        {
            //loadDetail();

            groupEntry.Text = "Lot Entry [<NEW>]";
            groupEntry.BackgroundImage = null;

            tbEntryLot.Text = "";
            tbEntryInv.Text = tempInventoryID;

            loadComboSource();
            loadComboStage();
            loadComboForm();
            loadComboCropYear();
            loadComboGrade();
            loadComboArea();
            loadComboColor();
            loadComboFerment();
            loadComboLength();
            loadComboStalk();

            tbEntryWeightRope.Text = "0";
            tbEntryWeightShipping.Text = "0";
            tbEntryWeightReceive.Text = "0";
            tbEntryWeightTare.Text = "0";
            tbEntryWeightNetto.Text = "0";
            tbEntryWeightUoM.Text = "KG";
            tbEntryRemark.Text = "";

            cbEntrySource.Enabled = true;
            cbEntryForm.Enabled = true;

            cbEntrySource.SelectedIndex = cbEntrySource.FindString(tempSource);
            cbEntryStage.SelectedIndex = cbEntryStage.FindString(tempStage);
            cbEntryForm.SelectedIndex = cbEntryForm.FindString(tempForm);
            cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(tempCropYear);
            cbEntryGrade.SelectedIndex = cbEntryGrade.FindStringExact(tempGrade);
            cbEntryArea.SelectedIndex = cbEntryArea.FindString(tempArea);
            cbEntryColor.SelectedIndex = cbEntryColor.FindString(tempColor);
            cbEntryFerment.SelectedIndex = cbEntryFerment.FindString(tempFermentation);
            cbEntryLength.SelectedIndex = cbEntryLength.FindString(tempLength);
            cbEntryStalk.SelectedIndex = cbEntryStalk.FindString(tempStalk);

            setLotNbr();

            switch (tbStatus.Text)
            {
                case "OPEN":
                    //if(tempUnappliedBalance == 0)
                    //{
                        btnAcumatica.Enabled = true;
                    //}
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = true;
                    btnPrintSUM.Enabled = true;
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
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintDoc.Enabled = true;
                    btnPrintSUM.Enabled = true;
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
                    btnSaveLot.Enabled = true;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = false;
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

            btnPrintLot.Enabled = false;
        }

        private void loadComboIN()
        {
            cbInventory.SelectedIndex = -1;
            cbInventory.Items.Clear();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbRefIN.SelectedIndex = -1;

                dtRefIN = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    *
                                    FROM
	                                    ProcessingLineIN
                                    WHERE
	                                    ProcessType = '{tempProcess}' AND UnappliedBalance > 0";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtRefIN);
                    string[] arrray = dtRefIN.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbRefIN);
                    cbRefIN.Items.Clear();
                    cbRefIN.Items.AddRange(arrray);
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
                    //string query = $@"SELECT DISTINCT
                    //                 ProcessingLineINDetail.InventoryID,
                    //                 InventoryItem.Descr
                    //                FROM
                    //                 dbo.ProcessingLineINDetail
                    //                 INNER JOIN
                    //                 dbo.InventoryItem
                    //                 ON
                    //                  ProcessingLineINDetail.InventoryID = InventoryItem.InventoryID
                    //                WHERE
                    //                 ProcessingLineINDetail.DocumentID = '{cbRefIN.SelectedItem.ToString()}'";

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

        private void loadRefINData()
        {
            DataView dv_filter = new DataView(dtRefIN, $"DocumentID LIKE '%{cbRefIN.SelectedItem.ToString()}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            foreach (DataRowView rowView in dv_filter)
            {
                DataRow row = rowView.Row;
                var INCost = Convert.ToDecimal(row.ItemArray[4]);
                var INWeight = Convert.ToDecimal(row.ItemArray[5]);
                var INUnapplied = Convert.ToDecimal(row.ItemArray[7]);
                //var UnitPrice = INCost / INWeight;

                tbINCost.Text = INCost.ToString("N2");
                tbINWeight.Text = INWeight.ToString("N2");
                tbUnappliedBalance.Text = INUnapplied.ToString("N2");
                //tempUnappliedBalance = INUnapplied;
                //tbUnitCost.Text = UnitPrice.ToString("N2");
                tbBuyerName.Text = row.ItemArray[9].ToString();
                setUnitCost();
            }
        }

        private void setUnitCost()
        {
            var INCost = Convert.ToDecimal(tbINCost.Text.Replace(",", ""));
            var INWeight = Convert.ToDecimal(tbINWeight.Text.Replace(",", ""));
            var DetailsWeight = Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));
            //var DetailsNonZeroCost = Convert.ToDecimal(tbDetailWNonZeroCost.Text.Replace(",", ""));
            decimal DetailsNonZeroCost = 0;

            if(cbRefIN.SelectedIndex >= 0)
            {
                //load
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        //string query = $"select SUM(WeightNetto) as MaterialUse from ProcessingLineOUTDetail where OldDocumentID = '{cbRefIN.SelectedItem.ToString() ?? ""}'";
                        string query = $@"SELECT
                                            SUM(ProcessingLineOUTDetail.WeightNetto) matUse
                                        FROM
                                            dbo.ProcessingLineOUTDetail
                                        WHERE
                                            ProcessingLineOUTDetail.OldDocumentID = '{cbRefIN.SelectedItem.ToString() ?? ""}'
                                        AND ProcessingLineOUTDetail.ZeroCost = 0";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();
                            DetailsNonZeroCost = Convert.ToDecimal(reader.GetValue(0).ToString() == "" ? "0" : reader.GetValue(0).ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }


            tempUnitPrice = 0;

            switch (PriceScenario)
            {
                case 1:
                    if (DetailsNonZeroCost > 0) tempUnitPrice = INCost / DetailsNonZeroCost;
                    break;

                case 2:
                    if (INWeight > 0) tempUnitPrice = INCost / INWeight;
                    break;
            }

            tbUnitCost.Text = tempUnitPrice.ToString("N2");
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void tbUnitCost_TextChanged(object sender, EventArgs e)
        {
            tempUnitPrice = Convert.ToDecimal(((TextBox)sender).Text.Replace(",", ""));
        }

        private void getDocLastIncrement()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from NumberingSetting where NumberingID = '{tempProcess}/OUT'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        DateTime dbDate = Convert.ToDateTime(reader.GetValue(2).ToString());
                        int dbIncrement = Convert.ToInt32(reader.GetValue(1).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        //var cur = currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).Year;
                        //var last = dbDate.AddMonths(-FiscalInfo.StartingFiscalMonth).Year;
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

            if(lastIncrementValue >= 0)
            {
                var currentIncrement = lastIncrementValue + 1;
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempProcess}/OUT-{currentIncrement.ToString().PadLeft(4, '0')}";


                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempProcess}/OUT-{currentIncrement.ToString().PadLeft(4, '0')}";

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
                    string query = $"IF EXISTS ( SELECT * FROM ProcessingLineOUT WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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

        private void setLotNbr()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from NumberingSetting where NumberingID = '{tempProcess}/LOT'";
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
                }
            }
            var currentLotIncrement = lastLotIncrementValue + 1;

            string lotnbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempInventoryID ?? "XX"}{tempSource ?? "X"}{tempProcess}{tempForm ?? "XX"}{currentLotIncrement.ToString().PadLeft(5, '0')}";
            tbEntryLot.Text = lotnbr.Trim();
        }

        private void saveDocument()
        {
            bool incrementError = false;
            int OperationType = 1;

            if (cbRefIN.SelectedItem != null)
            {
                
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

                            using (SqlCommand command = new SqlCommand("Insert_ProcessingLineOUT_v2", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                                command.Parameters.AddWithValue("@DocumentDate", tbProcessDate.Text);
                                command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                                command.Parameters.AddWithValue("@Status", tbStatus.Text);
                                command.Parameters.AddWithValue("@RefINNbr", cbRefIN.SelectedItem.ToString() ?? "");
                                command.Parameters.AddWithValue("@TotalCost", tbINCost.Text.Replace(",", ""));
                                command.Parameters.AddWithValue("@TotalWeight", tbINWeight.Text.Replace(",", ""));
                                command.Parameters.AddWithValue("@ProcessType", tempProcess);
                                command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                                command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text);
                                command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                                command.Parameters.AddWithValue("@Notes", tbNotes.Text);
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
                        //        this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}]";

                        //        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                        //        {
                        //            command.CommandType = CommandType.StoredProcedure;
                        //            command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/OUT");
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
                                this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}]";

                                using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/OUT");
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


            }
            else
            {
                MessageBox.Show($"Silakan pilih referensi untuk dokumen IN!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //if (tbStatus.Text == "OPEN" && !checkHold.Checked && tempUnappliedBalance == 0)
            if (tbStatus.Text == "OPEN" && !checkHold.Checked)
            {
               
                btnAcumatica.Enabled = true;
            }
            else
            {
                //if (tbStatus.Text == "OPEN" && !checkHold.Checked && tempUnappliedBalance != 0) {
                //    MessageBox.Show($"Jika dokumen ini tidak dapat di singkronise karena berat total In dan OUT tidak sama", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                btnAcumatica.Enabled = false;
            }

            if (tbStatus.Text == "OPEN" || tbStatus.Text == "SYNCED")
            {
                btnPrintDoc.Enabled = true;
                btnPrintSUM.Enabled = true;
            }
            else
            {
                btnPrintDoc.Enabled = false;
                btnPrintSUM.Enabled = false;
            }
        }

        private void saveLot(int OperationType)
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

                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineOUTDetail_v2", connection))
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
                        command.Parameters.AddWithValue("@Color", tempColor);
                        command.Parameters.AddWithValue("@Fermentation", tempFermentation);
                        command.Parameters.AddWithValue("@Length", tempLength);
                        command.Parameters.AddWithValue("@Process", tempProcess);
                        command.Parameters.AddWithValue("@StalkPosition", tempStalk);
                        command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(tbEntryWeightRope.Text == "" ? "0" : tbEntryWeightRope.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(tbEntryWeightShipping.Text == "" ? "0" : tbEntryWeightShipping.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(tbEntryWeightReceive.Text == "" ? "0" : tbEntryWeightReceive.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(tbEntryWeightTare.Text == "" ? "0" : tbEntryWeightTare.Text.Replace(",", "")));
                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(tbEntryWeightNetto.Text == "" ? "0" : tbEntryWeightNetto.Text.Replace(",","")));
                        command.Parameters.AddWithValue("@UoM", tbEntryWeightUoM.Text);
                        command.Parameters.AddWithValue("@Remark", tbEntryRemark.Text);
                        command.Parameters.AddWithValue("@OldDocumentID", cbRefIN.SelectedItem.ToString() ?? "");
                        command.Parameters.AddWithValue("@SyncDetail", 0);
                        command.Parameters.AddWithValue("@ZeroCost", checkZeroCost.Checked ? 1 : 0);
                        command.Parameters.AddWithValue("@BuyerName", tempBuyerName ?? "");
                        command.Parameters.AddWithValue("@OperationType", OperationType);

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
                                command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/LOT");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastLotIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }
                        }

                        ////new unapplied
                        //tbUnappliedBalance.Text = (lastUnappliedBalance + tempWeightChange).ToString("N2");
                        calcMaterialUse();

                        saveDocument();
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
                            //trigger print & entry
                            btnPrintLot.PerformClick();
                            //btnAddEntry.PerformClick();
                            resetEntry();
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

        private void calcMaterialUse()
        {
            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    //string query = $"select SUM(WeightNetto) as MaterialUse from ProcessingLineOUTDetail where OldDocumentID = '{cbRefIN.SelectedItem.ToString()}'";
  
                    string query = $@"SELECT ISNULL(SUM(MatUse), 0 ) as MaterialUse FROM
                                        (
                                        select SUM(WeightNetto) as MatUse from ProcessingLineOUTDetail where OldDocumentID = '{cbRefIN.SelectedItem.ToString()}'
                                        UNION ALL
                                        select SUM(TotalWeight) as MatUse from ProcessingLineOUTDonor where RefINNbr = '{cbRefIN.SelectedItem.ToString()}'
                                        UNION ALL
                                        select Abs(SUM(TotalWeight)) as MatUse from ProcessingLineOUTDonor where RefINDonor = '{cbRefIN.SelectedItem.ToString()}'
                                        UNION ALL
                                        select ShrinkBalance as MatUse from ProcessingLineIN where DocumentID = '{cbRefIN.SelectedItem.ToString()}'
                                        )as tbl";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tempMaterialUse = Convert.ToDecimal(reader.GetValue(0).ToString());

                        tbUnappliedBalance.Text = (tempMaterialIN - tempMaterialUse).ToString("N2");

                    }
                    reader.Close();

                    //update issue balance calc mat
                    using (SqlCommand command2 = new SqlCommand("Update_Issue_Balance", connection))
                    {
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.AddWithValue("@DocumentID", cbRefIN.SelectedItem.ToString());
                        command2.Parameters.AddWithValue("@UnappliedBalance", tempMaterialIN - tempMaterialUse);
                        command2.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
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

                    using (SqlCommand command = new SqlCommand("Delete_ProcessingLineOUTDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@LotNbr", tbEntryLot.Text);

                        command.ExecuteNonQuery();
                    }

                    calcMaterialUse();
                    ////new unapplied
                    //var lastUnappliedBalance = Convert.ToDecimal(tbUnappliedBalance.Text.Replace(",", ""));
                    //var currentApplied = Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", ""));
                    //tbUnappliedBalance.Text = (lastUnappliedBalance + currentApplied).ToString("N2");

                    ////end applied balance
                    ////Update Unapplied balance on issue
                    //try
                    //{
                    //    if (connection.State != ConnectionState.Open)
                    //    {
                    //        connection.Open();
                    //    }

                    //    using (SqlCommand command = new SqlCommand("Update_Issue_Balance", connection))
                    //    {
                    //        command.CommandType = CommandType.StoredProcedure;
                    //        command.Parameters.AddWithValue("@DocumentID", cbRefIN.SelectedItem.ToString());
                    //        command.Parameters.AddWithValue("@UnappliedBalance", tempUnappliedBalance);

                    //        command.ExecuteNonQuery();
                    //    }
                    //}
                    //catch (Exception e_update)
                    //{
                    //    MessageBox.Show($"--Update issue balance error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //}

                    saveDocument();
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Delete error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    MessageBox.Show("Remove lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    resetEntry();
                    loadDetail();
                }
            }
        }

        private bool weightCheck()
        {
            var newWeight = Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", ""));
            var unappliedBalance = Convert.ToDecimal(tbUnappliedBalance.Text.Replace(",", ""));
            var detailsWeight = Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));

            if (unappliedBalance >= (detailsWeight + newWeight))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool weightCheckTotal()
        {
            var unappliedBalance = Convert.ToDecimal(tbUnappliedBalance.Text.Replace(",", ""));
            var detailsWeight = Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));

            if (unappliedBalance >= detailsWeight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DocNumber = "<NEW>";
            resetScreen();
        }

        private void btnUpdateRefINStatus_Click(object sender, EventArgs e)
        {
            if (cbRefIN.SelectedIndex >= 0)
            {
                calcMaterialUse();
            }
        }

        private void cbRefIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefIN.SelectedIndex >= 0)
            {
                loadRefINData();
                loadComboInv();
            }
            else
            {
                tbINCost.Text = "0";
                tbINWeight.Text = "0";
                tbUnitCost.Text = "0";
                tbUnappliedBalance.Text = "0";
                cbInventory.SelectedIndex = -1;
                tbBuyerName.Text = "";
            }
        }

        private void cbInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbInventory.SelectedIndex >= 0)
            {
                tbEntryInv.Text = cbInventory.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempInventoryID = "";
            }

            //resetEntry();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
            resetEntry();
        }

        //load for entry
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

                    //new AutoCompleteBehavior(cbEntrySource);
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
                    string query = $"select * from TobaccoGrade where InventoryID = '{tempInventoryID}' and ProcessID = '{tempProcess}' and WarehouseID = '{tbWarehouse.Text}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryGrade);
                    string[] arrray = dtEntryGrade.Rows.OfType<DataRow>().Select(k => k[3].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryGrade);
                    cbEntryGrade.Items.Clear();
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

        private void loadComboColor()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryColor.SelectedIndex = -1;

                dtEntryColor = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'COLOR' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryColor);
                    string[] arrray = dtEntryColor.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryColor);
                    cbEntryColor.Items.Clear();
                    cbEntryColor.Items.AddRange(arrray);
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

        private void loadComboFerment()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryFerment.SelectedIndex = -1;

                dtEntryFerment = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'FERMENTATION' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryFerment);
                    string[] arrray = dtEntryFerment.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryFerment);
                    cbEntryFerment.Items.Clear();
                    cbEntryFerment.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboLength()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbEntryLength.SelectedIndex = -1;

                dtEntryLength = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'LENGTH' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntryLength);
                    string[] arrray = dtEntryLength.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbEntryLength);
                    cbEntryLength.Items.Clear();
                    cbEntryLength.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                dgvDetail.ClearSelection();
            }

            resetEntry();
        }

        private void tbEntryInv_TextChanged(object sender, EventArgs e)
        {
            //loadComboGrade();
            //setLotNbr();

            tempInventoryID = ((TextBox)sender).Text;
            resetEntry();
        }

        private void cbEntrySource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntrySource.SelectedItem != null) tempSource = cbEntrySource.SelectedItem.ToString().Split('|')[0].Trim();
            setLotNbr();
        }

        private void cbEntryForm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryForm.SelectedItem != null) tempForm = cbEntryForm.SelectedItem.ToString().Split('|')[0].Trim();
            setLotNbr();
        }

        private void btnSaveLot_Click(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                saveDocument();
                tbEntryLot.Focus();
            }

            if (DocNumber != "<NEW>")
            {
                if (groupEntry.Text.Contains("<NEW>"))
                {
                    setLotNbr();

                    string lotnbr = tbEntryLot.Text;

                    if (!checkLotNbrExist(lotnbr))
                    {
                        if (tbEntryWeightNetto.Text != "0" && Convert.ToDecimal(tbUnappliedBalance.Text.Replace(".", "")) > 0)
                        {
                            saveLot(0);
                            tbEntryLot.Focus();
                        }
                        else
                        {
                            if (tbEntryWeightNetto.Text == "0") { MessageBox.Show($"You have zero netto!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                            else { MessageBox.Show($"You're out of unapplied balance!\n[Negative unapplied balance]", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Lot number {lotnbr} already exist, or unable to check existing lot number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }



                }
                else
                {

                    if (CheckStockData(tbEntryLot.Text))
                    {
                        saveLot(1);
                        tbEntryLot.Focus();
                    }
                    else
                    {
                        MessageBox.Show($"Stock already used, cannot edit lot!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    //MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            tbEntryWeightReceive.Text = tbScale.Text;
        }

        private void cbEntryStage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryStage.SelectedItem != null)
            {
                tempStage = cbEntryStage.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempStage = "";
            }
        }

        private void cbEntryCropYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryCropYear.SelectedItem != null)
            {
                tempCropYear = cbEntryCropYear.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempCropYear = "";
            }
        }

        private void cbEntryGrade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryGrade.SelectedItem != null)
            {
                tempGrade = cbEntryGrade.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempGrade = "";
            }
        }

        private void cbEntryArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryArea.SelectedItem != null)
            {
                tempArea = cbEntryArea.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempArea = "";
            }
        }

        private void cbEntryColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryColor.SelectedItem != null)
            {
                tempColor = cbEntryColor.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempColor = "";
            }
        }

        private void cbEntryFerment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryFerment.SelectedItem != null)
            {
                tempFermentation = cbEntryFerment.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempFermentation = "";
            }
        }

        private void cbEntryLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryLength.SelectedItem != null)
            {
                tempLength = cbEntryLength.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempLength = "";
            }
        }

        private void cbEntryStalk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbEntryStalk.SelectedItem != null)
            {
                tempStalk = cbEntryStalk.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                //tempStalk = "";
            }
        }

        private void tbEntryWeightReceive_TextChanged(object sender, EventArgs e)
        {
            Decimal rope = 0;
            Decimal received = 0;
            Decimal tare = 0;
            Decimal newNetto = 0M;

            if (tbEntryWeightTare.TextLength > 0)
            {
                tare = Convert.ToDecimal(tbEntryWeightTare.Text);
            }

            if (tbEntryWeightReceive.TextLength > 0)
            {
                received = Convert.ToDecimal(tbEntryWeightReceive.Text);
            }

            if (tbEntryWeightRope.TextLength > 0)
            {
                rope = Convert.ToDecimal(tbEntryWeightRope.Text);
            }

            //tbEntryWeightNetto.Text = (received - rope - tare).ToString();
            newNetto = received - tare;
            tbEntryWeightNetto.Text = newNetto.ToString("N2");
        }

        private void tbEntryWeightRope_TextChanged(object sender, EventArgs e)
        {
            Decimal rope = 0;
            Decimal received = 0;
            Decimal tare = 0;
            Decimal newNetto = 0M;

            if (tbEntryWeightTare.TextLength > 0)
            {
                tare = Convert.ToDecimal(tbEntryWeightTare.Text);
            }

            if (tbEntryWeightReceive.TextLength > 0)
            {
                received = Convert.ToDecimal(tbEntryWeightReceive.Text);
            }

            if (tbEntryWeightRope.TextLength > 0)
            {
                rope = Convert.ToDecimal(tbEntryWeightRope.Text);
            }

            //tbEntryWeightNetto.Text = (received - rope - tare).ToString();
            newNetto = received - tare;
            tbEntryWeightNetto.Text = newNetto.ToString("N2");
        }

        private void tbEntryWeightTare_TextChanged(object sender, EventArgs e)
        {
            Decimal rope = 0;
            Decimal received = 0;
            Decimal tare = 0;
            Decimal newNetto = 0M;

            if (tbEntryWeightTare.TextLength > 0)
            {
                tare = Convert.ToDecimal(tbEntryWeightTare.Text);
            }

            if (tbEntryWeightReceive.TextLength > 0)
            {
                received = Convert.ToDecimal(tbEntryWeightReceive.Text);
            }

            if (tbEntryWeightRope.TextLength > 0)
            {
                rope = Convert.ToDecimal(tbEntryWeightRope.Text);
            }

            //tbEntryWeightNetto.Text = (received - rope - tare).ToString();
            newNetto = received - tare;
            tbEntryWeightNetto.Text = newNetto.ToString("N2");
        }

        private void tbUnappliedBalance_TextChanged(object sender, EventArgs e)
        {
            tempUnappliedBalance = Convert.ToDecimal(tbUnappliedBalance.Text.Replace(",", ""));

            if (tempUnappliedBalance < 0)
            {
                btnBalance.Enabled = true;
            }
            else
            {
                btnBalance.Enabled = false;
            }

            if (tempUnappliedBalance <= 0)
            {
                btnAddEntry.Enabled = false;
                btnShrinkBalance.Enabled = false;
            }
            else
            {
                btnAddEntry.Enabled = true;
                btnShrinkBalance.Enabled = true;
            }
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            //if (weightCheckTotal() && dtDetail.Rows.Count > 0)
            if (dtDetail.Rows.Count > 0)
            {
                this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}] - Syncing with Acumatica, please wait!";
                bool syncError = false;
                bool allSynced = true;
                string docNbr = "";
                string referenceNbr = "";
                var docBranch = GetBranch(tbWarehouse.Text,2);

                loadDetail();
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

                //issue
                InventoryReceipt inventoryReceipt = new InventoryReceipt();
                inventoryReceipt.Date = Convert.ToDateTime(tbProcessDate.Text);
                inventoryReceipt.Branch = docBranch;
                inventoryReceipt.Description = $"{tbWarehouse.Text} {tempProcessDescr} Processing OUT Transaction Receipt";
                inventoryReceipt.ExternalRef = tbDocNumber.Text;
                inventoryReceipt.Hold = false;

                List<InventoryReceiptDetail> receiptDetails = new List<InventoryReceiptDetail>();

                foreach (DataRowView rowView in dv_filter)
                {
                    //check if doc buying already synced
                    if (docNbr != rowView[22].ToString())
                    {
                        docNbr = rowView[22].ToString();
                        if (!checkDocumentSync(docNbr))
                        {
                            allSynced = false;
                        }
                    }
                    InventoryReceiptDetail receiptDetail = new InventoryReceiptDetail();
                    receiptDetail.InventoryID = rowView[1].ToString();
                    receiptDetail.Branch = docBranch;
                    receiptDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                    receiptDetail.Subitem = rowView[2].ToString().Replace(".", "");
                    receiptDetail.Qty = Convert.ToDecimal(rowView[19]);
                    receiptDetail.Description = rowView[3].ToString();
                    receiptDetail.WarehouseID = tbWarehouse.Text;
                    receiptDetail.UOM = rowView[20].ToString();
                    receiptDetail.ReasonCode = AcumaticaReasonCode;

                    if (rowView[24].ToString() == "0")
                    {
                        receiptDetail.UnitCost = tempUnitPrice;
                    }
                    else
                    {
                        receiptDetail.UnitCost = 0;
                    }

                    //issueDetail.LotSerialNbr = "";

                    receiptDetails.Add(receiptDetail);
                }
                inventoryReceipt.Details = receiptDetails;

                if (!allSynced)
                {
                    MessageBox.Show($"Documents for bale contained in this process is not synced to Acumatica!\nPlease sync required documents first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                    var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
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
                        //System.Threading.Thread.Sleep(1000);

                        //var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
                        //decimal tempTotalCost = (decimal)inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr }).TotalCost;
                        //checkReleasedIssue(referenceNbr, configuration);
                        authApi.AuthLogout();
                        if (!syncError)
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} OUT Process [{DocNumber}]";
                            tbStatus.Text = "SYNCED";

                            saveDocument();
                            MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                            ////new unapplied
                            //var lastUnappliedBalance = Convert.ToDecimal(tbUnappliedBalance.Text.Replace(",", ""));
                            //var currentApplied = Convert.ToDecimal(tbDetailWNetto.Text.Replace(",", ""));

                            //tbUnappliedBalance.Text = (lastUnappliedBalance - currentApplied).ToString("N2");
                            ////end applied balance
                            ////Update Unapplied balance on issue
                            //using (SqlConnection connection = new SqlConnection(ConnectionString))
                            //{
                            //    try
                            //    {
                            //        if (connection.State != ConnectionState.Open)
                            //        {
                            //            connection.Open();
                            //        }

                            //        using (SqlCommand command = new SqlCommand("Update_Issue_Balance", connection))
                            //        {
                            //            command.CommandType = CommandType.StoredProcedure;
                            //            command.Parameters.AddWithValue("@DocumentID", cbRefIN.SelectedItem.ToString());
                            //            command.Parameters.AddWithValue("@UnappliedBalance", tempUnappliedBalance);

                            //            command.ExecuteNonQuery();
                            //        }
                            //    }
                            //    catch (Exception e_update)
                            //    {
                            //        MessageBox.Show($"--Update issue balance error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    }
                            //}

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

                                        using (SqlCommand command = new SqlCommand("Update_ProcessingLineOUTDetail_Sync", connection))
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
            }
            else
            {
                MessageBox.Show($"No output item or Total weight over unapplied balance from issue!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnDelLot_Click(object sender, EventArgs e)
        {
            if (CheckStockData(tbEntryLot.Text))
            {
                removeLot();
                //saveDocument();
            } else
            {
                MessageBox.Show($"Stock already used, cannot remove lot!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private Boolean CheckStockData(string LotNbr)
        {
            //check data from stock
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT StatusStock
                                        FROM
	                                        StockItem
                                        WHERE
	                                        LotNbr = '{LotNbr}'
                                        AND DocumentID = '{DocNumber}'
                                        AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            return false;

        }
        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (dgvDetail.SelectedRows.Count > 0)
            {
               
                var lotNbr = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
                if (checkLotNbrInSTock(lotNbr))
                {
                 
                    groupEntry.Text = $"Lot Entry [{dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString()}]";
                    groupEntry.BackgroundImage = Properties.Resources.editMode;

                    tbEntryInv.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();

                    cbEntrySource.SelectedIndex = cbEntrySource.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[4].FormattedValue.ToString());
                    cbEntryStage.SelectedIndex = cbEntryStage.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].FormattedValue.ToString());
                    cbEntryForm.SelectedIndex = cbEntryForm.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[6].FormattedValue.ToString());
                    cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[7].FormattedValue.ToString());
               
                    cbEntryGrade.SelectedIndex = cbEntryGrade.FindStringExact(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].FormattedValue.ToString());
                    cbEntryArea.SelectedIndex = cbEntryArea.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[9].FormattedValue.ToString());
                    cbEntryColor.SelectedIndex = cbEntryColor.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].FormattedValue.ToString());
                    cbEntryFerment.SelectedIndex = cbEntryFerment.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[11].FormattedValue.ToString());
                    cbEntryLength.SelectedIndex = cbEntryLength.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[12].FormattedValue.ToString());
                    cbEntryStalk.SelectedIndex = cbEntryStalk.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[14].FormattedValue.ToString());

                    tbEntryWeightRope.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[15].Value.ToString();
                    tbEntryWeightShipping.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[16].Value.ToString();
                    tbEntryWeightReceive.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[17].Value.ToString();
                    tbEntryWeightTare.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[18].Value.ToString();
                    tbEntryWeightNetto.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].Value.ToString();
                    tbEntryWeightUoM.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[20].FormattedValue.ToString();

                    tbEntryRemark.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[21].FormattedValue.ToString();
                    checkZeroCost.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[24].Value.ToString() == "1" ? true : false;

                    tbEntryLot.Text = lotNbr;
                    //tbEntryLot.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
                    btnSaveLot.Enabled = true;
                    cbEntrySource.Enabled = false;
                    cbEntryForm.Enabled = false;
                    if (tbStatus.Text == "SYNCED")
                    {
                        btnSaveLot.Enabled = false;
                        btnDelLot.Enabled = false;
                        groupEntry.BackgroundImage = Properties.Resources.viewMode;
                    }
                    else
                    {
                        btnDelLot.Enabled = true;
                    }
                    
                    btnPrintLot.Enabled = true;

                }
                else
                {
                    groupEntry.Text = $"Lot Entry [{dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString()}]";
                    

                    tbEntryInv.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();

                    cbEntrySource.SelectedIndex = cbEntrySource.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[4].FormattedValue.ToString());
                    cbEntryStage.SelectedIndex = cbEntryStage.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].FormattedValue.ToString());
                    cbEntryForm.SelectedIndex = cbEntryForm.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[6].FormattedValue.ToString());
                    cbEntryCropYear.SelectedIndex = cbEntryCropYear.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[7].FormattedValue.ToString());
                    cbEntryGrade.SelectedIndex = cbEntryGrade.FindStringExact(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].FormattedValue.ToString());
                    cbEntryArea.SelectedIndex = cbEntryArea.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[9].FormattedValue.ToString());
                    cbEntryColor.SelectedIndex = cbEntryColor.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].FormattedValue.ToString());
                    cbEntryFerment.SelectedIndex = cbEntryFerment.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[11].FormattedValue.ToString());
                    cbEntryLength.SelectedIndex = cbEntryLength.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[12].FormattedValue.ToString());
                    cbEntryStalk.SelectedIndex = cbEntryStalk.FindString(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[14].FormattedValue.ToString());

                    tbEntryWeightRope.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[15].Value.ToString();
                    tbEntryWeightShipping.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[16].Value.ToString();
                    tbEntryWeightReceive.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[17].Value.ToString();
                    tbEntryWeightTare.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[18].Value.ToString();
                    tbEntryWeightNetto.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].Value.ToString();
                    tbEntryWeightUoM.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[20].FormattedValue.ToString();

                    tbEntryRemark.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[21].FormattedValue.ToString();
                    checkZeroCost.Checked = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[24].Value.ToString() == "1" ? true : false;

                    tbEntryLot.Text = lotNbr;
                    //tbEntryLot.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();

                    cbEntrySource.Enabled = false;
                    cbEntryForm.Enabled = false;
                    btnDelLot.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnPrintLot.Enabled = true;
                    groupEntry.BackgroundImage = Properties.Resources.viewMode;
                }

            }
        }


        private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in dgvDetail.Rows)
            {
                if (Convert.ToInt32(Myrow.Cells[24].Value) == 1)
                {
                    Myrow.DefaultCellStyle.BackColor = Color.PaleVioletRed;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.LightGreen;
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

        private void btnScaleComm_MouseEnter(object sender, EventArgs e)
        {
            myToolTip.Show("Connect / Disconnect scale communication", btnScaleComm);
        }

        private void btnScaleComm_MouseLeave(object sender, EventArgs e)
        {
            myToolTip.Hide(btnScaleComm);
        }

        private void btnScaleOverride_MouseEnter(object sender, EventArgs e)
        {
            myToolTip.Show("Manual override scale value", btnScaleComm);
        }

        private void btnScaleOverride_MouseLeave(object sender, EventArgs e)
        {
            myToolTip.Hide(btnScaleComm);
        }

        private void btnScale_MouseEnter(object sender, EventArgs e)
        {
            myToolTip.Show("Get scale value for entry", btnScaleComm);
        }

        private void btnScale_MouseLeave(object sender, EventArgs e)
        {
            myToolTip.Hide(btnScaleComm);
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
            if (e.KeyChar == '\r')
            {
                if (this.ActiveControl != null)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                e.Handled = true; // Mark the event as handled
            }

            //if (!char.IsDigit(e.KeyChar)) e.Handled = true;         //Just Digits
            //if (e.KeyChar == (char)44 || e.KeyChar == (char)46) e.Handled = false;            //Allow comma and period
        }

        private void tbScale_KeyPress(object sender, KeyPressEventArgs e)
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

        private void btnBalance_Click(object sender, EventArgs e)
        {
            //if (checkDocumentSync(cbRefIN.SelectedItem.ToString()))
            if (true)
            {
                Forms.GenericOUTProcesDonor genericOUTProcesDonor = new Forms.GenericOUTProcesDonor
                {
                    ConnectionString = ConnectionString,
                    DocNumber = DocNumber,
                    tempProcess = tempProcess,
                    RefINNbr = cbRefIN.SelectedItem.ToString(),
                    Userlog = Userlog,
                    WeightDonor = tempMaterialIN - tempMaterialUse,
                    BuyerName = tempBuyerName
                };
                genericOUTProcesDonor.ShowDialog();

                calcMaterialUse();
            }
            else
            {
                MessageBox.Show($"Processing IN document is still open, you can't request balance donation.\nPlease close (sync) IN document first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



            //using (SqlConnection connection = new SqlConnection(ConnectionString))
            //{
            //    //Update Unapplied balance on issue
            //    try
            //    {
            //        if (connection.State != ConnectionState.Open)
            //        {
            //            connection.Open();
            //        }

            //        using (SqlCommand command = new SqlCommand("Update_Issue_Balance", connection))
            //        {
            //            command.CommandType = CommandType.StoredProcedure;
            //            command.Parameters.AddWithValue("@DocumentID", cbRefIN.SelectedItem.ToString());
            //            command.Parameters.AddWithValue("@UnappliedBalance", tempUnappliedBalance);

            //            command.ExecuteNonQuery();
            //        }
            //    }
            //    catch (Exception e_update)
            //    {
            //        MessageBox.Show($"--Update issue balance error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }

            //    saveDocument();
            //}
        }

        private void tbBuyerName_TextChanged(object sender, EventArgs e)
        {
            tempBuyerName = ((TextBox)sender).Text;
        }

        private void tbINWeight_TextChanged(object sender, EventArgs e)
        {
            tempMaterialIN = Convert.ToDecimal(((TextBox)sender).Text.Replace(",", ""));
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

                    if (optLblSticker.Checked)
                    {
                        GenericLotPrint lotPrint = new GenericLotPrint
                        {
                            LotNumber = tbEntryLot.Text,
                            Source = cbEntrySource.SelectedItem.ToString().Split('|')[0].Trim(),
                            StalkPos = cbEntryStalk.SelectedItem.ToString().Split('|')[0].Trim(),
                            Ferment = cbEntryFerment.SelectedItem.ToString().Split('|')[0].Trim(),
                            Buyer = tbBuyerName.Text,
                            InventoryID = tbEntryInv.Text,
                            Process = tempProcess,
                            Stage = (tempProcess == "PC") ? "" : cbEntryStage.SelectedItem.ToString().Split('|')[0].Trim(),
                            Grade = cbEntryGrade.SelectedItem.ToString().Split('|')[0].Trim(),
                            Color = cbEntryColor.SelectedItem.ToString().Split('|')[0].Trim(),
                            Weight = tbEntryWeightNetto.Text,
                            Length = cbEntryLength.SelectedItem.ToString().Split('|')[0].Trim(),
                            Warehouse = tbWarehouse.Text,
                            Date = tbProcessDate.Text,
                            Remark = tbEntryRemark.Text,
                            Area = cbEntryArea.SelectedItem.ToString().Split('|')[0].Trim(),
                            QRImage = QRImage,
                            StrCrop = StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(0, 1))) + StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(1, 1))),
                            Forms = (tempProcess =="PC" ) ? "" : cbEntryForm.Text.Substring(0,2)
                        };
                        lotPrint.ShowDialog();
                    }
                    else if (optLblStickerSimple.Checked)
                    {
                        PackLotPrint lotPrint = new PackLotPrint
                        {
                            LotNumber = tbEntryLot.Text,
                            Source = cbEntrySource.SelectedItem.ToString().Split('|')[0].Trim(),
                            StalkPos = cbEntryStalk.SelectedItem.ToString().Split('|')[0].Trim(),
                            Ferment = cbEntryFerment.SelectedItem.ToString().Split('|')[0].Trim(),
                            Buyer = tbBuyerName.Text,
                            InventoryID = tbEntryInv.Text,
                            Process = tempProcess,
                            Stage = (tempProcess == "PC") ? "" : cbEntryStage.SelectedItem.ToString().Split('|')[0].Trim(),
                            Grade = cbEntryGrade.SelectedItem.ToString().Split('|')[0].Trim(),
                            Color = cbEntryColor.SelectedItem.ToString().Split('|')[0].Trim(),
                            Weight = tbEntryWeightNetto.Text,
                            Length = cbEntryLength.SelectedItem.ToString().Split('|')[0].Trim(),
                            Warehouse = tbWarehouse.Text,
                            Date = tbProcessDate.Text,
                            Remark = tbEntryRemark.Text,
                            Area = cbEntryArea.SelectedItem.ToString().Split('|')[0].Trim(),
                            QRImage = QRImage,
                            StrCrop = StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(0, 1))) + StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(1, 1)))
                        };
                        lotPrint.ShowDialog();
                    }

                    else
                    {
                        GenericLotPrintThermal lotPrint = new GenericLotPrintThermal
                        {
                            LotNumber = tbEntryLot.Text,
                            Source = cbEntrySource.SelectedItem.ToString().Split('|')[0].Trim(),
                            StalkPos = cbEntryStalk.SelectedItem.ToString().Split('|')[0].Trim(),
                            Ferment = cbEntryFerment.SelectedItem.ToString().Split('|')[0].Trim(),
                            Buyer = tbBuyerName.Text,
                            InventoryID = tbEntryInv.Text,
                            Process = tempProcess,
                            Stage = (tempProcess == "PC") ? "" : cbEntryStage.SelectedItem.ToString().Split('|')[0].Trim(),
                            Grade = cbEntryGrade.SelectedItem.ToString().Split('|')[0].Trim(),
                            Color = cbEntryColor.SelectedItem.ToString().Split('|')[0].Trim(),
                            Weight = tbEntryWeightNetto.Text,
                            Length = cbEntryLength.SelectedItem.ToString().Split('|')[0].Trim(),
                            Warehouse = tbWarehouse.Text,
                            Date = tbProcessDate.Text,
                            Remark = tbEntryRemark.Text,
                            Area = cbEntryArea.SelectedItem.ToString().Split('|')[0].Trim(),
                            QRImage = QRImage,
                            StrCrop = StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(0, 1))) + StrCrop(Convert.ToInt32(cbEntryCropYear.Text.Substring(1, 1))),
                            Forms = (tempProcess == "PC") ? "" : cbEntryForm.Text.Substring(0,2) 
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

        private void btnShrinkBalance_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Sesuaikan berat yang belum diterapkan sebagai penyusutan", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (checkDocumentSync(cbRefIN.SelectedItem.ToString()))
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        //Update Unapplied balance on issue
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Update_ProcessingLineIN_ShrinkBalance", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", cbRefIN.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@Process", tempProcess);
                                command.Parameters.AddWithValue("@UnappliedBalance", tempUnappliedBalance);

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Update issue balance error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        calcMaterialUse();
                        saveDocument();
                    }
                }
                else
                {
                    MessageBox.Show($"Processing IN document is still open, you can't adjust shrinkage.\nPlease close (sync) IN document first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
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
	                                        ProcessingLineOUTDetail
	                                    WHERE
		                                    ProcessingLineOUTDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        ProcessingLineOUTDetail.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.ProcessingLineOUTDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            GenericOUTProcessDocPrint genericOUTProcessDocPrint = new GenericOUTProcessDocPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = tempProcessDescr.ToUpper(),
                DocDate = tbProcessDate.Text,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc
            };
            genericOUTProcessDocPrint.ShowDialog();
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
            DataSetAddon myDoc = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
                                            d.DocumentID,
											d.InventoryID,
											d.SubItem,
											d.Color,
											d.Fermentation,
											d.Length,
											d.StalkPosition,
											COUNT(d.LotNbr) AS WeightRope,
											SUM(d.WeightNetto) as WeightNetto,
											d.Remark
                                        FROM
	                                        ProcessingLineOUTDetail d
	                                    WHERE
		                                    d.DocumentID = '{DocNumber}'
										GROUP BY
                                            d.DocumentID,
											d.InventoryID,
											d.SubItem,
											d.Color,
											d.Fermentation,
											d.Length,
											d.StalkPosition,
											d.Remark
                                        ORDER BY
                                            d.DocumentID,
											d.InventoryID,
											d.SubItem,
											d.Color,
											d.Fermentation,
											d.Length,
											d.StalkPosition,
											d.Remark";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.ProcessingLineOUTDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            GenericOUTProcessDocSumPrint genericOUTProcessDocSumPrint = new GenericOUTProcessDocSumPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "Summary " + tempProcessDescr.ToUpper(),
                DocDate = tbProcessDate.Text,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc
            };
            genericOUTProcessDocSumPrint.ShowDialog();

        }

        private void chkLblPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLblPrint.Checked)
            {
                optLblSticker.Enabled = true;
                if (tempProcess == "PC") 
                {
                    optLblStickerSimple.Enabled = true;
                }
                optLblThermal.Enabled = true;
            }
            else
            {
                optLblSticker.Enabled = false;
                optLblThermal.Enabled = false;
            }
        }

        private void cbRefIN_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbInventory_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbNotes_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryColor_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryFerment_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbEntryLength_KeyPress(object sender, KeyPressEventArgs e)
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

        private void checkZeroCost_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbEntryRemark_KeyPress(object sender, KeyPressEventArgs e)
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

                                using (SqlCommand command = new SqlCommand("Update_ProcessingLineOUTDetail_Sync", connection))
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

        private string StrCrop(int mnumber)
        {
            switch (mnumber)
            {
                case 1:
                    return "A";
                    break;
                case 2:
                    return "B";
                    break;
                case 3:
                    return "C";
                    break;
                case 4:
                    return "D";
                    break;
                case 5:
                    return "E";
                    break;
                case 6:
                    return "F";
                    break;
                case 7:
                    return "G";
                    break;
                case 8:
                    return "H";
                    break;
                case 9:
                    return "I";
                    break;
                case 0:
                    return "J";
                    break;

                default:
                    return "";
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx|Excel 97-2003 Workbook|*.xls|Excel CSV |*.csv" }){
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
                    catch(Exception ex) {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message+" ( Excel Jangan Dibuka OKE )");
                    }
                   

                }
            }
        }

        private void cbosheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDetailImport = dtImport[cbosheet.SelectedItem.ToString()];
            SaveImport.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {

            SqlTransaction objTrans = null;
            try
            {
                if (Convert.ToDecimal(dtDetailImport.Compute("SUM(WeightNetto)", string.Empty)) <= Convert.ToDecimal(tbUnappliedBalance.Text))
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
                                    row["Color"].ToString().Trim() != null &&
                                    row["Fermentation"].ToString().Trim() != null &&
                                    row["Length"].ToString().Trim() != null &&
                                    row["StalkPosition"].ToString().Trim() != null &&
                                    row["WeightRope"].ToString().Trim() != null &&
                                    row["WeightShipping"].ToString().Trim() != null &&
                                    row["WeightReceive"].ToString().Trim() != null &&
                                    row["WeightTare"].ToString().Trim() != null &&
                                    row["WeightNetto"].ToString().Trim() != null &&
                                    row["UoM"].ToString().Trim() != null &&
                                    row["ZeroCost"].ToString().Trim() != null &&
                                    row["BuyerName"].ToString().Trim() != null

                                    )
                                {

                                    string InventoryID = row["InventoryID"].ToString().Trim();
                                    string Source = row["Source"].ToString().Trim();
                                    string Stage = row["Stage"].ToString().Trim();
                                    string Form = row["Form"].ToString().Trim();
                                    string CropYear = row["CropYear"].ToString().Trim();
                                    string Grade = row["Grade"].ToString().Trim();
                                    string Area = row["Area"].ToString().Trim();
                                    string Color = row["Color"].ToString().Trim();
                                    string Fermentation = row["Fermentation"].ToString().Trim();
                                    string Length = row["Length"].ToString().Trim();
                                    string StalkPosition = row["StalkPosition"].ToString().Trim();
                                    string WeightRope = row["WeightRope"].ToString().Trim();
                                    string WeightShipping = row["WeightShipping"].ToString().Trim();
                                    string WeightReceive = row["WeightReceive"].ToString().Trim();
                                    string WeightTare = row["WeightTare"].ToString().Trim();
                                    string WeightNetto = row["WeightNetto"].ToString().Trim();
                                    string UoM = row["UoM"].ToString().Trim();
                                    string Remark = "IMPORT PROSESS OUT";
                                    string ZeroCostWeightNetto = row["ZeroCost"].ToString().Trim();
                                    string BuyerName = row["BuyerName"].ToString().Trim();
                                    string SubItem = $"{Stage}.{Form}.{CropYear}.{Grade}";

                                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineOUTDetail_v2_Import", connection, objTrans))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                        command.Parameters.AddWithValue("@InventoryID", InventoryID);
                                        command.Parameters.AddWithValue("@SubItem", SubItem);
                                        command.Parameters.AddWithValue("@LotNbr", $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-".ToString());
                                        command.Parameters.AddWithValue("@Source", Source);
                                        command.Parameters.AddWithValue("@Stage", Stage);
                                        command.Parameters.AddWithValue("@Form", Form);
                                        command.Parameters.AddWithValue("@CropYear", CropYear);
                                        command.Parameters.AddWithValue("@Grade", Grade);
                                        command.Parameters.AddWithValue("@Area", Area);
                                        command.Parameters.AddWithValue("@Color", Color);
                                        command.Parameters.AddWithValue("@Fermentation", Fermentation);
                                        command.Parameters.AddWithValue("@Length", Length);
                                        command.Parameters.AddWithValue("@Process", tempProcess);
                                        command.Parameters.AddWithValue("@StalkPosition", StalkPosition);
                                        command.Parameters.AddWithValue("@WeightRope", Convert.ToDecimal(WeightRope));
                                        command.Parameters.AddWithValue("@WeightShipping", Convert.ToDecimal(WeightShipping));
                                        command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(WeightReceive));
                                        command.Parameters.AddWithValue("@WeightTare", Convert.ToDecimal(WeightTare));
                                        command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(WeightNetto));
                                        command.Parameters.AddWithValue("@UoM", UoM);
                                        command.Parameters.AddWithValue("@Remark", Remark);
                                        command.Parameters.AddWithValue("@OldDocumentID", cbRefIN.SelectedItem.ToString() ?? "");
                                        command.Parameters.AddWithValue("@SyncDetail", 0);
                                        command.Parameters.AddWithValue("@ZeroCost", ZeroCostWeightNetto);
                                        command.Parameters.AddWithValue("@BuyerName", BuyerName);
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
                            loadProcessOUT();
                            loadDetail();
                            resetEntry();
                            dtImport.Clear();
                            cbosheet.Items.Clear();
                            cbosheet.Text="";
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
                    MessageBox.Show($"Total WeightNetto melebihi Unapplied Balance !", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"--{ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
         


        }
    }
}