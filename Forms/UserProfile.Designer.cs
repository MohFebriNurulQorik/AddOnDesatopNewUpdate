namespace ScaleAddon.Forms
{
    partial class UserProfile
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
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbNewPasswordRepeat = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.tbOldPassword = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.tbUserType = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.tbFullName = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.btnSave);
            this.GroupBox1.Controls.Add(this.tbNewPasswordRepeat);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.tbNewPassword);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.tbOldPassword);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Location = new System.Drawing.Point(8, 103);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GroupBox1.Size = new System.Drawing.Size(459, 159);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Change Password";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_16;
            this.btnSave.Location = new System.Drawing.Point(351, 123);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbNewPasswordRepeat
            // 
            this.tbNewPasswordRepeat.Location = new System.Drawing.Point(153, 87);
            this.tbNewPasswordRepeat.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbNewPasswordRepeat.Name = "tbNewPasswordRepeat";
            this.tbNewPasswordRepeat.PasswordChar = '*';
            this.tbNewPasswordRepeat.Size = new System.Drawing.Size(296, 22);
            this.tbNewPasswordRepeat.TabIndex = 2;
            this.tbNewPasswordRepeat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNewPasswordRepeat_KeyPress);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(8, 91);
            this.Label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(119, 17);
            this.Label6.TabIndex = 6;
            this.Label6.Text = "Repeat Password";
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.Location = new System.Drawing.Point(153, 55);
            this.tbNewPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.PasswordChar = '*';
            this.tbNewPassword.Size = new System.Drawing.Size(296, 22);
            this.tbNewPassword.TabIndex = 1;
            this.tbNewPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNewPassword_KeyPress);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(8, 59);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(100, 17);
            this.Label5.TabIndex = 4;
            this.Label5.Text = "New Password";
            // 
            // tbOldPassword
            // 
            this.tbOldPassword.Location = new System.Drawing.Point(153, 23);
            this.tbOldPassword.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.PasswordChar = '*';
            this.tbOldPassword.Size = new System.Drawing.Size(296, 22);
            this.tbOldPassword.TabIndex = 0;
            this.tbOldPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbOldPassword_KeyPress);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(8, 27);
            this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(95, 17);
            this.Label4.TabIndex = 2;
            this.Label4.Text = "Old Password";
            // 
            // tbUserType
            // 
            this.tbUserType.Location = new System.Drawing.Point(161, 71);
            this.tbUserType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUserType.Name = "tbUserType";
            this.tbUserType.ReadOnly = true;
            this.tbUserType.Size = new System.Drawing.Size(304, 22);
            this.tbUserType.TabIndex = 12;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(16, 75);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(87, 17);
            this.Label3.TabIndex = 11;
            this.Label3.Text = "User Access";
            // 
            // tbFullName
            // 
            this.tbFullName.Location = new System.Drawing.Point(161, 39);
            this.tbFullName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbFullName.Name = "tbFullName";
            this.tbFullName.ReadOnly = true;
            this.tbFullName.Size = new System.Drawing.Size(304, 22);
            this.tbFullName.TabIndex = 10;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(16, 43);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(71, 17);
            this.Label2.TabIndex = 9;
            this.Label2.Text = "Full Name";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(161, 7);
            this.tbUsername.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.ReadOnly = true;
            this.tbUsername.Size = new System.Drawing.Size(304, 22);
            this.tbUsername.TabIndex = 8;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(16, 11);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(79, 17);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "User Name";
            // 
            // UserProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 268);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.tbUserType);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.tbFullName);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.tbUsername);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserProfile";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Profile";
            this.Load += new System.EventHandler(this.UserProfile_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.TextBox tbNewPasswordRepeat;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox tbNewPassword;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox tbOldPassword;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox tbUserType;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox tbFullName;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox tbUsername;
        internal System.Windows.Forms.Label Label1;
    }
}