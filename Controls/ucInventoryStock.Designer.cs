namespace ScaleAddon.Controls
{
    partial class ucInventoryStock
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
            this.dgvItem = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbProcess = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.tbDetailLot = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailWTare = new System.Windows.Forms.TextBox();
            this.tbDetailWNetto = new System.Windows.Forms.TextBox();
            this.tbDetailWShipping = new System.Windows.Forms.TextBox();
            this.tbDetailWReceive = new System.Windows.Forms.TextBox();
            this.btnPrintLot = new System.Windows.Forms.Button();
            this.tnRelocate = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItem
            // 
            this.dgvItem.AllowUserToAddRows = false;
            this.dgvItem.AllowUserToDeleteRows = false;
            this.dgvItem.AllowUserToOrderColumns = true;
            this.dgvItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItem.Location = new System.Drawing.Point(0, 29);
            this.dgvItem.MultiSelect = false;
            this.dgvItem.Name = "dgvItem";
            this.dgvItem.ReadOnly = true;
            this.dgvItem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItem.Size = new System.Drawing.Size(900, 571);
            this.dgvItem.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tnRelocate);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbCategory);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbProcess);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 29);
            this.panel1.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Category";
            // 
            // cbCategory
            // 
            this.cbCategory.FormattingEnabled = true;
            this.cbCategory.Items.AddRange(new object[] {
            "Grade",
            "Stage",
            "Form",
            "Color",
            "Fermentation",
            "Length",
            "StalkPosition",
            "LotNbr",
            "DocumentID",
            "InventoryID",
            "LocationInfo"});
            this.cbCategory.Location = new System.Drawing.Point(236, 5);
            this.cbCategory.Name = "cbCategory";
            this.cbCategory.Size = new System.Drawing.Size(121, 21);
            this.cbCategory.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Process";
            // 
            // cbProcess
            // 
            this.cbProcess.FormattingEnabled = true;
            this.cbProcess.Location = new System.Drawing.Point(54, 5);
            this.cbProcess.Name = "cbProcess";
            this.cbProcess.Size = new System.Drawing.Size(121, 21);
            this.cbProcess.TabIndex = 9;
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(649, 8);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(29, 13);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "Filter";
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(691, 5);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(120, 20);
            this.tbFilter.TabIndex = 7;
            this.tbFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter_KeyDown);
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.btnPrintLot);
            this.pnlDetailSummary.Controls.Add(this.label34);
            this.pnlDetailSummary.Controls.Add(this.label33);
            this.pnlDetailSummary.Controls.Add(this.label32);
            this.pnlDetailSummary.Controls.Add(this.label31);
            this.pnlDetailSummary.Controls.Add(this.tbDetailLot);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWTare);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWNetto);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWShipping);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWReceive);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(0, 568);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(900, 32);
            this.pnlDetailSummary.TabIndex = 7;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(518, 6);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(55, 13);
            this.label34.TabIndex = 92;
            this.label34.Text = "Receiving";
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(660, 6);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(29, 13);
            this.label33.TabIndex = 91;
            this.label33.Text = "Tare";
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(780, 6);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(33, 13);
            this.label32.TabIndex = 90;
            this.label32.Text = "Netto";
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(220, 6);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(49, 13);
            this.label31.TabIndex = 89;
            this.label31.Text = "Total Lot";
            // 
            // tbDetailLot
            // 
            this.tbDetailLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailLot.Location = new System.Drawing.Point(275, 3);
            this.tbDetailLot.Name = "tbDetailLot";
            this.tbDetailLot.ReadOnly = true;
            this.tbDetailLot.Size = new System.Drawing.Size(75, 20);
            this.tbDetailLot.TabIndex = 88;
            this.tbDetailLot.Text = "0";
            this.tbDetailLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(356, 6);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(75, 13);
            this.label29.TabIndex = 87;
            this.label29.Text = "Total Shipping";
            // 
            // tbDetailWTare
            // 
            this.tbDetailWTare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWTare.Location = new System.Drawing.Point(699, 3);
            this.tbDetailWTare.Name = "tbDetailWTare";
            this.tbDetailWTare.ReadOnly = true;
            this.tbDetailWTare.Size = new System.Drawing.Size(75, 20);
            this.tbDetailWTare.TabIndex = 85;
            this.tbDetailWTare.Text = "0";
            this.tbDetailWTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWNetto
            // 
            this.tbDetailWNetto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWNetto.Location = new System.Drawing.Point(822, 3);
            this.tbDetailWNetto.Name = "tbDetailWNetto";
            this.tbDetailWNetto.ReadOnly = true;
            this.tbDetailWNetto.Size = new System.Drawing.Size(75, 20);
            this.tbDetailWNetto.TabIndex = 86;
            this.tbDetailWNetto.Text = "0";
            this.tbDetailWNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWShipping
            // 
            this.tbDetailWShipping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWShipping.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWShipping.Location = new System.Drawing.Point(437, 3);
            this.tbDetailWShipping.Name = "tbDetailWShipping";
            this.tbDetailWShipping.ReadOnly = true;
            this.tbDetailWShipping.Size = new System.Drawing.Size(75, 20);
            this.tbDetailWShipping.TabIndex = 83;
            this.tbDetailWShipping.Text = "0";
            this.tbDetailWShipping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWReceive
            // 
            this.tbDetailWReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWReceive.Location = new System.Drawing.Point(579, 3);
            this.tbDetailWReceive.Name = "tbDetailWReceive";
            this.tbDetailWReceive.ReadOnly = true;
            this.tbDetailWReceive.Size = new System.Drawing.Size(75, 20);
            this.tbDetailWReceive.TabIndex = 84;
            this.tbDetailWReceive.Text = "0";
            this.tbDetailWReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnPrintLot
            // 
            this.btnPrintLot.Image = global::ScaleAddon.Properties.Resources.icons8_print_16;
            this.btnPrintLot.Location = new System.Drawing.Point(3, 1);
            this.btnPrintLot.Name = "btnPrintLot";
            this.btnPrintLot.Size = new System.Drawing.Size(75, 23);
            this.btnPrintLot.TabIndex = 95;
            this.btnPrintLot.Text = "Print";
            this.btnPrintLot.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPrintLot.UseVisualStyleBackColor = true;
            this.btnPrintLot.Click += new System.EventHandler(this.btnPrintLot_Click);
            // 
            // tnRelocate
            // 
            this.tnRelocate.Image = global::ScaleAddon.Properties.Resources.icons8_box_move_16;
            this.tnRelocate.Location = new System.Drawing.Point(363, 3);
            this.tnRelocate.Name = "tnRelocate";
            this.tnRelocate.Size = new System.Drawing.Size(75, 23);
            this.tnRelocate.TabIndex = 96;
            this.tnRelocate.Text = "Relocate";
            this.tnRelocate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tnRelocate.UseVisualStyleBackColor = true;
            this.tnRelocate.Click += new System.EventHandler(this.tnRelocate_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_16;
            this.btnRefresh.Location = new System.Drawing.Point(817, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Search";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ucInventoryStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDetailSummary);
            this.Controls.Add(this.dgvItem);
            this.Controls.Add(this.panel1);
            this.Name = "ucInventoryStock";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.ucInventoryStock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItem)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvItem;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Button btnRefresh;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbProcess;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox tbDetailLot;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailWTare;
        private System.Windows.Forms.TextBox tbDetailWNetto;
        private System.Windows.Forms.TextBox tbDetailWShipping;
        private System.Windows.Forms.TextBox tbDetailWReceive;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbCategory;
        private System.Windows.Forms.Button btnPrintLot;
        private System.Windows.Forms.Button tnRelocate;
    }
}
