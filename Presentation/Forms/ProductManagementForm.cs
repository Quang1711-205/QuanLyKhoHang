//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ProductManagementForm : BaseForm
//    {
//        // Reference to UI elements created in SetupResponsiveLayout
//        private Panel mainContentPanel;
//        private Button addProductButton;

//        private TextBox searchTextBox;
//        private Button searchButton;

//        public ProductManagementForm()
//        {
//            InitializeComponent();
//        }

//        private void ProductManagementForm_Load(object sender, EventArgs e)
//        {
//            // First setup the responsive layout
//            SetupResponsiveLayout();

//            // Then load product data
//            LoadProductData();
//        }

//        public void LoadProductData()
//        {
//            string query = "SELECT id, name, price, stock_quantity FROM products";
//            DataTable productData = DatabaseHelper.ExecuteQuery(query);

//            // Remove empty rows if any
//            productData.Rows.Cast<DataRow>().Where(row =>
//                row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field.ToString()))
//            ).ToList().ForEach(row => row.Delete());
//            productData.AcceptChanges();

//            // Configure the data grid view
//            ConfigureDataGridView(productData);
//        }

//        // Phương thức Reset để làm mới bảng
//        public void Reset()
//        {
//            LoadProductData(); // Gọi lại LoadProductData để làm mới bảng
//        }


//        private void ConfigureDataGridView(DataTable productData)
//        {
//            // Unregister previous event handlers if any
//            dgvProduct.CellFormatting -= DgvProduct_CellFormatting;
//            dgvProduct.CellPainting -= DgvProduct_CellPainting;
//            dgvProduct.CellClick -= DgvProduct_CellClick;

//            // Clear existing columns before setting new data source
//            dgvProduct.DataSource = null;
//            dgvProduct.Columns.Clear();

//            // Configure DataGridView basic properties
//            dgvProduct.DataSource = productData;
//            dgvProduct.BorderStyle = BorderStyle.None;
//            dgvProduct.BackgroundColor = Color.White;
//            dgvProduct.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
//            dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
//            dgvProduct.RowTemplate.Height = 45; // More compact rows
//            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvProduct.RowHeadersVisible = false;
//            dgvProduct.AllowUserToAddRows = false;

//            // Remove fixed size constraints that cause layout problems
//            dgvProduct.AutoSize = false;
//            dgvProduct.Dock = DockStyle.Fill;

//            // Ensure scrollbars appear when needed
//            dgvProduct.ScrollBars = ScrollBars.Both;

//            // Configure header styling
//            dgvProduct.EnableHeadersVisualStyles = false;
//            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
//            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//            dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
//            dgvProduct.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//            dgvProduct.ColumnHeadersHeight = 45;
//            dgvProduct.ColumnHeadersVisible = true;

//            // Configure selection styling
//            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgvProduct.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
//            dgvProduct.DefaultCellStyle.SelectionForeColor = Color.White;

//            // Configure cell styling
//            dgvProduct.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
//            dgvProduct.DefaultCellStyle.Padding = new Padding(5);

//            // Configure alternating row colors for better readability
//            dgvProduct.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
//            dgvProduct.RowsDefaultCellStyle.BackColor = Color.White;

//            // Add Edit button column with improved styling
//            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
//            btnEdit.HeaderText = "Chỉnh sửa";
//            btnEdit.Name = "Edit";
//            btnEdit.Text = "Sửa";  // Shorter text
//            btnEdit.UseColumnTextForButtonValue = true;
//            btnEdit.FlatStyle = FlatStyle.Flat;
//            btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
//            btnEdit.DefaultCellStyle.ForeColor = Color.White;
//            btnEdit.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
//            btnEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            btnEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
//            btnEdit.DefaultCellStyle.SelectionForeColor = Color.White;
//            dgvProduct.Columns.Add(btnEdit);

//            // Add Delete button column with improved styling
//            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
//            btnDelete.HeaderText = "Xóa";
//            btnDelete.Name = "Delete";
//            btnDelete.Text = "Xóa";
//            btnDelete.UseColumnTextForButtonValue = true;
//            btnDelete.FlatStyle = FlatStyle.Flat;
//            btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
//            btnDelete.DefaultCellStyle.ForeColor = Color.White;
//            btnDelete.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
//            btnDelete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            btnDelete.DefaultCellStyle.SelectionBackColor = Color.FromArgb(192, 57, 43);
//            btnDelete.DefaultCellStyle.SelectionForeColor = Color.White;
//            dgvProduct.Columns.Add(btnDelete);

//            // Set column headers
//            dgvProduct.Columns["id"].HeaderText = "Mã SP";
//            dgvProduct.Columns["name"].HeaderText = "Tên Sản phẩm";
//            dgvProduct.Columns["price"].HeaderText = "Giá (VND)";  // Clarify currency
//            dgvProduct.Columns["stock_quantity"].HeaderText = "Số lượng";

//            // Set optimal column widths
//            dgvProduct.Columns["id"].FillWeight = 10;
//            dgvProduct.Columns["name"].FillWeight = 40;
//            dgvProduct.Columns["price"].FillWeight = 20;
//            dgvProduct.Columns["stock_quantity"].FillWeight = 15;
//            dgvProduct.Columns["Edit"].FillWeight = 12;
//            dgvProduct.Columns["Delete"].FillWeight = 12;

//            // Configure column properties
//            dgvProduct.Columns["id"].ReadOnly = true;
//            dgvProduct.Columns["id"].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

//            dgvProduct.Columns["name"].ReadOnly = false;
//            dgvProduct.Columns["price"].ReadOnly = false;
//            dgvProduct.Columns["stock_quantity"].ReadOnly = false;

//            dgvProduct.Columns["Edit"].ReadOnly = false;
//            dgvProduct.Columns["Delete"].ReadOnly = false;

//            // Format currency with thousand separators
//            dgvProduct.Columns["price"].DefaultCellStyle.Format = "N0";

//            // Align column contents
//            dgvProduct.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProduct.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
//            dgvProduct.Columns["price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgvProduct.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            // Disable column sorting
//            foreach (DataGridViewColumn column in dgvProduct.Columns)
//            {
//                column.SortMode = DataGridViewColumnSortMode.NotSortable;
//            }

//            // Set consistent row height
//            foreach (DataGridViewRow row in dgvProduct.Rows)
//            {
//                row.Height = 45;  // More compact than original 50
//            }

//            // Register events
//            dgvProduct.CellFormatting += DgvProduct_CellFormatting;
//            dgvProduct.CellPainting += DgvProduct_CellPainting;
//            dgvProduct.CellClick += DgvProduct_CellClick;
//        }

//        private void SetupResponsiveLayout()
//        {

//            Button originalThemSanPham = this.Controls["themsanpham"] as Button;
//            TextBox originalTxbSearch = this.Controls["txbSearch"] as TextBox;
//            Button originalSearchProduct = this.Controls["btnSearch"] as Button;
//            Label originalTitleProduct = this.Controls["titleProduct"] as Label;

//            mainContentPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(20),
//                BackColor = Color.White
//            };
//            this.Controls.Add(mainContentPanel);
//            mainContentPanel.BringToFront();

//            TableLayoutPanel topPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Top,
//                Height = 100,
//                ColumnCount = 2,
//                RowCount = 2,
//                Margin = new Padding(0, 0, 0, 15),
//                BackColor = Color.White
//            };

//            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));  // Cột 1: Tiêu đề và TextBox
//            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));  // Cột 2: Button Thêm và Tìm kiếm

//            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Hàng 1: Tiêu đề và Button Thêm
//            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Hàng 2: Tìm kiếm

//            mainContentPanel.Controls.Add(topPanel);

//            if (originalTitleProduct != null)
//            {
//                Label newTitle = new Label
//                {
//                    Text = originalTitleProduct.Text,
//                    Dock = DockStyle.Fill,
//                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                    ForeColor = Color.FromArgb(45, 66, 91),
//                    TextAlign = ContentAlignment.MiddleLeft
//                };
//                topPanel.Controls.Add(newTitle, 0, 0);
//            }

//            if (originalThemSanPham != null)
//            {
//                addProductButton = new Button
//                {
//                    Text = originalThemSanPham.Text,
//                    Size = new Size(172, 42),
//                    Dock = DockStyle.Fill,  // Đảm bảo button sẽ co giãn theo chiều rộng và chiều cao của ô
//                    FlatStyle = FlatStyle.Flat,
//                    BackColor = Color.FromArgb(46, 204, 113),
//                    ForeColor = Color.White,
//                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                    Cursor = Cursors.Hand,
//                };
//                addProductButton.Click += themsanpham_Click;
//                topPanel.Controls.Add(addProductButton, 1, 0);  // Cột 1, Hàng 0
//            }

//            TableLayoutPanel searchPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 2,
//                RowCount = 1,
//                Margin = new Padding(10, 5, 10, 5)
//            };
//            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));  // Cột 1: TextBox Tìm kiếm
//            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));  // Cột 2: Button Tìm kiếm

//            // Đảm bảo TextBox và Button có cùng chiều cao
//            searchPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Hàng duy nhất

//            int searchHeight = 35;
//            TextBox newSearchBox = new TextBox
//            {
//                Text = originalTxbSearch?.Text,
//                Dock = DockStyle.Fill,
//                Font = new Font("Segoe UI", 10F),
//                BorderStyle = BorderStyle.FixedSingle,
//                MinimumSize = new Size(0, searchHeight),
//                Multiline = true,
//            };
//            searchPanel.Controls.Add(newSearchBox, 0, 0);  // Cột 0, Hàng 0

//            Button newSearchButton = new Button
//            {
//                Text = originalSearchProduct?.Text,
//                Dock = DockStyle.Fill,  // Đảm bảo button co giãn
//                FlatStyle = FlatStyle.Flat,
//                BackColor = Color.FromArgb(52, 152, 219),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                Cursor = Cursors.Hand,
//                Margin = new Padding(5, 0, 0, 0),
//            };
//            searchPanel.Controls.Add(newSearchButton, 1, 0);  // Cột 1, Hàng 0

//            topPanel.Controls.Add(searchPanel, 0, 1);
//            topPanel.SetColumnSpan(searchPanel, 2);  // Đảm bảo tìm kiếm chiếm 2 cột

//            Panel gridContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(1),
//                BackColor = Color.FromArgb(240, 240, 240),
//                BorderStyle = BorderStyle.None
//            };
//            mainContentPanel.Controls.Add(gridContainer);

//            Panel innerGridContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(1, 10, 1, 1),
//                BackColor = Color.White
//            };
//            gridContainer.Controls.Add(innerGridContainer);

//            if (dgvProduct != null)
//            {
//                dgvProduct.Size = new Size(0, 0);
//                dgvProduct.AutoSize = false;
//                dgvProduct.Dock = DockStyle.Fill;
//                dgvProduct.Margin = new Padding(0, 5, 0, 0);
//                dgvProduct.ColumnHeadersVisible = true;
//                dgvProduct.BackgroundColor = Color.White;
//                dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
//                innerGridContainer.Controls.Add(dgvProduct);
//            }

//            // Thay đổi padding cho innerGridContainer
//            innerGridContainer.Padding = new Padding(1, 110, 1, 1); // Thêm padding phía trên

//            // Hoặc thêm margin cho DataGridView
//            dgvProduct.Margin = new Padding(0, 110, 0, 0); // Thêm margin phía trên
//            dgvProduct.ColumnHeadersVisible = true;

//            mainContentPanel.Location = new Point(200, 60);
//            mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);

//            // Trong phần tạo TextBox và Button tìm kiếm
//            searchTextBox = newSearchBox; // Lưu tham chiếu đến TextBox mới
//            searchButton = newSearchButton; // Lưu tham chiếu đến Button mới

//            // Gán sự kiện click cho button tìm kiếm mới
//            newSearchButton.Click += btnSearch_Click;

//            this.Resize += (sender, e) =>
//            {
//                mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);
//            };
//        }




//        private void DgvProduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Format price display
//            if (dgvProduct.Columns[e.ColumnIndex].Name == "price" && e.Value != null)
//            {
//                if (decimal.TryParse(e.Value.ToString(), out decimal value))
//                {
//                    e.Value = string.Format("{0:N0} VND", value);
//                    e.FormattingApplied = true;
//                }
//            }
//        }

//        private void DgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            // Create rounded buttons for Edit and Delete
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < dgvProduct.Columns.Count &&
//                    (dgvProduct.Columns[e.ColumnIndex].Name == "Edit" || dgvProduct.Columns[e.ColumnIndex].Name == "Delete"))
//            {
//                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Focus);

//                Rectangle buttonRect = new Rectangle(
//                    e.CellBounds.X + (e.CellBounds.Width - 80) / 2,
//                    e.CellBounds.Y + (e.CellBounds.Height - 28) / 2,
//                    80, 28);

//                using (GraphicsPath path = new GraphicsPath())
//                {
//                    path.AddRoundedRectangle(buttonRect, 4);

//                    // Button color based on type
//                    Color buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
//                        Color.FromArgb(52, 152, 219) : Color.FromArgb(231, 76, 60);

//                    // Darken color if row is selected
//                    if (dgvProduct.Rows[e.RowIndex].Selected)
//                    {
//                        buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
//                            Color.FromArgb(41, 128, 185) : Color.FromArgb(192, 57, 43);
//                    }

//                    using (SolidBrush brush = new SolidBrush(buttonColor))
//                    {
//                        e.Graphics.FillPath(brush, path);
//                    }

//                    // Draw button text
//                    string buttonText = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ? "Cập nhật" : "Xóa";
//                    using (SolidBrush textBrush = new SolidBrush(Color.White))
//                    {
//                        StringFormat format = new StringFormat
//                        {
//                            Alignment = StringAlignment.Center,
//                            LineAlignment = StringAlignment.Center
//                        };
//                        e.Graphics.DrawString(buttonText, new Font("Segoe UI", 9F, FontStyle.Bold),
//                                             textBrush, buttonRect, format);
//                    }
//                }
//                e.Handled = true;
//            }
//        }

//        private void RefreshProductData()
//        {
//            // Only reload the product data without touching the layout
//            LoadProductData();
//        }

//        private void DgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            // Check if click is on a button cell and not on the header
//            if (e.RowIndex >= 0)
//            {
//                // Get the clicked row data
//                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
//                int productId = Convert.ToInt32(row.Cells["id"].Value);

//                // Check if Edit button was clicked
//                if (dgvProduct.Columns[e.ColumnIndex].Name == "Edit")
//                {
//                    try
//                    {
//                        // Get the modified values from the datagridview
//                        string name = row.Cells["name"].Value.ToString();

//                        // Extract numeric value from formatted price string
//                        string priceStr = row.Cells["price"].Value.ToString();
//                        priceStr = priceStr.Replace(" VND", "").Replace(",", "");
//                        decimal price = Convert.ToDecimal(priceStr);

//                        int stockQuantity = Convert.ToInt32(row.Cells["stock_quantity"].Value);

//                        // Update the product in database
//                        string updateQuery = "UPDATE products SET name = @name, price = @price, stock_quantity = @stockQuantity WHERE id = @id";
//                        Dictionary<string, object> parameters = new Dictionary<string, object>
//                        {
//                            { "@name", name },
//                            { "@price", price },
//                            { "@stockQuantity", stockQuantity },
//                            { "@id", productId }
//                        };

//                        // Execute the update query
//                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            // Reload the data after update
//                            RefreshProductData();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Không thể cập nhật sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    }
//                }
//                // Check if Delete button was clicked
//                else if (dgvProduct.Columns[e.ColumnIndex].Name == "Delete")
//                {
//                    // Ask for confirmation before deletion
//                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận",
//                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                    if (result == DialogResult.Yes)
//                    {
//                        try
//                        {
//                            // Delete the product from database
//                            string deleteQuery = "DELETE FROM products WHERE id = @id";
//                            Dictionary<string, object> parameters = new Dictionary<string, object>
//                            {
//                                { "@id", productId }
//                            };

//                            // Execute the delete query
//                            int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteQuery, parameters);

//                            if (rowsAffected > 0)
//                            {
//                                MessageBox.Show("Sản phẩm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                                if (dgvProduct.Rows.Count == 1)
//                                {
//                                    DatabaseHelper.ExecuteNonQuery("ALTER TABLE products AUTO_INCREMENT = 1");
//                                }
//                                // Reload the data after deletion
//                                RefreshProductData();
//                            }
//                            else
//                            {
//                                MessageBox.Show("Không thể xóa sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//            }
//        }

//        private void themsanpham_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                AddProductForm addProductForm = new AddProductForm(this);
//                DialogResult result = addProductForm.ShowDialog();

//                // Refresh the product list if a product was added
//                if (result == DialogResult.OK)
//                {
//                    RefreshProductData();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi mở form thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            // Lấy từ khóa từ TextBox mới (searchTextBox) thay vì txbSearch cũ
//            string searchKeyword = searchTextBox.Text.Trim();

//            // Xóa khoảng trắng dư thừa giữa các từ
//            searchKeyword = System.Text.RegularExpressions.Regex.Replace(searchKeyword, @"\s+", " ");

//            // Kiểm tra dữ liệu đầu vào
//            if (string.IsNullOrWhiteSpace(searchKeyword))
//            {
//                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // Sửa lại truy vấn để phù hợp với bảng dữ liệu hiện tại
//            try
//            {
//                string query = @"
//            SELECT id, name, price, stock_quantity 
//            FROM products
//            WHERE LOWER(name) LIKE LOWER(@searchKeyword)";

//                // Chuẩn bị tham số cho truy vấn
//                Dictionary<string, object> parameters = new Dictionary<string, object>
//        {
//            { "@searchKeyword", $"%{searchKeyword}%" } // Thêm ký tự % để tìm kiếm gần đúng
//        };

//                // Thực thi truy vấn
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

//                // Hiển thị kết quả trong DataGridView
//                ConfigureDataGridView(dt);

//                // Thông báo nếu không tìm thấy kết quả
//                if (dt.Rows.Count == 0)
//                {
//                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }

//    // Extension method to draw rounded rectangle
//    public static class GraphicsExtensions
//    {
//        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle bounds, int radius)
//        {
//            int diameter = radius * 2;
//            Size size = new Size(diameter, diameter);
//            Rectangle arc = new Rectangle(bounds.Location, size);

//            // Top left arc
//            path.AddArc(arc, 180, 90);

//            // Top right arc
//            arc.X = bounds.Right - diameter;
//            path.AddArc(arc, 270, 90);

//            // Bottom right arc
//            arc.Y = bounds.Bottom - diameter;
//            path.AddArc(arc, 0, 90);

//            // Bottom left arc
//            arc.X = bounds.Left;
//            path.AddArc(arc, 90, 90);

//            path.CloseFigure();
//        }
//    }
//}

// Code chính gần nhất

//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class ProductManagementForm : BaseForm
//    {
//        // Reference to UI elements created in SetupResponsiveLayout
//        private Panel mainContentPanel;
//        private Button addProductButton;

//        private TextBox searchTextBox;
//        private Button searchButton;

//        public ProductManagementForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
//        }

//        private void ProductManagementForm_Load(object sender, EventArgs e)
//        {
//            // First setup the responsive layout
//            SetupResponsiveLayout();

//            // Then load product data
//            LoadProductData();
//        }

//        public void LoadProductData()
//        {
//            string query = "SELECT id, name, import_price, export_price, stock_quantity FROM products";
//            DataTable productData = DatabaseHelper.ExecuteQuery(query);

//            // Remove empty rows if any
//            productData.Rows.Cast<DataRow>().Where(row =>
//                row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field.ToString()))
//            ).ToList().ForEach(row => row.Delete());
//            productData.AcceptChanges();

//            // Configure the data grid view
//            ConfigureDataGridView(productData);
//        }

//        // Phương thức Reset để làm mới bảng
//        public void Reset()
//        {
//            LoadProductData(); // Gọi lại LoadProductData để làm mới bảng
//        }

//        private void ConfigureDataGridView(DataTable productData)
//        {
//            // Unregister previous event handlers if any
//            dgvProduct.CellFormatting -= DgvProduct_CellFormatting;
//            dgvProduct.CellPainting -= DgvProduct_CellPainting;
//            dgvProduct.CellClick -= DgvProduct_CellClick;

//            // Clear existing columns before setting new data source
//            dgvProduct.DataSource = null;
//            dgvProduct.Columns.Clear();

//            // Configure DataGridView basic properties
//            dgvProduct.DataSource = productData;
//            dgvProduct.BorderStyle = BorderStyle.None;
//            dgvProduct.BackgroundColor = Color.White;
//            dgvProduct.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
//            dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
//            dgvProduct.RowTemplate.Height = 45; // More compact rows
//            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
//            dgvProduct.RowHeadersVisible = false;
//            dgvProduct.AllowUserToAddRows = false;

//            // Remove fixed size constraints that cause layout problems
//            dgvProduct.AutoSize = false;
//            dgvProduct.Dock = DockStyle.Fill;

//            // Ensure scrollbars appear when needed
//            dgvProduct.ScrollBars = ScrollBars.Both;

//            // Configure header styling
//            dgvProduct.EnableHeadersVisualStyles = false;
//            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
//            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
//            dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
//            dgvProduct.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
//            dgvProduct.ColumnHeadersHeight = 45;
//            dgvProduct.ColumnHeadersVisible = true;

//            // Configure selection styling
//            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
//            dgvProduct.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
//            dgvProduct.DefaultCellStyle.SelectionForeColor = Color.White;

//            // Configure cell styling
//            dgvProduct.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
//            dgvProduct.DefaultCellStyle.Padding = new Padding(5);

//            // Configure alternating row colors for better readability
//            dgvProduct.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
//            dgvProduct.RowsDefaultCellStyle.BackColor = Color.White;

//            // Add Edit button column with improved styling
//            DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
//            btnEdit.HeaderText = "Chỉnh sửa";
//            btnEdit.Name = "Edit";
//            btnEdit.Text = "Sửa";  // Shorter text
//            btnEdit.UseColumnTextForButtonValue = true;
//            btnEdit.FlatStyle = FlatStyle.Flat;
//            btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
//            btnEdit.DefaultCellStyle.ForeColor = Color.White;
//            btnEdit.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
//            btnEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            btnEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
//            btnEdit.DefaultCellStyle.SelectionForeColor = Color.White;
//            dgvProduct.Columns.Add(btnEdit);

//            // Add Delete button column with improved styling
//            DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
//            btnDelete.HeaderText = "Xóa";
//            btnDelete.Name = "Delete";
//            btnDelete.Text = "Xóa";
//            btnDelete.UseColumnTextForButtonValue = true;
//            btnDelete.FlatStyle = FlatStyle.Flat;
//            btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
//            btnDelete.DefaultCellStyle.ForeColor = Color.White;
//            btnDelete.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
//            btnDelete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            btnDelete.DefaultCellStyle.SelectionBackColor = Color.FromArgb(192, 57, 43);
//            btnDelete.DefaultCellStyle.SelectionForeColor = Color.White;
//            dgvProduct.Columns.Add(btnDelete);

//            // Set column headers
//            dgvProduct.Columns["id"].HeaderText = "Mã SP";
//            dgvProduct.Columns["name"].HeaderText = "Tên Sản phẩm";
//            dgvProduct.Columns["import_price"].HeaderText = "Giá nhập (VND)";  // Đổi tên cột
//            dgvProduct.Columns["export_price"].HeaderText = "Giá bán (VND)";  // Thêm cột mới
//            dgvProduct.Columns["stock_quantity"].HeaderText = "Số lượng";

//            // Set optimal column widths
//            dgvProduct.Columns["id"].FillWeight = 10;
//            dgvProduct.Columns["name"].FillWeight = 35;  // Giảm bớt để nhường chỗ cho cột mới
//            dgvProduct.Columns["import_price"].FillWeight = 15;  // Điều chỉnh tỷ lệ
//            dgvProduct.Columns["export_price"].FillWeight = 15;  // Điều chỉnh tỷ lệ
//            dgvProduct.Columns["stock_quantity"].FillWeight = 13;
//            dgvProduct.Columns["Edit"].FillWeight = 12;
//            dgvProduct.Columns["Delete"].FillWeight = 12;

//            // Configure column properties
//            dgvProduct.Columns["id"].ReadOnly = true;
//            dgvProduct.Columns["id"].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

//            dgvProduct.Columns["name"].ReadOnly = false;
//            dgvProduct.Columns["import_price"].ReadOnly = false;
//            dgvProduct.Columns["export_price"].ReadOnly = false;
//            dgvProduct.Columns["stock_quantity"].ReadOnly = false;

//            dgvProduct.Columns["Edit"].ReadOnly = false;
//            dgvProduct.Columns["Delete"].ReadOnly = false;

//            // Format currency with thousand separators for both price columns
//            dgvProduct.Columns["import_price"].DefaultCellStyle.Format = "N0";
//            dgvProduct.Columns["export_price"].DefaultCellStyle.Format = "N0";

//            // Align column contents
//            dgvProduct.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
//            dgvProduct.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
//            dgvProduct.Columns["import_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgvProduct.Columns["export_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
//            dgvProduct.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

//            // Disable column sorting
//            foreach (DataGridViewColumn column in dgvProduct.Columns)
//            {
//                column.SortMode = DataGridViewColumnSortMode.NotSortable;
//            }

//            // Set consistent row height
//            foreach (DataGridViewRow row in dgvProduct.Rows)
//            {
//                row.Height = 45;  // More compact than original 50
//            }

//            // Register events
//            dgvProduct.CellFormatting += DgvProduct_CellFormatting;
//            dgvProduct.CellPainting += DgvProduct_CellPainting;
//            dgvProduct.CellClick += DgvProduct_CellClick;
//        }

//        private void SetupResponsiveLayout()
//        {
//            Button originalThemSanPham = this.Controls["themsanpham"] as Button;
//            TextBox originalTxbSearch = this.Controls["txbSearch"] as TextBox;
//            Button originalSearchProduct = this.Controls["btnSearch"] as Button;
//            Label originalTitleProduct = this.Controls["titleProduct"] as Label;

//            mainContentPanel = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(20),
//                BackColor = Color.White
//            };
//            this.Controls.Add(mainContentPanel);
//            mainContentPanel.BringToFront();

//            TableLayoutPanel topPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Top,
//                Height = 100,
//                ColumnCount = 2,
//                RowCount = 2,
//                Margin = new Padding(0, 0, 0, 15),
//                BackColor = Color.White
//            };

//            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));  // Cột 1: Tiêu đề và TextBox
//            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));  // Cột 2: Button Thêm và Tìm kiếm

//            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Hàng 1: Tiêu đề và Button Thêm
//            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));  // Hàng 2: Tìm kiếm

//            mainContentPanel.Controls.Add(topPanel);

//            if (originalTitleProduct != null)
//            {
//                Label newTitle = new Label
//                {
//                    Text = originalTitleProduct.Text,
//                    Dock = DockStyle.Fill,
//                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                    ForeColor = Color.FromArgb(45, 66, 91),
//                    TextAlign = ContentAlignment.MiddleLeft
//                };
//                topPanel.Controls.Add(newTitle, 0, 0);
//            }

//            if (originalThemSanPham != null)
//            {
//                addProductButton = new Button
//                {
//                    Text = originalThemSanPham.Text,
//                    Size = new Size(172, 42),
//                    Dock = DockStyle.Fill,  // Đảm bảo button sẽ co giãn theo chiều rộng và chiều cao của ô
//                    FlatStyle = FlatStyle.Flat,
//                    BackColor = Color.FromArgb(46, 204, 113),
//                    ForeColor = Color.White,
//                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                    Cursor = Cursors.Hand,
//                };
//                addProductButton.Click += themsanpham_Click;
//                topPanel.Controls.Add(addProductButton, 1, 0);  // Cột 1, Hàng 0
//            }

//            TableLayoutPanel searchPanel = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                ColumnCount = 2,
//                RowCount = 1,
//                Margin = new Padding(10, 5, 10, 5)
//            };
//            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));  // Cột 1: TextBox Tìm kiếm
//            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));  // Cột 2: Button Tìm kiếm

//            // Đảm bảo TextBox và Button có cùng chiều cao
//            searchPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Hàng duy nhất

//            int searchHeight = 35;
//            TextBox newSearchBox = new TextBox
//            {
//                Text = originalTxbSearch?.Text,
//                Dock = DockStyle.Fill,
//                Font = new Font("Segoe UI", 10F),
//                BorderStyle = BorderStyle.FixedSingle,
//                MinimumSize = new Size(0, searchHeight),
//                Multiline = true,
//            };
//            searchPanel.Controls.Add(newSearchBox, 0, 0);  // Cột 0, Hàng 0

//            Button newSearchButton = new Button
//            {
//                Text = originalSearchProduct?.Text,
//                Dock = DockStyle.Fill,  // Đảm bảo button co giãn
//                FlatStyle = FlatStyle.Flat,
//                BackColor = Color.FromArgb(52, 152, 219),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                Cursor = Cursors.Hand,
//                Margin = new Padding(5, 0, 0, 0),
//            };
//            searchPanel.Controls.Add(newSearchButton, 1, 0);  // Cột 1, Hàng 0

//            topPanel.Controls.Add(searchPanel, 0, 1);
//            topPanel.SetColumnSpan(searchPanel, 2);  // Đảm bảo tìm kiếm chiếm 2 cột

//            Panel gridContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(1),
//                BackColor = Color.FromArgb(240, 240, 240),
//                BorderStyle = BorderStyle.None
//            };
//            mainContentPanel.Controls.Add(gridContainer);

//            Panel innerGridContainer = new Panel
//            {
//                Dock = DockStyle.Fill,
//                Padding = new Padding(1, 10, 1, 1),
//                BackColor = Color.White
//            };
//            gridContainer.Controls.Add(innerGridContainer);

//            if (dgvProduct != null)
//            {
//                dgvProduct.Size = new Size(0, 0);
//                dgvProduct.AutoSize = false;
//                dgvProduct.Dock = DockStyle.Fill;
//                dgvProduct.Margin = new Padding(0, 5, 0, 0);
//                dgvProduct.ColumnHeadersVisible = true;
//                dgvProduct.BackgroundColor = Color.White;
//                dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
//                innerGridContainer.Controls.Add(dgvProduct);
//            }

//            // Thay đổi padding cho innerGridContainer
//            innerGridContainer.Padding = new Padding(1, 110, 1, 1); // Thêm padding phía trên

//            // Hoặc thêm margin cho DataGridView
//            dgvProduct.Margin = new Padding(0, 110, 0, 0); // Thêm margin phía trên
//            dgvProduct.ColumnHeadersVisible = true;

//            mainContentPanel.Location = new Point(200, 60);
//            mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);

//            // Trong phần tạo TextBox và Button tìm kiếm
//            searchTextBox = newSearchBox; // Lưu tham chiếu đến TextBox mới
//            searchButton = newSearchButton; // Lưu tham chiếu đến Button mới

//            // Gán sự kiện click cho button tìm kiếm mới
//            newSearchButton.Click += btnSearch_Click;

//            this.Resize += (sender, e) =>
//            {
//                mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);
//            };
//        }

//        private string FormatProductCode(int id)
//        {
//            if (id < 10) // 1 chữ số
//            {
//                return $"SP00{id}";
//            }
//            else if (id < 100) // 2 chữ số
//            {
//                return $"SP0{id}";
//            }
//            else // 3 chữ số trở lên
//            {
//                return $"SP{id}";
//            }
//        }

//        private void DgvProduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            // Format price display for both import_price and export_price
//            if ((dgvProduct.Columns[e.ColumnIndex].Name == "import_price" ||
//                 dgvProduct.Columns[e.ColumnIndex].Name == "export_price") && e.Value != null)
//            {
//                if (decimal.TryParse(e.Value.ToString(), out decimal value))
//                {
//                    e.Value = string.Format("{0:N0} VND", value);
//                    e.FormattingApplied = true;
//                }
//            }

//            // Format the "Mã SP" (id) column
//            if (dgvProduct.Columns[e.ColumnIndex].Name == "id" && e.Value != null)
//            {
//                if (int.TryParse(e.Value.ToString(), out int id))
//                {
//                    e.Value = FormatProductCode(id); // Định dạng mã sản phẩm
//                    e.FormattingApplied = true;
//                }
//            }
//        }

//        private void DgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            // Create rounded buttons for Edit and Delete
//            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < dgvProduct.Columns.Count &&
//                    (dgvProduct.Columns[e.ColumnIndex].Name == "Edit" || dgvProduct.Columns[e.ColumnIndex].Name == "Delete"))
//            {
//                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Focus);

//                Rectangle buttonRect = new Rectangle(
//                    e.CellBounds.X + (e.CellBounds.Width - 80) / 2,
//                    e.CellBounds.Y + (e.CellBounds.Height - 28) / 2,
//                    80, 28);

//                using (GraphicsPath path = new GraphicsPath())
//                {
//                    path.AddRoundedRectangle(buttonRect, 4);

//                    // Button color based on type
//                    Color buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
//                        Color.FromArgb(52, 152, 219) : Color.FromArgb(231, 76, 60);

//                    // Darken color if row is selected
//                    if (dgvProduct.Rows[e.RowIndex].Selected)
//                    {
//                        buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
//                            Color.FromArgb(41, 128, 185) : Color.FromArgb(192, 57, 43);
//                    }

//                    using (SolidBrush brush = new SolidBrush(buttonColor))
//                    {
//                        e.Graphics.FillPath(brush, path);
//                    }

//                    // Draw button text
//                    string buttonText = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ? "Cập nhật" : "Xóa";
//                    using (SolidBrush textBrush = new SolidBrush(Color.White))
//                    {
//                        StringFormat format = new StringFormat
//                        {
//                            Alignment = StringAlignment.Center,
//                            LineAlignment = StringAlignment.Center
//                        };
//                        e.Graphics.DrawString(buttonText, new Font("Segoe UI", 9F, FontStyle.Bold),
//                                             textBrush, buttonRect, format);
//                    }
//                }
//                e.Handled = true;
//            }
//        }

//        private void RefreshProductData()
//        {
//            // Only reload the product data without touching the layout
//            LoadProductData();
//        }

//        private void DgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            // Check if click is on a button cell and not on the header
//            if (e.RowIndex >= 0)
//            {
//                // Get the clicked row data
//                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
//                int productId = Convert.ToInt32(row.Cells["id"].Value);

//                // Check if Edit button was clicked
//                if (dgvProduct.Columns[e.ColumnIndex].Name == "Edit")
//                {
//                    try
//                    {
//                        // Get the modified values from the datagridview
//                        string name = row.Cells["name"].Value.ToString();

//                        // Extract numeric value from formatted import_price string
//                        string importPriceStr = row.Cells["import_price"].Value.ToString();
//                        importPriceStr = importPriceStr.Replace(" VND", "").Replace(",", "");
//                        decimal importPrice = Convert.ToDecimal(importPriceStr);

//                        // Extract numeric value from formatted export_price string
//                        string exportPriceStr = row.Cells["export_price"].Value.ToString();
//                        exportPriceStr = exportPriceStr.Replace(" VND", "").Replace(",", "");
//                        decimal exportPrice = Convert.ToDecimal(exportPriceStr);

//                        int stockQuantity = Convert.ToInt32(row.Cells["stock_quantity"].Value);

//                        // Update the product in database
//                        string updateQuery = "UPDATE products SET name = @name, import_price = @importPrice, export_price = @exportPrice, stock_quantity = @stockQuantity WHERE id = @id";
//                        Dictionary<string, object> parameters = new Dictionary<string, object>
//                        {
//                            { "@name", name },
//                            { "@importPrice", importPrice },
//                            { "@exportPrice", exportPrice },
//                            { "@stockQuantity", stockQuantity },
//                            { "@id", productId }
//                        };

//                        // Execute the update query
//                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);

//                        if (rowsAffected > 0)
//                        {
//                            MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                            // Reload the data after update
//                            RefreshProductData();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Không thể cập nhật sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    }
//                }
//                // Check if Delete button was clicked
//                else if (dgvProduct.Columns[e.ColumnIndex].Name == "Delete")
//                {
//                    // Ask for confirmation before deletion
//                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận",
//                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

//                    if (result == DialogResult.Yes)
//                    {
//                        try
//                        {
//                            // Delete the product from database
//                            string deleteQuery = "DELETE FROM products WHERE id = @id";
//                            Dictionary<string, object> parameters = new Dictionary<string, object>
//                            {
//                                { "@id", productId }
//                            };

//                            // Execute the delete query
//                            int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteQuery, parameters);

//                            if (rowsAffected > 0)
//                            {
//                                MessageBox.Show("Sản phẩm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

//                                if (dgvProduct.Rows.Count == 1)
//                                {
//                                    DatabaseHelper.ExecuteNonQuery("ALTER TABLE products AUTO_INCREMENT = 1");
//                                }
//                                // Reload the data after deletion
//                                RefreshProductData();
//                            }
//                            else
//                            {
//                                MessageBox.Show("Không thể xóa sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            }
//                        }
//                        catch (Exception ex)
//                        {
//                            MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//            }
//        }

//        private void themsanpham_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                AddProductForm addProductForm = new AddProductForm(this);
//                DialogResult result = addProductForm.ShowDialog();

//                // Refresh the product list if a product was added
//                if (result == DialogResult.OK)
//                {
//                    RefreshProductData();
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi mở form thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            // Lấy từ khóa từ TextBox mới (searchTextBox) thay vì txbSearch cũ
//            string searchKeyword = searchTextBox.Text.Trim();

//            // Xóa khoảng trắng dư thừa giữa các từ
//            searchKeyword = System.Text.RegularExpressions.Regex.Replace(searchKeyword, @"\s+", " ");

//            // Kiểm tra dữ liệu đầu vào
//            if (string.IsNullOrWhiteSpace(searchKeyword))
//            {
//                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            // Sửa lại truy vấn để phù hợp với bảng dữ liệu hiện tại
//            try
//            {
//                string query = @"
//                    SELECT id, name, import_price, export_price, stock_quantity 
//                    FROM products
//                    WHERE LOWER(name) LIKE LOWER(@searchKeyword)";

//                // Chuẩn bị tham số cho truy vấn
//                Dictionary<string, object> parameters = new Dictionary<string, object>
//                {
//                    { "@searchKeyword", $"%{searchKeyword}%" } // Thêm ký tự % để tìm kiếm gần đúng
//                };

//                // Thực thi truy vấn
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

//                // Hiển thị kết quả trong DataGridView
//                ConfigureDataGridView(dt);

//                // Thông báo nếu không tìm thấy kết quả
//                if (dt.Rows.Count == 0)
//                {
//                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }

//    // Extension method to draw rounded rectangle
//    public static class GraphicsExtensions
//    {
//        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle bounds, int radius)
//        {
//            int diameter = radius * 2;
//            Size size = new Size(diameter, diameter);
//            Rectangle arc = new Rectangle(bounds.Location, size);

//            // Top left arc
//            path.AddArc(arc, 180, 90);

//            // Top right arc
//            arc.X = bounds.Right - diameter;
//            path.AddArc(arc, 270, 90);

//            // Bottom right arc
//            arc.Y = bounds.Bottom - diameter;
//            path.AddArc(arc, 0, 90);

//            // Bottom left arc
//            arc.X = bounds.Left;
//            path.AddArc(arc, 90, 90);

//            path.CloseFigure();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class ProductManagementForm : BaseForm
    {
        private Panel mainContentPanel;
        private Button addProductButton;
        private TextBox searchTextBox;
        private Button searchButton;

        public ProductManagementForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            // Kiểm tra role trước khi hiển thị dữ liệu
            if (!IsValidRole())
            {
                MessageBox.Show("Role không hợp lệ! Bạn không có quyền truy cập Quản lý sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
        }

        private bool IsValidRole()
        {
            return RoleId == 1 || RoleId == 3; // Chỉ cho phép Admin (1) và Kế toán (3)
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {
            SetupResponsiveLayout();
            LoadProductData();
        }

        public void LoadProductData()
        {
            try
            {
                // Đảm bảo kết nối đã được khởi tạo
                DatabaseHelper.InitializeConnection();

                string query = "SELECT id, name, import_price, export_price, stock_quantity FROM products";
                DataTable productData = DatabaseHelper.ExecuteQuery(query);

                // Kiểm tra dữ liệu trả về
                if (productData == null)
                {
                    MessageBox.Show("Không thể lấy dữ liệu sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Remove empty rows if any
                productData.Rows.Cast<DataRow>().Where(row =>
                    row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field.ToString()))
                ).ToList().ForEach(row => row.Delete());
                productData.AcceptChanges();

                // Debug info
                Console.WriteLine($"Số lượng sản phẩm đọc được: {productData.Rows.Count}");

                // Configure the data grid view
                ConfigureDataGridView(productData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu sản phẩm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Reset()
        {
            LoadProductData();
        }

        private void ConfigureDataGridView(DataTable productData)
        {
            // Kiểm tra xem dgvProduct có được khởi tạo không
            if (dgvProduct == null)
            {
                MessageBox.Show("DataGridView 'dgvProduct' chưa được khởi tạo!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra dữ liệu trong productData
            if (productData == null || productData.Columns.Count == 0 || !productData.Columns.Contains("id"))
            {
                MessageBox.Show("Không có dữ liệu sản phẩm hoặc dữ liệu không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xóa các sự kiện cũ để tránh trùng lặp
            dgvProduct.CellFormatting -= DgvProduct_CellFormatting;
            dgvProduct.CellPainting -= DgvProduct_CellPainting;
            dgvProduct.CellClick -= DgvProduct_CellClick;

            // Xóa các cột cũ và gán DataSource
            dgvProduct.DataSource = null;
            dgvProduct.Columns.Clear();
            dgvProduct.DataSource = productData;

            // Cấu hình giao diện DataGridView
            dgvProduct.BorderStyle = BorderStyle.None;
            dgvProduct.BackgroundColor = Color.White;
            dgvProduct.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
            dgvProduct.RowTemplate.Height = 45;
            dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProduct.RowHeadersVisible = false;
            dgvProduct.AllowUserToAddRows = false;

            dgvProduct.AutoSize = false;
            dgvProduct.Dock = DockStyle.Fill;
            dgvProduct.ScrollBars = ScrollBars.Both;

            dgvProduct.EnableHeadersVisualStyles = false;
            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 66, 91);
            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProduct.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            dgvProduct.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvProduct.ColumnHeadersHeight = 45;
            dgvProduct.ColumnHeadersVisible = true;

            dgvProduct.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProduct.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dgvProduct.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvProduct.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvProduct.DefaultCellStyle.Padding = new Padding(5);

            dgvProduct.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 252);
            dgvProduct.RowsDefaultCellStyle.BackColor = Color.White;

            // Chỉ hiển thị cột "Chỉnh sửa" và "Xóa" nếu là Admin (roleId = 1)
            if (RoleId == 1)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.HeaderText = "Chỉnh sửa";
                btnEdit.Name = "Edit";
                btnEdit.Text = "Sửa";
                btnEdit.UseColumnTextForButtonValue = true;
                btnEdit.FlatStyle = FlatStyle.Flat;
                btnEdit.DefaultCellStyle.BackColor = Color.FromArgb(52, 152, 219);
                btnEdit.DefaultCellStyle.ForeColor = Color.White;
                btnEdit.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btnEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnEdit.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
                btnEdit.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvProduct.Columns.Add(btnEdit);

                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.HeaderText = "Xóa";
                btnDelete.Name = "Delete";
                btnDelete.Text = "Xóa";
                btnDelete.UseColumnTextForButtonValue = true;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.DefaultCellStyle.BackColor = Color.FromArgb(231, 76, 60);
                btnDelete.DefaultCellStyle.ForeColor = Color.White;
                btnDelete.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                btnDelete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                btnDelete.DefaultCellStyle.SelectionBackColor = Color.FromArgb(192, 57, 43);
                btnDelete.DefaultCellStyle.SelectionForeColor = Color.White;
                dgvProduct.Columns.Add(btnDelete);
            }

            // Gán HeaderText cho các cột, với kiểm tra cột tồn tại
            if (dgvProduct.Columns["id"] != null)
                dgvProduct.Columns["id"].HeaderText = "Mã SP";
            else
            {
                MessageBox.Show("Cột 'id' không tồn tại trong DataGridView!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvProduct.Columns["name"] != null)
                dgvProduct.Columns["name"].HeaderText = "Tên Sản phẩm";
            else
            {
                MessageBox.Show("Cột 'name' không tồn tại trong DataGridView!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvProduct.Columns["import_price"] != null)
                dgvProduct.Columns["import_price"].HeaderText = "Giá nhập (VND)";
            else
            {
                MessageBox.Show("Cột 'import_price' không tồn tại trong DataGridView!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvProduct.Columns["export_price"] != null)
                dgvProduct.Columns["export_price"].HeaderText = "Giá bán (VND)";
            else
            {
                MessageBox.Show("Cột 'export_price' không tồn tại trong DataGridView!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvProduct.Columns["stock_quantity"] != null)
                dgvProduct.Columns["stock_quantity"].HeaderText = "Số lượng";
            else
            {
                MessageBox.Show("Cột 'stock_quantity' không tồn tại trong DataGridView!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Điều chỉnh tỷ lệ cột dựa trên role
            if (RoleId == 1)
            {
                // Admin: Có cột "Chỉnh sửa" và "Xóa"
                if (dgvProduct.Columns["id"] != null) dgvProduct.Columns["id"].FillWeight = 10;
                if (dgvProduct.Columns["name"] != null) dgvProduct.Columns["name"].FillWeight = 35;
                if (dgvProduct.Columns["import_price"] != null) dgvProduct.Columns["import_price"].FillWeight = 15;
                if (dgvProduct.Columns["export_price"] != null) dgvProduct.Columns["export_price"].FillWeight = 15;
                if (dgvProduct.Columns["stock_quantity"] != null) dgvProduct.Columns["stock_quantity"].FillWeight = 13;
                if (dgvProduct.Columns["Edit"] != null) dgvProduct.Columns["Edit"].FillWeight = 12;
                if (dgvProduct.Columns["Delete"] != null) dgvProduct.Columns["Delete"].FillWeight = 12;
            }
            else
            {
                // Kế toán: Không có cột "Chỉnh sửa" và "Xóa"
                if (dgvProduct.Columns["id"] != null) dgvProduct.Columns["id"].FillWeight = 15;
                if (dgvProduct.Columns["name"] != null) dgvProduct.Columns["name"].FillWeight = 40;
                if (dgvProduct.Columns["import_price"] != null) dgvProduct.Columns["import_price"].FillWeight = 15;
                if (dgvProduct.Columns["export_price"] != null) dgvProduct.Columns["export_price"].FillWeight = 15;
                if (dgvProduct.Columns["stock_quantity"] != null) dgvProduct.Columns["stock_quantity"].FillWeight = 15;
            }

            if (dgvProduct.Columns["id"] != null)
            {
                dgvProduct.Columns["id"].ReadOnly = true;
                dgvProduct.Columns["id"].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            }

            // Kế toán (roleId = 3) chỉ được xem, không được chỉnh sửa
            if (RoleId == 3)
            {
                if (dgvProduct.Columns["name"] != null) dgvProduct.Columns["name"].ReadOnly = true;
                if (dgvProduct.Columns["import_price"] != null) dgvProduct.Columns["import_price"].ReadOnly = true;
                if (dgvProduct.Columns["export_price"] != null) dgvProduct.Columns["export_price"].ReadOnly = true;
                if (dgvProduct.Columns["stock_quantity"] != null) dgvProduct.Columns["stock_quantity"].ReadOnly = true;
            }
            else
            {
                if (dgvProduct.Columns["name"] != null) dgvProduct.Columns["name"].ReadOnly = false;
                if (dgvProduct.Columns["import_price"] != null) dgvProduct.Columns["import_price"].ReadOnly = false;
                if (dgvProduct.Columns["export_price"] != null) dgvProduct.Columns["export_price"].ReadOnly = false;
                if (dgvProduct.Columns["stock_quantity"] != null) dgvProduct.Columns["stock_quantity"].ReadOnly = false;
            }

            if (dgvProduct.Columns["import_price"] != null)
                dgvProduct.Columns["import_price"].DefaultCellStyle.Format = "N0";
            if (dgvProduct.Columns["export_price"] != null)
                dgvProduct.Columns["export_price"].DefaultCellStyle.Format = "N0";

            if (dgvProduct.Columns["id"] != null)
                dgvProduct.Columns["id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (dgvProduct.Columns["name"] != null)
                dgvProduct.Columns["name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (dgvProduct.Columns["import_price"] != null)
                dgvProduct.Columns["import_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (dgvProduct.Columns["export_price"] != null)
                dgvProduct.Columns["export_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            if (dgvProduct.Columns["stock_quantity"] != null)
                dgvProduct.Columns["stock_quantity"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (DataGridViewColumn column in dgvProduct.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                row.Height = 45;
            }

            // Gắn lại các sự kiện
            dgvProduct.CellFormatting += DgvProduct_CellFormatting;
            dgvProduct.CellPainting += DgvProduct_CellPainting;
            dgvProduct.CellClick += DgvProduct_CellClick;
        }

        private void SetupResponsiveLayout()
        {
            Button originalThemSanPham = this.Controls["themsanpham"] as Button;
            TextBox originalTxbSearch = this.Controls["txbSearch"] as TextBox;
            Button originalSearchProduct = this.Controls["btnSearch"] as Button;
            Label originalTitleProduct = this.Controls["titleProduct"] as Label;

            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = Color.White
            };
            this.Controls.Add(mainContentPanel);
            mainContentPanel.BringToFront();

            TableLayoutPanel topPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 100,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0, 0, 0, 15),
                BackColor = Color.White
            };

            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            topPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));

            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            topPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            mainContentPanel.Controls.Add(topPanel);

            if (originalTitleProduct != null)
            {
                Label newTitle = new Label
                {
                    Text = originalTitleProduct.Text,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(45, 66, 91),
                    TextAlign = ContentAlignment.MiddleLeft
                };
                topPanel.Controls.Add(newTitle, 0, 0);
            }

            if (originalThemSanPham != null)
            {
                addProductButton = new Button
                {
                    Text = originalThemSanPham.Text,
                    Size = new Size(172, 42),
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(46, 204, 113),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                };
                addProductButton.Click += themsanpham_Click;

                // Ẩn nút "Thêm sản phẩm" nếu là Kế toán (roleId = 3)
                if (RoleId == 3)
                {
                    addProductButton.Visible = false;
                }

                topPanel.Controls.Add(addProductButton, 1, 0);
            }

            TableLayoutPanel searchPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(10, 5, 10, 5)
            };
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            searchPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            searchPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            int searchHeight = 35;
            TextBox newSearchBox = new TextBox
            {
                Text = originalTxbSearch?.Text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                MinimumSize = new Size(0, searchHeight),
                Multiline = true,
            };
            searchPanel.Controls.Add(newSearchBox, 0, 0);

            Button newSearchButton = new Button
            {
                Text = originalSearchProduct?.Text,
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(5, 0, 0, 0),
            };
            searchPanel.Controls.Add(newSearchButton, 1, 0);

            topPanel.Controls.Add(searchPanel, 0, 1);
            topPanel.SetColumnSpan(searchPanel, 2);

            Panel gridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(1),
                BackColor = Color.FromArgb(240, 240, 240),
                BorderStyle = BorderStyle.None
            };
            mainContentPanel.Controls.Add(gridContainer);

            Panel innerGridContainer = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(1, 10, 1, 1),
                BackColor = Color.White
            };
            gridContainer.Controls.Add(innerGridContainer);

            if (dgvProduct != null)
            {
                dgvProduct.Size = new Size(0, 0);
                dgvProduct.AutoSize = false;
                dgvProduct.Dock = DockStyle.Fill;
                dgvProduct.Margin = new Padding(0, 5, 0, 0);
                dgvProduct.ColumnHeadersVisible = true;
                dgvProduct.BackgroundColor = Color.White;
                dgvProduct.GridColor = Color.FromArgb(240, 240, 240);
                innerGridContainer.Controls.Add(dgvProduct);
            }

            innerGridContainer.Padding = new Padding(1, 110, 1, 1);
            dgvProduct.Margin = new Padding(0, 110, 0, 0);
            dgvProduct.ColumnHeadersVisible = true;

            mainContentPanel.Location = new Point(200, 60);
            mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);

            searchTextBox = newSearchBox;
            searchButton = newSearchButton;

            newSearchButton.Click += btnSearch_Click;

            this.Resize += (sender, e) =>
            {
                mainContentPanel.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 60);
            };
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

        private void DgvProduct_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((dgvProduct.Columns[e.ColumnIndex].Name == "import_price" ||
                 dgvProduct.Columns[e.ColumnIndex].Name == "export_price") && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal value))
                {
                    e.Value = string.Format("{0:N0} VND", value);
                    e.FormattingApplied = true;
                }
            }

            if (dgvProduct.Columns[e.ColumnIndex].Name == "id" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int id))
                {
                    e.Value = FormatProductCode(id);
                    e.FormattingApplied = true;
                }
            }
        }

        private void DgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && e.ColumnIndex < dgvProduct.Columns.Count &&
                (dgvProduct.Columns[e.ColumnIndex].Name == "Edit" || dgvProduct.Columns[e.ColumnIndex].Name == "Delete"))
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Focus);

                Rectangle buttonRect = new Rectangle(
                    e.CellBounds.X + (e.CellBounds.Width - 80) / 2,
                    e.CellBounds.Y + (e.CellBounds.Height - 28) / 2,
                    80, 28);

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddRoundedRectangle(buttonRect, 4);

                    Color buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
                        Color.FromArgb(52, 152, 219) : Color.FromArgb(231, 76, 60);

                    if (dgvProduct.Rows[e.RowIndex].Selected)
                    {
                        buttonColor = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ?
                            Color.FromArgb(41, 128, 185) : Color.FromArgb(192, 57, 43);
                    }

                    using (SolidBrush brush = new SolidBrush(buttonColor))
                    {
                        e.Graphics.FillPath(brush, path);
                    }

                    string buttonText = dgvProduct.Columns[e.ColumnIndex].Name == "Edit" ? "Cập nhật" : "Xóa";
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        StringFormat format = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        e.Graphics.DrawString(buttonText, new Font("Segoe UI", 9F, FontStyle.Bold),
                                             textBrush, buttonRect, format);
                    }
                }
                e.Handled = true;
            }
        }

        private void RefreshProductData()
        {
            LoadProductData();
        }

        private void DgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Chỉ xử lý sự kiện nếu là Admin (roleId = 1)
            if (RoleId != 1) return;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                int productId = Convert.ToInt32(row.Cells["id"].Value);

                if (dgvProduct.Columns[e.ColumnIndex].Name == "Edit")
                {
                    try
                    {
                        string name = row.Cells["name"].Value.ToString();
                        string importPriceStr = row.Cells["import_price"].Value.ToString();
                        importPriceStr = importPriceStr.Replace(" VND", "").Replace(",", "");
                        decimal importPrice = Convert.ToDecimal(importPriceStr);

                        string exportPriceStr = row.Cells["export_price"].Value.ToString();
                        exportPriceStr = exportPriceStr.Replace(" VND", "").Replace(",", "");
                        decimal exportPrice = Convert.ToDecimal(exportPriceStr);

                        int stockQuantity = Convert.ToInt32(row.Cells["stock_quantity"].Value);

                        string updateQuery = "UPDATE products SET name = @name, import_price = @importPrice, export_price = @exportPrice, stock_quantity = @stockQuantity WHERE id = @id";
                        Dictionary<string, object> parameters = new Dictionary<string, object>
                        {
                            { "@name", name },
                            { "@importPrice", importPrice },
                            { "@exportPrice", exportPrice },
                            { "@stockQuantity", stockQuantity },
                            { "@id", productId }
                        };

                        int rowsAffected = DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Sản phẩm đã được cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshProductData();
                        }
                        else
                        {
                            MessageBox.Show("Không thể cập nhật sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "Delete")
                {
                    DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            string deleteQuery = "DELETE FROM products WHERE id = @id";
                            Dictionary<string, object> parameters = new Dictionary<string, object>
                            {
                                { "@id", productId }
                            };

                            int rowsAffected = DatabaseHelper.ExecuteNonQuery(deleteQuery, parameters);

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Sản phẩm đã được xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                if (dgvProduct.Rows.Count == 1)
                                {
                                    DatabaseHelper.ExecuteNonQuery1("ALTER TABLE products AUTO_INCREMENT = 1");
                                }
                                RefreshProductData();
                            }
                            else
                            {
                                MessageBox.Show("Không thể xóa sản phẩm. Vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void themsanpham_Click(object sender, EventArgs e)
        {
            // Chỉ cho phép Admin (roleId = 1) thêm sản phẩm
            if (RoleId != 1) return;

            try
            {
                AddProductForm addProductForm = new AddProductForm(this);
                DialogResult result = addProductForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    RefreshProductData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở form thêm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchKeyword = searchTextBox.Text.Trim();
            searchKeyword = System.Text.RegularExpressions.Regex.Replace(searchKeyword, @"\s+", " ");

            if (string.IsNullOrWhiteSpace(searchKeyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = @"
                    SELECT id, name, import_price, export_price, stock_quantity 
                    FROM products
                    WHERE LOWER(name) LIKE LOWER(@searchKeyword)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@searchKeyword", $"%{searchKeyword}%" }
                };

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                ConfigureDataGridView(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class GraphicsExtensions
    {
        public static void AddRoundedRectangle(this GraphicsPath path, Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
        }
    }
}