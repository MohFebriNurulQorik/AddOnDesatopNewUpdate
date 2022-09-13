using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class UserProfile : Form
    {
        public Boolean PassChanged { get; set; }
        public UserModel UserLog { get; set; }
        public string ConnectionString { get; set; }

        public UserProfile()
        {
            InitializeComponent();
        }

        private void UserProfile_Load(object sender, EventArgs e)
        {
            tbUsername.Text = UserLog.UserName;
            tbFullName.Text = UserLog.Fullname;
            tbUserType.Text = String.Join(", ", UserLog.UserRoles);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from UserData WHERE UserName='{tbUsername.Text}' and NewPassword='{Utils.Hash512(tbOldPassword.Text, "")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Close();
                        if (tbNewPassword.Text == tbNewPasswordRepeat.Text)
                        {
                            using (SqlCommand UpdateCommand = new SqlCommand("Update_UserData_Password", connection))
                            {
                                UpdateCommand.CommandType = CommandType.StoredProcedure;
                                //receipt
                                UpdateCommand.Parameters.AddWithValue("@UserID", UserLog.UserID);
                                UpdateCommand.Parameters.AddWithValue("@Password", Utils.Hash512(tbNewPassword.Text, ""));

                                int Updateresult = UpdateCommand.ExecuteNonQuery();

                                if (Updateresult > 0)
                                {
                                    PassChanged = true;
                                    MessageBox.Show("Password updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Updating password failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("New password missmatched", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show("Username / Password missmatched", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch
                {
                    MessageBox.Show($"Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }//end btnSave

        private void tbOldPassword_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbNewPassword_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbNewPasswordRepeat_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}