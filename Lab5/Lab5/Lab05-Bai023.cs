using System;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MailKit.Net.Pop3;
using System.Collections.Generic;

namespace Lab5
{
    public partial class Lab05_Bai02 : Form
    {
        public Lab05_Bai02()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.Columns.Add("Email", 250);
            listView1.Columns.Add("From", 200);
            listView1.Columns.Add("Thời gian", 150);
        }
        private void Imap()
        {
            var client = new ImapClient();
            client.Connect("imap.gmail.com", 993, true); // imap host, port, use ssl.
            client.Authenticate(textBox2.Text, textBox3.Text); // gmail accout, app password.
            var inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);
            for (int i = 0; i < inbox.Count; i++)
            {
                var message = inbox.GetMessage(i);
                // xử lý để hiển thị email lên listview: message.Subject; message.From;
                var listViewItem = new ListViewItem(message.Subject);
                listViewItem.SubItems.Add(message.From.ToString());
                listViewItem.SubItems.Add(message.Date.ToString());

                listView1.Items.Add(listViewItem);
            }

        }

        private void Pop()
        {
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true); // imap host, port, use ssl.
            client.Authenticate(textBox2.Text, textBox3.Text); // gmail accout, app password.
            for (int i = 0; i < client.Count; i++)
            {
                var message = client.GetMessage(i);
                // xử lý để hiển thị email lên listview: message.Subject; message.From;
                var listViewItem = new ListViewItem(message.Subject);
                listViewItem.SubItems.Add(message.From.ToString());
                listViewItem.SubItems.Add(message.Date.ToString());

                listView1.Items.Add(listViewItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string selectMethod = comboBox1?.SelectedItem.ToString();
            if (selectMethod == "imap")
                Imap();
            else if (selectMethod == "pop")
                Pop();
        }
    }
}
