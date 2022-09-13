using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class GenericOUTProcesDonor : Form
    {
        public string ConnectionString { get; set; }
        public string DocNumber { get; set; }
        public string RefINNbr { get; set; }
        public string RefINDonor { get; set; }
        public string BuyerName { get; set; }
        public Decimal WeightDonor { get; set; }
        public string tempProcess { get; set; }
        public UserModel Userlog { get; set; }
        private DataTable dtRefIN;
        private Decimal WeightDonorUse;
        private Decimal INUnapplied;

        public GenericOUTProcesDonor()
        {
            InitializeComponent();
        }

        private void GenericOUTProcesDonor_Load(object sender, EventArgs e)
        {
            loadComboIN();
        }

        private void loadComboIN()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbRefIN.SelectedIndex = -1;

                dtRefIN = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    *
                                    FROM
	                                    ProcessingLineIN
                                    WHERE
	                                    ProcessType = '{tempProcess}'
                                    AND BuyerName like '%{BuyerName}%'
                                    AND UnappliedBalance > 0";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtRefIN);
                    string[] arrray = dtRefIN.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbRefIN);
                    cbRefIN.Items.Clear();
                    cbRefIN.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void cbRefIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRefIN.SelectedIndex >= 0)
            {
                RefINDonor = cbRefIN.SelectedItem.ToString();
                loadRefINData();
            }
            else
            {
                tbUnappliedBalance.Text = "0";
            }
        }

        private void loadRefINData()
        {
            DataView dv_filter = new DataView(dtRefIN, $"DocumentID LIKE '%{RefINDonor}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            foreach (DataRowView rowView in dv_filter)
            {
                DataRow row = rowView.Row;
                INUnapplied = Convert.ToDecimal(row.ItemArray[7]);
                tbMaterialIN.Text = Convert.ToDecimal(row.ItemArray[5]).ToString("N2");
                tbUnappliedBalance.Text = INUnapplied.ToString("N2");

                if (INUnapplied > Math.Abs(WeightDonor))
                {
                    WeightDonorUse = WeightDonor;
                }
                else
                {
                    WeightDonorUse = -INUnapplied;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Insert_ProcessingLineOUTDonor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", DocNumber);
                        command.Parameters.AddWithValue("@RefINNbr", RefINNbr);
                        command.Parameters.AddWithValue("@RefINDonor", RefINDonor);
                        command.Parameters.AddWithValue("@TotalWeight", WeightDonorUse);
                        command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Update_Issue_Balance", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", RefINDonor);
                            command.Parameters.AddWithValue("@UnappliedBalance", INUnapplied - Math.Abs(WeightDonorUse));

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Update issue balance error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    this.Close();
                }
            }
        }
    }
}