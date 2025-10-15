using System;
using System.Data;
using System.Windows.Forms;
using WareHouse.DataAccess;
using WareHouse.Models; // Đảm bảo import namespace chứa Product

namespace WareHouse.Presentation.Forms
{
    public partial class AddWareHouse : Form
    {
        public Product SanPhamMoi { get; private set; }

        public AddWareHouse()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
            SetupControls();
            LoadProducts();
        }

        private void SetupControls()
        {
            // Đảm bảo ComboBox chỉ cho phép chọn, không cho phép nhập tay
            cbNameProduct.DropDownStyle = ComboBoxStyle.DropDownList;

            // Gán sự kiện Click cho nút "Thêm"
            addProductWH.Click += AddProductWH_Click;
        }

        private void LoadProducts()
        {
            try
            {
                string query = "SELECT id, name, import_price FROM products";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbNameProduct.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbNameProduct.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString(),
                        GiaNhap = Convert.ToDecimal(row["import_price"])
                    });
                }
                // Không cần DisplayMember vì Product đã override ToString()
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
                Product selectedProduct = cbNameProduct.SelectedItem as Product;
                if (selectedProduct == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int soLuong;
                if (!int.TryParse(txbQuantityWH.Text, out soLuong) || soLuong <= 0)
                {
                    MessageBox.Show("Số lượng phải là số nguyên dương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SanPhamMoi = new Product
                {
                    Id = selectedProduct.Id,
                    TenSanPham = selectedProduct.TenSanPham,
                    GiaNhap = selectedProduct.GiaNhap,
                    SoLuong = soLuong
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