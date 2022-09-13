
namespace ScaleAddon.Forms
{
    partial class WeightAdjustment
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
            this.tbAcumaticaIssue = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.cbLot = new System.Windows.Forms.ComboBox();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrintDoc = new System.Windows.Forms.Button();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbAcumaticaReceipt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupDetailInput = new System.Windows.Forms.GroupBox();
            this.dgvEntry = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.btnRemoveEntry = new System.Windows.Forms.Button();
            this.btnSaveEntry = new System.Windows.Forms.Button();
            this.tbEntryWReceiveNew = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEntryWNettoNew = new System.Windows.Forms.TextBox();
            this.tbEntryWTareNew = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tbEntryWTare = new System.Windows.Forms.TextBox();
            this.tbEntryWNetto = new System.Windows.Forms.TextBox();
            this.tbEntryWReceive = new System.Windows.Forms.TextBox();
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label38 = new System.Windows.Forms.Label();
            this.btnPrintLot = new System.Windows.Forms.Button();
            this.tbDetailLot = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDetailWTare = new System.Windows.Forms.TextBox();
            this.tbDetailWNetto = new System.Windows.Forms.TextBox();
            this.tbDetailWReceive = new System.Windows.Forms.TextBox();
            this.checkHold = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbScale = new System.Windows.Forms.TextBox();
            this.btnScaleOverride = new System.Windows.Forms.Button();
            this.btnScale = new System.Windows.Forms.Button();
            this.btnScaleComm = new System.Windows.Forms.Button();
            this.tbLot = new System.Windows.Forms.TextBox();
            this.btnToogle = new System.Windows.Forms.Button();
            this.remark = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupDetailInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntry)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAcumaticaIssue
            // 
            this.tbAcumaticaIssue.BackColor = System.Drawing.SystemColors.Info;
            this.tbAcumaticaIssue.Location = new System.Drawing.Point(189, 145);
            this.tbAcumaticaIssue.Margin = new System.Windows.Forms.Padding(4);
            this.tbAcumaticaIssue.Name = "tbAcumaticaIssue";
            this.tbAcumaticaIssue.ReadOnly = true;
            this.tbAcumaticaIssue.Size = new System.Drawing.Size(239, 22);
            this.tbAcumaticaIssue.TabIndex = 100;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(24, 149);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(110, 17);
            this.label30.TabIndex = 99;
            this.label30.Text = "Acumatica Issue";
            // 
            // cbLot
            // 
            this.cbLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLot.FormattingEnabled = true;
            this.cbLot.Location = new System.Drawing.Point(189, 177);
            this.cbLot.Margin = new System.Windows.Forms.Padding(4);
            this.cbLot.Name = "cbLot";
            this.cbLot.Size = new System.Drawing.Size(193, 24);
            this.cbLot.TabIndex = 98;
            this.cbLot.Visible = false;
            this.cbLot.SelectedIndexChanged += new System.EventHandler(this.cbLot_SelectedIndexChanged);
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(603, 149);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 97;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 181);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 17);
            this.label9.TabIndex = 96;
            this.label9.Text = "Tobacco Lot";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 117);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 17);
            this.label7.TabIndex = 95;
            this.label7.Text = "Document Date";
            // 
            // tbDate
            // 
            this.tbDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbDate.Location = new System.Drawing.Point(189, 113);
            this.tbDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbDate.Name = "tbDate";
            this.tbDate.ReadOnly = true;
            this.tbDate.Size = new System.Drawing.Size(239, 22);
            this.tbDate.TabIndex = 94;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 83);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 93;
            this.label3.Text = "Doc. Status";
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tbStatus.Location = new System.Drawing.Point(603, 81);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(167, 22);
            this.tbStatus.TabIndex = 92;
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Info;
            this.tbDocNumber.Location = new System.Drawing.Point(189, 81);
            this.tbDocNumber.Margin = new System.Windows.Forms.Padding(4);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(239, 22);
            this.tbDocNumber.TabIndex = 91;
            this.tbDocNumber.Text = "<NEW>";
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 90;
            this.label1.Text = "Document Number";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPrintDoc);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1232, 73);
            this.panel1.TabIndex = 89;
            // 
            // btnPrintDoc
            // 
            this.btnPrintDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintDoc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintDoc.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrintDoc.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintDoc.Location = new System.Drawing.Point(221, 4);
            this.btnPrintDoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintDoc.Name = "btnPrintDoc";
            this.btnPrintDoc.Size = new System.Drawing.Size(87, 65);
            this.btnPrintDoc.TabIndex = 110;
            this.btnPrintDoc.Text = "Print Doc";
            this.btnPrintDoc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrintDoc.UseVisualStyleBackColor = true;
            this.btnPrintDoc.Click += new System.EventHandler(this.btnPrintDoc_Click);
            // 
            // btnAcumatica
            // 
            this.btnAcumatica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcumatica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAcumatica.Enabled = false;
            this.btnAcumatica.Image = global::ScaleAddon.Properties.Resources.icons8_upload_to_the_cloud_30;
            this.btnAcumatica.Location = new System.Drawing.Point(1169, 4);
            this.btnAcumatica.Margin = new System.Windows.Forms.Padding(4);
            this.btnAcumatica.Name = "btnAcumatica";
            this.btnAcumatica.Size = new System.Drawing.Size(59, 54);
            this.btnAcumatica.TabIndex = 15;
            this.btnAcumatica.UseVisualStyleBackColor = true;
            this.btnAcumatica.Click += new System.EventHandler(this.btnAcumatica_Click);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 154);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 102;
            this.label8.Text = "Warehouse";
            // 
            // tbAcumaticaReceipt
            // 
            this.tbAcumaticaReceipt.BackColor = System.Drawing.SystemColors.Info;
            this.tbAcumaticaReceipt.Location = new System.Drawing.Point(603, 114);
            this.tbAcumaticaReceipt.Margin = new System.Windows.Forms.Padding(4);
            this.tbAcumaticaReceipt.Name = "tbAcumaticaReceipt";
            this.tbAcumaticaReceipt.ReadOnly = true;
            this.tbAcumaticaReceipt.Size = new System.Drawing.Size(239, 22);
            this.tbAcumaticaReceipt.TabIndex = 105;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(448, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 104;
            this.label2.Text = "Acumatica Receipt";
            // 
            // groupDetailInput
            // 
            this.groupDetailInput.Controls.Add(this.dgvEntry);
            this.groupDetailInput.Controls.Add(this.pnlDetailSummary);
            this.groupDetailInput.Location = new System.Drawing.Point(20, 261);
            this.groupDetailInput.Margin = new System.Windows.Forms.Padding(4);
            this.groupDetailInput.Name = "groupDetailInput";
            this.groupDetailInput.Padding = new System.Windows.Forms.Padding(4);
            this.groupDetailInput.Size = new System.Drawing.Size(1193, 223);
            this.groupDetailInput.TabIndex = 1;
            this.groupDetailInput.TabStop = false;
            this.groupDetailInput.Text = "Entry Details";
            // 
            // dgvEntry
            // 
            this.dgvEntry.AllowUserToAddRows = false;
            this.dgvEntry.AllowUserToDeleteRows = false;
            this.dgvEntry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEntry.Location = new System.Drawing.Point(4, 19);
            this.dgvEntry.Margin = new System.Windows.Forms.Padding(4);
            this.dgvEntry.MultiSelect = false;
            this.dgvEntry.Name = "dgvEntry";
            this.dgvEntry.ReadOnly = true;
            this.dgvEntry.RowHeadersWidth = 51;
            this.dgvEntry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEntry.Size = new System.Drawing.Size(1185, 167);
            this.dgvEntry.TabIndex = 1;
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.btnRemoveEntry);
            this.pnlDetailSummary.Controls.Add(this.btnSaveEntry);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWReceiveNew);
            this.pnlDetailSummary.Controls.Add(this.label4);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWNettoNew);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWTareNew);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWTare);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWNetto);
            this.pnlDetailSummary.Controls.Add(this.tbEntryWReceive);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 186);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(1185, 33);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // btnRemoveEntry
            // 
            this.btnRemoveEntry.Image = global::ScaleAddon.Properties.Resources.icons8_delete_16;
            this.btnRemoveEntry.Location = new System.Drawing.Point(427, 2);
            this.btnRemoveEntry.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveEntry.Name = "btnRemoveEntry";
            this.btnRemoveEntry.Size = new System.Drawing.Size(100, 28);
            this.btnRemoveEntry.TabIndex = 93;
            this.btnRemoveEntry.Text = "Remove";
            this.btnRemoveEntry.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveEntry.UseVisualStyleBackColor = true;
            this.btnRemoveEntry.Click += new System.EventHandler(this.btnRemoveEntry_Click);
            // 
            // btnSaveEntry
            // 
            this.btnSaveEntry.Image = global::ScaleAddon.Properties.Resources.icons8_save_16;
            this.btnSaveEntry.Location = new System.Drawing.Point(1081, 5);
            this.btnSaveEntry.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveEntry.Name = "btnSaveEntry";
            this.btnSaveEntry.Size = new System.Drawing.Size(100, 28);
            this.btnSaveEntry.TabIndex = 1;
            this.btnSaveEntry.Text = "Save";
            this.btnSaveEntry.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveEntry.UseVisualStyleBackColor = true;
            this.btnSaveEntry.Click += new System.EventHandler(this.btnSaveEntry_Click);
            // 
            // tbEntryWReceiveNew
            // 
            this.tbEntryWReceiveNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWReceiveNew.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWReceiveNew.Location = new System.Drawing.Point(757, 7);
            this.tbEntryWReceiveNew.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWReceiveNew.Name = "tbEntryWReceiveNew";
            this.tbEntryWReceiveNew.ReadOnly = true;
            this.tbEntryWReceiveNew.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWReceiveNew.TabIndex = 91;
            this.tbEntryWReceiveNew.Text = "0";
            this.tbEntryWReceiveNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEntryWReceiveNew.TextChanged += new System.EventHandler(this.tbEntryWReceiveNew_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(651, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 17);
            this.label4.TabIndex = 90;
            this.label4.Text = "Total New KG";
            // 
            // tbEntryWNettoNew
            // 
            this.tbEntryWNettoNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWNettoNew.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWNettoNew.Location = new System.Drawing.Point(973, 7);
            this.tbEntryWNettoNew.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWNettoNew.Name = "tbEntryWNettoNew";
            this.tbEntryWNettoNew.ReadOnly = true;
            this.tbEntryWNettoNew.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWNettoNew.TabIndex = 89;
            this.tbEntryWNettoNew.Text = "0";
            this.tbEntryWNettoNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbEntryWTareNew
            // 
            this.tbEntryWTareNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWTareNew.BackColor = System.Drawing.SystemColors.Window;
            this.tbEntryWTareNew.Location = new System.Drawing.Point(865, 7);
            this.tbEntryWTareNew.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWTareNew.Name = "tbEntryWTareNew";
            this.tbEntryWTareNew.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWTareNew.TabIndex = 0;
            this.tbEntryWTareNew.Text = "0";
            this.tbEntryWTareNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEntryWTareNew.TextChanged += new System.EventHandler(this.tbEntryWTareNew_TextChanged);
            this.tbEntryWTareNew.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEntryWTareNew_KeyPress);
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(4, 11);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(90, 17);
            this.label29.TabIndex = 87;
            this.label29.Text = "Total Old KG";
            // 
            // tbEntryWTare
            // 
            this.tbEntryWTare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWTare.Location = new System.Drawing.Point(211, 5);
            this.tbEntryWTare.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWTare.Name = "tbEntryWTare";
            this.tbEntryWTare.ReadOnly = true;
            this.tbEntryWTare.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWTare.TabIndex = 85;
            this.tbEntryWTare.Text = "0";
            this.tbEntryWTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbEntryWNetto
            // 
            this.tbEntryWNetto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWNetto.Location = new System.Drawing.Point(319, 5);
            this.tbEntryWNetto.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWNetto.Name = "tbEntryWNetto";
            this.tbEntryWNetto.ReadOnly = true;
            this.tbEntryWNetto.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWNetto.TabIndex = 86;
            this.tbEntryWNetto.Text = "0";
            this.tbEntryWNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbEntryWReceive
            // 
            this.tbEntryWReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEntryWReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWReceive.Location = new System.Drawing.Point(103, 5);
            this.tbEntryWReceive.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWReceive.Name = "tbEntryWReceive";
            this.tbEntryWReceive.ReadOnly = true;
            this.tbEntryWReceive.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWReceive.TabIndex = 84;
            this.tbEntryWReceive.Text = "0";
            this.tbEntryWReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvDetail);
            this.groupDetail.Controls.Add(this.panel2);
            this.groupDetail.Location = new System.Drawing.Point(20, 487);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4);
            this.groupDetail.Size = new System.Drawing.Size(1193, 227);
            this.groupDetail.TabIndex = 107;
            this.groupDetail.TabStop = false;
            this.groupDetail.Text = "Document Details";
            // 
            // dgvDetail
            // 
            this.dgvDetail.AllowUserToAddRows = false;
            this.dgvDetail.AllowUserToDeleteRows = false;
            this.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetail.Location = new System.Drawing.Point(4, 19);
            this.dgvDetail.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersWidth = 51;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(1185, 165);
            this.dgvDetail.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label38);
            this.panel2.Controls.Add(this.btnPrintLot);
            this.panel2.Controls.Add(this.tbDetailLot);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.tbDetailWTare);
            this.panel2.Controls.Add(this.tbDetailWNetto);
            this.panel2.Controls.Add(this.tbDetailWReceive);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(4, 184);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1185, 39);
            this.panel2.TabIndex = 2;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(611, 11);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(64, 17);
            this.label38.TabIndex = 116;
            this.label38.Text = "Total Lot";
            // 
            // btnPrintLot
            // 
            this.btnPrintLot.Image = global::ScaleAddon.Properties.Resources.icons8_print_16;
            this.btnPrintLot.Location = new System.Drawing.Point(8, 5);
            this.btnPrintLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintLot.Name = "btnPrintLot";
            this.btnPrintLot.Size = new System.Drawing.Size(100, 28);
            this.btnPrintLot.TabIndex = 94;
            this.btnPrintLot.Text = "Print";
            this.btnPrintLot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrintLot.UseVisualStyleBackColor = true;
            this.btnPrintLot.Click += new System.EventHandler(this.btnPrintLot_Click);
            // 
            // tbDetailLot
            // 
            this.tbDetailLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailLot.Location = new System.Drawing.Point(684, 7);
            this.tbDetailLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailLot.Name = "tbDetailLot";
            this.tbDetailLot.ReadOnly = true;
            this.tbDetailLot.Size = new System.Drawing.Size(99, 22);
            this.tbDetailLot.TabIndex = 115;
            this.tbDetailLot.Text = "0";
            this.tbDetailLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(792, 11);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 87;
            this.label5.Text = "Total KG";
            // 
            // tbDetailWTare
            // 
            this.tbDetailWTare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWTare.Location = new System.Drawing.Point(973, 7);
            this.tbDetailWTare.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWTare.Name = "tbDetailWTare";
            this.tbDetailWTare.ReadOnly = true;
            this.tbDetailWTare.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWTare.TabIndex = 85;
            this.tbDetailWTare.Text = "0";
            this.tbDetailWTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWNetto
            // 
            this.tbDetailWNetto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWNetto.Location = new System.Drawing.Point(1081, 7);
            this.tbDetailWNetto.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWNetto.Name = "tbDetailWNetto";
            this.tbDetailWNetto.ReadOnly = true;
            this.tbDetailWNetto.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWNetto.TabIndex = 86;
            this.tbDetailWNetto.Text = "0";
            this.tbDetailWNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWReceive
            // 
            this.tbDetailWReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWReceive.Location = new System.Drawing.Point(865, 7);
            this.tbDetailWReceive.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWReceive.Name = "tbDetailWReceive";
            this.tbDetailWReceive.ReadOnly = true;
            this.tbDetailWReceive.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWReceive.TabIndex = 83;
            this.tbDetailWReceive.Text = "0";
            this.tbDetailWReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkHold
            // 
            this.checkHold.AutoSize = true;
            this.checkHold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkHold.Location = new System.Drawing.Point(779, 82);
            this.checkHold.Margin = new System.Windows.Forms.Padding(4);
            this.checkHold.Name = "checkHold";
            this.checkHold.Size = new System.Drawing.Size(59, 21);
            this.checkHold.TabIndex = 108;
            this.checkHold.Text = "Hold";
            this.checkHold.UseVisualStyleBackColor = true;
            this.checkHold.CheckedChanged += new System.EventHandler(this.checkHold_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbScale);
            this.groupBox1.Controls.Add(this.btnScaleOverride);
            this.groupBox1.Controls.Add(this.btnScale);
            this.groupBox1.Controls.Add(this.btnScaleComm);
            this.groupBox1.Location = new System.Drawing.Point(911, 81);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(311, 178);
            this.groupBox1.TabIndex = 109;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Weighing Scale";
            // 
            // tbScale
            // 
            this.tbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbScale.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.tbScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbScale.Location = new System.Drawing.Point(9, 23);
            this.tbScale.Margin = new System.Windows.Forms.Padding(4);
            this.tbScale.Name = "tbScale";
            this.tbScale.ReadOnly = true;
            this.tbScale.Size = new System.Drawing.Size(292, 87);
            this.tbScale.TabIndex = 30;
            this.tbScale.Text = "0.00";
            this.tbScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnScaleOverride
            // 
            this.btnScaleOverride.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScaleOverride.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnScaleOverride.Image = global::ScaleAddon.Properties.Resources.icons8_scales_key_30;
            this.btnScaleOverride.Location = new System.Drawing.Point(128, 118);
            this.btnScaleOverride.Margin = new System.Windows.Forms.Padding(4);
            this.btnScaleOverride.Name = "btnScaleOverride";
            this.btnScaleOverride.Size = new System.Drawing.Size(59, 54);
            this.btnScaleOverride.TabIndex = 95;
            this.btnScaleOverride.UseVisualStyleBackColor = true;
            this.btnScaleOverride.Click += new System.EventHandler(this.btnScaleOverride_Click);
            // 
            // btnScale
            // 
            this.btnScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScale.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnScale.Image = global::ScaleAddon.Properties.Resources.icons8_scales_add_30;
            this.btnScale.Location = new System.Drawing.Point(244, 118);
            this.btnScale.Margin = new System.Windows.Forms.Padding(4);
            this.btnScale.Name = "btnScale";
            this.btnScale.Size = new System.Drawing.Size(59, 54);
            this.btnScale.TabIndex = 16;
            this.btnScale.UseVisualStyleBackColor = true;
            this.btnScale.Click += new System.EventHandler(this.btnScale_Click);
            // 
            // btnScaleComm
            // 
            this.btnScaleComm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScaleComm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnScaleComm.Image = global::ScaleAddon.Properties.Resources.icons8_scales_30;
            this.btnScaleComm.Location = new System.Drawing.Point(9, 118);
            this.btnScaleComm.Margin = new System.Windows.Forms.Padding(4);
            this.btnScaleComm.Name = "btnScaleComm";
            this.btnScaleComm.Size = new System.Drawing.Size(59, 54);
            this.btnScaleComm.TabIndex = 94;
            this.btnScaleComm.UseVisualStyleBackColor = true;
            this.btnScaleComm.Click += new System.EventHandler(this.btnScaleComm_Click);
            // 
            // tbLot
            // 
            this.tbLot.BackColor = System.Drawing.SystemColors.Window;
            this.tbLot.Location = new System.Drawing.Point(189, 177);
            this.tbLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbLot.Name = "tbLot";
            this.tbLot.Size = new System.Drawing.Size(193, 22);
            this.tbLot.TabIndex = 0;
            this.tbLot.TextChanged += new System.EventHandler(this.tbLot_TextChanged);
            this.tbLot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLot_KeyPress);
            // 
            // btnToogle
            // 
            this.btnToogle.Image = global::ScaleAddon.Properties.Resources.icons8_search_161;
            this.btnToogle.Location = new System.Drawing.Point(392, 174);
            this.btnToogle.Margin = new System.Windows.Forms.Padding(4);
            this.btnToogle.Name = "btnToogle";
            this.btnToogle.Size = new System.Drawing.Size(37, 28);
            this.btnToogle.TabIndex = 111;
            this.btnToogle.UseVisualStyleBackColor = true;
            this.btnToogle.Click += new System.EventHandler(this.btnToogle_Click);
            // 
            // remark
            // 
            this.remark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remark.BackColor = System.Drawing.SystemColors.Window;
            this.remark.Location = new System.Drawing.Point(603, 182);
            this.remark.Margin = new System.Windows.Forms.Padding(4);
            this.remark.Name = "remark";
            this.remark.Size = new System.Drawing.Size(193, 22);
            this.remark.TabIndex = 98;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(448, 185);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 17);
            this.label10.TabIndex = 97;
            this.label10.Text = "Remark";
            // 
            // WeightAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 714);
            this.Controls.Add(this.remark);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnToogle);
            this.Controls.Add(this.tbLot);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.checkHold);
            this.Controls.Add(this.cbLot);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.groupDetailInput);
            this.Controls.Add(this.tbAcumaticaReceipt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbAcumaticaIssue);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.tbDocNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "WeightAdjustment";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WeightAdjustment";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Closing);
            this.Load += new System.EventHandler(this.WeightAdjustment_Load);
            this.panel1.ResumeLayout(false);
            this.groupDetailInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntry)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbAcumaticaIssue;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ComboBox cbLot;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAcumatica;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbAcumaticaReceipt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupDetailInput;
        private System.Windows.Forms.DataGridView dgvEntry;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.TextBox tbEntryWReceiveNew;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbEntryWNettoNew;
        private System.Windows.Forms.TextBox tbEntryWTareNew;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbEntryWTare;
        private System.Windows.Forms.TextBox tbEntryWNetto;
        private System.Windows.Forms.TextBox tbEntryWReceive;
        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbDetailWTare;
        private System.Windows.Forms.TextBox tbDetailWNetto;
        private System.Windows.Forms.TextBox tbDetailWReceive;
        private System.Windows.Forms.Button btnSaveEntry;
        private System.Windows.Forms.Button btnRemoveEntry;
        private System.Windows.Forms.CheckBox checkHold;
        private System.Windows.Forms.Button btnPrintLot;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbScale;
        internal System.Windows.Forms.Button btnScaleOverride;
        internal System.Windows.Forms.Button btnScale;
        internal System.Windows.Forms.Button btnScaleComm;
        internal System.Windows.Forms.Button btnPrintDoc;
        private System.Windows.Forms.TextBox tbLot;
        private System.Windows.Forms.Button btnToogle;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox tbDetailLot;
        private System.Windows.Forms.TextBox remark;
        private System.Windows.Forms.Label label10;
    }
}