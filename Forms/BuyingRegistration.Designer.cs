namespace ScaleAddon.Forms
{
    partial class BuyingRegistration
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cbFarmer = new System.Windows.Forms.ComboBox();
            this.tbFarmer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbRegDoc = new System.Windows.Forms.TextBox();
            this.tbQueue = new System.Windows.Forms.TextBox();
            this.tbVendorDetails = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRegDate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbVendorClass = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvPO = new System.Windows.Forms.DataGridView();
            this.tbPO = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbEstWeight = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbEstLot = new System.Windows.Forms.TextBox();
            this.btnToogle = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.FarmPO = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPO)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1099, 74);
            this.panel1.TabIndex = 9;
            // 
            // btnPrint
            // 
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnPrint.Image = global::ScaleAddon.Properties.Resources.icons8_print_30;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPrint.Location = new System.Drawing.Point(115, 7);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(87, 65);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "Print Doc";
            this.btnPrint.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_30;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.Location = new System.Drawing.Point(20, 7);
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
            this.btnAdd.Enabled = false;
            this.btnAdd.Image = global::ScaleAddon.Properties.Resources.icons8_plus_30;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAdd.Location = new System.Drawing.Point(210, 7);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 65);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "New Doc";
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cbFarmer
            // 
            this.cbFarmer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFarmer.FormattingEnabled = true;
            this.cbFarmer.Location = new System.Drawing.Point(433, 111);
            this.cbFarmer.Margin = new System.Windows.Forms.Padding(4);
            this.cbFarmer.Name = "cbFarmer";
            this.cbFarmer.Size = new System.Drawing.Size(193, 24);
            this.cbFarmer.TabIndex = 21;
            this.cbFarmer.Visible = false;
            this.cbFarmer.SelectedIndexChanged += new System.EventHandler(this.cbFarmer_SelectedIndexChanged);
            // 
            // tbFarmer
            // 
            this.tbFarmer.BackColor = System.Drawing.SystemColors.Window;
            this.tbFarmer.Location = new System.Drawing.Point(434, 112);
            this.tbFarmer.Margin = new System.Windows.Forms.Padding(4);
            this.tbFarmer.Name = "tbFarmer";
            this.tbFarmer.Size = new System.Drawing.Size(194, 22);
            this.tbFarmer.TabIndex = 30;
            this.tbFarmer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFarmer_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(268, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Registration Number";
            // 
            // tbRegDoc
            // 
            this.tbRegDoc.BackColor = System.Drawing.SystemColors.Info;
            this.tbRegDoc.Location = new System.Drawing.Point(433, 80);
            this.tbRegDoc.Margin = new System.Windows.Forms.Padding(4);
            this.tbRegDoc.Name = "tbRegDoc";
            this.tbRegDoc.ReadOnly = true;
            this.tbRegDoc.Size = new System.Drawing.Size(239, 22);
            this.tbRegDoc.TabIndex = 11;
            this.tbRegDoc.Text = "<NEW>";
            // 
            // tbQueue
            // 
            this.tbQueue.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tbQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 56F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbQueue.Location = new System.Drawing.Point(20, 80);
            this.tbQueue.Margin = new System.Windows.Forms.Padding(4);
            this.tbQueue.Multiline = true;
            this.tbQueue.Name = "tbQueue";
            this.tbQueue.ReadOnly = true;
            this.tbQueue.Size = new System.Drawing.Size(243, 120);
            this.tbQueue.TabIndex = 12;
            this.tbQueue.Text = "9999";
            this.tbQueue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tbQueue.TextChanged += new System.EventHandler(this.tbQueue_TextChanged);
            // 
            // tbVendorDetails
            // 
            this.tbVendorDetails.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorDetails.Location = new System.Drawing.Point(433, 182);
            this.tbVendorDetails.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorDetails.Multiline = true;
            this.tbVendorDetails.Name = "tbVendorDetails";
            this.tbVendorDetails.ReadOnly = true;
            this.tbVendorDetails.Size = new System.Drawing.Size(652, 56);
            this.tbVendorDetails.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(268, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Farmer ID";
            // 
            // tbRegDate
            // 
            this.tbRegDate.BackColor = System.Drawing.SystemColors.Info;
            this.tbRegDate.Location = new System.Drawing.Point(847, 80);
            this.tbRegDate.Margin = new System.Windows.Forms.Padding(4);
            this.tbRegDate.Name = "tbRegDate";
            this.tbRegDate.ReadOnly = true;
            this.tbRegDate.Size = new System.Drawing.Size(239, 22);
            this.tbRegDate.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(681, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 16;
            this.label3.Text = "Registration Date";
            // 
            // tbVendorClass
            // 
            this.tbVendorClass.BackColor = System.Drawing.SystemColors.Info;
            this.tbVendorClass.Location = new System.Drawing.Point(847, 112);
            this.tbVendorClass.Margin = new System.Windows.Forms.Padding(4);
            this.tbVendorClass.Name = "tbVendorClass";
            this.tbVendorClass.ReadOnly = true;
            this.tbVendorClass.Size = new System.Drawing.Size(239, 22);
            this.tbVendorClass.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(681, 116);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 18;
            this.label4.Text = "Vendor Class";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(268, 186);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Farmer Details";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvPO);
            this.groupBox1.Location = new System.Drawing.Point(16, 292);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1071, 384);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Purchase Order (Based on Contract)";
            // 
            // dgvPO
            // 
            this.dgvPO.AllowUserToAddRows = false;
            this.dgvPO.AllowUserToDeleteRows = false;
            this.dgvPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPO.Location = new System.Drawing.Point(4, 19);
            this.dgvPO.Margin = new System.Windows.Forms.Padding(4);
            this.dgvPO.MultiSelect = false;
            this.dgvPO.Name = "dgvPO";
            this.dgvPO.ReadOnly = true;
            this.dgvPO.RowHeadersWidth = 51;
            this.dgvPO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPO.Size = new System.Drawing.Size(1063, 361);
            this.dgvPO.TabIndex = 0;
            this.dgvPO.SelectionChanged += new System.EventHandler(this.dgvPO_SelectionChanged);
            // 
            // tbPO
            // 
            this.tbPO.BackColor = System.Drawing.SystemColors.Info;
            this.tbPO.Location = new System.Drawing.Point(433, 246);
            this.tbPO.Margin = new System.Windows.Forms.Padding(4);
            this.tbPO.Name = "tbPO";
            this.tbPO.ReadOnly = true;
            this.tbPO.Size = new System.Drawing.Size(239, 22);
            this.tbPO.TabIndex = 24;
            this.tbPO.TextChanged += new System.EventHandler(this.tbPO_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(268, 250);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Purchase Order";
            // 
            // tbEstWeight
            // 
            this.tbEstWeight.BackColor = System.Drawing.SystemColors.Window;
            this.tbEstWeight.Location = new System.Drawing.Point(847, 246);
            this.tbEstWeight.Margin = new System.Windows.Forms.Padding(4);
            this.tbEstWeight.Name = "tbEstWeight";
            this.tbEstWeight.Size = new System.Drawing.Size(72, 22);
            this.tbEstWeight.TabIndex = 26;
            this.tbEstWeight.Text = "0";
            this.tbEstWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(681, 250);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 16);
            this.label7.TabIndex = 25;
            this.label7.Text = "Estimated Tobacco";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(928, 250);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 16);
            this.label8.TabIndex = 27;
            this.label8.Text = "KG";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1057, 250);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 16);
            this.label9.TabIndex = 29;
            this.label9.Text = "Lot";
            // 
            // tbEstLot
            // 
            this.tbEstLot.BackColor = System.Drawing.SystemColors.Window;
            this.tbEstLot.Location = new System.Drawing.Point(976, 246);
            this.tbEstLot.Margin = new System.Windows.Forms.Padding(4);
            this.tbEstLot.Name = "tbEstLot";
            this.tbEstLot.Size = new System.Drawing.Size(72, 22);
            this.tbEstLot.TabIndex = 28;
            this.tbEstLot.Text = "0";
            this.tbEstLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnToogle
            // 
            this.btnToogle.Image = global::ScaleAddon.Properties.Resources.icons8_search_161;
            this.btnToogle.Location = new System.Drawing.Point(636, 109);
            this.btnToogle.Margin = new System.Windows.Forms.Padding(4);
            this.btnToogle.Name = "btnToogle";
            this.btnToogle.Size = new System.Drawing.Size(37, 28);
            this.btnToogle.TabIndex = 31;
            this.btnToogle.UseVisualStyleBackColor = true;
            this.btnToogle.Click += new System.EventHandler(this.btnToogle_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(268, 149);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 16);
            this.label10.TabIndex = 32;
            this.label10.Text = "Farmer PO";
            // 
            // FarmPO
            // 
            this.FarmPO.BackColor = System.Drawing.SystemColors.Window;
            this.FarmPO.Location = new System.Drawing.Point(434, 146);
            this.FarmPO.Margin = new System.Windows.Forms.Padding(4);
            this.FarmPO.Name = "FarmPO";
            this.FarmPO.Size = new System.Drawing.Size(238, 22);
            this.FarmPO.TabIndex = 34;
            this.FarmPO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Find_PO);
            // 
            // BuyingRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 690);
            this.Controls.Add(this.FarmPO);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnToogle);
            this.Controls.Add(this.tbFarmer);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbFarmer);
            this.Controls.Add(this.tbEstLot);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbEstWeight);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbPO);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbVendorClass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbRegDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbVendorDetails);
            this.Controls.Add(this.tbQueue);
            this.Controls.Add(this.tbRegDoc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BuyingRegistration";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BuyingRegistration";
            this.Load += new System.EventHandler(this.BuyingRegistration_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnPrint;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbRegDoc;
        private System.Windows.Forms.TextBox tbQueue;
        private System.Windows.Forms.TextBox tbVendorDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRegDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbVendorClass;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvPO;
        private System.Windows.Forms.TextBox tbPO;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbEstWeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbEstLot;
        private System.Windows.Forms.TextBox tbFarmer;
        private System.Windows.Forms.ComboBox cbFarmer;
        private System.Windows.Forms.Button btnToogle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox FarmPO;
    }
}