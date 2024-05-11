using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab01
{
    public partial class Lab01_Bai3 : Form
    {
        public Lab01_Bai3()
        {
            InitializeComponent();

        }

        // Phương thức chuyển đổi số có ba chữ số thành chữ tiếng Việt
        private string ReadThreeDigitNumber(long number)
        {
            string[] ones = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string[] tens = { "", "mười", "hai mươi", "ba mươi", "bốn mươi", "năm mươi", "sáu mươi", "bảy mươi", "tám mươi", "chín mươi" };

            // Tách số thành hàng trăm, hàng chục và hàng đơn vị
            long hundred = number / 100;
            long remainder = number % 100;
            long ten = remainder / 10;
            long one = remainder % 10;

            string result = "";

            // Hàng trăm
            if (hundred >= 0)
            {
                result += ones[hundred] + " trăm ";
            }

            // Hàng chục và hàng đơn vị
            if (ten == 0 && one != 0)
            {
                result += "linh " + ones[one];
            }
            else if (ten == 1)
            {
                result += tens[ten] + " " + ones[one];
            }
            else if (ten > 1)
            {
                result += tens[ten];
                if (one == 1)
                {
                    result += " mốt";
                }
                else if (one > 1)
                {
                    result += " " + ones[one];
                }
            }
            else if (remainder > 0)
            {
                if (one == 1)
                {
                    result += "mốt";
                }
                else
                {
                    result += ones[remainder];
                }
            }

            return result.Trim();
        }

        // Xử lý sự kiện khi đổi giá trị trong textBox1
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            // Định dạng số người dùng nhập vào
            if (long.TryParse(textBox1.Text.Replace(",", ""), out long num))
            {
                textBox1.Text = string.Format("{0:N0}", num);
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.SelectionLength = 0;
            }

            // Kiểm tra số ký tự nhập vào, nếu lớn hơn 15 thì không cho nhập thêm
            if (textBox1.Text.Length >= 15)
            {
                textBox1.ReadOnly = true;
            }
            else
            {
                textBox1.ReadOnly = false;
            }
        }

        // Xử lý sự kiện khi nút "Chuyển đổi" được nhấn
        private void button1_Click(object sender, EventArgs e)
        {
            string Digit = textBox1.Text.Replace(",", "");
            if (long.TryParse(Digit, out long num))
            {
                long part = num / 1000000000;
                textBox2.Text = ReadThreeDigitNumber(part) + " tỷ ";

                part = (num % 1000000000) / 1000000;
                textBox2.Text += ReadThreeDigitNumber(part) + " triệu ";

                part = (num % 1000000) / 1000;
                textBox2.Text += ReadThreeDigitNumber(part) + " nghìn ";

                part = num % 1000;
                textBox2.Text += ReadThreeDigitNumber(part) + " đồng ";
            }
            else
            {
                MessageBox.Show("Số không hợp lệ!");
            }
        }

        // Xử lý sự kiện khi nút "Xóa" được nhấn
        private void button2_Click(object sender, EventArgs e)
        {
            // Xóa giá trị trong textBox1
            textBox1.Text = "";
        }

        // Xử lý sự kiện khi nút "Đóng" được nhấn
        private void button3_Click(object sender, EventArgs e)
        {
            // Đóng form
            this.Close();
        }

        // Xử lý sự kiện khi textBox2 nhận focus
        private void textBox2_Enter(object sender, EventArgs e)
        {
            // Focus được đặt vào textBox1
            textBox1.Focus();
        }
    }
}
