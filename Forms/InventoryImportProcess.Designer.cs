
namespace ScaleAddon.Forms
{
    partial class InventoryImportProcess
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbWarehouse = new System.Windows.Forms.TextBox();
            this.tbProcessDate = new System.Windows.Forms.TextBox();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupEntry = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pbImport = new System.Windows.Forms.ProgressBar();
            this.tbExcelFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddEntry = new System.Windows.Forms.Button();
            this.btnSaveLot = new System.Windows.Forms.Button();
            this.openFileDialogExcel = new System.Windows.Forms.OpenFileDialog();
            this.groupDetail = new System.Windows.Forms.GroupBox();
            this.dgvDetail = new System.Windows.Forms.DataGridView();
            this.pnlDetailSummary = new System.Windows.Forms.Panel();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.tbDetailWTare = new System.Windows.Forms.TextBox();
            this.tbDetailWNetto = new System.Windows.Forms.TextBox();
            this.tbDetailWShipping = new System.Windows.Forms.TextBox();
            this.tbDetailWReceive = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupEntry.SuspendLayout();
            this.groupDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).BeginInit();
            this.pnlDetailSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1232, 73);
            this.panel1.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_30;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.Location = new System.Drawing.Point(99, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 65);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "New Doc";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbWarehouse
            // 
            this.tbWarehouse.BackColor = System.Drawing.SystemColors.Info;
            this.tbWarehouse.Location = new System.Drawing.Point(976, 87);
            this.tbWarehouse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbWarehouse.Name = "tbWarehouse";
            this.tbWarehouse.ReadOnly = true;
            this.tbWarehouse.Size = new System.Drawing.Size(239, 22);
            this.tbWarehouse.TabIndex = 99;
            // 
            // tbProcessDate
            // 
            this.tbProcessDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbProcessDate.Location = new System.Drawing.Point(584, 87);
            this.tbProcessDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbProcessDate.Name = "tbProcessDate";
            this.tbProcessDate.ReadOnly = true;
            this.tbProcessDate.Size = new System.Drawing.Size(239, 22);
            this.tbProcessDate.TabIndex = 98;
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Info;
            this.tbDocNumber.Location = new System.Drawing.Point(181, 87);
            this.tbDocNumber.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(239, 22);
            this.tbDocNumber.TabIndex = 95;
            this.tbDocNumber.Text = "<NEW>";
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 94;
            this.label1.Text = "Document Number";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(436, 91);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 17);
            this.label7.TabIndex = 102;
            this.label7.Text = "Process Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(832, 91);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 17);
            this.label8.TabIndex = 103;
            this.label8.Text = "Warehouse";
            // 
            // groupEntry
            // 
            this.groupEntry.Controls.Add(this.label4);
            this.groupEntry.Controls.Add(this.pbImport);
            this.groupEntry.Controls.Add(this.tbExcelFilename);
            this.groupEntry.Controls.Add(this.label2);
            this.groupEntry.Controls.Add(this.btnAddEntry);
            this.groupEntry.Controls.Add(this.btnSaveLot);
            this.groupEntry.Location = new System.Drawing.Point(16, 119);
            this.groupEntry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupEntry.Name = "groupEntry";
            this.groupEntry.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupEntry.Size = new System.Drawing.Size(1200, 85);
            this.groupEntry.TabIndex = 104;
            this.groupEntry.TabStop = false;
            this.groupEntry.Text = "Import Excel Document";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 60);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(108, 17);
            this.label4.TabIndex = 108;
            this.label4.Text = "Import Progress";
            // 
            // pbImport
            // 
            this.pbImport.Location = new System.Drawing.Point(165, 55);
            this.pbImport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbImport.Name = "pbImport";
            this.pbImport.Size = new System.Drawing.Size(893, 21);
            this.pbImport.TabIndex = 107;
            // 
            // tbExcelFilename
            // 
            this.tbExcelFilename.BackColor = System.Drawing.SystemColors.Info;
            this.tbExcelFilename.Location = new System.Drawing.Point(165, 23);
            this.tbExcelFilename.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbExcelFilename.Name = "tbExcelFilename";
            this.tbExcelFilename.ReadOnly = true;
            this.tbExcelFilename.Size = new System.Drawing.Size(892, 22);
            this.tbExcelFilename.TabIndex = 106;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 17);
            this.label2.TabIndex = 105;
            this.label2.Text = "Document Filename";
            // 
            // btnAddEntry
            // 
            this.btnAddEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddEntry.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAddEntry.Image = global::ScaleAddon.Properties.Resources.icons8_microsoft_excel_add_30;
            this.btnAddEntry.Location = new System.Drawing.Point(1067, 23);
            this.btnAddEntry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddEntry.Name = "btnAddEntry";
            this.btnAddEntry.Size = new System.Drawing.Size(59, 54);
            this.btnAddEntry.TabIndex = 81;
            this.btnAddEntry.UseVisualStyleBackColor = true;
            this.btnAddEntry.Click += new System.EventHandler(this.btnAddEntry_Click);
            // 
            // btnSaveLot
            // 
            this.btnSaveLot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveLot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSaveLot.Enabled = false;
            this.btnSaveLot.Image = global::ScaleAddon.Properties.Resources.icons8_microsoft_excel_check_30;
            this.btnSaveLot.Location = new System.Drawing.Point(1133, 23);
            this.btnSaveLot.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSaveLot.Name = "btnSaveLot";
            this.btnSaveLot.Size = new System.Drawing.Size(59, 54);
            this.btnSaveLot.TabIndex = 43;
            this.btnSaveLot.UseVisualStyleBackColor = true;
            this.btnSaveLot.Click += new System.EventHandler(this.btnSaveLot_Click);
            // 
            // openFileDialogExcel
            // 
            this.openFileDialogExcel.AddExtension = false;
            this.openFileDialogExcel.Filter = "\"Excel Files|*.xls;*.xlsx;\"";
            // 
            // groupDetail
            // 
            this.groupDetail.Controls.Add(this.dgvDetail);
            this.groupDetail.Controls.Add(this.pnlDetailSummary);
            this.groupDetail.Location = new System.Drawing.Point(20, 214);
            this.groupDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupDetail.Name = "groupDetail";
            this.groupDetail.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupDetail.Size = new System.Drawing.Size(1193, 488);
            this.groupDetail.TabIndex = 105;
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
            this.dgvDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvDetail.MultiSelect = false;
            this.dgvDetail.Name = "dgvDetail";
            this.dgvDetail.ReadOnly = true;
            this.dgvDetail.RowHeadersWidth = 51;
            this.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetail.Size = new System.Drawing.Size(1185, 426);
            this.dgvDetail.TabIndex = 1;
            // 
            // pnlDetailSummary
            // 
            this.pnlDetailSummary.Controls.Add(this.label34);
            this.pnlDetailSummary.Controls.Add(this.label33);
            this.pnlDetailSummary.Controls.Add(this.label32);
            this.pnlDetailSummary.Controls.Add(this.label29);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWTare);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWNetto);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWShipping);
            this.pnlDetailSummary.Controls.Add(this.tbDetailWReceive);
            this.pnlDetailSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetailSummary.Location = new System.Drawing.Point(4, 445);
            this.pnlDetailSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlDetailSummary.Name = "pnlDetailSummary";
            this.pnlDetailSummary.Size = new System.Drawing.Size(1185, 39);
            this.pnlDetailSummary.TabIndex = 2;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(680, 11);
            this.label34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(70, 17);
            this.label34.TabIndex = 92;
            this.label34.Text = "Receiving";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(869, 11);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(38, 17);
            this.label33.TabIndex = 91;
            this.label33.Text = "Tare";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(1029, 11);
            this.label32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(42, 17);
            this.label32.TabIndex = 90;
            this.label32.Text = "Netto";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(464, 11);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(99, 17);
            this.label29.TabIndex = 87;
            this.label29.Text = "Total Shipping";
            // 
            // tbDetailWTare
            // 
            this.tbDetailWTare.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWTare.Location = new System.Drawing.Point(921, 7);
            this.tbDetailWTare.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDetailWTare.Name = "tbDetailWTare";
            this.tbDetailWTare.ReadOnly = true;
            this.tbDetailWTare.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWTare.TabIndex = 85;
            this.tbDetailWTare.Text = "0";
            this.tbDetailWTare.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWNetto
            // 
            this.tbDetailWNetto.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWNetto.Location = new System.Drawing.Point(1081, 7);
            this.tbDetailWNetto.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDetailWNetto.Name = "tbDetailWNetto";
            this.tbDetailWNetto.ReadOnly = true;
            this.tbDetailWNetto.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWNetto.TabIndex = 86;
            this.tbDetailWNetto.Text = "0";
            this.tbDetailWNetto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWShipping
            // 
            this.tbDetailWShipping.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWShipping.Location = new System.Drawing.Point(572, 7);
            this.tbDetailWShipping.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDetailWShipping.Name = "tbDetailWShipping";
            this.tbDetailWShipping.ReadOnly = true;
            this.tbDetailWShipping.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWShipping.TabIndex = 83;
            this.tbDetailWShipping.Text = "0";
            this.tbDetailWShipping.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tbDetailWReceive
            // 
            this.tbDetailWReceive.BackColor = System.Drawing.SystemColors.Info;
            this.tbDetailWReceive.Location = new System.Drawing.Point(761, 7);
            this.tbDetailWReceive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbDetailWReceive.Name = "tbDetailWReceive";
            this.tbDetailWReceive.ReadOnly = true;
            this.tbDetailWReceive.Size = new System.Drawing.Size(99, 22);
            this.tbDetailWReceive.TabIndex = 84;
            this.tbDetailWReceive.Text = "0";
            this.tbDetailWReceive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // InventoryImportProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 715);
            this.Controls.Add(this.groupDetail);
            this.Controls.Add(this.groupEntry);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbWarehouse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbProcessDate);
            this.Controls.Add(this.tbDocNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InventoryImportProcess";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InventoryImportProcess";
            this.Load += new System.EventHandler(this.InventoryImportProcess_Load);
            this.panel1.ResumeLayout(false);
            this.groupEntry.ResumeLayout(false);
            this.groupEntry.PerformLayout();
            this.groupDetail.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetail)).EndInit();
            this.pnlDetailSummary.ResumeLayout(false);
            this.pnlDetailSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox tbWarehouse;
        private System.Windows.Forms.TextBox tbProcessDate;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupEntry;
        internal System.Windows.Forms.Button btnAddEntry;
        internal System.Windows.Forms.Button btnSaveLot;
        private System.Windows.Forms.TextBox tbExcelFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialogExcel;
        private System.Windows.Forms.GroupBox groupDetail;
        private System.Windows.Forms.DataGridView dgvDetail;
        private System.Windows.Forms.Panel pnlDetailSummary;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox tbDetailWTare;
        private System.Windows.Forms.TextBox tbDetailWNetto;
        private System.Windows.Forms.TextBox tbDetailWShipping;
        private System.Windows.Forms.TextBox tbDetailWReceive;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar pbImport;
    }
}