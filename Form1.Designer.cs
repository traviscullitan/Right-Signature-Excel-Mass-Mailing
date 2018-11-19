namespace RightSignature
{
    partial class Form1
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
            this.TemplateComboBox = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sendContractsBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.emailSubjectBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TemplateComboBox
            // 
            this.TemplateComboBox.FormattingEnabled = true;
            this.TemplateComboBox.Location = new System.Drawing.Point(195, 30);
            this.TemplateComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TemplateComboBox.Name = "TemplateComboBox";
            this.TemplateComboBox.Size = new System.Drawing.Size(896, 24);
            this.TemplateComboBox.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(11, 144);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(1080, 222);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(195, 68);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(843, 22);
            this.textBox2.TabIndex = 2;
            // 
            // openFileBtn
            // 
            this.openFileBtn.Location = new System.Drawing.Point(1052, 58);
            this.openFileBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(38, 31);
            this.openFileBtn.TabIndex = 3;
            this.openFileBtn.Text = "...";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.openFileBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "RightSignature Template";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Excel Merge File";
            // 
            // sendContractsBtn
            // 
            this.sendContractsBtn.Location = new System.Drawing.Point(951, 94);
            this.sendContractsBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sendContractsBtn.Name = "sendContractsBtn";
            this.sendContractsBtn.Size = new System.Drawing.Size(140, 35);
            this.sendContractsBtn.TabIndex = 6;
            this.sendContractsBtn.Text = "Send Contracts";
            this.sendContractsBtn.UseVisualStyleBackColor = true;
            this.sendContractsBtn.Click += new System.EventHandler(this.sendContractsBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Email Subject Line";
            // 
            // emailSubjectBox
            // 
            this.emailSubjectBox.Location = new System.Drawing.Point(195, 108);
            this.emailSubjectBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.emailSubjectBox.Name = "emailSubjectBox";
            this.emailSubjectBox.Size = new System.Drawing.Size(398, 22);
            this.emailSubjectBox.TabIndex = 7;
            this.emailSubjectBox.Text = "Your Subject Line Here";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 374);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.emailSubjectBox);
            this.Controls.Add(this.sendContractsBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.openFileBtn);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.TemplateComboBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "SGS Send Contracts v1.0";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox TemplateComboBox;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sendContractsBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox emailSubjectBox;
    }
}