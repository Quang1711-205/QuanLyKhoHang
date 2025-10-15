//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class AddExportWareHouse : Form
//    {
//        public AddExportWareHouse()
//        {
//            InitializeComponent();
//        }
//    }
//}

using System;
using System.Data;
using System.Windows.Forms;
using WareHouse.DataAccess;
using WareHouse.Models;

namespace WareHouse.Presentation.Forms
{
    public partial class AddExportWareHouse : Form
    {
        // Thay Product bằng ExportProductWareHouse
        public ExportProductWareHouse SanPhamMoi { get; private set; }

        public AddExportWareHouse()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
            SetupControls();
            LoadProducts();
        }

        private void SetupControls()
        {
            // Đảm bảo ComboBox chỉ cho phép chọn, không cho phép nhập tay
            cbNameProduct1.DropDownStyle = ComboBoxStyle.DropDownList;

            // Gán sự kiện Click cho nút "Thêm"
            addProductWH1.Click += AddProductWH_Click;
        }

        private void LoadProducts()
        {
            try
            {
                string query = "SELECT id, name, export_price, stock_quantity FROM products WHERE stock_quantity > 0";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbNameProduct1.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    // Sử dụng ExportProductWareHouse thay vì Product
                    cbNameProduct1.Items.Add(new ExportProductWareHouse
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString(),
                        GiaXuat = Convert.ToDecimal(row["export_price"]),
                        SoLuong = Convert.ToInt32(row["stock_quantity"]) // Lưu số lượng tồn kho
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddProductWH_Click(object sender, EventArgs e)
        {
            try
            {
                // Sử dụng ExportProductWareHouse thay vì Product
                ExportProductWareHouse selectedProduct = cbNameProduct1.SelectedItem as ExportProductWareHouse;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuong;
                if (!int.TryParse(txbQuantityWH1.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số lượng tồn kho
                if (soLuong > selectedProduct.SoLuong)
                {
                    MessageBox.Show($"Số lượng xuất vượt quá số lượng tồn kho ({selectedProduct.SoLuong})!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo đối tượng ExportProductWareHouse mới để trả về
                SanPhamMoi = new ExportProductWareHouse
                {
                    Id = selectedProduct.Id,
                    TenSanPham = selectedProduct.TenSanPham,
                    GiaXuat = selectedProduct.GiaXuat,
                    SoLuong = soLuong // Lưu số lượng xuất mà người dùng nhập
                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}