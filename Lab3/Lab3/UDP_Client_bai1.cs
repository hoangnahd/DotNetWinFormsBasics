using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class UDP_Client_bai1 : Form
    {
        public UDP_Client_bai1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                UdpClient udpClient = new UdpClient();
                //Do ý đồ muốn gởi dữ liệu là “Hello World?” sang bên nhận. Nên cần chuyển chuỗi Hello
                Byte[] sendBytes = Encoding.ASCII.GetBytes(richTextBox1.Text);
                //Gởi dữ liệu mà không cần thiết lập kết nối với Server
                udpClient.Send(sendBytes, sendBytes.Length, textBox1.Text, Int32.Parse(textBox2.Text));

                MessageBox.Show("Message sent to the broadcast address");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
