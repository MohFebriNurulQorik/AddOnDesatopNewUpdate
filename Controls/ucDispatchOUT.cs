using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucDispatchOUT : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public UserModel Userlog { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        private DataTable dtList;
        private DateTime currentDate;
        private DateTime currentDate2;

        public ucDispatchOUT()
        {
            InitializeComponent();
        }

        private void ucDispatchOUT_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Dispatch OUT List";

            currentDate = DateTime.Now;

            dtpListDate.Value = currentDate;
            dtpListDate2.Value = currentDate;

            LoadData();
        }

        private void LoadData()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtList = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    //string query = $@"SELECT *
                    //                FROM
                    //                    DispatchOUTLine
                    //                WHERE
                    //                    DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";

                    string query = $@"SELECT *
                                    FROM
                                        DispatchOUTLine
                                    WHERE
                                        DocumentDate BETWEEN '{currentDate2.Date.ToString("yyyy-MM-dd")}' AND '{currentDate.Date.ToString("yyyy-MM-dd")}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtList);
                    dgvList.DataSource = dtList;

                    //Header Text
                    dgvList.Columns[0].HeaderText = "Document Number";
                    dgvList.Columns[1].HeaderText = "Date";
                    dgvList.Columns[2].HeaderText = "Dispatch From";
                    dgvList.Columns[3].HeaderText = "Dispatch To";
                    dgvList.Columns[4].HeaderText = "Status";
                    dgvList.Columns[5].HeaderText = "Total Cost";
                    dgvList.Columns[5].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[6].HeaderText = "Total Qty";
                    dgvList.Columns[6].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[7].HeaderText = "Acumatica Issue";
                    dgvList.Columns[8].HeaderText = "Note";
                    dgvList.Columns[8].Visible = false;
                    dgvList.Columns[9].HeaderText = "Created By";
                    //dgvList.Columns[9].Visible = false;
                    dgvList.Columns[10].HeaderText = "Created Date";
                   // dgvList.Columns[10].Visible = false;
                    dgvList.Columns[11].HeaderText = "Modified date";
                    //dgvList.Columns[11].Visible = false;
                    dgvList.Columns[12].HeaderText = "Logistic";
                    dgvList.Columns[12].Visible = false;
                    dgvList.Columns[13].HeaderText = "Lisence Plate";
                    dgvList.Columns[13].Visible = false;

                    dgvList.ClearSelection();
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
            DataView dv_filter = new DataView(dtList, $"DocumentID LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            dgvList.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtList, $"DocumentID LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
                dgvList.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dtpListDate_ValueChanged(object sender, EventArgs e)
        {
            //currentDate = dtpListDate.Value;
            //LoadData();
            currentDate = dtpListDate.Value;
            if (dtpListDate2.Value > dtpListDate.Value)
            {
                dtpListDate2.Value = dtpListDate.Value;
            }
            currentDate2 = dtpListDate2.Value;
            LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                Forms.DispatchOUTProcess dispatchOUTProcess = new Forms.DispatchOUTProcess
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    AcumaticaCred = AcumaticaCred,
                    DocNumber = dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[0].FormattedValue.ToString(),
                    currentDate = DateTime.Parse(dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[1].FormattedValue.ToString()),
                    Userlog = Userlog,
                    FiscalInfo = FiscalInfo,
                    ScaleCom = ScaleCom,
                    ScaleCalibration = ScaleCalibration
                };
                dispatchOUTProcess.ShowDialog();

                currentDate = dtpListDate.Value;
            }

            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Forms.DispatchOUTProcess dispatchOUTProcess = new Forms.DispatchOUTProcess
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                DocNumber = "<NEW>",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration
            };
            dispatchOUTProcess.ShowDialog();

            currentDate = dtpListDate.Value;
            LoadData();
        }

        private void dtpListDate2_ValueChanged(object sender, EventArgs e)
        {
            currentDate2 = dtpListDate2.Value;
            if (dtpListDate2.Value > dtpListDate.Value)
            {
                dtpListDate.Value = dtpListDate2.Value;
            }
            currentDate = dtpListDate.Value;
            LoadData();

        }
    }
}