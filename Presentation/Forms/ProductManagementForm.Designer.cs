namespace WareHouse.Presentation.Forms
{
    partial class ProductManagementForm
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
            this.titleProduct = new System.Windows.Forms.Label();
            this.themsanpham = new System.Windows.Forms.Button();
            this.txbSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // titleProduct
            // 
            this.titleProduct.AutoSize = true;
            this.titleProduct.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleProduct.Location = new System.Drawing.Point(291, 59);
            this.titleProduct.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.titleProduct.Name = "titleProduct";
            this.titleProduct.Size = new System.Drawing.Size(212, 31);
            this.titleProduct.TabIndex = 3;
            this.titleProduct.Text = "Quản Lý Sản Phẩm";
            // 
            // themsanpham
            // 
            this.themsanpham.BackColor = System.Drawing.Color.LimeGreen;
            this.themsanpham.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.themsanpham.ForeColor = System.Drawing.Color.White;
            this.themsanpham.Location = new System.Drawing.Point(974, 52);
            this.themsanpham.Name = "themsanpham";
            this.themsanpham.Size = new System.Drawing.Size(136, 44);
            this.themsanpham.TabIndex = 4;
            this.themsanpham.Text = "Thêm mới";
            this.themsanpham.UseVisualStyleBackColor = false;
            this.themsanpham.Click += new System.EventHandler(this.themsanpham_Click);
            // 
            // txbSearch
            // 
            this.txbSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txbSearch.Location = new System.Drawing.Point(297, 104);
            this.txbSearch.Multiline = true;
            this.txbSearch.Name = "txbSearch";
            this.txbSearch.Size = new System.Drawing.Size(671, 44);
            this.txbSearch.TabIndex = 6;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(974, 104);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(136, 44);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvProduct
            // 
            this.dgvProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProduct.Location = new System.Drawing.Point(297, 184);
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.RowHeadersWidth = 51;
            this.dgvProduct.RowTemplate.Height = 24;
            this.dgvProduct.Size = new System.Drawing.Size(871, 150);
            this.dgvProduct.TabIndex = 8;
            // 
            // ProductManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 783);
            this.Controls.Add(this.dgvProduct);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txbSearch);
            this.Controls.Add(this.themsanpham);
            this.Controls.Add(this.titleProduct);
            this.Name = "ProductManagementForm";
            this.Text = "ProductManagementForm";
            this.Load += new System.EventHandler(this.ProductManagementForm_Load);
            this.Controls.SetChildIndex(this.titleProduct, 0);
            this.Controls.SetChildIndex(this.themsanpham, 0);
            this.Controls.SetChildIndex(this.txbSearch, 0);
            this.Controls.SetChildIndex(this.btnSearch, 0);
            this.Controls.SetChildIndex(this.dgvProduct, 0);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleProduct;
        private System.Windows.Forms.Button themsanpham;
        private System.Windows.Forms.TextBox txbSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvProduct;
    }
}