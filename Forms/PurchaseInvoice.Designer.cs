namespace ScaleAddon.Forms
{
    partial class PurchaseInvoice
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
            this.tbAcumaticaRefNbr = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.tbTotalReceived = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.tbDetailDeduct = new System.Windows.Forms.TextBox();
            this.tbDetailTax = new System.Windows.Forms.TextBox();
            this.tbDetailPayment = new System.Windows.Forms.TextBox();
            this.cbBuyingNbr = new System.Windows.Forms.ComboBox();
            this.groupEntry = new System.Windows.Forms.GroupBox();
            this.label18 = new System.Windows.Forms.Label();
            this.addonreceiptamount = new System.Windows.Forms.TextBox();
            this.btnAllocateAll = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.tbEntryPayment = new System.Windows.Forms.TextBox();
            this.btnDelLot = new System.Windows.Forms.Button();
            this.btnSaveLot = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbEntryDeductAmount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbEntryTaxAmount = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.tbEntryDeductPct = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbEntryReceiptAmount = new System.Windows.Forms.TextBox();
            this.tbEntryDoc = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.checkNPWP = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbProcessDate = new System.Windows.Forms.TextBox();
            this.cbVendorID = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbVendorDetails = new System.Windows.Forms.TextBox();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.btnPrintInvoice = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdateVendor = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.tbCashAdvance = new System.Windows.Forms.TextBox();
            this.tbBuyerName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbAdminInvoice = new System.Windows.Forms.TextBox();
            this.checkHold = new System.Windows.Forms.CheckBox();
            this.noNPWP = new System.Windows.Forms.TextBox();
            this.unsyncing = new System.Windows.Forms.Button();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.groupEntry.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbAcumaticaRefNbr
            // 
            this.tbAcumaticaRefNbr.BackColor = System.Drawing.SystemColors.Info;
            this.tbAcumaticaRefNbr.Location = new System.Drawing.Point(147, 119);
            this.tbAcumaticaRefNbr.Margin = new System.Windows.Forms.Padding(4);
            this.tbAcumaticaRefNbr.Name = "tbAcumaticaRefNbr";
            this.tbAcumaticaRefNbr.ReadOnly = true;
            this.tbAcumaticaRefNbr.Size = new System.Drawing.Size(239, 22);
            this.tbAcumaticaRefNbr.TabIndex = 107;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(11, 123);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(126, 17);
            this.label30.TabIndex = 106;
            this.label30.Text = "Acumatica Ref Nbr";
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvDetail);
            this.groupDetail.Controls.Add(this.pnlDetailSummary);
            this.groupDetail.Location = new System.Drawing.Point(13, 398);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4);
            this.groupDetail.Size = new System.Drawing.Size(1201, 306);
            this.groupDetail.TabIndex = 105;
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
            this.dgvDetail.Size = new System.Drawing.Size(1193, 177);
            this.dgvDetail.TabIndex = 1;
            this.dgvDetail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetail_CellClick);
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label14);
            this.pnlDetailSummary.Controls.Add(this.tbTotalReceived);
            this.pnlDetailSummary.Controls.Add(this.label12);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.label28);
            this.pnlDetailSummary.Controls.Add(this.tbDetailDeduct);
            this.pnlDetailSummary.Controls.Add(this.tbDetailTax);
            this.pnlDetailSummary.Controls.Add(this.tbDetailPayment);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 196);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(1193, 106);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(449, 11);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(88, 17);
            this.label14.TabIndex = 89;
            this.label14.Text = "Total Invoice";
            // 
            // tbTotalReceived
            // 
            this.tbTotalReceived.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTotalReceived.BackColor = System.Drawing.SystemColors.Info;
            this.tbTotalReceived.Location = new System.Drawing.Point(584, 7);
            this.tbTotalReceived.Margin = new System.Windows.Forms.Padding(4);
            this.tbTotalReceived.Name = "tbTotalReceived";
            this.tbTotalReceived.ReadOnly = true;
            this.tbTotalReceived.Size = new System.Drawing.Size(229, 22);
            this.tbTotalReceived.TabIndex = 90;
            this.tbTotalReceived.Text = "0";
            this.tbTotalReceived.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(824, 43);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 17);
            this.label12.TabIndex = 88;
            this.label12.Text = "Loan Deduction";
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(823, 11);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(99, 17);
            this.label29.TabIndex = 87;
            this.label29.Text = "Tax Deduction";
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(824, 75);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(124, 17);
            this.label28.TabIndex = 81;
            this.label28.Text = "Total For Payment";
            // 
            // tbDetailDeduct
            // 
            this.tbDetailDeduct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailDeduct.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailDeduct.Location = new System.Drawing.Point(959, 39);
            this.tbDetailDeduct.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailDeduct.Name = "tbDetailDeduct";
            this.tbDetailDeduct.ReadOnly = true;
            this.tbDetailDeduct.Size = new System.Drawing.Size(229, 22);
            this.tbDetailDeduct.TabIndex = 86;
            this.tbDetailDeduct.Text = "0";
            this.tbDetailDeduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbDetailDeduct.TextChanged += new System.EventHandler(this.tbDetailDeduct_TextChanged);
            // 
            // tbDetailTax
            // 
            this.tbDetailTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailTax.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailTax.Location = new System.Drawing.Point(959, 7);
            this.tbDetailTax.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailTax.Name = "tbDetailTax";
            this.tbDetailTax.ReadOnly = true;
            this.tbDetailTax.Size = new System.Drawing.Size(229, 22);
            this.tbDetailTax.TabIndex = 83;
            this.tbDetailTax.Text = "0";
            this.tbDetailTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailPayment
            // 
            this.tbDetailPayment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailPayment.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailPayment.Location = new System.Drawing.Point(959, 71);
            this.tbDetailPayment.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailPayment.Name = "tbDetailPayment";
            this.tbDetailPayment.ReadOnly = true;
            this.tbDetailPayment.Size = new System.Drawing.Size(229, 22);
            this.tbDetailPayment.TabIndex = 82;
            this.tbDetailPayment.Text = "0";
            this.tbDetailPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbBuyingNbr
            // 
            this.cbBuyingNbr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBuyingNbr.FormattingEnabled = true;
            this.cbBuyingNbr.Location = new System.Drawing.Point(147, 217);
            this.cbBuyingNbr.Margin = new System.Windows.Forms.Padding(4);
            this.cbBuyingNbr.Name = "cbBuyingNbr";
            this.cbBuyingNbr.Size = new System.Drawing.Size(239, 24);
            this.cbBuyingNbr.TabIndex = 104;
            this.cbBuyingNbr.SelectedIndexChanged += new System.EventHandler(this.cbBuyingNbr_SelectedIndexChanged);
            // 
            // groupEntry
            // 
            this.groupEntry.Controls.Add(this.label18);
            this.groupEntry.Controls.Add(this.addonreceiptamount);
            this.groupEntry.Controls.Add(this.btnAllocateAll);
            this.groupEntry.Controls.Add(this.label13);
            this.groupEntry.Controls.Add(this.tbEntryPayment);
            this.groupEntry.Controls.Add(this.btnDelLot);
            this.groupEntry.Controls.Add(this.btnSaveLot);
            this.groupEntry.Controls.Add(this.label4);
            this.groupEntry.Controls.Add(this.tbEntryDeductAmount);
            this.groupEntry.Controls.Add(this.label2);
            this.groupEntry.Controls.Add(this.tbEntryTaxAmount);
            this.groupEntry.Controls.Add(this.label27);
            this.groupEntry.Controls.Add(this.tbEntryDeductPct);
            this.groupEntry.Controls.Add(this.label17);
            this.groupEntry.Controls.Add(this.tbEntryReceiptAmount);
            this.groupEntry.Controls.Add(this.tbEntryDoc);
            this.groupEntry.Controls.Add(this.label10);
            this.groupEntry.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupEntry.Location = new System.Drawing.Point(13, 288);
            this.groupEntry.Margin = new System.Windows.Forms.Padding(4);
            this.groupEntry.Name = "groupEntry";
            this.groupEntry.Padding = new System.Windows.Forms.Padding(4);
            this.groupEntry.Size = new System.Drawing.Size(1195, 102);
            this.groupEntry.TabIndex = 103;
            this.groupEntry.TabStop = false;
            this.groupEntry.Text = "Receipt Entry";
            this.groupEntry.Enter += new System.EventHandler(this.groupEntry_Enter);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(5, 28);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(168, 17);
            this.label18.TabIndex = 99;
            this.label18.Text = "Receipt Amount Add On :";
            this.label18.Click += new System.EventHandler(this.label18_Click);
            // 
            // addonreceiptamount
            // 
            this.addonreceiptamount.BackColor = System.Drawing.Color.LightCoral;
            this.addonreceiptamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addonreceiptamount.Location = new System.Drawing.Point(188, 25);
            this.addonreceiptamount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addonreceiptamount.Name = "addonreceiptamount";
            this.addonreceiptamount.ReadOnly = true;
            this.addonreceiptamount.Size = new System.Drawing.Size(177, 22);
            this.addonreceiptamount.TabIndex = 98;
            this.addonreceiptamount.Text = "0";
            this.addonreceiptamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.addonreceiptamount.TextChanged += new System.EventHandler(this.addonreceiptamount_TextChanged);
            // 
            // btnAllocateAll
            // 
            this.btnAllocateAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAllocateAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAllocateAll.Enabled = false;
            this.btnAllocateAll.Image = global::ScaleAddon.Properties.Resources.icons8_price_30;
            this.btnAllocateAll.Location = new System.Drawing.Point(981, 41);
            this.btnAllocateAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnAllocateAll.Name = "btnAllocateAll";
            this.btnAllocateAll.Size = new System.Drawing.Size(59, 54);
            this.btnAllocateAll.TabIndex = 97;
            this.btnAllocateAll.UseVisualStyleBackColor = true;
            this.btnAllocateAll.Click += new System.EventHandler(this.btnAllocateAll_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(795, 53);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(115, 17);
            this.label13.TabIndex = 95;
            this.label13.Text = "Payment Amount";
            // 
            // tbEntryPayment
            // 
            this.tbEntryPayment.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryPayment.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryPayment.Location = new System.Drawing.Point(795, 71);
            this.tbEntryPayment.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryPayment.Name = "tbEntryPayment";
            this.tbEntryPayment.ReadOnly = true;
            this.tbEntryPayment.Size = new System.Drawing.Size(173, 22);
            this.tbEntryPayment.TabIndex = 96;
            this.tbEntryPayment.Text = "0";
            this.tbEntryPayment.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEntryPayment.TextChanged += new System.EventHandler(this.tbEntryPayment_TextChanged);
            // 
            // btnDelLot
            // 
            this.btnDelLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDelLot.Enabled = false;
            this.btnDelLot.Image = global::ScaleAddon.Properties.Resources.icons8_delete_archive_30;
            this.btnDelLot.Location = new System.Drawing.Point(1116, 39);
            this.btnDelLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelLot.Name = "btnDelLot";
            this.btnDelLot.Size = new System.Drawing.Size(59, 54);
            this.btnDelLot.TabIndex = 94;
            this.btnDelLot.UseVisualStyleBackColor = true;
            this.btnDelLot.Click += new System.EventHandler(this.btnDelLot_Click);
            // 
            // btnSaveLot
            // 
            this.btnSaveLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSaveLot.Enabled = false;
            this.btnSaveLot.Image = global::ScaleAddon.Properties.Resources.icons8_save_archive_30;
            this.btnSaveLot.Location = new System.Drawing.Point(1049, 41);
            this.btnSaveLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveLot.Name = "btnSaveLot";
            this.btnSaveLot.Size = new System.Drawing.Size(59, 54);
            this.btnSaveLot.TabIndex = 93;
            this.btnSaveLot.UseVisualStyleBackColor = true;
            this.btnSaveLot.Click += new System.EventHandler(this.btnSaveLot_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(593, 53);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 17);
            this.label4.TabIndex = 85;
            this.label4.Text = "Deduction Amount";
            // 
            // tbEntryDeductAmount
            // 
            this.tbEntryDeductAmount.BackColor = System.Drawing.SystemColors.Window;
            this.tbEntryDeductAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryDeductAmount.Location = new System.Drawing.Point(595, 71);
            this.tbEntryDeductAmount.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryDeductAmount.Name = "tbEntryDeductAmount";
            this.tbEntryDeductAmount.Size = new System.Drawing.Size(193, 22);
            this.tbEntryDeductAmount.TabIndex = 86;
            this.tbEntryDeductAmount.Text = "0";
            this.tbEntryDeductAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEntryDeductAmount.TextChanged += new System.EventHandler(this.tbEntryDeductAmount_TextChanged);
            this.tbEntryDeductAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEntryDeductAmount_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(373, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 17);
            this.label2.TabIndex = 83;
            this.label2.Text = "Tax Amount";
            // 
            // tbEntryTaxAmount
            // 
            this.tbEntryTaxAmount.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryTaxAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryTaxAmount.Location = new System.Drawing.Point(373, 71);
            this.tbEntryTaxAmount.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryTaxAmount.Name = "tbEntryTaxAmount";
            this.tbEntryTaxAmount.ReadOnly = true;
            this.tbEntryTaxAmount.Size = new System.Drawing.Size(127, 22);
            this.tbEntryTaxAmount.TabIndex = 84;
            this.tbEntryTaxAmount.Text = "0";
            this.tbEntryTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(507, 53);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(79, 17);
            this.label27.TabIndex = 79;
            this.label27.Text = "Deduct (%)";
            // 
            // tbEntryDeductPct
            // 
            this.tbEntryDeductPct.BackColor = System.Drawing.SystemColors.Window;
            this.tbEntryDeductPct.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryDeductPct.Location = new System.Drawing.Point(509, 71);
            this.tbEntryDeductPct.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryDeductPct.Name = "tbEntryDeductPct";
            this.tbEntryDeductPct.Size = new System.Drawing.Size(77, 22);
            this.tbEntryDeductPct.TabIndex = 63;
            this.tbEntryDeductPct.Text = "0";
            this.tbEntryDeductPct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbEntryDeductPct.TextChanged += new System.EventHandler(this.tbEntryDeductPct_TextChanged);
            this.tbEntryDeductPct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEntryDeductPct_KeyPress);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(185, 52);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(177, 17);
            this.label17.TabIndex = 58;
            this.label17.Text = "Receipt Amount Acumatica";
            // 
            // tbEntryReceiptAmount
            // 
            this.tbEntryReceiptAmount.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryReceiptAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryReceiptAmount.Location = new System.Drawing.Point(188, 73);
            this.tbEntryReceiptAmount.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryReceiptAmount.Name = "tbEntryReceiptAmount";
            this.tbEntryReceiptAmount.ReadOnly = true;
            this.tbEntryReceiptAmount.Size = new System.Drawing.Size(177, 22);
            this.tbEntryReceiptAmount.TabIndex = 61;
            this.tbEntryReceiptAmount.Text = "0";
            this.tbEntryReceiptAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbEntryDoc
            // 
            this.tbEntryDoc.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEntryDoc.Location = new System.Drawing.Point(8, 73);
            this.tbEntryDoc.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryDoc.Name = "tbEntryDoc";
            this.tbEntryDoc.ReadOnly = true;
            this.tbEntryDoc.Size = new System.Drawing.Size(171, 22);
            this.tbEntryDoc.TabIndex = 43;
            this.tbEntryDoc.TextChanged += new System.EventHandler(this.tbEntryDoc_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 52);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 17);
            this.label10.TabIndex = 43;
            this.label10.Text = "Receipt Number";
            // 
            // checkNPWP
            // 
            this.checkNPWP.AutoSize = true;
            this.checkNPWP.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkNPWP.Checked = true;
            this.checkNPWP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkNPWP.Location = new System.Drawing.Point(424, 250);
            this.checkNPWP.Margin = new System.Windows.Forms.Padding(4);
            this.checkNPWP.Name = "checkNPWP";
            this.checkNPWP.Size = new System.Drawing.Size(97, 21);
            this.checkNPWP.TabIndex = 74;
            this.checkNPWP.Text = "No. NPWP";
            this.checkNPWP.UseVisualStyleBackColor = true;
            this.checkNPWP.CheckedChanged += new System.EventHandler(this.checkNPWP_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(421, 123);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 102;
            this.label8.Text = "Warehouse";
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(557, 119);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 101;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 222);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 17);
            this.label9.TabIndex = 100;
            this.label9.Text = "Buying Ref Nbr";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(421, 91);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 17);
            this.label7.TabIndex = 99;
            this.label7.Text = "Invoice Date";
            // 
            // tbProcessDate
            // 
            this.tbProcessDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbProcessDate.Location = new System.Drawing.Point(557, 87);
            this.tbProcessDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbProcessDate.Name = "tbProcessDate";
            this.tbProcessDate.ReadOnly = true;
            this.tbProcessDate.Size = new System.Drawing.Size(239, 22);
            this.tbProcessDate.TabIndex = 98;
            // 
            // cbVendorID
            // 
            this.cbVendorID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVendorID.FormattingEnabled = true;
            this.cbVendorID.Location = new System.Drawing.Point(557, 151);
            this.cbVendorID.Margin = new System.Windows.Forms.Padding(4);
            this.cbVendorID.Name = "cbVendorID";
            this.cbVendorID.Size = new System.Drawing.Size(239, 24);
            this.cbVendorID.TabIndex = 97;
            this.cbVendorID.SelectedIndexChanged += new System.EventHandler(this.cbVendorID_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(419, 155);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 17);
            this.label6.TabIndex = 96;
            this.label6.Text = "Vendor ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(833, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 17);
            this.label3.TabIndex = 94;
            this.label3.Text = "Doc. Status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(421, 187);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 17);
            this.label5.TabIndex = 92;
            this.label5.Text = "Farmer Details";
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tbStatus.Location = new System.Drawing.Point(969, 87);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(167, 22);
            this.tbStatus.TabIndex = 90;
            // 
            // tbVendorDetails
            // 
            this.tbVendorDetails.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorDetails.Location = new System.Drawing.Point(557, 183);
            this.tbVendorDetails.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorDetails.Multiline = true;
            this.tbVendorDetails.Name = "tbVendorDetails";
            this.tbVendorDetails.ReadOnly = true;
            this.tbVendorDetails.Size = new System.Drawing.Size(651, 56);
            this.tbVendorDetails.TabIndex = 89;
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Info;
            this.tbDocNumber.Location = new System.Drawing.Point(147, 87);
            this.tbDocNumber.Margin = new System.Windows.Forms.Padding(4);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(239, 22);
            this.tbDocNumber.TabIndex = 88;
            this.tbDocNumber.Text = "<NEW>";
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 87;
            this.label1.Text = "Document Number";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.unsyncing);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Controls.Add(this.btnPrintInvoice);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1221, 73);
            this.panel1.TabIndex = 86;
            // 
            // btnAcumatica
            // 
            this.btnAcumatica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcumatica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAcumatica.Enabled = false;
            this.btnAcumatica.Image = global::ScaleAddon.Properties.Resources.icons8_upload_to_the_cloud_30;
            this.btnAcumatica.Location = new System.Drawing.Point(1155, 4);
            this.btnAcumatica.Margin = new System.Windows.Forms.Padding(4);
            this.btnAcumatica.Name = "btnAcumatica";
            this.btnAcumatica.Size = new System.Drawing.Size(59, 54);
            this.btnAcumatica.TabIndex = 15;
            this.btnAcumatica.UseVisualStyleBackColor = true;
            this.btnAcumatica.Click += new System.EventHandler(this.btnAcumatica_Click);
            // 
            // btnPrintInvoice
            // 
            this.btnPrintInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintInvoice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintInvoice.Enabled = false;
            this.btnPrintInvoice.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrintInvoice.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintInvoice.Location = new System.Drawing.Point(236, 4);
            this.btnPrintInvoice.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintInvoice.Name = "btnPrintInvoice";
            this.btnPrintInvoice.Size = new System.Drawing.Size(87, 65);
            this.btnPrintInvoice.TabIndex = 14;
            this.btnPrintInvoice.Text = "Print Doc";
            this.btnPrintInvoice.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrintInvoice.UseVisualStyleBackColor = true;
            this.btnPrintInvoice.Click += new System.EventHandler(this.btnPrintLot_Click);
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
            // btnUpdateVendor
            // 
            this.btnUpdateVendor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateVendor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUpdateVendor.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_16;
            this.btnUpdateVendor.Location = new System.Drawing.Point(1144, 142);
            this.btnUpdateVendor.Margin = new System.Windows.Forms.Padding(4);
            this.btnUpdateVendor.Name = "btnUpdateVendor";
            this.btnUpdateVendor.Size = new System.Drawing.Size(59, 33);
            this.btnUpdateVendor.TabIndex = 49;
            this.btnUpdateVendor.UseVisualStyleBackColor = true;
            this.btnUpdateVendor.Click += new System.EventHandler(this.btnUpdateVendor_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(833, 155);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 17);
            this.label11.TabIndex = 109;
            this.label11.Text = "Farmer Advance";
            // 
            // tbCashAdvance
            // 
            this.tbCashAdvance.BackColor = System.Drawing.Color.LightCoral;
            this.tbCashAdvance.Location = new System.Drawing.Point(969, 151);
            this.tbCashAdvance.Margin = new System.Windows.Forms.Padding(4);
            this.tbCashAdvance.Name = "tbCashAdvance";
            this.tbCashAdvance.ReadOnly = true;
            this.tbCashAdvance.Size = new System.Drawing.Size(167, 22);
            this.tbCashAdvance.TabIndex = 108;
            this.tbCashAdvance.Text = "0";
            this.tbCashAdvance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbBuyerName
            // 
            this.tbBuyerName.BackColor = System.Drawing.SystemColors.Window;
            this.tbBuyerName.Location = new System.Drawing.Point(147, 151);
            this.tbBuyerName.Margin = new System.Windows.Forms.Padding(4);
            this.tbBuyerName.Name = "tbBuyerName";
            this.tbBuyerName.Size = new System.Drawing.Size(239, 22);
            this.tbBuyerName.TabIndex = 113;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 155);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 17);
            this.label15.TabIndex = 112;
            this.label15.Text = "Buyer";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 187);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(47, 17);
            this.label16.TabIndex = 111;
            this.label16.Text = "Admin";
            // 
            // tbAdminInvoice
            // 
            this.tbAdminInvoice.BackColor = System.Drawing.SystemColors.Window;
            this.tbAdminInvoice.Location = new System.Drawing.Point(147, 183);
            this.tbAdminInvoice.Margin = new System.Windows.Forms.Padding(4);
            this.tbAdminInvoice.Name = "tbAdminInvoice";
            this.tbAdminInvoice.Size = new System.Drawing.Size(239, 22);
            this.tbAdminInvoice.TabIndex = 110;
            // 
            // checkHold
            // 
            this.checkHold.AutoSize = true;
            this.checkHold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkHold.Location = new System.Drawing.Point(1147, 89);
            this.checkHold.Margin = new System.Windows.Forms.Padding(4);
            this.checkHold.Name = "checkHold";
            this.checkHold.Size = new System.Drawing.Size(59, 21);
            this.checkHold.TabIndex = 114;
            this.checkHold.Text = "Hold";
            this.checkHold.UseVisualStyleBackColor = true;
            this.checkHold.CheckedChanged += new System.EventHandler(this.checkHold_CheckedChanged);
            // 
            // noNPWP
            // 
            this.noNPWP.BackColor = System.Drawing.SystemColors.Info;
            this.noNPWP.Location = new System.Drawing.Point(557, 250);
            this.noNPWP.Name = "noNPWP";
            this.noNPWP.ReadOnly = true;
            this.noNPWP.Size = new System.Drawing.Size(651, 22);
            this.noNPWP.TabIndex = 115;
            // 
            // unsyncing
            // 
            this.unsyncing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unsyncing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.unsyncing.Enabled = false;
            this.unsyncing.Image = global::ScaleAddon.Properties.Resources.icons8_donwload_to_the_cloud_30;
            this.unsyncing.Location = new System.Drawing.Point(1088, 4);
            this.unsyncing.Margin = new System.Windows.Forms.Padding(4);
            this.unsyncing.Name = "unsyncing";
            this.unsyncing.Size = new System.Drawing.Size(59, 54);
            this.unsyncing.TabIndex = 169;
            this.unsyncing.UseVisualStyleBackColor = true;
            this.unsyncing.Click += new System.EventHandler(this.unsyncing_Click);
            // 
            // PurchaseInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1221, 708);
            this.Controls.Add(this.noNPWP);
            this.Controls.Add(this.btnUpdateVendor);
            this.Controls.Add(this.checkHold);
            this.Controls.Add(this.tbBuyerName);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbAdminInvoice);
            this.Controls.Add(this.tbCashAdvance);
            this.Controls.Add(this.tbAcumaticaRefNbr);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.checkNPWP);
            this.Controls.Add(this.cbBuyingNbr);
            this.Controls.Add(this.groupEntry);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbProcessDate);
            this.Controls.Add(this.cbVendorID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbVendorDetails);
            this.Controls.Add(this.tbDocNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PurchaseInvoice";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PurchaseInvoice";
            this.Load += new System.EventHandler(this.PurchaseInvoice_Load);
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.groupEntry.ResumeLayout(false);
            this.groupEntry.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbAcumaticaRefNbr;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox tbDetailDeduct;
        private System.Windows.Forms.TextBox tbDetailTax;
        private System.Windows.Forms.TextBox tbDetailPayment;
        private System.Windows.Forms.ComboBox cbBuyingNbr;
        private System.Windows.Forms.GroupBox groupEntry;
        internal System.Windows.Forms.Button btnPrintInvoice;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox tbEntryDeductPct;
        private System.Windows.Forms.CheckBox checkNPWP;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbEntryReceiptAmount;
        private System.Windows.Forms.TextBox tbEntryDoc;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbProcessDate;
        private System.Windows.Forms.ComboBox cbVendorID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TextBox tbVendorDetails;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAcumatica;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbEntryDeductAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEntryTaxAmount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbCashAdvance;
        private System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Button btnDelLot;
        internal System.Windows.Forms.Button btnSaveLot;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbEntryPayment;
        internal System.Windows.Forms.Button btnUpdateVendor;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbTotalReceived;
        internal System.Windows.Forms.Button btnAllocateAll;
        private System.Windows.Forms.TextBox tbBuyerName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbAdminInvoice;
        private System.Windows.Forms.CheckBox checkHold;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox addonreceiptamount;
        private System.Windows.Forms.TextBox noNPWP;
        internal System.Windows.Forms.Button unsyncing;
    }
}