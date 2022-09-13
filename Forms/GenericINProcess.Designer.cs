namespace ScaleAddon.Forms
{
    partial class GenericINProcess
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
            this.label6 = new System.Windows.Forms.Label();
            this.tbTotalCost = new System.Windows.Forms.TextBox();
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label38 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.tbDetailLot = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailWTare = new System.Windows.Forms.TextBox();
            this.tbDetailWNetto = new System.Windows.Forms.TextBox();
            this.tbDetailWShipping = new System.Windows.Forms.TextBox();
            this.tbDetailWReceive = new System.Windows.Forms.TextBox();
            this.groupEntry = new System.Windows.Forms.GroupBox();
            this.tbEntryDate = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbEntryLotGroup = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbEntryArea = new System.Windows.Forms.TextBox();
            this.tbEntryGrade = new System.Windows.Forms.TextBox();
            this.tbEntrySubItem = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.tbEntryWeightTare = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbEntryWeightNetto = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.tbEntryWeightReceive = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.tbEntryInv = new System.Windows.Forms.TextBox();
            this.tbEntryLot = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnDelLot = new System.Windows.Forms.Button();
            this.btnSaveLot = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbProcessDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.unsyncing = new System.Windows.Forms.Button();
            this.btnPrintSUM = new System.Windows.Forms.Button();
            this.btnPrintDoc = new System.Windows.Forms.Button();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbAcumaticaRefNbr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBuyerName = new System.Windows.Forms.TextBox();
            this.checkHold = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.tbLot = new System.Windows.Forms.TextBox();
            this.btnToogle = new System.Windows.Forms.Button();
            this.cbLot = new System.Windows.Forms.ComboBox();
            this.SaveImport = new System.Windows.Forms.Button();
            this.cbosheet = new System.Windows.Forms.ComboBox();
            this.textFilename = new System.Windows.Forms.TextBox();
            this.ImportButton = new System.Windows.Forms.Button();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.groupEntry.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(425, 117);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 81;
            this.label6.Text = "Total Cost";
            // 
            // tbTotalCost
            // 
            this.tbTotalCost.BackColor = System.Drawing.SystemColors.Info;
            this.tbTotalCost.Location = new System.Drawing.Point(561, 113);
            this.tbTotalCost.Margin = new System.Windows.Forms.Padding(4);
            this.tbTotalCost.Name = "tbTotalCost";
            this.tbTotalCost.ReadOnly = true;
            this.tbTotalCost.Size = new System.Drawing.Size(239, 22);
            this.tbTotalCost.TabIndex = 80;
            this.tbTotalCost.Text = "0";
            this.tbTotalCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvDetail);
            this.groupDetail.Controls.Add(this.pnlDetailSummary);
            this.groupDetail.Location = new System.Drawing.Point(20, 346);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4);
            this.groupDetail.Size = new System.Drawing.Size(1193, 372);
            this.groupDetail.TabIndex = 79;
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
            this.dgvDetail.Size = new System.Drawing.Size(1185, 310);
            this.dgvDetail.TabIndex = 1;
            this.dgvDetail.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDetail_CellClick);
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label38);
            this.pnlDetailSummary.Controls.Add(this.label34);
            this.pnlDetailSummary.Controls.Add(this.tbDetailLot);
            this.pnlDetailSummary.Controls.Add(this.label33);
            this.pnlDetailSummary.Controls.Add(this.label32);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWTare);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWNetto);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWShipping);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWReceive);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 329);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(1185, 39);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(279, 11);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(59, 16);
            this.label38.TabIndex = 116;
            this.label38.Text = "Total Lot";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(676, 11);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(68, 16);
            this.label34.TabIndex = 100;
            this.label34.Text = "Receiving";
            // 
            // tbDetailLot
            // 
            this.tbDetailLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailLot.Location = new System.Drawing.Point(352, 7);
            this.tbDetailLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailLot.Name = "tbDetailLot";
            this.tbDetailLot.ReadOnly = true;
            this.tbDetailLot.Size = new System.Drawing.Size(99, 22);
            this.tbDetailLot.TabIndex = 115;
            this.tbDetailLot.Text = "0";
            this.tbDetailLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(865, 11);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(36, 16);
            this.label33.TabIndex = 99;
            this.label33.Text = "Tare";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(1025, 11);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(39, 16);
            this.label32.TabIndex = 98;
            this.label32.Text = "Netto";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(460, 11);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(94, 16);
            this.label29.TabIndex = 97;
            this.label29.Text = "Total Shipping";
            // 
            // tbDetailWTare
            // 
            this.tbDetailWTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWTare.Location = new System.Drawing.Point(917, 7);
            this.tbDetailWTare.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWTare.Name = "tbDetailWTare";
            this.tbDetailWTare.ReadOnly = true;
            this.tbDetailWTare.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWTare.TabIndex = 95;
            this.tbDetailWTare.Text = "0";
            this.tbDetailWTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWNetto
            // 
            this.tbDetailWNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWNetto.Location = new System.Drawing.Point(1081, 7);
            this.tbDetailWNetto.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWNetto.Name = "tbDetailWNetto";
            this.tbDetailWNetto.ReadOnly = true;
            this.tbDetailWNetto.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWNetto.TabIndex = 96;
            this.tbDetailWNetto.Text = "0";
            this.tbDetailWNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWShipping
            // 
            this.tbDetailWShipping.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWShipping.Location = new System.Drawing.Point(568, 7);
            this.tbDetailWShipping.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWShipping.Name = "tbDetailWShipping";
            this.tbDetailWShipping.ReadOnly = true;
            this.tbDetailWShipping.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWShipping.TabIndex = 93;
            this.tbDetailWShipping.Text = "0";
            this.tbDetailWShipping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWReceive
            // 
            this.tbDetailWReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWReceive.Location = new System.Drawing.Point(757, 7);
            this.tbDetailWReceive.Margin = new System.Windows.Forms.Padding(4);
            this.tbDetailWReceive.Name = "tbDetailWReceive";
            this.tbDetailWReceive.ReadOnly = true;
            this.tbDetailWReceive.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWReceive.TabIndex = 94;
            this.tbDetailWReceive.Text = "0";
            this.tbDetailWReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupEntry
            // 
            this.groupEntry.Controls.Add(this.tbEntryDate);
            this.groupEntry.Controls.Add(this.label15);
            this.groupEntry.Controls.Add(this.tbEntryLotGroup);
            this.groupEntry.Controls.Add(this.label14);
            this.groupEntry.Controls.Add(this.tbEntryArea);
            this.groupEntry.Controls.Add(this.tbEntryGrade);
            this.groupEntry.Controls.Add(this.tbEntrySubItem);
            this.groupEntry.Controls.Add(this.label27);
            this.groupEntry.Controls.Add(this.tbEntryWeightTare);
            this.groupEntry.Controls.Add(this.label21);
            this.groupEntry.Controls.Add(this.tbEntryWeightNetto);
            this.groupEntry.Controls.Add(this.label17);
            this.groupEntry.Controls.Add(this.tbEntryWeightReceive);
            this.groupEntry.Controls.Add(this.label16);
            this.groupEntry.Controls.Add(this.label13);
            this.groupEntry.Controls.Add(this.label12);
            this.groupEntry.Controls.Add(this.label11);
            this.groupEntry.Controls.Add(this.tbEntryInv);
            this.groupEntry.Controls.Add(this.tbEntryLot);
            this.groupEntry.Controls.Add(this.label10);
            this.groupEntry.Location = new System.Drawing.Point(20, 210);
            this.groupEntry.Margin = new System.Windows.Forms.Padding(4);
            this.groupEntry.Name = "groupEntry";
            this.groupEntry.Padding = new System.Windows.Forms.Padding(4);
            this.groupEntry.Size = new System.Drawing.Size(1042, 127);
            this.groupEntry.TabIndex = 1;
            this.groupEntry.TabStop = false;
            this.groupEntry.Text = "Lot Entry";
            // 
            // tbEntryDate
            // 
            this.tbEntryDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryDate.Location = new System.Drawing.Point(337, 87);
            this.tbEntryDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryDate.Name = "tbEntryDate";
            this.tbEntryDate.ReadOnly = true;
            this.tbEntryDate.Size = new System.Drawing.Size(313, 22);
            this.tbEntryDate.TabIndex = 108;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(341, 68);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(90, 16);
            this.label15.TabIndex = 109;
            this.label15.Text = "Lot Entry Date";
            // 
            // tbEntryLotGroup
            // 
            this.tbEntryLotGroup.BackColor = System.Drawing.SystemColors.Window;
            this.tbEntryLotGroup.Location = new System.Drawing.Point(15, 87);
            this.tbEntryLotGroup.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryLotGroup.Name = "tbEntryLotGroup";
            this.tbEntryLotGroup.Size = new System.Drawing.Size(313, 22);
            this.tbEntryLotGroup.TabIndex = 0;
            this.tbEntryLotGroup.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEntryLotGroup_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(19, 68);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 16);
            this.label14.TabIndex = 107;
            this.label14.Text = "Lot Group";
            // 
            // tbEntryArea
            // 
            this.tbEntryArea.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryArea.Location = new System.Drawing.Point(620, 39);
            this.tbEntryArea.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryArea.Name = "tbEntryArea";
            this.tbEntryArea.ReadOnly = true;
            this.tbEntryArea.Size = new System.Drawing.Size(79, 22);
            this.tbEntryArea.TabIndex = 91;
            // 
            // tbEntryGrade
            // 
            this.tbEntryGrade.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryGrade.Location = new System.Drawing.Point(479, 39);
            this.tbEntryGrade.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryGrade.Name = "tbEntryGrade";
            this.tbEntryGrade.ReadOnly = true;
            this.tbEntryGrade.Size = new System.Drawing.Size(132, 22);
            this.tbEntryGrade.TabIndex = 90;
            // 
            // tbEntrySubItem
            // 
            this.tbEntrySubItem.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntrySubItem.Location = new System.Drawing.Point(337, 39);
            this.tbEntrySubItem.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntrySubItem.Name = "tbEntrySubItem";
            this.tbEntrySubItem.ReadOnly = true;
            this.tbEntrySubItem.Size = new System.Drawing.Size(132, 22);
            this.tbEntrySubItem.TabIndex = 89;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(820, 20);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(36, 16);
            this.label27.TabIndex = 79;
            this.label27.Text = "Tare";
            // 
            // tbEntryWeightTare
            // 
            this.tbEntryWeightTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWeightTare.Location = new System.Drawing.Point(816, 39);
            this.tbEntryWeightTare.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWeightTare.Name = "tbEntryWeightTare";
            this.tbEntryWeightTare.ReadOnly = true;
            this.tbEntryWeightTare.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWeightTare.TabIndex = 63;
            this.tbEntryWeightTare.Text = "0";
            this.tbEntryWeightTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(928, 21);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(39, 16);
            this.label21.TabIndex = 66;
            this.label21.Text = "Netto";
            // 
            // tbEntryWeightNetto
            // 
            this.tbEntryWeightNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWeightNetto.Location = new System.Drawing.Point(924, 39);
            this.tbEntryWeightNetto.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWeightNetto.Name = "tbEntryWeightNetto";
            this.tbEntryWeightNetto.ReadOnly = true;
            this.tbEntryWeightNetto.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWeightNetto.TabIndex = 65;
            this.tbEntryWeightNetto.Text = "0";
            this.tbEntryWeightNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(712, 20);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 16);
            this.label17.TabIndex = 58;
            this.label17.Text = "Receive";
            // 
            // tbEntryWeightReceive
            // 
            this.tbEntryWeightReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbEntryWeightReceive.Location = new System.Drawing.Point(708, 39);
            this.tbEntryWeightReceive.Margin = new System.Windows.Forms.Padding(4);
            this.tbEntryWeightReceive.Name = "tbEntryWeightReceive";
            this.tbEntryWeightReceive.ReadOnly = true;
            this.tbEntryWeightReceive.Size = new System.Drawing.Size(99, 22);
            this.tbEntryWeightReceive.TabIndex = 61;
            this.tbEntryWeightReceive.Text = "0";
            this.tbEntryWeightReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(341, 20);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 16);
            this.label16.TabIndex = 56;
            this.label16.Text = "Sub Item";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(624, 20);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(36, 16);
            this.label13.TabIndex = 51;
            this.label13.Text = "Area";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(483, 20);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 16);
            this.label12.TabIndex = 50;
            this.label12.Text = "Grade";
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
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 20);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 16);
            this.label10.TabIndex = 43;
            this.label10.Text = "Lot. Number";
            // 
            // btnDelLot
            // 
            this.btnDelLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDelLot.Enabled = false;
            this.btnDelLot.Image = global::ScaleAddon.Properties.Resources.icons8_delete_archive_30;
            this.btnDelLot.Location = new System.Drawing.Point(1154, 233);
            this.btnDelLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelLot.Name = "btnDelLot";
            this.btnDelLot.Size = new System.Drawing.Size(59, 54);
            this.btnDelLot.TabIndex = 4;
            this.btnDelLot.UseVisualStyleBackColor = true;
            this.btnDelLot.Click += new System.EventHandler(this.btnDelLot_Click);
            // 
            // btnSaveLot
            // 
            this.btnSaveLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSaveLot.Enabled = false;
            this.btnSaveLot.Image = global::ScaleAddon.Properties.Resources.icons8_save_archive_30;
            this.btnSaveLot.Location = new System.Drawing.Point(1090, 233);
            this.btnSaveLot.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveLot.Name = "btnSaveLot";
            this.btnSaveLot.Size = new System.Drawing.Size(59, 54);
            this.btnSaveLot.TabIndex = 3;
            this.btnSaveLot.UseVisualStyleBackColor = true;
            this.btnSaveLot.Click += new System.EventHandler(this.btnSaveLot_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 149);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 16);
            this.label8.TabIndex = 76;
            this.label8.Text = "Warehouse";
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(152, 145);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 75;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 181);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 16);
            this.label9.TabIndex = 74;
            this.label9.Text = "Tobacco Lot";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(425, 85);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 16);
            this.label7.TabIndex = 73;
            this.label7.Text = "Process Date";
            // 
            // tbProcessDate
            // 
            this.tbProcessDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbProcessDate.Location = new System.Drawing.Point(561, 81);
            this.tbProcessDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbProcessDate.Name = "tbProcessDate";
            this.tbProcessDate.ReadOnly = true;
            this.tbProcessDate.Size = new System.Drawing.Size(239, 22);
            this.tbProcessDate.TabIndex = 72;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(837, 85);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 16);
            this.label3.TabIndex = 71;
            this.label3.Text = "Doc. Status";
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.tbStatus.Location = new System.Drawing.Point(973, 81);
            this.tbStatus.Margin = new System.Windows.Forms.Padding(4);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(167, 22);
            this.tbStatus.TabIndex = 70;
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Info;
            this.tbDocNumber.Location = new System.Drawing.Point(152, 81);
            this.tbDocNumber.Margin = new System.Windows.Forms.Padding(4);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(239, 22);
            this.tbDocNumber.TabIndex = 69;
            this.tbDocNumber.Text = "<NEW>";
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 85);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 16);
            this.label1.TabIndex = 68;
            this.label1.Text = "Document Number";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.SaveImport);
            this.panel1.Controls.Add(this.unsyncing);
            this.panel1.Controls.Add(this.cbosheet);
            this.panel1.Controls.Add(this.btnPrintSUM);
            this.panel1.Controls.Add(this.textFilename);
            this.panel1.Controls.Add(this.ImportButton);
            this.panel1.Controls.Add(this.btnPrintDoc);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1232, 73);
            this.panel1.TabIndex = 67;
            // 
            // unsyncing
            // 
            this.unsyncing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unsyncing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.unsyncing.Enabled = false;
            this.unsyncing.Image = global::ScaleAddon.Properties.Resources.icons8_donwload_to_the_cloud_30;
            this.unsyncing.Location = new System.Drawing.Point(1102, 4);
            this.unsyncing.Margin = new System.Windows.Forms.Padding(4);
            this.unsyncing.Name = "unsyncing";
            this.unsyncing.Size = new System.Drawing.Size(59, 54);
            this.unsyncing.TabIndex = 168;
            this.unsyncing.UseVisualStyleBackColor = true;
            this.unsyncing.Click += new System.EventHandler(this.unsyncing_Click);
            // 
            // btnPrintSUM
            // 
            this.btnPrintSUM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintSUM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintSUM.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrintSUM.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintSUM.Location = new System.Drawing.Point(335, 4);
            this.btnPrintSUM.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintSUM.Name = "btnPrintSUM";
            this.btnPrintSUM.Size = new System.Drawing.Size(87, 65);
            this.btnPrintSUM.TabIndex = 112;
            this.btnPrintSUM.Text = "Print SUM";
            this.btnPrintSUM.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrintSUM.UseVisualStyleBackColor = true;
            this.btnPrintSUM.Click += new System.EventHandler(this.btnPrintSUM_Click);
            // 
            // btnPrintDoc
            // 
            this.btnPrintDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintDoc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrintDoc.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrintDoc.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrintDoc.Location = new System.Drawing.Point(240, 4);
            this.btnPrintDoc.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrintDoc.Name = "btnPrintDoc";
            this.btnPrintDoc.Size = new System.Drawing.Size(87, 65);
            this.btnPrintDoc.TabIndex = 111;
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
            // tbAcumaticaRefNbr
            // 
            this.tbAcumaticaRefNbr.BackColor = System.Drawing.SystemColors.Info;
            this.tbAcumaticaRefNbr.Location = new System.Drawing.Point(152, 113);
            this.tbAcumaticaRefNbr.Margin = new System.Windows.Forms.Padding(4);
            this.tbAcumaticaRefNbr.Name = "tbAcumaticaRefNbr";
            this.tbAcumaticaRefNbr.ReadOnly = true;
            this.tbAcumaticaRefNbr.Size = new System.Drawing.Size(239, 22);
            this.tbAcumaticaRefNbr.TabIndex = 83;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 16);
            this.label2.TabIndex = 82;
            this.label2.Text = "Acumatica Ref Nbr";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(837, 117);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 16);
            this.label4.TabIndex = 92;
            this.label4.Text = "Buyer Name";
            // 
            // tbBuyerName
            // 
            this.tbBuyerName.BackColor = System.Drawing.SystemColors.Info;
            this.tbBuyerName.Location = new System.Drawing.Point(973, 113);
            this.tbBuyerName.Margin = new System.Windows.Forms.Padding(4);
            this.tbBuyerName.Name = "tbBuyerName";
            this.tbBuyerName.ReadOnly = true;
            this.tbBuyerName.Size = new System.Drawing.Size(239, 22);
            this.tbBuyerName.TabIndex = 91;
            this.tbBuyerName.TextChanged += new System.EventHandler(this.tbBuyerName_TextChanged);
            // 
            // checkHold
            // 
            this.checkHold.AutoSize = true;
            this.checkHold.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkHold.Location = new System.Drawing.Point(1149, 82);
            this.checkHold.Margin = new System.Windows.Forms.Padding(4);
            this.checkHold.Name = "checkHold";
            this.checkHold.Size = new System.Drawing.Size(58, 20);
            this.checkHold.TabIndex = 104;
            this.checkHold.Text = "Hold";
            this.checkHold.UseVisualStyleBackColor = true;
            this.checkHold.CheckedChanged += new System.EventHandler(this.checkHold_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(425, 149);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 16);
            this.label5.TabIndex = 106;
            this.label5.Text = "Notes";
            // 
            // tbNotes
            // 
            this.tbNotes.BackColor = System.Drawing.SystemColors.Window;
            this.tbNotes.Location = new System.Drawing.Point(561, 145);
            this.tbNotes.Margin = new System.Windows.Forms.Padding(4);
            this.tbNotes.Multiline = true;
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(651, 57);
            this.tbNotes.TabIndex = 0;
            this.tbNotes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNotes_KeyPress);
            // 
            // tbLot
            // 
            this.tbLot.BackColor = System.Drawing.SystemColors.Window;
            this.tbLot.Location = new System.Drawing.Point(152, 177);
            this.tbLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbLot.Name = "tbLot";
            this.tbLot.Size = new System.Drawing.Size(193, 22);
            this.tbLot.TabIndex = 2;
            this.tbLot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLot_KeyPress);
            // 
            // btnToogle
            // 
            this.btnToogle.Image = global::ScaleAddon.Properties.Resources.icons8_search_161;
            this.btnToogle.Location = new System.Drawing.Point(355, 174);
            this.btnToogle.Margin = new System.Windows.Forms.Padding(4);
            this.btnToogle.Name = "btnToogle";
            this.btnToogle.Size = new System.Drawing.Size(37, 28);
            this.btnToogle.TabIndex = 113;
            this.btnToogle.UseVisualStyleBackColor = true;
            this.btnToogle.Click += new System.EventHandler(this.btnToogle_Click);
            // 
            // cbLot
            // 
            this.cbLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLot.FormattingEnabled = true;
            this.cbLot.Location = new System.Drawing.Point(152, 176);
            this.cbLot.Margin = new System.Windows.Forms.Padding(4);
            this.cbLot.Name = "cbLot";
            this.cbLot.Size = new System.Drawing.Size(193, 24);
            this.cbLot.TabIndex = 78;
            this.cbLot.Visible = false;
            this.cbLot.SelectedIndexChanged += new System.EventHandler(this.cbLot_SelectedIndexChanged);
            // 
            // SaveImport
            // 
            this.SaveImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveImport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SaveImport.Enabled = false;
            this.SaveImport.Image = global::ScaleAddon.Properties.Resources.icons8_microsoft_excel_check_30;
            this.SaveImport.Location = new System.Drawing.Point(1034, 4);
            this.SaveImport.Margin = new System.Windows.Forms.Padding(4);
            this.SaveImport.Name = "SaveImport";
            this.SaveImport.Size = new System.Drawing.Size(59, 54);
            this.SaveImport.TabIndex = 191;
            this.SaveImport.UseVisualStyleBackColor = true;
            this.SaveImport.Click += new System.EventHandler(this.SaveImport_Click);
            // 
            // cbosheet
            // 
            this.cbosheet.FormattingEnabled = true;
            this.cbosheet.Location = new System.Drawing.Point(718, 34);
            this.cbosheet.Name = "cbosheet";
            this.cbosheet.Size = new System.Drawing.Size(242, 24);
            this.cbosheet.TabIndex = 190;
            this.cbosheet.SelectedIndexChanged += new System.EventHandler(this.cbLot_SelectedIndexChanged);
            // 
            // textFilename
            // 
            this.textFilename.BackColor = System.Drawing.SystemColors.Window;
            this.textFilename.Location = new System.Drawing.Point(717, 5);
            this.textFilename.Margin = new System.Windows.Forms.Padding(4);
            this.textFilename.Name = "textFilename";
            this.textFilename.Size = new System.Drawing.Size(243, 22);
            this.textFilename.TabIndex = 188;
            // 
            // ImportButton
            // 
            this.ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ImportButton.Image = global::ScaleAddon.Properties.Resources.icons8_microsoft_excel_add_30;
            this.ImportButton.Location = new System.Drawing.Point(967, 4);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(59, 54);
            this.ImportButton.TabIndex = 189;
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // GenericINProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 720);
            this.Controls.Add(this.btnToogle);
            this.Controls.Add(this.tbLot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbNotes);
            this.Controls.Add(this.btnDelLot);
            this.Controls.Add(this.cbLot);
            this.Controls.Add(this.checkHold);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbBuyerName);
            this.Controls.Add(this.tbAcumaticaRefNbr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSaveLot);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbTotalCost);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.groupEntry);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbProcessDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.tbDocNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenericINProcess";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BirINProcess";
            this.Load += new System.EventHandler(this.GenericINProcess_Load);
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.groupEntry.ResumeLayout(false);
            this.groupEntry.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbTotalCost;
        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.GroupBox groupEntry;
        private System.Windows.Forms.TextBox tbEntryArea;
        private System.Windows.Forms.TextBox tbEntryGrade;
        private System.Windows.Forms.TextBox tbEntrySubItem;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox tbEntryWeightTare;
        internal System.Windows.Forms.Button btnSaveLot;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbEntryWeightNetto;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbEntryWeightReceive;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbEntryInv;
        private System.Windows.Forms.TextBox tbEntryLot;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbProcessDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAcumatica;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Button btnDelLot;
        private System.Windows.Forms.TextBox tbAcumaticaRefNbr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailWTare;
        private System.Windows.Forms.TextBox tbDetailWNetto;
        private System.Windows.Forms.TextBox tbDetailWShipping;
        private System.Windows.Forms.TextBox tbDetailWReceive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBuyerName;
        private System.Windows.Forms.CheckBox checkHold;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.TextBox tbEntryLotGroup;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbEntryDate;
        private System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Button btnPrintDoc;
        private System.Windows.Forms.TextBox tbLot;
        private System.Windows.Forms.Button btnToogle;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox tbDetailLot;
        internal System.Windows.Forms.Button btnPrintSUM;
        private System.Windows.Forms.ComboBox cbLot;
        internal System.Windows.Forms.Button unsyncing;
        internal System.Windows.Forms.Button SaveImport;
        private System.Windows.Forms.ComboBox cbosheet;
        private System.Windows.Forms.TextBox textFilename;
        internal System.Windows.Forms.Button ImportButton;
    }
}