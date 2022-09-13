using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucPurchaseInvoice : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        private DataTable dtList;
        private DateTime currentDate;
        private DateTime currentDate2;

        public ucPurchaseInvoice()
        {
            InitializeComponent();
        }

        private void ucPurchaseInvoice_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Purchase Invoice List";

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

                    string query = $@"SELECT *
                                    FROM
                                        PurchaseInvoice
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
                    dgvList.Columns[5].HeaderText = "Total Cash Advance";
                    dgvList.Columns[5].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[6].HeaderText = "Total Tax Deduction";
                    dgvList.Columns[6].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[7].HeaderText = "Total Loan Deduction";
                    dgvList.Columns[7].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[8].HeaderText = "Total For Payment";
                    dgvList.Columns[8].DefaultCellStyle.Format = "N2";
                    dgvList.Columns[9].HeaderText = "Document Status";
                    dgvList.Columns[10].HeaderText = "Acumatica Purchase Receipt";
                    dgvList.Columns[11].HeaderText = "NPWP";
                    dgvList.Columns[11].Visible = false;
                    dgvList.Columns[12].HeaderText = "Buyer Name";
                    dgvList.Columns[13].HeaderText = "Admin Name";
                    dgvList.Columns[14].HeaderText = "Creator ID";
                    dgvList.Columns[15].HeaderText = "Created Date";
                    dgvList.Columns[16].HeaderText = "Modified Date";

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
            if (dtpListDate2.Value > dtpListDate.Value)
            {
                dtpListDate2.Value = dtpListDate.Value;
            }
            currentDate2 = dtpListDate2.Value;
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Forms.PurchaseInvoice purchaseInvoice = new Forms.PurchaseInvoice
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                ScaleCom = ScaleCom,
                DocNumber = "<NEW>",
                Userlog = Userlog,
                FiscalInfo = FiscalInfo
            };
            purchaseInvoice.ShowDialog();

            currentDate = dtpListDate.Value;
            LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                Forms.PurchaseInvoice purchaseInvoice = new Forms.PurchaseInvoice
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    AcumaticaCred = AcumaticaCred,
                    ScaleCom = ScaleCom,
                    DocNumber = dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[0].FormattedValue.ToString(),
                    currentDate = DateTime.Parse(dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[1].FormattedValue.ToString()),
                    Userlog = Userlog,
                    FiscalInfo = FiscalInfo
                };
                purchaseInvoice.ShowDialog();

                currentDate = dtpListDate.Value;
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