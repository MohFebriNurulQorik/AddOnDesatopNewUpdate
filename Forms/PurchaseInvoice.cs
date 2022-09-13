using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using Acumatica.ULT_18_200_001.Api;
using Acumatica.ULT_18_200_001.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class PurchaseInvoice : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }

        private int lastIncrementValue = -1;

        private DataTable dtVendor;
        private DataTable dtVendorDetail;
        private DataTable dtReceipt;
        private DataTable dtDetail;
        private DataTable dtDetail2;
        private DataTable dtEntry;
        private DataTable dtBuying;

        private decimal taxPct = (decimal)0.0025;
        private decimal deductPct = (decimal)0.45;
        private decimal tempVolumeVariabel = 1M;
        private decimal tempVolumeCurrent = 1M;
        private string tempNoKontrak = "";
        private string tempVendorClass = "";
        private string tempVendorID = "";

        private bool overDeduct = false;
        private bool negativePayment = false;
        private bool overDeductTotal = false;
        private bool advanceUpdated = false;

        public PurchaseInvoice()
        {
            InitializeComponent();
        }

        private void PurchaseInvoice_Load(object sender, EventArgs e)
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

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice [{DocNumber}]";

            loadComboVendor();

            tbDocNumber.Text = DocNumber;
            checkNPWP.Checked = true;
            tbStatus.Text = "";
            tbCashAdvance.Text = "0";
            tbCashAdvance.BackColor = System.Drawing.Color.LightCoral;
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbVendorDetails.Text = "";
            tbAcumaticaRefNbr.Text = "";
            tbBuyerName.Text = "";
            tbAdminInvoice.Text = "";

            resetEntry();
        }

        private void resetEntry()
        {
            loadDetail();

            groupEntry.Text = "Invoice Entry [<NEW>]";

            tbEntryDoc.Text = "";
            ReceiptAmountAddOn(tbEntryDoc.Text);
            tbEntryReceiptAmount.Text = "0";
            //tbEntryDeductPct.Text = "45.0000%";
            tbEntryTaxAmount.Text = "0";
            tbEntryDeductAmount.Text = "0";
            tbEntryPayment.Text = "0";

            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = true;
                    btnAllocateAll.Enabled = true;
                    btnSave.Enabled = true;
                    //btnSaveLot.Enabled = true;
                    btnDelLot.Enabled = false;
                    btnPrintInvoice.Enabled = true;
                    btnUpdateVendor.Enabled = true;
                    tbBuyerName.Enabled = true;
                    tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                    tbAdminInvoice.Enabled = true;
                    tbAdminInvoice.BackColor = System.Drawing.SystemColors.Window;
                    checkHold.Enabled = true;
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
                    btnAllocateAll.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnDelLot.Enabled = false;
                    btnPrintInvoice.Enabled = true;
                    btnUpdateVendor.Enabled = false;
                    tbBuyerName.Enabled = false;
                    tbBuyerName.BackColor = System.Drawing.SystemColors.Info;
                    tbAdminInvoice.Enabled = false;
                    tbAdminInvoice.BackColor = System.Drawing.SystemColors.Info;
                    checkHold.Enabled = false;
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
                    btnAllocateAll.Enabled = true;
                    btnSave.Enabled = true;
                    //btnSaveLot.Enabled = true;
                    btnDelLot.Enabled = false;
                    btnPrintInvoice.Enabled = false;
                    btnUpdateVendor.Enabled = true;
                    tbBuyerName.Enabled = true;
                    tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                    tbAdminInvoice.Enabled = true;
                    tbAdminInvoice.BackColor = System.Drawing.SystemColors.Window;
                    checkHold.Enabled = true;
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

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice [{DocNumber}]";

            loadComboVendor();

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from PurchaseInvoice where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(9).ToString();
                        tbCashAdvance.Text = Convert.ToDecimal(reader.GetValue(5)).ToString("N2");
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(10).ToString();
                        //AcumaticaRefNbr = reader.GetValue(8).ToString();
                        cbVendorID.SelectedIndex = cbVendorID.FindString(reader.GetValue(3).ToString());
                        checkNPWP.Checked = reader.GetValue(11).ToString() == "1" ? true : false;
                        tbBuyerName.Text = reader.GetValue(12).ToString();
                        tbAdminInvoice.Text = reader.GetValue(13).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(9).ToString() == "HOLD" ? 1 : 0);
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
                dtDetail2 = new DataTable();
                try
                {
                    string query = $@"SELECT *
                                        FROM
	                                        PurchaseInvoiceDetail
                                        WHERE
	                                        DocumentID = '{DocNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                    da.Fill(dtDetail2);
                    dgvDetail.DataSource = dtDetail;

                    //Header Text
                    dgvDetail.Columns[0].HeaderText = "Document ID";
                    dgvDetail.Columns[0].Visible = false;
                    dgvDetail.Columns[1].HeaderText = "Receipt ID";
                    dgvDetail.Columns[2].HeaderText = "Receipt Amount";
                    dgvDetail.Columns[3].HeaderText = "Tax Percentage";
                    dgvDetail.Columns[3].DefaultCellStyle.Format = "P4";
                    dgvDetail.Columns[4].HeaderText = "Tax Amount";
                    dgvDetail.Columns[4].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[5].HeaderText = "Deduct Percentage";
                    dgvDetail.Columns[5].DefaultCellStyle.Format = "P4";
                    dgvDetail.Columns[6].HeaderText = "Deduct Amount";
                    dgvDetail.Columns[6].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[7].HeaderText = "Payment Amount";
                    dgvDetail.Columns[7].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[8].HeaderText = "Synced";
                    dgvDetail.Columns[9].Visible = false;
                    dgvDetail.Columns[10].Visible = false;

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        decimal sumTax = Convert.ToDecimal(dtDetail.Compute("SUM(TaxAmount)", string.Empty));
                        tbDetailTax.Text = sumTax.ToString("N2");
                        decimal sumLoan = Convert.ToDecimal(dtDetail.Compute("SUM(DeductAmount)", string.Empty));
                        tbDetailDeduct.Text = sumLoan.ToString("N2");
                        decimal sumPayment = Convert.ToDecimal(dtDetail.Compute("SUM(PaymentAmount)", string.Empty));
                        tbDetailPayment.Text = sumPayment.ToString("N2");
                        decimal sumInvoice = Convert.ToDecimal(dtDetail.Compute("SUM(ReceiptAmount)", string.Empty));
                        tbTotalReceived.Text = sumInvoice.ToString("N2");

                        //disable element
                        cbVendorID.Enabled = false;
                        tbBuyerName.ReadOnly = true;
                        tbBuyerName.BackColor = System.Drawing.SystemColors.Info;
                        tbAdminInvoice.ReadOnly = true;
                        tbAdminInvoice.BackColor = System.Drawing.SystemColors.Info;
                    }
                    else
                    {
                        decimal sumTax = 0;
                        tbDetailTax.Text = sumTax.ToString("N2");
                        decimal sumLoan = 0;
                        tbDetailDeduct.Text = sumLoan.ToString("N2");
                        decimal sumPayment = 0;
                        tbDetailPayment.Text = sumPayment.ToString("N2");
                        decimal sumInvoice = 0;
                        tbTotalReceived.Text = sumInvoice.ToString("N2");

                        //disable element
                        cbVendorID.Enabled = true;
                        tbBuyerName.ReadOnly = false;
                        tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
                        tbAdminInvoice.ReadOnly = false;
                        tbAdminInvoice.BackColor = System.Drawing.SystemColors.Window;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

        }

        public void ReceiptAmountAddOn(string docby)
        {
            int a = 0;
            if (docby == $"")
            {
                addonreceiptamount.Text = $"0";
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {

                    connection.Open();
                    string query = $@"SELECT
                                            SUM(CostNett)
                                        FROM
	                                        BuyingLineDetail
                                
                               
                                WHERE DocumentID='{docby}' AND StatusReject = 0
                                        ";
                    SqlCommand command = new SqlCommand(query, connection);


                    decimal count = (decimal)command.ExecuteScalar();

                    addonreceiptamount.Text = count.ToString("N2");
                    connection.Close();
                }

            }
        }

        private void loadEntry(string buyingNbr)
        {
            resetEntry();

            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtEntry = new DataTable();
                try
                {
                    string query = "";

                    query = $@"SELECT SUM(a.CostNett) as CostNett, a.NoKontrak, b.VendorClass FROM BuyingLineDetail as a left join BuyingLine as b on a.DocumentID = b.DocumentID  WHERE a.DocumentID = '{buyingNbr}' AND a.StatusReject = 0 GROUP BY  a.NoKontrak,b.VendorClass";

                    Console.WriteLine(query);
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntry);

                    tempNoKontrak = dtEntry.Rows[0].ItemArray[1].ToString();
                    tempVendorClass = dtEntry.Rows[0].ItemArray[2].ToString();

                    if (tempVendorClass == "FARMEROM")
                    {
                        tbEntryDeductAmount.Enabled = false;
                        tbEntryDeductPct.Enabled = false;

                    }
                    else
                    {
                        tbEntryDeductAmount.Enabled = true;
                        tbEntryDeductPct.Enabled = true;

                    }

                    if (tempNoKontrak != "")
                    {
                        dtEntry = new DataTable();

                        query = $@"SELECT
                                        SUM(BLDetail.CostNett) AS CostNett,
                                        VendorContract.VolumeTotal,
	                                    VendorContract.VolumePercentage,
	                                    VendorContract.VolumeVariable,
	                                    BLDetail.NoKontrak,
                           
	                                    (Select SUM(nested.WeightNetto) FROM BuyingLineDetail as nested WHERE nested.NoKontrak = BLDetail.NoKontrak ) as VolumeCurrent
                           
                                    FROM
                                        dbo.BuyingLineDetail as BLDetail
                                        INNER JOIN
                                        dbo.VendorContract
                                        ON
                                            BLDetail.NoKontrak = VendorContract.NoKontrak
                                    WHERE
                                        BLDetail.DocumentID = '{buyingNbr}'
                                        AND
                                        BLDetail.StatusReject = 0
                                    GROUP BY
                                        VendorContract.VolumeTotal,
	                                    VendorContract.VolumePercentage,
	                                    VendorContract.VolumeVariable,
	                                    BLDetail.NoKontrak";

                        command = new SqlCommand(query, connection);
                        if (connection.State != ConnectionState.Open) connection.Open();

                        da = new SqlDataAdapter(command);
                        da.Fill(dtEntry);
                    }


                    var receiptDoc = buyingNbr;
                    decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));
                    decimal receiptAmount = Convert.ToDecimal(dtEntry.Rows[0].ItemArray[0].ToString());
                    decimal taxAmount = Math.Round((taxPct * receiptAmount), 2);

                    if (tempNoKontrak != "")
                    {
                        tempVolumeVariabel = Convert.ToDecimal(dtEntry.Rows[0].ItemArray[3].ToString());
                        tempVolumeCurrent = Convert.ToDecimal(dtEntry.Rows[0].ItemArray[5].ToString());
                        try
                        {
                            deductPct = tempVolumeCurrent / tempVolumeVariabel;
                        }
                        catch
                        {
                            deductPct = 0.45M;
                        }


                    }
                    else
                    {
                        tempVolumeVariabel = 1M;
                        tempVolumeCurrent = 1M;
                    }




                    decimal defaultDeductAmount = farmerAdvance * deductPct;

                    if (farmerAdvance <= 0)
                    {
                        deductPct = 0;
                    }
                    else if (defaultDeductAmount >= (receiptAmount - taxAmount))
                    {
                        deductPct = (receiptAmount - taxAmount) / farmerAdvance;
                    }

                    if (tempVendorClass == "FARMEROM")
                    {
                        deductPct = 0;

                    }

                    decimal deductAmount = Math.Round((farmerAdvance * deductPct), 2);

                    decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

                    tbEntryDoc.Text = receiptDoc;
                    ReceiptAmountAddOn(receiptDoc);
                    tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
                    tbEntryTaxAmount.Text = taxAmount.ToString("N2");
                    tbEntryDeductPct.Text = deductPct.ToString("P4");
                    tbEntryDeductAmount.Text = deductAmount.ToString("N2");
                    tbEntryPayment.Text = paymentAmount.ToString("N2");
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
                //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-INV-{currentIncrement.ToString().PadLeft(4, '0')}";
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-INV-{currentIncrement.ToString().PadLeft(4, '0')}";
                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-INV-{currentIncrement.ToString().PadLeft(4, '0')}";

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
                    string query = $"IF EXISTS ( SELECT * FROM PurchaseInvoice WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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
                    string query = $"select * from NumberingSetting where NumberingID = 'INV'";
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

        private void loadComboVendor()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtVendor = new DataTable();
                try
                {
                    string query = $@"SELECT DISTINCT(BuyingLine.VendorID), VendorData.vendorName
                                        FROM
	                                        dbo.BuyingLine
	                                        INNER JOIN
	                                        dbo.VendorData
	                                        ON
		                                        BuyingLine.VendorID = VendorData.VendorID
                                        WHERE
	                                        BuyingLine.Status = 'SYNCED'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtVendor);
                    string[] arrray = dtVendor.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[1].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbVendorID);
                    cbVendorID.Items.Clear();
                    cbVendorID.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboReceipt()
        {
            if (cbVendorID.SelectedIndex >= 0)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //clear combo
                    //cbFarmer.DataSource = null;
                    dtReceipt = new DataTable();
                    try
                    {
                        string query = $@"SELECT
	                                            DocumentID
                                            FROM
	                                            BuyingLine
                                            WHERE
                                                VendorID = '{tempVendorID}'
                                            AND Status = 'SYNCED'
	                                        AND (InvoiceID = '' OR InvoiceID = NULL)";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtReceipt);
                        string[] arrray = dtReceipt.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                        //new AutoCompleteBehavior(cbBuyingNbr);
                        cbBuyingNbr.Items.Clear();
                        cbBuyingNbr.Items.AddRange(arrray);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }
        }

        private void loadVendorDetails()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtVendorDetail = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    VendorID,
                                        VendorName,
	                                    CONCAT(AddressLine1, ' ',AddressLine2, ' ',City) as Details,
	                                    (SELECT SUM(UnappliedBalance) FROM VendorPrepayment WHERE VendorID = '{cbVendorID.SelectedItem.ToString().Split('|')[0].Trim()}') as TotalCashAdvance,
                                        Ext_No_NPWP,
                                        Ext_VendorIsTaxAgency
                                    FROM
	                                    VendorData
                                    WHERE VendorID = '{tempVendorID}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtVendorDetail);

                    tbVendorDetails.Text = $"{dtVendorDetail.Rows[0].ItemArray[1].ToString()}{Environment.NewLine}{dtVendorDetail.Rows[0].ItemArray[2].ToString()}";
                    noNPWP.Text = $"{dtVendorDetail.Rows[0].ItemArray[4].ToString()}";
                    checkNPWP.Checked = ($"{dtVendorDetail.Rows[0].ItemArray[4].ToString()}" != "" && $"{dtVendorDetail.Rows[0].ItemArray[4].ToString()}" != null && $"{dtVendorDetail.Rows[0].ItemArray[4].ToString()}".Trim().Length >= 3) ? true : false;


                    if (advanceUpdated)
                    {
                        tbCashAdvance.Text = Convert.ToDecimal(dtVendorDetail.Rows[0].ItemArray[3] == DBNull.Value ? "0" : dtVendorDetail.Rows[0].ItemArray[3]).ToString("N2");
                    }
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

                        using (SqlCommand command = new SqlCommand("Insert_PurchaseInvoice_v2", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbProcessDate.Text);
                            command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@VendorID", cbVendorID.SelectedItem.ToString().Split('|')[0].Trim());
                            command.Parameters.AddWithValue("@VendorDetails", tbVendorDetails.Text);
                            command.Parameters.AddWithValue("@TotalCashAdvance", tbCashAdvance.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotaxTaxDeduct", tbDetailTax.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalPaymentDeduct", tbDetailDeduct.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalPayment", tbDetailPayment.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                            command.Parameters.AddWithValue("@NPWP", checkNPWP.Checked ? 1 : 0);
                            command.Parameters.AddWithValue("@BuyerName", tbBuyerName.Text ?? "");
                            command.Parameters.AddWithValue("@AdminName", tbAdminInvoice.Text ?? "");
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserID);
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
                        if (!insertError)
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"INV");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }

                            MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"INV");
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
                btnPrintInvoice.Enabled = true;
            }
            else
            {
                btnPrintInvoice.Enabled = false;
            }
        }

        private void cbVendorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            advanceUpdated = false;
            //tbCashAdvance.Text = "Please Update Vendor!";

            tempVendorID = ((ComboBox)sender).SelectedItem.ToString().Split('|')[0].Trim();

            if (tempVendorID != "")
            {
                cbBuyingNbr.SelectedIndex = -1;
                cbBuyingNbr.Items.Clear();

                loadComboReceipt();
                loadVendorDetails();
            }
            else
            {
                cbBuyingNbr.SelectedIndex = -1;
                cbBuyingNbr.Items.Clear();

                tbVendorDetails.Text = "";
                tbCashAdvance.Text = "0";
            }

            tbCashAdvance.BackColor = System.Drawing.Color.LightCoral;
            //cbBuyingNbr.Enabled = false;
        }

        private void saveReceipt()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.Replace(",", ""));
                    decimal taxAmount = Convert.ToDecimal(tbEntryTaxAmount.Text.Replace(",", ""));
                    decimal deductAmount = Convert.ToDecimal(tbEntryDeductAmount.Text.Replace(",", ""));
                    decimal paymentAmount = Convert.ToDecimal(tbEntryPayment.Text.Replace(",", ""));

                    using (SqlCommand command = new SqlCommand("Insert_PurchaseInvoiceDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@ReceiptID", tbEntryDoc.Text);
                        command.Parameters.AddWithValue("@ReceiptAmount", receiptAmount.ToString());
                        command.Parameters.AddWithValue("@TaxPercentage", taxPct.ToString());
                        command.Parameters.AddWithValue("@TaxAmount", taxAmount.ToString());
                        command.Parameters.AddWithValue("@DeductPercentage", deductPct.ToString());
                        command.Parameters.AddWithValue("@DeductAmount", deductAmount.ToString());
                        command.Parameters.AddWithValue("@PaymentAmount", paymentAmount.ToString());
                        command.Parameters.AddWithValue("@SyncDetail", 0);
                        command.Parameters.AddWithValue("@VolumeVariable", tempVolumeVariabel);
                        command.Parameters.AddWithValue("@VolumeCurrent", tempVolumeCurrent);

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
                    groupEntry.Text = $"Invoice Entry [{tbEntryDoc.Text}]";
                    cbBuyingNbr.SelectedIndex = -1;
                    loadComboReceipt();
                }
            }
        }

        private void removeReceipt()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Delete_PurchaseInvoiceDetail", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@ReceiptID", tbEntryDoc.Text);

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
                    cbBuyingNbr.SelectedIndex = -1;
                    loadComboReceipt();
                }
            }
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbVendorID.SelectedIndex >= 0)
            {
                saveDocument();
                loadDetail();
                resetEntry();
            }
            else
            {
                MessageBox.Show("Please select vendor!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetScreen();
        }

        private void cbBuyingNbr_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetEntry();
            if (cbBuyingNbr.SelectedIndex >= 0 && advanceUpdated == true)
            {
                loadEntry(cbBuyingNbr.SelectedItem.ToString());
            }
        }

        private void checkNPWP_CheckedChanged(object sender, EventArgs e)
        {
            if (checkNPWP.Checked)
            {
                taxPct = (decimal)0.0025;
            }
            else
            {
                taxPct = (decimal)0.005;
            }

            //var receiptDoc = dtEntry.Rows[0].ItemArray[0].ToString();

            decimal farmerAdvance = 0M;
            if (advanceUpdated)
            {
                farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));
            }
            decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.ToString().Replace(",", ""));
            decimal taxAmount = Math.Round((taxPct * receiptAmount), 2);

            if (tempNoKontrak != "")
            {
                tempVolumeVariabel = Convert.ToDecimal(dtEntry.Rows[0].ItemArray[3].ToString());
                tempVolumeCurrent = Convert.ToDecimal(dtEntry.Rows[0].ItemArray[5].ToString());
                try
                {
                    deductPct = tempVolumeCurrent / tempVolumeVariabel;
                }
                catch
                {
                    deductPct = 0.45M;
                }
            }
            else
            {
                tempVolumeVariabel = 1M;
                tempVolumeCurrent = 1M;
            }

            decimal defaultDeductAmount = farmerAdvance * deductPct;

            if (farmerAdvance <= 0)
            {
                deductPct = 0;
            }
            else if (defaultDeductAmount > (receiptAmount - taxAmount))
            {
                deductPct = (receiptAmount - taxAmount) / farmerAdvance;
            }

            decimal deductAmount = Math.Round((farmerAdvance * deductPct), 2);

            decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

            //tbEntryDoc.Text = receiptDoc;
            tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
            tbEntryTaxAmount.Text = taxAmount.ToString("N2");
            tbEntryDeductPct.Text = deductPct.ToString("P4");
            tbEntryDeductAmount.Text = deductAmount.ToString("N2");
            tbEntryPayment.Text = paymentAmount.ToString("N2");
        }

        private void tbEntryDeductPct_TextChanged(object sender, EventArgs e)
        {
            //if (tbEntryDeductPct.Text.Length > 0)
            //{
            //    deductPct = Convert.ToDecimal(tbEntryDeductPct.Text.Replace("%", "")) / 100;

            //    decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));

            //    decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.Replace(",", ""));
            //    decimal taxAmount = taxPct * receiptAmount;

            //    decimal deductAmount = farmerAdvance * deductPct;
            //    decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

            //    tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
            //    tbEntryTaxAmount.Text = taxAmount.ToString("N2");
            //    //tbEntryDeductPct.Text = deductPct.ToString("P4");
            //    tbEntryDeductAmount.Text = deductAmount.ToString("N2");
            //    tbEntryPayment.Text = paymentAmount.ToString("N2");
            //}
            //else
            //{
            //    tbEntryDeductAmount.Text = "0";
            //    tbEntryPayment.Text = "0";
            //}
        }

        private void tbEntryDeductAmount_TextChanged(object sender, EventArgs e)
        {
            //if (tbEntryDeductAmount.Text.Length > 0)
            //{
            //    decimal deductAmount = Convert.ToDecimal(tbEntryDeductAmount.Text.Replace(",", ""));
            //    //decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.Replace(",", ""));

            //    decimal farmerAdvance = 0M;
            //    if (advanceUpdated)
            //    {
            //        farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));
            //    }

            //    deductPct = (deductAmount/farmerAdvance) * 100;
            //    tbEntryDeductPct.Text = deductPct.ToString();
            //    //decimal totalLoanDeduct = Convert.ToDecimal(tbDetailDeduct.Text.Replace(",", ""));
            //    //decimal compareDeduct = deductAmount + totalLoanDeduct;
            //    //if (!groupEntry.Text.Contains("<NEW>"))
            //    //{
            //    //    compareDeduct = deductAmount;
            //    //}

            //    //if (compareDeduct > farmerAdvance)
            //    //{
            //    //    tbEntryDeductAmount.BackColor = System.Drawing.Color.Red;
            //    //    overDeduct = true;
            //    //}
            //    //else
            //    //{
            //    //    //tbEntryDeductAmount.BackColor = System.Drawing.SystemColors.Info;
            //    //    tbEntryDeductAmount.BackColor = System.Drawing.SystemColors.Window;
            //    //    overDeduct = false;
            //    //}
            //}
        }

        private void tbEntryPayment_TextChanged(object sender, EventArgs e)
        {
            if (tbEntryPayment.Text.Length > 0)
            {
                decimal paymentAmount = Convert.ToDecimal(tbEntryPayment.Text.Replace(",", ""));

                if (paymentAmount < 0)
                {
                    tbEntryPayment.BackColor = System.Drawing.Color.Red;
                    negativePayment = true;
                }
                else
                {
                    tbEntryPayment.BackColor = System.Drawing.SystemColors.Info;
                    negativePayment = false;
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
                if (overDeduct || negativePayment)
                {
                    MessageBox.Show("periksa pembayaran lebih dari pemotongan atau pembayaran negatif!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    /* if (tempNoKontrak != "" && deductPct < (tempVolumeCurrent / tempVolumeVariabel))*/

                    /*MessageBox.Show($"Deduction percentage cannot be lower than {(tempVolumeCurrent / tempVolumeVariabel).ToString("N4")} %!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);*/

                    if (addonreceiptamount.Text == tbEntryReceiptAmount.Text)
                    {
                        saveReceipt();
                    }
                    else
                    {
                        MessageBox.Show("Receipt Amount dari Add On Desktop dan Receipt Amount dari Acumatica tidak sama", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }


                }
            }
        }

        private void btnDelLot_Click(object sender, EventArgs e)
        {
            removeReceipt();
        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                groupEntry.Text = $"Invoice Entry [{dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString()}]";

                tbEntryDoc.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                ReceiptAmountAddOn(tbEntryDoc.Text);
                vendorcalass(tbEntryDoc.Text);
                tbEntryReceiptAmount.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[2].Value.ToString();
                tbEntryTaxAmount.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[4].Value.ToString();
                tbEntryDeductPct.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[5].FormattedValue.ToString();
                tbEntryDeductAmount.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[6].Value.ToString();
                tbEntryPayment.Text = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[7].Value.ToString();
                tempVolumeVariabel = Convert.ToDecimal(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[9].Value.ToString());
                tempVolumeCurrent = Convert.ToDecimal(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[10].Value.ToString());

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

        private void tbDetailDeduct_TextChanged(object sender, EventArgs e)
        {
            if (tbEntryDeductAmount.Text.Length > 0)
            {
                decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.Replace(",", ""));
                decimal totalLoanDeduct = Convert.ToDecimal(tbDetailDeduct.Text.Replace(",", ""));
                if (totalLoanDeduct > farmerAdvance)
                {
                    tbDetailDeduct.BackColor = System.Drawing.Color.Red;
                    overDeductTotal = true;
                }
                else
                {
                    tbDetailDeduct.BackColor = System.Drawing.SystemColors.Info;
                    overDeductTotal = false;
                }
            }
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            if (cbVendorID.SelectedIndex >= 0)
            {
                //updateVendor(cbVendorID.SelectedItem.ToString().Split('|')[0].Trim());
            }

            acumaticaSync();
        }

        private void acumaticaSync()
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            string referenceNbr = "";

            var docBranch = GetBranch(tbWarehouse.Text, 2);

            //get Buying data
            //create new dt

            loadDetail();
            dtBuying = new DataTable();

            foreach (DataRow row in dtDetail.Rows)
            {
                var docBuying = row["ReceiptID"].ToString();

                //load data details buying
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                        string query = $@"SELECT
                                            BuyingLineDetail.*,
	                                        BuyingLine.AcumaticaRefNbr
                                        FROM
                                            dbo.BuyingLineDetail
                                        INNER JOIN
                                            dbo.BuyingLine
                                            ON
                                            BuyingLineDetail.DocumentID = BuyingLine.DocumentID
                                        WHERE
	                                        BuyingLineDetail.DocumentID = '{docBuying}'
                                        AND StatusReject = 0";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtBuying);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
            }

            //prepare for API invoice
            Bill bill = new Bill();
            //header invoice
            bill.Vendor = cbVendorID.SelectedItem.ToString().Split('|')[0].Trim();
            bill.VendorRef = DocNumber;
            bill.Date = Convert.ToDateTime(tbProcessDate.Text);
            bill.LoanRepayment = Convert.ToDecimal(tbDetailDeduct.Text.Replace(",", ""));
            bill.Hold = false;
            bill.BranchID = docBranch;

            List<BillDetail> listDetail = new List<BillDetail>();
            var docTemp = "";
            int receiptLine = 1;
            //document details
            foreach (DataRow rowView in dtBuying.Rows)
            {
                if (rowView[0].ToString() != docTemp)
                {
                    docTemp = rowView[0].ToString();
                    receiptLine = 1;
                }
                else
                {
                    receiptLine = receiptLine + 2;
                }

                BillDetail detail = new BillDetail();

                //detail.Branch = Warehouse.Branch;
                detail.Branch = docBranch;
                detail.InventoryID = rowView[1].ToString();
                detail.Subitem = rowView[2].ToString().Replace(".", "");
                detail.Qty = Convert.ToDecimal(rowView[15]);
                //detail.TransactionDescription = rowView[0].ToString();
                detail.TransactionDescription = rowView[3].ToString();
                detail.UnitCost = Convert.ToDecimal(rowView[17]);
                detail.ExtendedCost = Convert.ToDecimal(rowView[21]);
                detail.UOM = rowView[16].ToString();
                //detail.Subaccount = "01-00";
                //detail.TaxCategory = "NONTAX";
                detail.TaxCategory = "NONVAT";

                if (rowView[26].ToString() != "")
                {
                    detail.POOrderNbr = rowView[26].ToString();
                    detail.POLine = 1;
                    detail.NoKontrak = rowView[27].ToString();
                }

                //receipt
                detail.POReceiptNbr = rowView[32].ToString();
                detail.POReceiptLine = receiptLine;

                listDetail.Add(detail);
            }

            foreach (DataRow rowView in dtDetail.Rows)
            {
                BillDetail detail = new BillDetail();

                //detail.Branch = Warehouse.Branch;
                detail.Branch = docBranch;
                //detail.InventoryID = rowView[1].ToString();
                //detail.Subitem = rowView[2].ToString().Replace(".", "");
                detail.Qty = -1;
                //detail.TransactionDescription = rowView[0].ToString();
                detail.TransactionDescription = $"Buying Tobacco - {rowView[1].ToString()}";
                detail.UnitCost = Convert.ToDecimal(rowView[4]);
                //detail.UOM = rowView[15].ToString();
                detail.Account = "3350002";
                detail.Subaccount = "01-00";

                listDetail.Add(detail);
            }


            bill.Details = listDetail;
            Console.WriteLine(bill);

            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
            try
            {
                var billApi = new BillApi(configuration);
                var responseBill = billApi.PutEntity(bill);

                referenceNbr = responseBill.ReferenceNbr.ToString();
                tbAcumaticaRefNbr.Text = referenceNbr;

                //applyRepaymentBill applyRepayment = new applyRepaymentBill((Bill)responseBill);
                //billApi.InvokeAction(applyRepayment);

                //ReleaseBill releaseBill = new ReleaseBill((Bill)responseBill);
                //billApi.InvokeAction(releaseBill);
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

                            foreach (DataRow rowView in dtDetail.Rows)
                            {
                                var rowDocNumber = rowView[0].ToString();
                                var rowReceiptNumber = rowView[1].ToString();

                                using (SqlCommand command = new SqlCommand("Update_PurchaseInvoiceDetail_Sync", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
                                    command.Parameters.AddWithValue("@ReceiptID", rowReceiptNumber);
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

        private void btnUpdateVendor_Click(object sender, EventArgs e)
        {
            if (cbVendorID.SelectedIndex >= 0)
            {
                updateVendor(cbVendorID.SelectedItem.ToString().Split('|')[0].Trim());

                saveDocument();

                cbBuyingNbr.SelectedIndex = -1;
            }
        }

        private void updateVendor(string currentVendorID)
        {
            DateTime localDate = DateTime.Now;
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            bool syncError = false;

            try
            {
                Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice List - Syncing Vendor with Acumatica, please wait!";

                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                var vendorApi = new VendorApi(configuration);
                var vendorsCurrent = vendorApi.GetList(expand: "MainContact,MainContact/Address,ContractList", filter: $"VendorID eq '{currentVendorID}'");

                //insert vendor
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var vendor in vendorsCurrent)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorData", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@VendorID", vendor.VendorID.Value);
                                command.Parameters.AddWithValue("@VendorName", vendor.VendorName.Value);
                                command.Parameters.AddWithValue("@Status", vendor.Status.Value);
                                command.Parameters.AddWithValue("@DisplayName", vendor.MainContact.DisplayName.Value);
                                command.Parameters.AddWithValue("@Phone1", vendor.MainContact.Phone1.Value ?? "");
                                command.Parameters.AddWithValue("@Phone2", vendor.MainContact.Phone2.Value ?? "");
                                command.Parameters.AddWithValue("@AddressLine1", vendor.MainContact.Address.AddressLine1.Value ?? "");
                                command.Parameters.AddWithValue("@AddressLine2", vendor.MainContact.Address.AddressLine2.Value ?? "");
                                command.Parameters.AddWithValue("@City", vendor.MainContact.Address.City.Value ?? "");
                                command.Parameters.AddWithValue("@Country", vendor.MainContact.Address.Country.Value ?? "");
                                command.Parameters.AddWithValue("@State", vendor.MainContact.Address.State.Value ?? "");
                                command.Parameters.AddWithValue("@PostalCode", vendor.MainContact.Address.PostalCode.Value ?? "");
                                command.Parameters.AddWithValue("@VendorClass", vendor.VendorClass.Value);
                                command.Parameters.AddWithValue("@LastModifiedDateTime", vendor.LastModifiedDateTime.Value);

                                command.ExecuteNonQuery();
                            }

                            foreach (var contract in vendor.ContractList)
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlCommand command = new SqlCommand("Insert_VendorContract", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@VendorID", contract.VendorID.Value);
                                    command.Parameters.AddWithValue("@NoKontrak", contract.NoKontrak.Value);
                                    command.Parameters.AddWithValue("@FarmerID", contract.FarmerID.Value ?? "");
                                    command.Parameters.AddWithValue("@Area", contract.Area.Value ?? "");
                                    command.Parameters.AddWithValue("@SubArea", contract.SubArea.Value ?? "");
                                    command.Parameters.AddWithValue("@Seri", contract.Seri.Value ?? "");
                                    command.Parameters.AddWithValue("@InventoryID", contract.InventoryID.Value ?? "");
                                    command.Parameters.AddWithValue("@NoKTP", contract.NoKTP.Value ?? "");
                                    command.Parameters.AddWithValue("@Active", contract.Active.Value ?? false);
                                    command.Parameters.AddWithValue("@VolumeTotal", contract.VolumeTotal.Value ?? 0);
                                    command.Parameters.AddWithValue("@VolumePercentage", contract.VolumePercentage.Value ?? 0);
                                    command.Parameters.AddWithValue("@VolumeVariable", contract.VolumeVariable.Value ?? 0);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                var purchaseOrderApi = new PurchaseOrderApi(configuration);
                var purchaseOrders = purchaseOrderApi.GetList(filter: $"Type eq 'Normal' and VendorID eq '{currentVendorID}'", expand: "Details");

                //insert vendor PO
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var purchaseOrder in purchaseOrders)
                    {
                        if (purchaseOrder.ContractNo.Value != null && purchaseOrder.ContractNo.Value.Length > 0)
                        {
                            //insert vendorPO
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlCommand command = new SqlCommand("Insert_VendorPO", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@VendorID", purchaseOrder.VendorID.Value);
                                    command.Parameters.AddWithValue("@OrderNbr", purchaseOrder.OrderNbr.Value);
                                    command.Parameters.AddWithValue("@NoKontrak", purchaseOrder.ContractNo.Value ?? "");
                                    command.Parameters.AddWithValue("@Status", purchaseOrder.Status.Value);
                                    command.Parameters.AddWithValue("@OrderType", purchaseOrder.Type.Value);

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e_update)
                            {
                                MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            //insert PO detail
                            foreach (var detail in purchaseOrder.Details)
                            {
                                try
                                {
                                    if (connection.State != ConnectionState.Open)
                                    {
                                        connection.Open();
                                    }

                                    using (SqlCommand command = new SqlCommand("Insert_VendorPODetail", connection))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@OrderNbr", detail.OrderNbr.Value);
                                        command.Parameters.AddWithValue("@LineNbr", detail.LineNbr.Value);
                                        command.Parameters.AddWithValue("@InventoryID", detail.InventoryID.Value);
                                        command.Parameters.AddWithValue("@Subitem", detail.Subitem.Value);
                                        command.Parameters.AddWithValue("@WarehouseID", detail.WarehouseID.Value);
                                        command.Parameters.AddWithValue("@OrderQty", detail.OrderQty.Value);
                                        command.Parameters.AddWithValue("@QtyOnReceipts", detail.QtyOnReceipts.Value);

                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception e_update)
                                {
                                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }

                var checkApi = new CheckApi(configuration);
                var checks = checkApi.GetList(filter: $"Type eq 'Prepayment' and Vendor eq '{currentVendorID}'");

                //insert vendor Prepayment
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var check in checks)
                    {
                        if (check.Status == "Balanced" || check.Status == "On Hold")
                        {
                            continue; // skipp balance and on hold doc
                        }

                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorPrepayment", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@VendorID", check.Vendor.Value);
                                command.Parameters.AddWithValue("@Description", check.Description.Value ?? "");
                                command.Parameters.AddWithValue("@PaymentAmount", check.PaymentAmount.Value);
                                command.Parameters.AddWithValue("@PaymentRef", check.PaymentRef.Value ?? "");
                                command.Parameters.AddWithValue("@ReferenceNbr", check.ReferenceNbr.Value);
                                command.Parameters.AddWithValue("@Status", check.Status.Value);
                                command.Parameters.AddWithValue("@Type", check.Type.Value);
                                command.Parameters.AddWithValue("@UnappliedBalance", check.UnappliedBalance.Value);

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception e_acumatica)
            {
                //Console.WriteLine(ex.Message);
                MessageBox.Show(e_acumatica.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                syncError = true;
            }
            finally
            {
                //we use logout in finally block because we need to always logout, even if the request failed for some reason
                authApi.AuthLogout();
                //Console.WriteLine("Logged Out...");
                ////MessageBox.Show("Logged Out...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!syncError)
                {
                    MessageBox.Show("Vendor update complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice List";
                    advanceUpdated = true;
                    tbCashAdvance.BackColor = System.Drawing.SystemColors.Info;
                    //cbBuyingNbr.Enabled = true;
                    loadVendorDetails();
                }
            }
        }


        private void vendorcalass(string dataVendorClass)
        {


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                DataTable dtEntrynew = new DataTable();
                try
                {
                    string query = "";

                    query = $@"SELECT SUM(a.CostNett) as CostNett, a.NoKontrak, b.VendorClass FROM BuyingLineDetail as a left join BuyingLine as b on a.DocumentID = b.DocumentID  WHERE a.DocumentID = '{dataVendorClass}' AND a.StatusReject = 0 GROUP BY  a.NoKontrak,b.VendorClass";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtEntrynew);
                    tempVendorClass = dtEntrynew.Rows[0].ItemArray[2].ToString();

                    if (tempVendorClass == "FARMEROM")
                    {
                        tbEntryDeductAmount.Enabled = false;
                        tbEntryDeductPct.Enabled = false;

                    }
                    else
                    {
                        tbEntryDeductAmount.Enabled = true;
                        tbEntryDeductPct.Enabled = true;

                    }

                }
                catch (Exception a)
                {
                    MessageBox.Show(a.ToString());
                }
            }
        }

        private void btnPrintLot_Click(object sender, EventArgs e)
        {
            if (cbVendorID.SelectedIndex >= 0 && !tbDocNumber.Text.Contains("<NEW>"))
            {
                DataSetAddon myInvoice = new DataSetAddon();
                var dataVendorClass = "";
                //dtDetail.TableName = "PurchaseInvoiceDetail";
                //myInvoice.Merge(dtDetail.Copy());

                //load data untuk grid
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //create new dt
                    //DataTable dt = new DataTable();
                    try
                    {
                        //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";


                        string query = $@"SELECT
	                            b.DocumentID,
	                            b.InventoryID,
	                            i.Descr,
	                            b.SubItem,
	                            b.Grade,
	                            b.LotNbr  AS Lot,
	                            b.WeightReceive  AS WeightReceive,
	                            b.WeightTare  AS WeightTare,
	                            b.WeightNetto  AS WeightNetto,
	                            b.CostGross  AS CostGross,
	                            b.CostNTRM  AS CostNTRM,
	                            b.CostNett  AS CostNett 
                            FROM
	                            dbo.BuyingLineDetail b
	                            INNER JOIN dbo.InventoryItem i ON b.InventoryID = i.InventoryID 
                            WHERE
	                            b.DocumentID IN ( SELECT ReceiptID FROM PurchaseInvoiceDetail WHERE DocumentID = '{DocNumber}' ) 
                            AND b.StatusReject = 0
                            ORDER BY
	                            b.DocumentID ASC";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(myInvoice.PurchaseInvoiceDetailGrade);
                        dataVendorClass = myInvoice.PurchaseInvoiceDetailGrade.Rows[0].ItemArray[0].ToString();



                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //create new dt
                    DataTable dtEntrynew = new DataTable();
                    try
                    {
                        string query = "";

                        query = $@"SELECT SUM(a.CostNett) as CostNett, a.NoKontrak, b.VendorClass FROM BuyingLineDetail as a left join BuyingLine as b on a.DocumentID = b.DocumentID  WHERE a.DocumentID = '{dataVendorClass}' AND a.StatusReject = 0 GROUP BY  a.NoKontrak,b.VendorClass";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtEntrynew);
                        tempVendorClass = dtEntrynew.Rows[0].ItemArray[2].ToString();

                    }
                    catch (Exception a)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }

                Console.WriteLine(tempVendorClass);

                PurchaseInvoicePrint invoicePrint = new PurchaseInvoicePrint
                {
                    Company = Warehouse.Company,
                    Warehouse = Warehouse.Descr,
                    Address = GetBranch(Warehouse.WarehouseID, 3),
                    Phone = GetBranch(Warehouse.WarehouseID, 4),
                    DocNumber = tbDocNumber.Text,
                    InvoiceDate = tbProcessDate.Text,
                    VendorID = cbVendorID.SelectedItem.ToString().Split('|')[0].Trim(),
                    VendorDetails = tbVendorDetails.Text,
                    invDetails = myInvoice,
                    CashAdvance = tbCashAdvance.Text,
                    TaxDeduct = tbDetailTax.Text,
                    LoanDeduct = tbDetailDeduct.Text,
                    TotalPayment = tbDetailPayment.Text,
                    BuyerName = tbBuyerName.Text,
                    AdminName = tbAdminInvoice.Text,
                    VendorClass = tempVendorClass
                };
                invoicePrint.ShowDialog();
            }
            else
            {
                MessageBox.Show("Create a document / select vendor first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnAllocateAll_Click(object sender, EventArgs e)
        {
            decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));
            decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.ToString().Replace(",", ""));
            decimal taxAmount = Math.Round((taxPct * receiptAmount), 2);
            decimal defaultDeductAmount = farmerAdvance;

            if (farmerAdvance <= 0)
            {
                deductPct = 0;
            }
            else if (defaultDeductAmount >= (receiptAmount - taxAmount))
            {
                deductPct = (receiptAmount - taxAmount) / farmerAdvance;
            }
            else
            {
                deductPct = 1;
            }

            if (tempVendorClass == "FARMEROM")
            {
                deductPct = 0;

            }

            decimal deductAmount = Math.Round((farmerAdvance * deductPct), 2);

            decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

            tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
            tbEntryTaxAmount.Text = taxAmount.ToString("N2");
            tbEntryDeductPct.Text = deductPct.ToString("P4");
            tbEntryDeductAmount.Text = deductAmount.ToString("N2");
            tbEntryPayment.Text = paymentAmount.ToString("N2");
        }

        #region TBInputCheck


        private void tbEntryDeductPct_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textbox_check(sender, e);
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                if (tbEntryDeductPct.Text.Length > 0)
                {
                    deductPct = Convert.ToDecimal(tbEntryDeductPct.Text.Replace("%", "")) / 100;

                    decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));

                    decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.Replace(",", ""));
                    decimal taxAmount = Math.Round((taxPct * receiptAmount), 2);

                    decimal deductAmount = Math.Round((farmerAdvance * deductPct), 2);
                    decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

                    tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
                    tbEntryTaxAmount.Text = taxAmount.ToString("N2");
                    //tbEntryDeductPct.Text = deductPct.ToString("P4");
                    tbEntryDeductAmount.Text = deductAmount.ToString("N2");
                    tbEntryPayment.Text = paymentAmount.ToString("N2");
                }
                else
                {
                    tbEntryDeductAmount.Text = "0";
                    tbEntryPayment.Text = "0";
                }
            }
        }

        private void tbEntryDeductAmount_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                if (tbEntryDeductAmount.Text.Length > 0)
                {
                    decimal deductAmount = Convert.ToDecimal(tbEntryDeductAmount.Text.Replace(",", ""));
                    //decimal farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.Replace(",", ""));

                    decimal farmerAdvance = 0M;
                    if (advanceUpdated)
                    {
                        farmerAdvance = Convert.ToDecimal(tbCashAdvance.Text.ToString().Replace(",", ""));
                    }

                    if (farmerAdvance <= 0)
                    {
                        MessageBox.Show("Farmer Advance sama dengan Rp. 0 \n  tidak dapat di kurangi lagi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    try
                    {
                        deductPct = (deductAmount / farmerAdvance);
                    }
                    catch (Exception x)
                    {
                        deductPct = 0;

                    }


                    decimal receiptAmount = Convert.ToDecimal(tbEntryReceiptAmount.Text.Replace(",", ""));
                    decimal taxAmount = Math.Round((taxPct * receiptAmount), 2);

                    decimal paymentAmount = receiptAmount - taxAmount - deductAmount;

                    tbEntryReceiptAmount.Text = receiptAmount.ToString("N2");
                    tbEntryTaxAmount.Text = taxAmount.ToString("N2");
                    tbEntryDeductPct.Text = deductPct.ToString("P2");
                    tbEntryPayment.Text = paymentAmount.ToString("N2");
                }
                else
                {
                    tbEntryDeductPct.Text = "0";
                    tbEntryPayment.Text = "0";
                }
            }
        }

        #endregion TBInputCheck

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

        private void tbEntryDoc_TextChanged(object sender, EventArgs e)
        {
            if (tbEntryDoc.Text != "" && tbStatus.Text != "SYNCED")
            {
                if (tbDocNumber.Text != "<NEW>") { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
            }
            else
            {
                btnSaveLot.Enabled = false;
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

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void groupEntry_Enter(object sender, EventArgs e)
        {

        }

        private void addonreceiptamount_TextChanged(object sender, EventArgs e)
        {

        }

        private void unsyncing_Click(object sender, EventArgs e)
        {
            loadDetail();
            if (dtDetail.Rows.Count > 0)
            {


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

                            foreach (DataRow rowView in dtDetail.Rows)
                            {
                                var rowDocNumber = rowView[0].ToString();
                                var rowReceiptNumber = rowView[1].ToString();

                                using (SqlCommand command = new SqlCommand("Update_PurchaseInvoiceDetail_Sync", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", rowDocNumber);
                                    command.Parameters.AddWithValue("@ReceiptID", rowReceiptNumber);
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
    }
}
