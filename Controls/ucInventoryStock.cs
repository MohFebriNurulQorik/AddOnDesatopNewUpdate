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
    public partial class ucInventoryStock : UserControl
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public UserModel Userlog { get; set; }

        public AcumaticaCredModel AcumaticaCred { get; set; }
        private DataTable dtItem;
        private DataTable dtProcess;

        public ucInventoryStock()
        {
            InitializeComponent();
        }

        private void ucInventoryStock_Load(object sender, EventArgs e)
        {
            Dock = DockStyle.Fill;
            ParentForm.Text = $"Universal Leaf [{Warehouse.Descr}] - Tobacco Stock List";

            loadComboProcess();
            LoadData();

            cbProcess.SelectedIndex = 0;
            cbCategory.SelectedIndex = 0;
        }



        private void LoadData()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //DataTable dt = new DataTable();
                dtItem = new DataTable();
                try
                {
                    string query = $"SELECT * from StockItem WHERE StatusStock > 0";
                    if (cbProcess.SelectedItem != null)
                    {
                        var processCode = cbProcess.SelectedItem.ToString().Split('|')[0].Trim();
                        if (processCode != "ALL")
                        {
                            query = $"SELECT * from StockItem WHERE StatusStock > 0 and Process = '{processCode}'";
                        }
                    }

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtItem);
                    dgvItem.DataSource = dtItem;

                    //Header Text
                    dgvItem.Columns[0].HeaderText = "Document ID";
                    dgvItem.Columns[1].HeaderText = "Inventory ID";
                    dgvItem.Columns[2].HeaderText = "Sub Item";
                    dgvItem.Columns[3].HeaderText = "Lot Number";
                    dgvItem.Columns[4].HeaderText = "Source";
                    dgvItem.Columns[5].HeaderText = "Stage";
                    dgvItem.Columns[6].HeaderText = "Form";
                    dgvItem.Columns[7].HeaderText = "CropYear";
                    dgvItem.Columns[8].HeaderText = "Grade";
                    dgvItem.Columns[9].HeaderText = "Area";
                    dgvItem.Columns[10].HeaderText = "Color";
                    dgvItem.Columns[11].HeaderText = "Fermentation";
                    dgvItem.Columns[12].HeaderText = "Length";
                    dgvItem.Columns[13].HeaderText = "Process";
                    dgvItem.Columns[14].HeaderText = "Stalk Position";
                    dgvItem.Columns[15].HeaderText = "Rope";
                    dgvItem.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[16].HeaderText = "Shipping";
                    dgvItem.Columns[16].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[17].HeaderText = "Receive";
                    dgvItem.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[18].HeaderText = "Tare";
                    dgvItem.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[19].HeaderText = "Netto";
                    dgvItem.Columns[19].DefaultCellStyle.Format = "N2";
                    dgvItem.Columns[20].HeaderText = "UoM";
                    dgvItem.Columns[21].HeaderText = "Remark";
                    dgvItem.Columns[22].HeaderText = "Stock";
                    dgvItem.Columns[22].Visible = false;
                    dgvItem.Columns[23].HeaderText = "Last Modified";
                    dgvItem.Columns[23].Visible = false;
                    dgvItem.Columns[24].HeaderText = "Buyer Name";
                    dgvItem.Columns[25].HeaderText = "Created ON";
                    dgvItem.Columns[25].Visible = false;

                    totalStockItem(dtItem);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboProcess()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbProcess.SelectedIndex = -1;

                dtProcess = new DataTable();
                try
                {
                    string query = $"select * from ItemAttribute where CodeType = 'PROCESS' and Active = 1";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtProcess);
                    string[] arrray = dtProcess.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[2].ToString()).ToArray();
                    string[] optionAll = { "ALL | All Process" };

                    //new AutoCompleteBehavior(cbProcess);
                    cbProcess.Items.Clear();
                    cbProcess.Items.AddRange(optionAll);
                    cbProcess.Items.AddRange(arrray);
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
            //DataView dv_filter = new DataView(dtItem, $"DocumentID LIKE '%{tbFilter.Text}%' or LotNbr LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            //var filter = tbFilter.Text;

            //if (filter == "")
            //{
            //    DataView dv_filter = new DataView(dtItem, $"{cbCategory.Text} like '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            //    dgvItem.DataSource = dv_filter;
            //    totalStockItem(dv_filter.ToTable());
            //}
            //else
            //{
            //    DataView dv_filter = new DataView(dtItem, $"{cbCategory.Text} like '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            //    dgvItem.DataSource = dv_filter;
            //    totalStockItem(dv_filter.ToTable());
            //}



            DataView dv_filter = new DataView(dtItem, $"{cbCategory.Text} like '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
            dgvItem.DataSource = dv_filter;
            totalStockItem(dv_filter.ToTable());
        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
                DataView dv_filter = new DataView(dtItem, $"DocumentID LIKE '%{tbFilter.Text}%' or LotNbr LIKE '%{tbFilter.Text}%'", "DocumentID Asc", DataViewRowState.CurrentRows);
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
                decimal sumWShipping = Convert.ToDecimal(dtGrid.Compute("SUM(WeightShipping)", string.Empty));
                tbDetailWShipping.Text = sumWShipping.ToString("N2");
                decimal sumWReceive = Convert.ToDecimal(dtGrid.Compute("SUM(WeightReceive)", string.Empty));
                tbDetailWReceive.Text = sumWReceive.ToString("N2");
                decimal sumWTare = Convert.ToDecimal(dtGrid.Compute("SUM(WeightTare)", string.Empty));
                tbDetailWTare.Text = sumWTare.ToString("N2");
                decimal sumWNetto = Convert.ToDecimal(dtGrid.Compute("SUM(WeightNetto)", string.Empty));
                tbDetailWNetto.Text = sumWNetto.ToString("N2");

                int sumLot = (int)dtGrid.Compute("COUNT(LotNbr)", string.Empty);
                tbDetailLot.Text = sumLot.ToString();
            }
            else
            {
                decimal sumWShipping = 0;
                tbDetailWShipping.Text = sumWShipping.ToString("N2");
                decimal sumWReceive = 0;
                tbDetailWReceive.Text = sumWReceive.ToString("N2");
                decimal sumWTare = 0;
                tbDetailWTare.Text = sumWTare.ToString("N2");
                decimal sumWNetto = 0;
                tbDetailWNetto.Text = sumWNetto.ToString("N2");

                int sumLot = 0;
                tbDetailLot.Text = sumLot.ToString();
            }
        }

        private void btnPrintLot_Click(object sender, EventArgs e)
        {
            if (dgvItem.SelectedRows.Count > 0)
            {
                var lotNbr = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[3].FormattedValue.ToString();

                QRCoder.QRCodeGenerator qRCodeGenerator = new QRCoder.QRCodeGenerator();
                QRCodeData qrCodeData = qRCodeGenerator.CreateQrCode($"{lotNbr}", QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);

                string QRImage = ImageToBase64(qrCodeImage, System.Drawing.Imaging.ImageFormat.Bmp);

                Forms.GenericLotPrint lotPrint = new Forms.GenericLotPrint
                {
                    LotNumber = lotNbr,
                    Source = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[4].FormattedValue.ToString(),
                    StalkPos = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[14].FormattedValue.ToString(),
                    Ferment = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[11].FormattedValue.ToString(),
                    Buyer = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[24].FormattedValue.ToString(),
                    InventoryID = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[1].FormattedValue.ToString(),
                    Process = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[13].FormattedValue.ToString(),
                    Stage = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[5].FormattedValue.ToString(),
                    Grade = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[8].FormattedValue.ToString(),
                    Color = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[10].FormattedValue.ToString(),
                    Weight = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[19].FormattedValue.ToString(),
                    Length = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[12].FormattedValue.ToString(),
                    Warehouse = Warehouse.WarehouseID,
                    Date = DateTime.Parse( dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[23].FormattedValue.ToString()).ToString("yyyy-MM-dd"),
                    Remark = dgvItem.Rows[dgvItem.SelectedRows[0].Index].Cells[21].FormattedValue.ToString(),
                    QRImage = QRImage
                };
                lotPrint.ShowDialog();
            }
            else
            {
                MessageBox.Show("Select bale/lot first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        private void tnRelocate_Click(object sender, EventArgs e)
        {

            Forms.InventoryRelocate inventoryRelocate = new Forms.InventoryRelocate
            {
                Warehouse = Warehouse,
                ConnectionString = ConnectionString,
                Userlog = Userlog
            };
            inventoryRelocate.ShowDialog();


        }


        //end of file
    }
}