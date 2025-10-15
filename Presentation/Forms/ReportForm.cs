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
//    public partial class ReportForm : Form
//    {
//        public ReportForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
//        }
//    }
//}



//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using WareHouse.DataAccess;
//using WareHouse.Presentation.Forms;

//public class ReportForm : BaseForm
//{
//    // Container chính chứa nội dung
//    private TableLayoutPanel tblMainContainer;
//    // Container cho thẻ tóm tắt
//    private FlowLayoutPanel flpSummaryCards;
//    // Container cho biểu đồ
//    private TableLayoutPanel tblCharts;
//    // Thẻ tóm tắt
//    private Label lblTotalRevenue;
//    private Label lblProfit;
//    private Label lblProfitMargin;
//    // Biểu đồ
//    private Chart chartRevenueByCategory;
//    private Chart chartProductRatio;
//    // Bộ lọc tháng
//    private ComboBox cmbMonthFilter;
//    private int selectedYear;
//    private int selectedMonth;
//    // Timer để cập nhật tự động
//    private System.Windows.Forms.Timer dataUpdateTimer;
//    private DateTime lastUpdateTime;

//    public ReportForm()
//    {
//        InitializeComponents();
//        InitializeTimer();
//    }

//    private void InitializeComponents()
//    {
//        // Cài đặt form
//        this.Text = "Báo Cáo";
//        this.Size = new Size(1200, 800);
//        this.FormBorderStyle = FormBorderStyle.Sizable; // Cho phép co giãn form
//        this.MaximizeBox = true; // Hiển thị nút phóng to/thu nhỏ

//        // Container chính (TableLayoutPanel để co giãn tự động và căn chỉnh)
//        tblMainContainer = new TableLayoutPanel
//        {
//            Dock = DockStyle.Fill,
//            RowCount = 3,
//            ColumnCount = 1,
//            Padding = new Padding(20),
//            AutoSize = true
//        };
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Bộ lọc
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Thẻ tóm tắt
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Biểu đồ
//        this.Controls.Add(tblMainContainer);

//        // Bộ lọc tháng (ComboBox)
//        Panel pnlFilter = new Panel { AutoSize = true };
//        Label lblFilter = new Label
//        {
//            Text = "Chọn Tháng:",
//            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(0, 5)
//        };
//        cmbMonthFilter = new ComboBox
//        {
//            DropDownStyle = ComboBoxStyle.DropDownList,
//            Location = new Point(100, 0),
//            Width = 150
//        };
//        for (int month = 1; month <= 12; month++)
//        {
//            cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//        }
//        cmbMonthFilter.SelectedIndex = 2; // Mặc định chọn Tháng 3/2025
//        selectedYear = 2025;
//        selectedMonth = 3;
//        cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//        pnlFilter.Controls.Add(lblFilter);
//        pnlFilter.Controls.Add(cmbMonthFilter);
//        tblMainContainer.Controls.Add(pnlFilter, 0, 0);

//        // Thẻ tóm tắt (FlowLayoutPanel để căn chỉnh thẻ)
//        flpSummaryCards = new FlowLayoutPanel
//        {
//            AutoSize = true,
//            FlowDirection = FlowDirection.LeftToRight,
//            WrapContents = false,
//            Padding = new Padding(0, 10, 0, 10)
//        };
//        // Thẻ Tổng Doanh Thu
//        Panel cardRevenue = CreateSummaryCard("Tổng Doanh Thu", "0 VNĐ");
//        lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardRevenue);
//        // Thẻ Lợi Nhuận
//        Panel cardProfit = CreateSummaryCard("Lợi Nhuận", "0 VNĐ");
//        lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardProfit);
//        // Thẻ Tỷ Lệ Lợi Nhuận
//        Panel cardProfitMargin = CreateSummaryCard("Tỷ Lệ Lợi Nhuận", "0 %");
//        lblProfitMargin = (Label)cardProfitMargin.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardProfitMargin);
//        tblMainContainer.Controls.Add(flpSummaryCards, 0, 1);

//        // Container cho biểu đồ (TableLayoutPanel để chia 2 cột)
//        tblCharts = new TableLayoutPanel
//        {
//            Dock = DockStyle.Fill,
//            RowCount = 1,
//            ColumnCount = 2,
//            Padding = new Padding(0, 10, 0, 10)
//        };
//        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
//        tblMainContainer.Controls.Add(tblCharts, 0, 2);

//        // Biểu đồ cột (Doanh Thu Theo Danh Mục)
//        chartRevenueByCategory = new Chart
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White
//        };
//        chartRevenueByCategory.ChartAreas.Add(new ChartArea());
//        tblCharts.Controls.Add(chartRevenueByCategory, 0, 0);

//        // Biểu đồ tròn (Tỉ Lệ Sản Phẩm Bán Ra)
//        chartProductRatio = new Chart
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White
//        };
//        chartProductRatio.ChartAreas.Add(new ChartArea());
//        tblCharts.Controls.Add(chartProductRatio, 1, 0);

//        // Căn giữa nội dung chính khi form co giãn
//        this.Resize += (s, e) =>
//        {
//            flpSummaryCards.Left = (flpSummaryCards.Parent.Width - flpSummaryCards.Width) / 2;
//        };
//    }

//    // Tạo thẻ tóm tắt
//    private Panel CreateSummaryCard(string title, string value)
//    {
//        Panel card = new Panel
//        {
//            Size = new Size(300, 100),
//            BorderStyle = BorderStyle.FixedSingle,
//            Margin = new Padding(10)
//        };
//        Label lblTitle = new Label
//        {
//            Text = title,
//            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(10, 10)
//        };
//        Label lblValue = new Label
//        {
//            Name = "lblValue",
//            Text = value,
//            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(10, 40),
//            ForeColor = Color.DodgerBlue
//        };
//        card.Controls.Add(lblTitle);
//        card.Controls.Add(lblValue);
//        return card;
//    }

//    // Bộ lọc tháng
//    private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string selected = cmbMonthFilter.SelectedItem.ToString();
//        selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//        selectedYear = int.Parse(selected.Split('/')[1]);
//        lastUpdateTime = DateTime.MinValue; // Reset để buộc cập nhật
//        UpdateReport();
//    }

//    // Timer để cập nhật tự động
//    private void InitializeTimer()
//    {
//        dataUpdateTimer = new System.Windows.Forms.Timer
//        {
//            Interval = 10000 // 10 giây
//        };
//        dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//        dataUpdateTimer.Start();

//        lastUpdateTime = GetLastUpdateTime();
//        UpdateReport();
//    }

//    private DateTime GetLastUpdateTime()
//    {
//        string query = $@"
//            SELECT MAX(updated_at)
//            FROM stock_exit_headers
//            WHERE YEAR(exit_date) = {selectedYear} AND MONTH(exit_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//            ? Convert.ToDateTime(dt.Rows[0][0])
//            : DateTime.MinValue;
//    }

//    private void DataUpdateTimer_Tick(object sender, EventArgs e)
//    {
//        DateTime currentUpdateTime = GetLastUpdateTime();
//        if (currentUpdateTime > lastUpdateTime)
//        {
//            lastUpdateTime = currentUpdateTime;
//            UpdateReport();
//        }
//    }

//    // Cập nhật báo cáo
//    private void UpdateReport()
//    {
//        lblTotalRevenue.Text = CalculateTotalRevenue().ToString("N0") + " VNĐ";
//        lblProfit.Text = CalculateProfit().ToString("N0") + " VNĐ";
//        lblProfitMargin.Text = CalculateProfitMargin().ToString("F2") + " %";
//        DisplayRevenueByCategoryChart();
//        DisplayProductRatioChart();
//    }

//    // Tính tổng doanh thu
//    private decimal CalculateTotalRevenue()
//    {
//        string query = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalRevenue
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//            ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//            : 0;
//    }

//    // Tính lợi nhuận
//    private decimal CalculateProfit()
//    {
//        decimal revenue = CalculateTotalRevenue();
//        string query = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalCost
//            FROM stock_entry_details sed
//            JOIN stock_entry_headers seh ON sed.stock_entry_id = seh.id
//            WHERE YEAR(seh.entry_date) = {selectedYear} AND MONTH(seh.entry_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        decimal cost = dt.Rows.Count > 0 && dt.Rows[0]["TotalCost"] != DBNull.Value
//            ? Convert.ToDecimal(dt.Rows[0]["TotalCost"])
//            : 0;
//        return revenue - cost;
//    }

//    // Tính tỷ lệ lợi nhuận
//    private decimal CalculateProfitMargin()
//    {
//        decimal revenue = CalculateTotalRevenue();
//        decimal profit = CalculateProfit();
//        return revenue > 0 ? (profit / revenue) * 100 : 0;
//    }

//    // Biểu đồ cột: Doanh Thu Theo Danh Mục
//    private class CategoryRevenue
//    {
//        public string CategoryName { get; set; }
//        public decimal Revenue { get; set; }
//    }

//    private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//    {
//        string top5Query = $@"
//            SELECT TOP 5 c.category_name, SUM(sed.quantity * sed.unit_price) AS CategoryRevenue
//            FROM stock_exit_headers seh
//            JOIN stock_exit_details sed ON seh.id = sed.stock_exit_id
//            JOIN products p ON sed.product_id = p.id
//            JOIN categories c ON p.category_id = c.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}
//            GROUP BY c.category_name
//            ORDER BY CategoryRevenue DESC";
//        DataTable top5Dt = DatabaseHelper.ExecuteQuery(top5Query);

//        List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//        foreach (DataRow row in top5Dt.Rows)
//        {
//            categoryList.Add(new CategoryRevenue
//            {
//                CategoryName = row["category_name"].ToString(),
//                Revenue = Convert.ToDecimal(row["CategoryRevenue"])
//            });
//        }

//        string totalQuery = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalRevenue
//            FROM stock_exit_headers seh
//            JOIN stock_exit_details sed ON seh.id = sed.stock_exit_id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable totalDt = DatabaseHelper.ExecuteQuery(totalQuery);
//        decimal totalRevenue = totalDt.Rows.Count > 0 && totalDt.Rows[0]["TotalRevenue"] != DBNull.Value
//            ? Convert.ToDecimal(totalDt.Rows[0]["TotalRevenue"])
//            : 0;

//        decimal top5Revenue = categoryList.Sum(c => c.Revenue);
//        decimal othersRevenue = totalRevenue - top5Revenue;
//        if (othersRevenue > 0)
//        {
//            categoryList.Add(new CategoryRevenue
//            {
//                CategoryName = "Khác",
//                Revenue = othersRevenue
//            });
//        }

//        return categoryList;
//    }

//    private void DisplayRevenueByCategoryChart()
//    {
//        List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//        if (categoryList.Count == 0)
//        {
//            chartRevenueByCategory.Series.Clear();
//            return;
//        }

//        chartRevenueByCategory.Series.Clear();
//        Series series = new Series("Doanh Thu") { ChartType = SeriesChartType.Column, Color = Color.DodgerBlue };

//        decimal maxRevenue = 0;
//        foreach (var category in categoryList)
//        {
//            decimal revenueInMillions = category.Revenue / 1000000;
//            series.Points.AddXY(category.CategoryName, revenueInMillions);
//            if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;
//        }

//        chartRevenueByCategory.Series.Add(series);
//        chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Danh Mục";
//        chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Doanh Thu (Triệu VNĐ)";
//        chartRevenueByCategory.ChartAreas[0].AxisY.Interval = 10;
//        chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//        chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = Math.Ceiling(maxRevenue / 10) * 10;
//        chartRevenueByCategory.Titles.Clear();
//        chartRevenueByCategory.Titles.Add("Doanh Thu Theo Danh Mục");
//        chartRevenueByCategory.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
//        series.IsValueShownAsLabel = true;
//        series.LabelFormat = "#,##0.##M";
//    }

//    // Biểu đồ tròn: Tỉ Lệ Sản Phẩm Bán Ra
//    private class ProductSales
//    {
//        public string ProductName { get; set; }
//        public decimal Quantity { get; set; }
//    }

//    private List<ProductSales> GetTop5ProductsAndOthers()
//    {
//        string top5Query = $@"
//            SELECT TOP 5 p.name AS ProductName, SUM(sed.quantity) AS TotalQuantity
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            JOIN products p ON sed.product_id = p.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}
//            GROUP BY p.id, p.name
//            ORDER BY SUM(sed.quantity) DESC";
//        DataTable top5Dt = DatabaseHelper.ExecuteQuery(top5Query);

//        List<ProductSales> productSalesList = new List<ProductSales>();
//        foreach (DataRow row in top5Dt.Rows)
//        {
//            productSalesList.Add(new ProductSales
//            {
//                ProductName = row["ProductName"].ToString(),
//                Quantity = Convert.ToDecimal(row["TotalQuantity"])
//            });
//        }

//        string totalQuery = $@"
//            SELECT SUM(sed.quantity) AS TotalQuantity
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable totalDt = DatabaseHelper.ExecuteQuery(top5Query);
//        decimal totalQuantity = totalDt.Rows.Count > 0 && totalDt.Rows[0]["TotalQuantity"] != DBNull.Value
//            ? Convert.ToDecimal(totalDt.Rows[0]["TotalQuantity"])
//            : 0;

//        decimal top5Quantity = productSalesList.Sum(p => p.Quantity);
//        decimal othersQuantity = totalQuantity - top5Quantity;
//        if (othersQuantity > 0)
//        {
//            productSalesList.Add(new ProductSales
//            {
//                ProductName = "Khác",
//                Quantity = othersQuantity
//            });
//        }

//        return productSalesList;
//    }

//    private void DisplayProductRatioChart()
//    {
//        List<ProductSales> productSalesList = GetTop5ProductsAndOthers();
//        if (productSalesList.Count == 0)
//        {
//            chartProductRatio.Series.Clear();
//            return;
//        }

//        chartProductRatio.Series.Clear();
//        Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//        decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//        if (totalQuantity == 0) return;

//        Color[] colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Gray };
//        int colorIndex = 0;

//        foreach (var product in productSalesList.OrderByDescending(p => p.Quantity))
//        {
//            decimal percentage = (product.Quantity / totalQuantity) * 100;
//            DataPoint point = new DataPoint();
//            point.SetValueXY(product.ProductName, percentage);
//            point.Label = $"{product.ProductName} ({percentage:F2}%)";
//            point.LegendText = $"{product.ProductName} ({percentage:F2}%)";
//            point.Color = colors[colorIndex];
//            series.Points.Add(point);
//            colorIndex = Math.Min(colorIndex + 1, colors.Length - 1);
//        }

//        chartProductRatio.Series.Add(series);
//        chartProductRatio.Titles.Clear();
//        chartProductRatio.Titles.Add("Tỉ Lệ Sản Phẩm Bán Ra");
//        chartProductRatio.Titles[0].Font = new Font("Segoe UI", 12F, FontStyle.Bold);
//        series.LabelForeColor = Color.Black;
//        series.Font = new Font("Segoe UI", 10F);
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using WareHouse.DataAccess;
//using WareHouse.Presentation.Forms;

//public class ReportForm : BaseForm
//{
//    // Container chính chứa nội dung
//    private TableLayoutPanel tblMainContainer;
//    // Container cho thẻ tóm tắt
//    private FlowLayoutPanel flpSummaryCards;
//    // Container cho biểu đồ
//    private TableLayoutPanel tblCharts;
//    // Thẻ tóm tắt
//    private Label lblTotalRevenue;
//    private Label lblProfit;
//    private Label lblProfitRatio;
//    // Biểu đồ
//    private Chart chartRevenueByCategory;
//    private Chart chartProductRatio;
//    // Bộ lọc tháng
//    private ComboBox cmbMonthFilter;
//    private int selectedYear;
//    private int selectedMonth;
//    // Timer để cập nhật tự động
//    private System.Windows.Forms.Timer dataUpdateTimer;
//    private DateTime lastUpdateTime;
//    // Tiêu đề trang
//    private Label lblTitle;

//    public ReportForm()
//    {
//        InitializeComponents();
//        InitializeTimer();
//    }

//    private void InitializeComponents()
//    {
//        // Cài đặt form
//        this.Text = "Báo Cáo";
//        this.Size = new Size(1200, 800);
//        this.FormBorderStyle = FormBorderStyle.Sizable;
//        this.MaximizeBox = true;

//        // Container chính
//        tblMainContainer = new TableLayoutPanel
//        {
//            Dock = DockStyle.Fill,
//            RowCount = 4,
//            ColumnCount = 1,
//            Padding = new Padding(20),
//            AutoSize = true,
//            BackColor = Color.WhiteSmoke
//        };
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Tiêu đề
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Bộ lọc
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Thẻ tóm tắt
//        tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Biểu đồ
//        this.Controls.Add(tblMainContainer);

//        // Tiêu đề trang
//        lblTitle = new Label
//        {
//            Text = "Báo Cáo Thống Kê",
//            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//            ForeColor = Color.FromArgb(64, 64, 64),
//            AutoSize = true,
//            Margin = new Padding(0, 10, 0, 20),
//            Dock = DockStyle.Fill,
//            TextAlign = ContentAlignment.MiddleLeft
//        };
//        tblMainContainer.Controls.Add(lblTitle, 0, 0);

//        // Bộ lọc tháng (ComboBox)
//        Panel pnlFilter = new Panel
//        {
//            AutoSize = true,
//            Margin = new Padding(0, 0, 0, 15)
//        };

//        cmbMonthFilter = new ComboBox
//        {
//            DropDownStyle = ComboBoxStyle.DropDownList,
//            Location = new Point(0, 0),
//            Width = 150,
//            Height = 30,
//            Font = new Font("Segoe UI", 10F),
//            BackColor = Color.White
//        };

//        for (int month = 1; month <= 12; month++)
//        {
//            cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//        }
//        cmbMonthFilter.SelectedIndex = 2; // Mặc định chọn Tháng 3/2025
//        selectedYear = 2025;
//        selectedMonth = 3;
//        cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;

//        pnlFilter.Controls.Add(cmbMonthFilter);
//        tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//        // Thẻ tóm tắt (FlowLayoutPanel)
//        flpSummaryCards = new FlowLayoutPanel
//        {
//            AutoSize = true,
//            FlowDirection = FlowDirection.LeftToRight,
//            WrapContents = false,
//            Padding = new Padding(0, 0, 0, 20),
//            Margin = new Padding(0)
//        };

//        // Thẻ Tổng Doanh Thu
//        Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//        lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardRevenue);

//        // Thẻ Lợi Nhuận
//        Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//        lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardProfit);

//        // Thẻ Tỷ Lệ Doanh Thu/Chi Phí
//        Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ doanh thu / chi phí", "0", Color.FromArgb(230, 126, 34));
//        lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//        flpSummaryCards.Controls.Add(cardProfitRatio);

//        tblMainContainer.Controls.Add(flpSummaryCards, 0, 2);

//        // Container cho biểu đồ
//        tblCharts = new TableLayoutPanel
//        {
//            Dock = DockStyle.Fill,
//            RowCount = 1,
//            ColumnCount = 2,
//            Padding = new Padding(0),
//            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
//            BackColor = Color.White
//        };
//        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//        tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//        tblMainContainer.Controls.Add(tblCharts, 0, 3);

//        // Biểu đồ cột: Doanh Thu Theo Danh Mục
//        Panel pnlColumnChart = new Panel
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White,
//            Padding = new Padding(10)
//        };

//        Label lblColumnChartTitle = new Label
//        {
//            Text = "Doanh Thu Theo Danh Mục",
//            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(10, 10)
//        };
//        pnlColumnChart.Controls.Add(lblColumnChartTitle);

//        chartRevenueByCategory = new Chart
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White,
//            Padding = new Padding(10, 40, 10, 10)
//        };

//        ChartArea columnChartArea = new ChartArea();
//        columnChartArea.AxisX.MajorGrid.Enabled = false;
//        columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//        chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//        pnlColumnChart.Controls.Add(chartRevenueByCategory);
//        tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//        // Biểu đồ tròn: Tỉ Lệ Sản Phẩm Bán Ra
//        Panel pnlPieChart = new Panel
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White,
//            Padding = new Padding(10)
//        };

//        Label lblPieChartTitle = new Label
//        {
//            Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(10, 10)
//        };
//        pnlPieChart.Controls.Add(lblPieChartTitle);

//        chartProductRatio = new Chart
//        {
//            Dock = DockStyle.Fill,
//            BackColor = Color.White,
//            Padding = new Padding(10, 40, 10, 10)
//        };

//        ChartArea pieChartArea = new ChartArea();
//        chartProductRatio.ChartAreas.Add(pieChartArea);
//        pnlPieChart.Controls.Add(chartProductRatio);
//        tblCharts.Controls.Add(pnlPieChart, 1, 0);

//        // Khởi tạo dữ liệu ban đầu
//        UpdateReport();
//    }

//    // Tạo thẻ tóm tắt
//    private Panel CreateSummaryCard(string title, string value, Color accentColor)
//    {
//        Panel card = new Panel
//        {
//            Size = new Size(300, 100),
//            BackColor = Color.White,
//            Margin = new Padding(0, 0, 15, 0)
//        };

//        Panel headerBar = new Panel
//        {
//            Size = new Size(300, 5),
//            BackColor = accentColor,
//            Dock = DockStyle.Top
//        };

//        Label lblTitle = new Label
//        {
//            Text = title,
//            Font = new Font("Segoe UI", 10F),
//            ForeColor = Color.Gray,
//            AutoSize = true,
//            Location = new Point(15, 20)
//        };

//        Label lblValue = new Label
//        {
//            Name = "lblValue",
//            Text = value,
//            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//            AutoSize = true,
//            Location = new Point(15, 45),
//            ForeColor = accentColor
//        };

//        card.Controls.Add(headerBar);
//        card.Controls.Add(lblTitle);
//        card.Controls.Add(lblValue);

//        return card;
//    }

//    // Bộ lọc tháng
//    private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//    {
//        string selected = cmbMonthFilter.SelectedItem.ToString();
//        selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//        selectedYear = int.Parse(selected.Split('/')[1]);
//        lastUpdateTime = DateTime.MinValue; // Reset để buộc cập nhật
//        UpdateReport();
//    }

//    // Timer để cập nhật tự động
//    private void InitializeTimer()
//    {
//        dataUpdateTimer = new System.Windows.Forms.Timer
//        {
//            Interval = 10000 // 10 giây
//        };
//        dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//        dataUpdateTimer.Start();

//        lastUpdateTime = GetLastUpdateTime();
//    }

//    private DateTime GetLastUpdateTime()
//    {
//        string query = $@"
//            SELECT MAX(updated_at)
//            FROM stock_exit_headers
//            WHERE YEAR(exit_date) = {selectedYear} AND MONTH(exit_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//            ? Convert.ToDateTime(dt.Rows[0][0])
//            : DateTime.MinValue;
//    }

//    private void DataUpdateTimer_Tick(object sender, EventArgs e)
//    {
//        DateTime currentUpdateTime = GetLastUpdateTime();
//        if (currentUpdateTime > lastUpdateTime)
//        {
//            lastUpdateTime = currentUpdateTime;
//            UpdateReport();
//        }
//    }

//    // Cập nhật báo cáo
//    private void UpdateReport()
//    {
//        decimal totalRevenue = CalculateTotalRevenue();
//        decimal totalCost = CalculateTotalCost();
//        decimal profit = totalRevenue - totalCost;
//        decimal profitRatio = totalCost > 0 ? totalRevenue / totalCost : 0;

//        // Cập nhật thẻ tóm tắt
//        lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//        lblProfit.Text = FormatCurrency(profit);
//        lblProfitRatio.Text = FormatRatio(profitRatio);

//        // Cập nhật biểu đồ
//        DisplayRevenueByCategoryChart();
//        DisplayProductRatioChart();
//    }

//    // Định dạng tiền tệ
//    private string FormatCurrency(decimal value)
//    {
//        return String.Format("{0:n0} VND", value);
//    }

//    // Định dạng tỷ lệ
//    private string FormatRatio(decimal value)
//    {
//        return String.Format("{0:0.00}", value);
//    }

//    // Tính tổng doanh thu
//    private decimal CalculateTotalRevenue()
//    {
//        string query = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalRevenue
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//            ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//            : 0;
//    }

//    // Tính tổng chi phí
//    private decimal CalculateTotalCost()
//    {
//        string query = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalCost
//            FROM stock_entry_details sed
//            JOIN stock_entry_headers seh ON sed.stock_entry_id = seh.id
//            WHERE YEAR(seh.entry_date) = {selectedYear} AND MONTH(seh.entry_date) = {selectedMonth}";
//        DataTable dt = DatabaseHelper.ExecuteQuery(query);
//        return dt.Rows.Count > 0 && dt.Rows[0]["TotalCost"] != DBNull.Value
//            ? Convert.ToDecimal(dt.Rows[0]["TotalCost"])
//            : 0;
//    }

//    // Lớp chứa thông tin doanh thu theo danh mục
//    private class CategoryRevenue
//    {
//        public string CategoryName { get; set; }
//        public decimal Revenue { get; set; }
//    }

//    // Lấy top 5 danh mục có doanh thu cao nhất và các danh mục còn lại
//    private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//    {
//        string top5Query = $@"
//            SELECT TOP 5 c.category_name, SUM(sed.quantity * sed.unit_price) AS CategoryRevenue
//            FROM stock_exit_headers seh
//            JOIN stock_exit_details sed ON seh.id = sed.stock_exit_id
//            JOIN products p ON sed.product_id = p.id
//            JOIN categories c ON p.category_id = c.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}
//            GROUP BY c.category_name
//            ORDER BY CategoryRevenue DESC";
//        DataTable top5Dt = DatabaseHelper.ExecuteQuery(top5Query);

//        List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//        foreach (DataRow row in top5Dt.Rows)
//        {
//            categoryList.Add(new CategoryRevenue
//            {
//                CategoryName = row["category_name"].ToString(),
//                Revenue = Convert.ToDecimal(row["CategoryRevenue"])
//            });
//        }

//        string totalQuery = $@"
//            SELECT SUM(sed.quantity * sed.unit_price) AS TotalRevenue
//            FROM stock_exit_headers seh
//            JOIN stock_exit_details sed ON seh.id = sed.stock_exit_id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable totalDt = DatabaseHelper.ExecuteQuery(totalQuery);
//        decimal totalRevenue = totalDt.Rows.Count > 0 && totalDt.Rows[0]["TotalRevenue"] != DBNull.Value
//            ? Convert.ToDecimal(totalDt.Rows[0]["TotalRevenue"])
//            : 0;

//        decimal top5Revenue = categoryList.Sum(c => c.Revenue);
//        decimal othersRevenue = totalRevenue - top5Revenue;
//        if (othersRevenue > 0)
//        {
//            categoryList.Add(new CategoryRevenue
//            {
//                CategoryName = "Khác",
//                Revenue = othersRevenue
//            });
//        }

//        return categoryList;
//    }

//    // Hiển thị biểu đồ doanh thu theo danh mục
//    private void DisplayRevenueByCategoryChart()
//    {
//        List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//        if (categoryList.Count == 0)
//        {
//            chartRevenueByCategory.Series.Clear();
//            return;
//        }

//        chartRevenueByCategory.Series.Clear();
//        Series series = new Series("Doanh Thu") { ChartType = SeriesChartType.Column, Color = Color.FromArgb(52, 152, 219) };

//        decimal maxRevenue = 0;
//        foreach (var category in categoryList)
//        {
//            decimal revenueInMillions = category.Revenue / 1000000;
//            series.Points.AddXY(category.CategoryName, revenueInMillions);
//            if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;
//        }

//        chartRevenueByCategory.Series.Add(series);

//        // Cài đặt trục X và Y
//        chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//        chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//        chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "0M";
//        chartRevenueByCategory.ChartAreas[0].AxisY.Interval = 10;
//        chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//        chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = (double)(decimal.Ceiling(maxRevenue / 10) * 10);

//        // Hiển thị giá trị trên mỗi cột
//        series.IsValueShownAsLabel = true;
//        series.LabelFormat = "#M";
//        series.Font = new Font("Segoe UI", 9F);
//    }

//    // Lớp chứa thông tin về tỷ lệ sản phẩm bán ra
//    private class ProductSales
//    {
//        public string ProductName { get; set; }
//        public decimal Quantity { get; set; }
//    }

//    // Lấy top 5 sản phẩm bán chạy nhất và các sản phẩm còn lại
//    private List<ProductSales> GetTop5ProductsAndOthers()
//    {
//        string top5Query = $@"
//            SELECT TOP 5 p.name AS ProductName, SUM(sed.quantity) AS TotalQuantity
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            JOIN products p ON sed.product_id = p.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}
//            GROUP BY p.id, p.name
//            ORDER BY SUM(sed.quantity) DESC";
//        DataTable top5Dt = DatabaseHelper.ExecuteQuery(top5Query);

//        List<ProductSales> productSalesList = new List<ProductSales>();
//        foreach (DataRow row in top5Dt.Rows)
//        {
//            productSalesList.Add(new ProductSales
//            {
//                ProductName = row["ProductName"].ToString(),
//                Quantity = Convert.ToDecimal(row["TotalQuantity"])
//            });
//        }

//        string totalQuery = $@"
//            SELECT SUM(sed.quantity) AS TotalQuantity
//            FROM stock_exit_details sed
//            JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            WHERE YEAR(seh.exit_date) = {selectedYear} AND MONTH(seh.exit_date) = {selectedMonth}";
//        DataTable totalDt = DatabaseHelper.ExecuteQuery(totalQuery);
//        decimal totalQuantity = totalDt.Rows.Count > 0 && totalDt.Rows[0]["TotalQuantity"] != DBNull.Value
//            ? Convert.ToDecimal(totalDt.Rows[0]["TotalQuantity"])
//            : 0;

//        decimal top5Quantity = productSalesList.Sum(p => p.Quantity);
//        decimal othersQuantity = totalQuantity - top5Quantity;
//        if (othersQuantity > 0)
//        {
//            productSalesList.Add(new ProductSales
//            {
//                ProductName = "Khác",
//                Quantity = othersQuantity
//            });
//        }

//        return productSalesList;
//    }

//    // Hiển thị biểu đồ tỷ lệ sản phẩm bán ra
//    private void DisplayProductRatioChart()
//    {
//        List<ProductSales> productSalesList = GetTop5ProductsAndOthers();
//        if (productSalesList.Count == 0)
//        {
//            chartProductRatio.Series.Clear();
//            return;
//        }

//        chartProductRatio.Series.Clear();
//        Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//        decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//        if (totalQuantity == 0) return;

//        // Sử dụng các màu phù hợp hơn với giao diện mẫu
//        Color[] colors = new Color[] {
//            Color.FromArgb(41, 128, 185),     // Xanh dương
//            Color.FromArgb(231, 76, 60),      // Đỏ
//            Color.FromArgb(46, 204, 113),     // Xanh lá
//            Color.FromArgb(243, 156, 18),     // Vàng
//            Color.FromArgb(142, 68, 173),     // Tím
//            Color.FromArgb(230, 126, 34)      // Cam
//        };
//        int colorIndex = 0;

//        foreach (var product in productSalesList.OrderByDescending(p => p.Quantity))
//        {
//            decimal percentage = (product.Quantity / totalQuantity) * 100;
//            DataPoint point = new DataPoint();
//            point.SetValueXY(product.ProductName, percentage);
//            point.LegendText = $"{product.ProductName} ({percentage:F0}%)";
//            point.Color = colors[colorIndex % colors.Length];
//            point.Font = new Font("Segoe UI", 9F);
//            point.LabelForeColor = Color.White;
//            series.Points.Add(point);
//            colorIndex++;
//        }

//        chartProductRatio.Series.Add(series);

//        // Cài đặt legend (chú thích)
//        chartProductRatio.Legends.Add(new Legend("ProductLegend"));
//        chartProductRatio.Legends["ProductLegend"].Docking = Docking.Bottom;
//        chartProductRatio.Legends["ProductLegend"].Alignment = StringAlignment.Center;
//        chartProductRatio.Legends["ProductLegend"].Font = new Font("Segoe UI", 9F);

//        // Cài đặt biểu đồ tròn
//        series.Label = "#PERCENT{0}%";
//        series.LabelFormat = "P0";
//        chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//    }
//}


// Code chính gần nhất mới là backup

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ReportForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private FlowLayoutPanel flpSummaryCards;
//        private TableLayoutPanel tblCharts;
//        private Label lblTotalRevenue;
//        private Label lblProfit;
//        private Label lblProfitRatio;
//        private ComboBox cmbMonthFilter;
//        private ComboBox cmbCategoryFilter;
//        private int selectedYear;
//        private int selectedMonth;
//        private int selectedCategoryId;
//        private System.Windows.Forms.Timer dataUpdateTimer;
//        private DateTime lastUpdateTime;
//        private Label lblTitle;
//        private Chart chartRevenueByCategory;
//        private Chart chartProductRatio;
//        private bool hasAccessPermission = false;
//        private List<KeyValuePair<int, string>> categories;

//        public ReportForm() : base()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        public ReportForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                LoadCategories();
//                InitializeComponents();
//                InitializeTimer();
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void LoadCategories()
//        {
//            categories = new List<KeyValuePair<int, string>>();
//            string query = "SELECT id, name FROM categories ORDER BY name";
//            DataTable dt = DatabaseHelper.ExecuteQuery(query);

//            categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//            foreach (DataRow row in dt.Rows)
//            {
//                int id = Convert.ToInt32(row["id"]);
//                string name = row["name"].ToString();
//                categories.Add(new KeyValuePair<int, string>(id, name));
//            }
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1 || RoleId == 3);
//        }

//        private void ShowAccessDeniedMessage()
//        {
//            this.Controls.Clear();

//            Label lblAccessDenied = new Label
//            {
//                Text = "Không có quyền truy cập!",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                AutoSize = true,
//                TextAlign = ContentAlignment.MiddleCenter
//            };

//            Panel pnlAccessDenied = new Panel
//            {
//                Dock = DockStyle.Fill
//            };

//            lblAccessDenied.Location = new Point(
//                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//            );

//            pnlAccessDenied.Controls.Add(lblAccessDenied);
//            this.Controls.Add(pnlAccessDenied);

//            this.Resize += (sender, e) =>
//            {
//                lblAccessDenied.Location = new Point(
//                    (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                    (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//                );
//            };
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "Báo Cáo";
//            this.Size = new Size(1200, 800);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;

//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 4,
//                ColumnCount = 1,
//                Padding = new Padding(260, 10, 10, 10),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header
//            Panel pnlHeader = new Panel
//            {
//                BackColor = Color.FromArgb(52, 152, 219),
//                Dock = DockStyle.Top,
//                Height = 60
//            };

//            lblTitle = new Label
//            {
//                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 15),
//                TextAlign = ContentAlignment.MiddleLeft
//            };
//            pnlHeader.Controls.Add(lblTitle);
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Filter Panel
//            Panel pnlFilter = new Panel
//            {
//                BackColor = Color.White,
//                Margin = new Padding(0, 10, 0, 10),
//                Padding = new Padding(15),
//                Height = 60,
//                Dock = DockStyle.Fill
//            };

//            FlowLayoutPanel flpFilter = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                FlowDirection = FlowDirection.LeftToRight,
//                Dock = DockStyle.Fill
//            };

//            Label lblMonthFilter = new Label
//            {
//                Text = "Tháng:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(0, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblMonthFilter);

//            cmbMonthFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 150,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            for (int month = 1; month <= 12; month++)
//            {
//                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//            }
//            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
//            selectedYear = 2025;
//            selectedMonth = DateTime.Now.Month;
//            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbMonthFilter);

//            Label lblCategoryFilter = new Label
//            {
//                Text = "Danh mục:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblCategoryFilter);

//            cmbCategoryFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 0, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            foreach (var category in categories)
//            {
//                cmbCategoryFilter.Items.Add(category.Value);
//            }
//            cmbCategoryFilter.SelectedIndex = 0;
//            selectedCategoryId = 0;
//            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbCategoryFilter);

//            Button btnExport = new Button
//            {
//                Text = "Xuất báo cáo",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(120, 30),
//                Margin = new Padding(40, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnExport.FlatAppearance.BorderSize = 0;
//            btnExport.Click += BtnExport_Click;
//            flpFilter.Controls.Add(btnExport);

//            pnlFilter.Controls.Add(flpFilter);
//            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//            // Summary Cards
//            flpSummaryCards = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = false,
//                Margin = new Padding(0, 0, 0, 20),
//                Anchor = AnchorStyles.Top
//            };

//            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardRevenue);

//            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfit);

//            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ doanh thu / chi phí", "0", Color.FromArgb(230, 126, 34));
//            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfitRatio);

//            tblMainContainer.Controls.Add(flpSummaryCards, 0, 2);

//            // Charts Container
//            tblCharts = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 2,
//                Margin = new Padding(0),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//            tblMainContainer.Controls.Add(tblCharts, 0, 3);

//            // Column Chart Panel
//            Panel pnlColumnChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 5, 0),
//                Padding = new Padding(15)
//            };

//            Label lblColumnChartTitle = new Label
//            {
//                Text = "Doanh Thu Theo Danh Mục",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlColumnChart.Controls.Add(lblColumnChartTitle);

//            chartRevenueByCategory = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea columnChartArea = new ChartArea();
//            columnChartArea.AxisX.MajorGrid.Enabled = false;
//            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.BackColor = Color.White;
//            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//            pnlColumnChart.Controls.Add(chartRevenueByCategory);

//            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//            // Pie Chart Panel
//            Panel pnlPieChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(5, 0, 0, 0),
//                Padding = new Padding(15)
//            };

//            Label lblPieChartTitle = new Label
//            {
//                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlPieChart.Controls.Add(lblPieChartTitle);

//            chartProductRatio = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea pieChartArea = new ChartArea();
//            pieChartArea.BackColor = Color.White;
//            chartProductRatio.ChartAreas.Add(pieChartArea);
//            pnlPieChart.Controls.Add(chartProductRatio);

//            tblCharts.Controls.Add(pnlPieChart, 1, 0);

//            UpdateReport();
//        }

//        private Panel CreateSummaryCard(string title, string value, Color accentColor)
//        {
//            Panel card = new Panel
//            {
//                Width = 275,
//                Height = 100,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 10, 0),
//                Anchor = AnchorStyles.None
//            };

//            Panel headerBar = new Panel
//            {
//                Size = new Size(275, 4),
//                BackColor = accentColor,
//                Dock = DockStyle.Top
//            };

//            Label lblTitle = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 10F),
//                ForeColor = Color.FromArgb(127, 140, 141),
//                AutoSize = true,
//                Margin = new Padding(15, 15, 0, 0),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };

//            Label lblValue = new Label
//            {
//                Name = "lblValue",
//                Text = value,
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                AutoSize = true,
//                Margin = new Padding(15, 40, 0, 0),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };

//            card.Controls.Add(headerBar);
//            card.Controls.Add(lblTitle);
//            card.Controls.Add(lblValue);

//            return card;
//        }

//        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string selected = cmbMonthFilter.SelectedItem.ToString();
//            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//            selectedYear = int.Parse(selected.Split('/')[1]);
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void UpdateTitle()
//        {
//            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
//            if (selectedCategoryId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
//            }
//            else
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
//            }
//        }

//        private void BtnExport_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void InitializeTimer()
//        {
//            dataUpdateTimer = new System.Windows.Forms.Timer
//            {
//                Interval = 10000
//            };
//            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//            dataUpdateTimer.Start();

//            lastUpdateTime = GetLastUpdateTime();
//        }

//        private DateTime GetLastUpdateTime()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                @"AND EXISTS (
//            SELECT 1 FROM stock_exit_details sed 
//            JOIN products p ON sed.product_id = p.id
//            WHERE sed.stock_exit_id = seh.id AND p.category_id = ?CategoryId
//        )" : "";

//            string query = @"
//        SELECT MAX(updated_at)
//        FROM stock_exit_headers seh
//        WHERE YEAR(updated_at) = ?Year 
//        AND MONTH(updated_at) = ?Month"
//                + categoryFilter;

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth)
//    };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//            return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//                ? Convert.ToDateTime(dt.Rows[0][0])
//                : DateTime.MinValue;
//        }

//        private void DataUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            DateTime currentUpdateTime = GetLastUpdateTime();
//            if (currentUpdateTime > lastUpdateTime)
//            {
//                lastUpdateTime = currentUpdateTime;
//                UpdateReport();
//            }
//        }

//        private void UpdateReport()
//        {
//            decimal totalRevenue = CalculateTotalRevenue();
//            decimal totalCost = CalculateTotalCost();
//            decimal profit = totalRevenue - totalCost;
//            decimal profitRatio = totalCost > 0 ? totalRevenue / totalCost : 0;

//            lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//            lblProfit.Text = FormatCurrency(profit);
//            lblProfitRatio.Text = FormatRatio(profitRatio);

//            DisplayRevenueByCategoryChart();
//            DisplayProductRatioChart();
//        }

//        private string FormatCurrency(decimal value)
//        {
//            return String.Format("{0:n0} VND", value);
//        }

//        private string FormatRatio(decimal value)
//        {
//            return String.Format("{0:0.00}", value);
//        }

//        private decimal CalculateTotalRevenue()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND p.category_id = ?CategoryId" : "";

//            string query = @"
//        SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS TotalRevenue
//        FROM stock_exit_details sed
//        JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//        JOIN products p ON sed.product_id = p.id
//        WHERE YEAR(seh.updated_at) = ?Year 
//        AND MONTH(seh.updated_at) = ?Month
//        " + categoryFilter;

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth)
//    };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//            return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//                ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//                : 0;
//        }

//        private decimal CalculateTotalCost()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND p.category_id = ?CategoryId" : "";

//            string query = @"
//        SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS TotalCost
//        FROM stock_entry_details sed
//        JOIN stock_entry_headers seh ON sed.stock_entry_id = seh.id
//        JOIN products p ON sed.product_id = p.id
//        WHERE YEAR(seh.updated_at) = ?Year 
//        AND MONTH(seh.updated_at) = ?Month
//        " + categoryFilter;

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth)
//    };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//            return dt.Rows.Count > 0 && dt.Rows[0]["TotalCost"] != DBNull.Value
//                ? Convert.ToDecimal(dt.Rows[0]["TotalCost"])
//                : 0;
//        }

//        private class CategoryRevenue
//        {
//            public string CategoryName { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND c.id = ?CategoryId" : "";

//            string query = @"
//        SELECT c.name, COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS CategoryRevenue
//        FROM categories c
//        LEFT JOIN products p ON p.category_id = c.id
//        LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//        LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            AND YEAR(seh.updated_at) = ?Year
//            AND MONTH(seh.updated_at) = ?Month
//        WHERE 1=1 " + categoryFilter + @"
//        GROUP BY c.id, c.name
//        HAVING CategoryRevenue > 0
//        ORDER BY CategoryRevenue DESC";

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth)
//    };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//            List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//            foreach (DataRow row in dt.Rows)
//            {
//                decimal revenue = row["CategoryRevenue"] != DBNull.Value ? Convert.ToDecimal(row["CategoryRevenue"]) : 0;
//                if (revenue > 0)
//                {
//                    categoryList.Add(new CategoryRevenue
//                    {
//                        CategoryName = row["name"].ToString(),
//                        Revenue = revenue
//                    });
//                }
//            }

//            if (categoryList.Count == 0)
//            {
//                return new List<CategoryRevenue>();
//            }

//            List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
//            decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

//            if (categoryList.Count > 5)
//            {
//                topCategories.Add(new CategoryRevenue
//                {
//                    CategoryName = "Khác",
//                    Revenue = othersRevenue
//                });
//            }

//            return topCategories;
//        }

//        private void DisplayRevenueByCategoryChart()
//        {
//            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();

//            if (categoryList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Column,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 0
//            };

//            decimal maxRevenue = 0;
//            foreach (var category in categoryList)
//            {
//                decimal revenueInMillions = category.Revenue / 1000000;
//                series.Points.AddXY(category.CategoryName, revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.BorderColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);

//            foreach (DataPoint point in series.Points)
//            {
//                point["PixelPointWidth"] = "40";
//            }
//        }

//        private class CategorySales
//        {
//            public string CategoryName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND c.id = ?CategoryId" : "";

//            string query = @"
//        SELECT c.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//        FROM categories c
//        LEFT JOIN products p ON p.category_id = c.id
//        LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//        LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//            AND YEAR(seh.updated_at) = ?Year
//            AND MONTH(seh.updated_at) = ?Month
//        WHERE 1=1 " + categoryFilter + @"
//        GROUP BY c.id, c.name
//        HAVING TotalQuantity > 0
//        ORDER BY TotalQuantity DESC";

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth)
//    };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//            List<CategorySales> categorySalesList = new List<CategorySales>();
//            foreach (DataRow row in dt.Rows)
//            {
//                decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                if (quantity > 0)
//                {
//                    categorySalesList.Add(new CategorySales
//                    {
//                        CategoryName = row["name"].ToString(),
//                        Quantity = quantity
//                    });
//                }
//            }

//            if (categorySalesList.Count == 0)
//            {
//                return new List<CategorySales>();
//            }

//            List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
//            decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

//            if (categorySalesList.Count > 5)
//            {
//                topCategories.Add(new CategorySales
//                {
//                    CategoryName = "Khác",
//                    Quantity = othersQuantity
//                });
//            }

//            return topCategories;
//        }

//        private void DisplayProductRatioChart()
//        {
//            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            if (categorySalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = categorySalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//        Color.FromArgb(41, 128, 185),
//        Color.FromArgb(231, 76, 60),
//        Color.FromArgb(46, 204, 113),
//        Color.FromArgb(155, 89, 182),
//        Color.FromArgb(243, 156, 18),
//        Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var category in categorySalesList)
//            {
//                double percentage = Math.Round((double)(category.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = category.CategoryName;
//                point.YValues = new double[] { Convert.ToDouble(category.Quantity) };
//                point.LegendText = $"{category.CategoryName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ReportForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private FlowLayoutPanel flpSummaryCards;
//        private TableLayoutPanel tblCharts;
//        private Label lblTotalRevenue;
//        private Label lblProfit;
//        private Label lblProfitRatio;
//        private ComboBox cmbMonthFilter;
//        private ComboBox cmbCategoryFilter;
//        private int selectedYear;
//        private int selectedMonth;
//        private int selectedCategoryId;
//        private System.Windows.Forms.Timer dataUpdateTimer;
//        private DateTime lastUpdateTime;
//        private Label lblTitle;
//        private Chart chartRevenueByCategory;
//        private Chart chartProductRatio;
//        private bool hasAccessPermission = false;
//        private List<KeyValuePair<int, string>> categories;

//        public ReportForm() : base()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        public ReportForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                LoadCategories();
//                InitializeComponents();
//                InitializeTimer();
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void LoadCategories()
//        {
//            categories = new List<KeyValuePair<int, string>>();
//            string query = "SELECT id, name FROM categories ORDER BY name";
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//                foreach (DataRow row in dt.Rows)
//                {
//                    int id = Convert.ToInt32(row["id"]);
//                    string name = row["name"].ToString();
//                    categories.Add(new KeyValuePair<int, string>(id, name));
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục")); // Fallback to avoid breaking the app
//            }
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1 || RoleId == 3);
//        }

//        private void ShowAccessDeniedMessage()
//        {
//            this.Controls.Clear();

//            Label lblAccessDenied = new Label
//            {
//                Text = "Không có quyền truy cập!",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                AutoSize = true,
//                TextAlign = ContentAlignment.MiddleCenter
//            };

//            Panel pnlAccessDenied = new Panel
//            {
//                Dock = DockStyle.Fill
//            };

//            lblAccessDenied.Location = new Point(
//                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//            );

//            pnlAccessDenied.Controls.Add(lblAccessDenied);
//            this.Controls.Add(pnlAccessDenied);

//            this.Resize += (sender, e) =>
//            {
//                lblAccessDenied.Location = new Point(
//                    (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                    (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//                );
//            };
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "Báo Cáo";
//            this.Size = new Size(1200, 800);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;

//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 4,
//                ColumnCount = 1,
//                Padding = new Padding(260, 10, 10, 10),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header
//            Panel pnlHeader = new Panel
//            {
//                BackColor = Color.FromArgb(52, 152, 219),
//                Dock = DockStyle.Top,
//                Height = 60
//            };

//            lblTitle = new Label
//            {
//                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 15),
//                TextAlign = ContentAlignment.MiddleLeft
//            };
//            pnlHeader.Controls.Add(lblTitle);
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Filter Panel
//            Panel pnlFilter = new Panel
//            {
//                BackColor = Color.White,
//                Margin = new Padding(0, 10, 0, 10),
//                Padding = new Padding(15),
//                Height = 60,
//                Dock = DockStyle.Fill
//            };

//            FlowLayoutPanel flpFilter = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                FlowDirection = FlowDirection.LeftToRight,
//                Dock = DockStyle.Fill
//            };

//            Label lblMonthFilter = new Label
//            {
//                Text = "Tháng:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(0, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblMonthFilter);

//            cmbMonthFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 150,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            for (int month = 1; month <= 12; month++)
//            {
//                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//            }
//            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
//            selectedYear = 2025;
//            selectedMonth = DateTime.Now.Month;
//            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbMonthFilter);

//            Label lblCategoryFilter = new Label
//            {
//                Text = "Danh mục:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblCategoryFilter);

//            cmbCategoryFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 0, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            foreach (var category in categories)
//            {
//                cmbCategoryFilter.Items.Add(category.Value);
//            }
//            cmbCategoryFilter.SelectedIndex = 0;
//            selectedCategoryId = 0;
//            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbCategoryFilter);

//            Button btnExport = new Button
//            {
//                Text = "Xuất báo cáo",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(120, 30),
//                Margin = new Padding(40, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnExport.FlatAppearance.BorderSize = 0;
//            btnExport.Click += BtnExport_Click;
//            flpFilter.Controls.Add(btnExport);

//            pnlFilter.Controls.Add(flpFilter);
//            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//            // Summary Cards
//            flpSummaryCards = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = false,
//                Margin = new Padding(0, 0, 0, 20),
//                Anchor = AnchorStyles.Top
//            };

//            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardRevenue);

//            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfit);

//            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ doanh thu / chi phí", "0", Color.FromArgb(230, 126, 34));
//            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfitRatio);

//            tblMainContainer.Controls.Add(flpSummaryCards, 0, 2);

//            // Charts Container
//            tblCharts = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 2,
//                Margin = new Padding(0),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//            tblMainContainer.Controls.Add(tblCharts, 0, 3);

//            // Column Chart Panel
//            Panel pnlColumnChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 5, 0),
//                Padding = new Padding(15)
//            };

//            Label lblColumnChartTitle = new Label
//            {
//                Text = "Doanh Thu Theo Danh Mục",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlColumnChart.Controls.Add(lblColumnChartTitle);

//            chartRevenueByCategory = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea columnChartArea = new ChartArea();
//            columnChartArea.AxisX.MajorGrid.Enabled = false;
//            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.BackColor = Color.White;
//            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//            pnlColumnChart.Controls.Add(chartRevenueByCategory);

//            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//            // Pie Chart Panel
//            Panel pnlPieChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(5, 0, 0, 0),
//                Padding = new Padding(15)
//            };

//            Label lblPieChartTitle = new Label
//            {
//                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlPieChart.Controls.Add(lblPieChartTitle);

//            chartProductRatio = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea pieChartArea = new ChartArea();
//            pieChartArea.BackColor = Color.White;
//            chartProductRatio.ChartAreas.Add(pieChartArea);
//            pnlPieChart.Controls.Add(chartProductRatio);

//            tblCharts.Controls.Add(pnlPieChart, 1, 0);

//            UpdateReport();
//        }

//        private Panel CreateSummaryCard(string title, string value, Color accentColor)
//        {
//            Panel card = new Panel
//            {
//                Width = 275,
//                Height = 100,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 10, 0),
//                Anchor = AnchorStyles.None
//            };

//            Panel headerBar = new Panel
//            {
//                Size = new Size(275, 4),
//                BackColor = accentColor,
//                Dock = DockStyle.Top
//            };

//            Label lblTitle = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 10F),
//                ForeColor = Color.FromArgb(127, 140, 141),
//                AutoSize = true,
//                Margin = new Padding(15, 15, 0, 0),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };

//            Label lblValue = new Label
//            {
//                Name = "lblValue",
//                Text = value,
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                AutoSize = true,
//                Margin = new Padding(15, 40, 0, 0),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };

//            card.Controls.Add(headerBar);
//            card.Controls.Add(lblTitle);
//            card.Controls.Add(lblValue);

//            return card;
//        }

//        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string selected = cmbMonthFilter.SelectedItem.ToString();
//            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//            selectedYear = int.Parse(selected.Split('/')[1]);
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void UpdateTitle()
//        {
//            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
//            if (selectedCategoryId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
//            }
//            else
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
//            }
//        }

//        private void BtnExport_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void InitializeTimer()
//        {
//            dataUpdateTimer = new System.Windows.Forms.Timer
//            {
//                Interval = 10000 // 10 seconds
//            };
//            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//            dataUpdateTimer.Start();

//            lastUpdateTime = GetLastUpdateTime();
//        }

//        private DateTime GetLastUpdateTime()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                @"AND EXISTS (
//                    SELECT 1 FROM stock_exit_details sed 
//                    JOIN products p ON sed.product_id = p.id
//                    WHERE sed.stock_exit_id = seh.id AND p.category_id = ?CategoryId
//                )" : "";

//            string query = @"
//                SELECT MAX(updated_at)
//                FROM stock_exit_headers seh
//                WHERE YEAR(updated_at) = ?Year 
//                AND MONTH(updated_at) = ?Month"
//                + categoryFilter;

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//                    ? Convert.ToDateTime(dt.Rows[0][0])
//                    : DateTime.MinValue;
//            }
//            catch (Exception ex)
//            {
//                // Log the error silently to avoid recurring popups
//                System.Diagnostics.Debug.WriteLine($"Error in GetLastUpdateTime: {ex.Message}");
//                return lastUpdateTime; // Return the last known update time to prevent triggering an update
//            }
//        }

//        private void DataUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            DateTime currentUpdateTime = GetLastUpdateTime();
//            if (currentUpdateTime > lastUpdateTime)
//            {
//                lastUpdateTime = currentUpdateTime;
//                UpdateReport();
//            }
//        }

//        private void UpdateReport()
//        {
//            bool hasData = HasData();
//            if (hasData)
//            {
//                decimal totalRevenue = CalculateTotalRevenue();
//                decimal totalCost = CalculateTotalCost();
//                decimal profit = totalRevenue - totalCost;
//                decimal profitRatio = totalCost > 0 ? totalRevenue / totalCost : 0;

//                lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//                lblProfit.Text = FormatCurrency(profit);
//                lblProfitRatio.Text = FormatRatio(profitRatio);
//            }
//            else
//            {
//                lblTotalRevenue.Text = "0 VND";
//                lblProfit.Text = "0 VND";
//                lblProfitRatio.Text = "0";
//            }

//            if (hasData)
//            {
//                if (selectedCategoryId == 0)
//                {
//                    DisplayRevenueByCategoryChart();
//                }
//                else
//                {
//                    DisplayWeeklyRevenueChart();
//                }

//                if (selectedCategoryId == 0)
//                {
//                    DisplayCategorySalesRatioChart();
//                }
//                else
//                {
//                    DisplayProductRatioChart();
//                }
//            }
//            else
//            {
//                DisplayNoDataCharts();
//            }
//        }

//        private bool HasData()
//        {
//            // First, check if there is any data for the selected month
//            string query = @"
//                SELECT COUNT(*) AS DataCount
//                FROM stock_exit_headers seh
//                WHERE YEAR(seh.updated_at) = ?Year 
//                AND MONTH(seh.updated_at) = ?Month";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                bool monthHasData = dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["DataCount"]) > 0;

//                if (!monthHasData)
//                {
//                    return false;
//                }

//                // If a specific category is selected, check if that category has data for the selected month
//                if (selectedCategoryId > 0)
//                {
//                    string categoryQuery = @"
//                        SELECT COUNT(*) AS DataCount
//                        FROM stock_exit_headers seh
//                        JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//                        JOIN products p ON sed.product_id = p.id
//                        WHERE YEAR(seh.updated_at) = ?Year 
//                        AND MONTH(seh.updated_at) = ?Month
//                        AND p.category_id = ?CategoryId";

//                    var categoryParameters = new List<MySqlParameter>
//                    {
//                        new MySqlParameter("?Year", selectedYear),
//                        new MySqlParameter("?Month", selectedMonth),
//                        new MySqlParameter("?CategoryId", selectedCategoryId)
//                    };

//                    DataTable categoryDt = DatabaseHelper.ExecuteQuery(categoryQuery, categoryParameters.ToArray());
//                    return categoryDt.Rows.Count > 0 && Convert.ToInt32(categoryDt.Rows[0]["DataCount"]) > 0;
//                }

//                return true;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi kiểm tra dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return false;
//            }
//        }

//        private void DisplayNoDataCharts()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartRevenueByCategory.Titles.Add(noDataTitle);

//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();
//            Title noDataTitlePie = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartProductRatio.Titles.Add(noDataTitlePie);
//        }

//        private string FormatCurrency(decimal value)
//        {
//            return String.Format("{0:n0} VND", value);
//        }

//        private string FormatRatio(decimal value)
//        {
//            return String.Format("{0:0.00}", value);
//        }

//        private decimal CalculateTotalRevenue()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND p.category_id = ?CategoryId" : "";

//            string query = @"
//                SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS TotalRevenue
//                FROM stock_exit_details sed
//                JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//                JOIN products p ON sed.product_id = p.id
//                WHERE YEAR(seh.updated_at) = ?Year 
//                AND MONTH(seh.updated_at) = ?Month"
//                + categoryFilter;

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//                    ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//                    : 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tính tổng doanh thu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }

//        private decimal CalculateTotalCost()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND p.category_id = ?CategoryId" : "";

//            string query = @"
//                SELECT COALESCE(SUM(sed.quantity * p.import_price), 0) AS TotalCost
//                FROM stock_exit_details sed
//                JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//                JOIN products p ON sed.product_id = p.id
//                WHERE YEAR(seh.updated_at) = ?Year 
//                AND MONTH(seh.updated_at) = ?Month"
//                + categoryFilter;

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalCost"] != DBNull.Value
//                    ? Convert.ToDecimal(dt.Rows[0]["TotalCost"])
//                    : 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tính tổng chi phí: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }

//        private class CategoryRevenue
//        {
//            public string CategoryName { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND c.id = ?CategoryId" : "";

//            string query = @"
//                SELECT c.name, COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS CategoryRevenue
//                FROM categories c
//                LEFT JOIN products p ON p.category_id = c.id
//                LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//                LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//                    AND YEAR(seh.updated_at) = ?Year
//                    AND MONTH(seh.updated_at) = ?Month
//                WHERE 1=1 " + categoryFilter + @"
//                GROUP BY c.id, c.name
//                HAVING CategoryRevenue > 0
//                ORDER BY CategoryRevenue DESC";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//                List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["CategoryRevenue"] != DBNull.Value ? Convert.ToDecimal(row["CategoryRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        categoryList.Add(new CategoryRevenue
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Revenue = revenue
//                        });
//                    }
//                }

//                if (categoryList.Count == 0)
//                {
//                    return new List<CategoryRevenue>();
//                }

//                List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
//                decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

//                if (categoryList.Count > 5)
//                {
//                    topCategories.Add(new CategoryRevenue
//                    {
//                        CategoryName = "Khác",
//                        Revenue = othersRevenue
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy doanh thu theo danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<CategoryRevenue>();
//            }
//        }

//        private void DisplayRevenueByCategoryChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();

//            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//            if (categoryList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Column,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 0
//            };

//            decimal maxRevenue = 0;
//            foreach (var category in categoryList)
//            {
//                decimal revenueInMillions = category.Revenue / 1000000;
//                series.Points.AddXY(category.CategoryName, revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.BorderColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);

//            foreach (DataPoint point in series.Points)
//            {
//                point["PixelPointWidth"] = "40";
//            }
//        }

//        private class WeeklyRevenue
//        {
//            public int WeekNumber { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        //private List<WeeklyRevenue> GetWeeklyRevenue()
//        //{
//        //    string query = @"
//        //        SELECT FLOOR((DAY(seh.updated_at) - 1) / 7) + 1 AS WeekNumber,
//        //               COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS WeeklyRevenue
//        //        FROM stock_exit_headers seh
//        //        JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//        //        JOIN products p ON sed.product_id = p.id
//        //        WHERE YEAR(seh.updated_at) = ?Year 
//        //        AND MONTH(seh.updated_at) = ?Month
//        //        AND p.category_id = ?CategoryId
//        //        GROUP BY FLOOR((DAY(seh.updated_at) - 1) / 7) + 1
//        //        ORDER BY WeekNumber";

//        //    var parameters = new List<MySqlParameter>
//        //    {
//        //        new MySqlParameter("?Year", selectedYear),
//        //        new MySqlParameter("?Month", selectedMonth),
//        //        new MySqlParameter("?CategoryId", selectedCategoryId)
//        //    };

//        //    List<WeeklyRevenue> weeklyList = new List<WeeklyRevenue>();
//        //    try
//        //    {
//        //        DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//        //        foreach (DataRow row in dt.Rows)
//        //        {
//        //            decimal revenue = row["WeeklyRevenue"] != DBNull.Value ? Convert.ToDecimal(row["WeeklyRevenue"]) : 0;
//        //            if (revenue > 0)
//        //            {
//        //                weeklyList.Add(new WeeklyRevenue
//        //                {
//        //                    WeekNumber = Convert.ToInt32(row["WeekNumber"]),
//        //                    Revenue = revenue
//        //                });
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        MessageBox.Show($"Lỗi lấy doanh thu hàng tuần: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        //        return new List<WeeklyRevenue>();
//        //    }

//        //    // Ensure we have weeks 1-5 represented (add empty weeks if needed)
//        //    for (int i = 1; i <= 5; i++)
//        //    {
//        //        if (!weeklyList.Any(w => w.WeekNumber == i))
//        //        {
//        //            weeklyList.Add(new WeeklyRevenue { WeekNumber = i, Revenue = 0 });
//        //        }
//        //    }

//        //    return weeklyList.OrderBy(w => w.WeekNumber).ToList();
//        //}

//        private List<WeeklyRevenue> GetWeeklyRevenue()
//        {
//            string query = @"
//        SELECT FLOOR((DAY(seh.updated_at) - 1) / 7) + 1 AS WeekNumber,
//               COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS WeeklyRevenue
//        FROM stock_exit_headers seh
//        JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//        LEFT JOIN products p ON sed.product_id = p.id
//        WHERE YEAR(seh.updated_at) = ?Year 
//        AND MONTH(seh.updated_at) = ?Month
//        AND (p.category_id = ?CategoryId OR ?CategoryId = 0)
//        GROUP BY FLOOR((DAY(seh.updated_at) - 1) / 7) + 1
//        ORDER BY WeekNumber";

//            var parameters = new List<MySqlParameter>
//    {
//        new MySqlParameter("?Year", selectedYear),
//        new MySqlParameter("?Month", selectedMonth),
//        new MySqlParameter("?CategoryId", selectedCategoryId)
//    };

//            List<WeeklyRevenue> weeklyList = new List<WeeklyRevenue>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["WeeklyRevenue"] != DBNull.Value ? Convert.ToDecimal(row["WeeklyRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        weeklyList.Add(new WeeklyRevenue
//                        {
//                            WeekNumber = Convert.ToInt32(row["WeekNumber"]),
//                            Revenue = revenue
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi truy vấn dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<WeeklyRevenue>();
//            }

//            // Ensure we have weeks 1-5 represented (add empty weeks if needed)
//            for (int i = 1; i <= 5; i++)
//            {
//                if (!weeklyList.Any(w => w.WeekNumber == i))
//                {
//                    weeklyList.Add(new WeeklyRevenue { WeekNumber = i, Revenue = 0 });
//                }
//            }

//            return weeklyList.OrderBy(w => w.WeekNumber).ToList();
//        }

//        private void DisplayWeeklyRevenueChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();

//            List<WeeklyRevenue> weeklyList = GetWeeklyRevenue();
//            if (weeklyList.Count == 0 || weeklyList.All(w => w.Revenue == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 2
//            };

//            decimal maxRevenue = 0;
//            foreach (var week in weeklyList)
//            {
//                decimal revenueInMillions = week.Revenue / 1000000;
//                series.Points.AddXY($"Tuần {week.WeekNumber}", revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Tuần";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class ProductSales
//        {
//            public string ProductName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<ProductSales> GetTop6ProductsBySales()
//        {
//            string query = @"
//                SELECT p.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//                FROM products p
//                LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//                LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//                AND YEAR(seh.updated_at) = ?Year 
//                AND MONTH(seh.updated_at) = ?Month
//                WHERE p.category_id = ?CategoryId
//                GROUP BY p.id, p.name
//                HAVING TotalQuantity > 0
//                ORDER BY TotalQuantity DESC
//                LIMIT 6";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth),
//                new MySqlParameter("?CategoryId", selectedCategoryId)
//            };

//            List<ProductSales> productSalesList = new List<ProductSales>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        productSalesList.Add(new ProductSales
//                        {
//                            ProductName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy dữ liệu sản phẩm bán ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<ProductSales>();
//            }

//            return productSalesList;
//        }

//        private void DisplayProductRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            List<ProductSales> productSalesList = GetTop6ProductsBySales();
//            if (productSalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var product in productSalesList)
//            {
//                double percentage = Math.Round((double)(product.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = product.ProductName;
//                point.YValues = new double[] { Convert.ToDouble(product.Quantity) };
//                point.LegendText = $"{product.ProductName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }

//        private class CategorySales
//        {
//            public string CategoryName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//                "AND c.id = ?CategoryId" : "";

//            string query = @"
//                SELECT c.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//                FROM categories c
//                LEFT JOIN products p ON p.category_id = c.id
//                LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//                LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//                    AND YEAR(seh.updated_at) = ?Year
//                    AND MONTH(seh.updated_at) = ?Month
//                WHERE 1=1 " + categoryFilter + @"
//                GROUP BY c.id, c.name
//                HAVING TotalQuantity > 0
//                ORDER BY TotalQuantity DESC";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//                List<CategorySales> categorySalesList = new List<CategorySales>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        categorySalesList.Add(new CategorySales
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }

//                if (categorySalesList.Count == 0)
//                {
//                    return new List<CategorySales>();
//                }

//                List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
//                decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

//                if (categorySalesList.Count > 5)
//                {
//                    topCategories.Add(new CategorySales
//                    {
//                        CategoryName = "Khác",
//                        Quantity = othersQuantity
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy tỉ lệ bán hàng theo danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<CategorySales>();
//            }
//        }

//        private void DisplayCategorySalesRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
//            if (categorySalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = categorySalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var category in categorySalesList)
//            {
//                double percentage = Math.Round((double)(category.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = category.CategoryName;
//                point.YValues = new double[] { Convert.ToDouble(category.Quantity) };
//                point.LegendText = $"{category.CategoryName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }
//    }
//}

// Code ok nhất
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;
//using System.Drawing.Drawing2D;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ReportForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private FlowLayoutPanel flpSummaryCards;
//        private TableLayoutPanel tblCharts;
//        private Label lblTotalRevenue;
//        private Label lblProfit;
//        private Label lblProfitRatio;
//        private ComboBox cmbMonthFilter;
//        private ComboBox cmbCategoryFilter;
//        private int selectedYear;
//        private int selectedMonth;
//        private int selectedCategoryId;
//        private System.Windows.Forms.Timer dataUpdateTimer;
//        private DateTime lastUpdateTime;
//        private Label lblTitle;
//        private Chart chartRevenueByCategory;
//        private Chart chartProductRatio;
//        private bool hasAccessPermission = false;
//        private List<KeyValuePair<int, string>> categories;

//        public ReportForm() : base()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        public ReportForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                LoadCategories();
//                InitializeComponents();
//                InitializeTimer();
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void LoadCategories()
//        {
//            categories = new List<KeyValuePair<int, string>>();
//            string query = "SELECT id, name FROM categories ORDER BY name";
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//                foreach (DataRow row in dt.Rows)
//                {
//                    int id = Convert.ToInt32(row["id"]);
//                    string name = row["name"].ToString();
//                    categories.Add(new KeyValuePair<int, string>(id, name));
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//            }
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1 || RoleId == 3);
//        }

//        private void ShowAccessDeniedMessage()
//        {
//            this.Controls.Clear();

//            Label lblAccessDenied = new Label
//            {
//                Text = "Không có quyền truy cập!",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                AutoSize = true,
//                TextAlign = ContentAlignment.MiddleCenter
//            };

//            Panel pnlAccessDenied = new Panel
//            {
//                Dock = DockStyle.Fill
//            };

//            lblAccessDenied.Location = new Point(
//            (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//            (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//            );

//            pnlAccessDenied.Controls.Add(lblAccessDenied);
//            this.Controls.Add(pnlAccessDenied);

//            this.Resize += (sender, e) =>
//            {
//                lblAccessDenied.Location = new Point(
//                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//                );
//            };
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "Báo Cáo";
//            this.Size = new Size(1200, 800);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;

//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 4,
//                ColumnCount = 1,
//                Padding = new Padding(260, 40, 10, 10),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header
//            Panel pnlHeader = new Panel
//            {
//                BackColor = Color.FromArgb(52, 152, 219),
//                Dock = DockStyle.Top,
//                Height = 60
//            };

//            lblTitle = new Label
//            {
//                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 15),
//                TextAlign = ContentAlignment.MiddleLeft
//            };
//            pnlHeader.Controls.Add(lblTitle);
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Filter Panel
//            Panel pnlFilter = new Panel
//            {
//                BackColor = Color.White,
//                Margin = new Padding(0, 10, 0, 10),
//                Padding = new Padding(15),
//                Height = 60,
//                Dock = DockStyle.Fill
//            };

//            FlowLayoutPanel flpFilter = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                FlowDirection = FlowDirection.LeftToRight,
//                Dock = DockStyle.Fill
//            };

//            Label lblMonthFilter = new Label
//            {
//                Text = "Tháng:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(0, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblMonthFilter);

//            cmbMonthFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 150,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            for (int month = 1; month <= 12; month++)
//            {
//                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//            }
//            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
//            selectedYear = 2025;
//            selectedMonth = DateTime.Now.Month;
//            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbMonthFilter);

//            Label lblCategoryFilter = new Label
//            {
//                Text = "Danh mục:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblCategoryFilter);

//            cmbCategoryFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 0, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            foreach (var category in categories)
//            {
//                cmbCategoryFilter.Items.Add(category.Value);
//            }
//            cmbCategoryFilter.SelectedIndex = 0;
//            selectedCategoryId = 0;
//            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbCategoryFilter);

//            Button btnExport = new Button
//            {
//                Text = "Xuất báo cáo",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(120, 30),
//                Margin = new Padding(40, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnExport.FlatAppearance.BorderSize = 0;
//            btnExport.Click += BtnExport_Click;
//            flpFilter.Controls.Add(btnExport);

//            pnlFilter.Controls.Add(flpFilter);
//            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//            // Summary Cards - Centered Horizontally
//            Panel pnlSummaryCardsContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(240, 242, 245),
//                Margin = new Padding(0, 0, 0, 20),
//                Padding = new Padding(10)
//            };

//            flpSummaryCards = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = false,
//                Anchor = AnchorStyles.Top,
//                BackColor = Color.FromArgb(240, 242, 245)
//            };

//            // Center flpSummaryCards initially and on resize
//            flpSummaryCards.Location = new Point(
//            (pnlSummaryCardsContainer.Width - flpSummaryCards.Width) / 2,
//            10
//            );

//            // Add summary cards
//            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardRevenue);

//            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfit);

//            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ lợi nhuận", "0%", Color.FromArgb(230, 126, 34));
//            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfitRatio);

//            pnlSummaryCardsContainer.Controls.Add(flpSummaryCards);
//            tblMainContainer.Controls.Add(pnlSummaryCardsContainer, 0, 2);

//            // Update position and size on container resize
//            pnlSummaryCardsContainer.Resize += (sender, e) =>
//            {
//                // Calculate total width of cards and gaps
//                int totalWidth = flpSummaryCards.Controls.Count * 320 + (flpSummaryCards.Controls.Count - 1) * 10;
//                int leftMargin = Math.Max(0, (pnlSummaryCardsContainer.Width - totalWidth) / 2);
//                flpSummaryCards.Location = new Point(leftMargin, 10);
//                pnlSummaryCardsContainer.Invalidate();
//                flpSummaryCards.Invalidate();
//            };

//            // Charts Container
//            tblCharts = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 2,
//                Margin = new Padding(0),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//            tblMainContainer.Controls.Add(tblCharts, 0, 3);

//            // Column Chart Panel
//            Panel pnlColumnChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 5, 0),
//                Padding = new Padding(15)
//            };

//            Label lblColumnChartTitle = new Label
//            {
//                Text = "Doanh Thu Theo Danh Mục",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlColumnChart.Controls.Add(lblColumnChartTitle);

//            chartRevenueByCategory = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea columnChartArea = new ChartArea();
//            columnChartArea.AxisX.MajorGrid.Enabled = false;
//            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.BackColor = Color.White;
//            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//            pnlColumnChart.Controls.Add(chartRevenueByCategory);

//            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//            // Pie Chart Panel
//            Panel pnlPieChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(5, 0, 0, 0),
//                Padding = new Padding(15)
//            };

//            Label lblPieChartTitle = new Label
//            {
//                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlPieChart.Controls.Add(lblPieChartTitle);

//            chartProductRatio = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea pieChartArea = new ChartArea();
//            pieChartArea.BackColor = Color.White;
//            chartProductRatio.ChartAreas.Add(pieChartArea);
//            pnlPieChart.Controls.Add(chartProductRatio);

//            tblCharts.Controls.Add(pnlPieChart, 1, 0);

//            UpdateReport();
//        }

//        private Panel CreateSummaryCard(string title, string value, Color accentColor)
//        {
//            Panel card = new Panel
//            {
//                Width = 320,
//                Height = 100,
//                BackColor = accentColor,
//                Margin = new Padding(5),
//                BorderStyle = BorderStyle.None
//            };

//            Label lblTitle = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 11F),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, 15)
//            };

//            Label lblValue = new Label
//            {
//                Name = "lblValue",
//                Text = value,
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                AutoSize = true,
//                ForeColor = Color.White,
//                Location = new Point(15, 40)
//            };

//            card.Controls.Add(lblTitle);
//            card.Controls.Add(lblValue);

//            return card;
//        }

//        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string selected = cmbMonthFilter.SelectedItem.ToString();
//            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//            selectedYear = int.Parse(selected.Split('/')[1]);
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void UpdateTitle()
//        {
//            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
//            if (selectedCategoryId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
//            }
//            else
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
//            }
//        }

//        private void BtnExport_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void InitializeTimer()
//        {
//            dataUpdateTimer = new System.Windows.Forms.Timer
//            {
//                Interval = 10000
//            };
//            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//            dataUpdateTimer.Start();

//            lastUpdateTime = GetLastUpdateTime();
//        }

//        private DateTime GetLastUpdateTime()
//        {
//            string categoryFilter = selectedCategoryId > 0 ?
//            "AND p.category_id = ?CategoryId" : "";

//            string query = @"
//SELECT MAX(seh.updated_at)
//FROM stock_exit_headers seh
//LEFT JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//LEFT JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month " + categoryFilter;

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//                ? Convert.ToDateTime(dt.Rows[0][0])
//                : DateTime.MinValue;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetLastUpdateTime: {ex.Message}");
//                return lastUpdateTime;
//            }
//        }

//        private void DataUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                DateTime currentUpdateTime = GetLastUpdateTime();
//                if (currentUpdateTime > lastUpdateTime)
//                {
//                    lastUpdateTime = currentUpdateTime;
//                    UpdateReport();
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in DataUpdateTimer_Tick: {ex.Message}");
//            }
//        }

//        private void UpdateReport()
//        {
//            bool hasData = HasData();
//            if (hasData)
//            {
//                decimal totalRevenue = CalculateTotalRevenue();
//                decimal totalStockEntryCost = CalculateTotalStockEntryCost();
//                decimal profit = totalRevenue - totalStockEntryCost;
//                decimal profitRatio = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

//                lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//                lblProfit.Text = FormatCurrency(profit);
//                lblProfitRatio.Text = FormatPercentage(profitRatio);
//            }
//            else
//            {
//                lblTotalRevenue.Text = "0 VND";
//                lblProfit.Text = "0 VND";
//                lblProfitRatio.Text = "0%";
//            }

//            if (hasData)
//            {
//                if (selectedCategoryId == 0)
//                {
//                    DisplayRevenueByCategoryChart();
//                }
//                else
//                {
//                    DisplayWeeklyRevenueChart();
//                }

//                if (selectedCategoryId == 0)
//                {
//                    DisplayCategorySalesRatioChart();
//                }
//                else
//                {
//                    DisplayProductRatioChart();
//                }
//            }
//            else
//            {
//                DisplayNoDataCharts();
//            }
//        }

//        private bool HasData()
//        {
//            string query = @"
//SELECT COUNT(*) AS DataCount
//FROM stock_exit_headers seh
//LEFT JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//LEFT JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["DataCount"]) > 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi kiểm tra dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return false;
//            }
//        }

//        private void DisplayNoDataCharts()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//            new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartRevenueByCategory.Titles.Add(noDataTitle);

//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();
//            Title noDataTitlePie = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//            new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartProductRatio.Titles.Add(noDataTitlePie);
//        }

//        private string FormatCurrency(decimal value)
//        {
//            return String.Format("{0:n0} VND", value);
//        }

//        private string FormatPercentage(decimal value)
//        {
//            return String.Format("{0:F2}%", value);
//        }

//        private decimal CalculateTotalRevenue()
//        {
//            string query = @"
//SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS TotalRevenue
//FROM stock_exit_headers seh
//INNER JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//                ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//                : 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tính tổng doanh thu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }

//        private decimal CalculateTotalStockEntryCost()
//        {
//            string query = @"
//SELECT COALESCE(SUM(sed.total_price), 0) AS TotalStockEntryCost
//FROM stock_entry_headers seh
//INNER JOIN stock_entry_details sed ON sed.stock_entry_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalStockEntryCost"] != DBNull.Value
//                ? Convert.ToDecimal(dt.Rows[0]["TotalStockEntryCost"])
//                : 0;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tính tổng chi phí nhập kho: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return 0;
//            }
//        }

//        private class CategoryRevenue
//        {
//            public string CategoryName { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//        {
//            string query = @"
//SELECT c.name, COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS CategoryRevenue
//FROM categories c
//LEFT JOIN products p ON p.category_id = c.id
//LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//AND YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month
//WHERE 1=1";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND c.id = ?CategoryId";
//            }

//            query += @"
//GROUP BY c.id, c.name
//HAVING CategoryRevenue > 0
//ORDER BY CategoryRevenue DESC";

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//                List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["CategoryRevenue"] != DBNull.Value ? Convert.ToDecimal(row["CategoryRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        categoryList.Add(new CategoryRevenue
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Revenue = revenue
//                        });
//                    }
//                }

//                if (categoryList.Count == 0)
//                {
//                    return new List<CategoryRevenue>();
//                }

//                List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
//                decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

//                if (categoryList.Count > 5)
//                {
//                    topCategories.Add(new CategoryRevenue
//                    {
//                        CategoryName = "Khác",
//                        Revenue = othersRevenue
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy doanh thu theo danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<CategoryRevenue>();
//            }
//        }

//        private void DisplayRevenueByCategoryChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();

//            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//            if (categoryList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Column,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 0
//            };

//            decimal maxRevenue = 0;
//            foreach (var category in categoryList)
//            {
//                decimal revenueInMillions = category.Revenue / 1000000;
//                series.Points.AddXY(category.CategoryName, revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.BorderColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);

//            foreach (DataPoint point in series.Points)
//            {
//                point["PixelPointWidth"] = "40";
//            }
//        }

//        private class WeeklyRevenue
//        {
//            public int WeekNumber { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<WeeklyRevenue> GetWeeklyRevenue()
//        {
//            string query = @"
//SELECT FLOOR((DAY(seh.updated_at) - 1) / 7) + 1 AS WeekNumber,
//COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS WeeklyRevenue
//FROM stock_exit_headers seh
//INNER JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            query += @"
//GROUP BY FLOOR((DAY(seh.updated_at) - 1) / 7) + 1
//ORDER BY WeekNumber";

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            List<WeeklyRevenue> weeklyList = new List<WeeklyRevenue>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["WeeklyRevenue"] != DBNull.Value ? Convert.ToDecimal(row["WeeklyRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        weeklyList.Add(new WeeklyRevenue
//                        {
//                            WeekNumber = Convert.ToInt32(row["WeekNumber"]),
//                            Revenue = revenue
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi truy vấn dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<WeeklyRevenue>();
//            }

//            for (int i = 1; i <= 5; i++)
//            {
//                if (!weeklyList.Any(w => w.WeekNumber == i))
//                {
//                    weeklyList.Add(new WeeklyRevenue { WeekNumber = i, Revenue = 0 });
//                }
//            }

//            return weeklyList.OrderBy(w => w.WeekNumber).ToList();
//        }

//        private void DisplayWeeklyRevenueChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();

//            List<WeeklyRevenue> weeklyList = GetWeeklyRevenue();
//            if (weeklyList.Count == 0 || weeklyList.All(w => w.Revenue == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 2
//            };

//            decimal maxRevenue = 0;
//            foreach (var week in weeklyList)
//            {
//                decimal revenueInMillions = week.Revenue / 1000000;
//                series.Points.AddXY($"Tuần {week.WeekNumber}", revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Tuần";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class ProductSales
//        {
//            public string ProductName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<ProductSales> GetTop6ProductsBySales()
//        {
//            string query = @"
//SELECT p.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//FROM products p
//INNER JOIN stock_exit_details sed ON sed.product_id = p.id
//INNER JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            query += @"
//GROUP BY p.id, p.name
//HAVING TotalQuantity > 0
//ORDER BY TotalQuantity DESC
//LIMIT 6";

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            List<ProductSales> productSalesList = new List<ProductSales>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        productSalesList.Add(new ProductSales
//                        {
//                            ProductName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy dữ liệu sản phẩm bán ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<ProductSales>();
//            }

//            return productSalesList;
//        }

//        private void DisplayProductRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            List<ProductSales> productSalesList = GetTop6ProductsBySales();
//            if (productSalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//Color.FromArgb(41, 128, 185),
//Color.FromArgb(231, 76, 60),
//Color.FromArgb(46, 204, 113),
//Color.FromArgb(155, 89, 182),
//Color.FromArgb(243, 156, 18),
//Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var product in productSalesList)
//            {
//                double percentage = Math.Round((double)(product.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = product.ProductName;
//                point.YValues = new double[] { Convert.ToDouble(product.Quantity) };
//                point.LegendText = $"{product.ProductName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }

//        private class CategorySales
//        {
//            public string CategoryName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
//        {
//            string query = @"
//SELECT c.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//FROM categories c
//LEFT JOIN products p ON p.category_id = c.id
//LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND c.id = ?CategoryId";
//            }

//            query += @"
//GROUP BY c.id, c.name
//HAVING TotalQuantity > 0
//ORDER BY TotalQuantity DESC";

//            var parameters = new List<MySqlParameter>
//{
//new MySqlParameter("?Year", selectedYear),
//new MySqlParameter("?Month", selectedMonth)
//};

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//                List<CategorySales> categorySalesList = new List<CategorySales>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        categorySalesList.Add(new CategorySales
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }

//                if (categorySalesList.Count == 0)
//                {
//                    return new List<CategorySales>();
//                }

//                List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
//                decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

//                if (categorySalesList.Count > 5)
//                {
//                    topCategories.Add(new CategorySales
//                    {
//                        CategoryName = "Khác",
//                        Quantity = othersQuantity
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi lấy tỉ lệ bán hàng theo danh mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return new List<CategorySales>();
//            }
//        }

//        private void DisplayCategorySalesRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
//            if (categorySalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ")
//            {
//                ChartType = SeriesChartType.Pie,
//                IsValueShownAsLabel = true,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                LabelForeColor = Color.White
//            };

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Alignment = StringAlignment.Center,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            decimal totalQuantity = categorySalesList.Sum(c => c.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Color[] colors = new Color[]
//            {
//Color.FromArgb(41, 128, 185),
//Color.FromArgb(231, 76, 60),
//Color.FromArgb(46, 204, 113),
//Color.FromArgb(155, 89, 182),
//Color.FromArgb(243, 156, 18),
//Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var category in categorySalesList)
//            {
//                double percentage = Math.Round((double)(category.Quantity / totalQuantity) * 100, 1);
//                DataPoint point = new DataPoint
//                {
//                    AxisLabel = category.CategoryName,
//                    YValues = new double[] { (double)category.Quantity },
//                    LegendText = $"{category.CategoryName} ({percentage}%)",
//                    Label = $"{percentage}%",
//                    Color = colors[colorIndex % colors.Length]
//                };
//                series.Points.Add(point);
//                colorIndex++;
//            }

//            chartProductRatio.Series.Add(series);

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//        }
//    }
//}


// Code mới cũng tạm tạm (Đang trong quá trình fix)
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ReportForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private FlowLayoutPanel flpSummaryCards;
//        private TableLayoutPanel tblCharts;
//        private Label lblTotalRevenue;
//        private Label lblProfit;
//        private Label lblProfitRatio;
//        private ComboBox cmbMonthFilter;
//        private ComboBox cmbCategoryFilter;
//        private ComboBox cmbProductFilter;
//        private int selectedYear;
//        private int selectedMonth;
//        private int selectedCategoryId;
//        private int selectedProductId;
//        private System.Windows.Forms.Timer dataUpdateTimer;
//        private DateTime lastUpdateTime;
//        private Label lblTitle;
//        private Chart chartRevenueByCategory;
//        private Chart chartProductRatio;
//        private Label lblColumnChartTitle;
//        private Panel pnlPieChart;
//        private bool hasAccessPermission = false;
//        private List<KeyValuePair<int, string>> categories;
//        private List<KeyValuePair<int, string>> products;

//        public ReportForm() : base()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        public ReportForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                LoadCategories();
//                LoadProducts();
//                InitializeComponents();
//                InitializeTimer();
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void LoadCategories()
//        {
//            categories = new List<KeyValuePair<int, string>>();
//            string query = "SELECT id, name FROM categories ORDER BY name";
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//                foreach (DataRow row in dt.Rows)
//                {
//                    int id = Convert.ToInt32(row["id"]);
//                    string name = row["name"].ToString();
//                    categories.Add(new KeyValuePair<int, string>(id, name));
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in LoadCategories: {ex.Message}");
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//            }
//        }

//        private void LoadProducts()
//        {
//            products = new List<KeyValuePair<int, string>>();
//            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));
//        }

//        private void UpdateProductFilter()
//        {
//            cmbProductFilter.Items.Clear();
//            products.Clear();
//            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));

//            if (selectedCategoryId > 0)
//            {
//                string query = "SELECT id, name FROM products WHERE category_id = ?CategoryId ORDER BY name";
//                var parameters = new List<MySqlParameter>
//                {
//                    new MySqlParameter("?CategoryId", selectedCategoryId)
//                };
//                try
//                {
//                    DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                    foreach (DataRow row in dt.Rows)
//                    {
//                        int id = Convert.ToInt32(row["id"]);
//                        string name = row["name"].ToString();
//                        products.Add(new KeyValuePair<int, string>(id, name));
//                    }
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error in UpdateProductFilter: {ex.Message}");
//                }
//            }

//            foreach (var product in products)
//            {
//                cmbProductFilter.Items.Add(product.Value);
//            }
//            cmbProductFilter.SelectedIndex = 0;
//            selectedProductId = 0;
//            cmbProductFilter.Enabled = selectedCategoryId > 0;
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1 || RoleId == 3);
//        }

//        private void ShowAccessDeniedMessage()
//        {
//            this.Controls.Clear();

//            Label lblAccessDenied = new Label
//            {
//                Text = "Không có quyền truy cập!",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                AutoSize = true,
//                TextAlign = ContentAlignment.MiddleCenter
//            };

//            Panel pnlAccessDenied = new Panel
//            {
//                Dock = DockStyle.Fill
//            };

//            lblAccessDenied.Location = new Point(
//                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//            );

//            pnlAccessDenied.Controls.Add(lblAccessDenied);
//            this.Controls.Add(pnlAccessDenied);

//            this.Resize += (sender, e) =>
//            {
//                lblAccessDenied.Location = new Point(
//                    (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                    (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//                );
//            };
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "Báo Cáo";
//            this.Size = new Size(1200, 800);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;

//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 4,
//                ColumnCount = 1,
//                Padding = new Padding(260, 40, 10, 10),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header
//            Panel pnlHeader = new Panel
//            {
//                BackColor = Color.FromArgb(52, 152, 219),
//                Dock = DockStyle.Top,
//                Height = 60
//            };

//            lblTitle = new Label
//            {
//                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 15),
//                TextAlign = ContentAlignment.MiddleLeft
//            };
//            pnlHeader.Controls.Add(lblTitle);
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Filter Panel
//            Panel pnlFilter = new Panel
//            {
//                BackColor = Color.White,
//                Margin = new Padding(0, 10, 0, 10),
//                Padding = new Padding(15),
//                Height = 60,
//                Dock = DockStyle.Fill
//            };

//            FlowLayoutPanel flpFilter = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                FlowDirection = FlowDirection.LeftToRight,
//                Dock = DockStyle.Fill
//            };

//            Label lblMonthFilter = new Label
//            {
//                Text = "Tháng:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(0, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblMonthFilter);

//            cmbMonthFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 150,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            for (int month = 1; month <= 12; month++)
//            {
//                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//            }
//            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
//            selectedYear = 2025;
//            selectedMonth = DateTime.Now.Month;
//            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbMonthFilter);

//            Label lblCategoryFilter = new Label
//            {
//                Text = "Danh mục:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblCategoryFilter);

//            cmbCategoryFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            foreach (var category in categories)
//            {
//                cmbCategoryFilter.Items.Add(category.Value);
//            }
//            cmbCategoryFilter.SelectedIndex = 0;
//            selectedCategoryId = 0;
//            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbCategoryFilter);

//            Label lblProductFilter = new Label
//            {
//                Text = "Sản phẩm:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblProductFilter);

//            cmbProductFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat,
//                Enabled = false
//            };

//            foreach (var product in products)
//            {
//                cmbProductFilter.Items.Add(product.Value);
//            }
//            cmbProductFilter.SelectedIndex = 0;
//            selectedProductId = 0;
//            cmbProductFilter.SelectedIndexChanged += CmbProductFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbProductFilter);

//            Button btnExport = new Button
//            {
//                Text = "Xuất báo cáo",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(120, 30),
//                Margin = new Padding(40, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnExport.FlatAppearance.BorderSize = 0;
//            btnExport.Click += BtnExport_Click;
//            flpFilter.Controls.Add(btnExport);

//            pnlFilter.Controls.Add(flpFilter);
//            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//            // Summary Cards - Centered Horizontally
//            Panel pnlSummaryCardsContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(240, 242, 245),
//                Margin = new Padding(0, 0, 0, 20),
//                Padding = new Padding(10)
//            };

//            flpSummaryCards = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = false,
//                Anchor = AnchorStyles.Top,
//                BackColor = Color.FromArgb(240, 242, 245)
//            };

//            flpSummaryCards.Location = new Point(
//                (pnlSummaryCardsContainer.Width - flpSummaryCards.Width) / 2,
//                10
//            );

//            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardRevenue);

//            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfit);

//            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ lợi nhuận", "0%", Color.FromArgb(230, 126, 34));
//            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfitRatio);

//            pnlSummaryCardsContainer.Controls.Add(flpSummaryCards);
//            tblMainContainer.Controls.Add(pnlSummaryCardsContainer, 0, 2);

//            pnlSummaryCardsContainer.Resize += (sender, e) =>
//            {
//                int totalWidth = flpSummaryCards.Controls.Count * 320 + (flpSummaryCards.Controls.Count - 1) * 10;
//                int leftMargin = Math.Max(0, (pnlSummaryCardsContainer.Width - totalWidth) / 2);
//                flpSummaryCards.Location = new Point(leftMargin, 10);
//                pnlSummaryCardsContainer.Invalidate();
//                flpSummaryCards.Invalidate();
//            };

//            // Charts Container
//            tblCharts = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 2,
//                Margin = new Padding(0),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//            tblMainContainer.Controls.Add(tblCharts, 0, 3);

//            // Column/Line Chart Panel
//            Panel pnlColumnChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 5, 0),
//                Padding = new Padding(15)
//            };

//            lblColumnChartTitle = new Label
//            {
//                Text = "Doanh Thu Theo Danh Mục",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlColumnChart.Controls.Add(lblColumnChartTitle);

//            chartRevenueByCategory = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea columnChartArea = new ChartArea();
//            columnChartArea.AxisX.MajorGrid.Enabled = false;
//            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.BackColor = Color.White;
//            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//            pnlColumnChart.Controls.Add(chartRevenueByCategory);

//            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//            // Pie Chart Panel
//            pnlPieChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(5, 0, 0, 0),
//                Padding = new Padding(15),
//                Visible = true
//            };

//            Label lblPieChartTitle = new Label
//            {
//                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlPieChart.Controls.Add(lblPieChartTitle);

//            chartProductRatio = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea pieChartArea = new ChartArea();
//            pieChartArea.BackColor = Color.White;
//            chartProductRatio.ChartAreas.Add(pieChartArea);
//            pnlPieChart.Controls.Add(chartProductRatio);

//            tblCharts.Controls.Add(pnlPieChart, 1, 0);

//            UpdateReport();
//        }

//        private Panel CreateSummaryCard(string title, string value, Color accentColor)
//        {
//            Panel card = new Panel
//            {
//                Width = 320,
//                Height = 100,
//                BackColor = accentColor,
//                Margin = new Padding(5),
//                BorderStyle = BorderStyle.None
//            };

//            Label lblTitle = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 11F),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, 15)
//            };

//            Label lblValue = new Label
//            {
//                Name = "lblValue",
//                Text = value,
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                AutoSize = true,
//                ForeColor = Color.White,
//                Location = new Point(15, 40)
//            };

//            card.Controls.Add(lblTitle);
//            card.Controls.Add(lblValue);

//            return card;
//        }

//        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string selected = cmbMonthFilter.SelectedItem.ToString();
//            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//            selectedYear = int.Parse(selected.Split('/')[1]);
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
//            UpdateProductFilter();
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbProductFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedProductId = products[cmbProductFilter.SelectedIndex].Key;
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void UpdateTitle()
//        {
//            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
//            string productName = products[cmbProductFilter.SelectedIndex].Value;
//            if (selectedProductId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName} - {productName}";
//            }
//            else if (selectedCategoryId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
//            }
//            else
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
//            }
//        }

//        private void BtnExport_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void InitializeTimer()
//        {
//            dataUpdateTimer = new System.Windows.Forms.Timer
//            {
//                Interval = 30000 // Increased to 30 seconds
//            };
//            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//            dataUpdateTimer.Start();

//            lastUpdateTime = GetLastUpdateTime();
//        }

//        private DateTime GetLastUpdateTime()
//        {
//            string categoryFilter = selectedCategoryId > 0 ? "AND p.category_id = ?CategoryId" : "";
//            string productFilter = selectedProductId > 0 ? "AND p.id = ?ProductId" : "";

//            string query = @"
//SELECT MAX(seh.updated_at)
//FROM stock_exit_headers seh
//LEFT JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//LEFT JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month " + categoryFilter + productFilter;

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }
//            if (selectedProductId > 0)
//            {
//                parameters.Add(new MySqlParameter("?ProductId", selectedProductId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value
//                    ? Convert.ToDateTime(dt.Rows[0][0])
//                    : DateTime.MinValue;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetLastUpdateTime: {ex.Message}");
//                return lastUpdateTime;
//            }
//        }

//        private void DataUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                DateTime currentUpdateTime = GetLastUpdateTime();
//                if (currentUpdateTime > lastUpdateTime)
//                {
//                    lastUpdateTime = currentUpdateTime;
//                    UpdateReport();
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in DataUpdateTimer_Tick: {ex.Message}");
//            }
//        }

//        private void UpdateReport()
//        {
//            bool hasData = HasData();
//            if (!hasData)
//            {
//                lblTotalRevenue.Text = "0 VND";
//                lblProfit.Text = "0 VND";
//                lblProfitRatio.Text = "0%";
//                DisplayNoDataCharts();
//                pnlPieChart.Visible = selectedProductId == 0;
//                AdjustChartLayout();
//                return;
//            }

//            decimal totalRevenue = CalculateTotalRevenue();
//            decimal totalStockEntryCost = CalculateTotalStockEntryCost();
//            decimal profit = totalRevenue - totalStockEntryCost;
//            decimal profitRatio = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

//            lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//            lblProfit.Text = FormatCurrency(profit);
//            lblProfitRatio.Text = FormatPercentage(profitRatio);

//            pnlPieChart.Visible = selectedProductId == 0;
//            AdjustChartLayout();

//            if (selectedProductId > 0)
//            {
//                DisplayProductSalesTrendChart();
//            }
//            else if (selectedCategoryId == 0)
//            {
//                DisplayRevenueByCategoryChart();
//            }
//            else
//            {
//                DisplayWeeklyRevenueChart();
//            }

//            if (pnlPieChart.Visible)
//            {
//                if (selectedCategoryId == 0)
//                {
//                    DisplayCategorySalesRatioChart();
//                }
//                else
//                {
//                    DisplayProductRatioChart();
//                }
//            }
//        }

//        private void AdjustChartLayout()
//        {
//            if (pnlPieChart.Visible)
//            {
//                if (tblCharts.ColumnCount == 1)
//                {
//                    tblCharts.ColumnCount = 2;
//                    tblCharts.ColumnStyles.Clear();
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//                    tblCharts.Controls.Add(pnlPieChart, 1, 0);
//                }
//                pnlPieChart.Show();
//            }
//            else
//            {
//                if (tblCharts.ColumnCount == 2)
//                {
//                    tblCharts.ColumnCount = 1;
//                    tblCharts.ColumnStyles.Clear();
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
//                    tblCharts.Controls.Remove(pnlPieChart);
//                }
//                pnlPieChart.Hide();
//            }
//        }

//        private bool HasData()
//        {
//            string query = @"
//SELECT COUNT(sed.id) AS DataCount
//FROM stock_exit_headers seh
//LEFT JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//LEFT JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month
//AND sed.id IS NOT NULL";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }
//            if (selectedProductId > 0)
//            {
//                query += " AND p.id = ?ProductId";
//            }

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }
//            if (selectedProductId > 0)
//            {
//                parameters.Add(new MySqlParameter("?ProductId", selectedProductId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["DataCount"]) > 0;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in HasData: {ex.Message}");
//                return false;
//            }
//        }

//        private void DisplayNoDataCharts()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";
//            Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartRevenueByCategory.Titles.Add(noDataTitle);

//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();
//            Title noDataTitlePie = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartProductRatio.Titles.Add(noDataTitlePie);
//        }

//        private string FormatCurrency(decimal value)
//        {
//            return String.Format("{0:n0} VND", value);
//        }

//        private string FormatPercentage(decimal value)
//        {
//            return String.Format("{0:F2}%", value);
//        }

//        private decimal CalculateTotalRevenue()
//        {
//            if (!HasData()) return 0;

//            string query = @"
//SELECT COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS TotalRevenue
//FROM stock_exit_headers seh
//INNER JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }
//            if (selectedProductId > 0)
//            {
//                query += " AND p.id = ?ProductId";
//            }

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }
//            if (selectedProductId > 0)
//            {
//                parameters.Add(new MySqlParameter("?ProductId", selectedProductId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalRevenue"] != DBNull.Value
//                    ? Convert.ToDecimal(dt.Rows[0]["TotalRevenue"])
//                    : 0;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalRevenue: {ex.Message}");
//                return 0;
//            }
//        }

//        private decimal CalculateTotalStockEntryCost()
//        {
//            if (!HasData()) return 0;

//            string query = @"
//SELECT COALESCE(SUM(sed.total_price), 0) AS TotalStockEntryCost
//FROM stock_entry_headers seh
//LEFT JOIN stock_entry_details sed ON sed.stock_entry_id = seh.id
//LEFT JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.updated_at) = ?Year
//AND MONTH(seh.updated_at) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }
//            if (selectedProductId > 0)
//            {
//                query += " AND p.id = ?ProductId";
//            }

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }
//            if (selectedProductId > 0)
//            {
//                parameters.Add(new MySqlParameter("?ProductId", selectedProductId));
//            }

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                return dt.Rows.Count > 0 && dt.Rows[0]["TotalStockEntryCost"] != DBNull.Value
//                    ? Convert.ToDecimal(dt.Rows[0]["TotalStockEntryCost"])
//                    : 0;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalStockEntryCost: {ex.Message}");
//                return 0;
//            }
//        }

//        private class CategoryRevenue
//        {
//            public string CategoryName { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//        {
//            if (!HasData()) return new List<CategoryRevenue>();

//            string query = @"
//SELECT c.name, COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS CategoryRevenue
//FROM categories c
//LEFT JOIN products p ON p.category_id = c.id
//LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month
//GROUP BY c.id, c.name
//HAVING CategoryRevenue > 0
//ORDER BY CategoryRevenue DESC";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                List<CategoryRevenue> categoryList = new List<CategoryRevenue>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["CategoryRevenue"] != DBNull.Value ? Convert.ToDecimal(row["CategoryRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        categoryList.Add(new CategoryRevenue
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Revenue = revenue
//                        });
//                    }
//                }

//                List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
//                decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

//                if (categoryList.Count > 5)
//                {
//                    topCategories.Add(new CategoryRevenue
//                    {
//                        CategoryName = "Khác",
//                        Revenue = othersRevenue
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthers: {ex.Message}");
//                return new List<CategoryRevenue>();
//            }
//        }

//        private void DisplayRevenueByCategoryChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//            if (categoryList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Column,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 0
//            };

//            decimal maxRevenue = 0;
//            foreach (var category in categoryList)
//            {
//                decimal revenueInMillions = category.Revenue / 1000000;
//                series.Points.AddXY(category.CategoryName, revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.BorderColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);

//            foreach (DataPoint point in series.Points)
//            {
//                point["PixelPointWidth"] = "40";
//            }
//        }

//        private class WeeklyRevenue
//        {
//            public int WeekNumber { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<WeeklyRevenue> GetWeeklyRevenue()
//        {
//            if (!HasData()) return new List<WeeklyRevenue>();

//            string query = @"
//SELECT FLOOR((DAY(seh.exit_date) - 1) / 7) + 1 AS WeekNumber,
//COALESCE(SUM(sed.quantity * sed.unit_price), 0) AS WeeklyRevenue
//FROM stock_exit_headers seh
//INNER JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }

//            query += @"
//GROUP BY FLOOR((DAY(seh.exit_date) - 1) / 7) + 1
//ORDER BY WeekNumber";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            List<WeeklyRevenue> weeklyList = new List<WeeklyRevenue>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal revenue = row["WeeklyRevenue"] != DBNull.Value ? Convert.ToDecimal(row["WeeklyRevenue"]) : 0;
//                    if (revenue > 0)
//                    {
//                        weeklyList.Add(new WeeklyRevenue
//                        {
//                            WeekNumber = Convert.ToInt32(row["WeekNumber"]),
//                            Revenue = revenue
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetWeeklyRevenue: {ex.Message}");
//                return new List<WeeklyRevenue>();
//            }

//            for (int i = 1; i <= 5; i++)
//            {
//                if (!weeklyList.Any(w => w.WeekNumber == i))
//                {
//                    weeklyList.Add(new WeeklyRevenue { WeekNumber = i, Revenue = 0 });
//                }
//            }

//            return weeklyList.OrderBy(w => w.WeekNumber).ToList();
//        }

//        private void DisplayWeeklyRevenueChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Tuần";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<WeeklyRevenue> weeklyList = GetWeeklyRevenue();
//            if (weeklyList.Count == 0 || weeklyList.All(w => w.Revenue == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 2
//            };

//            decimal maxRevenue = 0;
//            foreach (var week in weeklyList)
//            {
//                decimal revenueInMillions = week.Revenue / 1000000;
//                series.Points.AddXY($"Tuần {week.WeekNumber}", revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Tuần";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class DailyProductSales
//        {
//            public int Day { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<DailyProductSales> GetDailyProductSales()
//        {
//            if (!HasData()) return new List<DailyProductSales>();

//            string query = @"
//SELECT DATE(seh.exit_date) AS SaleDate,
//       DAY(seh.exit_date) AS SaleDay,
//       COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//FROM stock_exit_headers seh
//INNER JOIN stock_exit_details sed ON sed.stock_exit_id = seh.id
//INNER JOIN products p ON sed.product_id = p.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month
//AND p.id = ?ProductId";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth),
//                new MySqlParameter("?ProductId", selectedProductId)
//            };

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }

//            query += @"
//GROUP BY DATE(seh.exit_date), DAY(seh.exit_date)
//ORDER BY SaleDate";

//            List<DailyProductSales> dailySales = new List<DailyProductSales>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        dailySales.Add(new DailyProductSales
//                        {
//                            Day = Convert.ToInt32(row["SaleDay"]),
//                            Quantity = quantity
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetDailyProductSales: {ex.Message}");
//                return new List<DailyProductSales>();
//            }

//            int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
//            for (int day = 1; day <= daysInMonth; day++)
//            {
//                if (!dailySales.Any(s => s.Day == day))
//                {
//                    dailySales.Add(new DailyProductSales { Day = day, Quantity = 0 });
//                }
//            }

//            return dailySales.OrderBy(s => s.Day).ToList();
//        }

//        private void DisplayProductSalesTrendChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = $"Xu Hướng Bán Hàng - {products[cmbProductFilter.SelectedIndex].Value}";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<DailyProductSales> dailySales = GetDailyProductSales();
//            if (dailySales.Count == 0 || dailySales.All(s => s.Quantity == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Số Lượng Bán")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(231, 76, 60),
//                BorderWidth = 2
//            };

//            decimal maxQuantity = 0;
//            foreach (var sale in dailySales)
//            {
//                series.Points.AddXY($"Ngày {sale.Day}", sale.Quantity);
//                if (sale.Quantity > maxQuantity) maxQuantity = sale.Quantity;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(231, 76, 60);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Ngày trong tháng";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Số lượng";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
//            chartRevenueByCategory.ChartAreas[0].AxisX.Interval = 1;

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class ProductSales
//        {
//            public string ProductName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<ProductSales> GetTop6ProductsBySales()
//        {
//            if (!HasData()) return new List<ProductSales>();

//            string query = @"
//SELECT p.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//FROM products p
//INNER JOIN stock_exit_details sed ON sed.product_id = p.id
//INNER JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month";

//            if (selectedCategoryId > 0)
//            {
//                query += " AND p.category_id = ?CategoryId";
//            }
//            if (selectedProductId > 0)
//            {
//                query += " AND p.id = ?ProductId";
//            }

//            query += @"
//GROUP BY p.id, p.name
//HAVING TotalQuantity > 0
//ORDER BY TotalQuantity DESC
//LIMIT 6";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            if (selectedCategoryId > 0)
//            {
//                parameters.Add(new MySqlParameter("?CategoryId", selectedCategoryId));
//            }
//            if (selectedProductId > 0)
//            {
//                parameters.Add(new MySqlParameter("?ProductId", selectedProductId));
//            }

//            List<ProductSales> productSalesList = new List<ProductSales>();
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        productSalesList.Add(new ProductSales
//                        {
//                            ProductName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop6ProductsBySales: {ex.Message}");
//                return new List<ProductSales>();
//            }

//            return productSalesList;
//        }

//        private void DisplayProductRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            List<ProductSales> productSalesList = GetTop6ProductsBySales();
//            if (productSalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var product in productSalesList)
//            {
//                double percentage = Math.Round((double)(product.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = product.ProductName;
//                point.YValues = new double[] { Convert.ToDouble(product.Quantity) };
//                point.LegendText = $"{product.ProductName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }

//        private class CategorySales
//        {
//            public string CategoryName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
//        {
//            if (!HasData()) return new List<CategorySales>();

//            string query = @"
//SELECT c.name, COALESCE(SUM(sed.quantity), 0) AS TotalQuantity
//FROM categories c
//LEFT JOIN products p ON p.category_id = c.id
//LEFT JOIN stock_exit_details sed ON sed.product_id = p.id
//LEFT JOIN stock_exit_headers seh ON sed.stock_exit_id = seh.id
//WHERE YEAR(seh.exit_date) = ?Year
//AND MONTH(seh.exit_date) = ?Month
//GROUP BY c.id, c.name
//HAVING TotalQuantity > 0
//ORDER BY TotalQuantity DESC";

//            var parameters = new List<MySqlParameter>
//            {
//                new MySqlParameter("?Year", selectedYear),
//                new MySqlParameter("?Month", selectedMonth)
//            };

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());

//                List<CategorySales> categorySalesList = new List<CategorySales>();
//                foreach (DataRow row in dt.Rows)
//                {
//                    decimal quantity = row["TotalQuantity"] != DBNull.Value ? Convert.ToDecimal(row["TotalQuantity"]) : 0;
//                    if (quantity > 0)
//                    {
//                        categorySalesList.Add(new CategorySales
//                        {
//                            CategoryName = row["name"].ToString(),
//                            Quantity = quantity
//                        });
//                    }
//                }

//                List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
//                decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

//                if (categorySalesList.Count > 5)
//                {
//                    topCategories.Add(new CategorySales
//                    {
//                        CategoryName = "Khác",
//                        Quantity = othersQuantity
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthersForSales: {ex.Message}");
//                return new List<CategorySales>();
//            }
//        }

//        private void DisplayCategorySalesRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
//            if (categorySalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ")
//            {
//                ChartType = SeriesChartType.Pie,
//                IsValueShownAsLabel = true,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                LabelForeColor = Color.White
//            };

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Alignment = StringAlignment.Center,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            decimal totalQuantity = categorySalesList.Sum(c => c.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Color[] colors = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var category in categorySalesList)
//            {
//                double percentage = Math.Round((double)(category.Quantity / totalQuantity) * 100, 1);
//                DataPoint point = new DataPoint
//                {
//                    AxisLabel = category.CategoryName,
//                    YValues = new double[] { (double)category.Quantity },
//                    LegendText = $"{category.CategoryName} ({percentage}%)",
//                    Label = $"{percentage}%",
//                    Color = colors[colorIndex % colors.Length]
//                };
//                series.Points.Add(point);
//                colorIndex++;
//            }

//            chartProductRatio.Series.Add(series);

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//        }
//    }
//}

// Tính năng ok nhưng còn lỗi
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.Windows.Forms.DataVisualization.Charting;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ReportForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private FlowLayoutPanel flpSummaryCards;
//        private TableLayoutPanel tblCharts;
//        private Label lblTotalRevenue;
//        private Label lblProfit;
//        private Label lblProfitRatio;
//        private ComboBox cmbMonthFilter;
//        private ComboBox cmbCategoryFilter;
//        private ComboBox cmbProductFilter;
//        private int selectedYear;
//        private int selectedMonth;
//        private int selectedCategoryId;
//        private int selectedProductId;
//        private System.Windows.Forms.Timer dataUpdateTimer;
//        private DateTime lastUpdateTime;
//        private Label lblTitle;
//        private Chart chartRevenueByCategory;
//        private Chart chartProductRatio;
//        private Label lblColumnChartTitle;
//        private Panel pnlPieChart;
//        private bool hasAccessPermission = false;
//        private List<KeyValuePair<int, string>> categories;
//        private List<KeyValuePair<int, string>> products;

//        public ReportForm() : base()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        public ReportForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                LoadCategories();
//                LoadProducts();
//                InitializeComponents();
//                InitializeTimer();
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void LoadCategories()
//        {
//            categories = new List<KeyValuePair<int, string>>();
//            string query = "SELECT id, name FROM categories ORDER BY name";
//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query);
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//                foreach (DataRow row in dt.Rows)
//                {
//                    int id = Convert.ToInt32(row["id"]);
//                    string name = row["name"].ToString();
//                    categories.Add(new KeyValuePair<int, string>(id, name));
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in LoadCategories: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Query: {query}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
//            }
//        }

//        private void LoadProducts()
//        {
//            products = new List<KeyValuePair<int, string>>();
//            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));
//        }

//        private void UpdateProductFilter()
//        {
//            cmbProductFilter.Items.Clear();
//            products.Clear();
//            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));

//            if (selectedCategoryId > 0)
//            {
//                string query = $"SELECT id, name FROM products WHERE category_id = {selectedCategoryId} ORDER BY name";
//                try
//                {
//                    DataTable dt = DatabaseHelper.ExecuteQuery(query);
//                    foreach (DataRow row in dt.Rows)
//                    {
//                        int id = Convert.ToInt32(row["id"]);
//                        string name = row["name"].ToString();
//                        products.Add(new KeyValuePair<int, string>(id, name));
//                    }
//                }
//                catch (Exception ex)
//                {
//                    System.Diagnostics.Debug.WriteLine($"Error in UpdateProductFilter: {ex.Message}");
//                    System.Diagnostics.Debug.WriteLine($"Query: {query}");
//                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                }
//            }

//            foreach (var product in products)
//            {
//                cmbProductFilter.Items.Add(product.Value);
//            }
//            cmbProductFilter.SelectedIndex = 0;
//            selectedProductId = 0;
//            cmbProductFilter.Enabled = selectedCategoryId > 0;
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1 || RoleId == 3);
//        }

//        private void ShowAccessDeniedMessage()
//        {
//            this.Controls.Clear();

//            Label lblAccessDenied = new Label
//            {
//                Text = "Không có quyền truy cập!",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.Red,
//                AutoSize = true,
//                TextAlign = ContentAlignment.MiddleCenter
//            };

//            Panel pnlAccessDenied = new Panel
//            {
//                Dock = DockStyle.Fill
//            };

//            lblAccessDenied.Location = new Point(
//                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//            );

//            pnlAccessDenied.Controls.Add(lblAccessDenied);
//            this.Controls.Add(pnlAccessDenied);

//            this.Resize += (sender, e) =>
//            {
//                lblAccessDenied.Location = new Point(
//                    (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
//                    (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
//                );
//            };
//        }

//        private void InitializeComponents()
//        {
//            this.Text = "Báo Cáo";
//            this.Size = new Size(1200, 800);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;

//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 4,
//                ColumnCount = 1,
//                Padding = new Padding(260, 40, 10, 10),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header
//            Panel pnlHeader = new Panel
//            {
//                BackColor = Color.FromArgb(52, 152, 219),
//                Dock = DockStyle.Top,
//                Height = 60
//            };

//            lblTitle = new Label
//            {
//                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
//                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(20, 15),
//                TextAlign = ContentAlignment.MiddleLeft
//            };
//            pnlHeader.Controls.Add(lblTitle);
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Filter Panel
//            Panel pnlFilter = new Panel
//            {
//                BackColor = Color.White,
//                Margin = new Padding(0, 10, 0, 10),
//                Padding = new Padding(15),
//                Height = 60,
//                Dock = DockStyle.Fill
//            };

//            FlowLayoutPanel flpFilter = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                FlowDirection = FlowDirection.LeftToRight,
//                Dock = DockStyle.Fill
//            };

//            Label lblMonthFilter = new Label
//            {
//                Text = "Tháng:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(0, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblMonthFilter);

//            cmbMonthFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 150,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            for (int month = 1; month <= 12; month++)
//            {
//                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
//            }
//            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
//            selectedYear = 2025;
//            selectedMonth = DateTime.Now.Month;
//            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbMonthFilter);

//            Label lblCategoryFilter = new Label
//            {
//                Text = "Danh mục:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblCategoryFilter);

//            cmbCategoryFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat
//            };

//            foreach (var category in categories)
//            {
//                cmbCategoryFilter.Items.Add(category.Value);
//            }
//            cmbCategoryFilter.SelectedIndex = 0;
//            selectedCategoryId = 0;
//            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbCategoryFilter);

//            Label lblProductFilter = new Label
//            {
//                Text = "Sản phẩm:",
//                Font = new Font("Segoe UI", 10F),
//                AutoSize = true,
//                Margin = new Padding(20, 5, 5, 0)
//            };
//            flpFilter.Controls.Add(lblProductFilter);

//            cmbProductFilter = new ComboBox
//            {
//                DropDownStyle = ComboBoxStyle.DropDownList,
//                Width = 200,
//                Font = new Font("Segoe UI", 10F),
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 20, 0),
//                FlatStyle = FlatStyle.Flat,
//                Enabled = false
//            };

//            foreach (var product in products)
//            {
//                cmbProductFilter.Items.Add(product.Value);
//            }
//            cmbProductFilter.SelectedIndex = 0;
//            selectedProductId = 0;
//            cmbProductFilter.SelectedIndexChanged += CmbProductFilter_SelectedIndexChanged;
//            flpFilter.Controls.Add(cmbProductFilter);

//            Button btnExport = new Button
//            {
//                Text = "Xuất báo cáo",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(120, 30),
//                Margin = new Padding(40, 0, 0, 0),
//                Cursor = Cursors.Hand
//            };
//            btnExport.FlatAppearance.BorderSize = 0;
//            btnExport.Click += BtnExport_Click;
//            flpFilter.Controls.Add(btnExport);

//            pnlFilter.Controls.Add(flpFilter);
//            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

//            // Summary Cards - Centered Horizontally
//            Panel pnlSummaryCardsContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(240, 242, 245),
//                Margin = new Padding(0, 0, 0, 20),
//                Padding = new Padding(10)
//            };

//            flpSummaryCards = new FlowLayoutPanel
//            {
//                AutoSize = true,
//                AutoSizeMode = AutoSizeMode.GrowAndShrink,
//                FlowDirection = FlowDirection.LeftToRight,
//                WrapContents = false,
//                Anchor = AnchorStyles.Top,
//                BackColor = Color.FromArgb(240, 242, 245)
//            };

//            flpSummaryCards.Location = new Point(
//                (pnlSummaryCardsContainer.Width - flpSummaryCards.Width) / 2,
//                10
//            );

//            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
//            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardRevenue);

//            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
//            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfit);

//            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ lợi nhuận", "0%", Color.FromArgb(230, 126, 34));
//            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
//            flpSummaryCards.Controls.Add(cardProfitRatio);

//            pnlSummaryCardsContainer.Controls.Add(flpSummaryCards);
//            tblMainContainer.Controls.Add(pnlSummaryCardsContainer, 0, 2);

//            pnlSummaryCardsContainer.Resize += (sender, e) =>
//            {
//                int totalWidth = flpSummaryCards.Controls.Count * 320 + (flpSummaryCards.Controls.Count - 1) * 10;
//                int leftMargin = Math.Max(0, (pnlSummaryCardsContainer.Width - totalWidth) / 2);
//                flpSummaryCards.Location = new Point(leftMargin, 10);
//                pnlSummaryCardsContainer.Invalidate();
//                flpSummaryCards.Invalidate();
//            };

//            // Charts Container
//            tblCharts = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 1,
//                ColumnCount = 2,
//                Margin = new Padding(0),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//            tblMainContainer.Controls.Add(tblCharts, 0, 3);

//            // Column/Line Chart Panel
//            Panel pnlColumnChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 5, 0),
//                Padding = new Padding(15)
//            };

//            lblColumnChartTitle = new Label
//            {
//                Text = "Doanh Thu Theo Danh Mục",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlColumnChart.Controls.Add(lblColumnChartTitle);

//            chartRevenueByCategory = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea columnChartArea = new ChartArea();
//            columnChartArea.AxisX.MajorGrid.Enabled = false;
//            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
//            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
//            columnChartArea.BackColor = Color.White;
//            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
//            pnlColumnChart.Controls.Add(chartRevenueByCategory);

//            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

//            // Pie Chart Panel
//            pnlPieChart = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(5, 0, 0, 0),
//                Padding = new Padding(15),
//                Visible = true
//            };

//            Label lblPieChartTitle = new Label
//            {
//                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
//                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(44, 62, 80),
//                AutoSize = true,
//                Margin = new Padding(0, 0, 0, 10),
//                Anchor = AnchorStyles.Top | AnchorStyles.Left
//            };
//            pnlPieChart.Controls.Add(lblPieChartTitle);

//            chartProductRatio = new Chart
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 30, 0, 0),
//                BorderlineWidth = 0
//            };
//            ChartArea pieChartArea = new ChartArea();
//            pieChartArea.BackColor = Color.White;
//            chartProductRatio.ChartAreas.Add(pieChartArea);
//            pnlPieChart.Controls.Add(chartProductRatio);

//            tblCharts.Controls.Add(pnlPieChart, 1, 0);

//            UpdateReport();
//        }

//        private Panel CreateSummaryCard(string title, string value, Color accentColor)
//        {
//            Panel card = new Panel
//            {
//                Width = 320,
//                Height = 100,
//                BackColor = accentColor,
//                Margin = new Padding(5),
//                BorderStyle = BorderStyle.None
//            };

//            Label lblTitle = new Label
//            {
//                Text = title,
//                Font = new Font("Segoe UI", 11F),
//                ForeColor = Color.White,
//                AutoSize = true,
//                Location = new Point(15, 15)
//            };

//            Label lblValue = new Label
//            {
//                Name = "lblValue",
//                Text = value,
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                AutoSize = true,
//                ForeColor = Color.White,
//                Location = new Point(15, 40)
//            };

//            card.Controls.Add(lblTitle);
//            card.Controls.Add(lblValue);

//            return card;
//        }

//        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            string selected = cmbMonthFilter.SelectedItem.ToString();
//            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
//            selectedYear = int.Parse(selected.Split('/')[1]);
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
//            UpdateProductFilter();
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void CmbProductFilter_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            selectedProductId = products[cmbProductFilter.SelectedIndex].Key;
//            UpdateTitle();
//            lastUpdateTime = DateTime.MinValue;
//            UpdateReport();
//        }

//        private void UpdateTitle()
//        {
//            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
//            string productName = products[cmbProductFilter.SelectedIndex].Value;
//            if (selectedProductId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName} - {productName}";
//            }
//            else if (selectedCategoryId > 0)
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
//            }
//            else
//            {
//                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
//            }
//        }

//        private void BtnExport_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        private void InitializeTimer()
//        {
//            dataUpdateTimer = new System.Windows.Forms.Timer
//            {
//                Interval = 30000
//            };
//            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
//            dataUpdateTimer.Start();

//            lastUpdateTime = GetLastUpdateTime();
//        }

//        private DateTime GetLastUpdateTime()
//        {
//            DateTime maxUpdateTime = DateTime.MinValue;

//            try
//            {
//                string queryHeaders = "SELECT id, updated_at FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetLastUpdateTime.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime updatedAt = headerRow["updated_at"] != DBNull.Value ? Convert.ToDateTime(headerRow["updated_at"]) : DateTime.MinValue;
//                    int stockExitId = Convert.ToInt32(headerRow["id"]);

//                    string queryDetails = $"SELECT product_id FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                    DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                    foreach (DataRow detailRow in detailsDt.Rows)
//                    {
//                        int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;

//                        string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                        DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                        if (productsDt.Rows.Count > 0)
//                        {
//                            int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

//                            bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
//                            bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

//                            if (matchesCategory && matchesProduct && updatedAt > maxUpdateTime)
//                            {
//                                maxUpdateTime = updatedAt;
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetLastUpdateTime: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }

//            return maxUpdateTime;
//        }

//        private void DataUpdateTimer_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                DateTime currentUpdateTime = GetLastUpdateTime();
//                if (currentUpdateTime > lastUpdateTime)
//                {
//                    lastUpdateTime = currentUpdateTime;
//                    UpdateReport();
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in DataUpdateTimer_Tick: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }
//        }

//        private void UpdateReport()
//        {
//            bool hasData = HasData();
//            if (!hasData)
//            {
//                lblTotalRevenue.Text = "0 VND";
//                lblProfit.Text = "0 VND";
//                lblProfitRatio.Text = "0%";
//                DisplayNoDataCharts();
//                pnlPieChart.Visible = selectedProductId == 0;
//                AdjustChartLayout();
//                return;
//            }

//            decimal totalRevenue = CalculateTotalRevenue();
//            decimal totalStockEntryCost = CalculateTotalStockEntryCost();
//            decimal profit = totalRevenue - totalStockEntryCost;
//            decimal profitRatio = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

//            lblTotalRevenue.Text = FormatCurrency(totalRevenue);
//            lblProfit.Text = FormatCurrency(profit);
//            lblProfitRatio.Text = FormatPercentage(profitRatio);

//            pnlPieChart.Visible = selectedProductId == 0;
//            AdjustChartLayout();

//            if (selectedProductId > 0)
//            {
//                DisplayProductSalesTrendChart();
//            }
//            else if (selectedCategoryId == 0)
//            {
//                DisplayRevenueByCategoryChart();
//            }
//            else
//            {
//                DisplayWeeklyRevenueChart();
//            }

//            if (pnlPieChart.Visible)
//            {
//                if (selectedCategoryId == 0)
//                {
//                    DisplayCategorySalesRatioChart();
//                }
//                else
//                {
//                    DisplayProductRatioChart();
//                }
//            }
//        }

//        private void AdjustChartLayout()
//        {
//            if (pnlPieChart.Visible)
//            {
//                if (tblCharts.ColumnCount == 1)
//                {
//                    tblCharts.ColumnCount = 2;
//                    tblCharts.ColumnStyles.Clear();
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
//                    tblCharts.Controls.Add(pnlPieChart, 1, 0);
//                }
//                pnlPieChart.Show();
//            }
//            else
//            {
//                if (tblCharts.ColumnCount == 2)
//                {
//                    tblCharts.ColumnCount = 1;
//                    tblCharts.ColumnStyles.Clear();
//                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
//                    tblCharts.Controls.Remove(pnlPieChart);
//                }
//                pnlPieChart.Hide();
//            }
//        }

//        private bool HasData()
//        {
//            try
//            {
//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for HasData.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

//                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
//                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

//                                if (matchesCategory && matchesProduct)
//                                {
//                                    return true;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in HasData: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }

//            return false;
//        }

//        private void DisplayNoDataCharts()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";
//            Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartRevenueByCategory.Titles.Add(noDataTitle);

//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();
//            Title noDataTitlePie = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//            chartProductRatio.Titles.Add(noDataTitlePie);
//        }

//        private string FormatCurrency(decimal value)
//        {
//            return String.Format("{0:n0} VND", value);
//        }

//        private string FormatPercentage(decimal value)
//        {
//            return String.Format("{0:F2}%", value);
//        }

//        private decimal CalculateTotalRevenue()
//        {
//            decimal totalRevenue = 0;

//            try
//            {
//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for CalculateTotalRevenue.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
//                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

//                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
//                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

//                                if (matchesCategory && matchesProduct)
//                                {
//                                    totalRevenue += quantity * unitPrice;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalRevenue: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }

//            return totalRevenue;
//        }

//        private decimal CalculateTotalStockEntryCost()
//        {
//            decimal totalCost = 0;

//            try
//            {
//                string queryHeaders = "SELECT id, updated_at FROM stock_entry_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_entry_headers for CalculateTotalStockEntryCost.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime updatedAt = headerRow["updated_at"] != DBNull.Value ? Convert.ToDateTime(headerRow["updated_at"]) : DateTime.MinValue;
//                    if (updatedAt.Year == selectedYear && updatedAt.Month == selectedMonth)
//                    {
//                        int stockEntryId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, total_price FROM stock_entry_details WHERE stock_entry_id = {stockEntryId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal totalPrice = detailRow["total_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["total_price"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

//                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
//                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

//                                if (matchesCategory && matchesProduct)
//                                {
//                                    totalCost += totalPrice;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalStockEntryCost: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//            }

//            return totalCost;
//        }

//        private class CategoryRevenue
//        {
//            public string CategoryName { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
//        {
//            Dictionary<int, decimal> categoryRevenues = new Dictionary<int, decimal>();
//            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

//            try
//            {
//                // Lấy tất cả categories
//                string queryCategories = "SELECT id, name FROM categories";
//                DataTable categoriesDt = DatabaseHelper.ExecuteQuery(queryCategories);
//                foreach (DataRow row in categoriesDt.Rows)
//                {
//                    int categoryId = Convert.ToInt32(row["id"]);
//                    string categoryName = row["name"].ToString();
//                    categoryNames[categoryId] = categoryName;
//                    categoryRevenues[categoryId] = 0;
//                }

//                // Duyệt qua stock_exit_headers
//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop5CategoriesAndOthers.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
//                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
//                                if (categoryRevenues.ContainsKey(categoryId))
//                                {
//                                    categoryRevenues[categoryId] += quantity * unitPrice;
//                                }
//                            }
//                        }
//                    }
//                }

//                // Chuyển thành danh sách và lấy top 5
//                List<CategoryRevenue> categoryList = categoryRevenues
//                    .Where(kv => kv.Value > 0)
//                    .Select(kv => new CategoryRevenue
//                    {
//                        CategoryName = categoryNames.ContainsKey(kv.Key) ? categoryNames[kv.Key] : "Unknown",
//                        Revenue = kv.Value
//                    })
//                    .OrderByDescending(cr => cr.Revenue)
//                    .ToList();

//                List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
//                decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

//                if (categoryList.Count > 5)
//                {
//                    topCategories.Add(new CategoryRevenue
//                    {
//                        CategoryName = "Khác",
//                        Revenue = othersRevenue
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthers: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                return new List<CategoryRevenue>();
//            }
//        }

//        private void DisplayRevenueByCategoryChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
//            if (categoryList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Column,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 0
//            };

//            decimal maxRevenue = 0;
//            foreach (var category in categoryList)
//            {
//                decimal revenueInMillions = category.Revenue / 1000000;
//                series.Points.AddXY(category.CategoryName, revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.BorderColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);

//            foreach (DataPoint point in series.Points)
//            {
//                point["PixelPointWidth"] = "40";
//            }
//        }

//        private class WeeklyRevenue
//        {
//            public int WeekNumber { get; set; }
//            public decimal Revenue { get; set; }
//        }

//        private List<WeeklyRevenue> GetWeeklyRevenue()
//        {
//            Dictionary<int, decimal> weeklyRevenues = new Dictionary<int, decimal>();

//            try
//            {
//                // Khởi tạo các tuần
//                for (int i = 1; i <= 5; i++)
//                {
//                    weeklyRevenues[i] = 0;
//                }

//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetWeeklyRevenue.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int weekNumber = (exitDate.Day - 1) / 7 + 1;
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
//                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

//                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;

//                                if (matchesCategory)
//                                {
//                                    weeklyRevenues[weekNumber] += quantity * unitPrice;
//                                }
//                            }
//                        }
//                    }
//                }

//                return weeklyRevenues.Select(kv => new WeeklyRevenue
//                {
//                    WeekNumber = kv.Key,
//                    Revenue = kv.Value
//                }).OrderBy(w => w.WeekNumber).ToList();
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetWeeklyRevenue: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                return new List<WeeklyRevenue>();
//            }
//        }

//        private void DisplayWeeklyRevenueChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = "Doanh Thu Theo Tuần";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<WeeklyRevenue> weeklyList = GetWeeklyRevenue();
//            if (weeklyList.Count == 0 || weeklyList.All(w => w.Revenue == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Doanh Thu")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(52, 152, 219),
//                BorderWidth = 2
//            };

//            decimal maxRevenue = 0;
//            foreach (var week in weeklyList)
//            {
//                decimal revenueInMillions = week.Revenue / 1000000;
//                series.Points.AddXY($"Tuần {week.WeekNumber}", revenueInMillions);
//                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.Color = Color.FromArgb(52, 152, 219);
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(52, 152, 219);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Tuần";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}M";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class DailyProductSales
//        {
//            public int Day { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<DailyProductSales> GetDailyProductSales()
//        {
//            List<DailyProductSales> dailySales = new List<DailyProductSales>();

//            try
//            {
//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetDailyProductSales.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);
//                        System.Diagnostics.Debug.WriteLine($"Retrieved {detailsDt.Rows.Count} records from stock_exit_details for stock_exit_id = {stockExitId}.");

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

//                            if (productId == selectedProductId)
//                            {
//                                string queryProducts = $"SELECT category_id, name FROM products WHERE id = {productId}";
//                                DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                                System.Diagnostics.Debug.WriteLine($"Retrieved {productsDt.Rows.Count} records from products for product_id = {productId}.");

//                                if (productsDt.Rows.Count > 0)
//                                {
//                                    int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
//                                    if (selectedCategoryId == 0 || categoryId == selectedCategoryId)
//                                    {
//                                        dailySales.Add(new DailyProductSales
//                                        {
//                                            Day = exitDate.Day,
//                                            Quantity = quantity
//                                        });
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }

//                // Điền các ngày thiếu trong tháng
//                int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
//                for (int day = 1; day <= daysInMonth; day++)
//                {
//                    if (!dailySales.Any(s => s.Day == day))
//                    {
//                        dailySales.Add(new DailyProductSales { Day = day, Quantity = 0 });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetDailyProductSales: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                return new List<DailyProductSales>();
//            }

//            return dailySales.OrderBy(s => s.Day).ToList();
//        }

//        private void DisplayProductSalesTrendChart()
//        {
//            chartRevenueByCategory.Series.Clear();
//            chartRevenueByCategory.Titles.Clear();
//            lblColumnChartTitle.Text = $"Xu Hướng Bán Hàng - {products[cmbProductFilter.SelectedIndex].Value}";

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            List<DailyProductSales> dailySales = GetDailyProductSales();
//            if (dailySales.Count == 0 || dailySales.All(s => s.Quantity == 0))
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartRevenueByCategory.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Số Lượng Bán")
//            {
//                ChartType = SeriesChartType.Line,
//                Color = Color.FromArgb(231, 76, 60),
//                BorderWidth = 2
//            };

//            decimal maxQuantity = 0;
//            foreach (var sale in dailySales)
//            {
//                series.Points.AddXY($"Ngày {sale.Day}", sale.Quantity);
//                if (sale.Quantity > maxQuantity) maxQuantity = sale.Quantity;

//                DataPoint point = series.Points[series.Points.Count - 1];
//                point.MarkerStyle = MarkerStyle.Circle;
//                point.MarkerSize = 8;
//                point.MarkerColor = Color.FromArgb(231, 76, 60);
//            }

//            chartRevenueByCategory.Series.Add(series);

//            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Ngày trong tháng";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Số lượng";
//            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}";
//            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) : 10;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) * 5 : 50;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
//            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
//            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
//            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
//            chartRevenueByCategory.ChartAreas[0].AxisX.Interval = 1;

//            series.IsValueShownAsLabel = true;
//            series.LabelFormat = "{0}";
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.FromArgb(44, 62, 80);
//        }

//        private class ProductSales
//        {
//            public string ProductName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<ProductSales> GetTop6ProductsBySales()
//        {
//            Dictionary<int, decimal> productQuantities = new Dictionary<int, decimal>();
//            Dictionary<int, string> productNames = new Dictionary<int, string>();

//            try
//            {
//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop6ProductsBySales.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

//                            string queryProducts = $"SELECT category_id, name FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
//                                string productName = productsDt.Rows[0]["name"].ToString();

//                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
//                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

//                                if (matchesCategory && matchesProduct)
//                                {
//                                    if (!productQuantities.ContainsKey(productId))
//                                    {
//                                        productQuantities[productId] = 0;
//                                        productNames[productId] = productName;
//                                    }
//                                    productQuantities[productId] += quantity;
//                                }
//                            }
//                        }
//                    }
//                }

//                return productQuantities
//                    .Where(kv => kv.Value > 0)
//                    .Select(kv => new ProductSales
//                    {
//                        ProductName = productNames[kv.Key],
//                        Quantity = kv.Value
//                    })
//                    .OrderByDescending(ps => ps.Quantity)
//                    .Take(6)
//                    .ToList();
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop6ProductsBySales: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                return new List<ProductSales>();
//            }
//        }

//        private void DisplayProductRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            List<ProductSales> productSalesList = GetTop6ProductsBySales();
//            if (productSalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

//            decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            Color[] colorPalette = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var product in productSalesList)
//            {
//                double percentage = Math.Round((double)(product.Quantity / totalQuantity * 100), 1);

//                DataPoint point = new DataPoint();
//                point.AxisLabel = product.ProductName;
//                point.YValues = new double[] { Convert.ToDouble(product.Quantity) };
//                point.LegendText = $"{product.ProductName} ({percentage}%)";
//                point.Label = $"{percentage}%";
//                point.Color = colorPalette[colorIndex % colorPalette.Length];

//                series.Points.Add(point);
//                colorIndex++;
//            }

//            series.IsValueShownAsLabel = true;
//            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
//            series.LabelForeColor = Color.White;

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

//            chartProductRatio.Series.Add(series);
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
//        }

//        private class CategorySales
//        {
//            public string CategoryName { get; set; }
//            public decimal Quantity { get; set; }
//        }

//        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
//        {
//            Dictionary<int, decimal> categoryQuantities = new Dictionary<int, decimal>();
//            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

//            try
//            {
//                string queryCategories = "SELECT id, name FROM categories";
//                DataTable categoriesDt = DatabaseHelper.ExecuteQuery(queryCategories);
//                foreach (DataRow row in categoriesDt.Rows)
//                {
//                    int categoryId = Convert.ToInt32(row["id"]);
//                    string categoryName = row["name"].ToString();
//                    categoryNames[categoryId] = categoryName;
//                    categoryQuantities[categoryId] = 0;
//                }

//                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
//                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
//                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop5CategoriesAndOthersForSales.");

//                foreach (DataRow headerRow in headersDt.Rows)
//                {
//                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
//                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
//                    {
//                        int stockExitId = Convert.ToInt32(headerRow["id"]);

//                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
//                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

//                        foreach (DataRow detailRow in detailsDt.Rows)
//                        {
//                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
//                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

//                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
//                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
//                            if (productsDt.Rows.Count > 0)
//                            {
//                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
//                                if (categoryQuantities.ContainsKey(categoryId))
//                                {
//                                    categoryQuantities[categoryId] += quantity;
//                                }
//                            }
//                        }
//                    }
//                }

//                List<CategorySales> categorySalesList = categoryQuantities
//                    .Where(kv => kv.Value > 0)
//                    .Select(kv => new CategorySales
//                    {
//                        CategoryName = categoryNames.ContainsKey(kv.Key) ? categoryNames[kv.Key] : "Unknown",
//                        Quantity = kv.Value
//                    })
//                    .OrderByDescending(cs => cs.Quantity)
//                    .ToList();

//                List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
//                decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

//                if (categorySalesList.Count > 5)
//                {
//                    topCategories.Add(new CategorySales
//                    {
//                        CategoryName = "Khác",
//                        Quantity = othersQuantity
//                    });
//                }

//                return topCategories;
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthersForSales: {ex.Message}");
//                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
//                return new List<CategorySales>();
//            }
//        }

//        private void DisplayCategorySalesRatioChart()
//        {
//            chartProductRatio.Series.Clear();
//            chartProductRatio.Titles.Clear();
//            chartProductRatio.Legends.Clear();

//            if (!HasData())
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
//            if (categorySalesList.Count == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Series series = new Series("Tỉ Lệ")
//            {
//                ChartType = SeriesChartType.Pie,
//                IsValueShownAsLabel = true,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                LabelForeColor = Color.White
//            };

//            Legend legend = new Legend("Legend")
//            {
//                Docking = Docking.Right,
//                Alignment = StringAlignment.Center,
//                Font = new Font("Segoe UI", 9F),
//                BackColor = Color.Transparent
//            };
//            chartProductRatio.Legends.Add(legend);

//            decimal totalQuantity = categorySalesList.Sum(c => c.Quantity);
//            if (totalQuantity == 0)
//            {
//                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
//                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
//                chartProductRatio.Titles.Add(noDataTitle);
//                return;
//            }

//            Color[] colors = new Color[]
//            {
//                Color.FromArgb(41, 128, 185),
//                Color.FromArgb(231, 76, 60),
//                Color.FromArgb(46, 204, 113),
//                Color.FromArgb(155, 89, 182),
//                Color.FromArgb(243, 156, 18),
//                Color.FromArgb(127, 140, 141)
//            };

//            int colorIndex = 0;
//            foreach (var category in categorySalesList)
//            {
//                double percentage = Math.Round((double)(category.Quantity / totalQuantity) * 100, 1);
//                DataPoint point = new DataPoint
//                {
//                    AxisLabel = category.CategoryName,
//                    YValues = new double[] { (double)category.Quantity },
//                    LegendText = $"{category.CategoryName} ({percentage}%)",
//                    Label = $"{percentage}%",
//                    Color = colors[colorIndex % colors.Length]
//                };
//                series.Points.Add(point);
//                colorIndex++;
//            }

//            chartProductRatio.Series.Add(series);

//            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
//            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
//            chartProductRatio.Series[0]["PieLineColor"] = "White";
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class ReportForm : BaseForm
    {
        private TableLayoutPanel tblMainContainer;
        private FlowLayoutPanel flpSummaryCards;
        private TableLayoutPanel tblCharts;
        private Label lblTotalRevenue;
        private Label lblProfit;
        private Label lblProfitRatio;
        private ComboBox cmbMonthFilter;
        private ComboBox cmbCategoryFilter;
        private ComboBox cmbProductFilter;
        private int selectedYear;
        private int selectedMonth;
        private int selectedCategoryId;
        private int selectedProductId;
        private System.Windows.Forms.Timer dataUpdateTimer;
        private DateTime lastUpdateTime;
        private Label lblTitle;
        private Chart chartRevenueByCategory;
        private Chart chartProductRatio;
        private Label lblColumnChartTitle;
        private Panel pnlPieChart;
        private bool hasAccessPermission = false;
        private List<KeyValuePair<int, string>> categories;
        private List<KeyValuePair<int, string>> products;

        public ReportForm() : base()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public ReportForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            CheckAccessPermission();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (hasAccessPermission)
            {
                LoadCategories();
                LoadProducts();
                InitializeComponents();
                InitializeTimer();
            }
            else
            {
                ShowAccessDeniedMessage();
            }
        }

        private void LoadCategories()
        {
            categories = new List<KeyValuePair<int, string>>();
            string query = "SELECT id, name FROM categories ORDER BY name";
            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
                foreach (DataRow row in dt.Rows)
                {
                    int id = Convert.ToInt32(row["id"]);
                    string name = row["name"].ToString();
                    categories.Add(new KeyValuePair<int, string>(id, name));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoadCategories: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Query: {query}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                categories.Add(new KeyValuePair<int, string>(0, "Tất cả danh mục"));
            }
        }

        private void LoadProducts()
        {
            products = new List<KeyValuePair<int, string>>();
            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));
        }

        private void UpdateProductFilter()
        {
            cmbProductFilter.Items.Clear();
            products.Clear();
            products.Add(new KeyValuePair<int, string>(0, "Tất cả sản phẩm"));

            if (selectedCategoryId > 0)
            {
                string query = $"SELECT id, name FROM products WHERE category_id = {selectedCategoryId} ORDER BY name";
                try
                {
                    DataTable dt = DatabaseHelper.ExecuteQuery(query);
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string name = row["name"].ToString();
                        products.Add(new KeyValuePair<int, string>(id, name));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in UpdateProductFilter: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Query: {query}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }

            foreach (var product in products)
            {
                cmbProductFilter.Items.Add(product.Value);
            }
            cmbProductFilter.SelectedIndex = 0;
            selectedProductId = 0;
            cmbProductFilter.Enabled = selectedCategoryId > 0;
        }

        private void CheckAccessPermission()
        {
            hasAccessPermission = (RoleId == 1 || RoleId == 3);
        }

        private void ShowAccessDeniedMessage()
        {
            this.Controls.Clear();

            Label lblAccessDenied = new Label
            {
                Text = "Không có quyền truy cập!",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.Red,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Panel pnlAccessDenied = new Panel
            {
                Dock = DockStyle.Fill
            };

            lblAccessDenied.Location = new Point(
                (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
                (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
            );

            pnlAccessDenied.Controls.Add(lblAccessDenied);
            this.Controls.Add(pnlAccessDenied);

            this.Resize += (sender, e) =>
            {
                lblAccessDenied.Location = new Point(
                    (pnlAccessDenied.Width - lblAccessDenied.Width) / 2,
                    (pnlAccessDenied.Height - lblAccessDenied.Height) / 2
                );
            };
        }

        private void InitializeComponents()
        {
            this.Text = "Báo Cáo";
            this.Size = new Size(1200, 800);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;

            tblMainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                Padding = new Padding(260, 40, 10, 10),
                BackColor = Color.FromArgb(240, 242, 245)
            };
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(tblMainContainer);

            // Header
            Panel pnlHeader = new Panel
            {
                BackColor = Color.FromArgb(52, 152, 219),
                Dock = DockStyle.Top,
                Height = 60
            };

            lblTitle = new Label
            {
                Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 15),
                TextAlign = ContentAlignment.MiddleLeft
            };
            pnlHeader.Controls.Add(lblTitle);
            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

            // Filter Panel
            Panel pnlFilter = new Panel
            {
                BackColor = Color.White,
                Margin = new Padding(0, 10, 0, 10),
                Padding = new Padding(15),
                Height = 60,
                Dock = DockStyle.Fill
            };

            FlowLayoutPanel flpFilter = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill
            };

            Label lblMonthFilter = new Label
            {
                Text = "Tháng:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(0, 5, 5, 0)
            };
            flpFilter.Controls.Add(lblMonthFilter);

            cmbMonthFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 150,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0),
                FlatStyle = FlatStyle.Flat
            };

            for (int month = 1; month <= 12; month++)
            {
                cmbMonthFilter.Items.Add($"Tháng {month}/2025");
            }
            cmbMonthFilter.SelectedIndex = DateTime.Now.Month - 1;
            selectedYear = 2025;
            selectedMonth = DateTime.Now.Month;
            cmbMonthFilter.SelectedIndexChanged += CmbMonthFilter_SelectedIndexChanged;
            flpFilter.Controls.Add(cmbMonthFilter);

            Label lblCategoryFilter = new Label
            {
                Text = "Danh mục:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(20, 5, 5, 0)
            };
            flpFilter.Controls.Add(lblCategoryFilter);

            cmbCategoryFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0),
                FlatStyle = FlatStyle.Flat
            };

            foreach (var category in categories)
            {
                cmbCategoryFilter.Items.Add(category.Value);
            }
            cmbCategoryFilter.SelectedIndex = 0;
            selectedCategoryId = 0;
            cmbCategoryFilter.SelectedIndexChanged += CmbCategoryFilter_SelectedIndexChanged;
            flpFilter.Controls.Add(cmbCategoryFilter);

            Label lblProductFilter = new Label
            {
                Text = "Sản phẩm:",
                Font = new Font("Segoe UI", 10F),
                AutoSize = true,
                Margin = new Padding(20, 5, 5, 0)
            };
            flpFilter.Controls.Add(lblProductFilter);

            cmbProductFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 20, 0),
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };

            foreach (var product in products)
            {
                cmbProductFilter.Items.Add(product.Value);
            }
            cmbProductFilter.SelectedIndex = 0;
            selectedProductId = 0;
            cmbProductFilter.SelectedIndexChanged += CmbProductFilter_SelectedIndexChanged;
            flpFilter.Controls.Add(cmbProductFilter);

            Button btnExport = new Button
            {
                Text = "Xuất báo cáo",
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 30),
                Margin = new Padding(40, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += BtnExport_Click;
            flpFilter.Controls.Add(btnExport);

            pnlFilter.Controls.Add(flpFilter);
            tblMainContainer.Controls.Add(pnlFilter, 0, 1);

            // Summary Cards - Centered Horizontally
            Panel pnlSummaryCardsContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 242, 245),
                Margin = new Padding(0, 0, 0, 20),
                Padding = new Padding(10)
            };

            flpSummaryCards = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Anchor = AnchorStyles.Top,
                BackColor = Color.FromArgb(240, 242, 245)
            };

            flpSummaryCards.Location = new Point(
                (pnlSummaryCardsContainer.Width - flpSummaryCards.Width) / 2,
                10
            );

            Panel cardRevenue = CreateSummaryCard("Tổng doanh thu", "0 VND", Color.FromArgb(52, 152, 219));
            lblTotalRevenue = (Label)cardRevenue.Controls.Find("lblValue", true)[0];
            flpSummaryCards.Controls.Add(cardRevenue);

            Panel cardProfit = CreateSummaryCard("Lợi nhuận", "0 VND", Color.FromArgb(46, 204, 113));
            lblProfit = (Label)cardProfit.Controls.Find("lblValue", true)[0];
            flpSummaryCards.Controls.Add(cardProfit);

            Panel cardProfitRatio = CreateSummaryCard("Tỉ lệ lợi nhuận", "0%", Color.FromArgb(230, 126, 34));
            lblProfitRatio = (Label)cardProfitRatio.Controls.Find("lblValue", true)[0];
            flpSummaryCards.Controls.Add(cardProfitRatio);

            pnlSummaryCardsContainer.Controls.Add(flpSummaryCards);
            tblMainContainer.Controls.Add(pnlSummaryCardsContainer, 0, 2);

            pnlSummaryCardsContainer.Resize += (sender, e) =>
            {
                int totalWidth = flpSummaryCards.Controls.Count * 320 + (flpSummaryCards.Controls.Count - 1) * 10;
                int leftMargin = Math.Max(0, (pnlSummaryCardsContainer.Width - totalWidth) / 2);
                flpSummaryCards.Location = new Point(leftMargin, 10);
                pnlSummaryCardsContainer.Invalidate();
                flpSummaryCards.Invalidate();
            };

            // Charts Container
            tblCharts = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                Margin = new Padding(0),
                BackColor = Color.FromArgb(240, 242, 245)
            };
            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tblMainContainer.Controls.Add(tblCharts, 0, 3);

            // Column/Line Chart Panel
            Panel pnlColumnChart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 5, 0),
                Padding = new Padding(15)
            };

            lblColumnChartTitle = new Label
            {
                Text = "Doanh Thu Theo Danh Mục",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlColumnChart.Controls.Add(lblColumnChartTitle);

            chartRevenueByCategory = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 30, 0, 0),
                BorderlineWidth = 0
            };
            ChartArea columnChartArea = new ChartArea();
            columnChartArea.AxisX.MajorGrid.Enabled = false;
            columnChartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            columnChartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            columnChartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
            columnChartArea.BackColor = Color.White;
            chartRevenueByCategory.ChartAreas.Add(columnChartArea);
            pnlColumnChart.Controls.Add(chartRevenueByCategory);

            tblCharts.Controls.Add(pnlColumnChart, 0, 0);

            // Pie Chart Panel
            pnlPieChart = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(5, 0, 0, 0),
                Padding = new Padding(15),
                Visible = true
            };

            Label lblPieChartTitle = new Label
            {
                Text = "Tỉ Lệ Sản Phẩm Bán Ra",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 10),
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            pnlPieChart.Controls.Add(lblPieChartTitle);

            chartProductRatio = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 30, 0, 0),
                BorderlineWidth = 0
            };
            ChartArea pieChartArea = new ChartArea();
            pieChartArea.BackColor = Color.White;
            chartProductRatio.ChartAreas.Add(pieChartArea);
            pnlPieChart.Controls.Add(chartProductRatio);

            tblCharts.Controls.Add(pnlPieChart, 1, 0);

            UpdateReport();
        }

        private Panel CreateSummaryCard(string title, string value, Color accentColor)
        {
            Panel card = new Panel
            {
                Width = 320,
                Height = 100,
                BackColor = accentColor,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.None
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11F),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 15)
            };

            Label lblValue = new Label
            {
                Name = "lblValue",
                Text = value,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoSize = true,
                ForeColor = Color.White,
                Location = new Point(15, 40)
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);

            return card;
        }

        private void CmbMonthFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbMonthFilter.SelectedItem.ToString();
            selectedMonth = int.Parse(selected.Split('/')[0].Replace("Tháng ", ""));
            selectedYear = int.Parse(selected.Split('/')[1]);
            UpdateTitle();
            lastUpdateTime = DateTime.MinValue;
            UpdateReport();
        }

        private void CmbCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCategoryId = categories[cmbCategoryFilter.SelectedIndex].Key;
            UpdateProductFilter();
            UpdateTitle();
            lastUpdateTime = DateTime.MinValue;
            UpdateReport();
        }

        private void CmbProductFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProductId = products[cmbProductFilter.SelectedIndex].Key;
            UpdateTitle();
            lastUpdateTime = DateTime.MinValue;
            UpdateReport();
        }

        private void UpdateTitle()
        {
            string categoryName = categories[cmbCategoryFilter.SelectedIndex].Value;
            string productName = products[cmbProductFilter.SelectedIndex].Value;
            if (selectedProductId > 0)
            {
                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName} - {productName}";
            }
            else if (selectedCategoryId > 0)
            {
                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear} - {categoryName}";
            }
            else
            {
                lblTitle.Text = $"Báo Cáo Thống Kê - Tháng {selectedMonth}/{selectedYear}";
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Báo cáo đã được xuất thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeTimer()
        {
            dataUpdateTimer = new System.Windows.Forms.Timer
            {
                Interval = 30000
            };
            dataUpdateTimer.Tick += DataUpdateTimer_Tick;
            dataUpdateTimer.Start();

            lastUpdateTime = GetLastUpdateTime();
        }

        private DateTime GetLastUpdateTime()
        {
            DateTime maxUpdateTime = DateTime.MinValue;

            try
            {
                string queryHeaders = "SELECT id, updated_at FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetLastUpdateTime.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime updatedAt = headerRow["updated_at"] != DBNull.Value ? Convert.ToDateTime(headerRow["updated_at"]) : DateTime.MinValue;
                    int stockExitId = Convert.ToInt32(headerRow["id"]);

                    string queryDetails = $"SELECT product_id FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                    DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                    foreach (DataRow detailRow in detailsDt.Rows)
                    {
                        int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;

                        string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                        DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                        if (productsDt.Rows.Count > 0)
                        {
                            int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

                            bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
                            bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

                            if (matchesCategory && matchesProduct && updatedAt > maxUpdateTime)
                            {
                                maxUpdateTime = updatedAt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetLastUpdateTime: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return maxUpdateTime;
        }

        private void DataUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime currentUpdateTime = GetLastUpdateTime();
                if (currentUpdateTime > lastUpdateTime)
                {
                    lastUpdateTime = currentUpdateTime;
                    UpdateReport();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DataUpdateTimer_Tick: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private void UpdateReport()
        {
            bool hasData = HasData();
            if (!hasData)
            {
                lblTotalRevenue.Text = "0 VND";
                lblProfit.Text = "0 VND";
                lblProfitRatio.Text = "0%";
                DisplayNoDataCharts();
                pnlPieChart.Visible = selectedProductId == 0;
                AdjustChartLayout();
                return;
            }

            decimal totalRevenue = CalculateTotalRevenue();
            decimal totalStockEntryCost = CalculateTotalStockEntryCost();
            decimal profit = totalRevenue - totalStockEntryCost;
            decimal profitRatio = totalRevenue > 0 ? (profit / totalRevenue) * 100 : 0;

            lblTotalRevenue.Text = FormatCurrency(totalRevenue);
            lblProfit.Text = FormatCurrency(profit);
            lblProfitRatio.Text = FormatPercentage(profitRatio);

            pnlPieChart.Visible = selectedProductId == 0;
            AdjustChartLayout();

            if (selectedProductId > 0)
            {
                DisplayProductSalesTrendChart();
            }
            else if (selectedCategoryId == 0)
            {
                DisplayRevenueByCategoryChart();
            }
            else
            {
                DisplayWeeklyRevenueChart();
            }

            if (pnlPieChart.Visible)
            {
                if (selectedCategoryId == 0)
                {
                    DisplayCategorySalesRatioChart();
                }
                else
                {
                    DisplayProductRatioChart();
                }
            }
        }

        private void AdjustChartLayout()
        {
            if (pnlPieChart.Visible)
            {
                if (tblCharts.ColumnCount == 1)
                {
                    tblCharts.ColumnCount = 2;
                    tblCharts.ColumnStyles.Clear();
                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                    tblCharts.Controls.Add(pnlPieChart, 1, 0);
                }
                pnlPieChart.Show();
            }
            else
            {
                if (tblCharts.ColumnCount == 2)
                {
                    tblCharts.ColumnCount = 1;
                    tblCharts.ColumnStyles.Clear();
                    tblCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                    tblCharts.Controls.Remove(pnlPieChart);
                }
                pnlPieChart.Hide();
            }
        }

        private bool HasData()
        {
            try
            {
                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for HasData.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

                                if (matchesCategory && matchesProduct)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in HasData: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return false;
        }

        private void DisplayNoDataCharts()
        {
            chartRevenueByCategory.Series.Clear();
            chartRevenueByCategory.Titles.Clear();
            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";
            Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
            chartRevenueByCategory.Titles.Add(noDataTitle);

            chartProductRatio.Series.Clear();
            chartProductRatio.Titles.Clear();
            chartProductRatio.Legends.Clear();
            Title noDataTitlePie = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
            chartProductRatio.Titles.Add(noDataTitlePie);
        }

        private string FormatCurrency(decimal value)
        {
            return String.Format("{0:n0} VND", value);
        }

        private string FormatPercentage(decimal value)
        {
            return String.Format("{0:F2}%", value);
        }

        private decimal CalculateTotalRevenue()
        {
            decimal totalRevenue = 0;

            try
            {
                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for CalculateTotalRevenue.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

                                if (matchesCategory && matchesProduct)
                                {
                                    totalRevenue += quantity * unitPrice;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalRevenue: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return totalRevenue;
        }

        private decimal CalculateTotalStockEntryCost()
        {
            decimal totalCost = 0;

            try
            {
                string queryHeaders = "SELECT id, updated_at FROM stock_entry_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_entry_headers for CalculateTotalStockEntryCost.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime updatedAt = headerRow["updated_at"] != DBNull.Value ? Convert.ToDateTime(headerRow["updated_at"]) : DateTime.MinValue;
                    if (updatedAt.Year == selectedYear && updatedAt.Month == selectedMonth)
                    {
                        int stockEntryId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, total_price FROM stock_entry_details WHERE stock_entry_id = {stockEntryId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal totalPrice = detailRow["total_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["total_price"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

                                if (matchesCategory && matchesProduct)
                                {
                                    totalCost += totalPrice;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CalculateTotalStockEntryCost: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }

            return totalCost;
        }

        private class CategoryRevenue
        {
            public string CategoryName { get; set; }
            public decimal Revenue { get; set; }
        }

        private List<CategoryRevenue> GetTop5CategoriesAndOthers()
        {
            Dictionary<int, decimal> categoryRevenues = new Dictionary<int, decimal>();
            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

            try
            {
                // Lấy tất cả categories
                string queryCategories = "SELECT id, name FROM categories";
                DataTable categoriesDt = DatabaseHelper.ExecuteQuery(queryCategories);
                foreach (DataRow row in categoriesDt.Rows)
                {
                    int categoryId = Convert.ToInt32(row["id"]);
                    string categoryName = row["name"].ToString();
                    categoryNames[categoryId] = categoryName;
                    categoryRevenues[categoryId] = 0;
                }

                // Duyệt qua stock_exit_headers
                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop5CategoriesAndOthers.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
                                if (categoryRevenues.ContainsKey(categoryId))
                                {
                                    categoryRevenues[categoryId] += quantity * unitPrice;
                                }
                            }
                        }
                    }
                }

                // Chuyển thành danh sách và lấy top 5
                List<CategoryRevenue> categoryList = categoryRevenues
                    .Where(kv => kv.Value > 0)
                    .Select(kv => new CategoryRevenue
                    {
                        CategoryName = categoryNames.ContainsKey(kv.Key) ? categoryNames[kv.Key] : "Unknown",
                        Revenue = kv.Value
                    })
                    .OrderByDescending(cr => cr.Revenue)
                    .ToList();

                List<CategoryRevenue> topCategories = categoryList.Take(5).ToList();
                decimal othersRevenue = categoryList.Skip(5).Sum(c => c.Revenue);

                if (categoryList.Count > 5)
                {
                    topCategories.Add(new CategoryRevenue
                    {
                        CategoryName = "Khác",
                        Revenue = othersRevenue
                    });
                }

                return topCategories;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthers: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<CategoryRevenue>();
            }
        }

        private void DisplayRevenueByCategoryChart()
        {
            chartRevenueByCategory.Series.Clear();
            chartRevenueByCategory.Titles.Clear();
            lblColumnChartTitle.Text = "Doanh Thu Theo Danh Mục";

            if (!HasData())
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            List<CategoryRevenue> categoryList = GetTop5CategoriesAndOthers();
            if (categoryList.Count == 0)
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            Series series = new Series("Doanh Thu")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(52, 152, 219),
                BorderWidth = 0
            };

            decimal maxRevenue = 0;
            foreach (var category in categoryList)
            {
                decimal revenueInMillions = category.Revenue / 1000000;
                series.Points.AddXY(category.CategoryName, revenueInMillions);
                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

                DataPoint point = series.Points[series.Points.Count - 1];
                point.Color = Color.FromArgb(52, 152, 219);
                point.BorderColor = Color.FromArgb(52, 152, 219);
            }

            chartRevenueByCategory.Series.Add(series);

            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "";
            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

            series.IsValueShownAsLabel = true;
            series.LabelFormat = "{0}M";
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            series.LabelForeColor = Color.FromArgb(44, 62, 80);

            foreach (DataPoint point in series.Points)
            {
                point["PixelPointWidth"] = "40";
            }
        }

        private class WeeklyRevenue
        {
            public int WeekNumber { get; set; }
            public decimal Revenue { get; set; }
        }

        private List<WeeklyRevenue> GetWeeklyRevenue()
        {
            Dictionary<int, decimal> weeklyRevenues = new Dictionary<int, decimal>();

            try
            {
                // Khởi tạo các tuần
                for (int i = 1; i <= 5; i++)
                {
                    weeklyRevenues[i] = 0;
                }

                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetWeeklyRevenue.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int weekNumber = (exitDate.Day - 1) / 7 + 1;
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity, unit_price FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;
                            decimal unitPrice = detailRow["unit_price"] != DBNull.Value ? Convert.ToDecimal(detailRow["unit_price"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;

                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;

                                if (matchesCategory)
                                {
                                    weeklyRevenues[weekNumber] += quantity * unitPrice;
                                }
                            }
                        }
                    }
                }

                return weeklyRevenues.Select(kv => new WeeklyRevenue
                {
                    WeekNumber = kv.Key,
                    Revenue = kv.Value
                }).OrderBy(w => w.WeekNumber).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetWeeklyRevenue: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<WeeklyRevenue>();
            }
        }

        private void DisplayWeeklyRevenueChart()
        {
            chartRevenueByCategory.Series.Clear();
            chartRevenueByCategory.Titles.Clear();
            lblColumnChartTitle.Text = "Doanh Thu Theo Tuần";

            if (!HasData())
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            List<WeeklyRevenue> weeklyList = GetWeeklyRevenue();
            if (weeklyList.Count == 0 || weeklyList.All(w => w.Revenue == 0))
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            Series series = new Series("Doanh Thu")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(52, 152, 219),
                BorderWidth = 2
            };

            decimal maxRevenue = 0;
            foreach (var week in weeklyList)
            {
                decimal revenueInMillions = week.Revenue / 1000000;
                series.Points.AddXY($"Tuần {week.WeekNumber}", revenueInMillions);
                if (revenueInMillions > maxRevenue) maxRevenue = revenueInMillions;

                DataPoint point = series.Points[series.Points.Count - 1];
                point.Color = Color.FromArgb(52, 152, 219);
                point.MarkerStyle = MarkerStyle.Circle;
                point.MarkerSize = 8;
                point.MarkerColor = Color.FromArgb(52, 152, 219);
            }

            chartRevenueByCategory.Series.Add(series);

            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Tuần";
            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Triệu VNĐ";
            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}M";
            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) : 10;
            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxRevenue > 0 ? (double)Math.Ceiling(maxRevenue / 5) * 5 : 50;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);

            series.IsValueShownAsLabel = true;
            series.LabelFormat = "{0}M";
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            series.LabelForeColor = Color.FromArgb(44, 62, 80);
        }

        private class DailyProductSales
        {
            public int Day { get; set; }
            public decimal Quantity { get; set; }
        }

        private List<DailyProductSales> GetDailyProductSales()
        {
            List<DailyProductSales> dailySales = new List<DailyProductSales>();

            try
            {
                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetDailyProductSales.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);
                        System.Diagnostics.Debug.WriteLine($"Retrieved {detailsDt.Rows.Count} records from stock_exit_details for stock_exit_id = {stockExitId}.");

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

                            if (productId == selectedProductId)
                            {
                                string queryProducts = $"SELECT category_id, name FROM products WHERE id = {productId}";
                                DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                                System.Diagnostics.Debug.WriteLine($"Retrieved {productsDt.Rows.Count} records from products for product_id = {productId}.");

                                if (productsDt.Rows.Count > 0)
                                {
                                    int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
                                    if (selectedCategoryId == 0 || categoryId == selectedCategoryId)
                                    {
                                        // Kiểm tra xem ngày đã tồn tại trong dailySales chưa
                                        var existingSale = dailySales.FirstOrDefault(s => s.Day == exitDate.Day);
                                        if (existingSale != null)
                                        {
                                            // Nếu ngày đã tồn tại, cộng dồn quantity
                                            existingSale.Quantity += quantity;
                                            System.Diagnostics.Debug.WriteLine($"Updated quantity for day {exitDate.Day}: {existingSale.Quantity}");
                                        }
                                        else
                                        {
                                            // Nếu ngày chưa tồn tại, thêm mới
                                            dailySales.Add(new DailyProductSales
                                            {
                                                Day = exitDate.Day,
                                                Quantity = quantity
                                            });
                                            System.Diagnostics.Debug.WriteLine($"Added new day {exitDate.Day} with quantity {quantity}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Điền các ngày thiếu trong tháng
                int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
                for (int day = 1; day <= daysInMonth; day++)
                {
                    if (!dailySales.Any(s => s.Day == day))
                    {
                        dailySales.Add(new DailyProductSales { Day = day, Quantity = 0 });
                        System.Diagnostics.Debug.WriteLine($"Added missing day {day} with quantity 0");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetDailyProductSales: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<DailyProductSales>();
            }

            return dailySales.OrderBy(s => s.Day).ToList();
        }

        private void DisplayProductSalesTrendChart()
        {
            chartRevenueByCategory.Series.Clear();
            chartRevenueByCategory.Titles.Clear();
            lblColumnChartTitle.Text = $"Xu Hướng Bán Hàng - {products[cmbProductFilter.SelectedIndex].Value}";

            if (!HasData())
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            List<DailyProductSales> dailySales = GetDailyProductSales();
            if (dailySales.Count == 0 || dailySales.All(s => s.Quantity == 0))
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartRevenueByCategory.Titles.Add(noDataTitle);
                return;
            }

            Series series = new Series("Số Lượng Bán")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.FromArgb(231, 76, 60),
                BorderWidth = 2
            };

            decimal maxQuantity = 0;
            foreach (var sale in dailySales)
            {
                series.Points.AddXY($"Ngày {sale.Day}", sale.Quantity);
                if (sale.Quantity > maxQuantity) maxQuantity = sale.Quantity;

                DataPoint point = series.Points[series.Points.Count - 1];
                point.MarkerStyle = MarkerStyle.Circle;
                point.MarkerSize = 8;
                point.MarkerColor = Color.FromArgb(231, 76, 60);
            }

            chartRevenueByCategory.Series.Add(series);

            chartRevenueByCategory.ChartAreas[0].AxisX.Title = "Ngày trong tháng";
            chartRevenueByCategory.ChartAreas[0].AxisY.Title = "Số lượng";
            chartRevenueByCategory.ChartAreas[0].AxisY.LabelStyle.Format = "{0}";
            chartRevenueByCategory.ChartAreas[0].AxisY.Interval = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) : 10;
            chartRevenueByCategory.ChartAreas[0].AxisY.Minimum = 0;
            chartRevenueByCategory.ChartAreas[0].AxisY.Maximum = maxQuantity > 0 ? (double)Math.Ceiling(maxQuantity / 5) * 5 : 50;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Angle = 0;
            chartRevenueByCategory.ChartAreas[0].AxisX.LabelStyle.Font = new Font("Segoe UI", 8F);
            chartRevenueByCategory.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartRevenueByCategory.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            chartRevenueByCategory.ChartAreas[0].AxisX.Interval = 1;

            series.IsValueShownAsLabel = true;
            series.LabelFormat = "{0}";
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            series.LabelForeColor = Color.FromArgb(44, 62, 80);
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public decimal Quantity { get; set; }
        }

        private List<ProductSales> GetTop6ProductsBySales()
        {
            Dictionary<int, decimal> productQuantities = new Dictionary<int, decimal>();
            Dictionary<int, string> productNames = new Dictionary<int, string>();

            try
            {
                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop6ProductsBySales.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

                            string queryProducts = $"SELECT category_id, name FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
                                string productName = productsDt.Rows[0]["name"].ToString();

                                bool matchesCategory = selectedCategoryId == 0 || categoryId == selectedCategoryId;
                                bool matchesProduct = selectedProductId == 0 || productId == selectedProductId;

                                if (matchesCategory && matchesProduct)
                                {
                                    if (!productQuantities.ContainsKey(productId))
                                    {
                                        productQuantities[productId] = 0;
                                        productNames[productId] = productName;
                                    }
                                    productQuantities[productId] += quantity;
                                }
                            }
                        }
                    }
                }

                return productQuantities
                    .Where(kv => kv.Value > 0)
                    .Select(kv => new ProductSales
                    {
                        ProductName = productNames[kv.Key],
                        Quantity = kv.Value
                    })
                    .OrderByDescending(ps => ps.Quantity)
                    .Take(6)
                    .ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTop6ProductsBySales: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<ProductSales>();
            }
        }

        private void DisplayProductRatioChart()
        {
            chartProductRatio.Series.Clear();
            chartProductRatio.Titles.Clear();
            chartProductRatio.Legends.Clear();

            if (!HasData())
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            List<ProductSales> productSalesList = GetTop6ProductsBySales();
            if (productSalesList.Count == 0)
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            Series series = new Series("Tỉ Lệ") { ChartType = SeriesChartType.Pie };

            decimal totalQuantity = productSalesList.Sum(p => p.Quantity);
            if (totalQuantity == 0)
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Right,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.Transparent
            };
            chartProductRatio.Legends.Add(legend);

            Color[] colorPalette = new Color[]
            {
                Color.FromArgb(41, 128, 185),
                Color.FromArgb(231, 76, 60),
                Color.FromArgb(46, 204, 113),
                Color.FromArgb(155, 89, 182),
                Color.FromArgb(243, 156, 18),
                Color.FromArgb(127, 140, 141)
            };

            int colorIndex = 0;
            foreach (var product in productSalesList)
            {
                double percentage = Math.Round((double)(product.Quantity / totalQuantity * 100), 1);

                DataPoint point = new DataPoint();
                point.AxisLabel = product.ProductName;
                point.YValues = new double[] { Convert.ToDouble(product.Quantity) };
                point.LegendText = $"{product.ProductName} ({percentage}%)";
                point.Label = $"{percentage}%";
                point.Color = colorPalette[colorIndex % colorPalette.Length];

                series.Points.Add(point);
                colorIndex++;
            }

            series.IsValueShownAsLabel = true;
            series.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            series.LabelForeColor = Color.White;

            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
            chartProductRatio.ChartAreas[0].Area3DStyle.Inclination = 0;

            chartProductRatio.Series.Add(series);
            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
            chartProductRatio.Series[0]["PieLineColor"] = "White";
            chartProductRatio.Series[0]["PieDrawingStyle"] = "Default";
        }

        private class CategorySales
        {
            public string CategoryName { get; set; }
            public decimal Quantity { get; set; }
        }

        private List<CategorySales> GetTop5CategoriesAndOthersForSales()
        {
            Dictionary<int, decimal> categoryQuantities = new Dictionary<int, decimal>();
            Dictionary<int, string> categoryNames = new Dictionary<int, string>();

            try
            {
                string queryCategories = "SELECT id, name FROM categories";
                DataTable categoriesDt = DatabaseHelper.ExecuteQuery(queryCategories);
                foreach (DataRow row in categoriesDt.Rows)
                {
                    int categoryId = Convert.ToInt32(row["id"]);
                    string categoryName = row["name"].ToString();
                    categoryNames[categoryId] = categoryName;
                    categoryQuantities[categoryId] = 0;
                }

                string queryHeaders = "SELECT id, exit_date FROM stock_exit_headers";
                DataTable headersDt = DatabaseHelper.ExecuteQuery(queryHeaders);
                System.Diagnostics.Debug.WriteLine($"Retrieved {headersDt.Rows.Count} records from stock_exit_headers for GetTop5CategoriesAndOthersForSales.");

                foreach (DataRow headerRow in headersDt.Rows)
                {
                    DateTime exitDate = headerRow["exit_date"] != DBNull.Value ? Convert.ToDateTime(headerRow["exit_date"]) : DateTime.MinValue;
                    if (exitDate.Year == selectedYear && exitDate.Month == selectedMonth)
                    {
                        int stockExitId = Convert.ToInt32(headerRow["id"]);

                        string queryDetails = $"SELECT product_id, quantity FROM stock_exit_details WHERE stock_exit_id = {stockExitId}";
                        DataTable detailsDt = DatabaseHelper.ExecuteQuery(queryDetails);

                        foreach (DataRow detailRow in detailsDt.Rows)
                        {
                            int productId = detailRow["product_id"] != DBNull.Value ? Convert.ToInt32(detailRow["product_id"]) : 0;
                            decimal quantity = detailRow["quantity"] != DBNull.Value ? Convert.ToDecimal(detailRow["quantity"]) : 0;

                            string queryProducts = $"SELECT category_id FROM products WHERE id = {productId}";
                            DataTable productsDt = DatabaseHelper.ExecuteQuery(queryProducts);
                            if (productsDt.Rows.Count > 0)
                            {
                                int categoryId = productsDt.Rows[0]["category_id"] != DBNull.Value ? Convert.ToInt32(productsDt.Rows[0]["category_id"]) : 0;
                                if (categoryQuantities.ContainsKey(categoryId))
                                {
                                    categoryQuantities[categoryId] += quantity;
                                }
                            }
                        }
                    }
                }

                List<CategorySales> categorySalesList = categoryQuantities
                    .Where(kv => kv.Value > 0)
                    .Select(kv => new CategorySales
                    {
                        CategoryName = categoryNames.ContainsKey(kv.Key) ? categoryNames[kv.Key] : "Unknown",
                        Quantity = kv.Value
                    })
                    .OrderByDescending(cs => cs.Quantity)
                    .ToList();

                List<CategorySales> topCategories = categorySalesList.Take(5).ToList();
                decimal othersQuantity = categorySalesList.Skip(5).Sum(c => c.Quantity);

                if (categorySalesList.Count > 5)
                {
                    topCategories.Add(new CategorySales
                    {
                        CategoryName = "Khác",
                        Quantity = othersQuantity
                    });
                }

                return topCategories;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTop5CategoriesAndOthersForSales: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return new List<CategorySales>();
            }
        }

        private void DisplayCategorySalesRatioChart()
        {
            chartProductRatio.Series.Clear();
            chartProductRatio.Titles.Clear();
            chartProductRatio.Legends.Clear();

            if (!HasData())
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            List<CategorySales> categorySalesList = GetTop5CategoriesAndOthersForSales();
            if (categorySalesList.Count == 0)
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            Series series = new Series("Tỉ Lệ")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                LabelForeColor = Color.White
            };

            Legend legend = new Legend("Legend")
            {
                Docking = Docking.Right,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.Transparent
            };
            chartProductRatio.Legends.Add(legend);

            decimal totalQuantity = categorySalesList.Sum(c => c.Quantity);
            if (totalQuantity == 0)
            {
                Title noDataTitle = new Title("Không có dữ liệu để hiển thị", Docking.Top,
                    new Font("Segoe UI", 12, FontStyle.Bold), Color.Gray);
                chartProductRatio.Titles.Add(noDataTitle);
                return;
            }

            Color[] colors = new Color[]
            {
                Color.FromArgb(41, 128, 185),
                Color.FromArgb(231, 76, 60),
                Color.FromArgb(46, 204, 113),
                Color.FromArgb(155, 89, 182),
                Color.FromArgb(243, 156, 18),
                Color.FromArgb(127, 140, 141)
            };

            int colorIndex = 0;
            foreach (var category in categorySalesList)
            {
                double percentage = Math.Round((double)(category.Quantity / totalQuantity) * 100, 1);
                DataPoint point = new DataPoint
                {
                    AxisLabel = category.CategoryName,
                    YValues = new double[] { (double)category.Quantity },
                    LegendText = $"{category.CategoryName} ({percentage}%)",
                    Label = $"{percentage}%",
                    Color = colors[colorIndex % colors.Length]
                };
                series.Points.Add(point);
                colorIndex++;
            }

            chartProductRatio.Series.Add(series);

            chartProductRatio.ChartAreas[0].Area3DStyle.Enable3D = false;
            chartProductRatio.Series[0]["PieLabelStyle"] = "Inside";
            chartProductRatio.Series[0]["PieLineColor"] = "White";
        }
    }
}