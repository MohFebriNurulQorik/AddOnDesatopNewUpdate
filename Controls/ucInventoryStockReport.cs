using QRCoder;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ScaleAddon.Controls
{
    public partial class ucInventoryStockReport : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public UserModel Userlog { get; set; }

        private DataTable dtStock;

        public ucInventoryStockReport()
        {
            InitializeComponent();
        }

        private void ucInventoryStockReport_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Stock Movement";

            LoadData();

        }



        private void LoadData()
        {
            var startDate = dtpListDateStart.Value.ToString("yyyy-MM-dd");
            var endDate = dtpListDateEnd.Value.ToString("yyyy-MM-dd");


            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtStock = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                        InvTrans.InventoryID,
	                                        InvTrans.SubItem,
	                                        InvTrans.Process,
	                                        ISNULL((select sum(NettoIn - NettoOUT) from InventoryTransHistory Where InventoryID = InvTrans.InventoryID and Subitem = InvTrans.Subitem and Process = InvTrans.Process and TransactionDate <= '{startDate} 00:00:00' ),0) as BegBalance,
	                                        SUM(InvTrans.NettoIN) as SumNettoIN,
	                                        SUM(InvTrans.NettoOUT) as SumNettoOUT,
	                                        ISNULL((select sum(NettoIn - NettoOUT) from InventoryTransHistory Where InventoryID = InvTrans.InventoryID and Subitem = InvTrans.Subitem and Process = InvTrans.Process and TransactionDate <= '{endDate} 23:59:59'),0) as EndBalance
                                        FROM
	                                        dbo.InventoryTransHistory InvTrans
                                        WHERE
	                                        InvTrans.TransactionDate between '{startDate} 00:00:00' and '{endDate} 23:59:59'
                                        GROUP BY
	                                        InvTrans.InventoryID, InvTrans.SubItem,InvTrans.Process";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtStock);
                    dgvItem.DataSource = dtStock;

                    //Header Text
                    dgvItem.Columns[0].HeaderText = "Inventory ID";
                    dgvItem.Columns[1].HeaderText = "Sub Item";
                    dgvItem.Columns[2].HeaderText = "Process";
                    dgvItem.Columns[3].HeaderText = "Beginning Balance";
                    dgvItem.Columns[3].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItem.Columns[4].HeaderText = "Netto IN";
                    dgvItem.Columns[4].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItem.Columns[5].HeaderText = "Netto OUT";
                    dgvItem.Columns[5].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvItem.Columns[6].HeaderText = "Ending Balance";
                    dgvItem.Columns[6].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    totalStockItem(dtStock);
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

            DataView dv_filter = new DataView(dtStock, $"InventoryID LIKE '%{tbFilter.Text}%' or SubItem LIKE '%{tbFilter.Text}%'", "InventoryID Asc", DataViewRowState.CurrentRows);
            dgvItem.DataSource = dv_filter;
            totalStockItem(dv_filter.ToTable());
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtStock, $"InventoryID LIKE '%{tbFilter.Text}%' or SubItem LIKE '%{tbFilter.Text}%'", "InventoryID Asc", DataViewRowState.CurrentRows);
                dgvItem.DataSource = dv_filter;
                totalStockItem(dv_filter.ToTable());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void totalStockItem(DataTable dtGrid)
        {
            if (dtGrid.Rows.Count > 0)
            {
                decimal BegBalance = Convert.ToDecimal(dtGrid.Compute("SUM(BegBalance)", string.Empty));
                tbDetailBegBal.Text = BegBalance.ToString("N2");
                decimal SumNettoIN = Convert.ToDecimal(dtGrid.Compute("SUM(SumNettoIN)", string.Empty));
                tbDetailNettIN.Text = SumNettoIN.ToString("N2");
                decimal SumNettoOUT = Convert.ToDecimal(dtGrid.Compute("SUM(SumNettoOUT)", string.Empty));
                tbDetailNettOUT.Text = SumNettoOUT.ToString("N2");
                decimal EndBalance = Convert.ToDecimal(dtGrid.Compute("SUM(EndBalance)", string.Empty));
                tbDetailEndBal.Text = EndBalance.ToString("N2");

            }
            else
            {
                decimal BegBalance = 0;
                tbDetailBegBal.Text = BegBalance.ToString("N2");
                decimal SumNettoIN = 0;
                tbDetailNettIN.Text = SumNettoIN.ToString("N2");
                decimal SumNettoOUT = 0;
                tbDetailNettOUT.Text = SumNettoOUT.ToString("N2");
                decimal EndBalance = 0;
                tbDetailEndBal.Text = EndBalance.ToString("N2");

            }
        }


        //end of file
    }
}