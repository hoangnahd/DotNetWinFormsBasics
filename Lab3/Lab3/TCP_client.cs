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
    public partial class TCP_client : Form
    {
        public TCP_client()
        {
            InitializeComponent();
        }
        private TcpClient tcpClient;
        private NetworkStream ns;
        private void button1_Click(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);
            tcpClient.Connect(ipEndPoint);
            ns = tcpClient.GetStream();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.ASCII.GetBytes(richTextBox1.Text);
            ns.Write(data, 0, data.Length);
            richTextBox1.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Send a termination message to the server if needed
            // For example, if the server expects a "quit" message
            byte[] quitMessage = Encoding.ASCII.GetBytes("quit\n");
            ns.Write(quitMessage, 0, quitMessage.Length);

            // Close the network stream and the TCP client
            ns.Close();
            tcpClient.Close();
        }
    }
}
