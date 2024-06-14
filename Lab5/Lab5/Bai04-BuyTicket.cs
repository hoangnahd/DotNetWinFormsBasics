using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using MailKit.Net.Smtp;
using Button = System.Windows.Forms.Button;

namespace Lab5
{
    public partial class Bai04_BuyTicket : Form
    {
        public Bai04_BuyTicket()
        {
            InitializeComponent();
        }
        Dictionary<string, bool> selectedSeat = new Dictionary<string, bool>();

       
        private void Bai04_BuyTicket_Load(object sender, EventArgs e)
        {
            // Attach click event handlers to all buttons within panel1.
            foreach (Control control in panel1.Controls)
            {
                if (control is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            // Toggle the background color of the clicked button between yellow and its original color
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                if (clickedButton.BackColor == Color.Yellow)
                {
                    // Change back to normal color
                    clickedButton.BackColor = SystemColors.Control; // Or any other default color
                    selectedSeat[clickedButton.Text] = false;
                }
                else
                {
                    // Change to yellow
                    clickedButton.BackColor = Color.Yellow;
                    selectedSeat[clickedButton.Text] = true;
                }
            }
        }
        private void danhDauGheDaMua(string name)
        {
            foreach (Control control in this.Controls)
            {
                // Nếu control là button và có tên trùng với tên của ghế được chọn
                if (control is Button button && button.Text.Trim() == name)
                {
                    // Lưu lại button ghế được chọn
                    button.BackColor = Color.Orange;
                }
            }
        }
        private void danhDauGhe(string name)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button && button.Text.Trim() == name)
                {
                    // Lưu lại button ghế được chọn
                    button.BackColor = Color.White;
                }
            }
        }
        private void dat_mua_Click(object sender, EventArgs e)
        {
            string soGhe = "";
            foreach (var ghe in selectedSeat)
            {
                if(ghe.Value)
                    soGhe += ghe.Key+", ";
            }
            MessageBox.Show(soGhe);
            if (soGhe.Length > 0)
            {
                soGhe = soGhe.TrimEnd(',', ' ');
            }
            MessageBox.Show(soGhe);
            var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true); // smtp host, port, use ssl.
            client.Authenticate("shopeebot0001@gmail.com", "bclh jrgg tmkp jjyq"); // gmail account, app password

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("anhanh", "shopeebot0001@gmail.com"));
            message.To.Add(new MailboxAddress("", textBox1.Text));
            message.Subject = "Xác nhận đặt vé xem phim";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $@"
            <html>
            <body style='background-image: url({Lab05_Bai04.film.ImgUrl}); background-size: cover; background-position: center; padding: 20px; color: white;'>
                <div style='background-color: rgba(0, 0, 0, 0.7); padding: 20px; border-radius: 10px;'>
                    <h1 style='text-align: center;'>Xác nhận đặt vé xem phim</h1>
                    <p>Xin chào {chonTen.Text} ,</p>
                    <p>Cảm ơn bạn đã đặt vé xem phim tại rạp của chúng tôi. Dưới đây là thông tin chi tiết về vé của bạn:</p>
                    <ul>
                        <li><strong>Tên phim:</strong> {Lab05_Bai04.film.Title}</li>
                        <li><strong>Số ghế:</strong> {soGhe}</li>
                    </ul>
                    <p>Chúng tôi hy vọng bạn sẽ có những giây phút thư giãn và thú vị tại rạp của chúng tôi.</p>
                    <p><strong>{"Đắm chìm vào từng khoảnh khắc"}</strong></p>
                    <p>Chúc bạn một ngày tốt lành và hẹn gặp lại!</p>
                </div>
            </body>
            </html>";


            message.Body = bodyBuilder.ToMessageBody();

            client.Send(message);
            MessageBox.Show("Đã đặt vé thành công!");
            client.Disconnect(true);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
