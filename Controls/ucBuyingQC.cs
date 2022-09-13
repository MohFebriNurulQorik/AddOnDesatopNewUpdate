using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucBuyingQC : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public UserModel Userlog { get; set; }
        private DataTable dtList;
        private DateTime currentDate;
        //add new
        private DateTime currentDate2;

        public ucBuyingQC()
        {
            InitializeComponent();
        }

        private void ucBuyingQC_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying QC List";

            currentDate = DateTime.Now;

            dtpListDate.Value = currentDate;
            //add new
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
                    string query = $@"SELECT *
                                    FROM
                                        BuyingQC
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
                    dgvList.Columns[2].HeaderText = "Warehouse";
                    dgvList.Columns[3].HeaderText = "Vendor ID";
                    dgvList.Columns[4].HeaderText = "Vendor Details";
                    dgvList.Columns[5].HeaderText = "Registration Number";
                    dgvList.Columns[6].HeaderText = "Order Number";
                    dgvList.Columns[7].HeaderText = "Inventory ID";
                    dgvList.Columns[8].HeaderText = "Vendor Class";
                    dgvList.Columns[9].HeaderText = "Document Status";
                    dgvList.Columns[10].HeaderText = "Buyer Name";
                    dgvList.Columns[11].HeaderText = "Creator ID";
                    dgvList.Columns[12].HeaderText = "Created Date";
                    dgvList.Columns[13].HeaderText = "Modified Date";

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
            DataView dv_filter = new DataView(dtList, $"DocumentID LIKE '%{tbFilter.Text}%' or VendorID LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            dgvList.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtList, $"DocumentID LIKE '%{tbFilter.Text}%' or VendorID LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
                dgvList.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dtpBuyDate_ValueChanged(object sender, EventArgs e)
        {
            currentDate = dtpListDate.Value;
            if(dtpListDate2.Value > dtpListDate.Value)
            {
                dtpListDate2.Value = dtpListDate.Value;
            }
            currentDate2 = dtpListDate2.Value;
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Forms.BuyingQCForm buyingQCForm = new Forms.BuyingQCForm
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                ScaleCalibration = ScaleCalibration,
                Userlog = Userlog,
                DocNumber = "<NEW>",
                FiscalInfo = FiscalInfo
            };
            buyingQCForm.ShowDialog();

            currentDate = dtpListDate.Value;
            currentDate2 = dtpListDate2.Value;
            LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                Forms.BuyingQCForm buyingQCForm = new Forms.BuyingQCForm
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    AcumaticaCred = AcumaticaCred,
                    ScaleCom = ScaleCom,
                    ScaleCalibration = ScaleCalibration,
                    Userlog = Userlog,
                    DocNumber = dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[0].FormattedValue.ToString(),
                    //currentDate = dtpListDate.Value.Date,
                    currentDate = DateTime.Parse(dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[1].FormattedValue.ToString()),
                    FiscalInfo = FiscalInfo
                };
                buyingQCForm.ShowDialog();

                currentDate = dtpListDate.Value;
                currentDate2 = dtpListDate2.Value;
            }

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