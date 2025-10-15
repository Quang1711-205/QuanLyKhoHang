//namespace WareHouse.Presentation.Forms
//{
//    partial class DashBoardForm
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
//            this.titleDashBoard = new System.Windows.Forms.Label();
//            this.panelCard1 = new System.Windows.Forms.Panel();
//            this.namDoanhthu = new System.Windows.Forms.Label();
//            this.lblTitle1 = new System.Windows.Forms.Label();
//            this.panelCard2 = new System.Windows.Forms.Panel();
//            this.quantityE_warehouse = new System.Windows.Forms.Label();
//            this.label3 = new System.Windows.Forms.Label();
//            this.panelCard3 = new System.Windows.Forms.Panel();
//            this.quantityEX_warehouse = new System.Windows.Forms.Label();
//            this.label4 = new System.Windows.Forms.Label();
//            this.label5 = new System.Windows.Forms.Label();
//            this.panelHoatDong = new System.Windows.Forms.FlowLayoutPanel();
//            this.panelThongtin1 = new System.Windows.Forms.Panel();
//            this.lblDong1 = new System.Windows.Forms.Label();
//            this.lblTime1 = new System.Windows.Forms.Label();
//            this.panelThongtin2 = new System.Windows.Forms.Panel();
//            this.lblDong2 = new System.Windows.Forms.Label();
//            this.lblTime2 = new System.Windows.Forms.Label();
//            this.panelThongtin3 = new System.Windows.Forms.Panel();
//            this.lblDong3 = new System.Windows.Forms.Label();
//            this.lblTime3 = new System.Windows.Forms.Label();
//            this.label20 = new System.Windows.Forms.Label();
//            this.dgvLowStock = new System.Windows.Forms.DataGridView();
//            this.MãSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.TenSanPham = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.ConLai = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.HanhDong = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.panelCard1.SuspendLayout();
//            this.panelCard2.SuspendLayout();
//            this.panelCard3.SuspendLayout();
//            this.panelHoatDong.SuspendLayout();
//            this.panelThongtin1.SuspendLayout();
//            this.panelThongtin2.SuspendLayout();
//            this.panelThongtin3.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // titleDashBoard
//            // 
//            this.titleDashBoard.AutoSize = true;
//            this.titleDashBoard.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.titleDashBoard.ForeColor = System.Drawing.Color.Black;
//            this.titleDashBoard.Location = new System.Drawing.Point(286, 59);
//            this.titleDashBoard.Margin = new System.Windows.Forms.Padding(3, 15, 3, 15);
//            this.titleDashBoard.Name = "titleDashBoard";
//            this.titleDashBoard.Size = new System.Drawing.Size(132, 31);
//            this.titleDashBoard.TabIndex = 8;
//            this.titleDashBoard.Text = "Tổng Quan";
//            // 
//            // panelCard1
//            // 
//            this.panelCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
//            this.panelCard1.Controls.Add(this.namDoanhthu);
//            this.panelCard1.Controls.Add(this.lblTitle1);
//            this.panelCard1.Location = new System.Drawing.Point(292, 108);
//            this.panelCard1.Margin = new System.Windows.Forms.Padding(30, 3, 30, 15);
//            this.panelCard1.Name = "panelCard1";
//            this.panelCard1.Size = new System.Drawing.Size(250, 130);
//            this.panelCard1.TabIndex = 9;
//            // 
//            // namDoanhthu
//            // 
//            this.namDoanhthu.AutoSize = true;
//            this.namDoanhthu.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.namDoanhthu.ForeColor = System.Drawing.Color.White;
//            this.namDoanhthu.Location = new System.Drawing.Point(38, 63);
//            this.namDoanhthu.Name = "namDoanhthu";
//            this.namDoanhthu.Size = new System.Drawing.Size(143, 45);
//            this.namDoanhthu.TabIndex = 1;
//            this.namDoanhthu.Text = "200.000";
//            this.namDoanhthu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // lblTitle1
//            // 
//            this.lblTitle1.AutoSize = true;
//            this.lblTitle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblTitle1.ForeColor = System.Drawing.Color.White;
//            this.lblTitle1.Location = new System.Drawing.Point(42, 15);
//            this.lblTitle1.Name = "lblTitle1";
//            this.lblTitle1.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
//            this.lblTitle1.Size = new System.Drawing.Size(156, 33);
//            this.lblTitle1.TabIndex = 0;
//            this.lblTitle1.Text = "Doanh thu tháng";
//            // 
//            // panelCard2
//            // 
//            this.panelCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
//            this.panelCard2.Controls.Add(this.quantityE_warehouse);
//            this.panelCard2.Controls.Add(this.label3);
//            this.panelCard2.Location = new System.Drawing.Point(602, 108);
//            this.panelCard2.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
//            this.panelCard2.Name = "panelCard2";
//            this.panelCard2.Size = new System.Drawing.Size(251, 130);
//            this.panelCard2.TabIndex = 10;
//            // 
//            // quantityE_warehouse
//            // 
//            this.quantityE_warehouse.AutoSize = true;
//            this.quantityE_warehouse.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.quantityE_warehouse.ForeColor = System.Drawing.Color.White;
//            this.quantityE_warehouse.Location = new System.Drawing.Point(22, 63);
//            this.quantityE_warehouse.Name = "quantityE_warehouse";
//            this.quantityE_warehouse.Size = new System.Drawing.Size(209, 45);
//            this.quantityE_warehouse.TabIndex = 2;
//            this.quantityE_warehouse.Text = "120.000.000";
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label3.ForeColor = System.Drawing.Color.White;
//            this.label3.Location = new System.Drawing.Point(-9, 15);
//            this.label3.Name = "label3";
//            this.label3.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
//            this.label3.Size = new System.Drawing.Size(259, 33);
//            this.label3.TabIndex = 1;
//            this.label3.Text = "Tổng giá trị nhập kho (tháng)";
//            //this.label3.Click += new System.EventHandler(this.label3_Click);
//            // 
//            // panelCard3
//            // 
//            this.panelCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
//            this.panelCard3.Controls.Add(this.quantityEX_warehouse);
//            this.panelCard3.Controls.Add(this.label4);
//            this.panelCard3.Location = new System.Drawing.Point(913, 108);
//            this.panelCard3.Margin = new System.Windows.Forms.Padding(30, 3, 30, 3);
//            this.panelCard3.Name = "panelCard3";
//            this.panelCard3.Size = new System.Drawing.Size(250, 130);
//            this.panelCard3.TabIndex = 11;
//            // 
//            // quantityEX_warehouse
//            // 
//            this.quantityEX_warehouse.AutoSize = true;
//            this.quantityEX_warehouse.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.quantityEX_warehouse.ForeColor = System.Drawing.Color.White;
//            this.quantityEX_warehouse.Location = new System.Drawing.Point(32, 63);
//            this.quantityEX_warehouse.Name = "quantityEX_warehouse";
//            this.quantityEX_warehouse.Size = new System.Drawing.Size(190, 45);
//            this.quantityEX_warehouse.TabIndex = 2;
//            this.quantityEX_warehouse.Text = "1.200.0000";
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label4.ForeColor = System.Drawing.Color.White;
//            this.label4.Location = new System.Drawing.Point(-7, 15);
//            this.label4.Name = "label4";
//            this.label4.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
//            this.label4.Size = new System.Drawing.Size(254, 33);
//            this.label4.TabIndex = 1;
//            this.label4.Text = "Tổng giá trị xuất kho (tháng)";
//            // 
//            // label5
//            // 
//            this.label5.AutoSize = true;
//            this.label5.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label5.Location = new System.Drawing.Point(286, 253);
//            this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(221, 31);
//            this.label5.TabIndex = 12;
//            this.label5.Text = "Hoạt động gần đây";
//            // 
//            // panelHoatDong
//            // 
//            this.panelHoatDong.AutoScroll = true;
//            this.panelHoatDong.BackColor = System.Drawing.Color.Gainsboro;
//            this.panelHoatDong.Controls.Add(this.panelThongtin1);
//            this.panelHoatDong.Controls.Add(this.panelThongtin2);
//            this.panelHoatDong.Controls.Add(this.panelThongtin3);
//            this.panelHoatDong.Location = new System.Drawing.Point(292, 302);
//            this.panelHoatDong.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
//            this.panelHoatDong.Name = "panelHoatDong";
//            this.panelHoatDong.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
//            this.panelHoatDong.Size = new System.Drawing.Size(871, 228);
//            this.panelHoatDong.TabIndex = 13;
//            // 
//            // panelThongtin1
//            // 
//            this.panelThongtin1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.panelThongtin1.BackColor = System.Drawing.Color.White;
//            this.panelThongtin1.Controls.Add(this.lblDong1);
//            this.panelThongtin1.Controls.Add(this.lblTime1);
//            this.panelThongtin1.Location = new System.Drawing.Point(20, 10);
//            this.panelThongtin1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
//            this.panelThongtin1.Name = "panelThongtin1";
//            this.panelThongtin1.Size = new System.Drawing.Size(829, 42);
//            this.panelThongtin1.TabIndex = 6;
//            // 
//            // lblDong1
//            // 
//            this.lblDong1.AutoSize = true;
//            this.lblDong1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblDong1.Location = new System.Drawing.Point(5, 10);
//            this.lblDong1.Margin = new System.Windows.Forms.Padding(5, 10, 0, 9);
//            this.lblDong1.Name = "lblDong1";
//            this.lblDong1.Size = new System.Drawing.Size(392, 20);
//            this.lblDong1.TabIndex = 0;
//            this.lblDong1.Text = "[Admin] Thêm sản phẩm: Điện thoại SamSung Galaxy A52";
//            // 
//            // lblTime1
//            // 
//            this.lblTime1.AutoSize = true;
//            this.lblTime1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblTime1.Location = new System.Drawing.Point(734, 10);
//            this.lblTime1.Margin = new System.Windows.Forms.Padding(0, 10, 5, 9);
//            this.lblTime1.Name = "lblTime1";
//            this.lblTime1.Size = new System.Drawing.Size(90, 20);
//            this.lblTime1.TabIndex = 1;
//            this.lblTime1.Text = "5 phút trước";
//            // 
//            // panelThongtin2
//            // 
//            this.panelThongtin2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.panelThongtin2.BackColor = System.Drawing.Color.White;
//            this.panelThongtin2.Controls.Add(this.lblDong2);
//            this.panelThongtin2.Controls.Add(this.lblTime2);
//            this.panelThongtin2.Location = new System.Drawing.Point(20, 62);
//            this.panelThongtin2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
//            this.panelThongtin2.Name = "panelThongtin2";
//            this.panelThongtin2.Size = new System.Drawing.Size(829, 42);
//            this.panelThongtin2.TabIndex = 10;
//            // 
//            // lblDong2
//            // 
//            this.lblDong2.AutoSize = true;
//            this.lblDong2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblDong2.Location = new System.Drawing.Point(5, 10);
//            this.lblDong2.Margin = new System.Windows.Forms.Padding(5, 10, 0, 9);
//            this.lblDong2.Name = "lblDong2";
//            this.lblDong2.Size = new System.Drawing.Size(392, 20);
//            this.lblDong2.TabIndex = 0;
//            this.lblDong2.Text = "[Admin] Thêm sản phẩm: Điện thoại SamSung Galaxy A52";
//            // 
//            // lblTime2
//            // 
//            this.lblTime2.AutoSize = true;
//            this.lblTime2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblTime2.Location = new System.Drawing.Point(734, 10);
//            this.lblTime2.Margin = new System.Windows.Forms.Padding(0, 10, 5, 9);
//            this.lblTime2.Name = "lblTime2";
//            this.lblTime2.Size = new System.Drawing.Size(90, 20);
//            this.lblTime2.TabIndex = 1;
//            this.lblTime2.Text = "5 phút trước";
//            // 
//            // panelThongtin3
//            // 
//            this.panelThongtin3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.panelThongtin3.BackColor = System.Drawing.Color.White;
//            this.panelThongtin3.Controls.Add(this.lblDong3);
//            this.panelThongtin3.Controls.Add(this.lblTime3);
//            this.panelThongtin3.Location = new System.Drawing.Point(20, 114);
//            this.panelThongtin3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
//            this.panelThongtin3.Name = "panelThongtin3";
//            this.panelThongtin3.Size = new System.Drawing.Size(829, 42);
//            this.panelThongtin3.TabIndex = 7;
//            // 
//            // lblDong3
//            // 
//            this.lblDong3.AutoSize = true;
//            this.lblDong3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblDong3.Location = new System.Drawing.Point(5, 10);
//            this.lblDong3.Margin = new System.Windows.Forms.Padding(5, 10, 0, 9);
//            this.lblDong3.Name = "lblDong3";
//            this.lblDong3.Size = new System.Drawing.Size(392, 20);
//            this.lblDong3.TabIndex = 0;
//            this.lblDong3.Text = "[Admin] Thêm sản phẩm: Điện thoại SamSung Galaxy A52";
//            // 
//            // lblTime3
//            // 
//            this.lblTime3.AutoSize = true;
//            this.lblTime3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.lblTime3.Location = new System.Drawing.Point(734, 10);
//            this.lblTime3.Margin = new System.Windows.Forms.Padding(0, 10, 5, 9);
//            this.lblTime3.Name = "lblTime3";
//            this.lblTime3.Size = new System.Drawing.Size(90, 20);
//            this.lblTime3.TabIndex = 1;
//            this.lblTime3.Text = "5 phút trước";
//            // 
//            // label20
//            // 
//            this.label20.AutoSize = true;
//            this.label20.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.label20.Location = new System.Drawing.Point(286, 545);
//            this.label20.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
//            this.label20.Name = "label20";
//            this.label20.Size = new System.Drawing.Size(269, 31);
//            this.label20.TabIndex = 14;
//            this.label20.Text = "Cảnh Báo Sắp Hết Hàng";
//            // 
//            // dgvLowStock
//            // 
//            this.dgvLowStock.BackgroundColor = System.Drawing.Color.White;
//            this.dgvLowStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
//            this.dgvLowStock.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
//            this.dgvLowStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.dgvLowStock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            this.MãSP,
//            this.TenSanPham,
//            this.ConLai,
//            this.HanhDong});
//            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
//            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
//            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
//            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
//            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
//            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
//            this.dgvLowStock.DefaultCellStyle = dataGridViewCellStyle3;
//            this.dgvLowStock.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
//            this.dgvLowStock.Location = new System.Drawing.Point(292, 594);
//            this.dgvLowStock.Name = "dgvLowStock";
//            this.dgvLowStock.ReadOnly = true;
//            this.dgvLowStock.RowHeadersVisible = false;
//            this.dgvLowStock.RowHeadersWidth = 51;
//            this.dgvLowStock.RowTemplate.Height = 24;
//            this.dgvLowStock.Size = new System.Drawing.Size(871, 150);
//            this.dgvLowStock.TabIndex = 15;
//            // 
//            // MãSP
//            // 
//            this.MãSP.HeaderText = "\"Mã SP\"";
//            this.MãSP.MinimumWidth = 6;
//            this.MãSP.Name = "MãSP";
//            this.MãSP.ReadOnly = true;
//            this.MãSP.Width = 120;
//            // 
//            // TenSanPham
//            // 
//            this.TenSanPham.HeaderText = "\"Tên Sản Phẩm\"";
//            this.TenSanPham.MinimumWidth = 6;
//            this.TenSanPham.Name = "TenSanPham";
//            this.TenSanPham.ReadOnly = true;
//            this.TenSanPham.Width = 350;
//            // 
//            // ConLai
//            // 
//            this.ConLai.HeaderText = "\"Còn lại\"";
//            this.ConLai.MinimumWidth = 6;
//            this.ConLai.Name = "ConLai";
//            this.ConLai.ReadOnly = true;
//            this.ConLai.Width = 200;
//            // 
//            // HanhDong
//            // 
//            this.HanhDong.HeaderText = "\"Hành Động\"";
//            this.HanhDong.MinimumWidth = 6;
//            this.HanhDong.Name = "HanhDong";
//            this.HanhDong.ReadOnly = true;
//            this.HanhDong.Width = 200;
//            // 
//            // DashBoardForm
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.BackColor = System.Drawing.Color.White;
//            this.ClientSize = new System.Drawing.Size(1194, 783);
//            this.Controls.Add(this.dgvLowStock);
//            this.Controls.Add(this.label20);
//            this.Controls.Add(this.panelHoatDong);
//            this.Controls.Add(this.label5);
//            this.Controls.Add(this.panelCard3);
//            this.Controls.Add(this.panelCard2);
//            this.Controls.Add(this.panelCard1);
//            this.Controls.Add(this.titleDashBoard);
//            this.Name = "DashBoardForm";
//            this.Text = "DashBoardForm";
//            this.Controls.SetChildIndex(this.titleDashBoard, 0);
//            this.Controls.SetChildIndex(this.panelCard1, 0);
//            this.Controls.SetChildIndex(this.panelCard2, 0);
//            this.Controls.SetChildIndex(this.panelCard3, 0);
//            this.Controls.SetChildIndex(this.label5, 0);
//            this.Controls.SetChildIndex(this.panelHoatDong, 0);
//            this.Controls.SetChildIndex(this.label20, 0);
//            this.Controls.SetChildIndex(this.dgvLowStock, 0);
//            this.panelCard1.ResumeLayout(false);
//            this.panelCard1.PerformLayout();
//            this.panelCard2.ResumeLayout(false);
//            this.panelCard2.PerformLayout();
//            this.panelCard3.ResumeLayout(false);
//            this.panelCard3.PerformLayout();
//            this.panelHoatDong.ResumeLayout(false);
//            this.panelThongtin1.ResumeLayout(false);
//            this.panelThongtin1.PerformLayout();
//            this.panelThongtin2.ResumeLayout(false);
//            this.panelThongtin2.PerformLayout();
//            this.panelThongtin3.ResumeLayout(false);
//            this.panelThongtin3.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStock)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private System.Windows.Forms.Label titleDashBoard;
//        private System.Windows.Forms.Panel panelCard1;
//        private System.Windows.Forms.Label namDoanhthu;
//        private System.Windows.Forms.Label lblTitle1;
//        private System.Windows.Forms.Panel panelCard2;
//        private System.Windows.Forms.Label quantityE_warehouse;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Panel panelCard3;
//        private System.Windows.Forms.Label quantityEX_warehouse;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.FlowLayoutPanel panelHoatDong;
//        private System.Windows.Forms.Panel panelThongtin1;
//        private System.Windows.Forms.Label lblDong1;
//        private System.Windows.Forms.Label lblTime1;
//        private System.Windows.Forms.Panel panelThongtin2;
//        private System.Windows.Forms.Label lblDong2;
//        private System.Windows.Forms.Label lblTime2;
//        private System.Windows.Forms.Panel panelThongtin3;
//        private System.Windows.Forms.Label lblDong3;
//        private System.Windows.Forms.Label lblTime3;
//        private System.Windows.Forms.Label label20;
//        private System.Windows.Forms.DataGridView dgvLowStock;
//        private System.Windows.Forms.DataGridViewTextBoxColumn MãSP;
//        private System.Windows.Forms.DataGridViewTextBoxColumn TenSanPham;
//        private System.Windows.Forms.DataGridViewTextBoxColumn ConLai;
//        private System.Windows.Forms.DataGridViewTextBoxColumn HanhDong;
//    }
//}

namespace WareHouse.Presentation.Forms
{
    partial class DashBoardForm
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
            this.SuspendLayout();
            // 
            // DashBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1194, 783);
            this.Name = "DashBoardForm";
            this.Text = "DashBoardForm";
            this.ResumeLayout(false);

        }

        #endregion
    }
}