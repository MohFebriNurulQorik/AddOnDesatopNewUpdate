using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SetupWarehouse : Form
    {
        public Boolean WarehouseChanged { get; set; }
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }

        public SetupWarehouse()
        {
            InitializeComponent();
        }

        private void SetupWarehouse_Load(object sender, EventArgs e)
        {
            LoadWarehouse();
        }

        private void LoadWarehouse()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                cbWarehouseID.DataSource = null;

                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from WarehouseSite";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    cbWarehouseID.DataSource = dt;
                    cbWarehouseID.DisplayMember = "Descr";
                    cbWarehouseID.ValueMember = "WarehouseID";

                    cbWarehouseID.SelectedValue = Warehouse;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (SqlCommand UpdateCommand = new SqlCommand("Insert_AppSettings", connection))
                    {
                        connection.Open();
                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        //receipt
                        UpdateCommand.Parameters.AddWithValue("@SettingID", "WarehouseID");
                        UpdateCommand.Parameters.AddWithValue("@Val", cbWarehouseID.SelectedValue.ToString());

                        int Updateresult = UpdateCommand.ExecuteNonQuery();

                        if (Updateresult > 0)
                        {
                            MessageBox.Show($"Set warehouse success, warehouse set to {cbWarehouseID.Text}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Warehouse.WarehouseID = cbWarehouseID.SelectedValue.ToString();
                            WarehouseChanged = true;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Set warehouse failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } // using
            Warehouse.ClearWarehouse();
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

                        reader.Close();

                        Close();
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
    }
}