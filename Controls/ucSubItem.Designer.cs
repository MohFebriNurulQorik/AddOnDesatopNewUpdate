namespace ScaleAddon.Controls
{
    partial class ucSubItem
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
            this.dgvSubItem = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTruncate = new System.Windows.Forms.Button();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubItem)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvSubItem
            // 
            this.dgvSubItem.AllowUserToAddRows = false;
            this.dgvSubItem.AllowUserToDeleteRows = false;
            this.dgvSubItem.AllowUserToOrderColumns = true;
            this.dgvSubItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSubItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSubItem.Location = new System.Drawing.Point(0, 36);
            this.dgvSubItem.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvSubItem.MultiSelect = false;
            this.dgvSubItem.Name = "dgvSubItem";
            this.dgvSubItem.ReadOnly = true;
            this.dgvSubItem.RowHeadersWidth = 51;
            this.dgvSubItem.Size = new System.Drawing.Size(1200, 702);
            this.dgvSubItem.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnTruncate);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 36);
            this.panel1.TabIndex = 5;
            // 
            // btnTruncate
            // 
            this.btnTruncate.Image = global::ScaleAddon.Properties.Resources.icons8_erase_16;
            this.btnTruncate.Location = new System.Drawing.Point(4, 4);
            this.btnTruncate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.Size = new System.Drawing.Size(107, 28);
            this.btnTruncate.TabIndex = 11;
            this.btnTruncate.Text = "Clear";
            this.btnTruncate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTruncate.UseVisualStyleBackColor = true;
            this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
            // 
            // btnAcumatica
            // 
            this.btnAcumatica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcumatica.Image = global::ScaleAddon.Properties.Resources.icons8_cloud_sync_16;
            this.btnAcumatica.Location = new System.Drawing.Point(1089, 4);
            this.btnAcumatica.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAcumatica.Name = "btnAcumatica";
            this.btnAcumatica.Size = new System.Drawing.Size(107, 28);
            this.btnAcumatica.TabIndex = 11;
            this.btnAcumatica.Text = "Sync";
            this.btnAcumatica.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAcumatica.UseVisualStyleBackColor = true;
            this.btnAcumatica.Click += new System.EventHandler(this.btnAcumatica_Click);
            // 
            // Label1
            // 
            this.Label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(751, 10);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(39, 17);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "Filter";
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(807, 6);
            this.tbFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(159, 22);
            this.tbFilter.TabIndex = 7;
            this.tbFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbFilter_KeyDown);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_16;
            this.btnRefresh.Location = new System.Drawing.Point(975, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(107, 28);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Search";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ucSubItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvSubItem);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ucSubItem";
            this.Size = new System.Drawing.Size(1200, 738);
            this.Load += new System.EventHandler(this.ucSubItem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubItem)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSubItem;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAcumatica;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Button btnRefresh;
        internal System.Windows.Forms.Button btnTruncate;
    }
}
