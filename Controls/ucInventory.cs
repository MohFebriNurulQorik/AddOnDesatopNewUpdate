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
    public partial class ucInventory : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }

        public AcumaticaCredModel AcumaticaCred { get; set; }
        private DataTable dtItem;

        public ucInventory()
        {
            InitializeComponent();
        }

        private void ucInventory_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory List";

            LoadData();
        }

        private void LoadData()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtItem = new DataTable();
                try
                {
                    string query = $"SELECT * from InventoryItem";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtItem);
                    dgvItem.DataSource = dtItem;

                    //Header Text
                    dgvItem.Columns[0].HeaderText = "Inventory ID";
                    dgvItem.Columns[1].HeaderText = "Description";
                    dgvItem.Columns[2].HeaderText = "Status";
                    dgvItem.Columns[3].HeaderText = "Last Modified";
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
            DataView dv_filter = new DataView(dtItem, $"Descr LIKE '%{tbFilter.Text}%'", "InventoryID Asc", DataViewRowState.CurrentRows);
            dgvItem.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtItem, $"Descr LIKE '%{tbFilter.Text}%'", "InventoryID Asc", DataViewRowState.CurrentRows);
                dgvItem.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnAcumatica_Click(object sender, EventArgs e)
        {
            var authApi = new AuthApi(AcumaticaCred.AcumaticaSiteURL);

            try
            {
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory List - Syncing with Acumatica, please wait!";

                var configuration = LogIn(authApi, AcumaticaCred.AcumaticaSiteURL, AcumaticaCred.AcumaticaUser, AcumaticaCred.AcumaticaPassword, AcumaticaCred.AcumaticaTenant, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);
                var stockItemApi = new StockItemApi(configuration);
                var stockItems = stockItemApi.GetList(select: "InventoryID,Description,ItemStatus,LastModified", filter: "ItemClass eq 'TOBACCO'");

                //insert vendor attribute
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    foreach (var stockItem in stockItems)
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_Inventory", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@InventoryID", stockItem.InventoryID.Value);
                                command.Parameters.AddWithValue("@Descr", stockItem.Description.Value);
                                command.Parameters.AddWithValue("@ItemStatus", stockItem.ItemStatus.Value ?? "");
                                command.Parameters.AddWithValue("@LastModifiedDateTime", stockItem.LastModified.Value);

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
                LoadData();
                ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory List";
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
                        string query = $@"TRUNCATE TABLE InventoryItem;";

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