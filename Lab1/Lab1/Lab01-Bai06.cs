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
    public partial class Lab01_Bai06 : Form
    {
        public Lab01_Bai06()
        {
            InitializeComponent();

            // Thiết lập mặc định cho TextBox textBox1
            textBox1.Text = "dd/mm/yy";
            textBox1.ForeColor = Color.LightGray;
        }

        // Xử lý sự kiện khi TextBox textBox1 được focus
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "dd/mm/yy")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        // Xử lý sự kiện khi TextBox textBox1 không còn focus
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "dd/mm/yy";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Xác định cung hoàng đạo"
        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem ngày tháng năm sinh nhập vào có hợp lệ không
            if (DateTime.TryParseExact(textBox1.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime birthday))
            {
                int month = birthday.Month;
                int day = birthday.Day;

                // Xác định cung hoàng đạo tương ứng với ngày tháng sinh
                switch (month)
                {
                    case 1:
                        textBox2.Text = (day >= 21) ? "Bảo Bình" : "Ma Kết";
                        break;
                    case 2:
                        textBox2.Text = (day >= 20) ? "Song Ngư" : "Bảo Bình";
                        break;
                    case 3:
                        textBox2.Text = (day >= 21) ? "Bạch Dương" : "Song Ngư";
                        break;
                    case 4:
                        textBox2.Text = (day >= 21) ? "Kim Ngưu" : "Bạch Dương";
                        break;
                    case 5:
                        textBox2.Text = (day >= 22) ? "Song Tử" : "Kim Ngưu";
                        break;
                    case 6:
                        textBox2.Text = (day >= 22) ? "Cự Giải" : "Song Tử";
                        break;
                    case 7:
                        textBox2.Text = (day >= 23) ? "Sư Tử" : "Cự Giải";
                        break;
                    case 8:
                        textBox2.Text = (day >= 23) ? "Xử Nữ" : "Sư Tử";
                        break;
                    case 9:
                        textBox2.Text = (day >= 24) ? "Thiên Bình" : "Xử Nữ";
                        break;
                    case 10:
                        textBox2.Text = (day >= 24) ? "Thần Nông" : "Thiên Bình";
                        break;
                    case 11:
                        textBox2.Text = (day >= 23) ? "Nhân Mã" : "Thần Nông";
                        break;
                    case 12:
                        textBox2.Text = (day >= 22) ? "Ma Kết" : "Nhân Mã";
                        break;
                }

                textBox2.ReadOnly = true; // Đặt TextBox textBox2 chỉ đọc

            }
            else
            {
                MessageBox.Show("Ngày tháng năm sinh không hợp lệ");
            }
        }

        // Xử lý sự kiện khi TextBox textBox2 được focus
        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox1.Focus(); // Chuyển focus về TextBox textBox1
        }
    }
}
