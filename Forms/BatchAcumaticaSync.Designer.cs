
namespace ScaleAddon.Forms
{
    partial class BatchAcumaticaSync
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.pbSync = new System.Windows.Forms.ProgressBar();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAcumatica = new System.Windows.Forms.Button();
            this.groupList = new System.Windows.Forms.GroupBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label29 = new System.Windows.Forms.Label();
            this.tbListCount = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbInfo);
            this.panel1.Controls.Add(this.pbSync);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.btnAcumatica);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(734, 50);
            this.panel1.TabIndex = 11;
            // 
            // tbInfo
            // 
            this.tbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInfo.BackColor = System.Drawing.SystemColors.Info;
            this.tbInfo.Location = new System.Drawing.Point(53, 3);
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.Size = new System.Drawing.Size(628, 20);
            this.tbInfo.TabIndex = 88;
            this.tbInfo.Text = "Acumatica Batch Sync";
            this.tbInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pbSync
            // 
            this.pbSync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSync.Location = new System.Drawing.Point(53, 27);
            this.pbSync.Name = "pbSync";
            this.pbSync.Size = new System.Drawing.Size(628, 20);
            this.pbSync.TabIndex = 45;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUpdate.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_30;
            this.btnUpdate.Location = new System.Drawing.Point(3, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(44, 44);
            this.btnUpdate.TabIndex = 50;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAcumatica
            // 
            this.btnAcumatica.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAcumatica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAcumatica.Enabled = false;
            this.btnAcumatica.Image = global::ScaleAddon.Properties.Resources.icons8_upload_to_the_cloud_30;
            this.btnAcumatica.Location = new System.Drawing.Point(687, 3);
            this.btnAcumatica.Name = "btnAcumatica";
            this.btnAcumatica.Size = new System.Drawing.Size(44, 44);
            this.btnAcumatica.TabIndex = 15;
            this.btnAcumatica.UseVisualStyleBackColor = true;
            this.btnAcumatica.Click += new System.EventHandler(this.btnAcumatica_Click);
            // 
            // groupList
            // 
            this.groupList.Controls.Add(this.dgvList);
            this.groupList.Controls.Add(this.pnlDetailSummary);
            this.groupList.Location = new System.Drawing.Point(3, 56);
            this.groupList.Name = "groupList";
            this.groupList.Size = new System.Drawing.Size(731, 393);
            this.groupList.TabIndex = 44;
            this.groupList.TabStop = false;
            this.groupList.Text = "Document List";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvList.Location = new System.Drawing.Point(3, 16);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvList.Size = new System.Drawing.Size(725, 342);
            this.dgvList.TabIndex = 1;
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbListCount);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(3, 358);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(725, 32);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(555, 9);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(83, 13);
            this.label29.TabIndex = 87;
            this.label29.Text = "Total Document";
            // 
            // tbListCount
            // 
            this.tbListCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbListCount.BackColor = System.Drawing.SystemColors.Info;
            this.tbListCount.Location = new System.Drawing.Point(644, 6);
            this.tbListCount.Name = "tbListCount";
            this.tbListCount.ReadOnly = true;
            this.tbListCount.Size = new System.Drawing.Size(75, 20);
            this.tbListCount.TabIndex = 86;
            this.tbListCount.Text = "0";
            this.tbListCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // BatchAcumaticaSync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 461);
            this.Controls.Add(this.groupList);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BatchAcumaticaSync";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BatchAcumaticaSync";
            this.Load += new System.EventHandler(this.BatchAcumaticaSync_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnAcumatica;
        internal System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupList;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbListCount;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.ProgressBar pbSync;
    }
}