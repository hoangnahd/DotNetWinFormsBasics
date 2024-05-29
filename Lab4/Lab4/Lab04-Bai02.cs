using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Lab04_Bai02 : Form
    {
        public Lab04_Bai02()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            string url = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    // Download the HTML content of the website
                    string html = webClient.DownloadString(url);

                    // Create and configure the SaveFileDialog
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.FileName = "website.html"; // Default filename
                    saveFileDialog.Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*";
                    saveFileDialog.Title = "Save HTML File";
                    saveFileDialog.InitialDirectory = Path.GetFullPath("../../../data/");
                    // Show the SaveFileDialog and check if the user clicked "Save"
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the HTML content to the selected path
                        File.WriteAllText(saveFileDialog.FileName, html);
                        MessageBox.Show("HTML content downloaded and saved successfully.");
                    }
                    textBox2.Text = saveFileDialog.FileName;
                    richTextBox1.AppendText(html);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

    }
}
