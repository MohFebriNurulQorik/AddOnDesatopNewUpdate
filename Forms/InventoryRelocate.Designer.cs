
namespace ScaleAddon.Forms
{
    partial class InventoryRelocate
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
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvEntry = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.btnRemoveEntry = new System.Windows.Forms.Button();
            this.btnSaveEntry = new System.Windows.Forms.Button();
            this.cbLot = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbLocation = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLot = new System.Windows.Forms.TextBox();
            this.btnToogle = new System.Windows.Forms.Button();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntry)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvEntry);
            this.groupDetail.Controls.Add(this.pnlDetailSummary);
            this.groupDetail.Location = new System.Drawing.Point(20, 73);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupDetail.Size = new System.Drawing.Size(1193, 601);
            this.groupDetail.TabIndex = 85;
            this.groupDetail.TabStop = false;
            this.groupDetail.Text = "Document Details";
            // 
            // dgvEntry
            // 
            this.dgvEntry.AllowUserToAddRows = false;
            this.dgvEntry.AllowUserToDeleteRows = false;
            this.dgvEntry.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEntry.Location = new System.Drawing.Point(4, 19);
            this.dgvEntry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvEntry.MultiSelect = false;
            this.dgvEntry.Name = "dgvEntry";
            this.dgvEntry.ReadOnly = true;
            this.dgvEntry.RowHeadersWidth = 51;
            this.dgvEntry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEntry.Size = new System.Drawing.Size(1185, 539);
            this.dgvEntry.TabIndex = 1;
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.btnRemoveEntry);
            this.pnlDetailSummary.Controls.Add(this.btnSaveEntry);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 558);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(1185, 39);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // btnRemoveEntry
            // 
            this.btnRemoveEntry.Image = global::ScaleAddon.Properties.Resources.icons8_delete_16;
            this.btnRemoveEntry.Location = new System.Drawing.Point(973, 4);
            this.btnRemoveEntry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRemoveEntry.Name = "btnRemoveEntry";
            this.btnRemoveEntry.Size = new System.Drawing.Size(100, 28);
            this.btnRemoveEntry.TabIndex = 95;
            this.btnRemoveEntry.Text = "Remove";
            this.btnRemoveEntry.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRemoveEntry.UseVisualStyleBackColor = true;
            this.btnRemoveEntry.Click += new System.EventHandler(this.btnRemoveEntry_Click);
            // 
            // btnSaveEntry
            // 
            this.btnSaveEntry.Image = global::ScaleAddon.Properties.Resources.icons8_save_16;
            this.btnSaveEntry.Location = new System.Drawing.Point(1081, 4);
            this.btnSaveEntry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveEntry.Name = "btnSaveEntry";
            this.btnSaveEntry.Size = new System.Drawing.Size(100, 28);
            this.btnSaveEntry.TabIndex = 94;
            this.btnSaveEntry.Text = "Save";
            this.btnSaveEntry.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSaveEntry.UseVisualStyleBackColor = true;
            this.btnSaveEntry.Click += new System.EventHandler(this.btnSaveEntry_Click);
            // 
            // cbLot
            // 
            this.cbLot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLot.FormattingEnabled = true;
            this.cbLot.Location = new System.Drawing.Point(152, 39);
            this.cbLot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLot.Name = "cbLot";
            this.cbLot.Size = new System.Drawing.Size(193, 24);
            this.cbLot.TabIndex = 84;
            this.cbLot.Visible = false;
            this.cbLot.SelectedIndexChanged += new System.EventHandler(this.cbLot_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 11);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 82;
            this.label8.Text = "Warehouse";
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(152, 7);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 81;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 43);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 17);
            this.label9.TabIndex = 80;
            this.label9.Text = "Tobacco Lot";
            // 
            // cbLocation
            // 
            this.cbLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLocation.FormattingEnabled = true;
            this.cbLocation.Location = new System.Drawing.Point(573, 7);
            this.cbLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLocation.Name = "cbLocation";
            this.cbLocation.Size = new System.Drawing.Size(239, 24);
            this.cbLocation.TabIndex = 87;
            this.cbLocation.SelectedIndexChanged += new System.EventHandler(this.cbLocation_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(437, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 86;
            this.label1.Text = "Location";
            // 
            // tbLot
            // 
            this.tbLot.BackColor = System.Drawing.SystemColors.Window;
            this.tbLot.Location = new System.Drawing.Point(152, 39);
            this.tbLot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbLot.Name = "tbLot";
            this.tbLot.Size = new System.Drawing.Size(193, 22);
            this.tbLot.TabIndex = 111;
            this.tbLot.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLot_KeyPress);
            // 
            // btnToogle
            // 
            this.btnToogle.Image = global::ScaleAddon.Properties.Resources.icons8_search_161;
            this.btnToogle.Location = new System.Drawing.Point(355, 37);
            this.btnToogle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnToogle.Name = "btnToogle";
            this.btnToogle.Size = new System.Drawing.Size(37, 28);
            this.btnToogle.TabIndex = 112;
            this.btnToogle.UseVisualStyleBackColor = true;
            this.btnToogle.Click += new System.EventHandler(this.btnToogle_Click);
            // 
            // InventoryRelocate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 692);
            this.Controls.Add(this.btnToogle);
            this.Controls.Add(this.tbLot);
            this.Controls.Add(this.cbLot);
            this.Controls.Add(this.cbLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventoryRelocate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InventoryRelocate";
            this.Load += new System.EventHandler(this.InventoryRelocate_Load);
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntry)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvEntry;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.ComboBox cbLot;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveEntry;
        private System.Windows.Forms.Button btnSaveEntry;
        private System.Windows.Forms.TextBox tbLot;
        private System.Windows.Forms.Button btnToogle;
    }
}