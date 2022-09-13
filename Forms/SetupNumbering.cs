using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SetupNumbering : Form
    {
        public string ConnectionString { get; set; }
        private DataTable dt;

        public SetupNumbering()
        {
            InitializeComponent();
        }

        private void SetupNumbering_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                dt = new DataTable();
                try
                {
                    string query = "select * from NumberingSetting";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                    dgvItem.DataSource = dt;

                    //Header Text
                    dgvItem.Columns[0].HeaderText = "Numbering ID";
                    dgvItem.Columns[0].ReadOnly = true;
                    dgvItem.Columns[1].HeaderText = "last Increment value";
                    dgvItem.Columns[2].HeaderText = "Date";
                    dgvItem.Columns[2].ReadOnly = true;
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
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //insert update
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DateTime currentDate = DateTime.Now;
                foreach (DataGridViewRow row in dgvItem.Rows)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@NumberingID", row.Cells["NumberingID"].Value);
                            command.Parameters.AddWithValue("@LastIncrementValue", row.Cells["LastIncrementValue"].Value);
                            command.Parameters.AddWithValue("@NumberingDate", currentDate);

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                //MessageBox.Show("Update complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}