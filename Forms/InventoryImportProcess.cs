using ExcelDataReader;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class InventoryImportProcess : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }
        public String AcumaticaReasonCode { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public string ClientID { get; set; }

        private DataTable dtDetail;
        private DataSet dsImport;

        public InventoryImportProcess()
        {
            InitializeComponent();
        }

        private void InventoryImportProcess_Load(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
                loadDetail();
            }
        }

        #region ThreadSafe

        public delegate void addTextCallback(string someText, Control tb);

        public delegate void setTextCallback(string someText, Control tb);

        public void addText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new addTextCallback(addText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text += someText;
            }
        }

        public void setText(string someText, Control tb)
        {
            if (tb.InvokeRequired)
            {
                var d = new setTextCallback(setText);
                this.Invoke(d, new object[] { someText, tb });
            }
            else
            {
                tb.Text = someText;
            }
        }

        #endregion ThreadSafe

        private void loadProcess()
        {
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory Import Process [{DocNumber}]";

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from InventoryImport where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadDetail()
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                dtDetail = new DataTable();
                try
                {
                    string query = $@"SELECT *
                                        FROM
	                                        InventoryImportDetail
                                        WHERE
	                                        DocumentID = '{DocNumber}' ";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtDetail);
                    dgvDetail.DataSource = dtDetail;

                    //Header Text
                    dgvDetail.Columns[0].HeaderText = "Document ID";
                    dgvDetail.Columns[0].Visible = false;
                    dgvDetail.Columns[1].HeaderText = "Inventory ID";
                    dgvDetail.Columns[2].HeaderText = "Sub Item";
                    dgvDetail.Columns[3].HeaderText = "Lot Number";
                    dgvDetail.Columns[4].HeaderText = "Source";
                    dgvDetail.Columns[5].HeaderText = "Stage";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Form";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.Columns[7].HeaderText = "Crop Year";
                    dgvDetail.Columns[7].Visible = false;
                    dgvDetail.Columns[8].HeaderText = "Grade";
                    dgvDetail.Columns[8].Visible = false;
                    dgvDetail.Columns[9].HeaderText = "Area";
                    dgvDetail.Columns[10].HeaderText = "Color";
                    dgvDetail.Columns[11].HeaderText = "Fermentation";
                    dgvDetail.Columns[12].HeaderText = "Length";
                    dgvDetail.Columns[13].HeaderText = "Process";
                    dgvDetail.Columns[14].HeaderText = "Stalk Position";
                    dgvDetail.Columns[15].HeaderText = "Rope";
                    dgvDetail.Columns[15].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[16].HeaderText = "Shipping";
                    dgvDetail.Columns[16].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[17].HeaderText = "Receive";
                    dgvDetail.Columns[17].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[18].HeaderText = "Tare";
                    dgvDetail.Columns[18].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[19].HeaderText = "Netto";
                    dgvDetail.Columns[19].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[20].HeaderText = "UoM";
                    dgvDetail.Columns[21].HeaderText = "Cost Unit";
                    dgvDetail.Columns[21].DefaultCellStyle.Format = "N2";
                    dgvDetail.Columns[21].Visible = false;
                    dgvDetail.Columns[22].HeaderText = "Remark";
                    dgvDetail.Columns[23].HeaderText = "Synced";
                    dgvDetail.Columns[23].Visible = false;
                    dgvDetail.Columns[24].HeaderText = "Buyer Name";
                    dgvDetail.Columns[24].Visible = false;

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        decimal sumWShipping = Convert.ToDecimal(dtDetail.Compute("SUM(WeightShipping)", string.Empty));
                        tbDetailWShipping.Text = sumWShipping.ToString("N2");
                        decimal sumWReceive = Convert.ToDecimal(dtDetail.Compute("SUM(WeightReceive)", string.Empty));
                        tbDetailWReceive.Text = sumWReceive.ToString("N2");
                        decimal sumWTare = Convert.ToDecimal(dtDetail.Compute("SUM(WeightTare)", string.Empty));
                        tbDetailWTare.Text = sumWTare.ToString("N2");
                        decimal sumWNetto = Convert.ToDecimal(dtDetail.Compute("SUM(WeightNetto)", string.Empty));
                        tbDetailWNetto.Text = sumWNetto.ToString("N2");
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
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void resetScreen()
        {
            currentDate = DateTime.Now;
            tbProcessDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory Import Process [{DocNumber}]";

            tbDocNumber.Text = DocNumber;
            tbWarehouse.Text = Warehouse.WarehouseID;

            loadDetail();
        }

        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogExcel.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                tbExcelFilename.Text = openFileDialogExcel.FileName;
                btnSaveLot.Enabled = true;
                pbImport.Value = 1;
            }
        }

        private void btnSaveLot_Click(object sender, EventArgs e)
        {
            if (DocNumber == "<NEW>")
            {
                saveDocument();
            }

            if (DocNumber != "<NEW>")
            {
                saveLot();
            }
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void setDocNumber()
        {
            tbDocNumber.Text = $"{Warehouse.WarehouseID}-{ClientID}-{currentDate.ToString("yyMMdd-HHmm")}";
        }

        private void saveDocument()
        {
            if (DocNumber == "<NEW>")
            {
                setDocNumber();
            }
            bool insertError = false;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    using (SqlCommand command = new SqlCommand("Insert_InventoryImport", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                        command.Parameters.AddWithValue("@DocumentDate", tbProcessDate.Text);
                        command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                        command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e_update)
                {
                    MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    insertError = true;
                }
                finally
                {
                    if (!insertError)
                    {
                        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Inventory Import Process [{DocNumber}]";

                        MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void saveLot()
        {
            dsImport = new DataSet();

            if (IsFileLocked(new FileInfo(openFileDialogExcel.FileName)))
            {
                MessageBox.Show($"File is locked, please close any other software using the file!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                using (var stream = File.Open(openFileDialogExcel.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        dsImport = reader.AsDataSet();

                        int importSheet = 0;
                        int importRow = 0;
                        foreach (DataTable table in dsImport.Tables)
                        {
                            if (table.TableName.Contains("Warehouse"))
                            {
                                importSheet = importSheet + 1;
                                importRow = importRow + (table.Rows.Count - 1);
                            }
                        }

                        pbImport.Value = 1;
                        pbImport.Minimum = 1;
                        pbImport.Maximum = importRow;
                        pbImport.Step = 1;

                        var bgw = new BackgroundWorker();
                        bgw.ProgressChanged += bgw_ProgressChanged;
                        bgw.DoWork += bgw_importData;
                        bgw.WorkerReportsProgress = true;
                        bgw.RunWorkerAsync();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbImport.PerformStep();
            //loadDetail();
            if (pbImport.Value == pbImport.Maximum)
            {
                loadDetail();
            }
        }

        private void bgw_importData(object sender, DoWorkEventArgs e)
        {
            foreach (DataTable table in dsImport.Tables)
            {
                if (table.TableName.Contains("Warehouse"))
                {
                    foreach (DataRow row in table.Rows)
                    {
                        if (table.Rows.IndexOf(row) == 0)
                        {
                            continue;
                        }

                        //insert all
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            try
                            {
                                if (connection.State != ConnectionState.Open)
                                {
                                    connection.Open();
                                }

                                using (SqlCommand command = new SqlCommand("Insert_InventoryImportDetail", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@DocumentID", DocNumber);
                                    command.Parameters.AddWithValue("@InventoryID", row[1].ToString());
                                    command.Parameters.AddWithValue("@SubItem", $"{row[2].ToString()}.{row[3].ToString()}.{row[4].ToString()}.{row[5].ToString()}");
                                    command.Parameters.AddWithValue("@LotNbr", row[13].ToString());
                                    command.Parameters.AddWithValue("@Source", row[7].ToString());
                                    command.Parameters.AddWithValue("@Stage", row[2].ToString());
                                    command.Parameters.AddWithValue("@tForm", row[3].ToString());
                                    command.Parameters.AddWithValue("@CropYear", row[4].ToString());
                                    command.Parameters.AddWithValue("@Grade", row[5].ToString());
                                    command.Parameters.AddWithValue("@Area", row[12].ToString());
                                    command.Parameters.AddWithValue("@Color", row[9].ToString());
                                    command.Parameters.AddWithValue("@Fermentation", row[11].ToString());
                                    command.Parameters.AddWithValue("@Length", row[10].ToString());
                                    command.Parameters.AddWithValue("@Process", row[6].ToString());
                                    command.Parameters.AddWithValue("@StalkPosition", row[8].ToString());
                                    command.Parameters.AddWithValue("@WeightRope", 0);
                                    command.Parameters.AddWithValue("@WeightShipping", 0);
                                    command.Parameters.AddWithValue("@WeightReceive", Convert.ToDecimal(row[14].ToString()));
                                    command.Parameters.AddWithValue("@WeightTare", 0);
                                    command.Parameters.AddWithValue("@WeightNetto", Convert.ToDecimal(row[14].ToString()));
                                    command.Parameters.AddWithValue("@UoM", "KG");
                                    command.Parameters.AddWithValue("@Remark", "Inventory imported");
                                    command.Parameters.AddWithValue("@CostUnit", Convert.ToDecimal(row[15].ToString()));
                                    command.Parameters.AddWithValue("@SyncDetail", 0);
                                    command.Parameters.AddWithValue("@BuyerName", row[17].ToString());

                                    command.ExecuteNonQuery();
                                }
                            }
                            catch (Exception e_update)
                            {
                                MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            finally
                            {
                                ((BackgroundWorker)sender).ReportProgress(0);
                            }
                        }
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DocNumber = "<NEW>";
            resetScreen();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
        }

        //end of file
    }
}