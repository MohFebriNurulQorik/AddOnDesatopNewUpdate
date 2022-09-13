using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScaleAddon.Forms
{
    public partial class InventoryRelocate : Form
    {
        public WarehouseModel Warehouse { get; set; }
        public string ConnectionString { get; set; }
        public UserModel Userlog { get; set; }

        public DateTime currentDate { get; set; }

        private DataTable dtLot;
        private DataTable dtLoc;
        private DataTable dtEntry;

        private string tempLocation;

        public InventoryRelocate()
        {
            InitializeComponent();

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
        private static DataTable GetTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("LotNbr", typeof(string));
            table.Columns.Add("InventoryID", typeof(string));
            table.Columns.Add("SubItem", typeof(string));
            table.Columns.Add("OldLocationInfo", typeof(string));
            table.Columns.Add("NewLocationInfo", typeof(string));

            return table;
        }
        private void InventoryRelocate_Load(object sender, EventArgs e)
        {
            
            resetScreen();

        }


        private void resetScreen()
        {
            currentDate = DateTime.Now;
            this.Text = $"Universal Leaf [{Warehouse.Descr}] - Stock Relocation";
            dtEntry = GetTable();
            loadEntryGroup();

            tbWarehouse.Text = Warehouse.WarehouseID;
            loadComboLocation();
            loadComboLot();
        }

        private void loadEntry(string LotNbr)
        {
            //load data untuk grid
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //create new dt
                //dtEntry = new DataTable();
                try
                {
                    //string query = $"SELECT * from BuyingRegistration where RegistrationDate = '{currentDate.ToString()}'";

                    string query = $@"SELECT *
                                        FROM
	                                        StockItem
                                        WHERE
	                                        LotNbr = '{LotNbr}'
                                        AND StatusStock = 1";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        DataRow dr = dtEntry.NewRow();
                        bool alreadyExist = false;

                        dr["LotNbr"] = reader.GetValue(3).ToString();
                        dr["InventoryID"] = reader.GetValue(1).ToString();
                        dr["SubItem"] = reader.GetValue(2).ToString();
                        dr["OldLocationInfo"] = reader.GetValue(26).ToString();
                        dr["NewLocationInfo"] = tempLocation;

                        for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow drOld = dtEntry.Rows[i];
                            if (drOld["LotNbr"].ToString() == dr["LotNbr"].ToString()) { alreadyExist = true; }
                        }

                        if (!alreadyExist) { dtEntry.Rows.Add(dr); }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboLocation()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtLoc = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    WarehouseLocation.LocationID
                                    FROM
	                                    dbo.WarehouseLocation
                                    WHERE
	                                    WarehouseID = '{Warehouse.WarehouseID}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtLoc);
                    string[] arrray = dtLoc.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbLocation);
                    cbLocation.Items.Clear();
                    cbLocation.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void loadComboLot()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear combo
                //cbFarmer.DataSource = null;
                dtLot = new DataTable();
                try
                {
                    string query = $@"SELECT
	                                    StockItem.LotNbr
                                    FROM
	                                    dbo.StockItem
                                    WHERE
	                                    StockItem.StatusStock = 1
                                    AND StockItem.LocationInfo != '{tempLocation}'";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dtLot);
                    string[] arrray = dtLot.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();

                    //new AutoCompleteBehavior(cbLot);
                    cbLot.Items.Clear();
                    cbLot.Items.AddRange(arrray);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }

        private void cbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            tempLocation = cbLocation.SelectedItem.ToString(); ;
            loadComboLot();
        }

        private void cbLot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLot.SelectedIndex >= 0)
            {
                loadEntry(cbLot.SelectedItem.ToString());
                loadEntryGroup();

            }
        }

        private void loadEntryGroup()
        {
            dgvEntry.DataSource = dtEntry;

            //Header Text
            dgvEntry.Columns[0].HeaderText = "Lot Number";
            dgvEntry.Columns[1].HeaderText = "Inventory ID";
            dgvEntry.Columns[2].HeaderText = "Sub Item";
            dgvEntry.Columns[3].HeaderText = "Old Location Info";
            dgvEntry.Columns[4].HeaderText = "New Location Info";
            dgvEntry.ClearSelection();

            if (dtEntry.Rows.Count > 0)
            {
                cbLocation.Enabled = false;

                //disable change reg
            }
            else
            {
                cbLocation.Enabled = true;
                //enable change reg
            }
        }

        private void removeLot()
        {
            if (dgvEntry.SelectedRows.Count > 0)
            {
                var lotNbr = dgvEntry.Rows[dgvEntry.SelectedRows[0].Index].Cells[0].FormattedValue.ToString();

                for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtEntry.Rows[i];
                    if (dr["LotNbr"].ToString() == lotNbr) { dr.Delete(); }
                }
                dtEntry.AcceptChanges();

                loadEntryGroup();
            }
        }

        private void saveEntry()
        {
            if (dtEntry.Rows.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        for (int i = dtEntry.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtEntry.Rows[i];

                            using (SqlCommand command = new SqlCommand("Update_StockItemLocationInfo", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@LotNbr", dr["LotNbr"].ToString());
                                command.Parameters.AddWithValue("@LocationInfo", dr["NewLocationInfo"].ToString());

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception e_update)
                    {
                        MessageBox.Show($"--Insert error! {e_update.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        MessageBox.Show("Item relocation complete...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        resetScreen();
                    }
                }
            }
        }

        private void btnRemoveEntry_Click(object sender, EventArgs e)
        {
            removeLot();
        }

        private void btnSaveEntry_Click(object sender, EventArgs e)
        {
            saveEntry();
        }

        private void tbLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                int index = cbLot.FindStringExact(tbLot.Text);
                if (index >= 0)
                {
                    cbLot.SelectedIndex = index;
                }
                else
                {
                    MessageBox.Show($"Lot number not available !");
                }
            }
        }

        private void btnToogle_Click(object sender, EventArgs e)
        {
            if (tbLot.Visible)
            {
                tbLot.Visible = false;
                cbLot.Visible = true;
            }
            else
            {
                tbLot.Visible = true;
                cbLot.Visible = false;
            }
        }

        private bool checkLotNbrInSTock(string lotnbr)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                //clear
                try
                {
                    string query = $"IF EXISTS ( SELECT * FROM StockItem WHERE LotNbr = '{lotnbr}' and StatusStock = 1 ) BEGIN SELECT 1 END ELSE BEGIN SELECT 0 END";
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



        //end of file
    }
}
