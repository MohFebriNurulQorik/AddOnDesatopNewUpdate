
namespace ScaleAddon.Forms
{
    partial class GenericOUTProcesDonor
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
            this.label28 = new System.Windows.Forms.Label();
            this.tbUnappliedBalance = new System.Windows.Forms.TextBox();
            this.cbRefIN = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbMaterialIN = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(8, 68);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(121, 13);
            this.label28.TabIndex = 53;
            this.label28.Text = "Unapplied Balance (KG)";
            // 
            // tbUnappliedBalance
            // 
            this.tbUnappliedBalance.BackColor = System.Drawing.SystemColors.Info;
            this.tbUnappliedBalance.Location = new System.Drawing.Point(132, 65);
            this.tbUnappliedBalance.Name = "tbUnappliedBalance";
            this.tbUnappliedBalance.ReadOnly = true;
            this.tbUnappliedBalance.Size = new System.Drawing.Size(180, 20);
            this.tbUnappliedBalance.TabIndex = 52;
            this.tbUnappliedBalance.Text = "0";
            this.tbUnappliedBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbRefIN
            // 
            this.cbRefIN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRefIN.FormattingEnabled = true;
            this.cbRefIN.Location = new System.Drawing.Point(132, 12);
            this.cbRefIN.Name = "cbRefIN";
            this.cbRefIN.Size = new System.Drawing.Size(180, 21);
            this.cbRefIN.TabIndex = 51;
            this.cbRefIN.SelectedIndexChanged += new System.EventHandler(this.cbRefIN_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 13);
            this.label6.TabIndex = 50;
            this.label6.Text = "Reference IN Number";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Image = global::ScaleAddon.Properties.Resources.icons8_save_30;
            this.btnSave.Location = new System.Drawing.Point(325, 38);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(47, 47);
            this.btnSave.TabIndex = 54;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 56;
            this.label1.Text = "Material IN (KG)";
            // 
            // tbMaterialIN
            // 
            this.tbMaterialIN.BackColor = System.Drawing.SystemColors.Info;
            this.tbMaterialIN.Location = new System.Drawing.Point(132, 39);
            this.tbMaterialIN.Name = "tbMaterialIN";
            this.tbMaterialIN.ReadOnly = true;
            this.tbMaterialIN.Size = new System.Drawing.Size(180, 20);
            this.tbMaterialIN.TabIndex = 55;
            this.tbMaterialIN.Text = "0";
            this.tbMaterialIN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // GenericOUTProcesDonor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 93);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbMaterialIN);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.tbUnappliedBalance);
            this.Controls.Add(this.cbRefIN);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenericOUTProcesDonor";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GenericOUTProcesDonor";
            this.Load += new System.EventHandler(this.GenericOUTProcesDonor_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox tbUnappliedBalance;
        private System.Windows.Forms.ComboBox cbRefIN;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbMaterialIN;
    }
}