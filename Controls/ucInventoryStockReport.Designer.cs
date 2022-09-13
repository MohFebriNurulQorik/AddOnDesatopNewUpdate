namespace ScaleAddon.Controls
{
    partial class ucInventoryStockReport
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
            this.dtpListDateEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpListDateStart = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailNettOUT = new System.Windows.Forms.TextBox();
            this.tbDetailEndBal = new System.Windows.Forms.TextBox();
            this.tbDetailBegBal = new System.Windows.Forms.TextBox();
            this.tbDetailNettIN = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
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
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpListDateEnd);
            this.panel1.Controls.Add(this.dtpListDateStart);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(900, 29);
            this.panel1.TabIndex = 5;
            // 
            // dtpListDateEnd
            // 
            this.dtpListDateEnd.Location = new System.Drawing.Point(328, 5);
            this.dtpListDateEnd.Name = "dtpListDateEnd";
            this.dtpListDateEnd.Size = new System.Drawing.Size(200, 20);
            this.dtpListDateEnd.TabIndex = 14;
            // 
            // dtpListDateStart
            // 
            this.dtpListDateStart.Location = new System.Drawing.Point(64, 5);
            this.dtpListDateStart.Name = "dtpListDateStart";
            this.dtpListDateStart.Size = new System.Drawing.Size(200, 20);
            this.dtpListDateStart.TabIndex = 13;
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
            this.pnlDetailSummary.Controls.Add(this.label34);
            this.pnlDetailSummary.Controls.Add(this.label33);
            this.pnlDetailSummary.Controls.Add(this.label32);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailNettOUT);
            this.pnlDetailSummary.Controls.Add(this.tbDetailEndBal);
            this.pnlDetailSummary.Controls.Add(this.tbDetailBegBal);
            this.pnlDetailSummary.Controls.Add(this.tbDetailNettIN);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(0, 568);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(900, 32);
            this.pnlDetailSummary.TabIndex = 7;
            this.pnlDetailSummary.Visible = false;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(449, 6);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(47, 13);
            this.label34.TabIndex = 92;
            this.label34.Text = "Netto IN";
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(583, 6);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(59, 13);
            this.label33.TabIndex = 91;
            this.label33.Text = "Netto OUT";
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(729, 6);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(82, 13);
            this.label32.TabIndex = 90;
            this.label32.Text = "Ending Balance";
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(266, 6);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(96, 13);
            this.label29.TabIndex = 87;
            this.label29.Text = "Beginning Balance";
            // 
            // tbDetailNettOUT
            // 
            this.tbDetailNettOUT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailNettOUT.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailNettOUT.Location = new System.Drawing.Point(648, 3);
            this.tbDetailNettOUT.Name = "tbDetailNettOUT";
            this.tbDetailNettOUT.ReadOnly = true;
            this.tbDetailNettOUT.Size = new System.Drawing.Size(75, 20);
            this.tbDetailNettOUT.TabIndex = 85;
            this.tbDetailNettOUT.Text = "0";
            this.tbDetailNettOUT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailEndBal
            // 
            this.tbDetailEndBal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailEndBal.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailEndBal.Location = new System.Drawing.Point(822, 3);
            this.tbDetailEndBal.Name = "tbDetailEndBal";
            this.tbDetailEndBal.ReadOnly = true;
            this.tbDetailEndBal.Size = new System.Drawing.Size(75, 20);
            this.tbDetailEndBal.TabIndex = 86;
            this.tbDetailEndBal.Text = "0";
            this.tbDetailEndBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailBegBal
            // 
            this.tbDetailBegBal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailBegBal.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailBegBal.Location = new System.Drawing.Point(368, 3);
            this.tbDetailBegBal.Name = "tbDetailBegBal";
            this.tbDetailBegBal.ReadOnly = true;
            this.tbDetailBegBal.Size = new System.Drawing.Size(75, 20);
            this.tbDetailBegBal.TabIndex = 83;
            this.tbDetailBegBal.Text = "0";
            this.tbDetailBegBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailNettIN
            // 
            this.tbDetailNettIN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDetailNettIN.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailNettIN.Location = new System.Drawing.Point(502, 3);
            this.tbDetailNettIN.Name = "tbDetailNettIN";
            this.tbDetailNettIN.ReadOnly = true;
            this.tbDetailNettIN.Size = new System.Drawing.Size(75, 20);
            this.tbDetailNettIN.TabIndex = 84;
            this.tbDetailNettIN.Text = "0";
            this.tbDetailNettIN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Start Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "End Date";
            // 
            // ucInventoryStockReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlDetailSummary);
            this.Controls.Add(this.dgvItem);
            this.Controls.Add(this.panel1);
            this.Name = "ucInventoryStockReport";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.ucInventoryStockReport_Load);
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
        internal System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailNettOUT;
        private System.Windows.Forms.TextBox tbDetailEndBal;
        private System.Windows.Forms.TextBox tbDetailBegBal;
        private System.Windows.Forms.TextBox tbDetailNettIN;
        private System.Windows.Forms.DateTimePicker dtpListDateEnd;
        private System.Windows.Forms.DateTimePicker dtpListDateStart;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label2;
    }
}
