namespace DovizBar
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            this.txtEuro = new System.Windows.Forms.Label();
            this.txtDolar = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtEuro
            // 
            this.txtEuro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEuro.AutoSize = true;
            this.txtEuro.BackColor = System.Drawing.Color.Transparent;
            this.txtEuro.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.txtEuro.Font = new System.Drawing.Font("Segoe UI Black", 11F, System.Drawing.FontStyle.Bold);
            this.txtEuro.ForeColor = System.Drawing.Color.Gold;
            this.txtEuro.Location = new System.Drawing.Point(83, 17);
            this.txtEuro.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtEuro.Name = "txtEuro";
            this.txtEuro.Size = new System.Drawing.Size(37, 20);
            this.txtEuro.TabIndex = 7;
            this.txtEuro.Text = "null";
            this.txtEuro.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.txtEuro.TextChanged += new System.EventHandler(this.txtEuro_TextChanged);
            // 
            // txtDolar
            // 
            this.txtDolar.AutoSize = true;
            this.txtDolar.BackColor = System.Drawing.Color.Transparent;
            this.txtDolar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.txtDolar.Font = new System.Drawing.Font("Segoe UI Black", 11F, System.Drawing.FontStyle.Bold);
            this.txtDolar.ForeColor = System.Drawing.Color.Gold;
            this.txtDolar.Location = new System.Drawing.Point(2, 18);
            this.txtDolar.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtDolar.Name = "txtDolar";
            this.txtDolar.Size = new System.Drawing.Size(37, 20);
            this.txtDolar.TabIndex = 5;
            this.txtDolar.Text = "null";
            this.txtDolar.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(84, 5);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "EURO (€)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "DOLAR ($)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtClose
            // 
            this.txtClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClose.AutoSize = true;
            this.txtClose.BackColor = System.Drawing.Color.Transparent;
            this.txtClose.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtClose.ForeColor = System.Drawing.Color.White;
            this.txtClose.Location = new System.Drawing.Point(69, -1);
            this.txtClose.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.txtClose.Name = "txtClose";
            this.txtClose.Size = new System.Drawing.Size(18, 21);
            this.txtClose.TabIndex = 8;
            this.txtClose.Text = "x";
            this.txtClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.txtClose.Click += new System.EventHandler(this.txtClose_Click);
            this.txtClose.MouseLeave += new System.EventHandler(this.txtClose_MouseLeave);
            this.txtClose.MouseHover += new System.EventHandler(this.txtClose_MouseHover);
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(43)))), ((int)(((byte)(97)))));
            this.ClientSize = new System.Drawing.Size(150, 40);
            this.ControlBox = false;
            this.Controls.Add(this.txtClose);
            this.Controls.Add(this.txtEuro);
            this.Controls.Add(this.txtDolar);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(150, 40);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(150, 40);
            this.Name = "Form4";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Mini Döviz Bar";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form4_Load);
            this.DoubleClick += new System.EventHandler(this.Form4_DoubleClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtEuro;
        private System.Windows.Forms.Label txtDolar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label txtClose;
    }
}