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
    public partial class ucWarehouse : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }

        private int selRow = -10;
        private DataTable dtWarehouse;

        public ucWarehouse()
        {
            InitializeComponent();
        }

        private void ucWarehouse_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Warehouse List";
            tbWarehouse.Text = Warehouse.Descr;

            LoadData();
        }

        private void dgvWarehouse_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            { selRow = e.RowIndex; }
            catch
            {
                //do nothing
            }
        }

        private void LoadData()
        {
            selRow = -10;
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtWarehouse = new DataTable();
                try
                {
                    string query = $"SELECT * from WarehouseSite";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtWarehouse);
                    dgvWarehouse.DataSource = dtWarehouse;

                    //Header Text
                    dgvWarehouse.Columns[0].HeaderText = "Warehouse ID";
                    dgvWarehouse.Columns[1].HeaderText = "Description";
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
            DataView dv_filter = new DataView(dtWarehouse, $"Descr LIKE '%{tbFilter.Text}%'", "WarehouseID Asc", DataViewRowState.CurrentRows);
            dgvWarehouse.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtWarehouse, $"Descr LIKE '%{tbFilter.Text}%'", "WarehouseID Asc", DataViewRowState.CurrentRows);
                dgvWarehouse.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if (selRow < 0)
            {
                MessageBox.Show($"Please choose a warehouse first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string newSettingID = "WarehouseID";
                string newVal = dgvWarehouse.Rows[selRow].Cells[0].FormattedValue.ToString();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        string query = $"Insert_AppSettings";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SettingID", newSettingID);
                        command.Parameters.AddWithValue("@Val", newVal);
                        command.Parameters.AddWithValue("@ClientID", ClientID);

                        int Updateresult = command.ExecuteNonQuery();

                        if (Updateresult > 0)
                        {
                            //string infoMessage = $"Current Warehouse set {newVal}";
                            Warehouse.WarehouseID = newVal;
                            //MainForm.Warehouse = Warehouse;
                            //tbWarehouse.Text = Warehouse.Descr;
                            //MessageBox.Show(infoMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Modifying current warehouse failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch
                    {
                        MessageBox.Show($"Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }//end using connection

                updateWarehouseCache();
            }
        }

        private void updateWarehouseCache()
        {
            //update warehouse object in memory
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from WarehouseSite WHERE WarehouseID='{Warehouse.WarehouseID}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        Warehouse.WarehouseID = reader.GetValue(0).ToString();
                        Warehouse.Descr = reader.GetValue(1).ToString();
                        Warehouse.Company = reader.GetValue(3).ToString();
                        Warehouse.AddressLine1 = reader.GetValue(4).ToString();
                        Warehouse.AddressLine2 = reader.GetValue(5).ToString();
                        Warehouse.Phone1 = reader.GetValue(6).ToString();
                        Warehouse.Phone2 = reader.GetValue(7).ToString();

                        tbWarehouse.Text = Warehouse.Descr;

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        MessageBox.Show("Cannot find warehouse info", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }//end using connection
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            DateTime localDate = DateTime.Now;
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);

            try
            {
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Warehouse List - Syncing with Acumatica, please wait!";

                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                var warehouseApi = new WarehouseApi(configuration);
                var warehouses = warehouseApi.GetList(expand: "MainContact,MainContact/Address,Locations");

                //insert vendor attribute
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var warehouse in warehouses)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_Warehouse", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@WarehouseID", warehouse.WarehouseID.Value);
                                command.Parameters.AddWithValue("@Descr", warehouse.Description.Value);
                                command.Parameters.AddWithValue("@SyncDate", localDate);
                                command.Parameters.AddWithValue("@Company", warehouse.MainContact.CompanyName.Value ?? "");
                                command.Parameters.AddWithValue("@AddressLine1", warehouse.MainContact.Address.AddressLine1.Value ?? "");
                                command.Parameters.AddWithValue("@AddressLine2", warehouse.MainContact.Address.AddressLine2.Value ?? "");
                                command.Parameters.AddWithValue("@Phone1", warehouse.MainContact.Phone1.Value ?? "");
                                command.Parameters.AddWithValue("@Phone2", warehouse.MainContact.Phone2.Value ?? "");
                                command.Parameters.AddWithValue("@Branch", warehouse.Branch.Value ?? "");

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        foreach (var location in warehouse.Locations)
                        {
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlCommand command = new SqlCommand("Insert_WarehouseLocation", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@WarehouseID", warehouse.WarehouseID.Value);
                                    command.Parameters.AddWithValue("@LocationID", location.LocationID.Value);
                                    command.Parameters.AddWithValue("@Descr", location.Description.Value);

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
                updateWarehouseCache();

                MessageBox.Show("Sync complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadData();
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Warehouse List";
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
                        string query = $@"TRUNCATE TABLE WarehouseSite;
                                            TRUNCATE TABLE WarehouseLocation;";

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