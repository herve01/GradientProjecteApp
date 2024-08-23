namespace TpRaoGradienProjecte
{
    partial class PointDepart
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
            this.BtnOpt = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // BtnOpt
            // 
            this.BtnOpt.Location = new System.Drawing.Point(68, 172);
            this.BtnOpt.Name = "BtnOpt";
            this.BtnOpt.Size = new System.Drawing.Size(75, 23);
            this.BtnOpt.TabIndex = 0;
            this.BtnOpt.Text = "Optimiser";
            this.BtnOpt.UseVisualStyleBackColor = true;
            this.BtnOpt.Click += new System.EventHandler(this.BtnOpt_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 154);
            this.panel1.TabIndex = 1;
            // 
            // PointDepart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 201);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnOpt);
            this.Name = "PointDepart";
            this.Text = "PointDepart";
            this.Load += new System.EventHandler(this.PointDepart_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnOpt;
        private System.Windows.Forms.Panel panel1;
    }
}