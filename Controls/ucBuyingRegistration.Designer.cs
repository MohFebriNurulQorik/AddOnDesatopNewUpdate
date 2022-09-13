namespace ScaleAddon.Controls
{
    partial class ucBuyingRegistration
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
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnView = new System.Windows.Forms.Button();
            this.dtpListDate = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDetailLot = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.tbDetailReg = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailWeight = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AllowUserToOrderColumns = true;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(0, 29);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(900, 539);
            this.dgvList.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dtpListDate);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 29);
            this.panel1.TabIndex = 7;
            // 
            // btnView
            // 
            this.btnView.Image = global::ScaleAddon.Properties.Resources.icons8_search_16;
            this.btnView.Location = new System.Drawing.Point(89, 3);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(80, 23);
            this.btnView.TabIndex = 13;
            this.btnView.Text = "View";
            this.btnView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dtpListDate
            // 
            this.dtpListDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpListDate.Location = new System.Drawing.Point(443, 5);
            this.dtpListDate.Name = "dtpListDate";
            this.dtpListDate.Size = new System.Drawing.Size(200, 20);
            this.dtpListDate.TabIndex = 12;
            this.dtpListDate.ValueChanged += new System.EventHandler(this.dtpRegDate_ValueChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::ScaleAddon.Properties.Resources.icons8_plus_16;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "Add New";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
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
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label2);
            this.pnlDetailSummary.Controls.Add(this.tbDetailLot);
            this.pnlDetailSummary.Controls.Add(this.label31);
            this.pnlDetailSummary.Controls.Add(this.tbDetailReg);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWeight);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(0, 568);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(900, 32);
            this.pnlDetailSummary.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(549, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 91;
            this.label2.Text = "Estimated Lot";
            // 
            // tbDetailLot
            // 
            this.tbDetailLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailLot.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailLot.Location = new System.Drawing.Point(645, 3);
            this.tbDetailLot.Name = "tbDetailLot";
            this.tbDetailLot.ReadOnly = true;
            this.tbDetailLot.Size = new System.Drawing.Size(75, 20);
            this.tbDetailLot.TabIndex = 90;
            this.tbDetailLot.Text = "0";
            this.tbDetailLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(388, 6);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(54, 13);
            this.label31.TabIndex = 89;
            this.label31.Text = "Total Reg";
            // 
            // tbDetailReg
            // 
            this.tbDetailReg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailReg.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailReg.Location = new System.Drawing.Point(443, 3);
            this.tbDetailReg.Name = "tbDetailReg";
            this.tbDetailReg.ReadOnly = true;
            this.tbDetailReg.Size = new System.Drawing.Size(75, 20);
            this.tbDetailReg.TabIndex = 88;
            this.tbDetailReg.Text = "0";
            this.tbDetailReg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(726, 6);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(90, 13);
            this.label29.TabIndex = 87;
            this.label29.Text = "Estimated Weight";
            // 
            // tbDetailWeight
            // 
            this.tbDetailWeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailWeight.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWeight.Location = new System.Drawing.Point(822, 3);
            this.tbDetailWeight.Name = "tbDetailWeight";
            this.tbDetailWeight.ReadOnly = true;
            this.tbDetailWeight.Size = new System.Drawing.Size(75, 20);
            this.tbDetailWeight.TabIndex = 83;
            this.tbDetailWeight.Text = "0";
            this.tbDetailWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ucBuyingRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.pnlDetailSummary);
            this.Controls.Add(this.panel1);
            this.Name = "ucBuyingRegistration";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.ucBuyingRegistration_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DateTimePicker dtpListDate;
        internal System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox tbDetailReg;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDetailLot;
    }
}
