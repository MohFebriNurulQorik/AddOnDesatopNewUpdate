using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucUsers : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        private int selRow = -10;
        private DataTable dtUser;

        public ucUsers()
        {
            InitializeComponent();
        }

        private void ucUsers_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - User List";

            LoadData();
        }

        private void dgvUser_CellClick(object sender, DataGridViewCellEventArgs e)
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
                dtUser = new DataTable();
                try
                {
                    string query = $"SELECT UserID, UserName, FullName, " +
                                   $"(STUFF((SELECT ', ' + RoleID FROM UserRole WHERE UserID = UserData.userID FOR XML PATH('') ), 1, 1, '')) AS UserRoles, Status from UserData";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtUser);
                    dgvUser.DataSource = dtUser;

                    //Header Text
                    dgvUser.Columns[0].HeaderText = "User ID";
                    dgvUser.Columns[1].HeaderText = "User Name";
                    dgvUser.Columns[2].HeaderText = "Full Name";
                    dgvUser.Columns[3].HeaderText = "User Roles";
                    dgvUser.Columns[4].HeaderText = "Status";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnResetPass_Click(object sender, EventArgs e)
        {
            if (selRow < 0)
            {
                MessageBox.Show($"Please choose a user first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string UserID = dgvUser.Rows[selRow].Cells[0].FormattedValue.ToString();
                string Fullname = dgvUser.Rows[selRow].Cells[2].FormattedValue.ToString();
                string newPassword = "password";
                string Password = Utils.Hash512(newPassword, "");
                if (UserID == MainForm.Userlog.UserID)
                {
                    MessageBox.Show($"You can't reset your own password, please change it through profile page", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            using (SqlCommand UpdateCommand = new SqlCommand("Update_UserData_Password", connection))
                            {
                                connection.Open();
                                UpdateCommand.CommandType = CommandType.StoredProcedure;
                                //receipt
                                UpdateCommand.Parameters.AddWithValue("@UserID", UserID);
                                UpdateCommand.Parameters.AddWithValue("@Password", Password);

                                int Updateresult = UpdateCommand.ExecuteNonQuery();

                                if (Updateresult > 0)
                                {
                                    MessageBox.Show($"Password reset success, {Fullname}'s current password updated into '{newPassword}'", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    MessageBox.Show("Resetting password failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    } // using
                } // end if else same ID
            }
        }//end action

        private void btnStatus_Click(object sender, EventArgs e)
        {
            if (selRow < 0)
            {
                MessageBox.Show($"Please choose a user first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string UserID = dgvUser.Rows[selRow].Cells[0].FormattedValue.ToString();
                string currentStatus = dgvUser.Rows[selRow].Cells[4].FormattedValue.ToString();
                string Fullname = dgvUser.Rows[selRow].Cells[2].FormattedValue.ToString();
                string newStatus;
                if (currentStatus == "Active")
                {
                    newStatus = "Inactive";
                }
                else
                {
                    newStatus = "Active";
                }

                if (UserID == MainForm.Userlog.UserID)
                {
                    MessageBox.Show($"You can't toggle your own account's status", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            using (SqlCommand UpdateCommand = new SqlCommand("Update_UserData_Status", connection))
                            {
                                connection.Open();
                                UpdateCommand.CommandType = CommandType.StoredProcedure;
                                //receipt
                                UpdateCommand.Parameters.AddWithValue("@UserID", UserID);
                                UpdateCommand.Parameters.AddWithValue("@Status", newStatus);

                                int Updateresult = UpdateCommand.ExecuteNonQuery();

                                if (Updateresult > 0)
                                {
                                    LoadData();
                                    MessageBox.Show($"Toggling status success, {Fullname}'s current status updated from '{currentStatus}' into '{newStatus}'", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                                else
                                {
                                    MessageBox.Show("Toggling status failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    } // using
                } // end if else same ID
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Forms.UserAddNew AddNewUser = new Forms.UserAddNew
            {
                ConnectionString = ConnectionString
            };
            AddNewUser.ShowDialog();

            if (AddNewUser.UserAdded)
            {
                LoadData();
            }
        }

        private void btnRoles_Click(object sender, EventArgs e)
        {
            if (selRow < 0)
            {
                MessageBox.Show($"Please choose a user first", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Forms.UserRole EditRole = new Forms.UserRole
                {
                    ConnectionString = ConnectionString,
                    UserID = dgvUser.Rows[selRow].Cells[0].FormattedValue.ToString(),
                    FullName = dgvUser.Rows[selRow].Cells[2].FormattedValue.ToString()
                };
                EditRole.ShowDialog();

                if (EditRole.RoleChanged)
                {
                    LoadData();
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            DataView dv_filter = new DataView(dtUser, $"FullName LIKE '%{tbFilter.Text}%'", "UserID Asc", DataViewRowState.CurrentRows);
            dgvUser.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtUser, $"FullName LIKE '%{tbFilter.Text}%'", "UserID Asc", DataViewRowState.CurrentRows);
                dgvUser.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}