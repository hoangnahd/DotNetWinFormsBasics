using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;
using static Lab5.Lab05_Bai06;

namespace Lab5
{
    public partial class Bai06_SendEmail : Form
    {
        public Bai06_SendEmail()
        {
            InitializeComponent();
        }
        string FilePath;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var message = new MimeMessage();

                // Set the sender's email address
                message.From.Add(new MailboxAddress(textBox2.Text, email));

                // Set the recipient's email address
                message.To.Add(new MailboxAddress("", textBox5.Text));

                // Set the subject of the email
                message.Subject = textBox4.Text;

                // Determine the type of message body (HTML or plain text)
                string typeSend = checkBox1.Checked ? "html" : "plain";

                // Create the text part of the email body
                var textPart = new TextPart(typeSend)
                {
                    Text = richTextBox1.Text
                };

                // Create a multipart/mixed container to hold the text and the attachment (if any)
                var multipart = new Multipart("mixed");
                multipart.Add(textPart);

                // Check if FilePath is set and valid to include an attachment
                if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
                {
                    // Create the attachment part
                    var attachment = new MimePart()
                    {
                        Content = new MimeContent(File.OpenRead(FilePath)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(FilePath)
                    };

                    // Add the attachment to the multipart container
                    multipart.Add(attachment);
                }

                // Set the multipart as the message body
                message.Body = multipart;

                // Send the email
                client.Send(message);

                MessageBox.Show("Email sent successfully!");
            }
            catch (Exception ex)
            {
                // Show error message if something goes wrong
                MessageBox.Show("Error sending email: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set the initial directory to "../../../data/"
            openFileDialog.InitialDirectory = Path.GetFullPath("../../../data/");

            // Set the filter to display all files
            openFileDialog.Filter = "All files (*.*)|*.*";

            // Check if the user has selected a file
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get the full file path of the selected file
                string filePath = openFileDialog.FileName;

                // Get the directory path of the selected file
                string directoryPath = Path.GetFullPath(filePath);

                // Optionally, you can also store the full file path in a separate variable
                FilePath = filePath;

                // Display the directory path in textBox3
                textBox3.Text = directoryPath;
            }
        }


        private void Bai06_SendEmail_Load(object sender, EventArgs e)
        {
            textBox1.Text = email;
        }
    }
}
