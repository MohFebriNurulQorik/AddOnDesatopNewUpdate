using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using Acumatica.ULT_18_200_001.Api;
using Acumatica.ULT_18_200_001.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class BatchAcumaticaSync : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public string DocType { get; set; }
        public string HeaderTable { get; set; }
        public string DetailTable { get; set; }
        public string AcumaticaReasonCode { get; set; }
        public int PriceScenario { get; set; }

        private int docCount = 0;

        private DataTable dtHeader;
        private DataTable dtDetail;

        public BatchAcumaticaSync()
        {
            InitializeComponent();
        }

        private void BatchAcumaticaSync_Load(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Acumatica Sync Process [{DocType}]";

            loadHeader();

            setText($"{docCount} 'OPEN' Document(s) loaded!", tbInfo);
            pbSync.Value = 0;
        }

        #region ThreadSafe

        private delegate void addTextCallback(string someText, Control tb);

        private delegate void setTextCallback(string someText, Control tb);

        private void addText(string someText, Control tb)
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

        private void setText(string someText, Control tb)
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

        private void loadHeader()
        {
            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                dtHeader = new DataTable();
                try
                {
                    //string query = $"select * from {HeaderTable} where Status != 'SYNCED' and WarehouseID = '{Warehouse.WarehouseID}'";
                    string query = $@"select 
                                        {HeaderTable}.*,
                                        {DetailTable}.DocumentID as Detail
                                        from
	                                        {HeaderTable}
	                                    LEFT JOIN
	                                        {DetailTable}
	                                    ON 
		                                    {HeaderTable}.DocumentID = {DetailTable}.DocumentID
                                        where Status = 'OPEN' and WarehouseID = '{Warehouse.WarehouseID}'
                                        	and {DetailTable}.DocumentID is not NULL";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtHeader);
                    dgvList.DataSource = dtHeader;

                    //Header Text
                    dgvList.Columns[0].HeaderText = "Document ID";
                    dgvList.Columns[1].HeaderText = "Date";
                    dgvList.Columns[2].HeaderText = "Warehouse ID";

                    for (int i = 3; i < dtHeader.Columns.Count; i++)
                    {
                        if (dgvList.Columns[i].HeaderText == "Status")
                        {
                            dgvList.Columns[i].Visible = true;
                        }
                        else
                        {
                            dgvList.Columns[i].Visible = false;
                        }
                    }

                    dgvList.ClearSelection();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            //docCount = dtHeader.Rows.Count;
            DataView dvHeader_filter = new DataView(dtHeader, $"Status = 'OPEN'", "DocumentID Asc", DataViewRowState.CurrentRows);
            docCount = dvHeader_filter.Count;

            tbListCount.Text = docCount.ToString();

            if (docCount > 0)
            {
                btnAcumatica.Enabled = true;
            }
            else
            {
                btnAcumatica.Enabled = false;
            }
        }

        private void loadDetail(string docNumber)
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
	                                        {DetailTable}
                                        WHERE
	                                        DocumentID = '{docNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
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

        private decimal checkReleasedIssue(string referenceNbr, Configuration configuration)
        {
            var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
            decimal TotalCost = 0M;
            while (true)
            {
                var response = inventoryIssueApi.GetByKeys(new List<string>() { referenceNbr });

                if (response.Status == "Released")
                {
                    TotalCost = (decimal)response.TotalCost.Value;
                    //tbTotalCost.Text = TotalCost.ToString("N2");

                    break;
                }
            }

            return TotalCost;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            loadHeader();

            setText($"{docCount} 'OPEN' Document(s) loaded!", tbInfo);
            pbSync.Value = 1;
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            loadHeader();

            setText($"Syncing with Acumatica, please wait!", tbInfo);

            pbSync.Value = 1;
            pbSync.Minimum = 1;
            pbSync.Maximum = 100;
            pbSync.Step = 100 / docCount + (100 % docCount > 0 ? 1 : 0);

            var bgw = new BackgroundWorker();
            bgw.ProgressChanged += bgw_ProgressChanged;

            switch (DocType)
            {
                case "Buying":
                    //syncPurchaseReceipt();
                    bgw.DoWork += bgw_syncPurchaseReceipt;
                    break;

                case "Bir-Bir":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Butting":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Stripping":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Loosing":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Pras 1":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Pras 2":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Sortasi":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Final Sortasi":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Sortasi Packing":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Temporary Packing":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Final Packing":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;

                case "Fermenting":
                    if (AcumaticaReasonCode.Contains("ISSUE"))
                    {
                        bgw.DoWork += bgw_syncGenericIN;
                    }
                    else if (AcumaticaReasonCode.Contains("RECEIPT"))
                    {
                        bgw.DoWork += bgw_syncGenericOUT;
                    }
                    break;
            }
            bgw.WorkerReportsProgress = true;
            bgw.RunWorkerAsync();
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbSync.PerformStep();
            loadHeader();
        }

        private void bgw_syncPurchaseReceipt(object sender, DoWorkEventArgs e)
        {
            int orDocCount = docCount;
            DataView dvHeader_filter = new DataView(dtHeader, $"Status = 'OPEN'", "DocumentID Asc", DataViewRowState.CurrentRows);
            int i = 0;

            foreach (DataRowView rowViewHeader in dvHeader_filter)
            {
                bool syncError = false;
                string referenceNbr = "";
                string docNumber = rowViewHeader[0].ToString();
                i = i + 1;

                var docBranch = GetBranch(rowViewHeader[2].ToString(),2);

                setText($"Processing document [{docNumber}]! [{i} out of {orDocCount} ]", tbInfo);
                //tbInfo.Text = $"Processing document [{docNumber}]! [{i} out of {docCount} ]";

                PurchaseReceipt purchaseReceipt = new PurchaseReceipt();
                //header receipt
                //purchaseReceipt.Branch = rowViewHeader[2].ToString();
                purchaseReceipt.Branch = docBranch;
                purchaseReceipt.Type = "Receipt";
                purchaseReceipt.VendorID = rowViewHeader[3].ToString();
                purchaseReceipt.VendorRef = docNumber;
                purchaseReceipt.Date = Convert.ToDateTime(rowViewHeader[1].ToString());
                purchaseReceipt.Hold = false;

                loadDetail(docNumber);
                DataView dv_filter = new DataView(dtDetail, $"StatusReject = 0 and SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

                List<PurchaseReceiptDetail> listDetail = new List<PurchaseReceiptDetail>();

                //document details
                foreach (DataRowView rowView in dv_filter)
                {
                    PurchaseReceiptDetail detail = new PurchaseReceiptDetail();

                    //detail.Branch = rowViewHeader[2].ToString();
                    detail.Branch = docBranch;
                    detail.InventoryID = rowView[1].ToString();
                    detail.Location = AcumaticaCred.AcumaticaInvLocation;
                    detail.ReceiptQty = Convert.ToDecimal(rowView[15]);
                    detail.Subitem = rowView[2].ToString().Replace(".", "");
                    detail.TransactionDescription = rowView[3].ToString();
                    detail.UnitCost = Convert.ToDecimal(rowView[17]);
                    detail.ExtendedCost = Convert.ToDecimal(rowView[21]);
                    detail.UOM = rowView[16].ToString();
                    detail.Warehouse = rowViewHeader[2].ToString();

                    if (rowView[26].ToString() != "")
                    {
                        detail.POOrderNbr = rowView[26].ToString();
                        detail.POOrderType = "Normal";
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
                    var purchaseReceiptApi = new PurchaseReceiptApi(configuration);
                    var response = purchaseReceiptApi.PutEntity(purchaseReceipt);
                    ReleasePurchaseReceipt releasePurchaseReceipt = new ReleasePurchaseReceipt((PurchaseReceipt)response);
                    purchaseReceiptApi.InvokeAction(releasePurchaseReceipt);

                    referenceNbr = response.ReceiptNbr.ToString();
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
                        //update doc
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlCommand command = new SqlCommand("Insert_BuyingLine", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", rowViewHeader[0].ToString());
                                    command.Parameters.AddWithValue("@DocumentDate", rowViewHeader[1].ToString());
                                    command.Parameters.AddWithValue("@WarehouseID", rowViewHeader[2].ToString());
                                    command.Parameters.AddWithValue("@VendorID", rowViewHeader[3].ToString());
                                    command.Parameters.AddWithValue("@VendorDetails", rowViewHeader[4].ToString());
                                    command.Parameters.AddWithValue("@RegistrationNumber", rowViewHeader[5].ToString());
                                    command.Parameters.AddWithValue("@OrderNbr", rowViewHeader[6].ToString());
                                    command.Parameters.AddWithValue("@InventoryID", rowViewHeader[7].ToString());
                                    command.Parameters.AddWithValue("@VendorClass", rowViewHeader[8].ToString());
                                    command.Parameters.AddWithValue("@Status", "SYNCED");
                                    command.Parameters.AddWithValue("@AcumaticaRefNbr", referenceNbr);
                                    command.Parameters.AddWithValue("@BuyerName", rowViewHeader[12].ToString());
                                    command.Parameters.AddWithValue("@CreatorID", rowViewHeader[13].ToString());

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e_update)
                            {
                                MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                        setText($"Document [{docNumber}] completed! [{i} out of {orDocCount} ]", tbInfo);
                        ((BackgroundWorker)sender).ReportProgress(0);
                    }
                } //end finally
            } // end foreach dtHeader
        }//end sync acumatica

        private void bgw_syncGenericIN(object sender, DoWorkEventArgs e)
        {
            int orDocCount = docCount;
            DataView dvHeader_filter = new DataView(dtHeader, $"Status = 'OPEN'", "DocumentID Asc", DataViewRowState.CurrentRows);
            int i = 0;

            foreach (DataRowView rowViewHeader in dvHeader_filter)
            {
                bool syncError = false;
                bool allSynced = true;
                string referenceNbr = "";
                string docNbr = "";
                string docNumber = rowViewHeader[0].ToString();
                var docBranch = GetBranch(rowViewHeader[2].ToString(),2);

                i = i + 1;

                setText($"Processing document [{docNumber}]! [{i} out of {orDocCount} ]", tbInfo);
                //tbInfo.Text = $"Processing document [{docNumber}]! [{i} out of

                //issue
                ULTInventoryIssue inventoryIssue = new ULTInventoryIssue();
                inventoryIssue.Date = Convert.ToDateTime(rowViewHeader[1].ToString());
                inventoryIssue.Description = $"{rowViewHeader[2].ToString()} {DocType} Processing IN Transaction Issue";
                inventoryIssue.ExternalRef = docNumber;
                inventoryIssue.Hold = false;

                loadDetail(docNumber);
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

                List<ULTInventoryIssueDetail> issueDetails = new List<ULTInventoryIssueDetail>();

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
                    ULTInventoryIssueDetail issueDetail = new ULTInventoryIssueDetail();
                    issueDetail.InventoryID = rowView[1].ToString();
                    issueDetail.Branch = docBranch;
                    issueDetail.Location = AcumaticaCred.AcumaticaInvLocation;
                    issueDetail.Subitem = rowView[2].ToString().Replace(".", "");
                    issueDetail.Quantity = Convert.ToDecimal(rowView[19]);
                    issueDetail.Description = rowView[3].ToString();
                    issueDetail.Warehouse = rowViewHeader[2].ToString();
                    issueDetail.UOM = rowView[20].ToString();
                    issueDetail.ReasonCode = AcumaticaReasonCode;
                    //issueDetail.LotSerialNbr = "";

                    issueDetails.Add(issueDetail);
                }
                inventoryIssue.Details = issueDetails;

                if (allSynced)
                {
                    var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
                    var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                    try
                    {
                        var inventoryIssueApi = new ULTInventoryIssueApi(configuration);
                        var responseIssue = inventoryIssueApi.PutEntity(inventoryIssue);
                        ReleaseInventoryIssue releaseInventoryIssue = new ReleaseInventoryIssue((ULTInventoryIssue)responseIssue);
                        inventoryIssueApi.InvokeAction(releaseInventoryIssue);

                        referenceNbr = responseIssue.ReferenceNbr.ToString();
                    }
                    catch (Exception ePut)
                    {
                        MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        syncError = true;
                    }
                    finally
                    {
                        Decimal TotalCost = checkReleasedIssue(referenceNbr, configuration);

                        authApi.AuthLogout();
                        if (!syncError)
                        {
                            //update doc
                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                try
                                {
                                    if (connection.State != ConnectionState.Open)
                                    {
                                        connection.Open();
                                    }

                                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineIN", connection))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@DocumentID", rowViewHeader[0].ToString());
                                        command.Parameters.AddWithValue("@DocumentDate", rowViewHeader[1].ToString());
                                        command.Parameters.AddWithValue("@WarehouseID", rowViewHeader[2].ToString());
                                        command.Parameters.AddWithValue("@Status", "SYNCED");
                                        command.Parameters.AddWithValue("@TotalCost", TotalCost);
                                        command.Parameters.AddWithValue("@TotalWeight", rowViewHeader[5].ToString());
                                        command.Parameters.AddWithValue("@AcumaticaRefNbr", referenceNbr);
                                        command.Parameters.AddWithValue("@ProcessType", rowViewHeader[6].ToString());
                                        command.Parameters.AddWithValue("@BuyerName", rowViewHeader[9].ToString());
                                        command.Parameters.AddWithValue("@CreatorID", rowViewHeader[10].ToString());
                                        command.Parameters.AddWithValue("@Notes", rowViewHeader[13].ToString());
                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception e_update)
                                {
                                    //MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                    //MessageBox.Show($"--Update sync status error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }

                            setText($"Document [{docNumber}] completed! [{i} out of {orDocCount} ]", tbInfo);
                            ((BackgroundWorker)sender).ReportProgress(0);
                        }
                    }//end finally
                }//end if
            }//end dataheader
        }//end generic IN

        private void bgw_syncGenericOUT(object sender, DoWorkEventArgs e)
        {
            int orDocCount = docCount;
            DataView dvHeader_filter = new DataView(dtHeader, $"Status = 'OPEN'", "DocumentID Asc", DataViewRowState.CurrentRows);
            int i = 0;

            foreach (DataRowView rowViewHeader in dvHeader_filter)
            {
                bool syncError = false;
                bool allSynced = true;
                string referenceNbr = "";
                string docNbr = "";
                string docNumber = rowViewHeader[0].ToString();
                var docBranch = GetBranch(rowViewHeader[2].ToString(), 2);

                i = i + 1;

                setText($"Processing document [{docNumber}]! [{i} out of {orDocCount} ]", tbInfo);
                //tbInfo.Text = $"Processing document [{docNumber}]! [{i} out of

                //receipt
                InventoryReceipt inventoryReceipt = new InventoryReceipt();
                inventoryReceipt.Branch = docBranch;
                inventoryReceipt.Date = Convert.ToDateTime(rowViewHeader[1].ToString());
                inventoryReceipt.Description = $"{rowViewHeader[2].ToString()} {DocType} Processing OUT Transaction Receipt";
                inventoryReceipt.ExternalRef = docNumber;
                inventoryReceipt.Hold = false;

                loadDetail(docNumber);
                DataView dv_filter = new DataView(dtDetail, $"SyncDetail = 0", "LotNbr Asc", DataViewRowState.CurrentRows);

                //set unitcost
                decimal INCost = Convert.ToDecimal(rowViewHeader[5].ToString().Replace(",", ""));
                decimal INWeight = Convert.ToDecimal(rowViewHeader[6].ToString().Replace(",", ""));
                decimal DetailsWeight = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty));
                decimal DetailsNonZeroCost = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", "ZeroCost = 0"));
                decimal tempUnitPrice = 0;

                switch (PriceScenario)
                {
                    case 1:
                        if (DetailsNonZeroCost > 0) tempUnitPrice = INCost / DetailsNonZeroCost;
                        break;

                    case 2:
                        if (INWeight > 0) tempUnitPrice = INCost / INWeight;
                        break;
                }

                //detail
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
                    receiptDetail.WarehouseID = rowViewHeader[2].ToString();
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

                if (allSynced)
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
                    }
                    catch (Exception ePut)
                    {
                        MessageBox.Show($"--Sync error! {ePut.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        syncError = true;
                    }
                    finally
                    {
                        //Decimal TotalCost = checkReleasedIssue(referenceNbr, configuration);

                        authApi.AuthLogout();
                        if (!syncError)
                        {
                            //update doc
                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {
                                try
                                {
                                    if (connection.State != ConnectionState.Open)
                                    {
                                        connection.Open();
                                    }

                                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineOUT", connection))
                                    {
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@DocumentID", rowViewHeader[0].ToString());
                                        command.Parameters.AddWithValue("@DocumentDate", rowViewHeader[1].ToString());
                                        command.Parameters.AddWithValue("@WarehouseID", rowViewHeader[2].ToString());
                                        command.Parameters.AddWithValue("@Status", "SYNCED");
                                        command.Parameters.AddWithValue("@RefINNbr", rowViewHeader[4].ToString());
                                        command.Parameters.AddWithValue("@TotalCost", rowViewHeader[5].ToString());
                                        command.Parameters.AddWithValue("@TotalWeight", rowViewHeader[6].ToString());
                                        command.Parameters.AddWithValue("@AcumaticaRefNbr", referenceNbr);
                                        command.Parameters.AddWithValue("@ProcessType", rowViewHeader[7].ToString());
                                        command.Parameters.AddWithValue("@BuyerName", rowViewHeader[9].ToString());
                                        command.Parameters.AddWithValue("@CreatorID", rowViewHeader[10].ToString());
                                        command.Parameters.AddWithValue("@Notes", rowViewHeader[13].ToString());
                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception e_update)
                                {
                                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                            setText($"Document [{docNumber}] completed! [{i} out of {orDocCount} ]", tbInfo);
                            ((BackgroundWorker)sender).ReportProgress(0);
                        }
                    }//end finally
                }//end if
            }//end dataheader
        }//end generic OUT


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
        //end of file
    }
}