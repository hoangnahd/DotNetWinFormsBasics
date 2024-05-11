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
    public partial class Lab01_Bai05 : Form
    {
        public Lab01_Bai05()
        {
            InitializeComponent();
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Tính toán" hoặc "Hiển thị bảng cửu chương"
        private void button1_Click(object sender, EventArgs e)
        {
            // Xóa nội dung của groupBox trước khi thêm nội dung mới
            groupBox1.Controls.Clear();
            int result = 1;
            // Kiểm tra nếu combobox chọn tính toán được chọn và là "Tính toán giá trị"
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Tính toán giá trị")
            {
                // Kiểm tra liệu người dùng đã nhập số hợp lệ cho num1 và num2
                if (int.TryParse(textBox1.Text, out int num1) && int.TryParse(textBox2.Text, out int num2))
                {
                    // Tính giá trị của (num1 - num2)!
                    for (int i = 1; i <= num1 - num2; i++)
                    {
                        result *= i;
                    }
                    // Tạo label để hiển thị kết quả tính toán
                    Label label = new Label();
                    label.Text = $"Giá trị của ({num1}-{num2})! là: " + result.ToString() + "\n\nS= ";
                    double S = 0;
                    // Tính tổng S = A^1 + A^2 + ... + A^num2
                    for (int i = 1; i <= num2; i++)
                    {
                        S += Math.Pow(num1, i);
                        if (i == num2)
                            label.Text += $"{num1}^{i} = {S}";
                        else
                            label.Text += $"{num1}^{i} + ";

                    }
                    label.Dock = DockStyle.Fill; // Đặt label để lấp đầy không gian có sẵn

                    groupBox1.Controls.Add(label); // Thêm label vào groupBox
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng dữ liệu!");
                }
            }
            // Kiểm tra nếu combobox chọn tính toán được chọn và là "Bảng cửu chương"
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Bảng cửu chương")
            {
                TextBox textBox = new TextBox();
                textBox.Multiline = true;  // Cho phép đa dòng cho TextBox
                textBox.ScrollBars = ScrollBars.Vertical;  // Hiển thị thanh cuộn dọc

                // Kiểm tra liệu người dùng đã nhập số hợp lệ cho num1 và num2
                if (int.TryParse(textBox1.Text, out int num1) && int.TryParse(textBox2.Text, out int num2))
                {
                    // Tạo bảng cửu chương từ num2 đến num1
                    for (int i = num2; i <= num1; i++)
                    {
                        for (int j = 1; j <= 10; j++)
                        {
                            textBox.Text += $"{i} x {j} = {i * j}\r\n"; // Thêm một dòng vào TextBox
                        }
                    }
                    textBox.Dock = DockStyle.Fill; // Đặt TextBox để lấp đầy không gian có sẵn
                    groupBox1.Controls.Add(textBox); // Thêm TextBox vào groupBox
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đúng dữ liệu!");
                }
            }
            
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Xóa"
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); // Xóa nội dung của textBox1
            textBox2.Clear(); // Xóa nội dung của textBox2
        }

        // Xử lý sự kiện khi người dùng nhấn nút "Đóng"
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close(); // Đóng form
        }

        // Xử lý sự kiện khi groupBox được chọn
        private void groupBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Focus(); // Đặt focus vào textBox1
        }
    }
}
