using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse.Models
{
    internal class ComboBoxItem
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public decimal GiaNhap { get; set; } // Đổi từ ImportPrice thành GiaNhap để đồng bộ

        public override string ToString()
        {
            return FullName;
        }
    }
}
