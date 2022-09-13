using Acumatica.Auth.Api;
using Acumatica.Auth.Model;
using Acumatica.RESTClient.Client;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Windows.Forms;


namespace ScaleAddon.Forms
{
    public partial class BuyingQCForm : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public AcumaticaCredModel AcumaticaCred { get; set; }
        public ScaleComModel ScaleCom { get; set; }
        public ScaleCalibrationModel ScaleCalibration { get; set; }
        public UserModel Userlog { get; set; }
        public FiscalInfo FiscalInfo { get; set; }
        public DateTime currentDate { get; set; }
        public string DocNumber { get; set; }
        private int lastIncrementValue = -1;
        private DataTable dtRegistration;
        private DataTable dtInventory;
        private DataTable dtDetail;

        private string tempRegNumber;
        private string tempVendorID;
        private int tempTotalLot;
        private int tempSamplingRange;
        private string tempVendorName;
        private string tempPO;
        private string tempKontrak;
        private string tempInventoryID;

        private string tempUnitPrice;
        public BuyingQCForm()
        {
            InitializeComponent();
        }

        private void BuyingQCForm_Load(object sender, EventArgs e)
        {

            if (DocNumber == "<NEW>")
            {
                resetScreen();
            }
            else
            {
                loadProcess();
                loadDetail();
                resetEntry();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DocNumber = "<NEW>";
            resetScreen();
        }

        private void loadProcess()
        {
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            tbDocNumber.Text = DocNumber;

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying QC [{DocNumber}]";

            loadComboReg();
            loadComboInv();

            getDocLastIncrement();

            //load buying registration
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                DataTable dt = new DataTable();
                try
                {
                    string query = $"select * from BuyingQC where DocumentID = '{DocNumber}' and DocumentDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        tbStatus.Text = reader.GetValue(9).ToString();
                        tbWarehouse.Text = reader.GetValue(2).ToString();
                        tempRegNumber = reader.GetValue(5).ToString();
                        cbRegistration.SelectedIndex = cbRegistration.Items.IndexOf(tempRegNumber);
                        tbRegistration.Text = tempRegNumber;
                        tbTotalLot.Text = reader.GetValue(10).ToString();
                        tbSamplingRange.Text = reader.GetValue(14).ToString();

                        tempInventoryID = reader.GetValue(7).ToString();
                        cbInventory.SelectedIndex = cbInventory.FindString(tempInventoryID);
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
            tbBuyingDate.Text = currentDate.Date.ToString("yyyy-MM-dd");
            DocNumber = "<NEW>";

            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying QC [{DocNumber}]";

            loadComboReg();
            loadComboInv();

            tbDocNumber.Text = DocNumber;
            tbStatus.Text = "";
            tbVendorID.Text = "";
            tbVendorClass.Text = "";
            tbVendorDetails.Text = "";
            tbWarehouse.Text = Warehouse.WarehouseID;
            tbTotalLot.Text = "";

            tempVendorID = "";
            tempInventoryID = "";
            tempPO = "";
            tempKontrak = "";
            tempInventoryID = "";

            resetEntry();
            loadDetail();
        }

        private void loadComboReg()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbRegistration.SelectedIndex = -1;

                dtRegistration = new DataTable();
                try
                {
                    //string query = $@"SELECT
                    //                     dbo.BuyingRegistration.*, 
                    //                     dbo.BuyingQC.DocumentID, 
                    //                     dbo.BuyingQC.DocumentDate
                    //                    FROM
                    //                     dbo.BuyingRegistration
                    //                     LEFT JOIN
                    //                     dbo.BuyingQC
                    //                     ON 
                    //                      dbo.BuyingRegistration.RegistrationNumber = dbo.BuyingQC.RegistrationNumber AND
                    //                      dbo.BuyingRegistration.RegistrationDate = dbo.BuyingQC.DocumentDate
                    //                    WHERE
                    //                        dbo.BuyingRegistration.RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'
                    //                    AND
                    //                        dbo.BuyingRegistration.WarehouseID = '{Warehouse.WarehouseID}'";

                    string query = $@"SELECT
                                         	BuyingRegistration.*, 
                                             BuyingQC.DocumentID AS QCID, 
                                             BuyingQC.DocumentDate AS QCDate, 
                                             BuyingLine.DocumentID AS BuyingID, 
                                             BuyingLine.DocumentDate AS BuyingDate
                                        FROM
                                         dbo.BuyingRegistration
                                         LEFT JOIN
                                         dbo.BuyingQC
                                         ON 
                                          dbo.BuyingRegistration.RegistrationNumber = dbo.BuyingQC.RegistrationNumber AND
                                          dbo.BuyingRegistration.RegistrationDate = dbo.BuyingQC.DocumentDate
                                         LEFT JOIN
                                         dbo.BuyingLine
                                         ON 
                                          BuyingRegistration.RegistrationDate = BuyingLine.DocumentDate AND
                                          BuyingRegistration.RegistrationNumber = BuyingLine.RegistrationNumber
                                        WHERE
                                            dbo.BuyingRegistration.RegistrationDate = '{currentDate.Date.ToString("yyyy-MM-dd")}'
                                        AND
                                            dbo.BuyingRegistration.WarehouseID = '{Warehouse.WarehouseID}'";
                                        //AND
                                        // BuyingLine.DocumentID IS NULL";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtRegistration);
                    string[] arrray;
                    if (DocNumber.Contains("NEW"))
                    {
                        arrray = dtRegistration.Rows.OfType<DataRow>().Where(j => j[13].ToString().Length == 0 && j[15].ToString().Length == 0).Select(k => k[0].ToString()).ToArray();
                    }
                    else
                    {
                        //arrray = dtRegistration.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                        arrray = dtRegistration.Rows.OfType<DataRow>().Where(j => j[13].ToString().Length == 0 && j[15].ToString().Length == 0 || j[13].ToString() == DocNumber).Select(k => k[0].ToString()).ToArray();
                    }


                    //new AutoCompleteBehavior(cbRegistration);
                    cbRegistration.Items.Clear();
                    cbRegistration.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboInv()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                cbInventory.SelectedIndex = -1;

                dtInventory = new DataTable();
                try
                {
                    string query = "select * from InventoryItem";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtInventory);
                    string[] arrray = dtInventory.Rows.OfType<DataRow>().Select(k => k[0].ToString() + " | " + k[1].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbInventory);
                    cbInventory.Items.Clear();
                    cbInventory.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void getDocLastIncrement()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                DataTable dt = new DataTable();
                try
                {
                    string query = "select * from NumberingSetting where NumberingID = 'BY/QC'";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();

                        DateTime dbDate = Convert.ToDateTime(reader.GetValue(2).ToString());
                        int dbIncrement = Convert.ToInt32(reader.GetValue(1).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        if (currentDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year == dbDate.AddMonths(-FiscalInfo.StartingFiscalMonth).AddMonths(1).Year)
                        {
                            lastIncrementValue = dbIncrement;
                        }
                        else
                        {
                            lastIncrementValue = 0;
                        }
                    }
                    else
                    {
                        lastIncrementValue = 0;
                    }
                }
                catch (Exception e)
                {
                    lastIncrementValue = -1;
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void setDocNumber()
        {
            getDocLastIncrement();

            if (lastIncrementValue >= 0)
            {
                var currentIncrement = lastIncrementValue + 1;
                //tbDocNumber.Text = $"{currentDate.ToString("yy")}{Warehouse.WarehouseID}-BY/IN-{currentIncrement.ToString().PadLeft(4, '0')}";
                //tbDocNumber.Text = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-BY/QC-{currentIncrement.ToString().PadLeft(4, '0')}";
                var docNbr = $"{Math.Abs(FiscalInfo.CurrentFiscalYear) % 100}{Warehouse.WarehouseID}-BY/QC-{currentIncrement.ToString().PadLeft(4, '0')}";
                
                if (!checkDocNbrExist(docNbr))
                {
                    tbDocNumber.Text = docNbr;
                }
                else
                {
                    tbDocNumber.Text = "<NEW>";
                    MessageBox.Show($"Document number {docNbr} already exist, or unable to check existing document number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        private bool checkDocNbrExist(string docNbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM BuyingQC WHERE DocumentID = '{docNbr}' ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        int exist = Convert.ToInt32(reader.GetValue(0).ToString());
                        //if (currentDate.Year == dbDate.Year)
                        if (exist == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return true;
                    //MessageBox.Show(e.ToString());
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDocument();
            loadDetail();
            resetEntry();
        }

        private void saveDocument()
        {
            if (cbRegistration.SelectedItem != null && cbInventory.SelectedItem != null)
            {
                bool incrementError = false;
                int OperationType = 1;

                if (DocNumber == "<NEW>")
                {
                    incrementError = true;
                    OperationType = 0;
                    setDocNumber();
                    //tbStatus.Text = "OPEN";

                    if (DocNumber != "<NEW>" && lastIncrementValue >= 0)
                    {
                        incrementError = false;

                        tbStatus.Text = "OPEN";
                        lastIncrementValue = lastIncrementValue + 1;
                        //DocNumber = tbDocNumber.Text;

                    }

                }
                bool insertError = false;
                if (!incrementError)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                    {
                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            using (SqlCommand command = new SqlCommand("Insert_BuyingQC_v2", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@DocumentID", tbDocNumber.Text);
                                command.Parameters.AddWithValue("@DocumentDate", tbBuyingDate.Text);
                                command.Parameters.AddWithValue("@WarehouseID", tbWarehouse.Text);
                                command.Parameters.AddWithValue("@VendorID", tbVendorID.Text);
                                command.Parameters.AddWithValue("@VendorDetails", tbVendorDetails.Text);
                                command.Parameters.AddWithValue("@RegistrationNumber", cbRegistration.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@OrderNbr", tempPO);
                                command.Parameters.AddWithValue("@InventoryID", cbInventory.SelectedItem.ToString().Split('|')[0].Trim());
                                command.Parameters.AddWithValue("@VendorClass", tbVendorClass.Text);
                                command.Parameters.AddWithValue("@Status", tbStatus.Text);
                                command.Parameters.AddWithValue("@TotalLot", tbTotalLot.Text);
                                command.Parameters.AddWithValue("@CreatorID", Userlog.UserName);
                                command.Parameters.AddWithValue("@SamplingRange", tbSamplingRange.Text);
                                command.Parameters.AddWithValue("@OperationType", OperationType);

                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            insertError = true;
                            tbDocNumber.Text = "<NEW>";
                            loadComboReg();
                        }
                        //finally
                        //{
                        //    if (!insertError)
                        //    {
                        //        this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying QC [{DocNumber}]";
                                
                        //        using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                        //        {
                        //            command.CommandType = CommandType.StoredProcedure;
                        //            command.Parameters.AddWithValue("@NumberingID", "BY/QC");
                        //            command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                        //            command.Parameters.AddWithValue("@NumberingDate", currentDate);

                        //            command.ExecuteNonQuery();
                        //        }

                        //        //MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    }
                        //}


                        try
                        {
                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                            }

                            if (!insertError && this.Text.Contains("<NEW>"))
                            {
                                this.Text = $"Universal Leaf [{Warehouse.Descr}] - Buying QC [{DocNumber}]";

                                using (SqlCommand command = new SqlCommand("Insert_NumberingSetting", connection))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@NumberingID", "BY/QC");
                                    command.Parameters.AddWithValue("@LastIncrementValue", lastIncrementValue);
                                    command.Parameters.AddWithValue("@NumberingDate", currentDate);

                                    command.ExecuteNonQuery();
                                }

                                MessageBox.Show("Save complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        catch (Exception e_update)
                        {
                            MessageBox.Show($"Update numbering setting failed, please contact IT", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            insertError = true;
                        }




                    }
                }
                else
                {
                    MessageBox.Show($"Failed to get numbering setting, please check database connection", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {
                MessageBox.Show($"Please choose registration number and/or inventory !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            DocNumber = ((TextBox)sender).Text;
        }

        private void cbRegistration_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbRegistration.SelectedIndex >= 0)
            {
                tempRegNumber = cbRegistration.SelectedItem.ToString();

                var results = from myRow in dtRegistration.AsEnumerable()
                              where myRow.Field<string>("RegistrationNumber") == tempRegNumber
                              select myRow;

                foreach (var result in results)
                {
                    // do something with it
                    tempVendorID = result.ItemArray[1].ToString();
                    tempPO = result.ItemArray[2].ToString();
                    tempInventoryID = result.ItemArray[3].ToString();
                    tempKontrak = result.ItemArray[7].ToString();
                    tempTotalLot = Convert.ToInt32(result.ItemArray[9].ToString());

                    tbTotalLot.Text = tempTotalLot.ToString();
                }

                if (tempInventoryID.Length > 0)
                {
                    cbInventory.SelectedIndex = cbInventory.FindString(tempInventoryID);
                    cbInventory.Enabled = false;
                }
                else
                {
                    cbInventory.SelectedIndex = -1;
                    cbInventory.Enabled = true;
                }

                tbVendorID.Text = tempVendorID;
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        string query = $"select * from VendorData where VendorID = '{tempVendorID}'";
                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            reader.Read();

                            string details = "";
                            details = $@"{reader.GetValue(1)}{Environment.NewLine}{reader.GetValue(6) ?? ""}, {reader.GetValue(7) ?? ""}{Environment.NewLine}{reader.GetValue(8) ?? ""}, {reader.GetValue(9) ?? ""}";

                            tempVendorName = reader.GetValue(1).ToString();
                            tbVendorDetails.Text = details ?? "";
                            tbVendorClass.Text = reader.GetValue(12).ToString() ?? "";
                        }
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.ToString());
                    }
                }

            }
        }

        private void cbInventory_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbInventory.SelectedIndex >= 0)
            {
                tempInventoryID = cbInventory.SelectedItem.ToString().Split('|')[0].Trim();
            }
            else
            {
                tempInventoryID = "";
            }

            resetEntry();
        }

        private void btnToogle_Click(object sender, EventArgs e)
        {
            if (tbRegistration.Visible)
            {
                tbRegistration.Visible = false;
                cbRegistration.Visible = true;
            }
            else
            {
                tbRegistration.Visible = true;
                cbRegistration.Visible = false;
            }
        }

        private void tbRegistration_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                int index = cbRegistration.FindStringExact(tbRegistration.Text);
                if (index >= 0)
                {
                    cbRegistration.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Registration Code not available !");
                }
            }
        }

        private void resetEntry()
        {
            //loadDetail();

            groupEntry.Text = "Lot Entry [<NEW>]";
            groupEntry.BackgroundImage = null;

            tbEntryInv.Text = tempInventoryID;
            loadComboSamplingLot();

            setLotNbr();

            tbEntryRemark.Text = "";
            switch (tbStatus.Text)
            {
                case "OPEN":
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>" && tempSamplingRange > 0) { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnPrintDoc.Enabled = true;
                    break;

                case "SYNCED":
                    btnSave.Enabled = false;
                    btnSaveLot.Enabled = false;
                    btnPrintDoc.Enabled = true;
                    break;

                default:
                    btnSave.Enabled = true;
                    if (tbDocNumber.Text != "<NEW>" && tempSamplingRange > 0) { btnSaveLot.Enabled = true; } else { btnSaveLot.Enabled = false; }
                    btnPrintDoc.Enabled = false;
                    break;
            }

        }


        private void setLotNbr()
        {

            if (cbLotSample.SelectedIndex >= 0 && tempSamplingRange >0)
            {
                int currLot = Convert.ToInt32(cbLotSample.SelectedItem.ToString());

                int rangeStart = currLot - ((currLot-1) % tempSamplingRange);
                int rangeEnd = rangeStart + (tempSamplingRange-1);

                if(rangeEnd > tempTotalLot)
                {
                    rangeEnd = tempTotalLot;
                }

                string rStart = "";
                string rEnd = "";
                if (rangeStart >= 10) { rStart = rangeStart.ToString(); } else { rStart = $"0{rangeStart}"; }
                if (rangeEnd >= 10) { rEnd = rangeEnd.ToString(); } else { rEnd = $"0{rangeEnd}"; }

                string lotnbr = $"{rStart}-{rEnd}";
                tbEntryLot.Text = lotnbr.Trim();
            }

        }

        private void loadComboSamplingLot()
        {
            cbLotSample.Items.Clear();
            for (int i = 1; i <= tempTotalLot; i++)
            {
                cbLotSample.Items.Add(i);
            }
        
        }

        private void cbLotSample_SelectedIndexChanged(object sender, EventArgs e)
        {
            setLotNbr();
        }

        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            resetEntry();
        }

        private void btnSaveLot_Click(object sender, EventArgs e)
        {

            //if (DocNumber == "<NEW>")
            //{
            //    saveDocument();
            //}


            if (DocNumber != "<NEW>")
            {
                if (groupEntry.Text.Contains("<NEW>"))
                {
                    setLotNbr();
                }


                if (cbLotSample.SelectedIndex >= 0)
                {
                    saveLot();
                }
                else
                {
                    MessageBox.Show($"No lot selected, please check entry!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void saveLot()
        {
            if (cbLotSample.SelectedIndex>=0)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (SqlCommand command = new SqlCommand("Insert_BuyingQCDetail", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@DocumentID", DocNumber);
                            command.Parameters.AddWithValue("@InventoryID", tbEntryInv.Text);
                            command.Parameters.AddWithValue("@LotNbr", tbEntryLot.Text);
                            command.Parameters.AddWithValue("@Remark", tbEntryRemark.Text);
                            command.Parameters.AddWithValue("@StatusReject", checkReject.Checked ? 1 : 0);
                            command.Parameters.AddWithValue("@OrderNbr", tempPO);
                            command.Parameters.AddWithValue("@NoKontrak", tempKontrak ?? "");
                            command.Parameters.AddWithValue("@LotNbrSample", cbLotSample.SelectedItem.ToString());

                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        
                        loadDetail();
                        resetEntry();
                    }
                }
            }
        }


        private void tbSamplingRange_TextChanged(object sender, EventArgs e)
        {
            tempSamplingRange = Convert.ToInt32(((TextBox)sender).Text);

            if(tempSamplingRange > 0)
            {
                tbSamplingRange.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                tbSamplingRange.BackColor = System.Drawing.Color.LightCoral;
            }

            btnSaveLot.Enabled = false;

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
	                                        BuyingQCDetail
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
                    dgvDetail.Columns[2].HeaderText = "Lot Number";
                    dgvDetail.Columns[3].HeaderText = "Remark";
                    dgvDetail.Columns[4].HeaderText = "Rejected";
                    dgvDetail.Columns[4].Visible = false;
                    dgvDetail.Columns[5].HeaderText = "Order Number";
                    dgvDetail.Columns[5].Visible = false;
                    dgvDetail.Columns[6].HeaderText = "Contract";
                    dgvDetail.Columns[6].Visible = false;
                    dgvDetail.Columns[7].HeaderText = "Lot Sample";

                    dgvDetail.ClearSelection();

                    if (dtDetail.Rows.Count > 0)
                    {
                        int countLot = Convert.ToInt32(dtDetail.Compute("COUNT(LotNbr)", string.Empty));
                        tbDetailLot.Text = countLot.ToString();
                        //disable change reg
                        cbRegistration.Enabled = false;
                        tbSamplingRange.ReadOnly = true;
                        tbSamplingRange.BackColor = System.Drawing.SystemColors.Info;
                    }
                    else
                    {
                        //enable change reg
                        cbRegistration.Enabled = true;
                        tbSamplingRange.ReadOnly = false;
                        tbSamplingRange.BackColor = System.Drawing.SystemColors.Window;
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void dgvDetail_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in dgvDetail.Rows)
            {
                if (Convert.ToInt32(Myrow.Cells[4].Value) == 1)
                {
                    Myrow.DefaultCellStyle.BackColor = System.Drawing.Color.PaleVioletRed;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
            }
        }




        //end of file
    }
}
