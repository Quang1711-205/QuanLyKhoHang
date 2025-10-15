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
    public partial class ImportHistoryForm : BaseForm
    {
        private TableLayoutPanel mainContainer;
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnRefresh;
        private Button btnBack;
        private Button btnDelete;
        private GroupBox gbFilter;
        private Label lblFromDate;
        private DateTimePicker dtpFromDate;
        private Label lblToDate;
        private DateTimePicker dtpToDate;
        private Label lblImporter;
        private ComboBox cbImporter;
        private Label lblSupplier;
        private ComboBox cbSupplier;
        private Label lblWarehouse;
        private ComboBox cbWarehouse;
        private Label lblEntryCode;
        private TextBox txtEntryCode;
        private Button btnSearch;
        private DataGridView dgvHeaders;
        private GroupBox gbDetails;
        private DataGridView dgvDetails;
        private Label lblDetailTotalQuantity;
        private Label lblDetailTotalQuantityValue;
        private Label lblDetailTotalAmount;
        private Label lblDetailTotalAmountValue;

        private DataTable headerData; // Lưu dữ liệu phiếu nhập kho
        private DataTable detailData; // Lưu dữ liệu chi tiết phiếu nhập kho
        private bool isInitializing; // Biến để kiểm soát quá trình khởi tạo
        private ImportWareHouseForm parentForm; // Tham chiếu đến ImportWareHouseForm

        public ImportHistoryForm(int roleId, ImportWareHouseForm parent) : base(roleId)
        {
            isInitializing = true; // Bắt đầu quá trình khởi tạo
            parentForm = parent; // Lưu tham chiếu đến form cha
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (!IsValidRole())
            {
                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Lịch sử nhập kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            InitializeCustomComponents();
            SetupEventHandlers();
            LoadComboBoxData();
            LoadImportHistory();
            isInitializing = false; // Kết thúc quá trình khởi tạo
            ApplyFilters(); // Gọi ApplyFilters sau khi tất cả đã khởi tạo xong
        }

        private bool IsValidRole()
        {
            return RoleId == 1 || RoleId == 2; // Cho phép quản lý (1) và nhân viên kho (2)
        }

        private void SetupEventHandlers()
        {
            btnRefresh.Click += BtnRefresh_Click;
            btnBack.Click += BtnBack_Click;
            btnSearch.Click += BtnSearch_Click;
            btnDelete.Click += BtnDelete_Click;
            dtpFromDate.ValueChanged += Filter_ValueChanged;
            dtpToDate.ValueChanged += Filter_ValueChanged;
            cbImporter.SelectedIndexChanged += Filter_ValueChanged;
            cbSupplier.SelectedIndexChanged += Filter_ValueChanged;
            cbWarehouse.SelectedIndexChanged += Filter_ValueChanged;
            dgvHeaders.SelectionChanged += DgvHeaders_SelectionChanged;
            dgvHeaders.CellEndEdit += DgvHeaders_CellEndEdit; // Thêm sự kiện chỉnh sửa ô cho dgvHeaders
            dgvDetails.CellEndEdit += DgvDetails_CellEndEdit; // Thêm sự kiện chỉnh sửa ô cho dgvDetails
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            isInitializing = true; // Tạm vô hiệu hóa sự kiện khi làm mới
            dtpFromDate.Value = DateTime.Now.AddMonths(-1);
            dtpToDate.Value = DateTime.Now;
            cbImporter.SelectedIndex = 0; // Chọn "Tất cả"
            cbSupplier.SelectedIndex = 0;
            cbWarehouse.SelectedIndex = 0;
            txtEntryCode.Text = string.Empty;
            LoadImportHistory();
            isInitializing = false;
            ApplyFilters();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (parentForm != null)
            {
                this.Hide(); // Ẩn form ImportHistoryForm
                parentForm.Show(); // Hiện lại form ImportWareHouseForm
            }
            else
            {
                MessageBox.Show("Không có form cha để quay lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (RoleId != 1)
            {
                MessageBox.Show("Bạn không có quyền xóa phiếu nhập kho!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvHeaders.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa phiếu nhập kho này? Thao tác này sẽ xóa cả phiếu và các sản phẩm liên quan.", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;

                int stockEntryId = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["colHeaderId"].Value);

                try
                {
                    // Xóa chi tiết phiếu trước
                    string deleteDetailsQuery = "DELETE FROM stock_entry_details WHERE stock_entry_id = @stockEntryId";
                    Dictionary<string, object> deleteDetailsParams = new Dictionary<string, object> { { "@stockEntryId", stockEntryId } };
                    DatabaseHelper.ExecuteNonQuery(deleteDetailsQuery, deleteDetailsParams);

                    // Xóa phiếu nhập kho
                    string deleteHeaderQuery = "DELETE FROM stock_entry_headers WHERE id = @stockEntryId";
                    Dictionary<string, object> deleteHeaderParams = new Dictionary<string, object> { { "@stockEntryId", stockEntryId } };
                    DatabaseHelper.ExecuteNonQuery(deleteHeaderQuery, deleteHeaderParams);

                    MessageBox.Show("Xóa phiếu nhập kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadImportHistory();
                    ApplyFilters();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa phiếu nhập kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvHeaders_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (RoleId != 1) // Chỉ admin mới được chỉnh sửa
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadImportHistory();
                ApplyFilters();
                return;
            }

            int rowIndex = e.RowIndex;
            int stockEntryId = Convert.ToInt32(dgvHeaders.Rows[rowIndex].Cells["colHeaderId"].Value);

            // Lấy giá trị mới từ ô đã chỉnh sửa
            string columnName = dgvHeaders.Columns[e.ColumnIndex].Name;
            string newValue = dgvHeaders.Rows[rowIndex].Cells[e.ColumnIndex].Value?.ToString();

            try
            {
                string updateQuery = string.Empty;
                Dictionary<string, object> updateParams = new Dictionary<string, object> { { "@stockEntryId", stockEntryId } };

                switch (columnName)
                {
                    case "colSupplierName":
                        // Cập nhật supplier_id trong stock_entry_headers
                        int supplierId = GetSupplierIdByName(newValue);
                        if (supplierId == -1)
                        {
                            MessageBox.Show("Nhà cung cấp không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LoadImportHistory();
                            ApplyFilters();
                            return;
                        }
                        updateQuery = "UPDATE stock_entry_headers SET supplier_id = @supplierId WHERE id = @stockEntryId";
                        updateParams.Add("@supplierId", supplierId);
                        break;

                    case "colWarehouseName":
                        // Cập nhật warehouse_id trong stock_entry_headers
                        int warehouseId = GetWarehouseIdByName(newValue);
                        if (warehouseId == -1)
                        {
                            MessageBox.Show("Kho không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LoadImportHistory();
                            ApplyFilters();
                            return;
                        }
                        updateQuery = "UPDATE stock_entry_headers SET warehouse_id = @warehouseId WHERE id = @stockEntryId";
                        updateParams.Add("@warehouseId", warehouseId);
                        break;

                    case "colUserName":
                        // Cập nhật user_id trong stock_entry_headers
                        int userId = GetUserIdByName(newValue);
                        if (userId == -1)
                        {
                            MessageBox.Show("Người dùng không tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LoadImportHistory();
                            ApplyFilters();
                            return;
                        }
                        updateQuery = "UPDATE stock_entry_headers SET user_id = @userId WHERE id = @stockEntryId";
                        updateParams.Add("@userId", userId);
                        break;

                    case "colStatus":
                        updateQuery = "UPDATE stock_entry_headers SET status = @status WHERE id = @stockEntryId";
                        updateParams.Add("@status", newValue);
                        break;

                    case "colNote":
                        updateQuery = "UPDATE stock_entry_headers SET note = @note WHERE id = @stockEntryId";
                        updateParams.Add("@note", newValue);
                        break;
                }

                if (!string.IsNullOrEmpty(updateQuery))
                {
                    DatabaseHelper.ExecuteNonQuery(updateQuery, updateParams);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadImportHistory();
                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadImportHistory();
                ApplyFilters();
            }
        }

        private void DgvDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (RoleId != 1) // Chỉ admin mới được chỉnh sửa
            {
                MessageBox.Show("Bạn không có quyền chỉnh sửa dữ liệu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadImportDetails(Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["colHeaderId"].Value));
                return;
            }

            int rowIndex = e.RowIndex;
            int stockEntryId = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["colHeaderId"].Value);
            string productId = dgvDetails.Rows[rowIndex].Cells["colDetailProductId"].Value.ToString();
            int parsedProductId;

            // Loại bỏ tiền tố "SP" và các số 0 phía trước để lấy ID gốc
            if (productId.StartsWith("SP"))
            {
                string numericPart = productId.Substring(2).TrimStart('0');
                parsedProductId = int.Parse(numericPart);
            }
            else
            {
                parsedProductId = int.Parse(productId);
            }

            // Lấy giá trị mới từ ô đã chỉnh sửa
            string columnName = dgvDetails.Columns[e.ColumnIndex].Name;
            string newValue = dgvDetails.Rows[rowIndex].Cells[e.ColumnIndex].Value?.ToString();

            try
            {
                // Cập nhật cơ sở dữ liệu dựa trên cột đã chỉnh sửa
                if (columnName == "colDetailProductName")
                {
                    // Cập nhật tên sản phẩm trong bảng products
                    string updateProductQuery = "UPDATE products SET name = @name WHERE id = @productId";
                    Dictionary<string, object> updateProductParams = new Dictionary<string, object>
                    {
                        { "@name", newValue },
                        { "@productId", parsedProductId }
                    };
                    DatabaseHelper.ExecuteNonQuery(updateProductQuery, updateProductParams);
                }
                else if (columnName == "colDetailUnitPrice" || columnName == "colDetailQuantity")
                {
                    // Lấy giá trị mới của giá nhập và số lượng
                    decimal unitPrice = Convert.ToDecimal(dgvDetails.Rows[rowIndex].Cells["colDetailUnitPrice"].Value.ToString().Replace("N0", ""));
                    int quantity = Convert.ToInt32(dgvDetails.Rows[rowIndex].Cells["colDetailQuantity"].Value);

                    // Tính lại thành tiền
                    decimal total = unitPrice * quantity;
                    dgvDetails.Rows[rowIndex].Cells["colDetailTotal"].Value = total.ToString("N0") + " VND";

                    // Cập nhật vào bảng stock_entry_details
                    string updateDetailQuery = "UPDATE stock_entry_details SET unit_price = @unitPrice, quantity = @quantity WHERE stock_entry_id = @stockEntryId AND product_id = @productId";
                    Dictionary<string, object> updateDetailParams = new Dictionary<string, object>
                    {
                        { "@unitPrice", unitPrice },
                        { "@quantity", quantity },
                        { "@stockEntryId", stockEntryId },
                        { "@productId", parsedProductId }
                    };
                    DatabaseHelper.ExecuteNonQuery(updateDetailQuery, updateDetailParams);

                    // Cập nhật tổng số lượng và tổng giá trị
                    UpdateDetailTotals();
                }

                MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadImportDetails(stockEntryId); // Tải lại dữ liệu nếu có lỗi
            }
        }

        private int GetSupplierIdByName(string name)
        {
            string query = "SELECT id FROM suppliers WHERE name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", name } };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["id"]);
            return -1;
        }

        private int GetWarehouseIdByName(string name)
        {
            string query = "SELECT id FROM warehouses WHERE name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", name } };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["id"]);
            return -1;
        }

        private int GetUserIdByName(string name)
        {
            string query = "SELECT id FROM users WHERE full_name = @name";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@name", name } };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["id"]);
            return -1;
        }

        private void Filter_ValueChanged(object sender, EventArgs e)
        {
            if (!isInitializing) // Chỉ gọi ApplyFilters khi không trong quá trình khởi tạo
            {
                ApplyFilters();
            }
        }

        private void DgvHeaders_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvHeaders.SelectedRows.Count > 0)
            {
                int stockEntryId = Convert.ToInt32(dgvHeaders.SelectedRows[0].Cells["colHeaderId"].Value);
                LoadImportDetails(stockEntryId);
            }
            else
            {
                dgvDetails.Rows.Clear();
                UpdateDetailTotals();
            }
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
                cbSupplier.Items.Clear();
                cbSupplier.Items.Add(new Product { Id = 0, TenSanPham = "Tất cả" });

                string query = "SELECT id, name FROM suppliers";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    cbSupplier.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString()
                    });
                }
                cbSupplier.DisplayMember = "TenSanPham";
                cbSupplier.ValueMember = "Id";
                cbSupplier.SelectedIndex = 0;
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
                cbImporter.Items.Clear();
                cbImporter.Items.Add(new Product { Id = 0, TenSanPham = "Tất cả" });

                string query = "SELECT id, username, full_name FROM users";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    cbImporter.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["full_name"].ToString()
                    });
                }
                cbImporter.DisplayMember = "TenSanPham";
                cbImporter.ValueMember = "Id";
                cbImporter.SelectedIndex = 0;
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
                cbWarehouse.Items.Clear();
                cbWarehouse.Items.Add(new Product { Id = 0, TenSanPham = "Tất cả" });

                string query = "SELECT id, name FROM warehouses";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                foreach (DataRow row in dt.Rows)
                {
                    cbWarehouse.Items.Add(new Product
                    {
                        Id = Convert.ToInt32(row["id"]),
                        TenSanPham = row["name"].ToString()
                    });
                }
                cbWarehouse.DisplayMember = "TenSanPham";
                cbWarehouse.ValueMember = "Id";
                cbWarehouse.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadImportHistory()
        {
            try
            {
                string query = @"
                    SELECT seh.id, seh.entry_code, seh.entry_date, seh.status, seh.note,
                           s.id AS supplier_id, s.name AS supplier_name,
                           w.id AS warehouse_id, w.name AS warehouse_name,
                           u.id AS user_id, u.full_name AS user_name
                    FROM stock_entry_headers seh
                    JOIN suppliers s ON seh.supplier_id = s.id
                    JOIN warehouses w ON seh.warehouse_id = w.id
                    JOIN users u ON seh.user_id = u.id";
                headerData = DatabaseHelper.ExecuteQuery(query);

                if (headerData == null || headerData.Rows.Count == 0)
                {
                    MessageBox.Show("Không có phiếu nhập kho nào trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải lịch sử nhập kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilters()
        {
            dgvHeaders.Rows.Clear();
            if (headerData == null || headerData.Rows.Count == 0)
            {
                MessageBox.Show("Không có phiếu nhập kho nào trong cơ sở dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date;

            if (fromDate > toDate)
            {
                MessageBox.Show("Ngày bắt đầu không được lớn hơn ngày kết thúc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Product selectedImporter = cbImporter.SelectedItem as Product ?? new Product { Id = 0 };
            Product selectedSupplier = cbSupplier.SelectedItem as Product ?? new Product { Id = 0 };
            Product selectedWarehouse = cbWarehouse.SelectedItem as Product ?? new Product { Id = 0 };
            string searchEntryCode = txtEntryCode.Text.Trim().ToLower();

            foreach (DataRow row in headerData.Rows)
            {
                DateTime entryDate = Convert.ToDateTime(row["entry_date"]).Date;
                string entryCode = row["entry_code"].ToString().ToLower();
                int supplierId = Convert.ToInt32(row["supplier_id"]);
                int userId = Convert.ToInt32(row["user_id"]);
                int warehouseId = Convert.ToInt32(row["warehouse_id"]);

                bool matchesDate = entryDate >= fromDate && entryDate <= toDate;
                bool matchesImporter = selectedImporter.Id == 0 || userId == selectedImporter.Id;
                bool matchesSupplier = selectedSupplier.Id == 0 || supplierId == selectedSupplier.Id;
                bool matchesWarehouse = selectedWarehouse.Id == 0 || warehouseId == selectedWarehouse.Id;
                bool matchesEntryCode = string.IsNullOrEmpty(searchEntryCode) || entryCode.Contains(searchEntryCode);

                if (matchesDate && matchesImporter && matchesSupplier && matchesWarehouse && matchesEntryCode)
                {
                    dgvHeaders.Rows.Add(
                        row["id"],
                        row["entry_code"],
                        Convert.ToDateTime(row["entry_date"]).ToString("dd/MM/yyyy"),
                        row["supplier_name"],
                        row["warehouse_name"],
                        row["user_name"],
                        row["status"],
                        row["note"]
                    );
                }
            }

            if (dgvHeaders.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy phiếu nhập kho phù hợp với tiêu chí lọc!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string FormatProductCode(int id)
        {
            if (id < 10) // 1 chữ số
                return "SP00" + id;
            else if (id < 100) // 2 chữ số
                return "SP0" + id;
            else // 3 chữ số trở lên
                return "SP" + id;
        }

        private void LoadImportDetails(int stockEntryId)
        {
            try
            {
                string query = @"
                    SELECT sed.product_id, sed.quantity, sed.unit_price, p.name AS product_name
                    FROM stock_entry_details sed
                    JOIN products p ON sed.product_id = p.id
                    WHERE sed.stock_entry_id = @stockEntryId";
                Dictionary<string, object> paramsDict = new Dictionary<string, object> { { "@stockEntryId", stockEntryId } };
                detailData = DatabaseHelper.ExecuteQuery(query, paramsDict);

                dgvDetails.Rows.Clear();
                foreach (DataRow row in detailData.Rows)
                {
                    int productId = Convert.ToInt32(row["product_id"]);
                    string formattedProductCode = FormatProductCode(productId); // Định dạng mã sản phẩm
                    decimal unitPrice = Convert.ToDecimal(row["unit_price"]);
                    int quantity = Convert.ToInt32(row["quantity"]);
                    decimal total = unitPrice * quantity;

                    dgvDetails.Rows.Add(
                        formattedProductCode, // Định dạng mã sản phẩm
                        row["product_name"],
                        unitPrice.ToString("N0"),
                        quantity,
                        total.ToString("N0") + " VND"
                    );
                }
                UpdateDetailTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải chi tiết phiếu nhập kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDetailTotals()
        {
            int totalQuantity = 0;
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dgvDetails.Rows)
            {
                totalQuantity += Convert.ToInt32(row.Cells["colDetailQuantity"].Value);
                decimal total = Convert.ToDecimal(row.Cells["colDetailTotal"].Value.ToString().Replace(" VND", "").Replace(",", ""));
                totalAmount += total;
            }

            lblDetailTotalQuantityValue.Text = totalQuantity.ToString();
            lblDetailTotalAmountValue.Text = totalAmount.ToString("N0") + " VND";
        }

        private void InitializeCustomComponents()
        {
            mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.WhiteSmoke,
                Padding = new Padding(260, 50, 10, 10)
            };

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            InitializeHeaderPanel();
            mainContainer.Controls.Add(headerPanel, 0, 0);

            InitializeFilterGroup();
            mainContainer.Controls.Add(gbFilter, 0, 1);

            InitializeHeadersGrid();
            mainContainer.Controls.Add(dgvHeaders, 0, 2);

            InitializeDetailsGroup();
            mainContainer.Controls.Add(gbDetails, 0, 3);

            this.Controls.Add(mainContainer);
        }

        private void InitializeHeaderPanel()
        {
            headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 15, 0, 0)
            };

            lblTitle = new Label
            {
                Text = "Lịch Sử Nhập Kho",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Location = new Point(10, 8)
            };

            btnDelete = new Button
            {
                Text = "Xóa",
                BackColor = Color.Red,
                ForeColor = Color.White,
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 220, 15),
                Visible = RoleId == 1 // Chỉ hiển thị với admin
            };
            btnDelete.FlatAppearance.BorderSize = 0;

            btnRefresh = new Button
            {
                Text = "Làm mới",
                BackColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 110, 15)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;

            btnBack = new Button
            {
                Text = "Quay lại",
                BackColor = Color.Gray,
                ForeColor = Color.White,
                Size = new Size(100, 35),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(headerPanel.Width - 330, 15) // Đặt bên cạnh nút "Xóa"
            };
            btnBack.FlatAppearance.BorderSize = 0;

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(btnDelete);
            headerPanel.Controls.Add(btnRefresh);
            headerPanel.Controls.Add(btnBack);

            headerPanel.Resize += (sender, e) =>
            {
                //btnBack.Location = new Point(headerPanel.Width - 330, 15);
                //btnDelete.Location = new Point(headerPanel.Width - 220, 15);
                //btnRefresh.Location = new Point(headerPanel.Width - 110, 15);
                btnDelete.Location = new Point(headerPanel.Width - 330, 15);
                btnRefresh.Location = new Point(headerPanel.Width - 220, 15);
                btnBack.Location = new Point(headerPanel.Width - 110, 15);
            };
        }

        private void InitializeFilterGroup()
        {
            gbFilter = new GroupBox
            {
                Text = "Bộ lọc",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(10),
                Margin = new Padding(0, 10, 0, 10),
                BackColor = Color.WhiteSmoke
            };

            TableLayoutPanel tblFilter = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 8,
                RowCount = 2,
                Padding = new Padding(5),
                BackColor = Color.White
            };

            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
            tblFilter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            tblFilter.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tblFilter.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));

            lblFromDate = new Label { Text = "Từ ngày:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            dtpFromDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Now.AddMonths(-1), Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblToDate = new Label { Text = "Đến ngày:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            dtpToDate = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Value = DateTime.Now, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblImporter = new Label { Text = "Ng.nhập:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbImporter = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblSupplier = new Label { Text = "Nhà c.cấp:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbSupplier = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            tblFilter.Controls.Add(lblFromDate, 0, 0);
            tblFilter.Controls.Add(dtpFromDate, 1, 0);
            tblFilter.Controls.Add(lblToDate, 2, 0);
            tblFilter.Controls.Add(dtpToDate, 3, 0);
            tblFilter.Controls.Add(lblImporter, 4, 0);
            tblFilter.Controls.Add(cbImporter, 5, 0);
            tblFilter.Controls.Add(lblSupplier, 6, 0);
            tblFilter.Controls.Add(cbSupplier, 7, 0);

            lblWarehouse = new Label { Text = "Kho:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            cbWarehouse = new ComboBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            lblEntryCode = new Label { Text = "Mã phiếu:", Anchor = AnchorStyles.Left | AnchorStyles.Right, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 10F) };
            txtEntryCode = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(0, 5, 10, 5), Font = new Font("Segoe UI", 10F) };

            btnSearch = new Button
            {
                Text = "Tìm kiếm",
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F)
            };
            btnSearch.FlatAppearance.BorderSize = 0;

            tblFilter.Controls.Add(lblWarehouse, 0, 1);
            tblFilter.Controls.Add(cbWarehouse, 1, 1);
            tblFilter.Controls.Add(lblEntryCode, 2, 1);
            tblFilter.Controls.Add(txtEntryCode, 3, 1);
            tblFilter.Controls.Add(btnSearch, 4, 1);
            tblFilter.SetColumnSpan(btnSearch, 1);

            gbFilter.Controls.Add(tblFilter);
        }

        private void InitializeHeadersGrid()
        {
            dgvHeaders = new DataGridView
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
                AllowUserToOrderColumns = false,
                ReadOnly = RoleId != 1 // Chỉ admin mới được chỉnh sửa
            };

            dgvHeaders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvHeaders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHeaders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvHeaders.ColumnHeadersHeight = 35;
            dgvHeaders.EnableHeadersVisualStyles = false;

            dgvHeaders.Columns.Add("colHeaderId", "ID");
            dgvHeaders.Columns["colHeaderId"].Visible = false;

            dgvHeaders.Columns.Add("colEntryCode", "Mã phiếu");
            dgvHeaders.Columns["colEntryCode"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHeaders.Columns["colEntryCode"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHeaders.Columns["colEntryCode"].ReadOnly = true; // Không cho chỉnh sửa

            dgvHeaders.Columns.Add("colEntryDate", "Ngày nhập");
            dgvHeaders.Columns["colEntryDate"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHeaders.Columns["colEntryDate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHeaders.Columns["colEntryDate"].ReadOnly = true; // Không cho chỉnh sửa

            dgvHeaders.Columns.Add("colSupplierName", "Nhà cung cấp");
            dgvHeaders.Columns["colSupplierName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHeaders.Columns.Add("colWarehouseName", "Kho");
            dgvHeaders.Columns["colWarehouseName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHeaders.Columns.Add("colUserName", "Người nhập");
            dgvHeaders.Columns["colUserName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHeaders.Columns.Add("colStatus", "Trạng thái");
            dgvHeaders.Columns["colStatus"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHeaders.Columns["colStatus"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvHeaders.Columns.Add("colNote", "Ghi chú");
            dgvHeaders.Columns["colNote"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void InitializeDetailsGroup()
        {
            gbDetails = new GroupBox
            {
                Text = "Chi tiết phiếu nhập kho",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(15),
                Margin = new Padding(0, 15, 0, 15),
                BackColor = Color.WhiteSmoke
            };

            TableLayoutPanel tblDetails = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.White,
                Margin = new Padding(0, 5, 0, 0)
            };

            tblDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tblDetails.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tblDetails.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));

            dgvDetails = new DataGridView
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
                AllowUserToOrderColumns = false,
                ReadOnly = RoleId != 1, // Chỉ admin mới được chỉnh sửa
                MinimumSize = new Size(0, 150),
                ScrollBars = ScrollBars.Both
            };

            dgvDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDetails.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDetails.ColumnHeadersHeight = 35;
            dgvDetails.EnableHeadersVisualStyles = false;

            dgvDetails.Columns.Add("colDetailProductId", "Mã SP");
            dgvDetails.Columns["colDetailProductId"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns["colDetailProductId"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns["colDetailProductId"].ReadOnly = true; // Không cho chỉnh sửa

            dgvDetails.Columns.Add("colDetailProductName", "Tên Sản phẩm");
            dgvDetails.Columns["colDetailProductName"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDetails.Columns.Add("colDetailUnitPrice", "Giá nhập (VND)");
            dgvDetails.Columns["colDetailUnitPrice"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns["colDetailUnitPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvDetails.Columns.Add("colDetailQuantity", "Số lượng");
            dgvDetails.Columns["colDetailQuantity"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns["colDetailQuantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDetails.Columns.Add("colDetailTotal", "Thành tiền");
            dgvDetails.Columns["colDetailTotal"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDetails.Columns["colDetailTotal"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetails.Columns["colDetailTotal"].ReadOnly = true; // Không cho chỉnh sửa trực tiếp

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

            lblDetailTotalQuantity = new Label
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

            lblDetailTotalQuantityValue = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Dock = DockStyle.Fill,
                AutoSize = true,
                Margin = new Padding(0),
                Padding = new Padding(0, 10, 0, 0)
            };

            lblDetailTotalAmount = new Label
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

            lblDetailTotalAmountValue = new Label
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

            totalsPanel.Controls.Add(lblDetailTotalQuantity, 0, 0);
            totalsPanel.Controls.Add(lblDetailTotalQuantityValue, 1, 0);
            totalsPanel.Controls.Add(lblDetailTotalAmount, 0, 1);
            totalsPanel.Controls.Add(lblDetailTotalAmountValue, 1, 1);

            tblDetails.Controls.Add(dgvDetails, 0, 0);
            tblDetails.Controls.Add(totalsPanel, 0, 1);

            gbDetails.Controls.Add(tblDetails);
        }

        private void ImportHistoryForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1212, 830);
        }
    }
}