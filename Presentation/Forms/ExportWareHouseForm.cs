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
//    public partial class ExportWareHouseForm : Form
//    {
//        public ExportWareHouseForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WareHouse.DataAccess;
using WareHouse.Models;
using WareHouse.Utils;
using MySql.Data.MySqlClient;

namespace WareHouse.Presentation.Forms
{
    public partial class ExportWareHouseForm : BaseForm
    {
        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnAdd;
        private Button btnReset;
        private Button btnHistory; // Nút mới để mở lịch sử xuất kho
        private GroupBox gbExportInfo;
        private Label lblExportCode;
        public TextBox txtExportCode;
        private Label lblExportDate;
        public DateTimePicker dtpExportDate;
        private Label lblCustomer;
        public ComboBox cbCustomer;
        private Label lblExporter;
        public ComboBox cbExporter;
        private Label lblWarehouse;
        public ComboBox cbWarehouse;
        private Label lblStatus;
        public TextBox txtStatus;
        private Label lblNote;
        public TextBox txtNote;
        private DataGridView dgvProducts;
        private Button btnComplete;
        private Button btnCancel;
        private Label lblTotalQuantity;
        private Label lblTotalQuantityValue;
        private Label lblTotalAmount;
        private Label lblTotalAmountValue;
        private TableLayoutPanel productsPanel;

        public List<ExportProductWareHouse> danhSachTam = new List<ExportProductWareHouse>();

        public ExportWareHouseForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (!IsValidRole())
            {
                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Xuất Kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeCustomComponents();
            SetupEventHandlers();
            LoadComboBoxData();
            ResetForm();
        }

        private bool IsValidRole()
        {
            return RoleId == 2 || RoleId == 1; 
        }

        private void SetupEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnReset.Click += BtnReset_Click;
            btnHistory.Click += BtnHistory_Click; // Thêm sự kiện cho nút lịch sử
            btnComplete.Click += BtnComplete_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.CellValueChanged += DgvProducts_CellValueChanged;
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại (ImportWareHouseForm)
            ExportHistoryForm historyForm = new ExportHistoryForm(RoleId, this);
            historyForm.Show(); // Mở form ImportHistoryForm
        }

        private void LoadComboBoxData()
        {
            LoadCustomers();
            LoadUsers();
            LoadWarehouses();
        }

        private void LoadCustomers()
        {
            try
            {
                string query = "SELECT id, name FROM customers";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbCustomer.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbCustomer.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString()
                    });
                }
                cbCustomer.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                string query = "SELECT id, username, full_name FROM users";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbExporter.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbExporter.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["full_name"].ToString()
                    });
                }
                cbExporter.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadWarehouses()
        {
            try
            {
                string query = "SELECT id, name FROM warehouses";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbWarehouse.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbWarehouse.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString()
                    });
                }
                cbWarehouse.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateExitCode()
        {
            try
            {
                string query = "SELECT MAX(id) FROM stock_exit_headers";
                object result = DatabaseHelper.ExecuteScalar(query, null);

                int maxId = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                int newId = maxId + 1;

                if (newId < 10)
                {
                    return $"XK00{newId}";
                }
                else if (newId < 100)
                {
                    return $"XK0{newId}";
                }
                else
                {
                    return $"XK{newId}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo mã phiếu xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "XK001";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.BackColor = Color.MediumSeaGreen;

            using (AddExportWareHouse form = new AddExportWareHouse())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    danhSachTam.Add(form.SanPhamMoi);
                    UpdateDataGridView();
                }
            }
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            danhSachTam.Clear();
            UpdateDataGridView();

            txtExportCode.Text = GenerateExitCode();
            dtpExportDate.Value = DateTime.Now;
            cbCustomer.Text = cbCustomer.Items.Count > 0 ? (cbCustomer.Items[0] as Product).TenSanPham : "Công ty TNHH Việt Nam";
            cbExporter.Text = cbExporter.Items.Count > 0 ? (cbExporter.Items[0] as Product).TenSanPham : "Nguyễn Văn A";
            cbWarehouse.Text = cbWarehouse.Items.Count > 0 ? (cbWarehouse.Items[0] as Product).TenSanPham : "Kho chính";
            txtStatus.Text = "Chờ duyệt";
            txtNote.Text = "Xuất hàng đợt 1 tháng 4/2025";

            btnAdd.BackColor = SystemColors.Control;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (danhSachTam.Count > 0)
            {
                DialogResult result = MessageBox.Show("Dữ liệu chưa được lưu. Bạn có chắc chắn muốn hủy?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            this.Close();
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProducts.Columns["colDelete"].Index && e.RowIndex >= 0)
            {
                danhSachTam.RemoveAt(e.RowIndex);
                UpdateDataGridView();
            }
        }

        private void DgvProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int rowIndex = e.RowIndex;
                if (e.ColumnIndex == dgvProducts.Columns["colQuantity"].Index)
                {
                    try
                    {
                        decimal giaXuat = Convert.ToDecimal(dgvProducts.Rows[rowIndex].Cells["colPrice"].Value.ToString().Replace(",", ""));
                        int soLuong = Convert.ToInt32(dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value);

                        // Kiểm tra số lượng tồn kho
                        int productId = int.Parse(dgvProducts.Rows[rowIndex].Cells["colProductID"].Value.ToString().Replace("SP", ""));
                        string query = "SELECT stock_quantity FROM products WHERE id = @id";
                        Dictionary<string, object> paramsDict = new Dictionary<string, object> { { "@id", productId } };
                        object result = DatabaseHelper.ExecuteScalar(query, paramsDict);
                        int stockQuantity = result != null ? Convert.ToInt32(result) : 0;

                        if (soLuong > stockQuantity)
                        {
                            MessageBox.Show($"Số lượng xuất vượt quá số lượng tồn kho ({stockQuantity})!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            soLuong = stockQuantity;
                            dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value = soLuong;
                        }

                        danhSachTam[rowIndex].SoLuong = soLuong;

                        decimal thanhTien = giaXuat * soLuong;
                        dgvProducts.Rows[rowIndex].Cells["colTotal"].Value = thanhTien.ToString("N0") + " VND";

                        UpdateTotals();
                    }
                    catch
                    {
                        MessageBox.Show("Dữ liệu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string FormatProductCode(int id)
        {
            if (id < 10)
            {
                return $"SP00{id}";
            }
            else if (id < 100)
            {
                return $"SP0{id}";
            }
            else
            {
                return $"SP{id}";
            }
        }

        public void UpdateDataGridView()
        {
            dgvProducts.Rows.Clear();
            if (danhSachTam.Count == 0)
            {
                dgvProducts.ColumnHeadersVisible = false;
            }
            else
            {
                dgvProducts.ColumnHeadersVisible = true;
                foreach (var sp in danhSachTam)
                {
                    decimal thanhTien = sp.GiaXuat * sp.SoLuong;
                    string maSanPham = FormatProductCode(sp.Id);
                    dgvProducts.Rows.Add(maSanPham, sp.TenSanPham, sp.GiaXuat.ToString("N0"), sp.SoLuong, thanhTien.ToString("N0") + " VND", "Xóa");
                }
            }
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            int totalQuantity = 0;
            decimal totalAmount = 0;

            foreach (var sp in danhSachTam)
            {
                totalQuantity += sp.SoLuong;
                totalAmount += sp.GiaXuat * sp.SoLuong;
            }

            lblTotalQuantityValue.Text = totalQuantity.ToString();
            lblTotalAmountValue.Text = totalAmount.ToString("N0") + " VND";
        }

        private void BtnComplete_Click(object sender, EventArgs e)
        {
            if (danhSachTam.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtExportCode.Text) || string.IsNullOrWhiteSpace(cbCustomer.Text) ||
                string.IsNullOrWhiteSpace(cbExporter.Text) || string.IsNullOrWhiteSpace(cbWarehouse.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phiếu xuất kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra số lượng tồn kho trước khi lưu
            foreach (var sp in danhSachTam)
            {
                string query = "SELECT stock_quantity FROM products WHERE id = @id";
                Dictionary<string, object> paramsDict = new Dictionary<string, object> { { "@id", sp.Id } };
                object result = DatabaseHelper.ExecuteScalar(query, paramsDict);
                int stockQuantity = result != null ? Convert.ToInt32(result) : 0;

                if (sp.SoLuong > stockQuantity)
                {
                    MessageBox.Show($"Số lượng xuất của sản phẩm {sp.TenSanPham} vượt quá tồn kho ({stockQuantity})!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MySqlTransaction transaction = null;
            MySqlConnection connection = null;

            try
            {
                // Mở kết nối và bắt đầu transaction
                connection = DatabaseHelper.GetConnection();
                transaction = connection.BeginTransaction();

                int customerId = GetOrCreateCustomer(cbCustomer.Text);
                int userId = GetOrCreateUser(cbExporter.Text);
                int warehouseId = GetOrCreateWarehouse(cbWarehouse.Text);

                if (customerId == -1 || userId == -1 || warehouseId == -1)
                {
                    throw new Exception("Không thể lấy hoặc tạo ID cho khách hàng, người dùng hoặc kho!");
                }

                // Lưu thông tin phiếu xuất kho
                string insertHeaderQuery = @"
                    INSERT INTO stock_exit_headers (exit_code, exit_date, customer_id, warehouse_id, user_id, note, status)
                    VALUES (@exitCode, @exitDate, @customerId, @warehouseId, @userId, @note, @status);
                    SELECT LAST_INSERT_ID();";

                Dictionary<string, object> headerParams = new Dictionary<string, object>
                {
                    { "@exitCode", txtExportCode.Text },
                    { "@exitDate", dtpExportDate.Value },
                    { "@customerId", customerId },
                    { "@warehouseId", warehouseId },
                    { "@userId", userId },
                    { "@note", txtNote.Text },
                    { "@status", txtStatus.Text }
                };

                object stockExitId = DatabaseHelper.ExecuteScalar(insertHeaderQuery, headerParams, transaction, connection);

                if (stockExitId == null)
                {
                    throw new Exception("Không thể lưu phiếu xuất kho!");
                }

                int stockExitIdInt = Convert.ToInt32(stockExitId);

                // Lưu chi tiết phiếu xuất kho và cập nhật số lượng tồn kho
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    // Lưu chi tiết phiếu
                    string insertDetailQuery = @"
                        INSERT INTO stock_exit_details (stock_exit_id, product_id, quantity, unit_price)
                        VALUES (@stockExitId, @productId, @quantity, @unitPrice)";

                    decimal unitPrice = Convert.ToDecimal(row.Cells["colPrice"].Value.ToString().Replace(",", ""));
                    int quantity = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    string maSanPham = row.Cells["colProductID"].Value.ToString();
                    int productId = int.Parse(maSanPham.Replace("SP", ""));

                    Dictionary<string, object> detailParams = new Dictionary<string, object>
                    {
                        { "@stockExitId", stockExitIdInt },
                        { "@productId", productId },
                        { "@quantity", quantity },
                        { "@unitPrice", unitPrice }
                    };

                    DatabaseHelper.ExecuteNonQuery(insertDetailQuery, detailParams, transaction, connection);

                    // Cập nhật số lượng tồn kho
                    string updateStockQuery = @"
                        UPDATE products 
                        SET stock_quantity = stock_quantity - @quantity 
                        WHERE id = @productId";

                    Dictionary<string, object> updateStockParams = new Dictionary<string, object>
                    {
                        { "@quantity", quantity },
                        { "@productId", productId }
                    };

                    DatabaseHelper.ExecuteNonQuery(updateStockQuery, updateStockParams, transaction, connection);
                }

                // Commit transaction nếu tất cả thành công
                transaction.Commit();

                MessageBox.Show("Lưu phiếu xuất kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
            catch (Exception ex)
            {
                // Rollback transaction nếu có lỗi
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private int GetOrCreateCustomer(string customerName)
        {
            try
            {
                Product selectedCustomer = cbCustomer.SelectedItem as Product;
                if (selectedCustomer != null && selectedCustomer.TenSanPham == customerName)
                {
                    return selectedCustomer.Id;
                }

                string checkQuery = "SELECT id FROM customers WHERE name = @name";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@name", customerName } };
                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }

                string insertQuery = "INSERT INTO customers (name) VALUES (@name); SELECT LAST_INSERT_ID();";
                Dictionary<string, object> insertParams = new Dictionary<string, object> { { "@name", customerName } };
                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

                cbCustomer.Items.Add(new Product { Id = newId, TenSanPham = customerName });
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int GetOrCreateUser(string fullName)
        {
            try
            {
                Product selectedUser = cbExporter.SelectedItem as Product;
                if (selectedUser != null && selectedUser.TenSanPham == fullName)
                {
                    return selectedUser.Id;
                }

                string checkQuery = "SELECT id, username FROM users WHERE full_name = @fullName";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@fullName", fullName } };
                DataTable dt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);

                if (dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["id"]);
                }

                string username = fullName.ToLower().Replace(" ", "");
                string insertQuery = "INSERT INTO users (username, full_name) VALUES (@username, @fullName); SELECT LAST_INSERT_ID();";
                Dictionary<string, object> insertParams = new Dictionary<string, object>
                {
                    { "@username", username },
                    { "@fullName", fullName }
                };
                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

                cbExporter.Items.Add(new Product { Id = newId, TenSanPham = fullName });
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int GetOrCreateWarehouse(string warehouseName)
        {
            try
            {
                Product selectedWarehouse = cbWarehouse.SelectedItem as Product;
                if (selectedWarehouse != null && selectedWarehouse.TenSanPham == warehouseName)
                {
                    return selectedWarehouse.Id;
                }

                string checkQuery = "SELECT id FROM warehouses WHERE name = @name";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@name", warehouseName } };
                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }

                string insertQuery = "INSERT INTO warehouses (name) VALUES (@name); SELECT LAST_INSERT_ID();";
                Dictionary<string, object> insertParams = new Dictionary<string, object> { { "@name", warehouseName } };
                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

                cbWarehouse.Items.Add(new Product { Id = newId, TenSanPham = warehouseName });
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private void InitializeCustomComponents()
        {
            mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(260, 30, 15, 10)
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            InitializeHeaderPanel();
            mainContainer.Controls.Add(headerPanel, 0, 0);

            InitializeExportInfoGroup();
            mainContainer.Controls.Add(gbExportInfo, 0, 1);

            InitializeProductsPanel();
            mainContainer.Controls.Add(productsPanel, 0, 2);

            this.Controls.Add(mainContainer);
        }

        private void InitializeHeaderPanel()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 25, 0, 0)
            };

            lblTitle = new Label
            {
                Text = "Xuất Kho",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, 8)
            };

            btnAdd = new Button
            {
                Text = "Thêm mới",
                BackColor = SystemColors.Control,
                ForeColor = Color.Black,
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 110, 15)
            };
            btnAdd.FlatAppearance.BorderSize = 0;

            btnReset = new Button
            {
                Text = "Làm mới",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 230, 15)
            };
            btnReset.FlatAppearance.BorderSize = 0;

            btnHistory = new Button
            {
                Text = "Lịch sử xuất kho",
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(150, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 350, 15),
            };
            btnHistory.FlatAppearance.BorderSize = 0;

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnAdd);
            headerPanel.Controls.Add(btnReset);
            headerPanel.Controls.Add(btnHistory);

            headerPanel.Resize += (sender, e) =>
            {
                btnAdd.Location = new Point(headerPanel.Width - 110, 10);
                btnReset.Location = new Point(headerPanel.Width - 230, 10);
                btnHistory.Location = new Point(headerPanel.Width - 400, 10);
            };
        }

        private void InitializeExportInfoGroup()
        {
            gbExportInfo = new GroupBox
            {
                Text = "Thông tin phiếu xuất kho",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(15),
                Margin = new Padding(0, 15, 0, 15),
                BackColor = Color.WhiteSmoke
            };

            TableLayoutPanel tblExportInfo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 3,
                Padding = new Padding(5),
                BackColor = Color.White
            };

            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblExportCode = new Label { Text = "Mã phiếu:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            txtExportCode = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblExportDate = new Label { Text = "Ngày xuất:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            dtpExportDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Now, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblCustomer = new Label { Text = "Kh.hàng:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbCustomer = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            tblExportInfo.Controls.Add(lblExportCode, 0, 0);
            tblExportInfo.Controls.Add(txtExportCode, 1, 0);
            tblExportInfo.Controls.Add(lblExportDate, 2, 0);
            tblExportInfo.Controls.Add(dtpExportDate, 3, 0);
            tblExportInfo.Controls.Add(lblCustomer, 4, 0);
            tblExportInfo.Controls.Add(cbCustomer, 5, 0);

            lblExporter = new Label { Text = "Ng.xuất:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbExporter = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblWarehouse = new Label { Text = "Kho:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbWarehouse = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblStatus = new Label { Text = "Trạng thái:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            txtStatus = new TextBox { Dock = DockStyle.Fill, Text = "Chờ duyệt", Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            tblExportInfo.Controls.Add(lblExporter, 0, 1);
            tblExportInfo.Controls.Add(cbExporter, 1, 1);
            tblExportInfo.Controls.Add(lblWarehouse, 2, 1);
            tblExportInfo.Controls.Add(cbWarehouse, 3, 1);
            tblExportInfo.Controls.Add(lblStatus, 4, 1);
            tblExportInfo.Controls.Add(txtStatus, 5, 1);

            lblNote = new Label { Text = "Ghi chú:", Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.TopLeft, Margin = new Padding(0, 8, 0, 0), Font = new Font("Segoe UI", 10F) };
            txtNote = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                Text = "Xuất hàng đợt 1 tháng 4/2025",
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0, 5, 10, 5),
                Font = new Font("Segoe UI", 10F)
            };

            tblExportInfo.Controls.Add(lblNote, 0, 2);
            tblExportInfo.SetColumnSpan(txtNote, 5);
            tblExportInfo.Controls.Add(txtNote, 1, 2);

            gbExportInfo.Controls.Add(tblExportInfo);
        }

        private void InitializeProductsPanel()
        {
            productsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.White,
                Margin = new Padding(0, 5, 0, 0)
            };

            productsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            productsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            productsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                Font = new Font("Segoe UI", 9.5F),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                RowTemplate = { Height = 40 },
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                AllowUserToOrderColumns = false
            };

            dgvProducts.ReadOnly = false;

            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvProducts.ColumnHeadersHeight = 35;
            dgvProducts.EnableHeadersVisualStyles = false;

            dgvProducts.Columns.Add("colProductID", "Mã SP");
            dgvProducts.Columns["colProductID"].ReadOnly = true;
            dgvProducts.Columns["colProductID"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns["colProductID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProducts.Columns.Add("colProductName", "Tên Sản phẩm");
            dgvProducts.Columns["colProductName"].ReadOnly = true;
            dgvProducts.Columns["colProductName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProducts.Columns.Add("colPrice", "Giá xuất (VND)");
            dgvProducts.Columns["colPrice"].ReadOnly = true;
            dgvProducts.Columns["colPrice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns["colPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvProducts.Columns.Add("colQuantity", "Số lượng");
            dgvProducts.Columns["colQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns["colQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvProducts.Columns.Add("colTotal", "Thành tiền");
            dgvProducts.Columns["colTotal"].ReadOnly = true;
            dgvProducts.Columns["colTotal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.Columns["colTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvProducts.Columns.Add("colDelete", "Xóa");

            dgvProducts.Columns["colProductID"].Width = 30;
            dgvProducts.Columns["colProductName"].Width = 90;
            dgvProducts.Columns["colPrice"].Width = 40;
            dgvProducts.Columns["colQuantity"].Width = 30;
            dgvProducts.Columns["colTotal"].Width = 40;

            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
            btnColumn.HeaderText = "Xóa";
            btnColumn.Text = "Xóa";
            btnColumn.Name = "colDelete";
            btnColumn.UseColumnTextForButtonValue = true;
            btnColumn.FlatStyle = FlatStyle.Flat;
            btnColumn.DefaultCellStyle.BackColor = Color.Firebrick;
            btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            btnColumn.DefaultCellStyle.ForeColor = Color.White;
            btnColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

            dgvProducts.Columns.RemoveAt(5);
            dgvProducts.Columns.Add(btnColumn);
            dgvProducts.Columns["colDelete"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvProducts.Columns["colDelete"].Width = 100;

            dgvProducts.ColumnHeadersVisible = false;

            TableLayoutPanel bottomPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White,
                Margin = new Padding(0, 5, 0, 0)
            };

            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            TableLayoutPanel totalsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 5, 0, 0),
                ColumnCount = 2,
                RowCount = 2,
                AutoSize = true
            };

            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            totalsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            totalsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            lblTotalQuantity = new Label
            {
                Text = "Tổng số lượng:",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Padding = new Padding(40, 10, 10, 0),
                Margin = new Padding(0)
            };

            lblTotalQuantityValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0, 10, 0, 0)
            };

            lblTotalAmount = new Label
            {
                Text = "Tổng giá trị:",
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Padding = new Padding(40, 0, 10, 20),
                Margin = new Padding(0, 0, 0, 0)
            };

            lblTotalAmountValue = new Label
            {
                Text = "0 VND",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 0),
                Padding = new Padding(0, 0, 0, 20)
            };

            totalsPanel.Controls.Add(lblTotalQuantity, 0, 0);
            totalsPanel.Controls.Add(lblTotalQuantityValue, 1, 0);
            totalsPanel.Controls.Add(lblTotalAmount, 0, 1);
            totalsPanel.Controls.Add(lblTotalAmountValue, 1, 1);

            TableLayoutPanel buttonsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 5, 0, 0)
            };

            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            btnComplete = new Button
            {
                Text = "Hoàn thành",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F)
            };
            btnComplete.FlatAppearance.BorderSize = 0;

            btnCancel = new Button
            {
                Text = "Hủy",
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F)
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            btnComplete.Dock = DockStyle.None;
            btnComplete.Anchor = AnchorStyles.None;
            btnComplete.Size = new Size(120, 45);

            btnCancel.Dock = DockStyle.None;
            btnCancel.Anchor = AnchorStyles.None;
            btnCancel.Size = new Size(120, 45);

            buttonsPanel.Padding = new Padding(0, 10, 0, 5);
            buttonsPanel.Controls.Add(btnComplete, 0, 0);
            buttonsPanel.Controls.Add(btnCancel, 1, 0);

            bottomPanel.Controls.Add(totalsPanel, 0, 0);
            bottomPanel.Controls.Add(buttonsPanel, 1, 0);

            productsPanel.Controls.Add(dgvProducts, 0, 0);
            productsPanel.Controls.Add(bottomPanel, 0, 1);
        }

        private void ExportWareHouseForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1212, 830);
        }
    }
}






// Code chính

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Windows.Forms;
//using WareHouse.DataAccess;
//using WareHouse.Models;
//using WareHouse.Utils;
//using MySql.Data.MySqlClient; // Thêm thư viện để sử dụng transaction

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ExportWareHouseForm : BaseForm
//    {
//        private TableLayoutPanel mainContainer;
//        private Panel headerPanel;
//        private Label lblTitle;
//        private Button btnAdd;
//        private Button btnReset;
//        private GroupBox gbExportInfo;
//        private Label lblExportCode;
//        private TextBox txtExportCode;
//        private Label lblExportDate;
//        private DateTimePicker dtpExportDate;
//        private Label lblCustomer;
//        private ComboBox cbCustomer;
//        private Label lblExporter;
//        private ComboBox cbExporter;
//        private Label lblWarehouse;
//        private ComboBox cbWarehouse;
//        private Label lblStatus;
//        private TextBox txtStatus;
//        private Label lblNote;
//        private TextBox txtNote;
//        private DataGridView dgvProducts;
//        private Button btnComplete;
//        private Button btnCancel;
//        private Label lblTotalQuantity;
//        private Label lblTotalQuantityValue;
//        private Label lblTotalAmount;
//        private Label lblTotalAmountValue;
//        private TableLayoutPanel productsPanel;

//        private List<ExportProductWareHouse> danhSachTam = new List<ExportProductWareHouse>();

//        public ExportWareHouseForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (!IsValidRole())
//            {
//                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Xuất Kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                this.Close();
//                return;
//            }

//            InitializeCustomComponents();
//            SetupEventHandlers();
//            LoadComboBoxData();
//            ResetForm();
//        }

//        private bool IsValidRole()
//        {
//            return RoleId == 2; // Chỉ cho phép role Nhân viên kho (roleId = 2)
//        }

//        private void SetupEventHandlers()
//        {
//            btnAdd.Click += BtnAdd_Click;
//            btnReset.Click += BtnReset_Click;
//            btnComplete.Click += BtnComplete_Click;
//            btnCancel.Click += BtnCancel_Click;
//            dgvProducts.CellClick += DgvProducts_CellClick;
//            dgvProducts.CellValueChanged += DgvProducts_CellValueChanged;
//        }

//        private void LoadComboBoxData()
//        {
//            LoadCustomers();
//            LoadUsers();
//            LoadWarehouses();
//        }

//        private void LoadCustomers()
//        {
//            try
//            {
//                string query = "SELECT id, name FROM customers";
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);

//                cbCustomer.Items.Clear();
//                foreach (DataRow row in dt.Rows)
//                {
//                    cbCustomer.Items.Add(new Product
//                    {
//                        Id = Convert.ToInt32(row["id"]),
//                        TenSanPham = row["name"].ToString()
//                    });
//                }
//                cbCustomer.DropDownStyle = ComboBoxStyle.DropDown;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải danh sách khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void LoadUsers()
//        {
//            try
//            {
//                string query = "SELECT id, username, full_name FROM users";
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);

//                cbExporter.Items.Clear();
//                foreach (DataRow row in dt.Rows)
//                {
//                    cbExporter.Items.Add(new Product
//                    {
//                        Id = Convert.ToInt32(row["id"]),
//                        TenSanPham = row["full_name"].ToString()
//                    });
//                }
//                cbExporter.DropDownStyle = ComboBoxStyle.DropDown;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void LoadWarehouses()
//        {
//            try
//            {
//                string query = "SELECT id, name FROM warehouses";
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);

//                cbWarehouse.Items.Clear();
//                foreach (DataRow row in dt.Rows)
//                {
//                    cbWarehouse.Items.Add(new Product
//                    {
//                        Id = Convert.ToInt32(row["id"]),
//                        TenSanPham = row["name"].ToString()
//                    });
//                }
//                cbWarehouse.DropDownStyle = ComboBoxStyle.DropDown;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải danh sách kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private string GenerateExitCode()
//        {
//            try
//            {
//                string query = "SELECT MAX(id) FROM stock_exit_headers";
//                object result = DatabaseHelper.ExecuteScalar(query, null);

//                int maxId = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
//                int newId = maxId + 1;

//                if (newId < 10)
//                {
//                    return $"XK00{newId}";
//                }
//                else if (newId < 100)
//                {
//                    return $"XK0{newId}";
//                }
//                else
//                {
//                    return $"XK{newId}";
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tạo mã phiếu xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return "XK001";
//            }
//        }

//        private void BtnAdd_Click(object sender, EventArgs e)
//        {
//            btnAdd.BackColor = Color.MediumSeaGreen;

//            using (AddExportWareHouse form = new AddExportWareHouse())
//            {
//                if (form.ShowDialog() == DialogResult.OK)
//                {
//                    danhSachTam.Add(form.SanPhamMoi);
//                    UpdateDataGridView();
//                }
//            }
//        }

//        private void BtnReset_Click(object sender, EventArgs e)
//        {
//            ResetForm();
//        }

//        private void ResetForm()
//        {
//            danhSachTam.Clear();
//            UpdateDataGridView();

//            txtExportCode.Text = GenerateExitCode();
//            dtpExportDate.Value = DateTime.Now;
//            cbCustomer.Text = cbCustomer.Items.Count > 0 ? (cbCustomer.Items[0] as Product).TenSanPham : "Công ty TNHH Việt Nam";
//            cbExporter.Text = cbExporter.Items.Count > 0 ? (cbExporter.Items[0] as Product).TenSanPham : "Nguyễn Văn A";
//            cbWarehouse.Text = cbWarehouse.Items.Count > 0 ? (cbWarehouse.Items[0] as Product).TenSanPham : "Kho chính";
//            txtStatus.Text = "Chờ duyệt";
//            txtNote.Text = "Xuất hàng đợt 1 tháng 4/2025";

//            btnAdd.BackColor = SystemColors.Control;
//        }

//        private void BtnCancel_Click(object sender, EventArgs e)
//        {
//            if (danhSachTam.Count > 0)
//            {
//                DialogResult result = MessageBox.Show("Dữ liệu chưa được lưu. Bạn có chắc chắn muốn hủy?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                if (result == DialogResult.No)
//                {
//                    return;
//                }
//            }
//            this.Close();
//        }

//        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.ColumnIndex == dgvProducts.Columns["colDelete"].Index && e.RowIndex >= 0)
//            {
//                danhSachTam.RemoveAt(e.RowIndex);
//                UpdateDataGridView();
//            }
//        }

//        private void DgvProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0)
//            {
//                int rowIndex = e.RowIndex;
//                if (e.ColumnIndex == dgvProducts.Columns["colQuantity"].Index)
//                {
//                    try
//                    {
//                        decimal giaXuat = Convert.ToDecimal(dgvProducts.Rows[rowIndex].Cells["colPrice"].Value.ToString().Replace(",", ""));
//                        int soLuong = Convert.ToInt32(dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value);

//                        // Kiểm tra số lượng tồn kho
//                        int productId = int.Parse(dgvProducts.Rows[rowIndex].Cells["colProductID"].Value.ToString().Replace("SP", ""));
//                        string query = "SELECT stock_quantity FROM products WHERE id = @id";
//                        Dictionary<string, object> paramsDict = new Dictionary<string, object> { { "@id", productId } };
//                        object result = DatabaseHelper.ExecuteScalar(query, paramsDict);
//                        int stockQuantity = result != null ? Convert.ToInt32(result) : 0;

//                        if (soLuong > stockQuantity)
//                        {
//                            MessageBox.Show($"Số lượng xuất vượt quá số lượng tồn kho ({stockQuantity})!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                            soLuong = stockQuantity;
//                            dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value = soLuong;
//                        }

//                        danhSachTam[rowIndex].SoLuong = soLuong;

//                        decimal thanhTien = giaXuat * soLuong;
//                        dgvProducts.Rows[rowIndex].Cells["colTotal"].Value = thanhTien.ToString("N0") + " VND";

//                        UpdateTotals();
//                    }
//                    catch
//                    {
//                        MessageBox.Show("Dữ liệu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    }
//                }
//            }
//        }

//        private string FormatProductCode(int id)
//        {
//            if (id < 10)
//            {
//                return $"SP00{id}";
//            }
//            else if (id < 100)
//            {
//                return $"SP0{id}";
//            }
//            else
//            {
//                return $"SP{id}";
//            }
//        }

//        private void UpdateDataGridView()
//        {
//            dgvProducts.Rows.Clear();
//            if (danhSachTam.Count == 0)
//            {
//                dgvProducts.ColumnHeadersVisible = false;
//            }
//            else
//            {
//                dgvProducts.ColumnHeadersVisible = true;
//                foreach (var sp in danhSachTam)
//                {
//                    decimal thanhTien = sp.GiaXuat * sp.SoLuong;
//                    string maSanPham = FormatProductCode(sp.Id);
//                    dgvProducts.Rows.Add(maSanPham, sp.TenSanPham, sp.GiaXuat.ToString("N0"), sp.SoLuong, thanhTien.ToString("N0") + " VND", "Xóa");
//                }
//            }
//            UpdateTotals();
//        }

//        private void UpdateTotals()
//        {
//            int totalQuantity = 0;
//            decimal totalAmount = 0;

//            foreach (var sp in danhSachTam)
//            {
//                totalQuantity += sp.SoLuong;
//                totalAmount += sp.GiaXuat * sp.SoLuong;
//            }

//            lblTotalQuantityValue.Text = totalQuantity.ToString();
//            lblTotalAmountValue.Text = totalAmount.ToString("N0") + " VND";
//        }

//        private void BtnComplete_Click(object sender, EventArgs e)
//        {
//            if (danhSachTam.Count == 0)
//            {
//                MessageBox.Show("Chưa có sản phẩm nào để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            if (string.IsNullOrWhiteSpace(txtExportCode.Text) || string.IsNullOrWhiteSpace(cbCustomer.Text) ||
//                string.IsNullOrWhiteSpace(cbExporter.Text) || string.IsNullOrWhiteSpace(cbWarehouse.Text) ||
//                string.IsNullOrWhiteSpace(txtStatus.Text))
//            {
//                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phiếu xuất kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // Kiểm tra số lượng tồn kho trước khi lưu
//            foreach (var sp in danhSachTam)
//            {
//                string query = "SELECT stock_quantity FROM products WHERE id = @id";
//                Dictionary<string, object> paramsDict = new Dictionary<string, object> { { "@id", sp.Id } };
//                object result = DatabaseHelper.ExecuteScalar(query, paramsDict);
//                int stockQuantity = result != null ? Convert.ToInt32(result) : 0;

//                if (sp.SoLuong > stockQuantity)
//                {
//                    MessageBox.Show($"Số lượng xuất của sản phẩm {sp.TenSanPham} vượt quá tồn kho ({stockQuantity})!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    return;
//                }
//            }

//            MySqlTransaction transaction = null;
//            MySqlConnection connection = null;

//            try
//            {
//                // Mở kết nối và bắt đầu transaction
//                connection = DatabaseHelper.GetConnection();
//                transaction = connection.BeginTransaction();

//                int customerId = GetOrCreateCustomer(cbCustomer.Text);
//                int userId = GetOrCreateUser(cbExporter.Text);
//                int warehouseId = GetOrCreateWarehouse(cbWarehouse.Text);

//                if (customerId == -1 || userId == -1 || warehouseId == -1)
//                {
//                    throw new Exception("Không thể lấy hoặc tạo ID cho khách hàng, người dùng hoặc kho!");
//                }

//                // Lưu thông tin phiếu xuất kho
//                string insertHeaderQuery = @"
//                    INSERT INTO stock_exit_headers (exit_code, exit_date, customer_id, warehouse_id, user_id, note, status)
//                    VALUES (@exitCode, @exitDate, @customerId, @warehouseId, @userId, @note, @status);
//                    SELECT LAST_INSERT_ID();";

//                Dictionary<string, object> headerParams = new Dictionary<string, object>
//                {
//                    { "@exitCode", txtExportCode.Text },
//                    { "@exitDate", dtpExportDate.Value },
//                    { "@customerId", customerId },
//                    { "@warehouseId", warehouseId },
//                    { "@userId", userId },
//                    { "@note", txtNote.Text },
//                    { "@status", txtStatus.Text }
//                };

//                object stockExitId = DatabaseHelper.ExecuteScalar(insertHeaderQuery, headerParams, transaction, connection);

//                if (stockExitId == null)
//                {
//                    throw new Exception("Không thể lưu phiếu xuất kho!");
//                }

//                int stockExitIdInt = Convert.ToInt32(stockExitId);

//                // Lưu chi tiết phiếu xuất kho và cập nhật số lượng tồn kho
//                foreach (DataGridViewRow row in dgvProducts.Rows)
//                {
//                    // Lưu chi tiết phiếu
//                    string insertDetailQuery = @"
//                        INSERT INTO stock_exit_details (stock_exit_id, product_id, quantity, unit_price)
//                        VALUES (@stockExitId, @productId, @quantity, @unitPrice)";

//                    decimal unitPrice = Convert.ToDecimal(row.Cells["colPrice"].Value.ToString().Replace(",", ""));
//                    int quantity = Convert.ToInt32(row.Cells["colQuantity"].Value);
//                    string maSanPham = row.Cells["colProductID"].Value.ToString();
//                    int productId = int.Parse(maSanPham.Replace("SP", ""));

//                    Dictionary<string, object> detailParams = new Dictionary<string, object>
//                    {
//                        { "@stockExitId", stockExitIdInt },
//                        { "@productId", productId },
//                        { "@quantity", quantity },
//                        { "@unitPrice", unitPrice }
//                    };

//                    DatabaseHelper.ExecuteNonQuery(insertDetailQuery, detailParams, transaction, connection);

//                    // Cập nhật số lượng tồn kho
//                    string updateStockQuery = @"
//                        UPDATE products 
//                        SET stock_quantity = stock_quantity - @quantity 
//                        WHERE id = @productId";

//                    Dictionary<string, object> updateStockParams = new Dictionary<string, object>
//                    {
//                        { "@quantity", quantity },
//                        { "@productId", productId }
//                    };

//                    DatabaseHelper.ExecuteNonQuery(updateStockQuery, updateStockParams, transaction, connection);
//                }

//                // Commit transaction nếu tất cả thành công
//                transaction.Commit();

//                MessageBox.Show("Lưu phiếu xuất kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                ResetForm();
//            }
//            catch (Exception ex)
//            {
//                // Rollback transaction nếu có lỗi
//                //if (transaction != null)
//                //{
//                //    transaction.Rollback();
//                //}
//                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private int GetOrCreateCustomer(string customerName)
//        {
//            try
//            {
//                Product selectedCustomer = cbCustomer.SelectedItem as Product;
//                if (selectedCustomer != null && selectedCustomer.TenSanPham == customerName)
//                {
//                    return selectedCustomer.Id;
//                }

//                string checkQuery = "SELECT id FROM customers WHERE name = @name";
//                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@name", customerName } };
//                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

//                if (result != null && result != DBNull.Value)
//                {
//                    return Convert.ToInt32(result);
//                }

//                string insertQuery = "INSERT INTO customers (name) VALUES (@name); SELECT LAST_INSERT_ID();";
//                Dictionary<string, object> insertParams = new Dictionary<string, object> { { "@name", customerName } };
//                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

//                cbCustomer.Items.Add(new Product { Id = newId, TenSanPham = customerName });
//                return newId;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi xử lý khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return -1;
//            }
//        }

//        private int GetOrCreateUser(string fullName)
//        {
//            try
//            {
//                Product selectedUser = cbExporter.SelectedItem as Product;
//                if (selectedUser != null && selectedUser.TenSanPham == fullName)
//                {
//                    return selectedUser.Id;
//                }

//                string checkQuery = "SELECT id, username FROM users WHERE full_name = @fullName";
//                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@fullName", fullName } };
//                DataTable dt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);

//                if (dt.Rows.Count > 0)
//                {
//                    return Convert.ToInt32(dt.Rows[0]["id"]);
//                }

//                string username = fullName.ToLower().Replace(" ", "");
//                string insertQuery = "INSERT INTO users (username, full_name) VALUES (@username, @fullName); SELECT LAST_INSERT_ID();";
//                Dictionary<string, object> insertParams = new Dictionary<string, object>
//                {
//                    { "@username", username },
//                    { "@fullName", fullName }
//                };
//                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

//                cbExporter.Items.Add(new Product { Id = newId, TenSanPham = fullName });
//                return newId;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi xử lý người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return -1;
//            }
//        }

//        private int GetOrCreateWarehouse(string warehouseName)
//        {
//            try
//            {
//                Product selectedWarehouse = cbWarehouse.SelectedItem as Product;
//                if (selectedWarehouse != null && selectedWarehouse.TenSanPham == warehouseName)
//                {
//                    return selectedWarehouse.Id;
//                }

//                string checkQuery = "SELECT id FROM warehouses WHERE name = @name";
//                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@name", warehouseName } };
//                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

//                if (result != null && result != DBNull.Value)
//                {
//                    return Convert.ToInt32(result);
//                }

//                string insertQuery = "INSERT INTO warehouses (name) VALUES (@name); SELECT LAST_INSERT_ID();";
//                Dictionary<string, object> insertParams = new Dictionary<string, object> { { "@name", warehouseName } };
//                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

//                cbWarehouse.Items.Add(new Product { Id = newId, TenSanPham = warehouseName });
//                return newId;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi xử lý kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return -1;
//            }
//        }

//        private void InitializeCustomComponents()
//        {
//            mainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 1,
//                RowCount = 3,
//                BackColor = Color.WhiteSmoke,
//                Padding = new Padding(260, 30, 15, 10)
//            };

//            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 300F));
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

//            InitializeHeaderPanel();
//            mainContainer.Controls.Add(headerPanel, 0, 0);

//            InitializeExportInfoGroup();
//            mainContainer.Controls.Add(gbExportInfo, 0, 1);

//            InitializeProductsPanel();
//            mainContainer.Controls.Add(productsPanel, 0, 2);

//            this.Controls.Add(mainContainer);
//        }

//        private void InitializeHeaderPanel()
//        {
//            headerPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 25, 0, 0)
//            };

//            lblTitle = new Label
//            {
//                Text = "Xuất Kho",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Location = new Point(10, 8)
//            };

//            btnAdd = new Button
//            {
//                Text = "Thêm mới",
//                BackColor = SystemColors.Control,
//                ForeColor = Color.Black,
//                Size = new Size(100, 35),
//                FlatStyle = FlatStyle.Flat,
//                Font = new Font("Segoe UI", 10F),
//                Location = new Point(headerPanel.Width - 110, 15)
//            };
//            btnAdd.FlatAppearance.BorderSize = 0;

//            btnReset = new Button
//            {
//                Text = "Làm mới",
//                BackColor = Color.DodgerBlue,
//                ForeColor = Color.White,
//                Size = new Size(100, 35),
//                FlatStyle = FlatStyle.Flat,
//                Font = new Font("Segoe UI", 10F),
//                Location = new Point(headerPanel.Width - 230, 23)
//            };
//            btnReset.FlatAppearance.BorderSize = 0;

//            headerPanel.Controls.Add(lblTitle);
//            headerPanel.Controls.Add(btnAdd);
//            headerPanel.Controls.Add(btnReset);

//            headerPanel.Resize += (sender, e) =>
//            {
//                btnAdd.Location = new Point(headerPanel.Width - 110, 9);
//                btnReset.Location = new Point(headerPanel.Width - 230, 9);
//            };
//        }

//        private void InitializeExportInfoGroup()
//        {
//            gbExportInfo = new GroupBox
//            {
//                Text = "Thông tin phiếu xuất kho",
//                Dock = DockStyle.Fill,
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                Padding = new Padding(15),
//                Margin = new Padding(0, 15, 0, 15),
//                BackColor = Color.WhiteSmoke
//            };

//            TableLayoutPanel tblExportInfo = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 6,
//                RowCount = 3,
//                Padding = new Padding(5),
//                BackColor = Color.White
//            };

//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
//            tblExportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

//            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
//            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
//            tblExportInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

//            lblExportCode = new Label { Text = "Mã phiếu:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            txtExportCode = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            lblExportDate = new Label { Text = "Ngày xuất:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            dtpExportDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Now, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            lblCustomer = new Label { Text = "Kh.hàng:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            cbCustomer = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            tblExportInfo.Controls.Add(lblExportCode, 0, 0);
//            tblExportInfo.Controls.Add(txtExportCode, 1, 0);
//            tblExportInfo.Controls.Add(lblExportDate, 2, 0);
//            tblExportInfo.Controls.Add(dtpExportDate, 3, 0);
//            tblExportInfo.Controls.Add(lblCustomer, 4, 0);
//            tblExportInfo.Controls.Add(cbCustomer, 5, 0);

//            lblExporter = new Label { Text = "Ng.xuất:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            cbExporter = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            lblWarehouse = new Label { Text = "Kho:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            cbWarehouse = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            lblStatus = new Label { Text = "Trạng thái:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
//            txtStatus = new TextBox { Dock = DockStyle.Fill, Text = "Chờ duyệt", Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

//            tblExportInfo.Controls.Add(lblExporter, 0, 1);
//            tblExportInfo.Controls.Add(cbExporter, 1, 1);
//            tblExportInfo.Controls.Add(lblWarehouse, 2, 1);
//            tblExportInfo.Controls.Add(cbWarehouse, 3, 1);
//            tblExportInfo.Controls.Add(lblStatus, 4, 1);
//            tblExportInfo.Controls.Add(txtStatus, 5, 1);

//            lblNote = new Label { Text = "Ghi chú:", Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.TopLeft, Margin = new Padding(0, 8, 0, 0), Font = new Font("Segoe UI", 10F) };
//            txtNote = new TextBox
//            {
//                Dock = DockStyle.Fill,
//                Multiline = true,
//                Text = "Xuất hàng đợt 1 tháng 4/2025",
//                ScrollBars = ScrollBars.Vertical,
//                Margin = new Padding(0, 5, 10, 5),
//                Font = new Font("Segoe UI", 10F)
//            };

//            tblExportInfo.Controls.Add(lblNote, 0, 2);
//            tblExportInfo.SetColumnSpan(txtNote, 5);
//            tblExportInfo.Controls.Add(txtNote, 1, 2);

//            gbExportInfo.Controls.Add(tblExportInfo);
//        }

//        private void InitializeProductsPanel()
//        {
//            productsPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 1,
//                RowCount = 2,
//                BackColor = Color.White,
//                Margin = new Padding(0, 5, 0, 0)
//            };

//            productsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
//            productsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
//            productsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));

//            dgvProducts = new DataGridView
//            {
//                Dock = DockStyle.Fill,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                BackgroundColor = Color.White,
//                BorderStyle = BorderStyle.None,
//                RowHeadersVisible = false,
//                AllowUserToAddRows = false,
//                Font = new Font("Segoe UI", 9.5F),
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
//                RowTemplate = { Height = 40 },
//                AllowUserToResizeColumns = false,
//                AllowUserToResizeRows = false,
//                AllowUserToOrderColumns = false
//            };

//            dgvProducts.ReadOnly = false;

//            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
//            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//            dgvProducts.ColumnHeadersHeight = 35;
//            dgvProducts.EnableHeadersVisualStyles = false;

//            dgvProducts.Columns.Add("colProductID", "Mã SP");
//            dgvProducts.Columns["colProductID"].ReadOnly = true;
//            dgvProducts.Columns["colProductID"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProducts.Columns["colProductID"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            dgvProducts.Columns.Add("colProductName", "Tên Sản phẩm");
//            dgvProducts.Columns["colProductName"].ReadOnly = true;
//            dgvProducts.Columns["colProductName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            dgvProducts.Columns.Add("colPrice", "Giá xuất (VND)");
//            dgvProducts.Columns["colPrice"].ReadOnly = true;
//            dgvProducts.Columns["colPrice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProducts.Columns["colPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

//            dgvProducts.Columns.Add("colQuantity", "Số lượng");
//            dgvProducts.Columns["colQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProducts.Columns["colQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            dgvProducts.Columns.Add("colTotal", "Thành tiền");
//            dgvProducts.Columns["colTotal"].ReadOnly = true;
//            dgvProducts.Columns["colTotal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProducts.Columns["colTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

//            dgvProducts.Columns.Add("colDelete", "Xóa");

//            dgvProducts.Columns["colProductID"].Width = 30;
//            dgvProducts.Columns["colProductName"].Width = 90;
//            dgvProducts.Columns["colPrice"].Width = 40;
//            dgvProducts.Columns["colQuantity"].Width = 30;
//            dgvProducts.Columns["colTotal"].Width = 40;

//            DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
//            btnColumn.HeaderText = "Xóa";
//            btnColumn.Text = "Xóa";
//            btnColumn.Name = "colDelete";
//            btnColumn.UseColumnTextForButtonValue = true;
//            btnColumn.FlatStyle = FlatStyle.Flat;
//            btnColumn.DefaultCellStyle.BackColor = Color.Firebrick;
//            btnColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            btnColumn.DefaultCellStyle.ForeColor = Color.White;
//            btnColumn.DefaultCellStyle.Font = new Font("Segoe UI", 9F);

//            dgvProducts.Columns.RemoveAt(5);
//            dgvProducts.Columns.Add(btnColumn);
//            dgvProducts.Columns["colDelete"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
//            dgvProducts.Columns["colDelete"].Width = 100;

//            dgvProducts.ColumnHeadersVisible = false;

//            TableLayoutPanel bottomPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 2,
//                RowCount = 1,
//                BackColor = Color.White,
//                Margin = new Padding(0, 5, 0, 0)
//            };

//            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            bottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

//            TableLayoutPanel totalsPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                Margin = new Padding(0, 5, 0, 0),
//                ColumnCount = 2,
//                RowCount = 2,
//                AutoSize = true
//            };

//            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
//            totalsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
//            totalsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            totalsPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

//            lblTotalQuantity = new Label
//            {
//                Text = "Tổng số lượng:",
//                TextAlign = ContentAlignment.MiddleRight,
//                Dock = DockStyle.Fill,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Padding = new Padding(40, 10, 10, 0),
//                Margin = new Padding(0)
//            };

//            lblTotalQuantityValue = new Label
//            {
//                Text = "0",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                TextAlign = ContentAlignment.MiddleRight,
//                Dock = DockStyle.Fill,
//                AutoSize = true,
//                Margin = new Padding(0),
//                Padding = new Padding(0, 10, 0, 0)
//            };

//            lblTotalAmount = new Label
//            {
//                Text = "Tổng giá trị:",
//                TextAlign = ContentAlignment.MiddleRight,
//                Dock = DockStyle.Fill,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Padding = new Padding(40, 0, 10, 20),
//                Margin = new Padding(0, 0, 0, 0)
//            };

//            lblTotalAmountValue = new Label
//            {
//                Text = "0 VND",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                TextAlign = ContentAlignment.MiddleRight,
//                Dock = DockStyle.Fill,
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 0),
//                Padding = new Padding(0, 0, 0, 20)
//            };

//            totalsPanel.Controls.Add(lblTotalQuantity, 0, 0);
//            totalsPanel.Controls.Add(lblTotalQuantityValue, 1, 0);
//            totalsPanel.Controls.Add(lblTotalAmount, 0, 1);
//            totalsPanel.Controls.Add(lblTotalAmountValue, 1, 1);

//            TableLayoutPanel buttonsPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 2,
//                RowCount = 1,
//                Margin = new Padding(0, 5, 0, 0)
//            };

//            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

//            btnComplete = new Button
//            {
//                Text = "Hoàn thành",
//                BackColor = Color.MediumSeaGreen,
//                ForeColor = Color.White,
//                Dock = DockStyle.Fill,
//                FlatStyle = FlatStyle.Flat,
//                Font = new Font("Segoe UI", 10F)
//            };
//            btnComplete.FlatAppearance.BorderSize = 0;

//            btnCancel = new Button
//            {
//                Text = "Hủy",
//                BackColor = Color.Firebrick,
//                ForeColor = Color.White,
//                Dock = DockStyle.Fill,
//                FlatStyle = FlatStyle.Flat,
//                Font = new Font("Segoe UI", 10F)
//            };
//            btnCancel.FlatAppearance.BorderSize = 0;

//            btnComplete.Dock = DockStyle.None;
//            btnComplete.Anchor = AnchorStyles.None;
//            btnComplete.Size = new Size(120, 45);

//            btnCancel.Dock = DockStyle.None;
//            btnCancel.Anchor = AnchorStyles.None;
//            btnCancel.Size = new Size(120, 45);

//            buttonsPanel.Padding = new Padding(0, 10, 0, 5);
//            buttonsPanel.Controls.Add(btnComplete, 0, 0);
//            buttonsPanel.Controls.Add(btnCancel, 1, 0);

//            bottomPanel.Controls.Add(totalsPanel, 0, 0);
//            bottomPanel.Controls.Add(buttonsPanel, 1, 0);

//            productsPanel.Controls.Add(dgvProducts, 0, 0);
//            productsPanel.Controls.Add(bottomPanel, 0, 1);
//        }

//        private void ExportWareHouseForm_Load(object sender, EventArgs e)
//        {
//            this.Size = new Size(1212, 830);
//        }
//    }
//}