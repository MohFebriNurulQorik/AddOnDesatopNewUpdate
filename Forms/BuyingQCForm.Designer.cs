
namespace ScaleAddon.Forms
{
    partial class BuyingQCForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrintDoc = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnToogle = new System.Windows.Forms.Button();
            this.tbRegistration = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.tbTotalLot = new System.Windows.Forms.TextBox();
            this.cbInventory = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbBuyingDate = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVendorID = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbVendorClass = new System.Windows.Forms.TextBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbVendorDetails = new System.Windows.Forms.TextBox();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRegistration = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSamplingRange = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbEntryLot = new System.Windows.Forms.TextBox();
            this.tbEntryInv = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbEntryRemark = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.btnSaveLot = new System.Windows.Forms.Button();
            this.btnAddEntry = new System.Windows.Forms.Button();
            this.checkReject = new System.Windows.Forms.CheckBox();
            this.label31 = new System.Windows.Forms.Label();
            this.groupEntry = new System.Windows.Forms.GroupBox();
            this.cbLotSample = new System.Windows.Forms.ComboBox();
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label38 = new System.Windows.Forms.Label();
            this.tbDetailLot = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupEntry.SuspendLayout();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPrintDoc);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(859, 74);
            this.panel1.TabIndex = 11;
            // 
            // btnPrintDoc
            // 
            this.btnPrintDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintDoc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintDoc.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrintDoc.Location = new System.Drawing.Point(-236, 4);
            this.btnPrintDoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintDoc.Name = "btnPrintDoc";
            this.btnPrintDoc.Size = new System.Drawing.Size(59, 54);
            this.btnPrintDoc.TabIndex = 103;
            this.btnPrintDoc.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_30;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.Location = new System.Drawing.Point(99, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 65);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save Doc";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAdd.Image = global::ScaleAddon.Properties.Resources.icons8_plus_30;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 65);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "New Doc";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnToogle
            // 
            this.btnToogle.Image = global::ScaleAddon.Properties.Resources.icons8_search_161;
            this.btnToogle.Location = new System.Drawing.Point(386, 143);
            this.btnToogle.Margin = new System.Windows.Forms.Padding(4);
            this.btnToogle.Name = "btnToogle";
            this.btnToogle.Size = new System.Drawing.Size(37, 28);
            this.btnToogle.TabIndex = 132;
            this.btnToogle.UseVisualStyleBackColor = true;
            this.btnToogle.Click += new System.EventHandler(this.btnToogle_Click);
            // 
            // tbRegistration
            // 
            this.tbRegistration.BackColor = System.Drawing.SystemColors.Window;
            this.tbRegistration.Location = new System.Drawing.Point(183, 145);
            this.tbRegistration.Margin = new System.Windows.Forms.Padding(4);
            this.tbRegistration.Name = "tbRegistration";
            this.tbRegistration.Size = new System.Drawing.Size(193, 22);
            this.tbRegistration.TabIndex = 131;
            this.tbRegistration.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbRegistration_KeyPress);
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(431, 148);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(59, 16);
            this.label34.TabIndex = 130;
            this.label34.Text = "Total Lot";
            // 
            // tbTotalLot
            // 
            this.tbTotalLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbTotalLot.Location = new System.Drawing.Point(597, 144);
            this.tbTotalLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbTotalLot.Name = "tbTotalLot";
            this.tbTotalLot.ReadOnly = true;
            this.tbTotalLot.Size = new System.Drawing.Size(239, 22);
            this.tbTotalLot.TabIndex = 129;
            // 
            // cbInventory
            // 
            this.cbInventory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInventory.FormattingEnabled = true;
            this.cbInventory.Location = new System.Drawing.Point(183, 273);
            this.cbInventory.Margin = new System.Windows.Forms.Padding(4);
            this.cbInventory.Name = "cbInventory";
            this.cbInventory.Size = new System.Drawing.Size(239, 24);
            this.cbInventory.TabIndex = 128;
            this.cbInventory.SelectedIndexChanged += new System.EventHandler(this.cbInventory_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(431, 116);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 16);
            this.label8.TabIndex = 127;
            this.label8.Text = "Warehouse";
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(597, 112);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 126;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 277);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 16);
            this.label9.TabIndex = 125;
            this.label9.Text = "Inventory ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 116);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 16);
            this.label7.TabIndex = 124;
            this.label7.Text = "QC Date";
            // 
            // tbBuyingDate
            // 
            this.tbBuyingDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbBuyingDate.Location = new System.Drawing.Point(183, 112);
            this.tbBuyingDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbBuyingDate.Name = "tbBuyingDate";
            this.tbBuyingDate.ReadOnly = true;
            this.tbBuyingDate.Size = new System.Drawing.Size(239, 22);
            this.tbBuyingDate.TabIndex = 123;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 148);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(130, 16);
            this.label6.TabIndex = 122;
            this.label6.Text = "Registration Number";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(431, 181);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 121;
            this.label4.Text = "Vendor Class";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(431, 84);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 120;
            this.label3.Text = "Doc. Status";
            // 
            // tbVendorID
            // 
            this.tbVendorID.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorID.Location = new System.Drawing.Point(183, 177);
            this.tbVendorID.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorID.Name = "tbVendorID";
            this.tbVendorID.ReadOnly = true;
            this.tbVendorID.Size = new System.Drawing.Size(239, 22);
            this.tbVendorID.TabIndex = 119;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 181);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 118;
            this.label5.Text = "Farmer Details";
            // 
            // tbVendorClass
            // 
            this.tbVendorClass.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorClass.Location = new System.Drawing.Point(597, 177);
            this.tbVendorClass.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorClass.Name = "tbVendorClass";
            this.tbVendorClass.ReadOnly = true;
            this.tbVendorClass.Size = new System.Drawing.Size(239, 22);
            this.tbVendorClass.TabIndex = 117;
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tbStatus.Location = new System.Drawing.Point(597, 80);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(239, 22);
            this.tbStatus.TabIndex = 116;
            // 
            // tbVendorDetails
            // 
            this.tbVendorDetails.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorDetails.Location = new System.Drawing.Point(183, 209);
            this.tbVendorDetails.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorDetails.Multiline = true;
            this.tbVendorDetails.Name = "tbVendorDetails";
            this.tbVendorDetails.ReadOnly = true;
            this.tbVendorDetails.Size = new System.Drawing.Size(652, 56);
            this.tbVendorDetails.TabIndex = 115;
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Info;
            this.tbDocNumber.Location = new System.Drawing.Point(183, 80);
            this.tbDocNumber.Margin = new System.Windows.Forms.Padding(4);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(239, 22);
            this.tbDocNumber.TabIndex = 114;
            this.tbDocNumber.Text = "<NEW>";
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 113;
            this.label1.Text = "Document Number";
            // 
            // cbRegistration
            // 
            this.cbRegistration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRegistration.FormattingEnabled = true;
            this.cbRegistration.Location = new System.Drawing.Point(183, 144);
            this.cbRegistration.Margin = new System.Windows.Forms.Padding(4);
            this.cbRegistration.Name = "cbRegistration";
            this.cbRegistration.Size = new System.Drawing.Size(193, 24);
            this.cbRegistration.TabIndex = 133;
            this.cbRegistration.Visible = false;
            this.cbRegistration.SelectedIndexChanged += new System.EventHandler(this.cbRegistration_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(431, 277);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 16);
            this.label2.TabIndex = 135;
            this.label2.Text = "QC Range";
            // 
            // tbSamplingRange
            // 
            this.tbSamplingRange.BackColor = System.Drawing.Color.LightCoral;
            this.tbSamplingRange.Location = new System.Drawing.Point(597, 273);
            this.tbSamplingRange.Margin = new System.Windows.Forms.Padding(4);
            this.tbSamplingRange.Name = "tbSamplingRange";
            this.tbSamplingRange.Size = new System.Drawing.Size(239, 22);
            this.tbSamplingRange.TabIndex = 134;
            this.tbSamplingRange.Text = "0";
            this.tbSamplingRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbSamplingRange.TextChanged += new System.EventHandler(this.tbSamplingRange_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 20);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 43;
            this.label10.Text = "Lot. Range";
            // 
            // tbEntryLot
            // 
            this.tbEntryLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryLot.Location = new System.Drawing.Point(15, 39);
            this.tbEntryLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryLot.Name = "tbEntryLot";
            this.tbEntryLot.ReadOnly = true;
            this.tbEntryLot.Size = new System.Drawing.Size(225, 22);
            this.tbEntryLot.TabIndex = 43;
            // 
            // tbEntryInv
            // 
            this.tbEntryInv.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryInv.Location = new System.Drawing.Point(249, 39);
            this.tbEntryInv.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryInv.Name = "tbEntryInv";
            this.tbEntryInv.ReadOnly = true;
            this.tbEntryInv.Size = new System.Drawing.Size(79, 22);
            this.tbEntryInv.TabIndex = 44;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(253, 20);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 16);
            this.label11.TabIndex = 46;
            this.label11.Text = "Inventory";
            // 
            // tbEntryRemark
            // 
            this.tbEntryRemark.BackColor = System.Drawing.SystemColors.Window;
            this.tbEntryRemark.Location = new System.Drawing.Point(15, 86);
            this.tbEntryRemark.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryRemark.Name = "tbEntryRemark";
            this.tbEntryRemark.Size = new System.Drawing.Size(461, 22);
            this.tbEntryRemark.TabIndex = 71;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(19, 68);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(55, 16);
            this.label24.TabIndex = 72;
            this.label24.Text = "Remark";
            // 
            // btnSaveLot
            // 
            this.btnSaveLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveLot.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaveLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSaveLot.Enabled = false;
            this.btnSaveLot.Image = global::ScaleAddon.Properties.Resources.icons8_save_archive_30;
            this.btnSaveLot.Location = new System.Drawing.Point(740, 53);
            this.btnSaveLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveLot.Name = "btnSaveLot";
            this.btnSaveLot.Size = new System.Drawing.Size(59, 54);
            this.btnSaveLot.TabIndex = 43;
            this.btnSaveLot.UseVisualStyleBackColor = false;
            this.btnSaveLot.Click += new System.EventHandler(this.btnSaveLot_Click);
            // 
            // btnAddEntry
            // 
            this.btnAddEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddEntry.Image = global::ScaleAddon.Properties.Resources.icons8_create_archive_30;
            this.btnAddEntry.Location = new System.Drawing.Point(673, 53);
            this.btnAddEntry.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddEntry.Name = "btnAddEntry";
            this.btnAddEntry.Size = new System.Drawing.Size(59, 54);
            this.btnAddEntry.TabIndex = 81;
            this.btnAddEntry.UseVisualStyleBackColor = true;
            this.btnAddEntry.Click += new System.EventHandler(this.btnAddEntry_Click);
            // 
            // checkReject
            // 
            this.checkReject.AutoSize = true;
            this.checkReject.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkReject.Location = new System.Drawing.Point(485, 89);
            this.checkReject.Margin = new System.Windows.Forms.Padding(4);
            this.checkReject.Name = "checkReject";
            this.checkReject.Size = new System.Drawing.Size(84, 20);
            this.checkReject.TabIndex = 82;
            this.checkReject.Text = "Rejected";
            this.checkReject.UseVisualStyleBackColor = true;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(341, 20);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(75, 16);
            this.label31.TabIndex = 89;
            this.label31.Text = "Lot Sample";
            // 
            // groupEntry
            // 
            this.groupEntry.BackColor = System.Drawing.SystemColors.Control;
            this.groupEntry.Controls.Add(this.cbLotSample);
            this.groupEntry.Controls.Add(this.label31);
            this.groupEntry.Controls.Add(this.checkReject);
            this.groupEntry.Controls.Add(this.btnAddEntry);
            this.groupEntry.Controls.Add(this.btnSaveLot);
            this.groupEntry.Controls.Add(this.label24);
            this.groupEntry.Controls.Add(this.tbEntryRemark);
            this.groupEntry.Controls.Add(this.label11);
            this.groupEntry.Controls.Add(this.tbEntryInv);
            this.groupEntry.Controls.Add(this.tbEntryLot);
            this.groupEntry.Controls.Add(this.label10);
            this.groupEntry.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupEntry.Location = new System.Drawing.Point(20, 295);
            this.groupEntry.Margin = new System.Windows.Forms.Padding(4);
            this.groupEntry.Name = "groupEntry";
            this.groupEntry.Padding = new System.Windows.Forms.Padding(4);
            this.groupEntry.Size = new System.Drawing.Size(815, 123);
            this.groupEntry.TabIndex = 136;
            this.groupEntry.TabStop = false;
            this.groupEntry.Text = "Lot Entry";
            // 
            // cbLotSample
            // 
            this.cbLotSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLotSample.FormattingEnabled = true;
            this.cbLotSample.Location = new System.Drawing.Point(337, 39);
            this.cbLotSample.Margin = new System.Windows.Forms.Padding(4);
            this.cbLotSample.Name = "cbLotSample";
            this.cbLotSample.Size = new System.Drawing.Size(239, 24);
            this.cbLotSample.TabIndex = 138;
            this.cbLotSample.SelectedIndexChanged += new System.EventHandler(this.cbLotSample_SelectedIndexChanged);
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvDetail);
            this.groupDetail.Controls.Add(this.pnlDetailSummary);
            this.groupDetail.Location = new System.Drawing.Point(20, 441);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4);
            this.groupDetail.Size = new System.Drawing.Size(815, 239);
            this.groupDetail.TabIndex = 137;
            this.groupDetail.TabStop = false;
            this.groupDetail.Text = "Document Details";
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.Location = new System.Drawing.Point(4, 19);
            this.dgvDetail.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersWidth = 51;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(807, 177);
            this.dgvDetail.TabIndex = 1;
            this.dgvDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDetail_CellFormatting);
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label38);
            this.pnlDetailSummary.Controls.Add(this.tbDetailLot);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 196);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(807, 39);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(592, 11);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(98, 16);
            this.label38.TabIndex = 114;
            this.label38.Text = "Total Sampling";
            // 
            // tbDetailLot
            // 
            this.tbDetailLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailLot.Location = new System.Drawing.Point(703, 7);
            this.tbDetailLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailLot.Name = "tbDetailLot";
            this.tbDetailLot.ReadOnly = true;
            this.tbDetailLot.Size = new System.Drawing.Size(99, 22);
            this.tbDetailLot.TabIndex = 113;
            this.tbDetailLot.Text = "0";
            this.tbDetailLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BuyingQCForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 690);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.groupEntry);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbSamplingRange);
            this.Controls.Add(this.btnToogle);
            this.Controls.Add(this.tbRegistration);
            this.Controls.Add(this.cbRegistration);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.tbTotalLot);
            this.Controls.Add(this.cbInventory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbBuyingDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbVendorID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbVendorClass);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.tbVendorDetails);
            this.Controls.Add(this.tbDocNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuyingQCForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BuyingQCForm";
            this.Load += new System.EventHandler(this.BuyingQCForm_Load);
            this.panel1.ResumeLayout(false);
            this.groupEntry.ResumeLayout(false);
            this.groupEntry.PerformLayout();
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnPrintDoc;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnToogle;
        private System.Windows.Forms.TextBox tbRegistration;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox tbTotalLot;
        private System.Windows.Forms.ComboBox cbInventory;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbBuyingDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVendorID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbVendorClass;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TextBox tbVendorDetails;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRegistration;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSamplingRange;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbEntryLot;
        private System.Windows.Forms.TextBox tbEntryInv;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbEntryRemark;
        private System.Windows.Forms.Label label24;
        internal System.Windows.Forms.Button btnSaveLot;
        internal System.Windows.Forms.Button btnAddEntry;
        private System.Windows.Forms.CheckBox checkReject;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.GroupBox groupEntry;
        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.ComboBox cbLotSample;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox tbDetailLot;
    }
}