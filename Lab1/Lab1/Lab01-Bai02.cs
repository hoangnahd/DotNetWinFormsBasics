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
    public partial class Lab01_Bai02 : Form
    {
        public Lab01_Bai02()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện khi nút "Tính" được nhấn
        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra và tính toán số lớn nhất và nhỏ nhất trong ba số nhập vào
            if (float.TryParse(textBox1.Text, out float num1) &&
                float.TryParse(textBox2.Text, out float num2) &&
                float.TryParse(textBox4.Text, out float num3))
            {
                // Hiển thị số lớn nhất trong textBox3
                textBox3.Text = (Math.Max(num1, Math.Max(num2, num3))).ToString();

                // Hiển thị số nhỏ nhất trong textBox6
                textBox6.Text = (Math.Min(num1, Math.Min(num2, num3))).ToString();
            }
            else
            {
                // Hiển thị thông báo lỗi nếu đầu vào không hợp lệ
                MessageBox.Show("Vui lòng nhập số");
            }
        }

        // Xử lý sự kiện khi nút "Xóa" được nhấn
        private void button2_Click(object sender, EventArgs e)
        {
            // Đặt lại giá trị của các ô nhập liệu về "0"
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox4.Text = "0";
        }

        // Xử lý sự kiện khi nút "Đóng" được nhấn
        private void button3_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.Close();
        }

        // Xử lý sự kiện khi textBox3 được nhập
        private void textBox3_Enter(object sender, EventArgs e)
        {
            // Focus được đặt vào textBox1 khi textBox3 được nhập
            textBox1.Focus();
        }

        // Xử lý sự kiện khi textBox6 được nhập
        private void textBox6_Enter(object sender, EventArgs e)
        {
            // Focus được đặt vào textBox1 khi textBox6 được nhập
            textBox1.Focus();
        }
    }
}
