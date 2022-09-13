using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucBuyingProcess : UserControl
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

        public ucBuyingProcess()
        {
            InitializeComponent();
        }

        private void ucBuyingProcess_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Process List";

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
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    //string query = $@"SELECT *
                    //                FROM
                    //                    BuyingLine
                    //                WHERE
                    //                    DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";

                    string query = $@"SELECT BuyingLine.*
                                    FROM
                                        BuyingLine
                                    WHERE
                                        BuyingLine.DocumentDate BETWEEN '{currentDate2.Date.ToString("yyyy-MM-dd")}' AND '{currentDate.Date.ToString("yyyy-MM-dd")}'
                            
";
                    //Console.WriteLine(query);
                    

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
                    dgvList.Columns[10].HeaderText = "Acumatica Purchase Receipt";
                    dgvList.Columns[11].HeaderText = "Invoice ID";
                    dgvList.Columns[11].Visible = false;
                    dgvList.Columns[12].HeaderText = "Buyer Name";
                    dgvList.Columns[13].HeaderText = "Creator ID";
                    dgvList.Columns[14].HeaderText = "Created Date";
                    dgvList.Columns[15].HeaderText = "Modified Date";
                    dgvList.Columns[15].Visible = false;
                    //dgvList.Columns[16].Visible = false;
                    //dgvList.Columns[17].Visible = false;

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
            Forms.BuyingProcessV2 buyingProcess = new Forms.BuyingProcessV2
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
            buyingProcess.ShowDialog();

            currentDate = dtpListDate.Value;
            currentDate2 = dtpListDate2.Value;
            LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                Forms.BuyingProcessV2 buyingProcess = new Forms.BuyingProcessV2
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    AcumaticaCred = AcumaticaCred,
                    ScaleCom = ScaleCom,
                    ScaleCalibration = ScaleCalibration,
                    Userlog = Userlog,
                    DocNumber = dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[0].FormattedValue.ToString(),
                    //currentDate = dtpListDate.Value.Date,
                    currentDate = DateTime.Parse(dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[14].FormattedValue.ToString()),
                    FiscalInfo = FiscalInfo
                };
                buyingProcess.ShowDialog();

                currentDate = dtpListDate.Value;
                currentDate2 = dtpListDate2.Value;
            }

            LoadData();
        }

        private void btnBatchSync_Click(object sender, EventArgs e)
        {
            Forms.BatchAcumaticaSync batchAcumaticaSync = new Forms.BatchAcumaticaSync
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                AcumaticaCred = AcumaticaCred,
                DocType = "Buying",
                HeaderTable = "BuyingLine",
                DetailTable = "BuyingLineDetail",
                AcumaticaReasonCode = ""
            };
            batchAcumaticaSync.ShowDialog();

            currentDate = dtpListDate.Value;
            currentDate2 = dtpListDate2.Value;
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