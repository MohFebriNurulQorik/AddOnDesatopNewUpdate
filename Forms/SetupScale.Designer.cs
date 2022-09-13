namespace ScaleAddon.Forms
{
    partial class SetupScale
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetupScale));
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbBaudrate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbParity = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDatabits = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbStopbits = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbTerminator = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPostfix = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.checkManual = new System.Windows.Forms.CheckBox();
            this.tbClientID = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(96, 38);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(209, 20);
            this.tbPort.TabIndex = 1;
            // 
            // tbBaudrate
            // 
            this.tbBaudrate.Location = new System.Drawing.Point(96, 64);
            this.tbBaudrate.Name = "tbBaudrate";
            this.tbBaudrate.Size = new System.Drawing.Size(209, 20);
            this.tbBaudrate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Baud rate";
            // 
            // tbParity
            // 
            this.tbParity.Location = new System.Drawing.Point(96, 90);
            this.tbParity.Name = "tbParity";
            this.tbParity.Size = new System.Drawing.Size(209, 20);
            this.tbParity.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Parity";
            // 
            // tbDatabits
            // 
            this.tbDatabits.Location = new System.Drawing.Point(96, 116);
            this.tbDatabits.Name = "tbDatabits";
            this.tbDatabits.Size = new System.Drawing.Size(209, 20);
            this.tbDatabits.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 119);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Data bits";
            // 
            // tbStopbits
            // 
            this.tbStopbits.Location = new System.Drawing.Point(96, 142);
            this.tbStopbits.Name = "tbStopbits";
            this.tbStopbits.Size = new System.Drawing.Size(209, 20);
            this.tbStopbits.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 145);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Stop bits";
            // 
            // tbTerminator
            // 
            this.tbTerminator.Location = new System.Drawing.Point(96, 168);
            this.tbTerminator.Name = "tbTerminator";
            this.tbTerminator.Size = new System.Drawing.Size(209, 20);
            this.tbTerminator.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Terminator";
            // 
            // tbPrefix
            // 
            this.tbPrefix.Location = new System.Drawing.Point(96, 194);
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.Size = new System.Drawing.Size(209, 20);
            this.tbPrefix.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 197);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Stable Prefix";
            // 
            // tbPostfix
            // 
            this.tbPostfix.Location = new System.Drawing.Point(96, 220);
            this.tbPostfix.Name = "tbPostfix";
            this.tbPostfix.Size = new System.Drawing.Size(209, 20);
            this.tbPostfix.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Postfix";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(230, 295);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbOutput
            // 
            this.tbOutput.BackColor = System.Drawing.SystemColors.Info;
            this.tbOutput.Location = new System.Drawing.Point(96, 269);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(209, 20);
            this.tbOutput.TabIndex = 17;
            this.tbOutput.Text = "No Connection";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 272);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Test Config";
            // 
            // btnTest
            // 
            this.btnTest.Image = global::ScaleAddon.Properties.Resources.icons8_scales_16;
            this.btnTest.Location = new System.Drawing.Point(96, 295);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 21;
            this.btnTest.Text = "Test";
            this.btnTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 247);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "Manual Override";
            // 
            // checkManual
            // 
            this.checkManual.AutoSize = true;
            this.checkManual.Location = new System.Drawing.Point(103, 246);
            this.checkManual.Name = "checkManual";
            this.checkManual.Size = new System.Drawing.Size(59, 17);
            this.checkManual.TabIndex = 23;
            this.checkManual.Text = "Enable";
            this.checkManual.UseVisualStyleBackColor = true;
            // 
            // tbClientID
            // 
            this.tbClientID.BackColor = System.Drawing.SystemColors.Info;
            this.tbClientID.Location = new System.Drawing.Point(96, 12);
            this.tbClientID.Name = "tbClientID";
            this.tbClientID.Size = new System.Drawing.Size(209, 20);
            this.tbClientID.TabIndex = 25;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(47, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Client ID";
            // 
            // SetupScale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 327);
            this.Controls.Add(this.tbClientID);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.checkManual);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tbPostfix);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbPrefix);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbTerminator);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbStopbits);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDatabits);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbParity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbBaudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupScale";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Setup Scale Communication";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetupScale_FormClosing);
            this.Load += new System.EventHandler(this.SetupScale_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.TextBox tbBaudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbParity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDatabits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbStopbits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbTerminator;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbPostfix;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkManual;
        private System.Windows.Forms.TextBox tbClientID;
        private System.Windows.Forms.Label label11;
    }
}