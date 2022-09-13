using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucBuyingRegistration : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }

        public AcumaticaCredModel AcumaticaCred { get; set; }
        public UserModel Userlog { get; set; }
        private DataTable dtList;
        private DateTime currentDate;

        public ucBuyingRegistration()
        {
            InitializeComponent();
        }

        private void ucBuyingRegistration_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying Registration List";

            currentDate = DateTime.Now;

            dtpListDate.Value = currentDate;

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

                    string query = $@"SELECT
	                                    BuyingRegistration.RegistrationNumber,
	                                    VendorData.VendorName AS Vendor,
	                                    BuyingRegistration.OrderNbr,
	                                    BuyingRegistration.RegistrationDate,
	                                    WarehouseSite.Descr AS Warehouse,
                                        BuyingRegistration.EstLot,
                                        BuyingRegistration.EstWeight,
                                        BuyingRegistration.BuyingUse as [Buying Use]
                                    FROM
                                        dbo.BuyingRegistration
                                        INNER JOIN
                                        dbo.VendorData
                                        ON
                                            BuyingRegistration.VendorID = VendorData.VendorID
                                        INNER JOIN
                                        dbo.WarehouseSite
                                        ON
                                            BuyingRegistration.WarehouseID = WarehouseSite.WarehouseID
                                        WHERE
                                            BuyingRegistration.RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtList);
                    dgvList.DataSource = dtList;

                    //Header Text
                    dgvList.Columns[0].HeaderText = "Registration Number";
                    dgvList.Columns[1].HeaderText = "Vendor";
                    dgvList.Columns[2].HeaderText = "Order Number";
                    dgvList.Columns[3].HeaderText = "Registration Date";
                    dgvList.Columns[4].HeaderText = "Warehouse";
                    dgvList.Columns[5].HeaderText = "Est. Lot";
                    dgvList.Columns[6].HeaderText = "Est. Weight";
                    dgvList.Columns[6].DefaultCellStyle.Format = "N2";

                    dgvList.ClearSelection();

                    if (dtList.Rows.Count > 0)
                    {
                        decimal sumEstWeight = Convert.ToDecimal(dtList.Compute("SUM(EstWeight)", string.Empty));
                        tbDetailWeight.Text = sumEstWeight.ToString("N2");
                        int sumEstLot = Convert.ToInt32(dtList.Compute("SUM(EstLot)", string.Empty));
                        tbDetailLot.Text = sumEstLot.ToString();

                        int sumReg = (int)dtList.Compute("COUNT(RegistrationNumber)", string.Empty);
                        tbDetailReg.Text = sumReg.ToString();
                    }
                    else
                    {
                        decimal sumEstWeight = 0;
                        tbDetailWeight.Text = sumEstWeight.ToString("N2");
                        int sumEstLot = 0;
                        tbDetailLot.Text = sumEstLot.ToString();

                        int sumReg = 0;
                        tbDetailReg.Text = sumReg.ToString();
                    }
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
            DataView dv_filter = new DataView(dtList, $"Vendor LIKE '%{tbFilter.Text}%'", "RegistrationNumber Asc", DataViewRowState.CurrentRows);
            dgvList.DataSource = dv_filter;
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtList, $"Vendor LIKE '%{tbFilter.Text}%'", "RegistrationNumber Asc", DataViewRowState.CurrentRows);
                dgvList.DataSource = dv_filter;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dtpRegDate_ValueChanged(object sender, EventArgs e)
        {
            currentDate = dtpListDate.Value;
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Forms.BuyingRegistration buyingRegistration = new Forms.BuyingRegistration
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                RegNumber = "<NEW>",
                currentDate = dtpListDate.Value,
                Userlog = Userlog
            };
            buyingRegistration.ShowDialog();

            currentDate = dtpListDate.Value;
            LoadData();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                Forms.BuyingRegistration buyingRegistration = new Forms.BuyingRegistration
                {
                    Warehouse = Warehouse,
                    ConnectionString = ConnectionString,
                    RegNumber = dgvList.Rows[dgvList.SelectedRows[0].Index].Cells[0].FormattedValue.ToString(),
                    currentDate = dtpListDate.Value.Date,
                    Userlog = Userlog
                };
                buyingRegistration.ShowDialog();

                currentDate = dtpListDate.Value;
            }

            LoadData();
        }
    }
}