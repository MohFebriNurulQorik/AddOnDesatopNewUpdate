using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SetupAcumatica : Form
    {
        public Boolean AcumaticaChanged { get; set; }
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        private Boolean error = false;

        public SetupAcumatica()
        {
            InitializeComponent();
        }

        private void setupAcumatica_Load(object sender, EventArgs e)
        {
            tbClientID.Text = ClientID;
            tbSiteURL.Text = AcumaticaCred.AcumaticaSiteURL;
            tbUser.Text = AcumaticaCred.AcumaticaUser;
            tbPassword.Text = AcumaticaCred.AcumaticaPassword;
            tbTenant.Text = AcumaticaCred.AcumaticaTenant;
            tbEndpoint.Text = AcumaticaCred.AcumaticaEndpointName;
            tbVersion.Text = AcumaticaCred.AcumaticaEndpointVersion;
            tbInvLocation.Text = AcumaticaCred.AcumaticaInvLocation;

            tbAPITest.Text = "Configuration not tested";
            tbAPITest.BackColor = Color.LightYellow;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var authApi = new AuthApi(tbSiteURL.Text);

            try
            {
                var configuration = LogIn(authApi, tbSiteURL.Text, tbUser.Text, tbPassword.Text, tbTenant.Text, AcumaticaCred.AcumaticaBranch, AcumaticaCred.AcumaticaLocale);

                tbAPITest.Text = "Acumatica connection successfull!";
                tbAPITest.BackColor = Color.YellowGreen;
            }
            catch
            {
                //Console.WriteLine(ex.Message);
                tbAPITest.Text = "Acumatica connection failed!";
                tbAPITest.BackColor = Color.Red;
            }
            finally
            {
                //we use logout in finally block because we need to always logout, even if the request failed for some reason
                try
                {
                    authApi.AuthLogout();
                }
                catch
                {
                    //do nothing
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            error = false;
            insertSetting("AcumaticaSiteURL", tbSiteURL.Text);
            AcumaticaCred.AcumaticaSiteURL = tbSiteURL.Text;
            insertSetting("AcumaticaUser", tbUser.Text);
            AcumaticaCred.AcumaticaUser = tbUser.Text;
            insertSetting("AcumaticaPassword", tbPassword.Text);
            AcumaticaCred.AcumaticaPassword = tbPassword.Text;
            insertSetting("AcumaticaTenant", tbTenant.Text);
            AcumaticaCred.AcumaticaTenant = tbTenant.Text;
            insertSetting("AcumaticaEndpointName", tbEndpoint.Text);
            AcumaticaCred.AcumaticaEndpointName = tbEndpoint.Text;
            insertSetting("AcumaticaEndpointVersion", tbVersion.Text);
            AcumaticaCred.AcumaticaEndpointVersion = tbVersion.Text;
            insertSetting("AcumaticaEndpointVersion", tbVersion.Text);
            AcumaticaCred.AcumaticaEndpointVersion = tbVersion.Text;
            insertSetting("AcumaticaInvLocation", tbInvLocation.Text);
            AcumaticaCred.AcumaticaInvLocation = tbInvLocation.Text;

            if (!error)
            {
                MessageBox.Show("Setting saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            AcumaticaChanged = true;
        }

        private void insertSetting(string SettingID, string Val)
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
                        UpdateCommand.Parameters.AddWithValue("@SettingID", SettingID);
                        UpdateCommand.Parameters.AddWithValue("@Val", Val);
                        UpdateCommand.Parameters.AddWithValue("@ClientID", ClientID);

                        int Updateresult = UpdateCommand.ExecuteNonQuery();
                    }
                }
                catch
                {
                    error = true;
                    MessageBox.Show("Connection error", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            } // using
        }

        private Configuration LogIn(AuthApi authApi, string siteURL, string username, string password, string tenant, string branch = null, string locale = null)
        {
            var cookieContainer = new CookieContainer();
            authApi.Configuration.ApiClient.RestClient.CookieContainer = cookieContainer;

            authApi.AuthLogin(new Credentials(username, password, tenant, branch, locale));
            Console.WriteLine("Logged In...");
            var configuration = new Configuration($"{siteURL}/entity/{tbEndpoint.Text}/{tbVersion.Text}/");

            //share cookie container between API clients because we use different client for authentication and interaction with endpoint
            configuration.ApiClient.RestClient.CookieContainer = authApi.Configuration.ApiClient.RestClient.CookieContainer;
            return configuration;
        }

        private void tbSiteURL_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbUser_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbTenant_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbEndpoint_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbVersion_KeyPress(object sender, KeyPressEventArgs e)
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

        private void tbInvLocation_KeyPress(object sender, KeyPressEventArgs e)
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