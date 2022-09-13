using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using Acumatica.ULT_18_200_001.Api;
using Acumatica.ULT_18_200_001.Model;
using QRCoder;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace ScaleAddon.Forms
{
    public partial class ReturnBuying : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }

        public string tempProcess { get; set; }
        public string tempProcessDescr { get; set; }
        public String AcumaticaReasonCode { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }

        private int lastIncrementValue = -1;

        private DataTable dtLot;
        private DataTable dtDetail;
        private DataTable dtEntry;

        private string LotStockItem = null;
        private string tempBuyerName;


        private Decimal tempUnitPrice;
        private Decimal tempMaterialIN;
        private Decimal tempUnappliedBalance;
        private Decimal tempMaterialUse;

        //private string AcumaticaRefNbr;

        public ReturnBuying()
        {
            InitializeComponent();
        }

        private void ReturnGenericINProcess_Load(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
                resetEntry();
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

        private void resetScreen()
        {
            currentDate = DateTime.Now;
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");

            DocNumber = "<NEW>";
            //AcumaticaRefNbr = "";
            checkHold.Checked = true;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbTotalCost.Text = "0";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaRefNbr.Text = "";
            tbBuyerName.Text = "";
            tbNotes.Text = "";
            cbRefIN.Text = "";

            loadComboLot();
            resetEntry();
        }

        private void resetEntry()
        {
            loadDetail();

            groupEntry.Text = "Lot Entry [<NEW>]";

            tbEntryLot.Text = "";
            tbEntryInv.Text = "";
            tbEntryArea.Text = "";
            tbEntryGrade.Text = "";
            tbEntrySubItem.Text = "";
            tbEntryWeightReceive.Text = "";
            tbEntryWeightTare.Text = "";
            tbEntryWeightNetto.Text = "";
            tbEntryDate.Text = "";

            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = true;
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = true;
                    btnPrintSUM.Enabled = true;

                    break;

                case "SYNCED":
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintDoc.Enabled = true;
                    btnPrintSUM.Enabled = true;

                    break;

                default:
                    btnAcumatica.Enabled = false;
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintDoc.Enabled = false;
                    btnPrintSUM.Enabled = false;

                    break;
            }
        }

        private void loadProcess()
        {
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";

            //loadComboLot();

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from BuyingLineIN_Return where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(3).ToString();
                        tbTotalCost.Text = Convert.ToDecimal(reader.GetValue(4)).ToString("N2");
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(6).ToString();
                        tbBuyerName.Text = reader.GetValue(7).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(3).ToString() == "HOLD" ? 1 : 0);
                        tbNotes.Text = reader.GetValue(11).ToString();
                        cbRefIN.Text = reader.GetValue(12).ToString();
                        cbRefIN.ReadOnly = true;
                        cbRefIN.BackColor = System.Drawing.SystemColors.Info;
                        POOrderNbr.Text = reader.GetValue(16).ToString();
                        POOrderType.Text = reader.GetValue(15).ToString();
                        NoKontrak.Text = reader.GetValue(14).ToString();
                        VendorID.Text = reader.GetValue(13).ToString();
                        tbOldAcumaticaRefNbr.Text = reader.GetValue(17).ToString();
                        loadComboLot();
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
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        BuyingLineIN_Return_Detail
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
                    dgvDetail.Columns[24].HeaderText = "Buyer Name";
                    dgvDetail.Columns[24].Visible = false;
                    dgvDetail.Columns[25].HeaderText = "Lot Grouping";
                    dgvDetail.Columns[26].HeaderText = "Lot Entry Date";

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

                        //tbBuyerName.ReadOnly = false;
                        //tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                        tbBuyerName.Text = "";
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

                    string query = $@"SELECT *
                                        FROM
	                                        StockItem
                                        WHERE
	                                        LotNbr = '{LotNbr}'
                                        AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);
                    tbEntryLot.Text = dtEntry.Rows[0].ItemArray[3].ToString();
                    tbEntryInv.Text = dtEntry.Rows[0].ItemArray[1].ToString();
                    tbEntrySubItem.Text = dtEntry.Rows[0].ItemArray[2].ToString();
                    tbEntryGrade.Text = dtEntry.Rows[0].ItemArray[8].ToString();
                    tbEntryArea.Text = dtEntry.Rows[0].ItemArray[9].ToString();
                    tbEntryWeightReceive.Text = dtEntry.Rows[0].ItemArray[17].ToString();
                    tbEntryWeightTare.Text = dtEntry.Rows[0].ItemArray[18].ToString();
                    tbEntryWeightNetto.Text = dtEntry.Rows[0].ItemArray[19].ToString();

                    tempBuyerName = dtEntry.Rows[0].ItemArray[24].ToString();
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

                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempProcess}/IN-{currentIncrement.ToString().PadLeft(4, '0')}";

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
                    string query = $"IF EXISTS ( SELECT * FROM ProcessingLineIN WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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
                    string query = $"select * from NumberingSetting where NumberingID = '{tempProcess}/IN'";
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
	                                    LotNbr
                                    FROM
	                                    StockItem
                                    WHERE
	                                    DocumentID = '{cbRefIN.Text}' and StatusStock = 1";

                    //now can select teh smae process for input

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
                }


            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from BuyingRegistration where BuyingUse = '{cbRefIN.Text}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        POOrderNbr.Text = reader.GetValue(2).ToString();
                        POOrderType.Text = reader.GetValue(6).ToString();
                        NoKontrak.Text = reader.GetValue(7).ToString();
                        VendorID.Text = reader.GetValue(1).ToString();

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
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



                        using (SqlCommand command = new SqlCommand("Insert_Buying_v2_Return", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbBuyingDate.Text);
                            command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@TotalCost", tbTotalCost.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalWeight", tbDetailWNetto.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@DocumentRef", cbRefIN.Text);
                            command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                            command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text);
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                            command.Parameters.AddWithValue("@Notes", tbNotes.Text);
                            command.Parameters.AddWithValue("@OperationType", OperationType);
                            command.Parameters.AddWithValue("@VendorID", VendorID.Text);
                            command.Parameters.AddWithValue("@NoKontrak", NoKontrak.Text);
                            command.Parameters.AddWithValue("@POOrderType", POOrderType.Text);
                            command.Parameters.AddWithValue("@POOrderNbr", POOrderNbr.Text);
                            command.Parameters.AddWithValue("@OldAcumaticaRefNbr", tbOldAcumaticaRefNbr.Text);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        insertError = true;
                        tbDocNumber.Text = "<NEW>";
                    }
                    finally
                    {
                        if (!insertError)
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Return {tempProcessDescr} [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/IN");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }

                            //MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        if (!insertError && this.Text.Contains("<NEW>"))
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} Buying [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/IN");
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
                btnPrintSUM.Enabled = true;
            }
            else
            {
                btnPrintDoc.Enabled = false;
                btnPrintSUM.Enabled = false;
            }

            loadComboLot();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           

            if (tbDocNumber.Text == "<NEW>")
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {

                    try
                    {

                        string query = $"select * from BuyingLine where DocumentID = '{cbRefIN.Text}'";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        Console.WriteLine(reader.HasRows);
                        if (reader.HasRows)
                        {
                            reader.Read();
                            Console.WriteLine(reader.GetValue(11).ToString().Trim() == "");
                            Console.WriteLine(reader.GetValue(11).ToString().Trim());
                            if (reader.GetValue(9).ToString() == "SYNCED" && reader.GetValue(11).ToString().Trim() == "")
                            {
                                tbOldAcumaticaRefNbr.Text = reader.GetValue(10).ToString().Trim();

                                saveDocument();
                                loadDetail();
                                resetEntry();
                                cbRefIN.ReadOnly = true;
                                cbRefIN.BackColor = System.Drawing.SystemColors.Info;
                            }
                            else
                            {
                                MessageBox.Show($"Status Doc {cbRefIN.Text} masih {reader.GetValue(9).ToString()}, Hanya dapat di jika status Synced. \nAtau \nInvoice Telah Terbuat ( {reader.GetValue(11).ToString()} )");
                            }
                        }
                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(a.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else if (tbStatus.Text == "OPEN")
            {
                this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}] - Syncing with Acumatica, please wait!";


                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);

                try
                {
                    var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                    var PurchaseReceipt = new PurchaseReceiptApi(configuration);
                    var PurchaseReceiptDetail = PurchaseReceipt.GetList(filter: $"ReceiptNbr eq '{tbOldAcumaticaRefNbr.Text}' and Status eq 'R'", expand: "Details", select: "VendorRef,Details/LineNbr,Details/TransactionDescription");


                    //insert vendor PO
                    using (SqlConnection connection2 = new SqlConnection(ConnectionString))
                    {
                        foreach (var purchasePurchaseDetail in PurchaseReceiptDetail[0].Details)
                        {
                            if (purchasePurchaseDetail.TransactionDescription.Value.ToString().Trim() != "")
                            {
                                //insert vendorPO
                                try
                                {
                                    if (connection2.State != ConnectionState.Open)
                                    {
                                        connection2.Open();
                                    }

                                    using (SqlCommand command = new SqlCommand("Update_BuyingLineDetail_Line_Nbr_Return", connection2))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                        command.Parameters.AddWithValue("@LotNbr", purchasePurchaseDetail.TransactionDescription.Value.ToString());
                                        command.Parameters.AddWithValue("@LineNbr", purchasePurchaseDetail.LineNbr.Value.ToString());

                                        command.ExecuteNonQuery();

                                        saveDocument();
                                        loadDetail();
                                        resetEntry();
                                    }
                                }
                                catch (Exception e_update)
                                {
                                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                //insert PO detail

                            }
                        }
                    }


                }
                catch (Exception az)
                {
                    authApi.AuthLogout();
                }
                finally
                {
                    authApi.AuthLogout();

                }


            }
            else
            {
                saveDocument();
                loadDetail();
                resetEntry();

            }






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
            if (CheckInvoiceDoc()) {
                DataTable dtEntrySave = new DataTable();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //create new dt

                    try
                    {
                        //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                        string query = $@"SELECT *
                                        FROM
	                                        BuyingLineDetail
                                        WHERE
	                                        LotNbr = '{tbEntryLot.Text}'
                                        AND SyncDetail = 1 and StatusReject = 0";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtEntrySave);


                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }


                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_BuyingLineDetail_v3_Return", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            Console.WriteLine(DocNumber);
                            command.Parameters.AddWithValue("@DocumentID", DocNumber);
                            command.Parameters.AddWithValue("@InventoryID", dtEntrySave.Rows[0].ItemArray[1].ToString());
                            command.Parameters.AddWithValue("@SubItem", dtEntrySave.Rows[0].ItemArray[2].ToString());
                            command.Parameters.AddWithValue("@LotNbr", dtEntrySave.Rows[0].ItemArray[3].ToString());
                            command.Parameters.AddWithValue("@Source", dtEntrySave.Rows[0].ItemArray[4].ToString());
                            command.Parameters.AddWithValue("@Stage", dtEntrySave.Rows[0].ItemArray[5].ToString());
                            command.Parameters.AddWithValue("@tForm", dtEntrySave.Rows[0].ItemArray[6].ToString());
                            command.Parameters.AddWithValue("@CropYear", dtEntrySave.Rows[0].ItemArray[7].ToString());
                            command.Parameters.AddWithValue("@Grade", dtEntrySave.Rows[0].ItemArray[8].ToString());
                            command.Parameters.AddWithValue("@Area", dtEntrySave.Rows[0].ItemArray[9].ToString());
                            command.Parameters.AddWithValue("@StalkPosition", dtEntrySave.Rows[0].ItemArray[10].ToString());
                            command.Parameters.AddWithValue("@WeightRope", dtEntrySave.Rows[0].ItemArray[11].ToString());
                            command.Parameters.AddWithValue("@WeightShipping", dtEntrySave.Rows[0].ItemArray[12].ToString());
                            command.Parameters.AddWithValue("@WeightReceive", dtEntrySave.Rows[0].ItemArray[13].ToString());
                            command.Parameters.AddWithValue("@WeightTare", dtEntrySave.Rows[0].ItemArray[14].ToString());
                            command.Parameters.AddWithValue("@WeightNetto", dtEntrySave.Rows[0].ItemArray[15].ToString());
                            command.Parameters.AddWithValue("@UoM", dtEntrySave.Rows[0].ItemArray[16].ToString());
                            command.Parameters.AddWithValue("@CostUnit", dtEntrySave.Rows[0].ItemArray[17].ToString());
                            command.Parameters.AddWithValue("@CostGross", dtEntrySave.Rows[0].ItemArray[18].ToString());
                            command.Parameters.AddWithValue("@NTRM", dtEntrySave.Rows[0].ItemArray[19].ToString());
                            command.Parameters.AddWithValue("@CostNTRM", dtEntrySave.Rows[0].ItemArray[20].ToString());
                            command.Parameters.AddWithValue("@CostNett", dtEntrySave.Rows[0].ItemArray[21].ToString());
                            command.Parameters.AddWithValue("@MC", dtEntrySave.Rows[0].ItemArray[22].ToString());
                            command.Parameters.AddWithValue("@Remark", dtEntrySave.Rows[0].ItemArray[23].ToString());
                            command.Parameters.AddWithValue("@StatusReject", dtEntrySave.Rows[0].ItemArray[24].ToString());
                            command.Parameters.AddWithValue("@SyncDetail", 0);
                            command.Parameters.AddWithValue("@OrderNbr", dtEntrySave.Rows[0].ItemArray[26].ToString());
                            command.Parameters.AddWithValue("@NoKontrak", dtEntrySave.Rows[0].ItemArray[27].ToString());
                            command.Parameters.AddWithValue("@BuyerName", dtEntrySave.Rows[0].ItemArray[28].ToString());
                            command.Parameters.AddWithValue("@GradeDraft", dtEntrySave.Rows[0].ItemArray[29].ToString());
                            command.Parameters.AddWithValue("@OperationType", dtEntrySave.Rows[0].ItemArray[30].ToString());
                            command.Parameters.AddWithValue("@Residue", dtEntrySave.Rows[0].ItemArray[31].ToString());
                            command.Parameters.AddWithValue("@OldDocumentID", dtEntrySave.Rows[0].ItemArray[0].ToString());
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        tbBuyerName.Text = tempBuyerName;
                        //MessageBox.Show("Save lot complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //resetEntry();
                        loadDetail();
                        groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                        cbLot.SelectedIndex = -1;
                        loadComboLot();
                    }
                }
            }
        
        }

        public bool CheckInvoiceDoc() {

            bool statusINV=false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                try
                {

                    string query = $"select * from BuyingLine where DocumentID = '{cbRefIN.Text}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(9).ToString() == "SYNCED" && reader.GetValue(11).ToString().Trim() == "")
                        {
                            statusINV = true;
                        }
                        else
                        {
                            MessageBox.Show($"Status Doc {cbRefIN.Text} masih {reader.GetValue(9).ToString()}, Hanya dapat di jika status Synced. \nAtau \nInvoice Telah Terbuat ( {reader.GetValue(11).ToString()} )");


                        }
                    }
                }
                catch (Exception a)
                {
                    statusINV= false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return statusINV;
        }

        private void removeLot()
        {
            if (CheckInvoiceDoc())
            {

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Delete_BuyingLineINDetail", connection))
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

                if (tbLot.Visible)
                {
                    tbLot.Focus();
                }
                resetEntry();
            }
        }


        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                groupEntry.Text = $"Lot Entry [{dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString()}]";

                tbEntryLot.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
                tbEntryInv.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                tbEntrySubItem.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[2].FormattedValue.ToString();
                tbEntryGrade.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[8].FormattedValue.ToString();
                tbEntryArea.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[9].FormattedValue.ToString();

                tbEntryWeightReceive.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[13].Value.ToString();
                tbEntryWeightTare.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[14].Value.ToString();
                tbEntryWeightNetto.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[15].Value.ToString();
                tbEntryLotGroup.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[25].Value.ToString();
                tbEntryDate.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[26].Value.ToString();

                if (tbStatus.Text == "SYNCED")
                {
                    btnDelLot.Enabled = false;
                }
                else
                {
                    btnDelLot.Enabled = true;
                }
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

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            string referenceNbr = "";
            var docBranch = GetBranch(tbWarehouse.Text, 2);

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                try
                {

                    string query = $"select * from BuyingLine where DocumentID = '{cbRefIN.Text}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        if (reader.GetValue(9).ToString() == "SYNCED" && reader.GetValue(11).ToString().Trim() == "")
                        {
                            
                        }
                        else
                        {
                            syncError = true;
                            MessageBox.Show($"Status Doc {cbRefIN.Text} masih {reader.GetValue(9).ToString()}, Hanya dapat di jika status Synced. \nAtau \nInvoice Telah Terbuat ( {reader.GetValue(11).ToString()} )");
                        }
                    }
                }
                catch (Exception a)
                {
                    MessageBox.Show(a.ToString());
                }
                finally
                {
                    connection.Close();
                }
            }

            loadDetail();


            DataView dv_filter = new DataView(dtDetail, $"StatusReject = 0 and SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

            PurchaseReceipt purchaseReceipt = new PurchaseReceipt();
            //header receipt
            //purchaseReceipt.Branch = Warehouse.WarehouseID;
            purchaseReceipt.Branch = docBranch;
            purchaseReceipt.Type = "Return";
            purchaseReceipt.VendorID = VendorID.Text;
            purchaseReceipt.VendorRef = DocNumber;
            purchaseReceipt.Date = Convert.ToDateTime(tbBuyingDate.Text);
            purchaseReceipt.Status = "Balanced";
            purchaseReceipt.Hold = false;
            //purchaseReceipt.CostStrategi = "I";

            List<PurchaseReceiptDetail> listDetail = new List<PurchaseReceiptDetail>();

            //document details
            foreach (DataRowView rowView in dv_filter)
            {

                PurchaseReceiptDetail detail = new PurchaseReceiptDetail();
              

                    detail.POReceiptNbr = tbOldAcumaticaRefNbr.Text;
                    detail.POReceiptLineNbr = Convert.ToInt32(rowView[33].ToString());

                    listDetail.Add(detail);
             



            }
            if (!syncError)
            {
                purchaseReceipt.Details = listDetail;
            
                try
                {

                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);

              
                try
                {
                    var configuration = LogIn2(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);

                    var purchaseReceiptApi = new PurchaseReceiptApi(configuration);

                    //Console.Write(purchaseReceipt);
                    var response = purchaseReceiptApi.PutEntity(purchaseReceipt);
                    //Console.Write((PurchaseReceipt)response);
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

                                    using (SqlCommand command = new SqlCommand("Update_BuyingLineDetail_Sync_Return", connection))
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
            catch (Exception aa)
            {
                MessageBox.Show($"--Sync error! {aa.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            }
        }


        private Configuration LogIn2(AuthApi authApi, string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
        {

            var cookieContainer = new CookieContainer();
            authApi.Configuration.ApiClient.RestClient.CookieContainer = cookieContainer;

            authApi.AuthLogin(new Credentials(username, password, tenant, branch, locale));
            //Console.WriteLine("Logged In...");
            var configuration = new Configuration($"{AcumaticaCred.AcumaticaSiteURL}/entity/{AcumaticaCred.AcumaticaEndpointName}/20.200.001/");

            //share cookie container between API clients because we use different client for authentication and interaction with endpoint
            configuration.ApiClient.RestClient.CookieContainer = authApi.Configuration.ApiClient.RestClient.CookieContainer;
            return configuration;
        }
        private Configuration LogIn(AuthApi authApi, string siteURL, string username, string password, string tenant = null, string branch = null, string locale = null)
        {

            var cookieContainer = new CookieContainer();
            authApi.Configuration.ApiClient.RestClient.CookieContainer = cookieContainer;

            authApi.AuthLogin(new Credentials(username, password, tenant, branch, locale));
            //Console.WriteLine("Logged In...");
            var configuration = new Configuration($"{AcumaticaCred.AcumaticaSiteURL}/entity/{AcumaticaCred.AcumaticaEndpointName}/{AcumaticaCred.AcumaticaEndpointVersion}/");

            //share cookie container between API clients because we use different client for authentication and interaction with endpoint
            configuration.ApiClient.RestClient.CookieContainer = authApi.Configuration.ApiClient.RestClient.CookieContainer;
            return configuration;
        }

        private String checkReleasedIssue(string referenceNbr, Configuration configuration)
        {
            var inventoryIssueApi = new ULTInventoryIssueApi(configuration);

            var response = inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr });

            if (response.Status == "Released")
            {
                var TotalCost = (decimal)response.TotalCost.Value;
                tbTotalCost.Text = TotalCost.ToString("N2");
            }
            else
            {

            }

            return response.Status;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetScreen();
        }

        private void tbBuyerName_TextChanged(object sender, EventArgs e)
        {
            tempBuyerName = ((TextBox)sender).Text;
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
	                                        BuyingLineIN_Return_Detail
	                                    WHERE
		                                    BuyingLineIN_Return_Detail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        BuyingLineIN_Return_Detail.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.ProcessingLineINDetail);

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

            GenericINProcessDocPrint genericINProcessDocPrint = new GenericINProcessDocPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = tempProcessDescr.ToUpper(),
                DocDate = tbBuyingDate.Text,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc,
                QRImage = QRImage
            };
            genericINProcessDocPrint.ShowDialog();
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
	                                        BuyingLineIN_Return_Detail.DocumentID,
	                                        BuyingLineIN_Return_Detail.InventoryID, 
	                                        BuyingLineIN_Return_Detail.SubItem, 
	                                        SegmentValue.Descr AS Stage, 
	                                        BuyingLineIN_Return_Detail.Color , 
											BuyingLineIN_Return_Detail.Fermentation,
											BuyingLineIN_Return_Detail.Length,
											BuyingLineIN_Return_Detail.StalkPosition,
	                                        COUNT(BuyingLineIN_Return_Detail.LotNbr) as WeightRope, 
	                                        SUM(BuyingLineIN_Return_Detail.WeightNetto) as WeightNetto 
                                        FROM
	                                        dbo.BuyingLineIN_Return_Detail
	                                        INNER JOIN
	                                        dbo.ProcessingLineIN
	                                        ON 
		                                        BuyingLineIN_Return_Detail.DocumentID = ProcessingLineIN.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        BuyingLineIN_Return_Detail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
										WHERE BuyingLineIN_Return_Detail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        BuyingLineIN_Return_Detail.DocumentID,
	                                        BuyingLineIN_Return_Detail.InventoryID, 
	                                        BuyingLineIN_Return_Detail.SubItem, 
	                                        SegmentValue.Descr , 
	                                        BuyingLineIN_Return_Detail.Color , 
											BuyingLineIN_Return_Detail.Fermentation,
											BuyingLineIN_Return_Detail.Length,
											BuyingLineIN_Return_Detail.StalkPosition";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(myDoc.ProcessingLineINDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }


            GenericINProcessDocSumPrint genericINProcessDocSumPrint = new GenericINProcessDocSumPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "Summary " + tempProcessDescr.ToUpper(),
                DocDate = tbBuyingDate.Text,
                DocStatus = tbStatus.Text,
                DocDetails = myDoc,
            };
            genericINProcessDocSumPrint.ShowDialog();

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

        private void tbEntryLotGroup_KeyPress(object sender, KeyPressEventArgs e)
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




        private void panel1_Paint(object sender, PaintEventArgs e)
        {


        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if (tbStatus.Text == "HOLD")
            {

                checkHold.Checked = false;
                tbStatus.Text = "HOLD";
            }
            else
            {

                tbStatus.Text = "OPEN";
                checkHold.Checked = true;

            }
        }
    }
}