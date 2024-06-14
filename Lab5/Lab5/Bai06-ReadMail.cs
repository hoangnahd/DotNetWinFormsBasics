using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static Lab5.Lab05_Bai06;

namespace Lab5
{
    public partial class Bai06_ReadMail : Form
    {
        public Bai06_ReadMail()
        {
            InitializeComponent();
        }

        private void Bai06_ReadMail_Load(object sender, EventArgs e)
        {
            label4.Text = selectedInbox.From.ToString();
            label5.Text = email;
            label3.Text = selectedInbox.Subject.ToString();

            // Display body of the email
            Label lblBody = new Label();
            lblBody.Text = GetMessageBody(selectedInbox);
            lblBody.Location = new Point(1,1);
            lblBody.Width = panel2.Width; // Adjust width to fit within panel
            lblBody.Height = panel2.Height; // Adjust height to fit within panel
            lblBody.BorderStyle = BorderStyle.FixedSingle; // Add border for box-like appearance
            lblBody.Padding = new Padding(10); // Add padding inside the label

            panel2.Controls.Add(lblBody);
        }

        private string GetMessageBody(MimeMessage message)
        {
            // Example method to extract text body from MimeMessage
            if (message.TextBody != null)
            {
                return message.TextBody;
            }
            else if (message.HtmlBody != null)
            {
                // Strip HTML tags for display in TextBox
                return Regex.Replace(message.HtmlBody, "<.*?>", String.Empty);
            }
            else
            {
                return "No body content available.";
            }
        }
    }
}
