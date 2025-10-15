using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WareHouse.DataAccess;
using WareHouse.Models;
using WareHouse.Utils;

namespace WareHouse.Presentation.Forms
{
    public partial class ImportWareHouseForm : BaseForm
    {
        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnAdd;
        private Button btnReset;
        private Button btnHistory;
        private GroupBox gbImportInfo;
        private Label lblImportCode;
        public TextBox txtImportCode;
        private Label lblImportDate;
        public DateTimePicker dtpImportDate;
        private Label lblSupplier;
        public ComboBox cbSupplier;
        private Label lblImporter;
        public ComboBox cbImporter;
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

        public List<Product> danhSachTam = new List<Product>();

        public ImportWareHouseForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình

            // Kiểm tra role trước khi hiển thị dữ liệu
            if (!IsValidRole())
            {
                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Nhập Kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            return RoleId == 2 || RoleId == 1; // Chỉ cho phép role Nhân viên kho (roleId = 2)
        }

        private void SetupEventHandlers()
        {
            btnAdd.Click += BtnAdd_Click;
            btnReset.Click += BtnReset_Click;
            btnComplete.Click += BtnComplete_Click;
            btnCancel.Click += BtnCancel_Click;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.CellValueChanged += DgvProducts_CellValueChanged;
            btnHistory.Click += BtnHistory_Click;
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            this.Hide(); // Ẩn form hiện tại (ImportWareHouseForm)
            ImportHistoryForm historyForm = new ImportHistoryForm(RoleId, this);
            historyForm.Show(); // Mở form ImportHistoryForm
        }

        private void LoadComboBoxData()
        {
            LoadSuppliers();
            LoadUsers();
            LoadWarehouses();
        }

        private void LoadSuppliers()
        {
            try
            {
                // Đảm bảo kết nối đã được khởi tạo
                DatabaseHelper.InitializeConnection();

                string query = "SELECT id, name FROM suppliers";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbSupplier.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbSupplier.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString()
                    });
                }
                cbSupplier.DropDownStyle = ComboBoxStyle.DropDown;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUsers()
        {
            try
            {
                // Đảm bảo kết nối đã được khởi tạo
                DatabaseHelper.InitializeConnection();

                string query = "SELECT id, username, full_name FROM users";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                cbImporter.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cbImporter.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["full_name"].ToString()
                    });
                }
                cbImporter.DropDownStyle = ComboBoxStyle.DropDown;
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
                // Đảm bảo kết nối đã được khởi tạo
                DatabaseHelper.InitializeConnection();

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

        private string GenerateEntryCode()
        {
            try
            {
                string query = "SELECT MAX(id) FROM stock_entry_headers";
                object result = DatabaseHelper.ExecuteScalar(query, null);

                int maxId = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                int newId = maxId + 1;

                if (newId < 10)
                {
                    return $"NK00{newId}";
                }
                else if (newId < 100)
                {
                    return $"NK0{newId}";
                }
                else
                {
                    return $"NK{newId}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo mã phiếu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "NK001";
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            btnAdd.BackColor = Color.MediumSeaGreen;

            using (AddWareHouse form = new AddWareHouse())
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

            txtImportCode.Text = GenerateEntryCode();
            dtpImportDate.Value = DateTime.Now;
            cbSupplier.Text = cbSupplier.Items.Count > 0 ? (cbSupplier.Items[0] as Product).TenSanPham : "Công ty TNHH ABC";
            cbImporter.Text = cbImporter.Items.Count > 0 ? (cbImporter.Items[0] as Product).TenSanPham : "Nguyễn Văn A";
            cbWarehouse.Text = cbWarehouse.Items.Count > 0 ? (cbWarehouse.Items[0] as Product).TenSanPham : "Kho chính";
            txtStatus.Text = "Chờ duyệt";
            txtNote.Text = "Nhập hàng đợt 1 tháng 4/2025";

            btnAdd.BackColor = SystemColors.Control;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
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
                        decimal giaNhap = Convert.ToDecimal(dgvProducts.Rows[rowIndex].Cells["colPrice"].Value.ToString().Replace(",", ""));
                        int soLuong = Convert.ToInt32(dgvProducts.Rows[rowIndex].Cells["colQuantity"].Value);

                        danhSachTam[rowIndex].SoLuong = soLuong;

                        decimal thanhTien = giaNhap * soLuong;
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
                    decimal thanhTien = sp.GiaNhap * sp.SoLuong;
                    string maSanPham = FormatProductCode(sp.Id);
                    dgvProducts.Rows.Add(maSanPham, sp.TenSanPham, sp.GiaNhap.ToString("N0"), sp.SoLuong, thanhTien.ToString("N0") + " VND", "Xóa");
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
                totalAmount += sp.GiaNhap * sp.SoLuong;
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

            if (string.IsNullOrWhiteSpace(txtImportCode.Text) || string.IsNullOrWhiteSpace(cbSupplier.Text) ||
                string.IsNullOrWhiteSpace(cbImporter.Text) || string.IsNullOrWhiteSpace(cbWarehouse.Text) ||
                string.IsNullOrWhiteSpace(txtStatus.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin phiếu nhập kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int supplierId = GetOrCreateSupplier(cbSupplier.Text);
                int userId = GetOrCreateUser(cbImporter.Text);
                int warehouseId = GetOrCreateWarehouse(cbWarehouse.Text);

                if (supplierId == -1 || userId == -1 || warehouseId == -1)
                {
                    return;
                }

                string insertHeaderQuery = @"
                    INSERT INTO stock_entry_headers (entry_code, entry_date, supplier_id, warehouse_id, user_id, note, status)
                    VALUES (@entryCode, @entryDate, @supplierId, @warehouseId, @userId, @note, @status);
                    SELECT LAST_INSERT_ID();";

                Dictionary<string, object> headerParams = new Dictionary<string, object>
                {
                    { "@entryCode", txtImportCode.Text },
                    { "@entryDate", dtpImportDate.Value },
                    { "@supplierId", supplierId },
                    { "@warehouseId", warehouseId },
                    { "@userId", userId },
                    { "@note", txtNote.Text },
                    { "@status", txtStatus.Text }
                };

                object stockEntryId = DatabaseHelper.ExecuteScalar(insertHeaderQuery, headerParams);

                if (stockEntryId == null)
                {
                    MessageBox.Show("Lỗi khi lưu phiếu nhập kho!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int stockEntryIdInt = Convert.ToInt32(stockEntryId);

                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    string insertDetailQuery = @"
                        INSERT INTO stock_entry_details (stock_entry_id, product_id, quantity, unit_price)
                        VALUES (@stockEntryId, @productId, @quantity, @unitPrice)";

                    decimal unitPrice = Convert.ToDecimal(row.Cells["colPrice"].Value.ToString().Replace(",", ""));
                    int quantity = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    string maSanPham = row.Cells["colProductID"].Value.ToString();
                    int productId = int.Parse(maSanPham.Replace("SP", ""));

                    Dictionary<string, object> detailParams = new Dictionary<string, object>
                    {
                        { "@stockEntryId", stockEntryIdInt },
                        { "@productId", productId },
                        { "@quantity", quantity },
                        { "@unitPrice", unitPrice }
                    };

                    DatabaseHelper.ExecuteNonQuery(insertDetailQuery, detailParams);

                    string updateStockQuery = @"
                        UPDATE products 
                        SET stock_quantity = stock_quantity + @quantity 
                        WHERE id = @productId";

                    Dictionary<string, object> updateStockParams = new Dictionary<string, object>
                    {
                        { "@quantity", quantity },
                        { "@productId", productId }
                    };

                    DatabaseHelper.ExecuteNonQuery(updateStockQuery, updateStockParams);
                }

                MessageBox.Show("Lưu phiếu nhập kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetOrCreateSupplier(string supplierName)
        {
            try
            {
                Product selectedSupplier = cbSupplier.SelectedItem as Product;
                if (selectedSupplier != null && selectedSupplier.TenSanPham == supplierName)
                {
                    return selectedSupplier.Id;
                }

                string checkQuery = "SELECT id FROM suppliers WHERE name = @name";
                Dictionary<string, object> checkParams = new Dictionary<string, object> { { "@name", supplierName } };
                object result = DatabaseHelper.ExecuteScalar(checkQuery, checkParams);

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }

                string insertQuery = "INSERT INTO suppliers (name) VALUES (@name); SELECT LAST_INSERT_ID();";
                Dictionary<string, object> insertParams = new Dictionary<string, object> { { "@name", supplierName } };
                int newId = Convert.ToInt32(DatabaseHelper.ExecuteScalar(insertQuery, insertParams));

                cbSupplier.Items.Add(new Product { Id = newId, TenSanPham = supplierName });
                return newId;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xử lý nhà cung cấp: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }

        private int GetOrCreateUser(string fullName)
        {
            try
            {
                Product selectedUser = cbImporter.SelectedItem as Product;
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

                cbImporter.Items.Add(new Product { Id = newId, TenSanPham = fullName });
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

            InitializeImportInfoGroup();
            mainContainer.Controls.Add(gbImportInfo, 0, 1);

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
                Text = "Nhập Kho",
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
                Location = new Point(headerPanel.Width - 230, 23)
            };
            btnReset.FlatAppearance.BorderSize = 0;

            btnHistory = new Button
            {
                Text = "Lịch sử nhập kho",
                BackColor = Color.Orange,
                ForeColor = Color.White,
                Size = new Size(150, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 400, 15),
            };
            btnHistory.FlatAppearance.BorderSize = 0;

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnHistory);
            headerPanel.Controls.Add(btnAdd);
            headerPanel.Controls.Add(btnReset);

            headerPanel.Resize += (sender, e) =>
            {
                btnAdd.Location = new Point(headerPanel.Width - 110, 9);
                btnReset.Location = new Point(headerPanel.Width - 230, 9);
                btnHistory.Location = new Point(headerPanel.Width - 400, 9);
            };
        }

        private void InitializeImportInfoGroup()
        {
            gbImportInfo = new GroupBox
            {
                Text = "Thông tin phiếu nhập kho",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(15),
                Margin = new Padding(0, 15, 0, 15),
                BackColor = Color.WhiteSmoke
            };

            TableLayoutPanel tblImportInfo = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 3,
                Padding = new Padding(5),
                BackColor = Color.White
            };

            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tblImportInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            tblImportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tblImportInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tblImportInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblImportCode = new Label { Text = "Mã phiếu:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            txtImportCode = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblImportDate = new Label { Text = "Ngày nhập:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            dtpImportDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Now, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblSupplier = new Label { Text = "Nhà c.cấp:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbSupplier = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            tblImportInfo.Controls.Add(lblImportCode, 0, 0);
            tblImportInfo.Controls.Add(txtImportCode, 1, 0);
            tblImportInfo.Controls.Add(lblImportDate, 2, 0);
            tblImportInfo.Controls.Add(dtpImportDate, 3, 0);
            tblImportInfo.Controls.Add(lblSupplier, 4, 0);
            tblImportInfo.Controls.Add(cbSupplier, 5, 0);

            lblImporter = new Label { Text = "N.nhập:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbImporter = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblWarehouse = new Label { Text = "Kho:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbWarehouse = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblStatus = new Label { Text = "Trạng thái:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            txtStatus = new TextBox { Dock = DockStyle.Fill, Text = "Chờ duyệt", Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            tblImportInfo.Controls.Add(lblImporter, 0, 1);
            tblImportInfo.Controls.Add(cbImporter, 1, 1);
            tblImportInfo.Controls.Add(lblWarehouse, 2, 1);
            tblImportInfo.Controls.Add(cbWarehouse, 3, 1);
            tblImportInfo.Controls.Add(lblStatus, 4, 1);
            tblImportInfo.Controls.Add(txtStatus, 5, 1);

            lblNote = new Label { Text = "Ghi chú:", Anchor = AnchorStyles.Left, TextAlign = ContentAlignment.TopLeft, Margin = new Padding(0, 8, 0, 0), Font = new Font("Segoe UI", 10F) };
            txtNote = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                Text = "Nhập hàng đợt 1 tháng 4/2025",
                ScrollBars = ScrollBars.Vertical,
                Margin = new Padding(0, 5, 10, 5),
                Font = new Font("Segoe UI", 10F)
            };

            tblImportInfo.Controls.Add(lblNote, 0, 2);
            tblImportInfo.SetColumnSpan(txtNote, 5);
            tblImportInfo.Controls.Add(txtNote, 1, 2);

            gbImportInfo.Controls.Add(tblImportInfo);
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

            dgvProducts.Columns.Add("colPrice", "Giá nhập (VND)");
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

        private void ImportWareHouseForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1212, 830);
        }
    }
}