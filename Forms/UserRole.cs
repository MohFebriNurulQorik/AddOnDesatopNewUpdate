using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class UserRole : Form
    {
        public bool RoleChanged { get; set; }
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string ConnectionString { get; set; }

        public UserRole()
        {
            InitializeComponent();
        }

        private void UserRole_Load(object sender, EventArgs e)
        {
            Text = $"User role for [{FullName}]";
            LoadRole();
            LoadData();
        }

        private void LoadData()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select UserRole.RoleID, RoleData.RoleDesc from UserRole " +
                        $"left join RoleData on UserRole.RoleID = RoleData.RoleID where UserID = {UserID}";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);
                    dgvRole.DataSource = dt;

                    //Header Text
                    dgvRole.Columns[0].HeaderText = "Role ID";
                    dgvRole.Columns[1].HeaderText = "Description";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        } //end loaddata

        private void LoadRole()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                cbRole.DataSource = null;

                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from RoleData";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    cbRole.DataSource = dt;
                    cbRole.DisplayMember = "RoleDesc";
                    cbRole.ValueMember = "RoleID";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnAddRemove_Click(object sender, EventArgs e)
        {
            string newRoleID = cbRole.SelectedValue.ToString();
            string newRoleDesc = cbRole.SelectedText.ToString();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (SqlCommand UpdateCommand = new SqlCommand("Insert_UserRole", connection))
                    {
                        connection.Open();

                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        UpdateCommand.Parameters.AddWithValue("@UserID", UserID);
                        UpdateCommand.Parameters.AddWithValue("@RoleID", newRoleID);

                        int Updateresult = UpdateCommand.ExecuteNonQuery();

                        if (Updateresult > 0)
                        {
                            RoleChanged = true;
                            LoadData();
                            string infoMessage = $"Modifying {FullName}'s role on {newRoleDesc} success";
                            //MessageBox.Show(infoMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("Modifying role failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}