using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.Models
{
    public class Product
    {
        public int Id { get; set; }         // ID sản phẩm
        public string TenSanPham { get; set; } // Tên sản phẩm
        public decimal GiaNhap { get; set; }   // Giá nhập
        public int SoLuong { get; set; }       // Số lượng

        public override string ToString()
        {
            return TenSanPham; // Hiển thị tên sản phẩm trong ComboBox
        }
    }
}
