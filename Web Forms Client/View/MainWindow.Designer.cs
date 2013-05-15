namespace Web_Forms_Client
{
    partial class MainWindow
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
            this.ChancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.FromBox = new System.Windows.Forms.ComboBox();
            this.ToBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ChancelButton
            // 
            this.ChancelButton.Location = new System.Drawing.Point(80, 143);
            this.ChancelButton.Name = "ChancelButton";
            this.ChancelButton.Size = new System.Drawing.Size(75, 23);
            this.ChancelButton.TabIndex = 0;
            this.ChancelButton.Text = "Cancel";
            this.ChancelButton.UseVisualStyleBackColor = true;
            this.ChancelButton.Click += new System.EventHandler(this.ChancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(174, 143);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // FromBox
            // 
            this.FromBox.FormattingEnabled = true;
            this.FromBox.Location = new System.Drawing.Point(13, 45);
            this.FromBox.Name = "FromBox";
            this.FromBox.Size = new System.Drawing.Size(217, 21);
            this.FromBox.TabIndex = 2;
            // 
            // ToBox
            // 
            this.ToBox.FormattingEnabled = true;
            this.ToBox.Location = new System.Drawing.Point(13, 103);
            this.ToBox.Name = "ToBox";
            this.ToBox.Size = new System.Drawing.Size(217, 21);
            this.ToBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Fra";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Til";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 184);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ToBox);
            this.Controls.Add(this.FromBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.ChancelButton);
            this.Name = "MainWindow";
            this.Text = "Søg Rejse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ChancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox FromBox;
        private System.Windows.Forms.ComboBox ToBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

