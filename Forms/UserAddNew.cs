using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class UserAddNew : Form
    {
        public bool UserAdded = false;
        public string ConnectionString { get; set; }

        public UserAddNew()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string newUserName = tbUsername.Text;
            string newFullName = tbFullName.Text;
            string newPassword = "password";
            string Password = Utils.Hash512(newPassword, "");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    using (SqlCommand UpdateCommand = new SqlCommand("Insert_UserData", connection))
                    {
                        connection.Open();

                        UpdateCommand.CommandType = CommandType.StoredProcedure;
                        UpdateCommand.Parameters.AddWithValue("@UserName", newUserName);
                        UpdateCommand.Parameters.AddWithValue("@Fullname", newFullName);
                        UpdateCommand.Parameters.AddWithValue("@Password", Password);

                        int Updateresult = UpdateCommand.ExecuteNonQuery();

                        if (Updateresult > 0)
                        {
                            UserAdded = true;
                            string infoMessage = $"User {newFullName} added using credentials as follows,{Environment.NewLine}" +
                                $"User name '{newUserName}' and password '{newPassword}'";
                            MessageBox.Show(infoMessage, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Add new user failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }//end save
    }
}