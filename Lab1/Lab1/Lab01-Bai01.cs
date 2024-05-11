using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Lab01_Bai01 : Form
    {
        public Lab01_Bai01()
        {
            InitializeComponent();

        }

        // Xử lý sự kiện cho nút "Tính"
        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đầu vào trong textBox1 và textBox2 có thể chuyển đổi thành số nguyên không
            if (int.TryParse(textBox1.Text, out int num1) &&
                int.TryParse(textBox2.Text, out int num2))
            {
                // Chuyển đổi đầu vào thành số nguyên
                num1 = Int32.Parse(textBox1.Text.Trim());
                num2 = Int32.Parse(textBox2.Text.Trim());

                // Tính tổng của num1 và num2
                int sum = num1 + num2;

                // Hiển thị tổng trong textBox3
                textBox3.Text = sum.ToString();
            }
            else
            {
                // Hiển thị thông báo lỗi nếu đầu vào không hợp lệ
                MessageBox.Show("Vui lòng nhập số nguyên!");
            }
        }

        // Xử lý sự kiện cho nút "Xóa"
        private void button2_Click(object sender, EventArgs e)
        {
            // Đặt lại văn bản trong textBox1 và textBox2 thành "0"
            textBox1.Text = "0";
            textBox2.Text = "0";
        }

        // Xử lý sự kiện cho nút "Đóng"
        private void button3_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.Close();
        }

        // Xử lý sự kiện cho sự kiện Enter của textBox3
        private void textBox3_Enter(object sender, EventArgs e)
        {
            // Khi textBox3 được nhập, focus được đặt vào textBox1
            textBox1.Focus();
        }
    }
}
