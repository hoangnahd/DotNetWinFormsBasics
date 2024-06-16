using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using MailKit.Net.Imap;
using static Lab5.Lab05_Bai04;
using System.Net.Mail;

namespace Lab5
{
    public partial class Lab05_Bai06 : Form
    {
        public Lab05_Bai06()
        {
            InitializeComponent();
        }
        static public MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
        ImapClient imapClient = new ImapClient();
        static public MimeMessage selectedInbox;
        static public string email;
        private void DisplayMailBox()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("#", 25);
            listView1.Columns.Add("From", 300);
            listView1.Columns.Add("Subject", 200);
            listView1.Columns.Add("Datetime", 170);
            
            
            var inbox = imapClient.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            int maxLength = inbox.Count > 70 ? 70 : inbox.Count;
            for (int i = 0; i < maxLength; i++)
            {
                var message = inbox.GetMessage(i);
                // xử lý để hiển thị email lên listview: message.Subject; message.From;
                var listViewItem = new ListViewItem((i+1).ToString());
                listViewItem.SubItems.Add(message.From.ToString());
                listViewItem.SubItems.Add(message.Subject);
                listViewItem.SubItems.Add(message.Date.ToString());
                listView1.Items.Add(listViewItem);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

            listView1.Clear();
            
            if(button1.Text == "Đăng nhập")
            {
                client.Connect("smtp.gmail.com", 465, true); // imap host, port, use ssl.
                client.Authenticate(textBox1.Text, textBox2.Text); // gmail accout, app password.
                imapClient.Connect("imap.gmail.com", 993, true);
                imapClient.Authenticate(textBox1.Text, textBox2.Text);
                DisplayMailBox();
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                button2.Visible = true;
                button3.Visible = true;
                button1.Text = "Đăng xuất";
                email = textBox1.Text;
            }
            else
            {
                client.Disconnect(true);
                imapClient.Disconnect(true);
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button2.Visible = false;
                button3.Visible = false;
                button1.Text = "Đăng nhập";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.Disconnect(true);
            imapClient.Disconnect(true);
            client.Connect("smtp.gmail.com", 465, true); // imap host, port, use ssl.
            client.Authenticate(textBox1.Text, textBox2.Text); // gmail accout, app password.
            imapClient.Connect("imap.gmail.com", 993, true);
            imapClient.Authenticate(textBox1.Text, textBox2.Text);
            listView1.Clear();
            DisplayMailBox();
        }

        private void Lab05_Bai06_Load(object sender, EventArgs e)
        {
            button2.Visible = false;
            button3.Visible = false;
            button1.Text = "Đăng nhập";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai06_SendEmail bai06_SendEmail = new Bai06_SendEmail();
            bai06_SendEmail.ShowDialog();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            var inbox = imapClient.Inbox;
            selectedInbox = inbox.GetMessage(int.Parse(listView1.SelectedItems[0].SubItems[0].Text.ToString())-1);
            Bai06_ReadMail bai06_ReadMail = new Bai06_ReadMail();
            bai06_ReadMail.ShowDialog();
        }
    }

}
