using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Bai03_Client : Form
    {
        public Bai03_Client()
        {
            InitializeComponent();
        }
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] EncryptData(string dataToEncrypt, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataToEncryptBytes = Encoding.UTF8.GetBytes(dataToEncrypt);
                byte[] encryptedData = rsa.Encrypt(dataToEncryptBytes, false);
                return encryptedData;
            }
        }

        private string DecryptData(byte[] dataToDecrypt, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        private void SendData(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            stream.Write(buffer, 0, buffer.Length);
        }
        private void DisplayMailBox(string resources)
        {
            listView1.Clear();
            List<string> encryptedMessage = new List<string>(resources.Split('\n'));

            listView1.View = View.Details;
            listView1.Columns.Add("#", 25);
            listView1.Columns.Add("Encrypted resource", 700);

            for (int i = 0; i < encryptedMessage.Count; i++)
            {
                // xử lý để hiển thị email lên listview: message.Subject; message.From;
                var listViewItem = new ListViewItem((i + 1).ToString());
                listViewItem.SubItems.Add(encryptedMessage[i]);
                listView1.Items.Add(listViewItem);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                tcpClient = new TcpClient(textBox2.Text, 1234);
                stream = tcpClient.GetStream();
                Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
                receiveThread.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void ReceiveData()
        {
            byte[] buffer = new byte[2048];
            int bytesRead;

            while (true)
            {
                try
                {
                    // Read data from the server
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DisplayMailBox(dataReceived);
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

        private void button3_Click(object sender, EventArgs e)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKey = rsa.ToXmlString(false); // public key
            string privateKey = rsa.ToXmlString(true); // private key

            // Encrypt the text in textBox3 using the public key
            byte[] encryptedData = EncryptData(textBox3.Text, publicKey);

            // Convert encrypted data to a base64 string
            string encryptedDataString = Convert.ToBase64String(encryptedData);
            SendData(encryptedDataString);

            try
            {
                // Create a SaveFileDialog instance
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                // Set initial directory to ../../../data
                saveFileDialog.InitialDirectory = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\data\"));

                // Set default file name (you can change this if needed)
                saveFileDialog.FileName = "key.txt";

                // Set filter if needed
                // saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                // saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file path
                    string filePath = saveFileDialog.FileName;

                    // Save the private key to the selected file
                    File.WriteAllText(filePath, privateKey);

                    MessageBox.Show("Private key saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving key: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            string encryptedMessage = listView1.SelectedItems[0].SubItems[1].Text.ToString();
            byte[] data = Convert.FromBase64String(encryptedMessage); // Convert from base64 to byte array

            // Create an OpenFileDialog to select a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Path.GetFullPath(Path.Combine(Application.StartupPath, @"..\..\..\data\"));
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of the selected file
                    string filePath = openFileDialog.FileName;

                    // Read the text from the selected file
                    string fileContent;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        fileContent = reader.ReadToEnd();
                    }

                    // Decrypt the data using the private key from the file
                    string decryptedMessage = DecryptData(data, fileContent);

                    // Display the decrypted message
                    MessageBox.Show(decryptedMessage, "Decrypted Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Bai03_Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            tcpClient.Close();
            stream.Close();
            var openForms = Application.OpenForms.Cast<Form>().ToList();

            // Close each form
            foreach (var form in openForms)
            {
                form.Close();
            }

            // Exit the application
            Application.Exit();
        }

    }
}
