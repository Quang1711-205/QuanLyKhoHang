//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WareHouse.Utils;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class BaseForm : Form
//    {
//        public BaseForm()
//        {
//            InitializeComponent();
//            // Code để làm tròn góc cho button1
//            int radius = 15;

//            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
//            path.AddArc(0, 0, radius, radius, 180, 90);
//            path.AddArc(button1.Width - radius, 0, radius, radius, 270, 90);
//            path.AddArc(button1.Width - radius, button1.Height - radius, radius, radius, 0, 90);
//            path.AddArc(0, button1.Height - radius, radius, radius, 90, 90);
//            path.CloseFigure();

//            button1.Region = new Region(path);
//            button1.FlatStyle = FlatStyle.Flat;
//            button1.FlatAppearance.BorderSize = 0;

//            // Danh sách các button trong sidebar cần bo tròn
//            List<Button> sidebarButtons = new List<Button>
//             {
//                btnDashBoard, btnProduct, btnEnterWarehouse,
//                btnWarehouseExport, btnReport, btnAccManage
//             };

//            // Bo tròn cho các button trong sidebar
//            foreach (Button btn in sidebarButtons)
//            {
//                UIHelper.ApplyRoundedCorners(btn, 5);
//            }

//            // Bo tròn cho panel Avatar
//            UIHelper.ApplyRoundedCorners(panelAvatar, 5);
//        }
//    }
//}


// Code chính 1

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WareHouse.Utils;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class BaseForm : Form
//    {
//        private Button selectedButton; // Lưu button hiện tại được chọn
//        private Color defaultButtonColor = Color.FromArgb(44, 62, 80); // Màu mặc định của button
//        private Color selectedButtonColor = Color.DodgerBlue; // Màu khi button được chọn

//        public BaseForm()
//        {
//            InitializeComponent();

//            // Code để làm tròn góc cho button1
//            int radius = 15;

//            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
//            path.AddArc(0, 0, radius, radius, 180, 90);
//            path.AddArc(button1.Width - radius, 0, radius, radius, 270, 90);
//            path.AddArc(button1.Width - radius, button1.Height - radius, radius, radius, 0, 90);
//            path.AddArc(0, button1.Height - radius, radius, radius, 90, 90);
//            path.CloseFigure();

//            button1.Region = new Region(path);
//            button1.FlatStyle = FlatStyle.Flat;
//            button1.FlatAppearance.BorderSize = 0;

//            // Danh sách các button trong sidebar cần bo tròn
//            List<Button> sidebarButtons = new List<Button>
//            {
//                btnDashBoard, btnProduct, btnEnterWarehouse,
//                btnWarehouseExport, btnReport, btnAccManage
//            };

//            // Bo tròn cho các button trong sidebar và thiết lập màu mặc định
//            foreach (Button btn in sidebarButtons)
//            {
//                UIHelper.ApplyRoundedCorners(btn, 5);
//                btn.BackColor = defaultButtonColor; // Thiết lập màu mặc định
//            }

//            // Bo tròn cho panel Avatar
//            UIHelper.ApplyRoundedCorners(panelAvatar, 5);

//            // Gán sự kiện Click cho các button trong sidebar
//            btnDashBoard.Click += (s, e) =>
//            {
//                SetSelectedButton(btnDashBoard);
//                NavigateToForm(new DashBoardForm()); // Thay DashBoardForm bằng form tương ứng
//            };

//            btnProduct.Click += (s, e) =>
//            {
//                SetSelectedButton(btnProduct);
//                NavigateToForm(new ProductManagementForm());
//            };

//            btnEnterWarehouse.Click += (s, e) =>
//            {
//                SetSelectedButton(btnEnterWarehouse);
//                NavigateToForm(new ImportWareHouseForm());
//            };

//            btnWarehouseExport.Click += (s, e) =>
//            {
//                SetSelectedButton(btnWarehouseExport);
//                NavigateToForm(new ExportWareHouseForm()); // Thay WarehouseExportForm bằng form tương ứng
//            };

//            btnReport.Click += (s, e) =>
//            {
//                SetSelectedButton(btnReport);
//                NavigateToForm(new ReportForm()); // Thay ReportForm bằng form tương ứng
//            };

//            btnAccManage.Click += (s, e) =>
//            {
//                SetSelectedButton(btnAccManage);
//                NavigateToForm(new UserManagementForm()); // Thay AccountManagementForm bằng form tương ứng
//            };

//            // Thiết lập button mặc định được chọn (dựa trên form hiện tại)
//            if (this is ImportWareHouseForm)
//            {
//                SetSelectedButton(btnEnterWarehouse);
//            }
//            else if (this is ProductManagementForm)
//            {
//                SetSelectedButton(btnProduct);
//            }
//            // Thêm các điều kiện khác cho các form khác nếu cần
//        }

//        private void SetSelectedButton(Button button)
//        {
//            // Khôi phục màu mặc định cho button trước đó (nếu có)
//            if (selectedButton != null)
//            {
//                selectedButton.BackColor = defaultButtonColor;
//            }

//            // Đặt màu cho button mới được chọn
//            selectedButton = button;
//            selectedButton.BackColor = selectedButtonColor;
//        }

//        private void NavigateToForm(Form newForm)
//        {
//            // Ẩn form hiện tại
//            this.Hide();

//            // Mở form mới
//            newForm.Show();

//            // Đóng form hiện tại khi form mới đóng
//            newForm.FormClosed += (s, args) => this.Close();
//        }
//    }
//}


// Code mới

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using MySql.Data.MySqlClient;
//using WareHouse.Presentation.Controls;
//using WareHouse.Utils;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class DashBoardForm : BaseForm
//    {
//        public DashBoardForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình

//            // Load data when the form is shown
//            this.Load += DashBoardForm_Load;
//        }

//        private void DashBoardForm_Load(object sender, EventArgs e)
//        {
//            LoadDashboardData();
//            LoadRecentActivities();
//            LoadLowStockWarnings();
//        }

//        private void LoadDashboardData()
//        {
//            try
//            {
//                // 1. Get monthly revenue from orders
//                //    string revenueSql = @"
//                //SELECT COALESCE(SUM(oi.price * oi.quantity), 0) as TotalRevenue
//                //FROM orders o
//                //JOIN order_items oi ON o.id = oi.order_id
//                //WHERE o.status = 'Completed'
//                //AND MONTH(o.order_date) = MONTH(CURRENT_DATE())
//                //AND YEAR(o.order_date) = YEAR(CURRENT_DATE())";

//                // 
//                string revenueSql = @"
//                    SELECT COALESCE(SUM(oi.price * oi.quantity), 0) as TotalRevenue
//                    FROM orders o
//                    JOIN order_items oi ON o.id = oi.order_id
//                    WHERE o.status = 'Completed'
//                    AND MONTH(o.order_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(o.order_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";


//                using (MySqlCommand revenueCmd = new MySqlCommand(revenueSql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalRevenue = Convert.ToDecimal(revenueCmd.ExecuteScalar());
//                    namDoanhthu.Text = String.Format("{0:n0}", totalRevenue);
//                }

//                // 2. Get monthly stock entries total value
//                //    string entrySql = @"
//                //SELECT COALESCE(SUM(se.quantity * p.price), 0) as TotalEntryValue
//                //FROM stock_entries se
//                //JOIN products p ON se.product_id = p.id
//                //WHERE MONTH(se.entry_date) = MONTH(CURRENT_DATE())
//                //AND YEAR(se.entry_date) = YEAR(CURRENT_DATE())";
//                string entrySql = @"
//                    SELECT COALESCE(SUM(se.quantity * p.price), 0) as TotalEntryValue
//                    FROM stock_entries se
//                    JOIN products p ON se.product_id = p.id
//                    WHERE MONTH(se.entry_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(se.entry_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";

//                using (MySqlCommand entryCmd = new MySqlCommand(entrySql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalEntryValue = Convert.ToDecimal(entryCmd.ExecuteScalar());
//                    quantityE_warehouse.Text = String.Format("{0:n0}", totalEntryValue);
//                }

//                // 3. Get monthly stock exits total value
//                //    string exitSql = @"
//                //SELECT COALESCE(SUM(sx.quantity * p.price), 0) as TotalExitValue
//                //FROM stock_exits sx
//                //JOIN products p ON sx.product_id = p.id
//                //WHERE MONTH(sx.exit_date) = MONTH(CURRENT_DATE())
//                //AND YEAR(sx.exit_date) = YEAR(CURRENT_DATE())";
//                string exitSql = @"
//                    SELECT COALESCE(SUM(sx.quantity * p.price), 0) as TotalExitValue
//                    FROM stock_exits sx
//                    JOIN products p ON sx.product_id = p.id
//                    WHERE MONTH(sx.exit_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(sx.exit_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";

//                using (MySqlCommand exitCmd = new MySqlCommand(exitSql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalExitValue = Convert.ToDecimal(exitCmd.ExecuteScalar());
//                    quantityEX_warehouse.Text = String.Format("{0:n0}", totalExitValue);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải dữ liệu tổng quan: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//        private void LoadRecentActivities()
//        {
//            try
//            {
//                panelHoatDong.Controls.Clear(); // Xóa dữ liệu cũ trước khi tải mới
//                panelHoatDong.AutoScroll = true; // Bật thanh cuộn khi nội dung dài
//                UIHelper.ApplyRoundedCorners(panelHoatDong, 5);

//                string activitySql = @"
//            SELECT al.action, al.timestamp, u.username 
//            FROM activity_logs al
//            LEFT JOIN users u ON al.user_id = u.id
//            ORDER BY al.timestamp DESC
//            LIMIT 10"; // Lấy tối đa 10 hoạt động gần đây

//                using (MySqlCommand activityCmd = new MySqlCommand(activitySql, DatabaseHelper.GetConnection()))
//                {
//                    MySqlDataReader reader = activityCmd.ExecuteReader();

//                    int yPosition = 10; // Vị trí Y để sắp xếp các panel con
//                    int panelWidth = panelHoatDong.Width - 30; // Chừa chỗ cho thanh cuộn nếu có

//                    while (reader.Read())
//                    {
//                        string action = reader["action"].ToString();
//                        DateTime timestamp = Convert.ToDateTime(reader["timestamp"]);
//                        TimeSpan diff = DateTime.Now - timestamp;

//                        string timeText;
//                        if (diff.TotalMinutes < 60)
//                            timeText = $"{Math.Floor(diff.TotalMinutes)} phút trước";
//                        else if (diff.TotalHours < 24)
//                            timeText = $"{Math.Floor(diff.TotalHours)} giờ trước";
//                        else
//                            timeText = $"{Math.Floor(diff.TotalDays)} ngày trước";

//                        // Panel chứa thông tin hoạt động
//                        Panel panel = new Panel
//                        {
//                            Size = new Size(panelWidth, 35), // Giảm chiều rộng để tránh bị tràn khi có thanh cuộn
//                            Location = new Point((panelHoatDong.Width - panelWidth) / 2, yPosition), // Căn giữa
//                            BorderStyle = BorderStyle.None,
//                            BackColor = Color.White,
//                        };
//                        UIHelper.ApplyRoundedCorners(panel, 5);

//                        // Label hiển thị hành động
//                        Label lblAction = new Label
//                        {
//                            Text = action,
//                            AutoSize = false,
//                            Width = panel.Width - 100,
//                            Height = 30,
//                            Font = new Font("Arial", 10, FontStyle.Bold),
//                            TextAlign = ContentAlignment.MiddleLeft,
//                            Location = new Point(10, 2),
//                            AutoEllipsis = true
//                        };

//                        // Label hiển thị thời gian (căn phải)
//                        Label lblTime = new Label
//                        {
//                            Text = timeText,
//                            AutoSize = false,
//                            Width = 90,
//                            Height = 30,
//                            Font = new Font("Arial", 9),
//                            ForeColor = Color.DarkGray,
//                            TextAlign = ContentAlignment.MiddleRight,
//                            Location = new Point(panel.Width - 100, 7)
//                        };

//                        // Thêm các Label vào Panel con
//                        panel.Controls.Add(lblAction);
//                        panel.Controls.Add(lblTime);

//                        // Thêm Panel con vào panelHoatDong
//                        panelHoatDong.Controls.Add(panel);

//                        // Dịch chuyển vị trí xuống dưới cho panel tiếp theo
//                        yPosition += 40;
//                    }

//                    reader.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải hoạt động gần đây: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void LoadLowStockWarnings()
//        {
//            try
//            {
//                // Đầu tiên xóa tất cả sự kiện để tránh đăng ký nhiều lần
//                dgvLowStock.CellClick -= DgvLowStock_CellClick; // Bỏ sự kiện cũ nếu có
//                dgvLowStock.CellPainting -= DgvLowStock_CellPainting; // Bỏ sự kiện cũ nếu có

//                string lowStockSql = @"
//SELECT p.id, p.name, p.stock_quantity
//FROM products p
//WHERE p.stock_quantity <= 10
//ORDER BY p.stock_quantity ASC";

//                using (MySqlCommand lowStockCmd = new MySqlCommand(lowStockSql, DatabaseHelper.GetConnection()))
//                {
//                    // Tạo DataTable mới
//                    DataTable lowStockTable = new DataTable();
//                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(lowStockCmd))
//                    {
//                        adapter.Fill(lowStockTable);
//                    }

//                    // Đặt lại DataGridView hoàn toàn
//                    dgvLowStock.DataSource = null;
//                    dgvLowStock.Columns.Clear();
//                    dgvLowStock.Font = new Font("Segoe UI", 10F);

//                    // Thiết lập DataSource
//                    dgvLowStock.DataSource = lowStockTable;

//                    // Đặt tên cột
//                    dgvLowStock.Columns["id"].HeaderText = "Mã SP";
//                    dgvLowStock.Columns["name"].HeaderText = "Tên sản phẩm";
//                    dgvLowStock.Columns["stock_quantity"].HeaderText = "Còn lại";

//                    // Thêm cột Button một lần duy nhất
//                    DataGridViewButtonColumn btnColumn = new DataGridViewButtonColumn();
//                    btnColumn.HeaderText = "Hành động";
//                    btnColumn.Name = "colAction"; // Đặt tên cụ thể để dễ kiểm tra
//                    btnColumn.Text = "Nhập hàng";
//                    btnColumn.UseColumnTextForButtonValue = true;
//                    dgvLowStock.Columns.Add(btnColumn);

//                    // Đặt chiều rộng cột
//                    int columnWidth = dgvLowStock.Width / 4;
//                    dgvLowStock.Columns["id"].Width = columnWidth;
//                    dgvLowStock.Columns["name"].Width = columnWidth;
//                    dgvLowStock.Columns["stock_quantity"].Width = columnWidth;
//                    dgvLowStock.Columns["colAction"].Width = columnWidth;

//                    // Các thiết lập căn giữa và readonly
//                    dgvLowStock.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    dgvLowStock.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    dgvLowStock.Columns["colAction"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//                    dgvLowStock.Columns["id"].ReadOnly = true;
//                    dgvLowStock.Columns["name"].ReadOnly = true;
//                    dgvLowStock.Columns["stock_quantity"].ReadOnly = true;

//                    // Thiết lập header
//                    dgvLowStock.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//                    dgvLowStock.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
//                    dgvLowStock.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//                    dgvLowStock.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    dgvLowStock.EnableHeadersVisualStyles = false;
//                    dgvLowStock.ColumnHeadersHeight = 45;
//                    dgvLowStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//                    dgvLowStock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

//                    // Bỏ border
//                    dgvLowStock.BorderStyle = BorderStyle.None;
//                    dgvLowStock.CellBorderStyle = DataGridViewCellBorderStyle.None;

//                    // Thiết lập hàng
//                    dgvLowStock.RowTemplate.Height = 40;
//                    dgvLowStock.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
//                    dgvLowStock.DefaultCellStyle.SelectionBackColor = dgvLowStock.DefaultCellStyle.BackColor;
//                    dgvLowStock.DefaultCellStyle.SelectionForeColor = dgvLowStock.DefaultCellStyle.ForeColor;

//                    // Đăng ký sự kiện CellPainting để tùy chỉnh button
//                    dgvLowStock.CellPainting += DgvLowStock_CellPainting;

//                    // Đăng ký sự kiện CellClick cho button
//                    dgvLowStock.CellClick += DgvLowStock_CellClick;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải cảnh báo hết hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Tách hàm xử lý sự kiện CellPainting ra riêng để dễ quản lý
//        private void DgvLowStock_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            // Chỉ vẽ cho cột button và có dữ liệu
//            if (e.ColumnIndex >= 0 &&
//                e.RowIndex >= 0 &&
//                dgvLowStock.Columns[e.ColumnIndex].Name == "colAction")
//            {
//                e.PaintBackground(e.ClipBounds, true);

//                Rectangle buttonRect = new Rectangle(
//                    e.CellBounds.X + 2,
//                    e.CellBounds.Y + 2,
//                    e.CellBounds.Width - 4,
//                    e.CellBounds.Height - 4);

//                using (SolidBrush buttonBrush = new SolidBrush(Color.DeepSkyBlue))
//                {
//                    e.Graphics.FillRectangle(buttonBrush, buttonRect);

//                    using (StringFormat sf = new StringFormat())
//                    {
//                        sf.Alignment = StringAlignment.Center;
//                        sf.LineAlignment = StringAlignment.Center;

//                        using (Font buttonFont = new Font("Segoe UI", 10F))
//                        {
//                            e.Graphics.DrawString("Nhập hàng",
//                                buttonFont,
//                                Brushes.White,
//                                buttonRect,
//                                sf);
//                        }
//                    }
//                }
//                e.Handled = true;
//            }
//        }

//        // Tách hàm xử lý sự kiện CellClick ra riêng để dễ quản lý
//        private void DgvLowStock_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvLowStock.Columns[e.ColumnIndex].Name == "colAction")
//            {
//                int productId = Convert.ToInt32(dgvLowStock.Rows[e.RowIndex].Cells["id"].Value);
//                string productName = dgvLowStock.Rows[e.RowIndex].Cells["name"].Value.ToString();

//                // Xử lý sự kiện nhập hàng cho sản phẩm
//                MessageBox.Show($"Mở form nhập hàng cho sản phẩm: {productName} (ID: {productId})");
//            }
//        }
//        private void label3_Click(object sender, EventArgs e)
//        {
//            // Code xử lý khi click vào label3
//        }
//    }
//}


// Code chính 


//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WareHouse.Utils;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class BaseForm : Form
//    {
//        private Button selectedButton; // Lưu button hiện tại được chọn
//        private Color defaultButtonColor = Color.FromArgb(44, 62, 80); // Màu mặc định của button
//        private Color selectedButtonColor = Color.DodgerBlue; // Màu khi button được chọn
//        protected int RoleId { get; set; } // Thuộc tính lưu roleId

//        public BaseForm()
//        {
//            InitializeComponent();
//            // Default constructor for designer support
//            this.RoleId = -1; // Or some default value
//        }

//        public BaseForm(int roleId)
//        {
//            InitializeComponent();
//            this.RoleId = roleId; // Lưu roleId

//            // Code để làm tròn góc cho button1
//            int radius = 15;

//            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
//            path.AddArc(0, 0, radius, radius, 180, 90);
//            path.AddArc(button1.Width - radius, 0, radius, radius, 270, 90);
//            path.AddArc(button1.Width - radius, button1.Height - radius, radius, radius, 0, 90);
//            path.AddArc(0, button1.Height - radius, radius, radius, 90, 90);
//            path.CloseFigure();

//            button1.Region = new Region(path);
//            button1.FlatStyle = FlatStyle.Flat;
//            button1.FlatAppearance.BorderSize = 0;

//            // Danh sách các button trong sidebar cần bo tròn
//            List<Button> sidebarButtons = new List<Button>
//            {
//                btnDashBoard, btnProduct, btnEnterWarehouse,
//                btnWarehouseExport, btnReport, btnAccManage,
//            };

//            // Bo tròn cho các button trong sidebar và thiết lập màu mặc định
//            foreach (Button btn in sidebarButtons)
//            {
//                UIHelper.ApplyRoundedCorners(btn, 5);
//                btn.BackColor = defaultButtonColor; // Thiết lập màu mặc định
//            }

//            // Bo tròn cho panel Avatar
//            UIHelper.ApplyRoundedCorners(panelAvatar, 5);

//            // Gán sự kiện Click cho các button trong sidebar
//            btnDashBoard.Click += (s, e) =>
//            {
//                SetSelectedButton(btnDashBoard);
//                NavigateToForm(new DashBoardForm(RoleId)); // Truyền RoleId
//            };

//            btnProduct.Click += (s, e) =>
//            {
//                SetSelectedButton(btnProduct);
//                NavigateToForm(new ProductManagementForm(RoleId)); // Truyền RoleId
//            };

//            btnEnterWarehouse.Click += (s, e) =>
//            {
//                SetSelectedButton(btnEnterWarehouse);
//                NavigateToForm(new ImportWareHouseForm(RoleId)); // Truyền RoleId
//            };

//            btnWarehouseExport.Click += (s, e) =>
//            {
//                SetSelectedButton(btnWarehouseExport);
//                NavigateToForm(new ExportWareHouseForm(RoleId)); // Truyền RoleId
//            };

//            btnReport.Click += (s, e) =>
//            {
//                SetSelectedButton(btnReport);
//                NavigateToForm(new ReportForm(RoleId)); // Truyền RoleId
//            };

//            btnAccManage.Click += (s, e) =>
//            {
//                SetSelectedButton(btnAccManage);
//                NavigateToForm(new UserManagementForm(RoleId)); // Truyền RoleId
//            };

//            // Thiết lập button mặc định được chọn (dựa trên form hiện tại)
//            if (this is ImportWareHouseForm)
//            {
//                SetSelectedButton(btnEnterWarehouse);
//            }
//            else if (this is ProductManagementForm)
//            {
//                SetSelectedButton(btnProduct);
//            }
//            else if (this is DashBoardForm)
//            {
//                SetSelectedButton(btnDashBoard);
//            }
//            else if (this is ExportWareHouseForm)
//            {
//                SetSelectedButton(btnWarehouseExport);
//            }
//            else if (this is ReportForm)
//            {
//                SetSelectedButton(btnReport);
//            }
//            else if (this is UserManagementForm)
//            {
//                SetSelectedButton(btnAccManage);
//            }
//        }

//        private void SetSelectedButton(Button button)
//        {
//            // Khôi phục màu mặc định cho button trước đó (nếu có)
//            if (selectedButton != null)
//            {
//                selectedButton.BackColor = defaultButtonColor;
//            }

//            // Đặt màu cho button mới được chọn
//            selectedButton = button;
//            selectedButton.BackColor = selectedButtonColor;
//        }

//        private void NavigateToForm(Form newForm)
//        {
//            // Ẩn form hiện tại5
//            this.Hide();

//            // Mở form mới
//            newForm.Show();

//            // Đóng form hiện tại khi form mới đóng
//            newForm.FormClosed += (s, args) => this.Close();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WareHouse.Utils;
using MySql.Data.MySqlClient;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class BaseForm : Form
    {
        private Button selectedButton;
        private Color defaultButtonColor = Color.FromArgb(44, 62, 80);
        private Color selectedButtonColor = Color.DodgerBlue;
        protected int RoleId { get; set; }

        public BaseForm()
        {
            InitializeComponent();
            this.RoleId = -1;
            this.WindowState = FormWindowState.Maximized; // Đặt form ở trạng thái fullscreen
        }

        public BaseForm(int roleId)
        {
            InitializeComponent();
            this.RoleId = roleId;
            this.WindowState = FormWindowState.Maximized; // Đặt form ở trạng thái fullscreen

            List<Button> sidebarButtons = new List<Button>
            {
                btnDashBoard, btnProduct, btnEnterWarehouse,
                btnWarehouseExport, btnReport, btnAccManage, btnLogout
            };

            foreach (Button btn in sidebarButtons)
            {
                UIHelper.ApplyRoundedCorners(btn, 5);
                btn.BackColor = defaultButtonColor;
            }

            btnLogout.BackColor = Color.FromArgb(231, 76, 60);

            btnDashBoard.Click += (s, e) =>
            {
                SetSelectedButton(btnDashBoard);
                NavigateToForm(new DashBoardForm(RoleId));
            };

            btnProduct.Click += (s, e) =>
            {
                SetSelectedButton(btnProduct);
                NavigateToForm(new ProductManagementForm(RoleId));
            };

            btnEnterWarehouse.Click += (s, e) =>
            {
                SetSelectedButton(btnEnterWarehouse);
                NavigateToForm(new ImportWareHouseForm(RoleId));
            };

            btnWarehouseExport.Click += (s, e) =>
            {
                SetSelectedButton(btnWarehouseExport);
                NavigateToForm(new ExportWareHouseForm(RoleId));
            };

            btnReport.Click += (s, e) =>
            {
                SetSelectedButton(btnReport);
                NavigateToForm(new ReportForm(RoleId));
            };

            btnAccManage.Click += (s, e) =>
            {
                SetSelectedButton(btnAccManage);
                NavigateToForm(new UserManagementForm(RoleId));
            };

            btnLogout.Click += (s, e) =>
            {
                string username = LoginForm.CurrentUsername ?? "Unknown";
                LogActivity(username, $"{username} đã đăng xuất {DateTime.Now}");
                RoleId = -1;
                this.Close();
            };

            if (this is ImportWareHouseForm) SetSelectedButton(btnEnterWarehouse);
            else if (this is ProductManagementForm) SetSelectedButton(btnProduct);
            else if (this is DashBoardForm) SetSelectedButton(btnDashBoard);
            else if (this is ExportWareHouseForm) SetSelectedButton(btnWarehouseExport);
            else if (this is ReportForm) SetSelectedButton(btnReport);
            else if (this is UserManagementForm) SetSelectedButton(btnAccManage);
        }

        private void LogActivity(string username, string action)
        {
            try
            {
                string query = "INSERT INTO activity_logs (user_id, action, timestamp) VALUES ((SELECT id FROM users WHERE username = @username), @action, @timestamp)";
                using (MySqlConnection connection = DatabaseHelper.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@action", action);
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi log: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetSelectedButton(Button button)
        {
            if (selectedButton != null)
            {
                selectedButton.BackColor = defaultButtonColor;
            }
            selectedButton = button;
            selectedButton.BackColor = selectedButtonColor;
        }

        private void NavigateToForm(Form newForm)
        {
            this.Hide();
            newForm.WindowState = FormWindowState.Maximized; // Đặt form mới ở trạng thái fullscreen
            newForm.Show();
            newForm.FormClosed += (s, args) => this.Close();
        }
    }
}