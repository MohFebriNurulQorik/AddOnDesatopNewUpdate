namespace ScaleAddon.Controls
{
    partial class ucProcessingOUTGeneric
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
            this.label2 = new System.Windows.Forms.Label();
            this.dtpListDate2 = new System.Windows.Forms.DateTimePicker();
            this.btnBatchSync = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.dtpListDate = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.panel1.SuspendLayout();
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
            this.dgvList.Location = new System.Drawing.Point(0, 36);
            this.dgvList.Margin = new System.Windows.Forms.Padding(4);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.RowHeadersWidth = 51;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(1200, 702);
            this.dgvList.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpListDate2);
            this.panel1.Controls.Add(this.btnBatchSync);
            this.panel1.Controls.Add(this.btnView);
            this.panel1.Controls.Add(this.dtpListDate);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.tbFilter);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 36);
            this.panel1.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(695, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 20;
            this.label2.Text = "to";
            // 
            // dtpListDate2
            // 
            this.dtpListDate2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpListDate2.CustomFormat = "";
            this.dtpListDate2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpListDate2.Location = new System.Drawing.Point(553, 6);
            this.dtpListDate2.Margin = new System.Windows.Forms.Padding(4);
            this.dtpListDate2.Name = "dtpListDate2";
            this.dtpListDate2.Size = new System.Drawing.Size(132, 22);
            this.dtpListDate2.TabIndex = 19;
            this.dtpListDate2.ValueChanged += new System.EventHandler(this.dtpListDate2_ValueChanged);
            // 
            // btnBatchSync
            // 
            this.btnBatchSync.Enabled = false;
            this.btnBatchSync.Image = global::ScaleAddon.Properties.Resources.icons8_upload_to_the_cloud_16;
            this.btnBatchSync.Location = new System.Drawing.Point(233, 4);
            this.btnBatchSync.Margin = new System.Windows.Forms.Padding(4);
            this.btnBatchSync.Name = "btnBatchSync";
            this.btnBatchSync.Size = new System.Drawing.Size(107, 28);
            this.btnBatchSync.TabIndex = 16;
            this.btnBatchSync.Text = "Sync";
            this.btnBatchSync.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBatchSync.UseVisualStyleBackColor = true;
            this.btnBatchSync.Click += new System.EventHandler(this.btnBatchSync_Click);
            // 
            // btnView
            // 
            this.btnView.Image = global::ScaleAddon.Properties.Resources.icons8_search_16;
            this.btnView.Location = new System.Drawing.Point(119, 4);
            this.btnView.Margin = new System.Windows.Forms.Padding(4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(107, 28);
            this.btnView.TabIndex = 13;
            this.btnView.Text = "View";
            this.btnView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // dtpListDate
            // 
            this.dtpListDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpListDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpListDate.Location = new System.Drawing.Point(724, 6);
            this.dtpListDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpListDate.Name = "dtpListDate";
            this.dtpListDate.Size = new System.Drawing.Size(132, 22);
            this.dtpListDate.TabIndex = 12;
            this.dtpListDate.ValueChanged += new System.EventHandler(this.dtpListDate_ValueChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::ScaleAddon.Properties.Resources.icons8_plus_16;
            this.btnAdd.Location = new System.Drawing.Point(4, 4);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(107, 28);
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
            this.Label1.Location = new System.Drawing.Point(865, 10);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(39, 17);
            this.Label1.TabIndex = 8;
            this.Label1.Text = "Filter";
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(921, 6);
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
            this.btnRefresh.Location = new System.Drawing.Point(1089, 4);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(107, 28);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Search";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ucProcessingOUTGeneric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvList);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ucProcessingOUTGeneric";
            this.Size = new System.Drawing.Size(1200, 738);
            this.Load += new System.EventHandler(this.ucProcessingOUTGeneric_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnView;
        private System.Windows.Forms.DateTimePicker dtpListDate;
        internal System.Windows.Forms.Button btnAdd;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox tbFilter;
        internal System.Windows.Forms.Button btnRefresh;
        internal System.Windows.Forms.Button btnBatchSync;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpListDate2;
    }
}
