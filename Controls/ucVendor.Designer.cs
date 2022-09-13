namespace ScaleAddon.Controls
{
    partial class ucVendor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvVendor = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTruncate = new System.Windows.Forms.Button();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nonpwp = new System.Windows.Forms.TextBox();
            this.tasdas = new System.Windows.Forms.Label();
            this.tabVendorInfo = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dgvContract = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvPO = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.tbDetailOrderQty = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailOpenQty = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvPrepayment = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.tbPrepayment = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tbUnappliedBalance = new System.Windows.Forms.TextBox();
            this.tbVendorClass = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tbPostalCode = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbState = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCountry = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbCity = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbAddress2 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbAddress1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPhone2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPhone1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDisplayName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVendorName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendor)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabVendorInfo.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContract)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPO)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrepayment)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvVendor
            // 
            this.dgvVendor.AllowUserToAddRows = false;
            this.dgvVendor.AllowUserToDeleteRows = false;
            this.dgvVendor.AllowUserToOrderColumns = true;
            this.dgvVendor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVendor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVendor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVendor.Location = new System.Drawing.Point(4, 55);
            this.dgvVendor.Margin = new System.Windows.Forms.Padding(4);
            this.dgvVendor.MultiSelect = false;
            this.dgvVendor.Name = "dgvVendor";
            this.dgvVendor.ReadOnly = true;
            this.dgvVendor.RowHeadersWidth = 51;
            this.dgvVendor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVendor.Size = new System.Drawing.Size(613, 679);
            this.dgvVendor.TabIndex = 5;
            this.dgvVendor.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVendor_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTruncate);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(4, 19);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(613, 36);
            this.panel1.TabIndex = 4;
            // 
            // btnTruncate
            // 
            this.btnTruncate.Image = global::ScaleAddon.Properties.Resources.icons8_erase_16;
            this.btnTruncate.Location = new System.Drawing.Point(4, 4);
            this.btnTruncate.Margin = new System.Windows.Forms.Padding(4);
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.Size = new System.Drawing.Size(107, 28);
            this.btnTruncate.TabIndex = 10;
            this.btnTruncate.Text = "Clear";
            this.btnTruncate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTruncate.UseVisualStyleBackColor = true;
            this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
            // 
            // btnAcumatica
            // 
            this.btnAcumatica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcumatica.Image = global::ScaleAddon.Properties.Resources.icons8_cloud_sync_16;
            this.btnAcumatica.Location = new System.Drawing.Point(503, 4);
            this.btnAcumatica.Margin = new System.Windows.Forms.Padding(4);
            this.btnAcumatica.Name = "btnAcumatica";
            this.btnAcumatica.Size = new System.Drawing.Size(107, 28);
            this.btnAcumatica.TabIndex = 9;
            this.btnAcumatica.Text = "Sync";
            this.btnAcumatica.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAcumatica.UseVisualStyleBackColor = true;
            this.btnAcumatica.Click += new System.EventHandler(this.btnAcumatica_Click);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(164, 10);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(36, 16);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "Filter";
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(220, 6);
            this.tbFilter.Margin = new System.Windows.Forms.Padding(4);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(159, 22);
            this.tbFilter.TabIndex = 7;
            this.tbFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter_KeyDown);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_16;
            this.btnRefresh.Location = new System.Drawing.Point(388, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(107, 28);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Search";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.groupBox1);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(621, 738);
            this.pnlLeft.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvVendor);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(621, 738);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vendor List";
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.groupBox2);
            this.pnlInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlInfo.Location = new System.Drawing.Point(621, 0);
            this.pnlInfo.Margin = new System.Windows.Forms.Padding(4);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(579, 738);
            this.pnlInfo.TabIndex = 7;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nonpwp);
            this.groupBox2.Controls.Add(this.tasdas);
            this.groupBox2.Controls.Add(this.tabVendorInfo);
            this.groupBox2.Controls.Add(this.tbVendorClass);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.tbPostalCode);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.tbState);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.tbCountry);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.tbCity);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.tbAddress2);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.tbAddress1);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbPhone2);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbPhone1);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbDisplayName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tbStatus);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.tbVendorName);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(579, 738);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vendor Details";
            // 
            // nonpwp
            // 
            this.nonpwp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nonpwp.Location = new System.Drawing.Point(119, 410);
            this.nonpwp.Margin = new System.Windows.Forms.Padding(4);
            this.nonpwp.Name = "nonpwp";
            this.nonpwp.ReadOnly = true;
            this.nonpwp.Size = new System.Drawing.Size(451, 22);
            this.nonpwp.TabIndex = 27;
            // 
            // tasdas
            // 
            this.tasdas.AutoSize = true;
            this.tasdas.Location = new System.Drawing.Point(9, 413);
            this.tasdas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tasdas.Name = "tasdas";
            this.tasdas.Size = new System.Drawing.Size(72, 16);
            this.tasdas.TabIndex = 26;
            this.tasdas.Text = "No. NPWP";
            // 
            // tabVendorInfo
            // 
            this.tabVendorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabVendorInfo.Controls.Add(this.tabPage1);
            this.tabVendorInfo.Controls.Add(this.tabPage2);
            this.tabVendorInfo.Controls.Add(this.tabPage3);
            this.tabVendorInfo.Location = new System.Drawing.Point(12, 444);
            this.tabVendorInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tabVendorInfo.Name = "tabVendorInfo";
            this.tabVendorInfo.SelectedIndex = 0;
            this.tabVendorInfo.Size = new System.Drawing.Size(559, 290);
            this.tabVendorInfo.TabIndex = 25;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvContract);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(551, 261);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Contract";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvContract
            // 
            this.dgvContract.AllowUserToAddRows = false;
            this.dgvContract.AllowUserToDeleteRows = false;
            this.dgvContract.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContract.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvContract.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvContract.Location = new System.Drawing.Point(4, 4);
            this.dgvContract.Margin = new System.Windows.Forms.Padding(4);
            this.dgvContract.MultiSelect = false;
            this.dgvContract.Name = "dgvContract";
            this.dgvContract.RowHeadersWidth = 51;
            this.dgvContract.Size = new System.Drawing.Size(543, 253);
            this.dgvContract.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvPO);
            this.tabPage2.Controls.Add(this.pnlDetailSummary);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(551, 261);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Purchase Order";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvPO
            // 
            this.dgvPO.AllowUserToAddRows = false;
            this.dgvPO.AllowUserToDeleteRows = false;
            this.dgvPO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPO.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPO.Location = new System.Drawing.Point(4, 4);
            this.dgvPO.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPO.MultiSelect = false;
            this.dgvPO.Name = "dgvPO";
            this.dgvPO.RowHeadersWidth = 51;
            this.dgvPO.Size = new System.Drawing.Size(543, 214);
            this.dgvPO.TabIndex = 1;
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label14);
            this.pnlDetailSummary.Controls.Add(this.tbDetailOrderQty);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailOpenQty);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 218);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(543, 39);
            this.pnlDetailSummary.TabIndex = 10;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(162, 7);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 16);
            this.label14.TabIndex = 91;
            this.label14.Text = "Order Qty";
            // 
            // tbDetailOrderQty
            // 
            this.tbDetailOrderQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailOrderQty.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailOrderQty.Location = new System.Drawing.Point(239, 4);
            this.tbDetailOrderQty.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailOrderQty.Name = "tbDetailOrderQty";
            this.tbDetailOrderQty.ReadOnly = true;
            this.tbDetailOrderQty.Size = new System.Drawing.Size(99, 22);
            this.tbDetailOrderQty.TabIndex = 90;
            this.tbDetailOrderQty.Text = "0";
            this.tbDetailOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(347, 7);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(77, 16);
            this.label29.TabIndex = 87;
            this.label29.Text = "Receipt Qty";
            // 
            // tbDetailOpenQty
            // 
            this.tbDetailOpenQty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailOpenQty.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailOpenQty.Location = new System.Drawing.Point(439, 4);
            this.tbDetailOpenQty.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailOpenQty.Name = "tbDetailOpenQty";
            this.tbDetailOpenQty.ReadOnly = true;
            this.tbDetailOpenQty.Size = new System.Drawing.Size(99, 22);
            this.tbDetailOpenQty.TabIndex = 83;
            this.tbDetailOpenQty.Text = "0";
            this.tbDetailOpenQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvPrepayment);
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(551, 261);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Prepayment";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvPrepayment
            // 
            this.dgvPrepayment.AllowUserToAddRows = false;
            this.dgvPrepayment.AllowUserToDeleteRows = false;
            this.dgvPrepayment.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dgvPrepayment.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrepayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPrepayment.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPrepayment.Location = new System.Drawing.Point(4, 4);
            this.dgvPrepayment.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPrepayment.MultiSelect = false;
            this.dgvPrepayment.Name = "dgvPrepayment";
            this.dgvPrepayment.RowHeadersWidth = 51;
            this.dgvPrepayment.Size = new System.Drawing.Size(543, 214);
            this.dgvPrepayment.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label15);
            this.panel2.Controls.Add(this.tbPrepayment);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.tbUnappliedBalance);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(4, 218);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(543, 39);
            this.panel2.TabIndex = 11;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(62, 7);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 16);
            this.label15.TabIndex = 91;
            this.label15.Text = "Prepayment";
            // 
            // tbPrepayment
            // 
            this.tbPrepayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrepayment.BackColor = System.Drawing.SystemColors.Info;
            this.tbPrepayment.Location = new System.Drawing.Point(154, 4);
            this.tbPrepayment.Margin = new System.Windows.Forms.Padding(4);
            this.tbPrepayment.Name = "tbPrepayment";
            this.tbPrepayment.ReadOnly = true;
            this.tbPrepayment.Size = new System.Drawing.Size(119, 22);
            this.tbPrepayment.TabIndex = 90;
            this.tbPrepayment.Text = "0";
            this.tbPrepayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(282, 7);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(123, 16);
            this.label16.TabIndex = 87;
            this.label16.Text = "Unapplied Balance";
            // 
            // tbUnappliedBalance
            // 
            this.tbUnappliedBalance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUnappliedBalance.BackColor = System.Drawing.SystemColors.Info;
            this.tbUnappliedBalance.Location = new System.Drawing.Point(419, 4);
            this.tbUnappliedBalance.Margin = new System.Windows.Forms.Padding(4);
            this.tbUnappliedBalance.Name = "tbUnappliedBalance";
            this.tbUnappliedBalance.ReadOnly = true;
            this.tbUnappliedBalance.Size = new System.Drawing.Size(119, 22);
            this.tbUnappliedBalance.TabIndex = 83;
            this.tbUnappliedBalance.Text = "0";
            this.tbUnappliedBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbVendorClass
            // 
            this.tbVendorClass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVendorClass.Location = new System.Drawing.Point(119, 378);
            this.tbVendorClass.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorClass.Name = "tbVendorClass";
            this.tbVendorClass.ReadOnly = true;
            this.tbVendorClass.Size = new System.Drawing.Size(451, 22);
            this.tbVendorClass.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 382);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(88, 16);
            this.label13.TabIndex = 22;
            this.label13.Text = "Vendor Class";
            // 
            // tbPostalCode
            // 
            this.tbPostalCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPostalCode.Location = new System.Drawing.Point(119, 346);
            this.tbPostalCode.Margin = new System.Windows.Forms.Padding(4);
            this.tbPostalCode.Name = "tbPostalCode";
            this.tbPostalCode.ReadOnly = true;
            this.tbPostalCode.Size = new System.Drawing.Size(451, 22);
            this.tbPostalCode.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 350);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 16);
            this.label12.TabIndex = 20;
            this.label12.Text = "Postal Code";
            // 
            // tbState
            // 
            this.tbState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbState.Location = new System.Drawing.Point(119, 314);
            this.tbState.Margin = new System.Windows.Forms.Padding(4);
            this.tbState.Name = "tbState";
            this.tbState.ReadOnly = true;
            this.tbState.Size = new System.Drawing.Size(451, 22);
            this.tbState.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 318);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 16);
            this.label11.TabIndex = 18;
            this.label11.Text = "State";
            // 
            // tbCountry
            // 
            this.tbCountry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCountry.Location = new System.Drawing.Point(119, 282);
            this.tbCountry.Margin = new System.Windows.Forms.Padding(4);
            this.tbCountry.Name = "tbCountry";
            this.tbCountry.ReadOnly = true;
            this.tbCountry.Size = new System.Drawing.Size(451, 22);
            this.tbCountry.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 286);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 16);
            this.label10.TabIndex = 16;
            this.label10.Text = "Country";
            // 
            // tbCity
            // 
            this.tbCity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCity.Location = new System.Drawing.Point(119, 250);
            this.tbCity.Margin = new System.Windows.Forms.Padding(4);
            this.tbCity.Name = "tbCity";
            this.tbCity.ReadOnly = true;
            this.tbCity.Size = new System.Drawing.Size(451, 22);
            this.tbCity.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 254);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 16);
            this.label9.TabIndex = 14;
            this.label9.Text = "City";
            // 
            // tbAddress2
            // 
            this.tbAddress2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddress2.Location = new System.Drawing.Point(119, 218);
            this.tbAddress2.Margin = new System.Windows.Forms.Padding(4);
            this.tbAddress2.Name = "tbAddress2";
            this.tbAddress2.ReadOnly = true;
            this.tbAddress2.Size = new System.Drawing.Size(451, 22);
            this.tbAddress2.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 222);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Address Line 2";
            // 
            // tbAddress1
            // 
            this.tbAddress1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAddress1.Location = new System.Drawing.Point(119, 186);
            this.tbAddress1.Margin = new System.Windows.Forms.Padding(4);
            this.tbAddress1.Name = "tbAddress1";
            this.tbAddress1.ReadOnly = true;
            this.tbAddress1.Size = new System.Drawing.Size(451, 22);
            this.tbAddress1.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 190);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 16);
            this.label7.TabIndex = 10;
            this.label7.Text = "Address Line 1";
            // 
            // tbPhone2
            // 
            this.tbPhone2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPhone2.Location = new System.Drawing.Point(119, 154);
            this.tbPhone2.Margin = new System.Windows.Forms.Padding(4);
            this.tbPhone2.Name = "tbPhone2";
            this.tbPhone2.ReadOnly = true;
            this.tbPhone2.Size = new System.Drawing.Size(451, 22);
            this.tbPhone2.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 158);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Phone 2";
            // 
            // tbPhone1
            // 
            this.tbPhone1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPhone1.Location = new System.Drawing.Point(119, 122);
            this.tbPhone1.Margin = new System.Windows.Forms.Padding(4);
            this.tbPhone1.Name = "tbPhone1";
            this.tbPhone1.ReadOnly = true;
            this.tbPhone1.Size = new System.Drawing.Size(451, 22);
            this.tbPhone1.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 126);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Phone 1";
            // 
            // tbDisplayName
            // 
            this.tbDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDisplayName.Location = new System.Drawing.Point(119, 90);
            this.tbDisplayName.Margin = new System.Windows.Forms.Padding(4);
            this.tbDisplayName.Name = "tbDisplayName";
            this.tbDisplayName.ReadOnly = true;
            this.tbDisplayName.Size = new System.Drawing.Size(451, 22);
            this.tbDisplayName.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 94);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Display name";
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.Location = new System.Drawing.Point(119, 58);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(451, 22);
            this.tbStatus.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 62);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Status";
            // 
            // tbVendorName
            // 
            this.tbVendorName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbVendorName.Location = new System.Drawing.Point(119, 26);
            this.tbVendorName.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorName.Name = "tbVendorName";
            this.tbVendorName.ReadOnly = true;
            this.tbVendorName.Size = new System.Drawing.Size(451, 22);
            this.tbVendorName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Vendor Name";
            // 
            // ucVendor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlInfo);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucVendor";
            this.Size = new System.Drawing.Size(1200, 738);
            this.Load += new System.EventHandler(this.ucVendor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVendor)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlLeft.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabVendorInfo.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContract)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPO)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrepayment)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvVendor;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbVendorName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPhone1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDisplayName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVendorClass;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbPostalCode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbState;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbCountry;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbAddress2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbAddress1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPhone2;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button btnAcumatica;
        private System.Windows.Forms.DataGridView dgvContract;
        private System.Windows.Forms.TabControl tabVendorInfo;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvPO;
        private System.Windows.Forms.DataGridView dgvPrepayment;
        internal System.Windows.Forms.Button btnTruncate;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbDetailOrderQty;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailOpenQty;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbPrepayment;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbUnappliedBalance;
        private System.Windows.Forms.TextBox nonpwp;
        private System.Windows.Forms.Label tasdas;
        private System.Windows.Forms.TabPage tabPage3;
    }
}
