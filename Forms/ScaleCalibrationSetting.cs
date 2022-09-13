using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class ScaleCalibrationSetting : Form
    {
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public UserModel Userlog { get; set; }
        public string ClientID { get; set; }

        private DataTable dtDetail;

        private DateTime currentDate;
        private bool curIsActive;

        public ScaleCalibrationSetting()
        {
            InitializeComponent();
        }

        private void ScaleCalibration_Load(object sender, EventArgs e)
        {
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Scale Calibration";
            currentDate = DateTime.Now;
            loadCalibration();
            loadHistory();
        }

        private void loadHistory()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtDetail = new DataTable();
                try
                {
                    string query = $@"SELECT
                                            *
                                        FROM
	                                        ScaleCalibration
                                        WHERE
                                            DocumentDate between '{currentDate.AddDays(-14)}' and '{currentDate}'
                                            AND ClientID = '{ClientID}'
                                            AND WarehouseID = '{Warehouse.WarehouseID}'
                                        ORDER BY DocumentDate Desc";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                    dgvDetail.DataSource = dtDetail;

                    //Header Text
                    dgvDetail.Columns[0].HeaderText = "Document ID";
                    dgvDetail.Columns[1].HeaderText = "Date-Time";
                    dgvDetail.Columns[2].HeaderText = "Warehouse ID";
                    dgvDetail.Columns[2].Visible = false;
                    dgvDetail.Columns[3].HeaderText = "Client ID";
                    dgvDetail.Columns[3].Visible = false;
                    dgvDetail.Columns[4].HeaderText = "Creator ID";
                    dgvDetail.Columns[5].HeaderText = "Created Date";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Modified Date";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.ClearSelection();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadCalibration()
        {
            ScaleCalibration.ClearCalibration();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    string query = $"select * from ScaleCalibration WHERE WarehouseID='{Warehouse.WarehouseID}' AND ClientID = '{ClientID}' ORDER BY DocumentDate Desc";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        ScaleCalibration.DocumentID = reader.GetValue(0).ToString();
                        ScaleCalibration.DocumentDate = reader.GetValue(1).ToString();
                        ScaleCalibration.WarehouseID = reader.GetValue(3).ToString();
                        ScaleCalibration.ClientID = reader.GetValue(4).ToString();
                        ScaleCalibration.CreatorID = reader.GetValue(5).ToString();

                        tbDocNumber.Text = ScaleCalibration.DocumentID;
                        tbDate.Text = ScaleCalibration.DocumentDate;
                        tbWarehouse.Text = ScaleCalibration.WarehouseID;
                        tbClientID.Text = ScaleCalibration.ClientID;
                        tbCreatorID.Text = ScaleCalibration.CreatorID;
                        tbStatus.Text = ScaleCalibration.isActive() == true ? "Active" : "Expired";

                        reader.Close();
                    }
                    else
                    {
                        reader.Close();

                        MessageBox.Show("Cannot find calibration info", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }//end using connection
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ScaleCalibration.isActive())
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_ScaleCalibration", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", $"{Warehouse.WarehouseID}-{ClientID}-{currentDate.ToString("yyMMdd-HHmm")}");
                            command.Parameters.AddWithValue("@DocumentDate", currentDate.ToString("yyyy-MM-dd HH:mm:ss"));
                            command.Parameters.AddWithValue("@WarehouseID", Warehouse.WarehouseID);
                            command.Parameters.AddWithValue("@ClientID", ClientID);
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
                        loadHistory();
                        loadCalibration();
                    }
                }
            }
            else
            {
                MessageBox.Show($"Current calibration is still active, calibration not needed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //end of file
    }
}