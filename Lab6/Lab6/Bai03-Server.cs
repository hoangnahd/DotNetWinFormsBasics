using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace Lab6
{
    public partial class Bai03_Server : Form
    {
        public Bai03_Server()
        {
            InitializeComponent();
        }
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<string> resources = new List<string>();
        TcpClient client;
        NetworkStream stream;
        public void Start(string serverIp)
        {
            tcpListener = new TcpListener(IPAddress.Parse(serverIp), 1234);
            tcpListener.Start();
            richTextBox1.AppendText("Server started, waiting for clients...");

            // Start accepting client connections asynchronously
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void HandleClientConnection(IAsyncResult ar)
        {      
            TcpClient client = null;
            client = tcpListener.EndAcceptTcpClient(ar);
            // Add client to list and start a new thread to receive data
            clients.Add(client);
            richTextBox1.AppendText("\nClient connected.");

            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            receiveThread.Start(client);
            

            // Continue accepting more client connections
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void ReceiveData(object clientObj)
        {
            client = (TcpClient)clientObj;
            stream = client.GetStream();

            byte[] buffer = new byte[2048];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read data from the client
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // Connection closed
                        break;
                    }
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    resources.Add(dataReceived);
                    string data2send = "";
                    for (int i = 0; i < resources.Count; i++) 
                    {
                        data2send += resources[i].ToString() + '\n';
                    }
                    Broadcast(data2send);
                }
                catch (Exception ex)
                {
                    richTextBox1.AppendText($"Error: {ex.Message}");
                    break;
                }
            }
            // Clean up when the client disconnects
            stream.Close();
            client.Close();
            clients.Remove(client);
            richTextBox1.AppendText("Client disconnected");
        }
        
        private void Broadcast(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            foreach (TcpClient client in clients)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Broadcast error: {ex.Message}");
                }
            }
        }
        public string GetLocalIPAddress()
        {
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Start(GetLocalIPAddress());
            textBox1.Text = GetLocalIPAddress();
            textBox1.Enabled = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            stream.Close();
            client.Close();
            Application.Exit();
        }
    }
}
