using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WareHouse.Presentation.Forms;
using WareHouse.DataAccess;

namespace WareHouse
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        static void Main()
        {
            SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            // Khởi tạo kết nối khi chạy chương trình
            DatabaseHelper.InitializeConnection();

            Application.Run(new LoginForm());

            // Đóng kết nối khi thoát ứng dụng
            DatabaseHelper.CloseConnection();
        }
    }
}
