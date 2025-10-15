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
//    public partial class LoginForm : Form
//    {
//        public LoginForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen; // Đặt form ở giữa màn hình
//            UIHelper.ApplyRoundedCorners(pnlLogin, 10);
//        }

//        private void txtUsername_TextChanged(object sender, EventArgs e)
//        {

//        }

//        private void titleLogin_Click(object sender, EventArgs e)
//        {

//        }
//    }
//}

// Code chính 1

//using System;
//using System.Windows.Forms;
//using MySql.Data.MySqlClient;
//using WareHouse.DataAccess;
//using System.Security.Cryptography;
//using System.Text;

//namespace WareHouse.Presentation.Forms
//{
//    public partial class LoginForm : Form
//    {
//        public LoginForm()
//        {
//            InitializeComponent();
//            this.StartPosition = FormStartPosition.CenterScreen;
//        }

//        private void btnLogin_Click(object sender, EventArgs e)
//        {
//            string username = txtUsername.Text;
//            string password = txtPassword.Text;

//            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//            {
//                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            try
//            {
//                // Truy vấn để lấy thông tin người dùng, bao gồm mật khẩu đã mã hóa
//                string query = "SELECT id, role_id, password FROM users WHERE username = @username";
//                using (MySqlConnection connection = DatabaseHelper.GetConnection())
//                using (MySqlCommand cmd = new MySqlCommand(query, connection))
//                {
//                    cmd.Parameters.AddWithValue("@username", username);

//                    using (MySqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            int userId = reader.GetInt32("id");
//                            int roleId = reader.GetInt32("role_id");
//                            string hashedPassword = reader.GetString("password");

//                            // Kiểm tra mật khẩu bằng BCrypt
//                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
//                            {
//                                // Chuyển hướng dựa trên roleId
//                                if (roleId == 1 || roleId == 3) // Admin hoặc Kế toán
//                                {
//                                    DashBoardForm dashBoardForm = new DashBoardForm(roleId);
//                                    dashBoardForm.ShowDialog();
//                                }
//                                else if (roleId == 2) // Nhân viên kho
//                                {
//                                    ImportWareHouseForm importForm = new ImportWareHouseForm(roleId);
//                                    importForm.ShowDialog();
//                                }
//                                else
//                                {
//                                    MessageBox.Show("Role không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                }

//                                // Đóng form đăng nhập
//                                this.Close();
//                            }
//                            else
//                            {
//                                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            }
//                        }
//                        else
//                        {
//                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        private void btnLogin_Click_1(object sender, EventArgs e)
//        {
//            string username = txtUsername.Text;
//            string password = txtPassword.Text;

//            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
//            {
//                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            try
//            {
//                // Truy vấn để lấy thông tin người dùng, bao gồm mật khẩu đã mã hóa
//                string query = "SELECT id, role_id, password_hash FROM users WHERE username = @username";
//                using (MySqlConnection connection = DatabaseHelper.GetConnection())
//                using (MySqlCommand cmd = new MySqlCommand(query, connection))
//                {
//                    cmd.Parameters.AddWithValue("@username", username);

//                    using (MySqlDataReader reader = cmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            int userId = reader.GetInt32("id");
//                            int roleId = reader.GetInt32("role_id");
//                            string hashedPassword = reader.GetString("password_hash");

//                            // Băm mật khẩu người dùng nhập bằng SHA256
//                            string inputHashedPassword = HashPassword(password);

//                            // So sánh mật khẩu đã băm
//                            if (inputHashedPassword == hashedPassword)
//                            {
//                                // Chuyển hướng dựa trên roleId
//                                if (roleId == 1 || roleId == 3) // Admin hoặc Kế toán
//                                {
//                                    ProductManagementForm dashBoardForm = new ProductManagementForm(roleId);
//                                    this.Hide();  // Ẩn form đăng nhập
//                                    dashBoardForm.Show();
//                                    dashBoardForm.FormClosed += (s, args) => this.Close();  // Khi form mới đóng, đóng form đăng nhập
//                                }
//                                else if (roleId == 2) // Nhân viên kho
//                                {
//                                    ImportWareHouseForm importForm = new ImportWareHouseForm(roleId);
//                                    this.Hide();  // Ẩn form đăng nhập
//                                    importForm.Show();
//                                    importForm.FormClosed += (s, args) => this.Close();  // Khi form mới đóng, đóng form đăng nhập
//                                }
//                                else
//                                {
//                                    MessageBox.Show("Role không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                                }
//                            }
//                            else
//                            {
//                                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                            }
//                        }
//                        else
//                        {
//                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }

//        // Phương thức băm mật khẩu bằng SHA256
//        public static string HashPassword(string password)
//        {
//            using (SHA256 sha256 = SHA256.Create())
//            {
//                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//                StringBuilder builder = new StringBuilder();
//                for (int i = 0; i < bytes.Length; i++)
//                {
//                    builder.Append(bytes[i].ToString("x2"));
//                }
//                return builder.ToString(); // Trả về chuỗi băm SHA256
//            }
//        }

//    }
//}


using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WareHouse.DataAccess;
using System.Security.Cryptography;
using System.Text;

namespace WareHouse.Presentation.Forms
{
    public partial class LoginForm : Form
    {
        public static string CurrentUsername { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LogActivity(string username, string action)
        {
            try
            {
                using (MySqlConnection connection = DatabaseHelper.GetConnection())
                {
                    string query = "INSERT INTO activity_logs (user_id, action, timestamp) VALUES ((SELECT id FROM users WHERE username = @username), @action, @timestamp)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@action", action);
                        cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ghi log: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int roleId = -1;
            bool loginSuccessful = false;

            try
            {
                using (MySqlConnection connection = DatabaseHelper.GetConnection())
                {
                    string query = "SELECT id, role_id, password_hash FROM users WHERE username = @username";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                roleId = reader.GetInt32("role_id");
                                string hashedPassword = reader.GetString("password_hash");

                                string inputHashedPassword = HashPassword(password);

                                if (inputHashedPassword == hashedPassword)
                                {
                                    loginSuccessful = true;
                                    CurrentUsername = username;
                                }
                            }
                        }
                    }
                }

                if (loginSuccessful)
                {
                    string loginAction = $"{username} đã đăng nhập thành công {DateTime.Now}";
                    LogActivity(username, loginAction);

                    Form nextForm = null;
                    if (roleId == 1 || roleId == 3) // Admin hoặc Kế toán
                    {
                        nextForm = new ProductManagementForm(roleId);
                    }
                    else if (roleId == 2) // Nhân viên kho
                    {
                        nextForm = new ImportWareHouseForm(roleId);
                    }
                    else
                    {
                        MessageBox.Show("Role không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    this.Hide();
                    nextForm.Show();
                    nextForm.FormClosed += (s, args) => this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}