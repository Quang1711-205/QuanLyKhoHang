using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using WareHouse.DataAccess;

namespace WareHouse.Presentation.Forms
{
    public partial class AddUserForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtPhone;
        private ComboBox cmbRole;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnSave;
        private Button btnCancel;

        public AddUserForm()
        {
            InitializeComponents();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeComponents()
        {
            this.Text = "Thêm Người Dùng Mới";
            this.Size = new Size(400, 500);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            TableLayoutPanel tblLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 8,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };
            tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tblLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            for (int i = 0; i < 8; i++)
                tblLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            this.Controls.Add(tblLayout);

            // Username
            tblLayout.Controls.Add(new Label
            {
                Text = "Tên người dùng:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 0);
            txtUsername = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F)
            };
            tblLayout.Controls.Add(txtUsername, 1, 0);

            // Full Name
            tblLayout.Controls.Add(new Label
            {
                Text = "Họ tên:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 1);
            txtFullName = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F)
            };
            tblLayout.Controls.Add(txtFullName, 1, 1);

            // Email
            tblLayout.Controls.Add(new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 2);
            txtEmail = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F)
            };
            tblLayout.Controls.Add(txtEmail, 1, 2);

            // Phone
            tblLayout.Controls.Add(new Label
            {
                Text = "Số điện thoại:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 3);
            txtPhone = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F)
            };
            tblLayout.Controls.Add(txtPhone, 1, 3);

            // Role
            tblLayout.Controls.Add(new Label
            {
                Text = "Vai trò:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 4);
            cmbRole = new ComboBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new[]
            {
new { Text = "Quản trị viên", Value = 1 },
new { Text = "Nhân viên nhập", Value = 2 },
new { Text = "Nhân viên xuất", Value = 3 },
new { Text = "Kế toán", Value = 4 }
});
            cmbRole.DisplayMember = "Text";
            cmbRole.ValueMember = "Value";
            cmbRole.SelectedIndex = 0;
            tblLayout.Controls.Add(cmbRole, 1, 4);

            // Password
            tblLayout.Controls.Add(new Label
            {
                Text = "Mật khẩu:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 5);
            txtPassword = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                UseSystemPasswordChar = true
            };
            tblLayout.Controls.Add(txtPassword, 1, 5);

            // Confirm Password
            tblLayout.Controls.Add(new Label
            {
                Text = "Xác nhận mật khẩu:",
                Font = new Font("Segoe UI", 10F),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = true
            }, 0, 6);
            txtConfirmPassword = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                UseSystemPasswordChar = true
            };
            tblLayout.Controls.Add(txtConfirmPassword, 1, 6);

            // Buttons
            FlowLayoutPanel flpButtons = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill
            };
            btnCancel = new Button
            {
                Text = "Hủy",
                Font = new Font("Segoe UI", 10F),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White
            };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;
            flpButtons.Controls.Add(btnCancel);

            btnSave = new Button
            {
                Text = "Lưu",
                Font = new Font("Segoe UI", 10F),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White
            };
            btnSave.Click += BtnSave_Click;
            flpButtons.Controls.Add(btnSave);
            tblLayout.Controls.Add(flpButtons, 0, 7);
            tblLayout.SetColumnSpan(flpButtons, 2);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập email!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for duplicate username or email
            string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @Username OR email = @Email";
            var checkParams = new Dictionary<string, object>
{
{ "@Username", txtUsername.Text },
{ "@Email", txtEmail.Text }
};
            try
            {
                DataTable dt = DatabaseHelper.ExecuteQuery(checkQuery, checkParams);
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    MessageBox.Show("Tên người dùng hoặc email đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra trùng lặp: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hash password (placeholder SHA256, adjust if using different hashing)
            string passwordHash = ComputeSha256Hash(txtPassword.Text);

            // Insert user
            string insertQuery = @"
INSERT INTO users (username, password_hash, full_name, email, phone, role_id, status)
VALUES (@Username, @PasswordHash, @FullName, @Email, @Phone, @RoleId, 'Active')";
            var insertParams = new Dictionary<string, object>
{
{ "@Username", txtUsername.Text },
{ "@PasswordHash", passwordHash },
{ "@FullName", txtFullName.Text },
{ "@Email", txtEmail.Text },
{ "@Phone", txtPhone.Text },
{ "@RoleId", ((dynamic)cmbRole.SelectedItem).Value },
{ "@Status", "Active" }
};

            try
            {
                DatabaseHelper.ExecuteNonQuery(insertQuery, insertParams);
                MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thêm người dùng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}