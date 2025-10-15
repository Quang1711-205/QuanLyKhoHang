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
//    public partial class AddProductForm : Form
//    {
//        public AddProductForm()
//        {
//            InitializeComponent();
//        }

//        private void label4_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class AddProductForm : Form
    {
        private ProductManagementForm _parentForm;

        public AddProductForm(ProductManagementForm parentForm)
        {
            InitializeComponent();
            // Đăng ký sự kiện Click cho button addProduct
            this.addProduct.Click += new System.EventHandler(this.addProduct_Click);
            LoadCategories();
            LoadSuppliers();
            _parentForm = parentForm;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Có thể để trống nếu không cần xử lý sự kiện này
        }

        private void LoadCategories()
        {
            try
            {
                // Lấy danh sách category từ database
                string query = "SELECT name FROM categories";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                // Thêm các category vào combobox
                cbCategory.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbCategory.Items.Add(row["name"].ToString());
                }

                // Cho phép nhập text mới vào combobox
                cbCategory.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                // Lấy danh sách supplier từ database
                string query = "SELECT name FROM suppliers";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                // Thêm các supplier vào combobox
                cbSupplier.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbSupplier.Items.Add(row["name"].ToString());
                }

                // Cho phép nhập text mới vào combobox
                cbSupplier.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetOrCreateCategory(string categoryName)
        {
            try
            {
                // Kiểm tra xem category đã tồn tại chưa
                string checkQuery = "SELECT id FROM categories WHERE name = @categoryName";
                Dictionary<string, object> checkParams = new Dictionary<string, object>
                {
                    { "@categoryName", categoryName }
                };

                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (result != null && result != DBNull.Value)
                {
                    // Nếu category đã tồn tại, trả về id
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Nếu category chưa tồn tại, thêm mới và trả về id mới
                    string insertQuery = "INSERT INTO categories (name) VALUES (@categoryName); SELECT LAST_INSERT_ID();";
                    Dictionary<string, object> insertParams = new Dictionary<string, object>
                    {
                        { "@categoryName", categoryName }
                    };

                    object newId = DatabaseHelper.ExecuteScalar(insertQuery, insertParams);
                    return Convert.ToInt32(newId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int GetOrCreateSupplier(string supplierName)
        {
            try
            {
                // Kiểm tra xem supplier đã tồn tại chưa
                string checkQuery = "SELECT id FROM suppliers WHERE name = @supplierName";
                Dictionary<string, object> checkParams = new Dictionary<string, object>
                {
                    { "@supplierName", supplierName }
                };

                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (result != null && result != DBNull.Value)
                {
                    // Nếu supplier đã tồn tại, trả về id
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Nếu supplier chưa tồn tại, thêm mới và trả về id mới
                    string insertQuery = "INSERT INTO suppliers (name) VALUES (@supplierName); SELECT LAST_INSERT_ID();";
                    Dictionary<string, object> insertParams = new Dictionary<string, object>
                    {
                        { "@supplierName", supplierName }
                    };

                    object newId = DatabaseHelper.ExecuteScalar(insertQuery, insertParams);
                    return Convert.ToInt32(newId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private void addProduct_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrWhiteSpace(txbProduct.Text) ||
                    string.IsNullOrWhiteSpace(txbImPrice.Text) ||
                    string.IsNullOrWhiteSpace(txbExPrice.Text) ||
                    string.IsNullOrWhiteSpace(cbCategory.Text) ||
                    string.IsNullOrWhiteSpace(cbSupplier.Text) ||
                    string.IsNullOrWhiteSpace(txbQuantity.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin sản phẩm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra giá nhập và giá bán có phải là số không
                if (!decimal.TryParse(txbImPrice.Text, out decimal importPrice) || importPrice <= 0)
                {
                    MessageBox.Show("Giá nhập không hợp lệ! Vui lòng nhập số lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txbExPrice.Text, out decimal exportPrice) || exportPrice <= 0)
                {
                    MessageBox.Show("Giá bán không hợp lệ! Vui lòng nhập số lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra số lượng có phải là số không
                if (!int.TryParse(txbQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Số lượng sản phẩm không hợp lệ! Vui lòng nhập số không âm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra giá bán có lớn hơn hoặc bằng giá nhập không
                if (exportPrice < importPrice)
                {
                    MessageBox.Show("Giá bán phải lớn hơn hoặc bằng giá nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy hoặc tạo mới category_id và supplier_id
                int categoryId = GetOrCreateCategory(cbCategory.Text);
                int supplierId = GetOrCreateSupplier(cbSupplier.Text);

                if (categoryId == -1 || supplierId == -1)
                {
                    return; // Đã có lỗi xảy ra và đã hiển thị thông báo
                }

                // Thêm sản phẩm vào database
                string insertQuery = @"INSERT INTO products (name, import_price, export_price, category_id, supplier_id, stock_quantity, description)
                                    VALUES (@productName, @importPrice, @exportPrice, @categoryId, @supplierId, @quantity, @description)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@productName", txbProduct.Text },
                    { "@importPrice", importPrice },
                    { "@exportPrice", exportPrice },
                    { "@categoryId", categoryId },
                    { "@supplierId", supplierId },
                    { "@quantity", quantity },
                    { "@description", string.IsNullOrEmpty(txbDescription.Text) ? DBNull.Value : (object)txbDescription.Text }
                };

                int result = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);

                if (result > 0)
                {
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();

                    // Cập nhật lại combobox
                    LoadCategories();
                    LoadSuppliers();

                    if (_parentForm != null)
                    {
                        _parentForm.Reset(); // Gọi Reset để làm mới bảng trong ProductManagementForm
                    }

                    // Đóng form sau khi thêm thành công
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm sản phẩm thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txbProduct.Clear();
            txbImPrice.Clear();
            txbExPrice.Clear();
            txbQuantity.Clear();
            txbDescription.Clear();
            cbCategory.Text = "";
            cbSupplier.Text = "";
        }

        private void AddProductForm_Load(object sender, EventArgs e)
        {
            // Có thể để trống nếu không cần xử lý sự kiện này
        }
    }
}




