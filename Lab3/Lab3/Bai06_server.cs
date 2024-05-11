using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Lab3
{
    public partial class Bai06_server : Form
    {
        public Bai06_server()
        {
            InitializeComponent();
        }
        class Message
        {
            public string messages { get; set; }
            public string from { get; set; }
            public string to { get; set; }

        }
        private TcpListener tcpListener;
        private List<TcpClient> clients = new List<TcpClient>();
        private List<string> partipants = new List<string>();
        private List<string> messageGroup = new List<string>();
        private Dictionary<string, Dictionary<string, List<string>>> images = new Dictionary<string, Dictionary<string, List<string>>>();
        private Dictionary<string, Dictionary<string, List<string>>> messagePrivate = new Dictionary<string, Dictionary<string, List<string>>>();
        private Dictionary<string, TcpClient> connectedClients = new Dictionary<string, TcpClient>();
        //string temp;

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Waiting for connection...\n");
            tcpListener = new TcpListener(IPAddress.Any, 8080);
            tcpListener.Start();

            // Start accepting client connections asynchronously
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var kvp in connectedClients)
            {
                TcpClient client = kvp.Value;
                client.Close();
                
            }
            this.Close();
        }
        private void displayMessage(string message)
        {
            richTextBox1.AppendText(message);
        }
        private void HandleClientConnection(IAsyncResult ar)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(ar);
            clients.Add(client);
            displayMessage($"Client connected: {client.Client.RemoteEndPoint}\n");

            // Start listening for messages from the client
            Thread receiveThread = new Thread(new ParameterizedThreadStart(ReceiveData));
            receiveThread.Start(client);

            // Continue accepting more client connections
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(HandleClientConnection), null);
        }
        private void ReceiveData(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[2048];
            int bytesRead;
            bool isGetLength = false;

            while (true)
            {
                try
                {
                    // Read data from the client
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (dataReceived.StartsWith("name:"))
                    {

                        partipants.Add(dataReceived.Substring(5));
                        connectedClients[partipants[partipants.Count - 1]] = clients[clients.Count - 1];
                        string sentData = "newMember:";
                        sentData += string.Join("\n", partipants);
                        // Send back the received data to all clients (broadcast)
                        Broadcast(sentData);
                    }
                    else
                    {
                        string data;
                        List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(dataReceived);

                        if (messages[0].to == "")
                        {
                            messageGroup.Add(messages[0].from + ":" + messages[0].messages);
                            data = "public:" + string.Join("\n", messageGroup);
                            Broadcast(data);
                        }
                        else
                        {
                            string from = messages[0].from;
                            string to = messages[0].to;
                            string message = messages[0].from + ":" + messages[0].messages;

                            // Check if the 'from' key exists in the outer dictionary
                            if (!messagePrivate.ContainsKey(from))
                            {
                                // If 'from' key doesn't exist, add it along with an empty inner dictionary
                                messagePrivate[from] = new Dictionary<string, List<string>>();
                            }

                            // Check if the 'to' key exists in the inner dictionary corresponding to 'from'
                            if (!messagePrivate[from].ContainsKey(to))
                            {
                                // If 'to' key doesn't exist, add it along with an empty list
                                messagePrivate[from][to] = new List<string>();
                            }

                            // Add the message to the list corresponding to 'from' and 'to'
                            messagePrivate[from][to].Add(message);
                            data = string.Join("\n", messagePrivate[messages[0].from][messages[0].to]);
                            privateMessage(connectedClients[messages[0].to], messages[0].from + ":" + data);
                            privateMessage(connectedClients[messages[0].from], messages[0].to + ":" + data);
                        }
                    }
                                  
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    break;
                }
            }

            stream.Close();
            client.Close();
        }
        private void privateMessage(TcpClient client, string message)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        private void Broadcast(string message)
        {
            foreach (var kvp in connectedClients)
            {
                TcpClient client = kvp.Value;
                NetworkStream stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }


    }
}
