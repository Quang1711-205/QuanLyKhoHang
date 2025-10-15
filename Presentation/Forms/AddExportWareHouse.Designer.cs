namespace WareHouse.Presentation.Forms
{
    partial class AddExportWareHouse
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbNameProduct1 = new System.Windows.Forms.ComboBox();
            this.txbQuantityWH1 = new System.Windows.Forms.TextBox();
            this.addProductWH1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(101, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tên sản phẩm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(101, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 31);
            this.label3.TabIndex = 16;
            this.label3.Text = "Số lượng";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbNameProduct1
            // 
            this.cbNameProduct1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNameProduct1.FormattingEnabled = true;
            this.cbNameProduct1.Location = new System.Drawing.Point(238, 90);
            this.cbNameProduct1.Name = "cbNameProduct1";
            this.cbNameProduct1.Size = new System.Drawing.Size(222, 31);
            this.cbNameProduct1.TabIndex = 17;
            // 
            // txbQuantityWH1
            // 
            this.txbQuantityWH1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbQuantityWH1.Location = new System.Drawing.Point(238, 141);
            this.txbQuantityWH1.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txbQuantityWH1.Multiline = true;
            this.txbQuantityWH1.Name = "txbQuantityWH1";
            this.txbQuantityWH1.Size = new System.Drawing.Size(222, 28);
            this.txbQuantityWH1.TabIndex = 18;
            this.txbQuantityWH1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // addProductWH1
            // 
            this.addProductWH1.BackColor = System.Drawing.Color.DodgerBlue;
            this.addProductWH1.FlatAppearance.BorderSize = 0;
            this.addProductWH1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addProductWH1.ForeColor = System.Drawing.Color.White;
            this.addProductWH1.Location = new System.Drawing.Point(238, 215);
            this.addProductWH1.Name = "addProductWH1";
            this.addProductWH1.Size = new System.Drawing.Size(116, 38);
            this.addProductWH1.TabIndex = 19;
            this.addProductWH1.Text = "Thêm";
            this.addProductWH1.UseVisualStyleBackColor = false;
            // 
            // AddExportWareHouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 353);
            this.Controls.Add(this.addProductWH1);
            this.Controls.Add(this.txbQuantityWH1);
            this.Controls.Add(this.cbNameProduct1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "AddExportWareHouse";
            this.Text = "AddExportWareHouse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbNameProduct1;
        private System.Windows.Forms.TextBox txbQuantityWH1;
        private System.Windows.Forms.Button addProductWH1;
    }
}