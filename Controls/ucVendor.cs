using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using Acumatica.ULT_18_200_001.Api;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucVendor : UserControl
    {
        public WarehouseModel Warehouse { get; set; }

        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }

        private int selRow = -10;
        private DataTable dtVendor;
        private DataTable dtVendorContract;
        private DataTable dtVendorPO;
        private DataTable dtVendorPrepayment;
        public static UserModel Userlog = new UserModel();

        public ucVendor()
        {
            InitializeComponent();
        }

        private void ucVendor_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Vendor List";
            LoadData();
        }

        private void LoadData()
        {
            selRow = -10;
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtVendor = new DataTable();
                try
                {
                    string query = $"SELECT * from VendorData";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtVendor);
                    dgvVendor.DataSource = dtVendor;

                    //Header Text
                    dgvVendor.Columns[0].HeaderText = "Vendor ID";
                    dgvVendor.Columns[0].Visible = true;
                    dgvVendor.Columns[1].HeaderText = "Vendor Name";
                    dgvVendor.Columns[1].Visible = true;
                    dgvVendor.Columns[2].HeaderText = "Status";
                    dgvVendor.Columns[2].Visible = true;
                    dgvVendor.Columns[3].HeaderText = "Display name";
                    dgvVendor.Columns[3].Visible = false;
                    dgvVendor.Columns[4].HeaderText = "Phone 1";
                    dgvVendor.Columns[4].Visible = false;
                    dgvVendor.Columns[5].HeaderText = "Phone 2";
                    dgvVendor.Columns[5].Visible = false;
                    dgvVendor.Columns[6].HeaderText = "Address Line 1";
                    dgvVendor.Columns[6].Visible = false;
                    dgvVendor.Columns[7].HeaderText = "Address Line 2";
                    dgvVendor.Columns[7].Visible = false;
                    dgvVendor.Columns[8].HeaderText = "City";
                    dgvVendor.Columns[9].HeaderText = "Country";
                    dgvVendor.Columns[9].Visible = false;
                    dgvVendor.Columns[10].HeaderText = "State";
                    dgvVendor.Columns[10].Visible = false;
                    dgvVendor.Columns[11].HeaderText = "Postal Code";
                    dgvVendor.Columns[11].Visible = false;
                    dgvVendor.Columns[12].HeaderText = "Vendor Class";
                    dgvVendor.Columns[13].HeaderText = "Last Modified";
                    dgvVendor.Columns[13].Visible = false;
                    dgvVendor.Columns[14].HeaderText = "No. NPWP";
                    dgvVendor.Columns[15].HeaderText = "Vendor Tax Agency";

                    clearInfo();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            DataView dv_filter = new DataView(dtVendor, $"VendorID LIKE '%{tbFilter.Text}%' or VendorName LIKE '%{tbFilter.Text}%' or City LIKE '%{tbFilter.Text}%' or VendorClass LIKE '%{tbFilter.Text}%'", "VendorID Asc", DataViewRowState.CurrentRows);
            dgvVendor.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtVendor, $"VendorID LIKE '%{tbFilter.Text}%' or VendorName LIKE '%{tbFilter.Text}%' or City LIKE '%{tbFilter.Text}%' or VendorClass LIKE '%{tbFilter.Text}%'", "VendorID Asc", DataViewRowState.CurrentRows);
                dgvVendor.DataSource = dv_filter;

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dgvVendor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            clearInfo();

            try
            {
                selRow = e.RowIndex;

                string VendorID = dgvVendor.Rows[selRow].Cells[0].FormattedValue.ToString().Replace("'", "''");

                //set info
                tbVendorName.Text = dgvVendor.Rows[selRow].Cells[1].FormattedValue.ToString();
                tbStatus.Text = dgvVendor.Rows[selRow].Cells[2].FormattedValue.ToString();
                tbDisplayName.Text = dgvVendor.Rows[selRow].Cells[3].FormattedValue.ToString();
                tbPhone1.Text = dgvVendor.Rows[selRow].Cells[4].FormattedValue.ToString();
                tbPhone2.Text = dgvVendor.Rows[selRow].Cells[5].FormattedValue.ToString();
                tbAddress1.Text = dgvVendor.Rows[selRow].Cells[6].FormattedValue.ToString();
                tbAddress2.Text = dgvVendor.Rows[selRow].Cells[7].FormattedValue.ToString();
                tbCity.Text = dgvVendor.Rows[selRow].Cells[8].FormattedValue.ToString();
                tbCountry.Text = dgvVendor.Rows[selRow].Cells[9].FormattedValue.ToString();
                tbState.Text = dgvVendor.Rows[selRow].Cells[10].FormattedValue.ToString();
                tbPostalCode.Text = dgvVendor.Rows[selRow].Cells[11].FormattedValue.ToString();
                tbVendorClass.Text = dgvVendor.Rows[selRow].Cells[12].FormattedValue.ToString();
                nonpwp.Text= dgvVendor.Rows[selRow].Cells[14].FormattedValue.ToString();

                //load data untuk grid
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    //create new dt
                    //DataTable dt = new DataTable();
                    dtVendorContract = new DataTable();
                    try
                    {
                        string query = $"SELECT VendorID,NoKontrak,NoKTP,FarmerID,Area,SubArea,Seri,InventoryID from VendorContract where VendorID='{VendorID}' and Active=1";

                        SqlCommand command = new SqlCommand(query, connection);
                        if (connection.State != ConnectionState.Open)
                        { connection.Open(); }

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtVendorContract);
                        dgvContract.DataSource = dtVendorContract;

                        //Header Text
                        dgvContract.Columns[0].HeaderText = "Vendor ID";
                        dgvContract.Columns[0].Visible = false;
                        dgvContract.Columns[1].HeaderText = "Contract";
                        dgvContract.Columns[2].HeaderText = "ID Card";
                        dgvContract.Columns[3].HeaderText = "Farmer ID";
                        dgvContract.Columns[3].Visible = false;
                        dgvContract.Columns[4].HeaderText = "Area";
                        dgvContract.Columns[4].Visible = false;
                        dgvContract.Columns[5].HeaderText = "Sub Area";
                        dgvContract.Columns[5].Visible = false;
                        dgvContract.Columns[6].HeaderText = "Series";
                        dgvContract.Columns[7].HeaderText = "Tobacco Type";
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }

                    dtVendorPO = new DataTable();
                    try
                    {
                        //string query = $"SELECT VendorID,OrderNbr,NoKontrak,Status from VendorPO where VendorID='{VendorID}'";

                        string query = $@"SELECT
                                            VendorPO.VendorID,
	                                        VendorPO.OrderNbr,
	                                        VendorPO.NoKontrak,
	                                        VendorPO.Status,
	                                        VendorPODetail.LineNbr,
	                                        VendorPODetail.InventoryID,
	                                        VendorPODetail.Subitem,
	                                        VendorPODetail.WarehouseID,
	                                        VendorPODetail.OrderQty,
	                                        VendorPODetail.QtyOnReceipts
                                        FROM
                                            dbo.VendorPO
                                            INNER JOIN
                                            dbo.VendorPODetail
                                            ON
                                                VendorPO.OrderNbr = VendorPODetail.OrderNbr
                                        WHERE
                                            VendorPO.VendorID ='{VendorID}'";

                        SqlCommand command = new SqlCommand(query, connection);
                        if (connection.State != ConnectionState.Open)
                        { connection.Open(); }

                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dtVendorPO);
                        dgvPO.DataSource = dtVendorPO;

                        //Header Text
                        dgvPO.Columns[0].HeaderText = "Vendor ID";
                        dgvPO.Columns[0].Visible = false;
                        dgvPO.Columns[1].HeaderText = "Order Number";
                        dgvPO.Columns[2].HeaderText = "Contract Number";
                        dgvPO.Columns[3].HeaderText = "Status";
                        dgvPO.Columns[4].HeaderText = "PO Line";
                        dgvPO.Columns[5].HeaderText = "Inventory ID";
                        dgvPO.Columns[6].HeaderText = "Sub Item";
                        dgvPO.Columns[7].HeaderText = "Warehouse ID";
                        dgvPO.Columns[8].HeaderText = "Order Qty";
                        dgvPO.Columns[8].DefaultCellStyle.Format = "N2";
                        dgvPO.Columns[9].HeaderText = "On-Receipt Qty";
                        dgvPO.Columns[9].DefaultCellStyle.Format = "N2";

                        if (dtVendorPO.Rows.Count > 0)
                        {
                            decimal sumOrderQty = Convert.ToDecimal(dtVendorPO.Compute("SUM(OrderQty)", string.Empty));
                            tbDetailOrderQty.Text = sumOrderQty.ToString("N2");
                            decimal sumOpenQty = Convert.ToInt32(dtVendorPO.Compute("SUM(QtyOnReceipts)", string.Empty));
                            tbDetailOpenQty.Text = sumOpenQty.ToString("N2");
                        }
                        else
                        {
                            decimal sumOrderQty = 0;
                            tbDetailOrderQty.Text = sumOrderQty.ToString("N2");
                            decimal sumOpenQty = 0;
                            tbDetailOpenQty.Text = sumOpenQty.ToString("N2");
                        }
                    }
                    catch (Exception e2)
                    {
                        MessageBox.Show(e2.ToString());
                    }
                    if (Userlog.UserRoles.Contains("PREPAYMENT"))
                    {
                        dtVendorPrepayment = new DataTable();
                        try
                        {
                            string query = $@"SELECT VendorID, ReferenceNbr, Description, PaymentAmount,PaymentRef, Status, UnappliedBalance
                                        FROM
                                            VendorPrepayment
                                        Where VendorID='{VendorID}'";

                            SqlCommand command = new SqlCommand(query, connection);
                            if (connection.State != ConnectionState.Open)
                            { connection.Open(); }

                            SqlDataAdapter da = new SqlDataAdapter(command);
                            da.Fill(dtVendorPrepayment);
                            dgvPrepayment.DataSource = dtVendorPrepayment;

                            //Header Text
                            dgvPrepayment.Columns[0].HeaderText = "Vendor ID";
                            dgvPrepayment.Columns[0].Visible = false;
                            dgvPrepayment.Columns[1].HeaderText = "Reference Number";
                            dgvPrepayment.Columns[2].HeaderText = "Description";
                            dgvPrepayment.Columns[3].HeaderText = "Payment Amount";
                            //dgvPrepayment.Columns[3].Visible = false;
                            dgvPrepayment.Columns[3].DefaultCellStyle.Format = "N2";
                            dgvPrepayment.Columns[4].HeaderText = "Payment Ref";
                            dgvPrepayment.Columns[4].Visible = false;
                            dgvPrepayment.Columns[5].HeaderText = "Status";
                            dgvPrepayment.Columns[6].HeaderText = "Unapplied Balance";
                            dgvPrepayment.Columns[6].DefaultCellStyle.Format = "N2";

                            if (dtVendorPrepayment.Rows.Count > 0)
                            {
                                decimal sumPrepayment = Convert.ToDecimal(dtVendorPrepayment.Compute("SUM(PaymentAmount)", string.Empty));
                                tbPrepayment.Text = sumPrepayment.ToString("N2");
                                decimal sumUnappliedBalance = Convert.ToInt32(dtVendorPrepayment.Compute("SUM(UnappliedBalance)", string.Empty));
                                tbUnappliedBalance.Text = sumUnappliedBalance.ToString("N2");
                            }
                            else
                            {
                                decimal sumPrepayment = 0;
                                tbPrepayment.Text = sumPrepayment.ToString("N2");
                                decimal sumUnappliedBalance = 0;
                                tbUnappliedBalance.Text = sumUnappliedBalance.ToString("N2");
                            }
                        }
                        catch (Exception e2)
                        {
                            MessageBox.Show(e2.ToString());
                        }


                    }
                    else {
                        
                        tabVendorInfo.TabPages.Remove(tabPage3);
                    }

                }
            }
            catch
            {
                //do nothing
            }
        }

        private void clearInfo()
        {
            //set info
            tbVendorName.Text = "";
            tbStatus.Text = "";
            tbDisplayName.Text = "";
            tbPhone1.Text = "";
            tbPhone2.Text = "";
            tbAddress1.Text = "";
            tbAddress2.Text = "";
            tbCity.Text = "";
            tbCountry.Text = "";
            tbState.Text = "";
            tbPostalCode.Text = "";
            tbVendorClass.Text = "";
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);

            try
            {
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Vendor List - Syncing with Acumatica, please wait!";

                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                var vendorApi = new VendorApi(configuration);
                var vendorsCORP = vendorApi.GetList(expand: "MainContact,MainContact/Address,ContractList", filter: "VendorClass eq 'FARMERCORP'");
                var vendorsIPS = vendorApi.GetList(expand: "MainContact,MainContact/Address,ContractList", filter: "VendorClass eq 'FARMERIPS'");
                var vendorsOM = vendorApi.GetList(expand: "MainContact,MainContact/Address,ContractList", filter: "VendorClass eq 'FARMEROM'");
                var vendorsPF = vendorApi.GetList(expand: "MainContact,MainContact/Address,ContractList", filter: "VendorClass eq 'FARMERPF'");

                //insert vendor Farmercorp
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var vendor in vendorsCORP)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorData_C", connection))
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
                                command.Parameters.AddWithValue("@Ext_No_NPWP", vendor.TaxRegistrationID.Value ?? "");
                                command.Parameters.AddWithValue("@Ext_VendorIsTaxAgency", vendor.VendorIsTaxAgency.Value == true ? 1 : 0);

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

                //insert vendor Farmerips
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var vendor in vendorsIPS)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorData_C", connection))
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
                                command.Parameters.AddWithValue("@Ext_No_NPWP", vendor.TaxRegistrationID.Value ?? "");
                                command.Parameters.AddWithValue("@Ext_VendorIsTaxAgency", vendor.VendorIsTaxAgency.Value == true ? 1 : 0  );

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

                //insert vendor FarmerPF
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var vendor in vendorsPF)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorData_C", connection))
                            {
                                Console.WriteLine(vendor.VendorID.Value);
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
                                command.Parameters.AddWithValue("@Ext_No_NPWP", vendor.TaxRegistrationID.Value ?? "");
                                command.Parameters.AddWithValue("@Ext_VendorIsTaxAgency", vendor.VendorIsTaxAgency.Value == true ? 1 : 0);

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


                //insert vendor Farmerom
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var vendor in vendorsOM)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_VendorData_C", connection))
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
                                command.Parameters.AddWithValue("@Ext_No_NPWP", vendor.TaxRegistrationID.Value ?? "");
                                command.Parameters.AddWithValue("@Ext_VendorIsTaxAgency", vendor.VendorIsTaxAgency.Value == true ? 1 : 0);

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
                var purchaseOrders = purchaseOrderApi.GetList(filter: "Type eq 'Normal'", expand: "Details");

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
                                        Console.WriteLine(detail.WarehouseID.Value);
                                        Console.WriteLine(detail.OrderNbr.Value);
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.AddWithValue("@OrderNbr", detail.OrderNbr.Value);
                                        command.Parameters.AddWithValue("@LineNbr", detail.LineNbr.Value);
                                        command.Parameters.AddWithValue("@InventoryID", detail.InventoryID.Value);
                                        command.Parameters.AddWithValue("@Subitem", detail.Subitem.Value);
                                        command.Parameters.AddWithValue("@WarehouseID", detail.WarehouseID.Value);
                                        command.Parameters.AddWithValue("@OrderQty", (Convert.ToDouble(detail.MaxReceiptPercent.Value)/100 * Convert.ToDouble(detail.OrderQty.Value)));
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
                var checks = checkApi.GetList(filter: "Type eq 'Prepayment'");

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
            }
            finally
            {
                //we use logout in finally block because we need to always logout, even if the request failed for some reason
                authApi.AuthLogout();
                //Console.WriteLine("Logged Out...");
                ////MessageBox.Show("Logged Out...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                MessageBox.Show("Sync complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Vendor List";
                LoadData();
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

        private void btnTruncate_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete all master data from addon ?", "Warning", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //truncate vendor
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        string query = $@"DELETE FROM VendorPrepayment;
                                        DELETE FROM VendorPODetail;
                                        DELETE FROM VendorPO;
                                        DELETE FROM VendorContract;
                                        DELETE FROM VendorData;";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_truncate)
                    {
                        MessageBox.Show($"--Truncate error! {e_truncate.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        LoadData();
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }
    }
}