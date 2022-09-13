namespace ScaleAddon.Forms
{
    partial class SetupFiscal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupFiscal));
            this.label1 = new System.Windows.Forms.Label();
            this.cbChangeMonth = new System.Windows.Forms.ComboBox();
            this.tbMonth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbYear = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Fiscal Month";
            // 
            // cbChangeMonth
            // 
            this.cbChangeMonth.FormattingEnabled = true;
            this.cbChangeMonth.Items.AddRange(new object[] {
            "1 - January",
            "2 - February",
            "3 - March",
            "4 - April",
            "5 - May",
            "6 - June",
            "7 - July",
            "8 - August",
            "9 - September",
            "10 - October",
            "11 - November",
            "12 - December"});
            this.cbChangeMonth.Location = new System.Drawing.Point(143, 90);
            this.cbChangeMonth.Name = "cbChangeMonth";
            this.cbChangeMonth.Size = new System.Drawing.Size(162, 21);
            this.cbChangeMonth.TabIndex = 4;
            this.cbChangeMonth.SelectedIndexChanged += new System.EventHandler(this.cbChangeMonth_SelectedIndexChanged);
            // 
            // tbMonth
            // 
            this.tbMonth.BackColor = System.Drawing.SystemColors.Info;
            this.tbMonth.Location = new System.Drawing.Point(143, 38);
            this.tbMonth.Name = "tbMonth";
            this.tbMonth.ReadOnly = true;
            this.tbMonth.Size = new System.Drawing.Size(162, 20);
            this.tbMonth.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Change Fiscal Month";
            // 
            // tbYear
            // 
            this.tbYear.BackColor = System.Drawing.SystemColors.Info;
            this.tbYear.Location = new System.Drawing.Point(143, 64);
            this.tbYear.Name = "tbYear";
            this.tbYear.ReadOnly = true;
            this.tbYear.Size = new System.Drawing.Size(162, 20);
            this.tbYear.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Current Fiscal Year";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(230, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::ScaleAddon.Properties.Resources.icons8_refresh_16;
            this.btnRefresh.Location = new System.Drawing.Point(143, 117);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 22;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tbClientID
            // 
            this.tbClientID.BackColor = System.Drawing.SystemColors.Info;
            this.tbClientID.Location = new System.Drawing.Point(143, 12);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.ReadOnly = true;
            this.tbClientID.Size = new System.Drawing.Size(162, 20);
            this.tbClientID.TabIndex = 24;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Client ID";
            // 
            // SetupFiscal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 149);
            this.Controls.Add(this.tbClientID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.tbYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbMonth);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbChangeMonth);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupFiscal";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Fiscal";
            this.Load += new System.EventHandler(this.SetupFiscal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbChangeMonth;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbYear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.Label label4;
    }
}