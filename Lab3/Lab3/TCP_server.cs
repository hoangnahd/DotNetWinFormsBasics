using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace Lab3
{
    public partial class TCP_server : Form
    {
        public TCP_server()
        {
            InitializeComponent();
        }
        private bool stopServer = false;
        private Socket listenerSocket;
        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Waiting for a connection...");
            Thread serverThread = new Thread(new ThreadStart(StartUnsafeThread));
            serverThread.Start();
        }

        private async void StartUnsafeThread()
        {
            byte[] buffer = new byte[2048];
            // Tạo socket bên gởi
            Socket clientSocket;

            Socket listenerSocket = new Socket(
            AddressFamily.InterNetwork,
            SocketType.Stream,
            ProtocolType.Tcp
            );
            IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            listenerSocket.Bind(ipepServer);
            listenerSocket.Listen(-1);
            clientSocket = listenerSocket.Accept();
            TcpClient tcpClient = new TcpClient();
            tcpClient.Client = clientSocket;
            NetworkStream stream = tcpClient.GetStream();
            richTextBox1.AppendText("New client connected\n");
            richTextBox1.AppendText("Connection accepted from 127.0.0.1:8080\n");
            while (clientSocket.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();
                richTextBox1.AppendText(message + '\n');
            }
            listenerSocket.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stopServer = true;
            if (listenerSocket != null)
            {
                listenerSocket.Close();
            }
            foreach (Form form in Application.OpenForms)
            {
                form.Close();
            }
        }
    }
}
