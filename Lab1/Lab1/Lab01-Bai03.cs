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
    public partial class Lab01_Bai03 : Form
    {
        public Lab01_Bai03()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện khi nút "Chuyển đổi" được nhấn
        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra và chuyển đổi số nhập vào thành chữ tương ứng
            if (!int.TryParse(textBox1.Text, out _) ||  // Kiểm tra xem đầu vào có phải là số không
                int.Parse(textBox1.Text) > 9 ||        // Kiểm tra xem số nhập vào có lớn hơn 9 không
                int.Parse(textBox1.Text) < 0)          // Kiểm tra xem số nhập vào có nhỏ hơn 0 không
            {
                // Hiển thị thông báo lỗi nếu số không hợp lệ và đặt lại giá trị của textBox1
                MessageBox.Show("Số không hợp lệ!!");
                textBox1.Text = "";
            }
            else
            {
                // Chuyển đổi số thành chữ và hiển thị kết quả trong textBox2
                switch (int.Parse(textBox1.Text))
                {
                    case 0:
                        textBox2.Text = "Không";
                        break;
                    case 1:
                        textBox2.Text = "Một";
                        break;
                    case 2:
                        textBox2.Text = "Hai";
                        break;
                    case 3:
                        textBox2.Text = "Ba";
                        break;
                    case 4:
                        textBox2.Text = "Bốn";
                        break;
                    case 5:
                        textBox2.Text = "Năm";
                        break;
                    case 6:
                        textBox2.Text = "Sáu";
                        break;
                    case 7:
                        textBox2.Text = "Bảy";
                        break;
                    case 8:
                        textBox2.Text = "Tám";
                        break;
                    case 9:
                        textBox2.Text = "Chín";
                        break;
                }
            }
        }

        // Xử lý sự kiện khi nút "Xóa" được nhấn
        private void button2_Click(object sender, EventArgs e)
        {
            // Đặt lại giá trị của textBox1 về rỗng
            textBox1.Text = "";
        }

        // Xử lý sự kiện khi nút "Đóng" được nhấn
        private void button3_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.Close();
        }

        // Xử lý sự kiện khi textBox2 được nhập
        private void textBox2_Enter(object sender, EventArgs e)
        {
            // Focus được đặt vào textBox1 khi textBox2 được nhập
            textBox1.Focus();
        }
    }
}
