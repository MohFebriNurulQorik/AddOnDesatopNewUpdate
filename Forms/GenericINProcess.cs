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
using ExcelDataReader;
using System.Text.Json;

namespace ScaleAddon.Forms
{
    public partial class GenericINProcess : Form
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

        private string LotStockItem=null;
        private string tempBuyerName;

        private DataTableCollection dtImport;
        private DataTable dtDetailImport;

        List<dynamic> MapArray = new List<dynamic>();

        //private string AcumaticaRefNbr;

        public GenericINProcess()
        {
            InitializeComponent();
        }

        private void GenericINProcess_Load(object sender, EventArgs e)
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
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
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
                    if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
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
        }

        private void loadProcess()
        {
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
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
                    string query = $"select * from ProcessingLineIN where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(3).ToString();
                        tbTotalCost.Text = Convert.ToDecimal(reader.GetValue(4)).ToString("N2");
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(8).ToString();
                        //AcumaticaRefNbr = reader.GetValue(8).ToString();
                        tbBuyerName.Text = reader.GetValue(9).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(3).ToString() == "HOLD" ? 1 : 0);
                        tbNotes.Text = reader.GetValue(13).ToString();

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
	                                        ProcessingLineINDetail
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
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-{tempProcess}/IN-{currentIncrement.ToString().PadLeft(4, '0')}";
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
                    //string query = $@"SELECT
	                   //                 StockItem.LotNbr
                    //                FROM
	                   //                 dbo.StockItem
                    //                WHERE
	                   //                 StockItem.StatusStock = 1
                    //                AND
                    //                    StockItem.Process != '{tempProcess}'
                    //                AND BuyerName like '%{tempBuyerName}%'";

                    //now can select teh smae process for input
                    string query = $@"SELECT
	                                    StockItem.LotNbr
                                    FROM
	                                    dbo.StockItem
                                    WHERE
	                                    StockItem.StatusStock = 1";
                                    //AND BuyerName like '%{tempBuyerName}%'";

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

                        using (SqlCommand command = new SqlCommand("Insert_ProcessingLineIN_v2", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbProcessDate.Text);
                            command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@TotalCost", tbTotalCost.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalWeight", tbDetailWNetto.Text.Replace(",", ""));
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
                    finally
                    {
                        //if (!insertError)
                        //{
                        //    this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";

                        //    using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                        //    {
                        //        command.CommandType = CommandType.StoredProcedure;
                        //        command.Parameters.AddWithValue("@NumberingID", $"{tempProcess}/IN");
                        //        command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                        //        command.Parameters.AddWithValue("@NumberingDate", currentDate);

                        //        command.ExecuteNonQuery();
                        //    }

                        //    //MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //}
                    }

                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        if (!insertError && this.Text.Contains("<NEW>"))
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";

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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineINDetail", connection))
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
                        command.Parameters.AddWithValue("@WeightShipping", dtEntry.Rows[0].ItemArray[16].ToString());
                        command.Parameters.AddWithValue("@WeightReceive", dtEntry.Rows[0].ItemArray[17].ToString());
                        command.Parameters.AddWithValue("@WeightTare", dtEntry.Rows[0].ItemArray[18].ToString());
                        command.Parameters.AddWithValue("@WeightNetto", dtEntry.Rows[0].ItemArray[19].ToString());
                        command.Parameters.AddWithValue("@UoM", dtEntry.Rows[0].ItemArray[20].ToString());
                        command.Parameters.AddWithValue("@Remark", dtEntry.Rows[0].ItemArray[21].ToString());
                        command.Parameters.AddWithValue("@OldDocumentID", dtEntry.Rows[0].ItemArray[0].ToString());
                        command.Parameters.AddWithValue("@SyncDetail", 0);
                        command.Parameters.AddWithValue("@BuyerName", tempBuyerName ?? "");
                        command.Parameters.AddWithValue("@LotGroup", tbEntryLotGroup.Text);

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

                    using (SqlCommand command = new SqlCommand("Delete_ProcessingLineINDetail", connection))
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

                tbEntryWeightReceive.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[17].Value.ToString();
                tbEntryWeightTare.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[18].Value.ToString();
                tbEntryWeightNetto.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[19].Value.ToString();
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
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            bool allSynced = true;
            string msalahdocNbr = "";
            string docNbr = "";
            string referenceNbr = tbAcumaticaRefNbr.Text ?? "";
            var docBranch = GetBranch(tbWarehouse.Text, 2);

            loadDetail();
            DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

            //issue
            ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
            inventoryIssue.Date = Convert.ToDateTime(tbProcessDate.Text);
            inventoryIssue.Description = $"{tbWarehouse.Text} {tempProcessDescr} Processing IN Transaction Issue";
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
                        msalahdocNbr=rowView[22].ToString();
                        allSynced = false;
                    }
                }
                ULTInventoryIssueDetail issueDetail = new ULTInventoryIssueDetail();
                issueDetail.InventoryID = rowView[1].ToString();
                issueDetail.Branch = docBranch;
                issueDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                issueDetail.Subitem = rowView[2].ToString().Replace(".", "");
                issueDetail.Quantity = Convert.ToDecimal(rowView[19]);
                issueDetail.Description = rowView[3].ToString();
                issueDetail.Warehouse = tbWarehouse.Text;
                issueDetail.UOM = rowView[20].ToString();
                issueDetail.ReasonCode = AcumaticaReasonCode;
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
                var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                try
                {

                    var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
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
                    }


                    authApi.AuthLogout();
                    if (!syncError)
                    {
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";
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
                }
            }//end if
        }

        //private void btnAcumatica_Click(object sender, EventArgs e)
        //{
        //    this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}] - Syncing with Acumatica, please wait!";
        //    bool syncError = false;
        //    bool allSynced = true;
        //    string docNbr = "";
        //    string referenceNbr = "";
        //    var docBranch = GetBranch(tbWarehouse.Text,2);

        //    loadDetail();
        //    DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

        //    //issue
        //    ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
        //    inventoryIssue.Date = Convert.ToDateTime(tbProcessDate.Text);
        //    inventoryIssue.Description = $"{tbWarehouse.Text} {tempProcessDescr} Processing IN Transaction Issue";
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
        //        issueDetail.ReasonCode = AcumaticaReasonCode;
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
        //        var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
        //        var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
        //        try
        //        {
        //            var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
        //            var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);
        //            ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
        //            inventoryIssueApi.InvokeAction(releaseInventoryIssue);

        //            referenceNbr = responseIssue.ReferenceNbr.ToString();
        //            tbAcumaticaRefNbr.Text = referenceNbr;
        //        }
        //        catch (Exception ePut)
        //        {
        //            MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            syncError = true;
        //        }
        //        finally
        //        {
        //           var status_acumatica = checkReleasedIssue(referenceNbr, configuration);

        //            authApi.AuthLogout();
        //            if (!syncError || status_acumatica == "Released")
        //            {
        //                this.Text = $"Universal Leaf [{Warehouse.Descr}] - {tempProcessDescr} IN Process [{DocNumber}]";
        //                tbStatus.Text = "SYNCED";
        //                saveDocument();
        //                MessageBox.Show($"--Sync Complete!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        //                using (SqlConnection connection = new SqlConnection(ConnectionString))
        //                {
        //                    try
        //                    {
        //                        if (connection.State != ConnectionState.Open)
        //                        {
        //                            connection.Open();
        //                        }

        //                        foreach (DataRowView rowView in dv_filter)
        //                        {
        //                            var rowDocNumber = rowView[0].ToString();
        //                            var rowLotNumber = rowView[3].ToString();

        //                            using (SqlCommand command = new SqlCommand("Update_ProcessingLineINDetail_Sync", connection))
        //                            {
        //                                command.CommandType = CommandType.StoredProcedure;
        //                                command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
        //                                command.Parameters.AddWithValue("@LotNbr", rowLotNumber);
        //                                command.Parameters.AddWithValue("@SyncDetail", 1);
        //                                command.ExecuteNonQuery();
        //                            }
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
        //    }//end if
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
        
        private String checkReleasedIssue(string referenceNbr, Configuration configuration)
        {
                var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
                
                var response = inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr });

                if (response.Status == "Released")
                {
                    var TotalCost = (decimal)response.TotalCost.Value;
                    tbTotalCost.Text = TotalCost.ToString("N2");
                }else {
                    
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
	                                        ProcessingLineINDetail
	                                    WHERE
		                                    ProcessingLineINDetail.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        ProcessingLineINDetail.LotNbr";

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
                DocDate = tbProcessDate.Text,
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
                            if (Convert.ToInt32(reader.GetValue(1))== 1) {
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
	                                        ProcessingLineINDetail.DocumentID,
	                                        ProcessingLineINDetail.InventoryID, 
	                                        ProcessingLineINDetail.SubItem, 
	                                        SegmentValue.Descr AS Stage, 
	                                        ProcessingLineINDetail.Color , 
											ProcessingLineINDetail.Fermentation,
											ProcessingLineINDetail.Length,
											ProcessingLineINDetail.StalkPosition,
	                                        COUNT(ProcessingLineINDetail.LotNbr) as WeightRope, 
	                                        SUM(ProcessingLineINDetail.WeightNetto) as WeightNetto 
                                        FROM
	                                        dbo.ProcessingLineINDetail
	                                        INNER JOIN
	                                        dbo.ProcessingLineIN
	                                        ON 
		                                        ProcessingLineINDetail.DocumentID = ProcessingLineIN.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        ProcessingLineINDetail.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
										WHERE ProcessingLineInDetail.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        ProcessingLineINDetail.DocumentID,
	                                        ProcessingLineINDetail.InventoryID, 
	                                        ProcessingLineINDetail.SubItem, 
	                                        SegmentValue.Descr , 
	                                        ProcessingLineINDetail.Color , 
											ProcessingLineINDetail.Fermentation,
											ProcessingLineINDetail.Length,
											ProcessingLineINDetail.StalkPosition";

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
                DocDate = tbProcessDate.Text,
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
                MessageBox.Show($"Tidak Singkron Salah", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                    Console.WriteLine("ListArray is null" + MapArray[indexku]);
                                    objTrans.Rollback();

                                    break;
                                }

                                using (SqlCommand command = new SqlCommand("Insert_ProcessingLineINDetail", connection, objTrans))
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
                                    command.Parameters.AddWithValue("@BuyerName", MapArray[indexku][22]);
                                    command.Parameters.AddWithValue("@LotGroup", tbEntryLotGroup.Text);
                                    command.ExecuteNonQuery();

                                }

                            }
                            else
                            {
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

        private void checklotnumber(string lot)
        {
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
                catch (Exception e)
                {
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