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
using QRCoder;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class ShipmentInfo : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public UserModel Userlog { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }

        private int lastIncrementValue = -1;
        private int SOLine = 0;

        private DataTable dtDetail;
        private DataTable dtAllocation;
        private DataTable dtLot;
        private DataTable dtEntry;

        private string LotStockItem = null;

        private string subItemLot = "";
        private decimal allocationLimit = 0M;

        public ShipmentInfo()
        {
            InitializeComponent();
        }

        private void ShipmentInfo_Load(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
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
            tbShipmentDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";
            subItemLot = "";
            //AcumaticaRefNbr = "";

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}]";

            loadComboLot();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbTotalQty.Text = "0";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbAcumaticaRefNbr.Text = "";
            tbCustOrderNo.Text = "";
            tbCustExtReff.Text = "";
            tbDescription.Text = "";
            tbLogisticService.Text = "";
            tbLisencePlate.Text = "";

            checkHold.Checked = true;

            loadDetail();
            resetEntry();
            tbLot.Focus();
        }

        private void resetEntry()
        {
            loadAllocation();

            groupEntry.Text = "Lot Entry [<NEW>]";

            tbEntryLot.Text = "";
            tbEntryInv.Text = "";
            tbEntryArea.Text = "";
            tbEntryGrade.Text = "";
            tbEntrySubItem.Text = "";
            tbEntryWeightReceive.Text = "";
            tbEntryWeightTare.Text = "";
            tbEntryWeightNetto.Text = "";


            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnAcumatica.Enabled = false;
                    btnAcumaticaShipment.Enabled = true;
                    btnSave.Enabled = true;
                    btnSaveLot.Enabled = true;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintWL.Enabled = true;
                    btnPrintSJ.Enabled = false;
                    btnPrintWLSum.Enabled = true;
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
                    btnAcumaticaShipment.Enabled = false;
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = false;
                    btnPrintWL.Enabled = true;
                    btnPrintSJ.Enabled = true;
                    btnPrintWLSum.Enabled = true;
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
                    btnAcumaticaShipment.Enabled = false;
                    btnSave.Enabled = true;
                    btnSaveLot.Enabled = true;
                    btnDelLot.Enabled = false;
                    checkHold.Enabled = true;
                    btnPrintWL.Enabled = false;
                    btnPrintSJ.Enabled = false;
                    btnPrintWLSum.Enabled = false;
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
            tbShipmentDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}]";

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from ShipmentInfo where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(3).ToString();
                        tbTotalQty.Text = Convert.ToDecimal(reader.GetValue(4)).ToString("N2");
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tbAcumaticaRefNbr.Text = reader.GetValue(6).ToString();
                        tbCustomerName.Text = reader.GetValue(7).ToString();
                        tbCustomerAddress.Text = reader.GetValue(8).ToString();
                        checkHold.Checked = Convert.ToBoolean(reader.GetValue(3).ToString() == "HOLD" ? 1 : 0);
                        tbAcumaticaShipmentNbr.Text = reader.GetValue(9).ToString();
                        tbDescription.Text = reader.GetValue(13).ToString();
                        tbLogisticService.Text = reader.GetValue(14).ToString();
                        tbLisencePlate.Text = reader.GetValue(15).ToString();
                        dtpShippingDate.Value = DateTime.Parse(reader.GetValue(16).ToString());
                        tbCustOrderNo.Text = reader.GetValue(17).ToString();
                        tbCustExtReff.Text = reader.GetValue(18).ToString();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }

            loadDetail();
            resetEntry();
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
	                                        ShipmentInfoDetail
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
                    dgvDetail.Columns[1].HeaderText = "Warehouse ID";
                    dgvDetail.Columns[2].HeaderText = "Inventory ID";
                    dgvDetail.Columns[3].HeaderText = "Sub Item";
                    dgvDetail.Columns[4].HeaderText = "Open Qty";
                    dgvDetail.Columns[4].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[5].HeaderText = "UoM";
                    dgvDetail.Columns[7].HeaderText = "Ordered Qty";
                    dgvDetail.Columns[7].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[8].HeaderText = "Qty On Shipments";
                    dgvDetail.Columns[8].DefaultCellStyle.Format = "N2";

                    dgvDetail.ClearSelection();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadAllocation()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtAllocation = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        ShipmentInfoAllocation
                                        WHERE
	                                        DocumentID = '{DocNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtAllocation);
                    dgvAlloc.DataSource = dtAllocation;

                    //Header Text
                    dgvAlloc.Columns[0].HeaderText = "Document ID";
                    dgvAlloc.Columns[0].Visible = false;
                    dgvAlloc.Columns[1].HeaderText = "Inventory ID";
                    dgvAlloc.Columns[2].HeaderText = "Sub Item";
                    dgvAlloc.Columns[3].HeaderText = "Lot Number";
                    dgvAlloc.Columns[4].HeaderText = "Source";
                    dgvAlloc.Columns[5].HeaderText = "Stage";
                    dgvAlloc.Columns[6].HeaderText = "Form";
                    dgvAlloc.Columns[7].HeaderText = "Crop Year";
                    dgvAlloc.Columns[8].HeaderText = "Grade";
                    dgvAlloc.Columns[9].HeaderText = "Area";
                    dgvAlloc.Columns[10].HeaderText = "Color";
                    dgvAlloc.Columns[11].HeaderText = "Fermentation";
                    dgvAlloc.Columns[12].HeaderText = "Length";
                    dgvAlloc.Columns[13].HeaderText = "Process";
                    dgvAlloc.Columns[14].HeaderText = "Stalk Position";
                    dgvAlloc.Columns[15].HeaderText = "Rope";
                    dgvAlloc.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvAlloc.Columns[16].HeaderText = "Shipping";
                    dgvAlloc.Columns[16].DefaultCellStyle.Format = "N2";
                    dgvAlloc.Columns[17].HeaderText = "Receive";
                    dgvAlloc.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvAlloc.Columns[18].HeaderText = "Tare";
                    dgvAlloc.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvAlloc.Columns[19].HeaderText = "Netto";
                    dgvAlloc.Columns[19].DefaultCellStyle.Format = "N2";
                    dgvAlloc.Columns[20].HeaderText = "UoM";
                    dgvAlloc.Columns[21].HeaderText = "Remark";
                    dgvAlloc.Columns[22].HeaderText = "Old Document ID";
                    dgvAlloc.Columns[23].HeaderText = "Synced";
                    dgvAlloc.Columns[25].HeaderText = "Marking ID";
                    dgvAlloc.Columns[26].HeaderText = "Head Mark / Customer Grade";
                    dgvAlloc.Columns[27].HeaderText = "Crop Year";
                    dgvAlloc.Columns[28].HeaderText = "Inventory ID";
                    dgvAlloc.Columns[29].HeaderText = "Note";

                    dgvAlloc.ClearSelection();

                    if (dgvAlloc.Rows.Count > 0)
                    {
                        int countLot = Convert.ToInt32(dtAllocation.Compute("COUNT(LotNbr)", string.Empty));
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWShipping = Convert.ToDecimal(dtAllocation.Compute("SUM(WeightShipping)", string.Empty));
                        tbAllocShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = Convert.ToDecimal(dtAllocation.Compute("SUM(WeightReceive)", string.Empty));
                        tbAllocReceiving.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = Convert.ToDecimal(dtAllocation.Compute("SUM(WeightTare)", string.Empty));
                        tbAllocTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = Convert.ToDecimal(dtAllocation.Compute("SUM(WeightNetto)", string.Empty));
                        tbAllocNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                        tbAcumaticaRefNbr.ReadOnly = true;
                        tbAcumaticaRefNbr.BackColor = System.Drawing.SystemColors.Info;
                    }
                    else
                    {
                        int countLot = 0;
                        tbDetailLot.Text = countLot.ToString();
                        decimal sumWShipping = 0;
                        tbAllocShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = 0;
                        tbAllocReceiving.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = 0;
                        tbAllocTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = 0;
                        tbAllocNetto.Text = sumWNetto.ToString("N2");
                        //disable change reg
                        tbAcumaticaRefNbr.ReadOnly = false;
                        tbAcumaticaRefNbr.BackColor = System.Drawing.SystemColors.Window;
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

            var currentIncrement = lastIncrementValue + 1;
            //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-SO-{currentIncrement.ToString().PadLeft(4, '0')}";
            tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-SO-{currentIncrement.ToString().PadLeft(4, '0')}";
        }

        private void getDocLastIncrement()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from NumberingSetting where NumberingID = 'SO'";
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

            if (DocNumber == "<NEW>")
            {
                incrementError = true;
                setDocNumber();
                //tbStatus.Text = "OPEN";

                if (lastIncrementValue >= 0)
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

                        using (SqlCommand command = new SqlCommand("Insert_ShipmentInfo", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                            command.Parameters.AddWithValue("@DocumentDate", tbShipmentDate.Text);
                            command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                            command.Parameters.AddWithValue("@Status", tbStatus.Text);
                            command.Parameters.AddWithValue("@TotalQty", tbTotalQty.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@TotalAllocation", tbAllocNetto.Text.Replace(",", ""));
                            command.Parameters.AddWithValue("@AcumaticaRefNbr", tbAcumaticaRefNbr.Text);
                            command.Parameters.AddWithValue("@CustomerName", tbCustomerName.Text);
                            command.Parameters.AddWithValue("@CustomerLocation", tbCustomerAddress.Text);
                            command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                            command.Parameters.AddWithValue("@AcumaticaShipmentNbr", tbAcumaticaShipmentNbr.Text);
                            command.Parameters.AddWithValue("@SODescription", tbDescription.Text);
                            command.Parameters.AddWithValue("@LogisticService", tbLogisticService.Text);
                            command.Parameters.AddWithValue("@LisencePlate", tbLisencePlate.Text);
                            command.Parameters.AddWithValue("@ShippingDate", dtpShippingDate.Value.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@CustOrderNo", tbCustOrderNo.Text);
                            command.Parameters.AddWithValue("@CustExtReff", tbCustExtReff.Text);

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
                        if (!insertError)
                        {
                            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}]";

                            using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@NumberingID", $"SO");
                                command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                                command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                command.ExecuteNonQuery();
                            }

                            //MessageBox.Show("Process complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show($"Failed to get numbering setting, please check database connection", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }



            if (tbStatus.Text == "OPEN" && !checkHold.Checked)
            {
                btnAcumatica.Enabled = false;
                btnAcumaticaShipment.Enabled = true;
            }
            else
            {
                btnAcumatica.Enabled = true;
                btnAcumaticaShipment.Enabled = false;
            }

            if (tbStatus.Text == "OPEN" || tbStatus.Text == "SYNCED")
            {
                btnPrintWL.Enabled = true;
                btnPrintWLSum.Enabled = true;
            }
            else
            {
                btnPrintWL.Enabled = false;
                btnPrintWLSum.Enabled = false;
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
            saveDocument();
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            resetScreen();
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            if (tbAcumaticaRefNbr.TextLength < 5)
            {
                MessageBox.Show($"Please check dispatch out number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                saveDocument();
                acumaticaSync();
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

        private void acumaticaSync()
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            string OrderNbr = tbAcumaticaRefNbr.Text;
            SalesOrder salesOrder;

            removeDetails();

            //get transfer DATA
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
            try
            {
                var salesOrderApi = new SalesOrderApi(configuration);
                var responseSales = salesOrderApi.GetList(expand: "Details, ShipToContact,ShipToAddress", filter: $"OrderType eq 'SO' and OrderNbr eq '{OrderNbr}'");

                if (responseSales.Count > 0)
                {
                    salesOrder = responseSales[0];

                    tbTotalQty.Text = Convert.ToDecimal(salesOrder.OrderedQty.Value).ToString("N2");
                    tbCustomerName.Text = $"{salesOrder.CustomerID.Value} - {salesOrder.ShipToContact.BusinessName.Value}";
                    tbCustomerAddress.Text = $"{salesOrder.ShipToAddress.City.Value} - {salesOrder.ShipToAddress.Country.Value}";
                    tbCustOrderNo.Text = $"{salesOrder.CustomerOrder.Value}";
                    tbCustExtReff.Text = $"{salesOrder.ExternalRef.Value}";
                    tbDescription.Text = salesOrder.Description.Value;
                    saveDetails(salesOrder.Details);
                }
                else
                {
                    MessageBox.Show($"Sales order {OrderNbr} is not available", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}]";
                    saveDocument();
                    loadDetail();
                    resetEntry();
                }
            }
        }

        private void saveDetails(List<SalesOrderDetail> salesOrderDetails)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    foreach (SalesOrderDetail salesOrderDetail in salesOrderDetails)
                    {
                        Console.WriteLine(salesOrderDetail);
                        using (SqlCommand command = new SqlCommand("Insert_ShipmentInfoDetail_C", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", DocNumber);
                            command.Parameters.AddWithValue("@WarehouseID", salesOrderDetail.Branch.Value);
                            command.Parameters.AddWithValue("@InventoryID", salesOrderDetail.InventoryID.Value);
                            command.Parameters.AddWithValue("@SubItem", salesOrderDetail.Subitem.Value);
                            command.Parameters.AddWithValue("@Weight", salesOrderDetail.OpenQty.Value ?? 0);
                            command.Parameters.AddWithValue("@UoM", salesOrderDetail.UOM.Value);
                            command.Parameters.AddWithValue("@SOLine", salesOrderDetail.LineNbr.Value);
                            command.Parameters.AddWithValue("@Ext_OrderQty", salesOrderDetail.OrderQty.Value ?? 0);
                            command.Parameters.AddWithValue("@Ext_QtyOnShipments", salesOrderDetail.QtyOnShipments.Value ?? 0);

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
                    loadDetail();
                }
            }
        }

        private void removeDetails()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Clear_ShipmentInfoDetail", connection))
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
                    //loadDetail();
                    //resetEntry();
                    tbTotalQty.Text = "0";

                    cbLot.SelectedIndex = -1;
                    cbLot.Items.Clear();
                }
            }
        }

        private void loadComboLot()
        {
            cbLot.SelectedIndex = -1;
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
	                                    StockItem.StatusStock = 1
                                    AND REPLACE(SubItem, '.', '') = '{subItemLot}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtLot);
                    string[] arrray = dtLot.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    new AutoCompleteBehavior(cbLot);
                    cbLot.Items.Clear();
                    cbLot.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void dgvDetail_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDetail.SelectedRows.Count > 0)
            {
                SOLine = Convert.ToInt32(dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[6].FormattedValue.ToString());
                subItemLot = dgvDetail.Rows[dgvDetail.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
                loadComboLot();
            }
        }

        private void cbLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLot.SelectedIndex >= 0)
            {
                loadEntry(cbLot.SelectedItem.ToString());
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
                if (groupEntry.Text.Contains("<NEW>"))
                {
                    if (tbEntryWeightNetto.Text != "0" && weightCheck())
                    {
                        saveLot();
                    }
                    else
                    {
                        MessageBox.Show($"New lot has 0 weight, or total weight is larger than allocation limit ({allocationLimit.ToString("N2")} KG)!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    saveLot();
                }

                tbLot.Text = "";
                saveDocument();
                tbLot.Focus();
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

                    using (SqlCommand command = new SqlCommand("Insert_ShipmentInfoAllocation_C", connection))
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
                        command.Parameters.AddWithValue("@SOLine", SOLine);
                        command.Parameters.AddWithValue("@Ext_MarkingId", Ext_MarkingId.Text);
                        command.Parameters.AddWithValue("@Ext_HeadMark_GradeCustomer", Ext_HeadMark_GradeCustomer.Text);
                        command.Parameters.AddWithValue("@Ext_CropYear", Ext_CropYear.Text);
                        command.Parameters.AddWithValue("@Ext_Quality", Ext_Quality.Text);
                        command.Parameters.AddWithValue("@Ext_Note", Ext_Note.Text);
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
                    loadAllocation();
                    groupEntry.Text = $"Lot Entry [{tbEntryLot.Text}]";
                    cbLot.SelectedIndex = -1;
                    loadComboLot();
                }
            }
        }

        private bool weightCheck()
        {
            decimal currentAllocation = Convert.ToDecimal(tbAllocNetto.Text.Replace(",", ""));
            decimal newWeight = Convert.ToDecimal(tbEntryWeightNetto.Text.Replace(",", ""));

            if (allocationLimit >= (currentAllocation + newWeight))
            {
                return true;
            }
            else
            {
                return false;
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

                    using (SqlCommand command = new SqlCommand("Delete_ShipmentInfoAllocation", connection))
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
                    loadAllocation();
                    resetEntry();
                    cbLot.SelectedIndex = -1;
                    loadComboLot();
                }
            }
        }
        private void dgvAlloc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAlloc.SelectedRows.Count > 0)
            {
                groupEntry.Text = $"Lot Entry [{dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[3].FormattedValue.ToString()}]";

                tbEntryLot.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();
                tbEntryInv.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[1].FormattedValue.ToString();
                tbEntrySubItem.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[2].FormattedValue.ToString();
                tbEntryGrade.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[8].FormattedValue.ToString();
                tbEntryArea.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[9].FormattedValue.ToString();

                tbEntryWeightReceive.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[17].Value.ToString();
                tbEntryWeightTare.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[18].Value.ToString();
                tbEntryWeightNetto.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[19].Value.ToString();

                Ext_MarkingId.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[25].FormattedValue.ToString();
                Ext_HeadMark_GradeCustomer.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[26].FormattedValue.ToString();
                Ext_CropYear.Text= dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[27].FormattedValue.ToString();
                Ext_Quality.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[28].FormattedValue.ToString();
                Ext_Note.Text = dgvAlloc.Rows[dgvAlloc.SelectedRows[0].Index].Cells[29].FormattedValue.ToString();

                btnSaveLot.Enabled = false;
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

        private void btnDelLot_Click(object sender, EventArgs e)
        {
            removeLot();
            saveDocument();
        }

        private void tbTotalQty_TextChanged(object sender, EventArgs e)
        {
            decimal currentOrderQty = Convert.ToDecimal(tbTotalQty.Text.Replace(",", ""));
            allocationLimit = currentOrderQty * 1.1M;
        }

        private void btnAcumaticaShipment_Click(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Marking [{DocNumber}] - Syncing with Acumatica, please wait!";
            bool syncError = false;
            string referenceNbr = "";

            loadAllocation();
            DataView dv_filter = new DataView(dtAllocation, $"SyncDetail = 0", "SubItem Asc", DataViewRowState.CurrentRows);
            Console.WriteLine(dtAllocation);
            Shipment shipment = new Shipment();
            //header shipment
            shipment.WarehouseID = Warehouse.WarehouseID;
            shipment.Type = "Shipment";
            shipment.CustomerID = tbCustomerName.Text.Split('-')[0].Trim();
            //shipment.ShipmentDate = Convert.ToDateTime(tbShipmentDate.Text);
            shipment.ShipmentDate = Convert.ToDateTime(dtpShippingDate.Value.ToString("yyyy-MM-dd"));
            shipment.Hold = false;

            List<ShipmentDetail> listDetail = new List<ShipmentDetail>();
       
            //document details
            //todo => harus di kelompokkan based on SOLine
            var curIndex = "";
            int indexarray = -1;
            foreach (DataRowView rowView in dv_filter)
            {
                ShipmentDetail detail = new ShipmentDetail();

                detail.WarehouseID = Warehouse.WarehouseID;
                detail.InventoryID = rowView[1].ToString();
                detail.LocationID = AcumaticaCred.AcumaticaInvLocation;
                detail.OrderType = "SO";
                detail.OrderNbr = tbAcumaticaRefNbr.Text;
                detail.OrderLineNbr = Convert.ToInt32(rowView[24]);
                detail.ShippedQty = Convert.ToDecimal(rowView[19]);
                detail.Subitem = rowView[2].ToString().Replace(".", "");
                //detail.TransactionDescription = rowView[0].ToString();
                detail.Description = rowView[3].ToString();
                detail.UOM = rowView[20].ToString();
                detail.WarehouseID = tbWarehouse.Text;


                if (detail.Subitem.Value != curIndex)
                {
                    indexarray++;
                    listDetail.Add(detail);
                    curIndex = detail.Subitem.Value;

                }
                else
                {

                    listDetail[indexarray].ShippedQty.Value = listDetail[indexarray].ShippedQty.Value + detail.ShippedQty.Value;
                }

            }
            shipment.Details = listDetail;

            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);
            var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);

            Console.WriteLine(shipment);
            try
            {
                var shipmentApi = new ShipmentApi(configuration);
                var response = shipmentApi.PutEntity(shipment);

                referenceNbr = response.ShipmentNbr.ToString();
                tbAcumaticaShipmentNbr.Text = referenceNbr;
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
                    this.Text = $"Universal Leaf [{Warehouse.Descr}] - Shimpment Info [{DocNumber}]";
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

                                using (SqlCommand command = new SqlCommand("Update_ShipmentInfoAllocation_Sync", connection))
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
	                                        ShipmentInfoAllocation.DocumentID, 
	                                        ShipmentInfoAllocation.InventoryID, 
	                                        ShipmentInfoAllocation.SubItem, 
	                                        ShipmentInfoAllocation.LotNbr, 
	                                        SegmentValue.Descr AS Stage, 
	                                        ShipmentInfoAllocation.WeightNetto as OldNetto, 
	                                        ShipmentInfoAllocation.WeightReceive, 
	                                        ShipmentInfoAllocation.WeightTare, 
	                                        ShipmentInfoAllocation.WeightNetto as NewNetto, 
	                                        ShipmentInfoAllocation.UoM, 
	                                        ShipmentInfoAllocation.Remark, 
	                                        CONCAT(ShipmentInfo.LogisticService,'/',ShipmentInfo.LisencePlate) as Note
                                        FROM
	                                        dbo.ShipmentInfoAllocation
	                                        INNER JOIN
	                                        dbo.ShipmentInfo
	                                        ON 
		                                        ShipmentInfoAllocation.DocumentID = ShipmentInfo.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        ShipmentInfoAllocation.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE ShipmentInfoAllocation.DocumentID = '{DocNumber}'
                                        ORDER BY
	                                        ShipmentInfoAllocation.LotNbr";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(myDispatch.WeightListLineDetail2);
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

            //WeightListShipPrint weightListShipPrint = new WeightListShipPrint
            WeightListPrint weightListPrint = new WeightListPrint
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "SHIPMENT",
                DocStatus = tbStatus.Text,
                DispatchDate = tbShipmentDate.Text,
                DispatchDetails = myDispatch,
                FromTo = tbCustomerName.Text,
                QRImage = QRImage,
                LogisticService= tbLogisticService.Text,
                LisencePlate=tbLisencePlate.Text
            };
            //weightListShipPrint.ShowDialog();
            weightListPrint.ShowDialog();
        }

        private void btnPrintSJ_Click(object sender, EventArgs e)
        {

            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        ShipmentInfoAllocation.InventoryID, 
	                                        InventoryItem.Descr,
	                                        COUNT(ShipmentInfoAllocation.LotNbr) AS PackQty, 
	                                        SUM(ShipmentInfoAllocation.WeightReceive) AS OldWeightNetto,
	                                        SUM(ShipmentInfoAllocation.WeightNetto) AS WeightNetto
                                        FROM
	                                        dbo.ShipmentInfoAllocation
	                                        INNER JOIN
	                                        dbo.InventoryItem
	                                        ON 
		                                        ShipmentInfoAllocation.InventoryID = InventoryItem.InventoryID
                                        WHERE ShipmentInfoAllocation.DocumentID = '{DocNumber}'
                                        GROUP BY
	                                        ShipmentInfoAllocation.InventoryID,
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
                Warehouse2 = tbCustomerName.Text,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                Address2 = tbCustomerAddress.Text,
                DocNumber = tbDocNumber.Text,
                DocType = "SHIPMENT",
                //DocDate = tbShipmentDate.Text,
                DocDate = dtpShippingDate.Value.ToString("yyyy-MM-dd"),
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

        private void btnPrintWLSum_Click(object sender, EventArgs e)
        {
            DataSetAddon myDispatch = new DataSetAddon();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {

                    string query = $@"SELECT
	                                        ShipmentInfoAllocation.DocumentID, 
	                                        ShipmentInfoAllocation.InventoryID, 
	                                        ShipmentInfoAllocation.SubItem, 
	                                        COUNT(ShipmentInfoAllocation.LotNbr) AS LotNbr, 
	                                        SegmentValue.Descr AS Stage, 
	                                        SUM(ShipmentInfoAllocation.WeightNetto) as OldNetto, 
	                                        SUM(ShipmentInfoAllocation.WeightReceive) AS WeightReceive,  
	                                        SUM(ShipmentInfoAllocation.WeightTare) AS WeightTare, 
	                                        SUM(ShipmentInfoAllocation.WeightNetto) AS WeightNetto, 
	                                        ShipmentInfoAllocation.UoM, 
	                                        ShipmentInfoAllocation.Remark, 
	                                        CONCAT(ShipmentInfo.LogisticService,'/',ShipmentInfo.LisencePlate) as Note
                                        FROM
	                                        dbo.ShipmentInfoAllocation
	                                        INNER JOIN
	                                        dbo.ShipmentInfo
	                                        ON 
		                                        ShipmentInfoAllocation.DocumentID = ShipmentInfo.DocumentID
	                                        INNER JOIN
	                                        dbo.SegmentValue
	                                        ON 
		                                        ShipmentInfoAllocation.Stage = SegmentValue.SegmentValue AND
		                                        SegmentValue.SegmentID = 1
                                        WHERE ShipmentInfoAllocation.DocumentID = '{DocNumber}'
										GROUP BY
											ShipmentInfoAllocation.DocumentID, 
	                                        ShipmentInfoAllocation.InventoryID, 
	                                        ShipmentInfoAllocation.SubItem, 
	                                        SegmentValue.Descr, 
	                                        ShipmentInfoAllocation.UoM, 
	                                        ShipmentInfoAllocation.Remark, 
	                                        CONCAT(ShipmentInfo.LogisticService,'/',ShipmentInfo.LisencePlate)

                                        ORDER BY
											ShipmentInfoAllocation.DocumentID, 
	                                        ShipmentInfoAllocation.InventoryID, 
	                                        ShipmentInfoAllocation.SubItem, 
	                                        SegmentValue.Descr, 
	                                        ShipmentInfoAllocation.UoM, 
	                                        ShipmentInfoAllocation.Remark, 
	                                        CONCAT(ShipmentInfo.LogisticService,'/',ShipmentInfo.LisencePlate)";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    //da.Fill(myDispatch.WeightListLineDetail2);
                    da.Fill(myDispatch.WeightListLineDetail);

                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.ToString());
                }
            }

            //WeightListShipPrint weightListShipPrint = new WeightListShipPrint

            WeightListShipPrintSum weightListShipPrintSum = new WeightListShipPrintSum
            {
                Company = Warehouse.Company,
                Warehouse = Warehouse.Descr,
                Address = GetBranch(Warehouse.WarehouseID, 3),
                Phone = GetBranch(Warehouse.WarehouseID, 4),
                DocNumber = tbDocNumber.Text,
                DocType = "SHIPMENT",
                DocStatus = tbStatus.Text,
                DispatchDate = tbShipmentDate.Text,
                DispatchDetails = myDispatch
            };
            //weightListShipPrint.ShowDialog();
            weightListShipPrintSum.ShowDialog();

        }

        private void tbAcumaticaRefNbr_KeyPress(object sender, KeyPressEventArgs e)
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

                                using (SqlCommand command = new SqlCommand("Update_ShipmentInfoAllocation_Sync", connection))
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



        //end of file
    }
}