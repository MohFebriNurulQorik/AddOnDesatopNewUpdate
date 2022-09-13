namespace ScaleAddon.Forms
{
    partial class SetupAcumatica
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
            this.tbSiteURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbTenant = new System.Windows.Forms.TextBox();
            this.tbEndpoint = new System.Windows.Forms.TextBox();
            this.tbVersion = new System.Windows.Forms.TextBox();
            this.tbAPITest = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbInvLocation = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbSiteURL
            // 
            this.tbSiteURL.Location = new System.Drawing.Point(131, 38);
            this.tbSiteURL.Name = "tbSiteURL";
            this.tbSiteURL.Size = new System.Drawing.Size(375, 20);
            this.tbSiteURL.TabIndex = 0;
            this.tbSiteURL.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSiteURL_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Acumatica URL";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(131, 64);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(375, 20);
            this.tbUser.TabIndex = 1;
            this.tbUser.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbUser_KeyPress);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(131, 90);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(375, 20);
            this.tbPassword.TabIndex = 2;
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPassword_KeyPress);
            // 
            // tbTenant
            // 
            this.tbTenant.Location = new System.Drawing.Point(131, 116);
            this.tbTenant.Name = "tbTenant";
            this.tbTenant.Size = new System.Drawing.Size(375, 20);
            this.tbTenant.TabIndex = 3;
            this.tbTenant.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTenant_KeyPress);
            // 
            // tbEndpoint
            // 
            this.tbEndpoint.Location = new System.Drawing.Point(131, 142);
            this.tbEndpoint.Name = "tbEndpoint";
            this.tbEndpoint.Size = new System.Drawing.Size(375, 20);
            this.tbEndpoint.TabIndex = 4;
            this.tbEndpoint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbEndpoint_KeyPress);
            // 
            // tbVersion
            // 
            this.tbVersion.Location = new System.Drawing.Point(131, 168);
            this.tbVersion.Name = "tbVersion";
            this.tbVersion.Size = new System.Drawing.Size(375, 20);
            this.tbVersion.TabIndex = 5;
            this.tbVersion.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbVersion_KeyPress);
            // 
            // tbAPITest
            // 
            this.tbAPITest.Location = new System.Drawing.Point(131, 220);
            this.tbAPITest.Name = "tbAPITest";
            this.tbAPITest.ReadOnly = true;
            this.tbAPITest.Size = new System.Drawing.Size(375, 20);
            this.tbAPITest.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tenant";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Endpoint";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Endpoint Version";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 223);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "API Test";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_16;
            this.btnSave.Location = new System.Drawing.Point(431, 246);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnTest
            // 
            this.btnTest.Image = global::ScaleAddon.Properties.Resources.icons8_cloud_sync_16;
            this.btnTest.Location = new System.Drawing.Point(131, 246);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 15;
            this.btnTest.Text = "Test API";
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Client ID";
            // 
            // tbClientID
            // 
            this.tbClientID.BackColor = System.Drawing.SystemColors.Info;
            this.tbClientID.Location = new System.Drawing.Point(131, 12);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.ReadOnly = true;
            this.tbClientID.Size = new System.Drawing.Size(375, 20);
            this.tbClientID.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 197);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Inventory LocationID";
            // 
            // tbInvLocation
            // 
            this.tbInvLocation.Location = new System.Drawing.Point(131, 194);
            this.tbInvLocation.Name = "tbInvLocation";
            this.tbInvLocation.Size = new System.Drawing.Size(375, 20);
            this.tbInvLocation.TabIndex = 6;
            this.tbInvLocation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbInvLocation_KeyPress);
            // 
            // SetupAcumatica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 276);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbInvLocation);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbClientID);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbAPITest);
            this.Controls.Add(this.tbVersion);
            this.Controls.Add(this.tbEndpoint);
            this.Controls.Add(this.tbTenant);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSiteURL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupAcumatica";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Acumatica";
            this.Load += new System.EventHandler(this.setupAcumatica_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSiteURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbTenant;
        private System.Windows.Forms.TextBox tbEndpoint;
        private System.Windows.Forms.TextBox tbVersion;
        private System.Windows.Forms.TextBox tbAPITest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbInvLocation;
    }
}