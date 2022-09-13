using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class SetupFiscal : Form
    {
        public Boolean FiscalChanged { get; set; }
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        private Boolean error = false;

        public SetupFiscal()
        {
            InitializeComponent();
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

        private void SetupFiscal_Load(object sender, EventArgs e)
        {
            tbClientID.Text = ClientID;
            tbMonth.Text = FiscalInfo.StartingFiscalMonth.ToString();
            tbYear.Text = FiscalInfo.CurrentFiscalYear.ToString();
        }

        private void cbChangeMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbChangeMonth.SelectedIndex >= 0)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            error = false;
            var month = cbChangeMonth.SelectedItem.ToString().Split('-')[0].Trim();
            insertSetting("FiscalMonth", month);

            if (!error)
            {
                FiscalInfo.StartingFiscalMonth = Convert.ToInt32(month);
                DateTime currentDate = DateTime.Now;
                FiscalInfo.CurrentFiscalYear = currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year;
                tbMonth.Text = FiscalInfo.StartingFiscalMonth.ToString();
                tbYear.Text = FiscalInfo.CurrentFiscalYear.ToString();

                MessageBox.Show("Setting saved!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            FiscalChanged = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dtSettings = new DataTable();
                try
                {
                    string query = $"select * from AppSettings";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtSettings);

                    foreach (DataRow setting in dtSettings.Rows)
                    {
                        switch ((string)setting[0])
                        {
                            case "FiscalMonth":
                                FiscalInfo.StartingFiscalMonth = Convert.ToInt32(setting[1]);
                                DateTime currentDate = DateTime.Now;
                                FiscalInfo.CurrentFiscalYear = currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year;
                                break;
                        }
                    }

                    tbMonth.Text = FiscalInfo.StartingFiscalMonth.ToString();
                    tbYear.Text = FiscalInfo.CurrentFiscalYear.ToString();

                    MessageBox.Show("Cache updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch
                {
                    //do nothing
                }
            }//end using connection
        }
    }
}