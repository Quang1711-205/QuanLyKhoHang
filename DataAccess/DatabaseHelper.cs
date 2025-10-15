using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WareHouse.DataAccess
{
    internal class DatabaseHelper
    {
        private static string connectionString = "server=localhost;database=warehouse;user=root;password=123456;";
        private static MySqlConnection connection;

        // Hàm khởi tạo kết nối khi ứng dụng chạy
        public static void InitializeConnection()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("✅ Kết nối MySQL thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi kết nối MySQL: " + ex.Message);
            }
        }

        // Lấy kết nối để dùng trong các Form hoặc class khác
        public static MySqlConnection GetConnection()
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                InitializeConnection();
            }
            return connection;
        }

        // Hàm đóng kết nối khi ứng dụng kết thúc
        public static void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("🔒 Kết nối MySQL đã đóng.");
            }
        }

        // Phương thức để thực hiện truy vấn SQL và trả về DataTable
        public static DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return dt;
        }

        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi truy vấn cơ sở dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return new DataTable();
                }
            }
        }

        //ExecuteQuery - có tham số Param
        // Phương thức để thực hiện truy vấn SQL và trả về DataTable(có hỗ trợ tham số)
        public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = new DataTable();
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Thêm parameters nếu có
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            return dt;
        }

        // In your DatabaseHelper class
        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            int rowsAffected = 0;

            using (MySqlConnection connection = new MySqlConnection("server=localhost;database=warehouse;user=root;password=123456;"))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters if provided
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return rowsAffected;
        }

        //public static DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        // Kiểm tra và tạo kết nối nếu cần
        //        if (connection == null || connection.State != ConnectionState.Open)
        //        {
        //            InitializeConnection();
        //        }

        //        using (MySqlCommand command = new MySqlCommand(query, connection))
        //        {
        //            // Thêm parameters nếu có
        //            if (parameters != null)
        //            {
        //                foreach (var param in parameters)
        //                {
        //                    command.Parameters.AddWithValue(param.Key, param.Value);
        //                }
        //            }

        //            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
        //            {
        //                adapter.Fill(dt);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log lỗi và thông báo
        //        Console.WriteLine("Lỗi trong ExecuteQuery: " + ex.Message);
        //        MessageBox.Show("Lỗi truy vấn dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    return dt;
        //}

        // Sửa ExecuteNonQuery để sử dụng connection toàn cục
        public static int ExecuteNonQuery1(string query, Dictionary<string, object> parameters = null)
        {
            int rowsAffected = 0;

            try
            {
                // Sử dụng connection toàn cục thay vì tạo mới
                if (connection == null || connection.State != ConnectionState.Open)
                {
                    InitializeConnection();
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters if provided
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }

                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return rowsAffected;
        }

        public static int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null, MySqlTransaction transaction = null, MySqlConnection connection = null)
        {
            bool closeConnection = false;
            try
            {
                if (connection == null)
                {
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                    closeConnection = true;
                }

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    if (transaction != null)
                    {
                        cmd.Transaction = transaction;
                    }

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                        }
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thực hiện ExecuteNonQuery: {ex.Message}", ex);
            }
            finally
            {
                if (closeConnection && connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    


        public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null, MySqlTransaction transaction = null, MySqlConnection connection = null)
                {
                    bool closeConnection = false;
                    try
                    {
                        if (connection == null)
                        {
                            connection = new MySqlConnection(connectionString);
                            connection.Open();
                            closeConnection = true;
                        }

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            if (transaction != null)
                            {
                                cmd.Transaction = transaction;
                            }

                            if (parameters != null)
                            {
                                foreach (var param in parameters)
                                {
                                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                                }
                            }

                            return cmd.ExecuteScalar();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Lỗi khi thực hiện ExecuteScalar: {ex.Message}", ex);
                    }
                    finally
                    {
                        if (closeConnection && connection != null && connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                    }
                }

        // Phương thức thực hiện truy vấn và trả về giá trị đầu tiên của kết quả
        public static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            object result = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Thêm parameters nếu có
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }
                        result = command.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi cơ sở dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return result;
        }

        // Phương thức băm mật khẩu bằng SHA256
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
                return builder.ToString(); // Trả về chuỗi băm SHA256
            }
        }


    }
}


