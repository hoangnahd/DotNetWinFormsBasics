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
    public partial class Lab01_Bai08 : Form
    {
        // Khai báo List để lưu danh sách món ăn và biến đếm
        private List<string> monAn = new List<string>();
        private int Count = 0;

        public Lab01_Bai08()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Thêm món ăn"
        private void button1_Click(object sender, EventArgs e)
        {
            // Thêm món ăn vào danh sách và hiển thị trên RichTextBox
            monAn.Add(textBox1.Text);
            richTextBox1.AppendText(textBox1.Text + Environment.NewLine);
            textBox1.Text = "";
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Chọn món ăn"
        private void button2_Click(object sender, EventArgs e)
        {
            if (monAn == null || monAn.Count == 0)
            {
                MessageBox.Show("Bạn phải nhập ít nhất 1 món ăn");
            }
            else
            {
                // Chọn ngẫu nhiên một món ăn từ danh sách và hiển thị trên TextBox
                Random random = new Random();
                int randomIndex = random.Next(monAn.Count);
                textBox2.Text = monAn[randomIndex];
            }
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Đóng"
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Xóa món ăn cuối cùng"
        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Lines.Length > 0)
            {
                // Tạo một mảng mới để lưu tất cả các dòng trừ dòng cuối cùng
                string[] newLines = new string[richTextBox1.Lines.Length - 1];

                // Sao chép tất cả các dòng trừ dòng cuối cùng vào mảng mới
                Array.Copy(richTextBox1.Lines, newLines, richTextBox1.Lines.Length - 1);

                // Thiết lập mảng mới của các dòng cho RichTextBox
                richTextBox1.Lines = newLines;
                Count += 1;
            }

            if (monAn.Count > 0 && Count > 1)
            {
                // Xóa phần tử cuối cùng khỏi danh sách món ăn
                monAn.RemoveAt(monAn.Count - 1);
            }
        }

        // Xử lý sự kiện khi TextBox textBox2 được focus
        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
