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
    public partial class Bai04_Server : Form
    {
        public Bai04_Server()
        {
            InitializeComponent();
        }
        private bool stopServer = false;
        private Socket listenerSocket;

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Waiting for a connection...");
            Thread serverThread = new Thread(new ThreadStart(StartServer));
            serverThread.Start();
        }
        string receivedString = "";

        private async void StartServer()
        {
            try
            {
                listenerSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
                IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepServer);
                listenerSocket.Listen(10); // Listen backlog set to 10

                richTextBox1.AppendText("Server started. Waiting for connections...\n");

                while (!stopServer)
                {
                    // Accept connection asynchronously
                    Socket clientSocket = await listenerSocket.AcceptAsync();
                    richTextBox1.AppendText("Connection accepted from: " + clientSocket.RemoteEndPoint + "\n");

                    // Handle client asynchronously
                    _ = HandleClientAsync(clientSocket);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (listenerSocket != null)
                {
                    listenerSocket.Close();
                }
            }
        }

        private async Task HandleClientAsync(Socket clientSocket)
        {
            try
            {
                byte[] buffer = new byte[4096];
                string jsonFilePath = "../../../data/bai4.json"; // Provide the path to your JSON file
                string jsonData = File.ReadAllText(jsonFilePath);

                while (clientSocket.Connected)
                {
                    // Send pre-defined JSON data
                    richTextBox1.AppendText("Sending data to client\n");
                    if (receivedString != "")
                    {
                        jsonData = receivedString;
                    }
                    byte[] data = Encoding.UTF8.GetBytes(jsonData);
                    await clientSocket.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                    richTextBox1.AppendText("Data is sent to client\n");

                    // Receive data from client
                    richTextBox1.AppendText("Waiting data from client\n");
                    int receivedBytes = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    if (receivedBytes > 0)
                    {
                        receivedString = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                        MessageBox.Show(receivedString);
                        richTextBox1.AppendText("Received data from client\n");
                    }
                    else
                        receivedString = "";

                    // Close connection after data exchange
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    richTextBox1.AppendText("Connection closed\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
