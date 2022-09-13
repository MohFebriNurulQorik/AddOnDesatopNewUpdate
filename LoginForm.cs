using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
namespace ScaleAddon
{
    public partial class LoginForm : Form
    {
        public Boolean LoginCancel { get; set; }
        public Boolean LoggedIn { get; set; }
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public WarehouseModel Warehouse { get; set; }
        public UserModel UserLog { get; set; }

        private DataTable versiapp;
        public LoginForm()
        {
            InitializeComponent();

            LoginCancel = true;
        }

        private void LoginLogic()
        {
            MainForm.Userlog.ClearUser();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string queryversi = $"select TOP 1 Version_app from DataVersion WHERE Date <= '{DateTime.Now.Date.ToString("yyyy-MM-dd")}'  and  Status=1 ORDER BY id Desc";
                    Console.WriteLine(queryversi);
                    SqlCommand commandversi = new SqlCommand(queryversi, connection);
                    connection.Open();
                    SqlDataReader readerversi = commandversi.ExecuteReader();
                    readerversi.Read();

                    Console.WriteLine(readerversi.GetValue(0).ToString());
                    if (readerversi.GetValue(0).ToString() == $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}-R2ULT300822") {

                        readerversi.Close();
                        using (SqlConnection connection2 = new SqlConnection(ConnectionString))
                        {

                            try
                            {
                                string query = $"select * from UserData WHERE UserName='{tbUsername.Text}' and NewPassword='{Utils.Hash512(tbPassword.Text, "")}' and Status='Active'";
                                SqlCommand command = new SqlCommand(query, connection2);
                                connection2.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                if (reader.HasRows)
                                {
                                    LoggedIn = true;
                                    LoginCancel = false;
                                    reader.Read();

                                    MainForm.Userlog.UserID = reader.GetValue(0).ToString();
                                    MainForm.Userlog.UserName = reader.GetValue(1).ToString();
                                    MainForm.Userlog.Fullname = reader.GetValue(2).ToString();

                                    reader.Close();

                                    try
                                    {
                                        MainForm.Userlog.UserRoles.Clear();
                                        query = $"select * from UserRole WHERE UserID='{MainForm.Userlog.UserID}'";
                                        command = new SqlCommand(query, connection2);
                                        //connection.Open();
                                        reader = command.ExecuteReader();

                                        while (reader.Read())
                                        {
                                            MainForm.Userlog.UserRoles.Add(reader.GetValue(1).ToString());
                                        }
                                    }
                                    catch
                                    {
                                        reader.Close();

                                        MessageBox.Show($"Get roles failed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }

                                    Close();
                                }
                                else
                                {
                                    LoggedIn = false;
                                    reader.Close();

                                    MessageBox.Show("Login failed, username / Password missmatched", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                    tbUsername.Text = "";
                                    tbPassword.Text = "";

                                    tbUsername.Focus();
                                }
                            }
                            catch (Exception e)
                            {
                                LoggedIn = false;
                                MessageBox.Show(e.ToString());
                            }
                        }

            }
                    else
                    {
                        LoggedIn = false;
                       
                       

                        MessageBox.Show($"Versi aplikasi {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}-R2ULT300822 telah expired, Silakan update aplikasi ke versi {readerversi.GetValue(0).ToString()} ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        try
                        {
                            readerversi.Close();
                        }
                        catch (Exception e)
                        {

                        }
                    }

                }
                catch (Exception e)
                {
                    LoggedIn = false;
                    MessageBox.Show(e.ToString());
                }
            }//end using connection
        }//end loginlogic

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginLogic();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoginCancel = true;
            DialogResult result = MessageBox.Show("Cancel Login?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                LoggedIn = false;
                this.Dispose();
                Application.Exit();
            }
        }

        private void tbPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginLogic();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Text = $"LOGIN [{ClientID}]";
            tbWarehouse.Text = Warehouse.Descr;
          
            lblInfo.Text = $"Scale Addon ver.{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}-R2ULT300822";
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginLogic();

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}