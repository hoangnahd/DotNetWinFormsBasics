using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace Lab3
{
    public partial class Bai06_Client : Form
    {
        public Bai06_Client()
        {
            InitializeComponent();

        }
        private TcpClient tcpClient;
        class Message
        {
            public string messages { get; set; }
            public string from { get; set; }
            public string to { get; set; }

        }
        private List<Message> messages;
        private Dictionary<string, string> privateMessage = new Dictionary<string, string>();
        private void button1_Click(object sender, EventArgs e)
        {

            tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 8080);
            // Send data to the server
            if (textBox1.Text != "")
            {
                SendData("name:" + textBox1.Text.Trim());
                comboBox2.Items.Add("Group");
                // Start a background thread for receiving messages from the server
                Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.Start();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text != "" && comboBox2.SelectedItem?.ToString() != null)
            {
                // Start a background thread to continuously receive data from the server
                messages = new List<Message>();
                Message message = new Message();
                message.from = textBox1.Text.Trim();
                message.to = comboBox2.SelectedItem?.ToString() != "Group" ? comboBox2.SelectedItem?.ToString() : "";
                message.messages = textBox2.Text.Trim();
                messages.Add(message);
                string jsonData = JsonConvert.SerializeObject(messages);
                SendData(jsonData);
                
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            
        }

        private void SendData(string message)
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        string CheckAndReplace(string data, string textBox1Text)
        {
            // Split the data by ':'
            string[] parts = data.Split(':');

            // Check if the first part equals textBox1Text
            if (parts.Length > 0 && parts[0] == textBox1Text)
            {
                // Replace the first part with "Me"
                parts[0] = "Me";
            }

            // Reconstruct the string
            string newData = string.Join(":", parts);

            return newData;
        }


        private void ReceiveData()
        {
            NetworkStream stream = tcpClient.GetStream();
            byte[] buffer = new byte[2048];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read data from the server
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (dataReceived.StartsWith("newMember:"))
                    {
                        richTextBox3.Clear();
                        richTextBox3.AppendText(dataReceived.Substring(10));
                        foreach(string user in dataReceived.Substring(10).Split('\n'))
                        {
                            if (!comboBox2.Items.Contains(user.Trim()) && textBox1.Text.Trim() != user)
                            {
                                comboBox2.Items.Add(user.Trim());
                            }
                        }
                    }
                    else
                    {
                        bool isPublic = true ? dataReceived.StartsWith("public:"):false;

                        string[] data = isPublic? dataReceived.Substring(7).Trim().Split('\n'): dataReceived.Trim().Split('\n');
                        string updateData = "";
                        string to = data[0].Split(':')[0];
                        if (data[0].Split(':').Count() > 2)
                        {
                            data[0] = data[0].Substring(to.Count()+1);
                        }
                        foreach (string s in data)
                        {
                            updateData += CheckAndReplace(s, textBox1.Text.Trim()) + '\n';
                        }
                        if (isPublic)
                        {
                            richTextBox1.Clear();
                            richTextBox1.AppendText(updateData);
                        }
                        else
                        {
                            privateMessage[to.Trim()] = updateData;
                            if (updateData.Split('\n')[0].Split(':')[0] != "Me")
                            {
                                richTextBox2.AppendText("New message from " + to +" at "+ DateTime.Now.ToString()+'\n');
                            }
                            if (!comboBox1.Items.Contains(to.Trim()))
                            {
                                comboBox1.Items.Add(to.Trim());
                            }
                            comboBox1_SelectedIndexChanged(null, EventArgs.Empty);
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
            tcpClient.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected item from comboBox1
            string selectedItem = comboBox1.SelectedItem?.ToString();

            // Check if the selected item is not null and exists in the privateMessage HashTable
            if (selectedItem != null && privateMessage.ContainsKey(selectedItem))
            {
                // Retrieve the value associated with the selected item
                string privateMsg = (string)privateMessage[selectedItem];

                // Do something with the private message (e.g., display it)
                richTextBox2.Clear();
                richTextBox2.AppendText(privateMsg);
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    
    }
}
