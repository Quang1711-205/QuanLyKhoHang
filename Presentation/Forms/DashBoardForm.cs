//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using MySql.Data.MySqlClient;
//using WareHouse.Utils;
//using WareHouse.DataAccess;
//using System.Threading.Tasks;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class DashBoardForm : BaseForm
//    {
//        // Constants for layout management
//        private const int SIDEBAR_WIDTH = 250;
//        private const int HEADER_HEIGHT = 40;
//        private const int MIN_CARD_WIDTH = 200;
//        private const int CARD_MARGIN = 10;
//        private const int SECTION_MARGIN = 20; // Added section margin constant

//        // UI Components
//        private Panel contentPanel;
//        private TableLayoutPanel mainContainer;
//        private FlowLayoutPanel summaryCardsPanel;
//        private Panel activitiesPanel;
//        private Panel lowStockPanel;
//        private FlowLayoutPanel activitiesItemsContainer;
//        private DataGridView lowStockGridView;

//        // Track summary cards and window state
//        private Dictionary<string, Label> summaryLabels = new Dictionary<string, Label>();
//        private FormWindowState previousWindowState;
//        private bool layoutInitialized = false;

//        public DashBoardForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//            this.previousWindowState = this.WindowState;

//            // Validate user role before showing dashboard
//            if (!IsValidRole())
//            {
//                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Dashboard.",
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                this.Close();
//                return;
//            }

//            // Set up the main UI structure
//            InitializeUI();

//            // Register resize events for responsive layout updates
//            this.Load += DashBoardForm_Load;
//            this.SizeChanged += DashBoardForm_SizeChanged;
//            this.ResizeEnd += DashBoardForm_ResizeEnd;
//        }

//        private bool IsValidRole()
//        {
//            return RoleId == 1 || RoleId == 3; // 1: Admin, 3: Kế toán
//        }

//        private void InitializeUI()
//        {
//            // Create or find the main content panel
//            contentPanel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "contentPanel");
//            if (contentPanel == null)
//            {
//                contentPanel = new Panel
//                {
//                    Dock = DockStyle.Fill,
//                    BackColor = Color.White,
//                    Name = "contentPanel"
//                };
//                this.Controls.Add(contentPanel);
//            }

//            // Create the main container with proper padding to account for sidebar and header
//            mainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(SIDEBAR_WIDTH + 10, HEADER_HEIGHT + 10, 10, 10),
//                BackColor = Color.White,
//                ColumnCount = 1,
//                RowCount = 4,
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink
//            };

//            // Configure rows for different dashboard sections with improved spacing
//            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Title
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Summary cards
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Activities
//            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Low stock

//            contentPanel.Controls.Add(mainContainer);

//            // Add dashboard title
//            CreateDashboardTitle();

//            // Create summary cards section
//            CreateSummaryCardsSection();

//            // Create recent activities section
//            CreateRecentActivitiesSection();

//            // Create low stock warnings section
//            CreateLowStockSection();
//        }

//        private void CreateDashboardTitle()
//        {
//            Label titleLabel = new Label
//            {
//                Text = "Tổng Quan",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                AutoSize = true,
//                Dock = DockStyle.Left,
//                Padding = new Padding(10, 10, 0, 0)
//            };
//            mainContainer.Controls.Add(titleLabel, 0, 0);
//        }

//        private void CreateSummaryCardsSection()
//        {
//            // Panel to hold summary cards with wrapping support
//            summaryCardsPanel = new FlowLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = true,
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                MinimumSize = new Size(200, 80),
//                Margin = new Padding(CARD_MARGIN)
//            };

//            mainContainer.Controls.Add(summaryCardsPanel, 0, 1);

//            // Add summary cards
//            AddSummaryCard("Doanh thu tháng", "0", Color.FromArgb(52, 152, 219), "revenueSummary");
//            AddSummaryCard("Tổng giá trị nhập kho (tháng)", "0", Color.FromArgb(46, 204, 113), "warehouseEntrySummary");
//            AddSummaryCard("Tổng giá trị xuất kho (tháng)", "0", Color.FromArgb(231, 76, 60), "warehouseExitSummary");
//            AddSummaryCard("Tổng số sản phẩm trong kho", "0", Color.FromArgb(255, 202, 40), "totalStockSummary");
//        }

//        private void AddSummaryCard(string title, string value, Color color, string name)
//        {
//            int cardWidth = CalculateCardWidth();
//            int cardHeight = 100;

//            Panel card = new Panel
//            {
//                Width = cardWidth,
//                Height = cardHeight,
//                Margin = new Padding(CARD_MARGIN),
//                BackColor = color,
//                Name = $"card_{name}",
//                MinimumSize = new Size(MIN_CARD_WIDTH, 80)
//            };

//            // Apply rounded corners
//            ApplyRoundedCorners(card, 8);

//            // Title label
//            Label titleLabel = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 10F),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, 15)
//            };
//            card.Controls.Add(titleLabel);

//            // Value label
//            Label valueLabel = new Label
//            {
//                Text = value,
//                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, titleLabel.Bottom + 10),
//                Name = name
//            };
//            card.Controls.Add(valueLabel);

//            // Store reference to value label for later updates
//            summaryLabels[name] = valueLabel;
//            summaryCardsPanel.Controls.Add(card);
//        }

//        private void CreateRecentActivitiesSection()
//        {
//            // Panel for recent activities with improved margin
//            activitiesPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Margin = new Padding(CARD_MARGIN, SECTION_MARGIN, CARD_MARGIN, SECTION_MARGIN), // Improved margin
//                BackColor = Color.White,
//                BorderStyle = BorderStyle.None,
//                Padding = new Padding(0, 35, 0, 0)
//            };
//            mainContainer.Controls.Add(activitiesPanel, 0, 2);

//            // Title for activities section
//            Label activityTitleLabel = new Label
//            {
//                Text = "Hoạt động gần đây",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                AutoSize = true,
//                Dock = DockStyle.None,
//                Padding = new Padding(5, 5, 0, 10),
//                Location = new Point(0, -10)
//            };
//            activitiesPanel.Controls.Add(activityTitleLabel);

//            // Container for activity items - FIXED: Increased top margin to avoid overlap with title
//            activitiesItemsContainer = new FlowLayoutPanel
//            {
//                FlowDirection = FlowDirection.TopDown,
//                WrapContents = false,
//                AutoScroll = true,
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Padding = new Padding(0, 0, 0, 5),
//                Margin = new Padding(0, 20, 0, 0) // Increased from 35 to 45 to create more space
//            };
//            activitiesPanel.Controls.Add(activitiesItemsContainer);

//            // Handle resize events to maintain activity item layout
//            activitiesItemsContainer.Resize += (s, e) => UpdateActivityItemsLayout();
//        }

//        private void CreateLowStockSection()
//        {
//            // Panel for low stock warnings with improved margin to prevent overlap
//            lowStockPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Margin = new Padding(CARD_MARGIN, SECTION_MARGIN, CARD_MARGIN, CARD_MARGIN), // Improved margin
//                BackColor = Color.White,
//                BorderStyle = BorderStyle.None,
//                Padding = new Padding(0, 35, 0, 0)
//            };
//            mainContainer.Controls.Add(lowStockPanel, 0, 3);

//            // Title for low stock section
//            Label lowStockTitleLabel = new Label
//            {
//                Text = "Cảnh báo sắp hết hàng",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                AutoSize = true,
//                Dock = DockStyle.None,
//                Padding = new Padding(5, 5, 0, 10),
//                Location = new Point(0, -10),
//            };
//            lowStockPanel.Controls.Add(lowStockTitleLabel);

//            // DataGridView for low stock items - FIXED: Added top margin to avoid overlap with title
//            lowStockGridView = new DataGridView
//            {
//                Dock = DockStyle.Fill,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                AllowUserToAddRows = false,
//                AllowUserToDeleteRows = false,
//                ReadOnly = true,
//                BorderStyle = BorderStyle.None,
//                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
//                BackgroundColor = Color.White,
//                RowHeadersVisible = false,
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                //Margin = new Padding(0, 20, 0, 0) // Added top margin for spacing
//            };

//            lowStockPanel.Controls.Add(lowStockGridView);
//        }

//        private int CalculateCardWidth()
//        {
//            // Calculate width based on available space
//            int availableWidth = summaryCardsPanel.Width - 2 * CARD_MARGIN;
//            if (availableWidth <= 0)
//                availableWidth = contentPanel.Width - SIDEBAR_WIDTH - 40;

//            // Determine number of cards per row based on window width
//            int cardsPerRow;
//            if (this.Width < 1000)
//                cardsPerRow = 1;
//            else if (this.Width < 1400)
//                cardsPerRow = 2;
//            else
//                cardsPerRow = 4;

//            // Calculate optimal width considering margins
//            int cardWidth = (availableWidth / cardsPerRow) - (2 * CARD_MARGIN);

//            // Ensure minimum card width
//            return Math.Max(cardWidth, MIN_CARD_WIDTH);
//        }

//        private void UpdateActivityItemsLayout()
//        {
//            if (activitiesItemsContainer?.Controls == null || activitiesItemsContainer.Controls.Count == 0)
//                return;

//            // Calculate available width accounting for scrollbar if present
//            int availableWidth = activitiesItemsContainer.ClientSize.Width;
//            if (activitiesItemsContainer.VerticalScroll.Visible)
//                availableWidth -= SystemInformation.VerticalScrollBarWidth;

//            // Update width of each activity item
//            foreach (Control control in activitiesItemsContainer.Controls)
//            {
//                if (control is Panel activityPanel && activityPanel.Tag?.ToString() == "activity_item")
//                {
//                    activityPanel.Width = Math.Max(0, availableWidth - 10);

//                    // Update internal labels if needed
//                    Label actionLabel = activityPanel.Controls.OfType<Label>()
//                        .FirstOrDefault(l => l.Tag?.ToString() == "action");
//                    Label timeLabel = activityPanel.Controls.OfType<Label>()
//                        .FirstOrDefault(l => l.Tag?.ToString() == "time");

//                    if (actionLabel != null && timeLabel != null)
//                    {
//                        timeLabel.Width = 80; // Fixed width for time
//                        actionLabel.Width = activityPanel.Width - timeLabel.Width - 20;
//                    }
//                }
//            }
//        }

//        private void UpdateSummaryCardSizes()
//        {
//            int cardWidth = CalculateCardWidth();

//            // Update width of all summary cards
//            foreach (Panel card in summaryCardsPanel.Controls.OfType<Panel>())
//            {
//                card.Width = cardWidth;
//            }
//        }

//        private void UpdateLowStockGridLayout()
//        {
//            if (lowStockGridView == null || lowStockGridView.Columns.Count == 0)
//                return;

//            // Update row height based on window state
//            lowStockGridView.RowTemplate.Height = this.WindowState == FormWindowState.Maximized ? 40 : 35;

//            // Update existing rows
//            foreach (DataGridViewRow row in lowStockGridView.Rows)
//            {
//                row.Height = lowStockGridView.RowTemplate.Height;
//            }

//            // Force refresh
//            lowStockGridView.Refresh();
//        }

//        // Apply rounded corners to panels
//        private void ApplyRoundedCorners(Control control, int radius)
//        {
//            // In a real implementation, you'd use UIHelper.ApplyRoundedCorners
//            // For simplicity, we'll leave this as a stub
//            // UIHelper.ApplyRoundedCorners(control, radius);
//        }

//        private void UpdateLayout()
//        {
//            if (this.WindowState == FormWindowState.Minimized)
//                return;

//            this.SuspendLayout();

//            UpdateSummaryCardSizes();
//            UpdateActivityItemsLayout();
//            UpdateLowStockGridLayout();

//            this.ResumeLayout(true);
//            this.Refresh();
//        }

//        #region Event Handlers

//        private async void DashBoardForm_Load(object sender, EventArgs e)
//        {
//            await LoadDashboardDataAsync();
//            LoadRecentActivities();
//            LoadLowStockWarnings();

//            layoutInitialized = true;
//            UpdateLayout();
//        }

//        private void DashBoardForm_SizeChanged(object sender, EventArgs e)
//        {
//            if (!layoutInitialized) return;

//            // Check if window state changed
//            if (previousWindowState != this.WindowState)
//            {
//                previousWindowState = this.WindowState;
//                UpdateLayout();
//            }
//            else
//            {
//                // Light update during resize
//                UpdateSummaryCardSizes();
//                UpdateActivityItemsLayout();
//            }
//        }

//        private void DashBoardForm_ResizeEnd(object sender, EventArgs e)
//        {
//            // Full layout update when resize is complete
//            UpdateLayout();
//        }

//        #endregion

//        #region Data Loading Methods

//        private async Task LoadDashboardDataAsync()
//        {
//            DatabaseHelper.InitializeConnection();
//            try
//            {
//                // Load revenue data
//                string revenueSql = @"
//                    SELECT COALESCE(SUM(oi.price * oi.quantity), 0) as TotalRevenue
//                    FROM orders o
//                    JOIN order_items oi ON o.id = oi.order_id
//                    WHERE o.status = 'Completed'
//                    AND MONTH(o.order_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(o.order_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";

//                using (MySqlCommand revenueCmd = new MySqlCommand(revenueSql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalRevenue = Convert.ToDecimal(await revenueCmd.ExecuteScalarAsync());
//                    if (summaryLabels.ContainsKey("revenueSummary"))
//                    {
//                        summaryLabels["revenueSummary"].Text = String.Format("{0:n0}", totalRevenue);
//                    }
//                }

//                // Load warehouse entry data
//                string entrySql = @"
//                    SELECT COALESCE(SUM(se.quantity * p.import_price), 0) as TotalEntryValue
//                    FROM stock_entries se
//                    JOIN products p ON se.product_id = p.id
//                    WHERE MONTH(se.entry_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(se.entry_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";

//                using (MySqlCommand entryCmd = new MySqlCommand(entrySql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalEntryValue = Convert.ToDecimal(await entryCmd.ExecuteScalarAsync());
//                    if (summaryLabels.ContainsKey("warehouseEntrySummary"))
//                    {
//                        summaryLabels["warehouseEntrySummary"].Text = String.Format("{0:n0}", totalEntryValue);
//                    }
//                }

//                // Load warehouse exit data
//                string exitSql = @"
//                    SELECT COALESCE(SUM(sx.quantity * p.export_price), 0) as TotalExitValue
//                    FROM stock_exits sx
//                    JOIN products p ON sx.product_id = p.id
//                    WHERE MONTH(sx.exit_date) = MONTH(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))
//                    AND YEAR(sx.exit_date) = YEAR(DATE_SUB(CURRENT_DATE(), INTERVAL 1 MONTH))";

//                using (MySqlCommand exitCmd = new MySqlCommand(exitSql, DatabaseHelper.GetConnection()))
//                {
//                    decimal totalExitValue = Convert.ToDecimal(await exitCmd.ExecuteScalarAsync());
//                    if (summaryLabels.ContainsKey("warehouseExitSummary"))
//                    {
//                        summaryLabels["warehouseExitSummary"].Text = String.Format("{0:n0}", totalExitValue);
//                    }
//                }

//                // Load total stock data
//                string totalStockSql = @"
//                    SELECT COALESCE(SUM(stock_quantity), 0) as TotalStock
//                    FROM products";

//                using (MySqlCommand totalStockCmd = new MySqlCommand(totalStockSql, DatabaseHelper.GetConnection()))
//                {
//                    int totalStock = Convert.ToInt32(await totalStockCmd.ExecuteScalarAsync());
//                    if (summaryLabels.ContainsKey("totalStockSummary"))
//                    {
//                        summaryLabels["totalStockSummary"].Text = totalStock.ToString();
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải dữ liệu tổng quan: " + ex.Message,
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void LoadRecentActivities()
//        {
//            DatabaseHelper.InitializeConnection();
//            try
//            {
//                // Clear existing activities
//                activitiesItemsContainer.Controls.Clear();

//                // Load recent activities from database
//                string activitySql = @"
//                    SELECT al.action, al.timestamp
//                    FROM activity_logs al
//                    LEFT JOIN users u ON al.user_id = u.id
//                    ORDER BY al.timestamp DESC
//                    LIMIT 5";

//                using (MySqlCommand activityCmd = new MySqlCommand(activitySql, DatabaseHelper.GetConnection()))
//                {
//                    using (MySqlDataReader reader = activityCmd.ExecuteReader())
//                    {
//                        bool hasActivities = false;

//                        while (reader.Read())
//                        {
//                            hasActivities = true;
//                            string action = reader["action"].ToString();
//                            DateTime timestamp = Convert.ToDateTime(reader["timestamp"]);
//                            TimeSpan timeDiff = DateTime.Now - timestamp;

//                            // Create and add activity item
//                            Panel activityItem = CreateActivityItem(action, FormatTimeAgo(timeDiff));
//                            activitiesItemsContainer.Controls.Add(activityItem);
//                        }

//                        // Display message if no activities
//                        if (!hasActivities)
//                        {
//                            Label noActivitiesLabel = new Label
//                            {
//                                Text = "Không có hoạt động nào gần đây.",
//                                Font = new Font("Segoe UI", 10F),
//                                ForeColor = Color.Gray,
//                                AutoSize = true,
//                                TextAlign = ContentAlignment.MiddleCenter,
//                                Padding = new Padding(10)
//                            };
//                            activitiesItemsContainer.Controls.Add(noActivitiesLabel);
//                        }
//                    }
//                }

//                // Update layout
//                UpdateActivityItemsLayout();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải hoạt động gần đây: " + ex.Message,
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private Panel CreateActivityItem(string action, string timeAgo)
//        {
//            // Calculate panel height based on window state
//            int panelHeight = this.WindowState == FormWindowState.Maximized ? 45 : 35;

//            // Create the activity panel
//            Panel activityPanel = new Panel
//            {
//                Height = panelHeight,
//                Width = activitiesItemsContainer.ClientSize.Width - 10,
//                BackColor = Color.FromArgb(245, 245, 245),
//                Margin = new Padding(0, 0, 0, 8),
//                Tag = "activity_item"
//            };
//            ApplyRoundedCorners(activityPanel, 5);

//            // Time label (right-aligned)
//            Label timeLabel = new Label
//            {
//                Text = timeAgo,
//                Font = new Font("Segoe UI", 9F),
//                ForeColor = Color.DarkGray,
//                TextAlign = ContentAlignment.MiddleRight,
//                Width = 80,
//                Dock = DockStyle.Right,
//                Padding = new Padding(0, 0, 10, 0),
//                Tag = "time"
//            };
//            activityPanel.Controls.Add(timeLabel);

//            // Action label (left-aligned, fills remaining space)
//            Label actionLabel = new Label
//            {
//                Text = action,
//                Font = new Font("Segoe UI", 10F),
//                AutoEllipsis = true,
//                TextAlign = ContentAlignment.MiddleLeft,
//                Dock = DockStyle.Fill,
//                Padding = new Padding(10, 0, 0, 0),
//                Tag = "action"
//            };
//            activityPanel.Controls.Add(actionLabel);

//            return activityPanel;
//        }

//        private string FormatTimeAgo(TimeSpan diff)
//        {
//            if (diff.TotalMinutes < 1) return "Vừa xong";
//            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} phút trước";
//            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} giờ trước";
//            if (diff.TotalDays < 30) return $"{(int)diff.TotalDays} ngày trước";
//            return $"{(int)(diff.TotalDays / 30)} tháng trước";
//        }

//        private void LoadLowStockWarnings()
//        {
//            try
//            {
//                // Clear existing event handlers
//                if (lowStockGridView != null)
//                {
//                    lowStockGridView.CellClick -= LowStockGridView_CellClick;
//                    lowStockGridView.CellPainting -= LowStockGridView_CellPainting;
//                }

//                // Load low stock data
//                string lowStockSql = @"
//                    SELECT p.id, p.name, p.stock_quantity
//                    FROM products p
//                    WHERE p.stock_quantity <= 10
//                    ORDER BY p.stock_quantity ASC";

//                using (MySqlCommand lowStockCmd = new MySqlCommand(lowStockSql, DatabaseHelper.GetConnection()))
//                {
//                    DataTable lowStockTable = new DataTable();
//                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(lowStockCmd))
//                    {
//                        adapter.Fill(lowStockTable);
//                    }

//                    // Configure grid view
//                    lowStockGridView.DataSource = null;
//                    lowStockGridView.Columns.Clear();
//                    lowStockGridView.DataSource = lowStockTable;

//                    // Configure columns
//                    lowStockGridView.Columns["id"].HeaderText = "Mã SP";
//                    lowStockGridView.Columns["name"].HeaderText = "Tên sản phẩm";
//                    lowStockGridView.Columns["stock_quantity"].HeaderText = "Còn lại";

//                    // Add action button column
//                    DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn
//                    {
//                        HeaderText = "Hành động",
//                        Name = "actionColumn",
//                        Text = "Nhập hàng",
//                        UseColumnTextForButtonValue = true
//                    };
//                    lowStockGridView.Columns.Add(actionColumn);

//                    // Configure column styles
//                    lowStockGridView.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    lowStockGridView.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    lowStockGridView.Columns["actionColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//                    // Configure header styles
//                    lowStockGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//                    lowStockGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
//                    lowStockGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//                    lowStockGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    lowStockGridView.EnableHeadersVisualStyles = false;
//                    lowStockGridView.ColumnHeadersHeight = 40;

//                    // Configure grid style
//                    lowStockGridView.BorderStyle = BorderStyle.None;
//                    lowStockGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
//                    lowStockGridView.GridColor = Color.FromArgb(230, 230, 230);
//                    lowStockGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
//                    lowStockGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

//                    // Row height
//                    lowStockGridView.RowTemplate.Height = this.WindowState == FormWindowState.Maximized ? 40 : 35;

//                    // Register event handlers
//                    lowStockGridView.CellPainting += LowStockGridView_CellPainting;
//                    lowStockGridView.CellClick += LowStockGridView_CellClick;
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tải cảnh báo hết hàng: " + ex.Message,
//                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void LowStockGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            // Custom paint the action button cells
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
//                lowStockGridView.Columns[e.ColumnIndex].Name == "actionColumn")
//            {
//                e.PaintBackground(e.ClipBounds, true);

//                // Draw custom button
//                Rectangle buttonRect = new Rectangle(
//                    e.CellBounds.X + 3,
//                    e.CellBounds.Y + 3,
//                    e.CellBounds.Width - 6,
//                    e.CellBounds.Height - 6);

//                using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(66, 165, 245)))
//                {
//                    e.Graphics.FillRectangle(buttonBrush, buttonRect);

//                    // Draw button text
//                    using (StringFormat sf = new StringFormat())
//                    {
//                        sf.Alignment = StringAlignment.Center;
//                        sf.LineAlignment = StringAlignment.Center;

//                        e.Graphics.DrawString("Nhập hàng",
//                            new Font("Segoe UI", 10F),
//                            Brushes.White,
//                            buttonRect,
//                            sf);
//                    }
//                }

//                e.Handled = true;
//            }
//        }

//        private void LowStockGridView_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            // Handle action button clicks
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
//                lowStockGridView.Columns[e.ColumnIndex].Name == "actionColumn")
//            {
//                int productId = Convert.ToInt32(lowStockGridView.Rows[e.RowIndex].Cells["id"].Value);
//                string productName = lowStockGridView.Rows[e.RowIndex].Cells["name"].Value.ToString();

//                // Show message (in a real app, this would open a form to add stock)
//                MessageBox.Show($"Mở form nhập hàng cho sản phẩm: {productName} (ID: {productId})");
//            }
//        }

//        #endregion
//    }
//}

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WareHouse.Utils;
using WareHouse.DataAccess;
using System.Threading.Tasks;

namespace WareHouse.Presentation.Forms
{
    public partial class DashBoardForm : BaseForm
    {
        // Constants for layout management
        private const int SIDEBAR_WIDTH = 250;
        private const int HEADER_HEIGHT = 40;
        private const int MIN_CARD_WIDTH = 200;
        private const int CARD_MARGIN = 10;
        private const int SECTION_MARGIN = 20; // Added section margin constant

        // UI Components
        private Panel contentPanel;
        private TableLayoutPanel mainContainer;
        private FlowLayoutPanel summaryCardsPanel;
        private Panel activitiesPanel;
        private Panel lowStockPanel;
        private FlowLayoutPanel activitiesItemsContainer;
        private DataGridView lowStockGridView;

        // Track summary cards and window state
        private Dictionary<string, Label> summaryLabels = new Dictionary<string, Label>();
        private FormWindowState previousWindowState;
        private bool layoutInitialized = false;

        public DashBoardForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.previousWindowState = this.WindowState;

            // Validate user role before showing dashboard
            if (!IsValidRole())
            {
                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Dashboard.",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            // Set up the main UI structure
            InitializeUI();

            // Register resize events for responsive layout updates
            this.Load += DashBoardForm_Load;
            this.SizeChanged += DashBoardForm_SizeChanged;
            this.ResizeEnd += DashBoardForm_ResizeEnd;
        }

        private bool IsValidRole()
        {
            return RoleId == 1 || RoleId == 3; // 1: Admin, 3: Kế toán
        }

        private void InitializeUI()
        {
            // Create or find the main content panel
            contentPanel = this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "contentPanel");
            if (contentPanel == null)
            {
                contentPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.White,
                    Name = "contentPanel"
                };
                this.Controls.Add(contentPanel);
            }

            // Create the main container with proper padding to account for sidebar and header
            mainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(SIDEBAR_WIDTH + 10, HEADER_HEIGHT + 10, 10, 10),
                BackColor = Color.White,
                ColumnCount = 1,
                RowCount = 4,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            // Configure rows for different dashboard sections with improved spacing
            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F)); // Title
            mainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Summary cards
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Activities
            mainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 50F)); // Low stock

            contentPanel.Controls.Add(mainContainer);

            // Add dashboard title
            CreateDashboardTitle();

            // Create summary cards section
            CreateSummaryCardsSection();

            // Create recent activities section
            CreateRecentActivitiesSection();

            // Create low stock warnings section
            CreateLowStockSection();
        }

        private void CreateDashboardTitle()
        {
            Label titleLabel = new Label
            {
                Text = "Tổng Quan",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.Left,
                Padding = new Padding(10, 10, 0, 0)
            };
            mainContainer.Controls.Add(titleLabel, 0, 0);
        }

        private void CreateSummaryCardsSection()
        {
            // Panel to hold summary cards with wrapping support
            summaryCardsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MinimumSize = new Size(200, 80),
                Margin = new Padding(CARD_MARGIN)
            };

            mainContainer.Controls.Add(summaryCardsPanel, 0, 1);

            // Add summary cards
            AddSummaryCard("Lợi nhuận tháng", "0", Color.FromArgb(52, 152, 219), "revenueSummary");
            AddSummaryCard("Tổng giá trị nhập kho (tháng)", "0", Color.FromArgb(46, 204, 113), "warehouseEntrySummary");
            AddSummaryCard("Tổng giá trị xuất kho (tháng)", "0", Color.FromArgb(231, 76, 60), "warehouseExitSummary");
            AddSummaryCard("Tổng số sản phẩm trong kho", "0", Color.FromArgb(255, 202, 40), "totalStockSummary");
        }

        private void AddSummaryCard(string title, string value, Color color, string name)
        {
            int cardWidth = CalculateCardWidth();
            int cardHeight = 100;

            Panel card = new Panel
            {
                Width = cardWidth,
                Height = cardHeight,
                Margin = new Padding(CARD_MARGIN),
                BackColor = color,
                Name = $"card_{name}",
                MinimumSize = new Size(MIN_CARD_WIDTH, 80)
            };

            // Apply rounded corners
            ApplyRoundedCorners(card, 8);

            // Title label
            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 15)
            };
            card.Controls.Add(titleLabel);

            // Value label
            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, titleLabel.Bottom + 10),
                Name = name
            };
            card.Controls.Add(valueLabel);

            // Store reference to value label for later updates
            summaryLabels[name] = valueLabel;
            summaryCardsPanel.Controls.Add(card);
        }

        private void CreateRecentActivitiesSection()
        {
            // Panel for recent activities with improved margin
            activitiesPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(CARD_MARGIN, SECTION_MARGIN, CARD_MARGIN, SECTION_MARGIN), // Improved margin
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0, 35, 0, 0)
            };
            mainContainer.Controls.Add(activitiesPanel, 0, 2);

            // Title for activities section
            Label activityTitleLabel = new Label
            {
                Text = "Hoạt động gần đây",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.None,
                Padding = new Padding(5, 5, 0, 10),
                Location = new Point(0, -10)
            };
            activitiesPanel.Controls.Add(activityTitleLabel);

            // Container for activity items - FIXED: Increased top margin to avoid overlap with title
            activitiesItemsContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0, 0, 0, 5),
                Margin = new Padding(0, 20, 0, 0) // Increased from 35 to 45 to create more space
            };
            activitiesPanel.Controls.Add(activitiesItemsContainer);

            // Handle resize events to maintain activity item layout
            activitiesItemsContainer.Resize += (s, e) => UpdateActivityItemsLayout();
        }

        private void CreateLowStockSection()
        {
            // Panel for low stock warnings with improved margin to prevent overlap
            lowStockPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(CARD_MARGIN, SECTION_MARGIN, CARD_MARGIN, CARD_MARGIN), // Improved margin
                BackColor = Color.White,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(0, 35, 0, 0)
            };
            mainContainer.Controls.Add(lowStockPanel, 0, 3);

            // Title for low stock section
            Label lowStockTitleLabel = new Label
            {
                Text = "Cảnh báo sắp hết hàng",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Dock = DockStyle.None,
                Padding = new Padding(5, 5, 0, 10),
                Location = new Point(0, -10),
            };
            lowStockPanel.Controls.Add(lowStockTitleLabel);

            // DataGridView for low stock items - FIXED: Added top margin to avoid overlap with title
            lowStockGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                //Margin = new Padding(0, 20, 0, 0) // Added top margin for spacing
            };

            lowStockPanel.Controls.Add(lowStockGridView);
        }

        private int CalculateCardWidth()
        {
            // Calculate width based on available space
            int availableWidth = summaryCardsPanel.Width - 2 * CARD_MARGIN;
            if (availableWidth <= 0)
                availableWidth = contentPanel.Width - SIDEBAR_WIDTH - 40;

            // Determine number of cards per row based on window width
            int cardsPerRow;
            if (this.Width < 1000)
                cardsPerRow = 1;
            else if (this.Width < 1400)
                cardsPerRow = 2;
            else
                cardsPerRow = 4;

            // Calculate optimal width considering margins
            int cardWidth = (availableWidth / cardsPerRow) - (2 * CARD_MARGIN);

            // Ensure minimum card width
            return Math.Max(cardWidth, MIN_CARD_WIDTH);
        }

        private void UpdateActivityItemsLayout()
        {
            if (activitiesItemsContainer?.Controls == null || activitiesItemsContainer.Controls.Count == 0)
                return;

            // Calculate available width accounting for scrollbar if present
            int availableWidth = activitiesItemsContainer.ClientSize.Width;
            if (activitiesItemsContainer.VerticalScroll.Visible)
                availableWidth -= SystemInformation.VerticalScrollBarWidth;

            // Update width of each activity item
            foreach (Control control in activitiesItemsContainer.Controls)
            {
                if (control is Panel activityPanel && activityPanel.Tag?.ToString() == "activity_item")
                {
                    activityPanel.Width = Math.Max(0, availableWidth - 10);

                    // Update internal labels if needed
                    Label actionLabel = activityPanel.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Tag?.ToString() == "action");
                    Label timeLabel = activityPanel.Controls.OfType<Label>()
                        .FirstOrDefault(l => l.Tag?.ToString() == "time");

                    if (actionLabel != null && timeLabel != null)
                    {
                        timeLabel.Width = 80; // Fixed width for time
                        actionLabel.Width = activityPanel.Width - timeLabel.Width - 20;
                    }
                }
            }
        }

        private void UpdateSummaryCardSizes()
        {
            int cardWidth = CalculateCardWidth();

            // Update width of all summary cards
            foreach (Panel card in summaryCardsPanel.Controls.OfType<Panel>())
            {
                card.Width = cardWidth;
            }
        }

        private void UpdateLowStockGridLayout()
        {
            if (lowStockGridView == null || lowStockGridView.Columns.Count == 0)
                return;

            // Update row height based on window state
            lowStockGridView.RowTemplate.Height = this.WindowState == FormWindowState.Maximized ? 40 : 35;

            // Update existing rows
            foreach (DataGridViewRow row in lowStockGridView.Rows)
            {
                row.Height = lowStockGridView.RowTemplate.Height;
            }

            // Force refresh
            lowStockGridView.Refresh();
        }

        // Apply rounded corners to panels
        private void ApplyRoundedCorners(Control control, int radius)
        {
            // In a real implementation, you'd use UIHelper.ApplyRoundedCorners
            // For simplicity, we'll leave this as a stub
            // UIHelper.ApplyRoundedCorners(control, radius);
        }

        private void UpdateLayout()
        {
            if (this.WindowState == FormWindowState.Minimized)
                return;

            this.SuspendLayout();

            UpdateSummaryCardSizes();
            UpdateActivityItemsLayout();
            UpdateLowStockGridLayout();

            this.ResumeLayout(true);
            this.Refresh();
        }

        #region Event Handlers

        private async void DashBoardForm_Load(object sender, EventArgs e)
        {
            await LoadDashboardDataAsync();
            LoadRecentActivities();
            LoadLowStockWarnings();

            layoutInitialized = true;
            UpdateLayout();
        }

        private void DashBoardForm_SizeChanged(object sender, EventArgs e)
        {
            if (!layoutInitialized) return;

            // Check if window state changed
            if (previousWindowState != this.WindowState)
            {
                previousWindowState = this.WindowState;
                UpdateLayout();
            }
            else
            {
                // Light update during resize
                UpdateSummaryCardSizes();
                UpdateActivityItemsLayout();
            }
        }

        private void DashBoardForm_ResizeEnd(object sender, EventArgs e)
        {
            // Full layout update when resize is complete
            UpdateLayout();
        }

        #endregion

        #region Data Loading Methods

        private async Task LoadDashboardDataAsync()
        {
            DatabaseHelper.InitializeConnection();
            try
            {
                // Load warehouse entry data (tổng giá trị nhập kho)
                decimal totalEntryValue = 0;
                string entrySql = @"
                    SELECT COALESCE(SUM(sed.total_price), 0) as TotalEntryValue
                    FROM stock_entry_headers seh
                    JOIN stock_entry_details sed ON seh.id = sed.stock_entry_id
                    WHERE seh.status = 'Chờ duyệt'
                    AND MONTH(seh.entry_date) = MONTH(CURRENT_DATE())
                    AND YEAR(seh.entry_date) = YEAR(CURRENT_DATE())";

                using (MySqlCommand entryCmd = new MySqlCommand(entrySql, DatabaseHelper.GetConnection()))
                {
                    totalEntryValue = Convert.ToDecimal(await entryCmd.ExecuteScalarAsync());
                    if (summaryLabels.ContainsKey("warehouseEntrySummary"))
                    {
                        summaryLabels["warehouseEntrySummary"].Text = String.Format("{0:n0}", totalEntryValue);
                    }
                }

                // Load warehouse exit data (tổng giá trị xuất kho)
                decimal totalExitValue = 0;
                string exitSql = @"
                    SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) as TotalExitValue
                    FROM stock_exit_headers seh
                    JOIN stock_exit_details sed ON seh.id = sed.stock_exit_id
                    WHERE seh.status = 'Chờ duyệt'
                    AND MONTH(seh.exit_date) = MONTH(CURRENT_DATE())
                    AND YEAR(seh.exit_date) = YEAR(CURRENT_DATE())";

                using (MySqlCommand exitCmd = new MySqlCommand(exitSql, DatabaseHelper.GetConnection()))
                {
                    totalExitValue = Convert.ToDecimal(await exitCmd.ExecuteScalarAsync());
                    if (summaryLabels.ContainsKey("warehouseExitSummary"))
                    {
                        summaryLabels["warehouseExitSummary"].Text = String.Format("{0:n0}", totalExitValue);
                    }
                }

                // Load revenue data (tổng doanh thu = tổng giá trị xuất kho - tổng giá trị nhập kho)
                decimal totalRevenue = totalExitValue - totalEntryValue;
                if (summaryLabels.ContainsKey("revenueSummary"))
                {
                    summaryLabels["revenueSummary"].Text = String.Format("{0:n0}", totalRevenue);
                }

                // Load total stock data (giữ nguyên)
                string totalStockSql = @"
                    SELECT COALESCE(SUM(stock_quantity), 0) as TotalStock
                    FROM products";

                using (MySqlCommand totalStockCmd = new MySqlCommand(totalStockSql, DatabaseHelper.GetConnection()))
                {
                    int totalStock = Convert.ToInt32(await totalStockCmd.ExecuteScalarAsync());
                    if (summaryLabels.ContainsKey("totalStockSummary"))
                    {
                        summaryLabels["totalStockSummary"].Text = totalStock.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu tổng quan: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRecentActivities()
        {
            DatabaseHelper.InitializeConnection();
            try
            {
                // Clear existing activities
                activitiesItemsContainer.Controls.Clear();

                // Load recent activities from database
                string activitySql = @"
                    SELECT al.action, al.timestamp
                    FROM activity_logs al
                    LEFT JOIN users u ON al.user_id = u.id
                    ORDER BY al.timestamp DESC
                    LIMIT 10";

                using (MySqlCommand activityCmd = new MySqlCommand(activitySql, DatabaseHelper.GetConnection()))
                {
                    using (MySqlDataReader reader = activityCmd.ExecuteReader())
                    {
                        bool hasActivities = false;

                        while (reader.Read())
                        {
                            hasActivities = true;
                            string action = reader["action"].ToString();
                            DateTime timestamp = Convert.ToDateTime(reader["timestamp"]);
                            TimeSpan timeDiff = DateTime.Now - timestamp;

                            // Create and add activity item
                            Panel activityItem = CreateActivityItem(action, FormatTimeAgo(timeDiff));
                            activitiesItemsContainer.Controls.Add(activityItem);
                        }

                        // Display message if no activities
                        if (!hasActivities)
                        {
                            Label noActivitiesLabel = new Label
                            {
                                Text = "Không có hoạt động nào gần đây.",
                                Font = new Font("Segoe UI", 10F),
                                ForeColor = Color.Gray,
                                AutoSize = true,
                                TextAlign = ContentAlignment.MiddleCenter,
                                Padding = new Padding(10)
                            };
                            activitiesItemsContainer.Controls.Add(noActivitiesLabel);
                        }
                    }
                }

                // Update layout
                UpdateActivityItemsLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hoạt động gần đây: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateActivityItem(string action, string timeAgo)
        {
            // Calculate panel height based on window state
            int panelHeight = this.WindowState == FormWindowState.Maximized ? 45 : 35;

            // Create the activity panel
            Panel activityPanel = new Panel
            {
                Height = panelHeight,
                Width = activitiesItemsContainer.ClientSize.Width - 10,
                BackColor = Color.FromArgb(245, 245, 245),
                Margin = new Padding(0, 0, 0, 8),
                Tag = "activity_item"
            };
            ApplyRoundedCorners(activityPanel, 5);

            // Time label (right-aligned)
            Label timeLabel = new Label
            {
                Text = timeAgo,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DarkGray,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 80,
                Dock = DockStyle.Right,
                Padding = new Padding(0, 0, 10, 0),
                Tag = "time"
            };
            activityPanel.Controls.Add(timeLabel);

            // Action label (left-aligned, fills remaining space)
            Label actionLabel = new Label
            {
                Text = action,
                Font = new Font("Segoe UI", 10F),
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                Dock = DockStyle.Fill,
                Padding = new Padding(10, 0, 0, 0),
                Tag = "action"
            };
            activityPanel.Controls.Add(actionLabel);

            return activityPanel;
        }

        private string FormatTimeAgo(TimeSpan diff)
        {
            if (diff.TotalMinutes < 1) return "Vừa xong";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} phút trước";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} giờ trước";
            if (diff.TotalDays < 30) return $"{(int)diff.TotalDays} ngày trước";
            return $"{(int)(diff.TotalDays / 30)} tháng trước";
        }

        private void LoadLowStockWarnings()
        {
            try
            {
                // Clear existing event handlers
                if (lowStockGridView != null)
                {
                    lowStockGridView.CellClick -= LowStockGridView_CellClick;
                    lowStockGridView.CellPainting -= LowStockGridView_CellPainting;
                }

                // Load low stock data
                string lowStockSql = @"
                    SELECT p.id, p.name, p.stock_quantity
                    FROM products p
                    WHERE p.stock_quantity <= 10
                    ORDER BY p.stock_quantity ASC";

                using (MySqlCommand lowStockCmd = new MySqlCommand(lowStockSql, DatabaseHelper.GetConnection()))
                {
                    DataTable lowStockTable = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(lowStockCmd))
                    {
                        adapter.Fill(lowStockTable);
                    }

                    // Configure grid view
                    lowStockGridView.DataSource = null;
                    lowStockGridView.Columns.Clear();
                    lowStockGridView.DataSource = lowStockTable;

                    // Configure columns
                    lowStockGridView.Columns["id"].HeaderText = "Mã SP";
                    lowStockGridView.Columns["name"].HeaderText = "Tên sản phẩm";
                    lowStockGridView.Columns["stock_quantity"].HeaderText = "Còn lại";

                    // Add action button column
                    DataGridViewButtonColumn actionColumn = new DataGridViewButtonColumn
                    {
                        HeaderText = "Hành động",
                        Name = "actionColumn",
                        Text = "Nhập hàng",
                        UseColumnTextForButtonValue = true
                    };
                    lowStockGridView.Columns.Add(actionColumn);

                    // Configure column styles
                    lowStockGridView.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    lowStockGridView.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    lowStockGridView.Columns["actionColumn"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Configure header styles
                    lowStockGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lowStockGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
                    lowStockGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    lowStockGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    lowStockGridView.EnableHeadersVisualStyles = false;
                    lowStockGridView.ColumnHeadersHeight = 40;

                    // Configure grid style
                    lowStockGridView.BorderStyle = BorderStyle.None;
                    lowStockGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    lowStockGridView.GridColor = Color.FromArgb(230, 230, 230);
                    lowStockGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 220, 220);
                    lowStockGridView.DefaultCellStyle.SelectionForeColor = Color.Black;

                    // Row height
                    lowStockGridView.RowTemplate.Height = this.WindowState == FormWindowState.Maximized ? 40 : 35;

                    // Register event handlers
                    lowStockGridView.CellPainting += LowStockGridView_CellPainting;
                    lowStockGridView.CellClick += LowStockGridView_CellClick;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải cảnh báo hết hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LowStockGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Custom paint the action button cells
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                lowStockGridView.Columns[e.ColumnIndex].Name == "actionColumn")
            {
                e.PaintBackground(e.ClipBounds, true);

                // Draw custom button
                Rectangle buttonRect = new Rectangle(
                    e.CellBounds.X + 3,
                    e.CellBounds.Y + 3,
                    e.CellBounds.Width - 6,
                    e.CellBounds.Height - 6);

                using (SolidBrush buttonBrush = new SolidBrush(Color.FromArgb(66, 165, 245)))
                {
                    e.Graphics.FillRectangle(buttonBrush, buttonRect);

                    // Draw button text
                    using (StringFormat sf = new StringFormat())
                    {
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;

                        e.Graphics.DrawString("Nhập hàng",
                            new Font("Segoe UI", 10F),
                            Brushes.White,
                            buttonRect,
                            sf);
                    }
                }

                e.Handled = true;
            }
        }

        private void LowStockGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle action button clicks
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                lowStockGridView.Columns[e.ColumnIndex].Name == "actionColumn")
            {
                int productId = Convert.ToInt32(lowStockGridView.Rows[e.RowIndex].Cells["id"].Value);
                string productName = lowStockGridView.Rows[e.RowIndex].Cells["name"].Value.ToString();

                // Show message (in a real app, this would open a form to add stock)
                MessageBox.Show($"Mở form nhập hàng cho sản phẩm: {productName} (ID: {productId})");
            }
        }

        #endregion
    }
}