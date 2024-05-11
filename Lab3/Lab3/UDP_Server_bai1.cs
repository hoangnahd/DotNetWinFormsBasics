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
using System.Diagnostics;

namespace Lab3
{
    public partial class UDP_Server_bai1 : Form
    {
        private bool isListening = false;
        private UdpClient udpClient;

        public UDP_Server_bai1()
        {
            InitializeComponent();
        }

        private void serverThread()
        {
            udpClient = new UdpClient(Int32.Parse(textBox1.Text));
            while (isListening)
            {
                try
                {
                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);
                    string mess = RemoteIpEndPoint.Address.ToString() + ":" +
                     returnData.ToString();
                    // Call InfoMessage to display the message on ListView
                    InfoMessage(mess);
                }
                catch (SocketException ex) {}
            }
            // Close the UdpClient when the server loop exits
            udpClient.Close();
        }

        private void InfoMessage(string mess)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => InfoMessage(mess)));
            }
            else
            {
                listView1.Items.Add(DateTime.Now.ToString() + ": " + mess);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Start listening
            isListening = true;
            MessageBox.Show("Server is running");
            Thread thdUDPserver = new Thread(new ThreadStart(serverThread));
            thdUDPserver.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Stop listening
            isListening = false;
            MessageBox.Show("Server is stopped");
            // Close the UdpClient if it's still open
            if (udpClient != null)
                udpClient.Close();
        }
    }
}
