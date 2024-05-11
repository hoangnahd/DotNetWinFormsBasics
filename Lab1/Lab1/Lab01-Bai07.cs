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
    public partial class Lab01_Bai07 : Form
    {
        public Lab01_Bai07()
        {
            InitializeComponent();

            // Thiết lập mặc định cho TextBox textBox1
            textBox1.Text = "Họ tên, điểm môn 1, điểm môn 2,...";
            textBox1.ForeColor = Color.LightGray;
        }

        // Xử lý sự kiện khi TextBox textBox1 được focus
        private void textBox1_Enter(object sender, EventArgs e)
        {
            // Xóa nội dung của groupBox1 và thay đổi màu và nội dung của TextBox textBox1 nếu cần
            groupBox1.Controls.Clear();
            if (textBox1.Text == "Họ tên, điểm môn 1, điểm môn 2,...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        // Xử lý sự kiện khi TextBox textBox1 không còn focus
        private void textBox1_Leave(object sender, EventArgs e)
        {
            // Thiết lập lại nội dung và màu sắc của TextBox textBox1 nếu nó rỗng
            if (textBox1.Text == "")
            {
                textBox1.Text = "Họ tên, điểm môn 1, điểm môn 2,...";
                textBox1.ForeColor = Color.LightGray;
            }
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Xác định"
        private void button1_Click_1(object sender, EventArgs e)
        {
            // Xóa nội dung của groupBox1
            groupBox1.Controls.Clear();

            // Chia nội dung của TextBox textBox1 thành mảng các chuỗi
            string[] text = textBox1.Text.Split(',');

            // Biến kiểm tra định dạng đầu vào và các thông số cần tính toán
            bool isRightFormat = true;
            float maxScore = 0, minScore = 10;
            float average = 0.0f;
            string classifier;
            int notPassed = 0;
            int typeScore = 0;

            // Kiểm tra định dạng của chuỗi nhập vào
            if (float.TryParse(text[0], out _) || text.Length < 2)
            {
                isRightFormat = false;
            }

            // Hiển thị thông tin họ tên và điểm môn học lên Label
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Text = $"Họ và tên: {text[0]}\n";
            for (int i = 1; i < text.Length; i++)
            {
                if (!float.TryParse(text[i], out _) || float.Parse(text[i]) > 10 || float.Parse(text[i]) < 0)
                {
                    isRightFormat = false;
                    break;
                }
                else
                {
                    text[i] = text[i].Trim();
                    label.Text += $"Môn {i}: {text[i]}   ";
                    average += float.Parse(text[i]);
                    maxScore = Math.Max(maxScore, float.Parse(text[i]));
                    minScore = Math.Min(minScore, float.Parse(text[i]));
                    if (float.Parse(text[i]) < 6.5 && typeScore == 0) { typeScore = 1; }

                    if (float.Parse(text[i]) < 5 && (typeScore != 3 && typeScore != 4))
                    {
                        typeScore = 2;
                    }
                    if (float.Parse(text[i]) < 5) { notPassed += 1; }
                    if (float.Parse(text[i]) < 3.5 && typeScore != 4) { typeScore = 3; }

                    if (float.Parse(text[i]) < 2) { typeScore = 4; }
                }
            }

            // Xác định phân loại và điểm trung bình
            if (average >= 8 && typeScore == 0) { classifier = "Giỏi"; }
            else if (average >= 6.5 && typeScore == 1) { classifier = "Khá"; }
            else if (average >= 5 && typeScore == 2) { classifier = "TB"; }
            else if (average >= 3.5 && typeScore == 3) { classifier = "Yếu"; }
            else { classifier = "Kém"; }

            average /= (text.Length - 1);

            // Hiển thị thông tin trên TextBox và Label
            if (isRightFormat)
            {
                label9.Text = "(Đã nhập đúng format.)";
                textBox3.Text = classifier.ToString();
                textBox2.Text = average.ToString();
                textBox4.Text = maxScore.ToString();
                textBox5.Text = minScore.ToString();
                textBox6.Text = notPassed.ToString();
                textBox7.Text = (text.Length - 1 - notPassed).ToString();
                groupBox1.Controls.Add(label);
            }
            else
            {
                label9.Text = "(Đã nhập sai format.)";
            }
            label9.ForeColor = Color.Red;
            label9.Font = new System.Drawing.Font("Arial", 7, System.Drawing.FontStyle.Regular);
        }

        // Xử lý sự kiện khi TextBox textBox2 được focus
        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        // Xử lý sự kiện khi TextBox textBox3 được focus
        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        // Xử lý sự kiện khi TextBox textBox4 được focus
        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        // Xử lý sự kiện khi TextBox textBox5 được focus
        private void textBox5_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        // Xử lý sự kiện khi TextBox textBox6 được focus
        private void textBox6_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        // Xử lý sự kiện khi TextBox textBox7 được focus
        private void textBox7_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
