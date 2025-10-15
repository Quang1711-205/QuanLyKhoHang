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
//    public partial class UserManagementForm : Form
//    {
//        public UserManagementForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
//        }
//    }
//}


// Code chính 

//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Windows.Forms;
//using WareHouse.DataAccess;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class UserManagementForm : BaseForm
//    {
//        private TableLayoutPanel tblMainContainer;
//        private DataGridView dgvUsers;
//        private TextBox txtSearch;
//        private Button btnAddUser;
//        private Label lblTitle;
//        private Panel pnlHeader;
//        private Panel pnlContent;
//        private bool hasAccessPermission = false;

//        public UserManagementForm(int roleId) : base(roleId)
//        {
//            InitializeComponent();
//            CheckAccessPermission();
//            this.StartPosition = FormStartPosition.CenterScreen;

//            if (hasAccessPermission)
//            {
//                InitializeComponents();
//                LoadUserData("");
//            }
//            else
//            {
//                ShowAccessDeniedMessage();
//            }
//        }

//        private void CheckAccessPermission()
//        {
//            hasAccessPermission = (RoleId == 1); // Chỉ admin mới được truy cập
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
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(245, 247, 250)
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
//            this.Text = "QuickStock - Quản Lý Tài Khoản";
//            this.Size = new Size(1280, 720);
//            this.FormBorderStyle = FormBorderStyle.Sizable;
//            this.MaximizeBox = true;
//            this.BackColor = Color.FromArgb(240, 242, 245);

//            // Main container
//            tblMainContainer = new TableLayoutPanel
//            {
//                Dock = DockStyle.Fill,
//                RowCount = 2,
//                ColumnCount = 1,
//                Padding = new Padding(260, 20, 20, 20),
//                BackColor = Color.FromArgb(240, 242, 245)
//            };
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
//            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
//            this.Controls.Add(tblMainContainer);

//            // Header section
//            pnlHeader = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Margin = new Padding(0, 0, 0, 10)
//            };
//            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

//            // Title
//            lblTitle = new Label
//            {
//                Text = "QUẢN LÝ TÀI KHOẢN",
//                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
//                ForeColor = Color.FromArgb(41, 128, 185),
//                AutoSize = true,
//                Location = new Point(20, 30)
//            };
//            pnlHeader.Controls.Add(lblTitle);

//            // Add new user button
//            btnAddUser = new Button
//            {
//                Text = "Thêm người dùng",
//                BackColor = Color.FromArgb(46, 204, 113),
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
//                FlatStyle = FlatStyle.Flat,
//                Size = new Size(180, 40),
//                Location = new Point(pnlHeader.Width - 420, 30),
//                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(39, 174, 96) },
//                Cursor = Cursors.Hand
//            };
//            btnAddUser.Click += BtnAddUser_Click;
//            pnlHeader.Controls.Add(btnAddUser);

//            // Search box
//            txtSearch = new TextBox
//            {
//                Width = 220,
//                Height = 35,
//                Font = new Font("Segoe UI", 11F),
//                Location = new Point(pnlHeader.Width - 230, 30),
//                ForeColor = Color.FromArgb(149, 165, 166),
//                BackColor = Color.FromArgb(245, 247, 250),
//                Text = "Tìm kiếm người dùng...",
//                BorderStyle = BorderStyle.FixedSingle,
//                Padding = new Padding(10)
//            };
//            txtSearch.GotFocus += (s, e) =>
//            {
//                if (txtSearch.Text == "Tìm kiếm người dùng...")
//                {
//                    txtSearch.Text = "";
//                    txtSearch.ForeColor = Color.FromArgb(44, 62, 80);
//                }
//            };
//            txtSearch.LostFocus += (s, e) =>
//            {
//                if (string.IsNullOrEmpty(txtSearch.Text))
//                {
//                    txtSearch.Text = "Tìm kiếm người dùng...";
//                    txtSearch.ForeColor = Color.FromArgb(149, 165, 166);
//                }
//            };
//            txtSearch.TextChanged += TxtSearch_TextChanged;
//            pnlHeader.Controls.Add(txtSearch);

//            // Resize buttons and search box on panel resize
//            pnlHeader.Resize += (sender, e) =>
//            {
//                btnAddUser.Location = new Point(pnlHeader.Width - 420, 30);
//                txtSearch.Location = new Point(pnlHeader.Width - 230, 30);
//            };

//            // Content section
//            pnlContent = new Panel
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.White,
//                Padding = new Padding(0),
//                Margin = new Padding(0)
//            };
//            tblMainContainer.Controls.Add(pnlContent, 0, 1);

//            // DataGridView for users
//            dgvUsers = new DataGridView
//            {
//                Dock = DockStyle.Fill,
//                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
//                RowHeadersVisible = false,
//                AllowUserToAddRows = false,
//                AllowUserToDeleteRows = false,
//                ReadOnly = true,
//                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
//                BackgroundColor = Color.White,
//                BorderStyle = BorderStyle.None,
//                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
//                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
//                ColumnHeadersHeight = 50,
//                RowTemplate = { Height = 55 },
//                GridColor = Color.FromArgb(234, 236, 239),
//                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
//                {
//                    BackColor = Color.FromArgb(44, 62, 80),
//                    ForeColor = Color.White,
//                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
//                    Alignment = DataGridViewContentAlignment.MiddleCenter,
//                    Padding = new Padding(15, 10, 15, 10)
//                },
//                DefaultCellStyle = new DataGridViewCellStyle
//                {
//                    Font = new Font("Segoe UI", 10.5F),
//                    ForeColor = Color.FromArgb(44, 62, 80),
//                    BackColor = Color.White,
//                    Padding = new Padding(15, 10, 15, 10),
//                    SelectionBackColor = Color.FromArgb(200, 220, 255),
//                    SelectionForeColor = Color.FromArgb(44, 62, 80),
//                    Alignment = DataGridViewContentAlignment.MiddleCenter
//                },
//                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
//                {
//                    BackColor = Color.FromArgb(249, 251, 253),
//                    Font = new Font("Segoe UI", 10.5F),
//                    ForeColor = Color.FromArgb(44, 62, 80),
//                    Padding = new Padding(15, 10, 15, 10),
//                    SelectionBackColor = Color.FromArgb(200, 220, 255),
//                    SelectionForeColor = Color.FromArgb(44, 62, 80),
//                    Alignment = DataGridViewContentAlignment.MiddleCenter
//                }
//            };
//            dgvUsers.CellClick += DgvUsers_CellClick;
//            dgvUsers.CellFormatting += DgvUsers_CellFormatting;
//            dgvUsers.CellPainting += DgvUsers_CellPainting;

//            // Khởi tạo các cột
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "id",
//                HeaderText = "Mã",
//                DataPropertyName = "id",
//                Width = 60,
//                FillWeight = 50
//            });
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "username",
//                HeaderText = "Tên đăng nhập",
//                DataPropertyName = "username",
//                FillWeight = 100
//            });
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "full_name",
//                HeaderText = "Họ tên",
//                DataPropertyName = "full_name",
//                FillWeight = 130
//            });
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "email",
//                HeaderText = "Email",
//                DataPropertyName = "email",
//                FillWeight = 140
//            });
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "phone",
//                HeaderText = "Số điện thoại",
//                DataPropertyName = "phone",
//                FillWeight = 100
//            });
//            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                Name = "role",
//                HeaderText = "Vai trò",
//                DataPropertyName = "role",
//                FillWeight = 100
//            });
//            dgvUsers.Columns.Add(new DataGridViewButtonColumn
//            {
//                Name = "status_button",
//                HeaderText = "Trạng thái",
//                Text = "",
//                UseColumnTextForButtonValue = false,
//                FlatStyle = FlatStyle.Flat,
//                FillWeight = 80
//            });
//            dgvUsers.Columns.Add(new DataGridViewButtonColumn
//            {
//                Name = "edit",
//                HeaderText = "Chỉnh sửa",
//                Text = "Cập nhật",
//                UseColumnTextForButtonValue = true,
//                FlatStyle = FlatStyle.Flat,
//                FillWeight = 70
//            });
//            dgvUsers.Columns.Add(new DataGridViewButtonColumn
//            {
//                Name = "delete",
//                HeaderText = "Xóa",
//                Text = "Xóa",
//                UseColumnTextForButtonValue = true,
//                FlatStyle = FlatStyle.Flat,
//                FillWeight = 70
//            });

//            pnlContent.Controls.Add(dgvUsers);
//        }

//        private void DgvUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

//            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index ||
//                e.ColumnIndex == dgvUsers.Columns["edit"].Index ||
//                e.ColumnIndex == dgvUsers.Columns["delete"].Index)
//            {
//                e.PaintBackground(e.CellBounds, true);

//                // Tùy chỉnh nút
//                string text = e.FormattedValue?.ToString() ?? "";
//                if (!string.IsNullOrEmpty(text))
//                {
//                    Color backColor = e.CellStyle.BackColor;
//                    Color foreColor = e.CellStyle.ForeColor;

//                    // Hiệu ứng hover
//                    if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
//                    {
//                        backColor = e.CellStyle.SelectionBackColor;
//                        foreColor = e.CellStyle.SelectionForeColor;
//                    }

//                    using (SolidBrush backgroundBrush = new SolidBrush(backColor))
//                    using (SolidBrush textBrush = new SolidBrush(foreColor))
//                    {
//                        // Vẽ nền nút với góc bo tròn
//                        int radius = 5;
//                        Rectangle rect = new Rectangle(
//                            e.CellBounds.X + 5,
//                            e.CellBounds.Y + 5,
//                            e.CellBounds.Width - 10,
//                            e.CellBounds.Height - 10
//                        );
//                        using (GraphicsPath path = new GraphicsPath())
//                        {
//                            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
//                            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
//                            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
//                            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
//                            path.CloseFigure();

//                            e.Graphics.FillPath(backgroundBrush, path);
//                        }

//                        // Vẽ chữ
//                        SizeF textSize = e.Graphics.MeasureString(text, e.CellStyle.Font);
//                        float textX = e.CellBounds.X + (e.CellBounds.Width - textSize.Width) / 2;
//                        float textY = e.CellBounds.Y + (e.CellBounds.Height - textSize.Height) / 2;
//                        e.Graphics.DrawString(text, e.CellStyle.Font, textBrush, textX, textY);
//                    }
//                }

//                e.Handled = true;
//            }
//        }

//        private void LoadUserData(string searchTerm)
//        {
//            string query = @"
//            SELECT u.id, u.username, u.full_name, u.email, u.phone, u.status, r.role_name AS role
//            FROM users u
//            LEFT JOIN roles r ON u.role_id = r.id
//            WHERE u.id LIKE @Search
//               OR u.username LIKE @Search
//               OR u.full_name LIKE @Search
//               OR u.email LIKE @Search
//               OR u.phone LIKE @Search
//            ORDER BY u.id";

//            var parameters = new Dictionary<string, object>
//            {
//                { "@Search", $"%{searchTerm}%" }
//            };

//            try
//            {
//                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
//                dgvUsers.DataSource = dt;
//                FormatDataGridView();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void FormatDataGridView()
//        {
//            if (dgvUsers.Columns.Contains("status"))
//                dgvUsers.Columns["status"].Visible = false;
//        }

//        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
//        {
//            if (e.RowIndex < 0) return;

//            // Format status button
//            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index)
//            {
//                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
//                string status = row.Cells["status"].Value?.ToString();

//                if (status == "Active")
//                {
//                    e.Value = "Hoạt động";
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(46, 204, 113);
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(39, 174, 96);
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//                }
//                else if (status == "Inactive")
//                {
//                    e.Value = "Đã khóa";
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(231, 76, 60);
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(192, 57, 43);
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//                }
//            }

//            // Format edit button
//            if (e.ColumnIndex == dgvUsers.Columns["edit"].Index)
//            {
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(52, 152, 219);
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(41, 128, 185);
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//            }

//            // Format delete button
//            if (e.ColumnIndex == dgvUsers.Columns["delete"].Index)
//            {
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(231, 76, 60);
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(192, 57, 43);
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
//                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
//            }
//        }

//        private void TxtSearch_TextChanged(object sender, EventArgs e)
//        {
//            string searchTerm = txtSearch.Text;
//            if (searchTerm == "Tìm kiếm người dùng...") searchTerm = "";
//            LoadUserData(searchTerm);
//        }

//        private void BtnAddUser_Click(object sender, EventArgs e)
//        {
//            using (var addUserForm = new AddUserForm())
//            {
//                if (addUserForm.ShowDialog(this) == DialogResult.OK)
//                {
//                    LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text);
//                }
//            }
//        }

//        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex < 0) return;

//            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
//            int userId = Convert.ToInt32(row.Cells["id"].Value);
//            string username = row.Cells["username"].Value.ToString();
//            string status = row.Cells["status"].Value?.ToString() ?? "";

//            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index)
//            {
//                string newStatus = (status == "Active") ? "Inactive" : "Active";
//                string action = (status == "Active") ? "khóa" : "mở khóa";

//                DialogResult result = MessageBox.Show(
//                    $"Bạn có chắc muốn {action} tài khoản '{username}' không?",
//                    "Xác nhận thay đổi trạng thái",
//                    MessageBoxButtons.YesNo,
//                    MessageBoxIcon.Question);

//                if (result == DialogResult.Yes)
//                {
//                    UpdateUserStatus(userId, newStatus);
//                }
//            }
//            else if (e.ColumnIndex == dgvUsers.Columns["edit"].Index)
//            {
//                MessageBox.Show($"Chức năng sửa tài khoản '{username}' chưa được triển khai.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//            }
//            else if (e.ColumnIndex == dgvUsers.Columns["delete"].Index)
//            {
//                DialogResult result = MessageBox.Show(
//                    $"Bạn có chắc muốn xóa tài khoản '{username}' không?",
//                    "Xác nhận xóa",
//                    MessageBoxButtons.YesNo,
//                    MessageBoxIcon.Warning);

//                if (result == DialogResult.Yes)
//                {
//                    DeleteUser(userId);
//                }
//            }
//        }

//        private void UpdateUserStatus(int userId, string newStatus)
//        {
//            // Đảm bảo newStatus chỉ là "Active" hoặc "Inactive"
//            if (newStatus != "Active" && newStatus != "Inactive")
//            {
//                MessageBox.Show("Giá trị trạng thái không hợp lệ! Chỉ chấp nhận 'Active' hoặc 'Inactive'.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            string query = "UPDATE users SET status = @Status WHERE id = @Id";
//            var parameters = new Dictionary<string, object>
//            {
//                { "@Status", newStatus },
//                { "@Id", userId }
//            };

//            try
//            {
//                DatabaseHelper.ExecuteNonQuery(query, parameters);
//                MessageBox.Show("Cập nhật trạng thái thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi cập nhật trạng thái: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void DeleteUser(int userId)
//        {
//            string query = "DELETE FROM users WHERE id = @Id";
//            var parameters = new Dictionary<string, object>
//            {
//                { "@Id", userId }
//            };

//            try
//            {
//                DatabaseHelper.ExecuteNonQuery(query, parameters);
//                MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
//                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text);
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Lỗi xóa tài khoản: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//    }
//}

// Code tạm



using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class UserManagementForm : BaseForm
    {
        private TableLayoutPanel tblMainContainer;
        private DataGridView dgvUsers;
        private TextBox txtSearch;
        private Button btnAddUser;
        private Button btnRefresh;
        private ComboBox cboFilterRole;
        private Label lblTitle;
        private Panel pnlHeader;
        private Panel pnlContent;
        private Panel pnlSearch;
        private bool hasAccessPermission = false;
        private bool isEditing = false;

        public UserManagementForm(int roleId) : base(roleId)
        {
            InitializeComponent();
            CheckAccessPermission();
            this.StartPosition = FormStartPosition.CenterScreen;

            if (hasAccessPermission)
            {
                InitializeComponents();
                LoadRolesForFilter();
                LoadUserData("", 0); // 0 = All roles
            }
            else
            {
                ShowAccessDeniedMessage();
            }
        }

        private void CheckAccessPermission()
        {
            hasAccessPermission = (RoleId == 1); // Chỉ admin mới được truy cập
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
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250)
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
            this.Text = "QuickStock - Quản Lý Tài Khoản";
            this.Size = new Size(1212, 830);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // Main container
            tblMainContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(260, 20, 20, 20),
                BackColor = Color.FromArgb(240, 242, 245)
            };
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F)); // Tăng kích thước header
            tblMainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(tblMainContainer);

            // Header section
            pnlHeader = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(0, 0, 0, 10)
            };
            tblMainContainer.Controls.Add(pnlHeader, 0, 0);

            // Title
            lblTitle = new Label
            {
                Text = "QUẢN LÝ TÀI KHOẢN",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Location = new Point(20, 20)
            };
            pnlHeader.Controls.Add(lblTitle);

            // Search and Filter panel
            pnlSearch = new Panel
            {
                Width = 800,
                Height = 40,
                Location = new Point(20, 65),
                BackColor = Color.Transparent
            };
            pnlHeader.Controls.Add(pnlSearch);

            // Filter by role
            Label lblFilter = new Label
            {
                Text = "Lọc theo vai trò:",
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                Location = new Point(0, 10)
            };
            pnlSearch.Controls.Add(lblFilter);

            cboFilterRole = new ComboBox
            {
                Width = 150,
                Location = new Point(110, 7),
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboFilterRole.SelectedIndexChanged += CboFilterRole_SelectedIndexChanged;
            pnlSearch.Controls.Add(cboFilterRole);

            // Search box
            txtSearch = new TextBox
            {
                Width = 250,
                Height = 35,
                Font = new Font("Segoe UI", 11F),
                Location = new Point(280, 5),
                ForeColor = Color.FromArgb(149, 165, 166),
                BackColor = Color.FromArgb(245, 247, 250),
                Text = "Tìm kiếm người dùng...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10)
            };
            txtSearch.GotFocus += (s, e) =>
            {
                if (txtSearch.Text == "Tìm kiếm người dùng...")
                {
                    txtSearch.Text = "";
                    txtSearch.ForeColor = Color.FromArgb(44, 62, 80);
                }
            };
            txtSearch.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    txtSearch.Text = "Tìm kiếm người dùng...";
                    txtSearch.ForeColor = Color.FromArgb(149, 165, 166);
                }
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlSearch.Controls.Add(txtSearch);

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Làm mới",
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 35),
                Location = new Point(550, 5),
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(41, 128, 185) },
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += BtnRefresh_Click;
            pnlSearch.Controls.Add(btnRefresh);

            // Add new user button
            btnAddUser = new Button
            {
                Text = "Thêm người dùng",
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 40),
                Location = new Point(pnlHeader.Width - 200, 20),
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(39, 174, 96) },
                Cursor = Cursors.Hand
            };
            btnAddUser.Click += BtnAddUser_Click;
            pnlHeader.Controls.Add(btnAddUser);

            // Resize buttons on panel resize
            pnlHeader.Resize += (sender, e) =>
            {
                btnAddUser.Location = new Point(pnlHeader.Width - 200, 20);
            };

            // Content section
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };
            tblMainContainer.Controls.Add(pnlContent, 0, 1);

            // DataGridView for users
            dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                ColumnHeadersHeight = 50,
                RowTemplate = { Height = 55 },
                GridColor = Color.FromArgb(234, 236, 239),
                EditMode = DataGridViewEditMode.EditOnEnter,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(44, 62, 80),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Padding = new Padding(15, 10, 15, 10)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = Color.FromArgb(44, 62, 80),
                    BackColor = Color.White,
                    Padding = new Padding(15, 10, 15, 10),
                    SelectionBackColor = Color.FromArgb(200, 220, 255),
                    SelectionForeColor = Color.FromArgb(44, 62, 80),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(249, 251, 253),
                    Font = new Font("Segoe UI", 10.5F),
                    ForeColor = Color.FromArgb(44, 62, 80),
                    Padding = new Padding(15, 10, 15, 10),
                    SelectionBackColor = Color.FromArgb(200, 220, 255),
                    SelectionForeColor = Color.FromArgb(44, 62, 80),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            };
            dgvUsers.CellClick += DgvUsers_CellClick;
            dgvUsers.CellFormatting += DgvUsers_CellFormatting;
            dgvUsers.CellPainting += DgvUsers_CellPainting;
            dgvUsers.CellBeginEdit += DgvUsers_CellBeginEdit;
            dgvUsers.CellEndEdit += DgvUsers_CellEndEdit;
            dgvUsers.DataError += DgvUsers_DataError;

            // Khởi tạo các cột
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "id",
                HeaderText = "Mã",
                DataPropertyName = "id",
                Width = 60,
                FillWeight = 50,
                ReadOnly = true
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "username",
                HeaderText = "Tên đăng nhập",
                DataPropertyName = "username",
                FillWeight = 100,
                ReadOnly = true // Username không thể thay đổi
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "full_name",
                HeaderText = "Họ tên",
                DataPropertyName = "full_name",
                FillWeight = 130,
                ReadOnly = false // Có thể chỉnh sửa
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "email",
                HeaderText = "Email",
                DataPropertyName = "email",
                FillWeight = 140,
                ReadOnly = false // Có thể chỉnh sửa
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "phone",
                HeaderText = "Số điện thoại",
                DataPropertyName = "phone",
                FillWeight = 100,
                ReadOnly = false // Có thể chỉnh sửa
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role",
                HeaderText = "Vai trò",
                DataPropertyName = "role",
                FillWeight = 100,
                ReadOnly = true
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "role_id",
                HeaderText = "Role ID",
                DataPropertyName = "role_id",
                Visible = false
            });
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "status",
                HeaderText = "Status",
                DataPropertyName = "status",
                Visible = false
            });
            dgvUsers.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "status_button",
                HeaderText = "Trạng thái",
                Text = "",
                UseColumnTextForButtonValue = false,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 80
            });
            dgvUsers.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "grant_access",
                HeaderText = "Cấp quyền",
                Text = "",
                UseColumnTextForButtonValue = false,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 70
            });
            dgvUsers.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "reset_password",
                HeaderText = "Reset MK",
                Text = "Reset MK",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 70
            });
            dgvUsers.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "delete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                FillWeight = 60
            });

            pnlContent.Controls.Add(dgvUsers);
        }

        private void LoadRolesForFilter()
        {
            string query = "SELECT id, role_name FROM roles ORDER BY id";

            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query);

                // Thêm tùy chọn "Tất cả vai trò"
                DataRow allRolesRow = dt.NewRow();
                allRolesRow["id"] = 0;
                allRolesRow["role_name"] = "Tất cả vai trò";
                dt.Rows.InsertAt(allRolesRow, 0);

                cboFilterRole.DataSource = dt;
                cboFilterRole.DisplayMember = "role_name";
                cboFilterRole.ValueMember = "id";
                cboFilterRole.SelectedIndex = 0; // Chọn "Tất cả vai trò" mặc định
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải danh sách vai trò: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboFilterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = cboFilterRole.SelectedItem as DataRowView;
            int selectedRoleId = drv != null ? Convert.ToInt32(drv["id"]) : 0;

            string searchTerm = txtSearch.Text;
            if (searchTerm == "Tìm kiếm người dùng...") searchTerm = "";

            LoadUserData(searchTerm, selectedRoleId);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "Tìm kiếm người dùng...";
            txtSearch.ForeColor = Color.FromArgb(149, 165, 166);
            cboFilterRole.SelectedIndex = 0; // Chọn "Tất cả vai trò"
            LoadUserData("", 0);
        }

        private void DgvUsers_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            isEditing = true;
        }

        private void DgvUsers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (!isEditing) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
            int userId = Convert.ToInt32(row.Cells["id"].Value);
            string columnName = dgvUsers.Columns[e.ColumnIndex].Name;
            string newValue = row.Cells[e.ColumnIndex].Value?.ToString() ?? "";

            switch (columnName)
            {
                case "full_name":
                case "email":
                case "phone":
                    UpdateUserField(userId, columnName, newValue);
                    break;
            }

            isEditing = false;
        }

        private void DgvUsers_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            MessageBox.Show("Đã xảy ra lỗi khi chỉnh sửa dữ liệu. Vui lòng kiểm tra lại.",
                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void UpdateUserField(int userId, string fieldName, string newValue)
        {
            // Validate input
            if (fieldName == "email" && !IsValidEmail(newValue))
            {
                MessageBox.Show("Email không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));
                return;
            }

            if (fieldName == "phone" && !IsValidPhone(newValue))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));
                return;
            }

            string query = $"UPDATE users SET {fieldName} = @Value WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Value", newValue },
                { "@Id", userId }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                // Không cần reload toàn bộ dữ liệu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi cập nhật thông tin: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhone(string phone)
        {
            return !string.IsNullOrEmpty(phone) && phone.Length >= 9 && phone.Length <= 15 && phone.All(char.IsDigit);
        }

        private void DgvUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index ||
                e.ColumnIndex == dgvUsers.Columns["grant_access"].Index ||
                e.ColumnIndex == dgvUsers.Columns["reset_password"].Index ||
                e.ColumnIndex == dgvUsers.Columns["delete"].Index)
            {
                e.PaintBackground(e.CellBounds, true);

                // Tùy chỉnh nút
                string text = e.FormattedValue?.ToString() ?? "";
                if (!string.IsNullOrEmpty(text))
                {
                    Color backColor = e.CellStyle.BackColor;
                    Color foreColor = e.CellStyle.ForeColor;

                    // Hiệu ứng hover
                    if ((e.State & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                    {
                        backColor = e.CellStyle.SelectionBackColor;
                        foreColor = e.CellStyle.SelectionForeColor;
                    }

                    using (SolidBrush backgroundBrush = new SolidBrush(backColor))
                    using (SolidBrush textBrush = new SolidBrush(foreColor))
                    {
                        // Vẽ nền nút với góc bo tròn
                        int radius = 5;
                        Rectangle rect = new Rectangle(
                            e.CellBounds.X + 5,
                            e.CellBounds.Y + 5,
                            e.CellBounds.Width - 10,
                            e.CellBounds.Height - 10
                        );
                        using (GraphicsPath path = new GraphicsPath())
                        {
                            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
                            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
                            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
                            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
                            path.CloseFigure();

                            e.Graphics.FillPath(backgroundBrush, path);
                        }

                        // Vẽ chữ
                        SizeF textSize = e.Graphics.MeasureString(text, e.CellStyle.Font);
                        float textX = e.CellBounds.X + (e.CellBounds.Width - textSize.Width) / 2;
                        float textY = e.CellBounds.Y + (e.CellBounds.Height - textSize.Height) / 2;
                        e.Graphics.DrawString(text, e.CellStyle.Font, textBrush, textX, textY);
                    }
                }

                e.Handled = true;
            }
        }

        private void LoadUserData(string searchTerm, int roleId)
        {
            string query = @"
            SELECT u.id, u.username, u.full_name, u.email, u.phone, u.status, r.role_name AS role, u.role_id
            FROM users u
            LEFT JOIN roles r ON u.role_id = r.id
            WHERE (u.id LIKE @Search
               OR u.username LIKE @Search
               OR u.full_name LIKE @Search
               OR u.email LIKE @Search
               OR u.phone LIKE @Search)";

            // Thêm điều kiện lọc theo role nếu có
            if (roleId > 0)
            {
                query += " AND u.role_id = @RoleId";
            }

            query += " ORDER BY u.id";

            var parameters = new Dictionary<string, object>
            {
                { "@Search", $"%{searchTerm}%" }
            };

            if (roleId > 0)
            {
                parameters.Add("@RoleId", roleId);
            }

            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);
                dgvUsers.DataSource = dt;
                lblTitle.Text = $"QUẢN LÝ TÀI KHOẢN ({dt.Rows.Count})";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text;
            if (searchTerm == "Tìm kiếm người dùng...") searchTerm = "";

            int selectedRoleId = Convert.ToInt32(cboFilterRole.SelectedValue);
            LoadUserData(searchTerm, selectedRoleId);
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (var addUserForm = new AddUserForm())
            {
                if (addUserForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                        Convert.ToInt32(cboFilterRole.SelectedValue));
                }
            }
        }

        private void DgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Format status button
            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                string status = row.Cells["status"].Value?.ToString() ?? "";

                if (status == "Active")
                {
                    e.Value = "Hoạt động";
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(46, 204, 113);
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(39, 174, 96);
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
                else if (status == "Inactive")
                {
                    e.Value = "Đã khóa";
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(231, 76, 60);
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(192, 57, 43);
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
            }

            // Format grant access button
            if (e.ColumnIndex == dgvUsers.Columns["grant_access"].Index)
            {
                DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
                int roleId = Convert.ToInt32(row.Cells["role_id"].Value);

                if (roleId == 1)
                {
                    e.Value = "Hủy quyền";
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(155, 89, 182);
                }
                else
                {
                    e.Value = "Cấp quyền";
                    dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(52, 152, 219);
                }
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor =
                    roleId == 1 ? Color.FromArgb(142, 68, 173) : Color.FromArgb(41, 128, 185);
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            // Format reset password button
            if (e.ColumnIndex == dgvUsers.Columns["reset_password"].Index)
            {
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(243, 156, 18);
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(211, 84, 0);
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }

            // Format delete button
            if (e.ColumnIndex == dgvUsers.Columns["delete"].Index)
            {
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.FromArgb(231, 76, 60);
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionBackColor = Color.FromArgb(192, 57, 43);
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.SelectionForeColor = Color.White;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvUsers.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
            int userId = Convert.ToInt32(row.Cells["id"].Value);
            string username = row.Cells["username"].Value?.ToString() ?? "";
            string status = row.Cells["status"].Value?.ToString() ?? "";
            int roleId = Convert.ToInt32(row.Cells["role_id"].Value);

            // Handle status button click
            if (e.ColumnIndex == dgvUsers.Columns["status_button"].Index)
            {
                string newStatus = status == "Active" ? "Inactive" : "Active";
                string confirmMessage = status == "Active"
                    ? $"Bạn có chắc chắn muốn khóa tài khoản {username}?"
                    : $"Bạn có chắc chắn muốn kích hoạt tài khoản {username}?";

                if (DialogResult.Yes == MessageBox.Show(confirmMessage, "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ChangeUserStatus(userId, newStatus);
                }
            }
            // Handle grant access button click
            else if (e.ColumnIndex == dgvUsers.Columns["grant_access"].Index)
            {
                if (DialogResult.Yes == MessageBox.Show(
                    roleId == 1
                        ? $"Bạn có chắc chắn muốn hủy quyền quản trị của {username}?"
                        : $"Bạn có chắc chắn muốn cấp quyền quản trị cho {username}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ChangeUserRole(userId, roleId == 1 ? 2 : 1); // Toggle giữa Admin (1) và User (2)
                }
            }
            // Handle reset password button click
            else if (e.ColumnIndex == dgvUsers.Columns["reset_password"].Index)
            {
                if (DialogResult.Yes == MessageBox.Show(
                    $"Bạn có chắc chắn muốn đặt lại mật khẩu cho tài khoản {username}?\n" +
                    "Mật khẩu mới sẽ là: 'password123'",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    ResetUserPassword(userId);
                }
            }
            // Handle delete button click
            else if (e.ColumnIndex == dgvUsers.Columns["delete"].Index)
            {
                if (DialogResult.Yes == MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa tài khoản {username}?\n" +
                    "Hành động này không thể hoàn tác!",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    DeleteUser(userId);
                }
            }
        }

        private void ChangeUserStatus(int userId, string newStatus)
        {
            string query = "UPDATE users SET status = @Status WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Status", newStatus },
                { "@Id", userId }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));

                MessageBox.Show(
                    newStatus == "Active"
                        ? "Tài khoản đã được kích hoạt thành công."
                        : "Tài khoản đã bị khóa thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thay đổi trạng thái tài khoản: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChangeUserRole(int userId, int newRoleId)
        {
            string query = "UPDATE users SET role_id = @RoleId WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@RoleId", newRoleId },
                { "@Id", userId }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));

                MessageBox.Show(
                    newRoleId == 1
                        ? "Quyền quản trị đã được cấp thành công."
                        : "Quyền quản trị đã được hủy thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thay đổi vai trò tài khoản: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetUserPassword(int userId)
        {
            string defaultPassword = "password123";
            string hashedPassword = HashPassword(defaultPassword);

            string query = "UPDATE users SET password = @Password WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Password", hashedPassword },
                { "@Id", userId }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Mật khẩu đã được đặt lại thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đặt lại mật khẩu: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string HashPassword(string password)
        {
            // Đây là một hàm đơn giản để mã hóa mật khẩu
            // Trong ứng dụng thực tế, bạn nên sử dụng một thuật toán mã hóa mạnh hơn
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private void DeleteUser(int userId)
        {
            string query = "DELETE FROM users WHERE id = @Id";
            var parameters = new Dictionary<string, object>
            {
                { "@Id", userId }
            };

            try
            {
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                LoadUserData(txtSearch.Text == "Tìm kiếm người dùng..." ? "" : txtSearch.Text,
                    Convert.ToInt32(cboFilterRole.SelectedValue));

                MessageBox.Show("Tài khoản đã được xóa thành công.",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xóa tài khoản: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}