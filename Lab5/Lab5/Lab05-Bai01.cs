using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;

namespace Lab5
{
    public partial class Lab05_Bai01 : Form
    {
        public Lab05_Bai01()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new MailKit.Net.Smtp.SmtpClient();
                client.Connect("smtp.gmail.com", 465, true); // smtp host, port, use ssl.
                client.Authenticate(textBox2.Text, "bclh jrgg tmkp jjyq"); // gmail account, app password
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("anhanh", textBox2.Text));
                message.To.Add(new MailboxAddress("", textBox3.Text));
                message.Subject = textBox1.Text;
                message.Body = new TextPart("plain") // gửi ở dạng plain text, hoặc có thể thay
                {
                    Text = richTextBox1.Text
                };
                client.Send(message);
                MessageBox.Show("Đã gửi thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
        }
    }
}
