namespace WareHouse.Presentation.Forms
{
    partial class AddWareHouse
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
            this.cbNameProduct = new System.Windows.Forms.ComboBox();
            this.addProductWH = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txbQuantityWH = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(94, 92);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tên sản phẩm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbNameProduct
            // 
            this.cbNameProduct.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbNameProduct.FormattingEnabled = true;
            this.cbNameProduct.Location = new System.Drawing.Point(249, 92);
            this.cbNameProduct.Name = "cbNameProduct";
            this.cbNameProduct.Size = new System.Drawing.Size(222, 31);
            this.cbNameProduct.TabIndex = 11;
            // 
            // addProductWH
            // 
            this.addProductWH.BackColor = System.Drawing.Color.DodgerBlue;
            this.addProductWH.FlatAppearance.BorderSize = 0;
            this.addProductWH.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addProductWH.ForeColor = System.Drawing.Color.White;
            this.addProductWH.Location = new System.Drawing.Point(233, 226);
            this.addProductWH.Name = "addProductWH";
            this.addProductWH.Size = new System.Drawing.Size(116, 38);
            this.addProductWH.TabIndex = 13;
            this.addProductWH.Text = "Thêm";
            this.addProductWH.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(94, 143);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 31);
            this.label3.TabIndex = 15;
            this.label3.Text = "Số lượng";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txbQuantityWH
            // 
            this.txbQuantityWH.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbQuantityWH.Location = new System.Drawing.Point(249, 143);
            this.txbQuantityWH.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.txbQuantityWH.Multiline = true;
            this.txbQuantityWH.Name = "txbQuantityWH";
            this.txbQuantityWH.Size = new System.Drawing.Size(222, 28);
            this.txbQuantityWH.TabIndex = 16;
            this.txbQuantityWH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // AddWareHouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 353);
            this.Controls.Add(this.txbQuantityWH);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.addProductWH);
            this.Controls.Add(this.cbNameProduct);
            this.Controls.Add(this.label1);
            this.Name = "AddWareHouse";
            this.Text = "AddWareHouse";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbNameProduct;
        private System.Windows.Forms.Button addProductWH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbQuantityWH;
    }
}