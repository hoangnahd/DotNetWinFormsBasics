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
    public partial class Lab04_Bai03 : Form
    {
        public Lab04_Bai03()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            webView21.Source = new Uri(textBox1.Text.Trim());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 != null)
            {
                webView21.Reload();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
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
                    saveFileDialog.InitialDirectory = Path.GetFullPath("../../../data/"); // Set the default directory

                    // Show the SaveFileDialog and check if the user clicked "Save"
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Save the HTML content to the selected path
                        File.WriteAllText(saveFileDialog.FileName, html);
                        MessageBox.Show("HTML content downloaded and saved successfully.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Please enter a valid URL.");
                return;
            }

            using (WebClient webClient = new WebClient())
            {
                string html = webClient.DownloadString(url);
                HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);

                // Extract all image URLs
                var imageNodes = document.DocumentNode.SelectNodes("//img[@src]");
                if (imageNodes == null)
                {
                    MessageBox.Show("No images found on the website.");
                    return;
                }

                // Create a folder dialog to select where to save images
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string folderPath = folderDialog.SelectedPath;

                        foreach (var imgNode in imageNodes)
                        {
                            string imgUrl = imgNode.GetAttributeValue("src", "");
                            if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
                            {
                                Uri baseUri = new Uri(url);
                                imgUrl = new Uri(baseUri, imgUrl).ToString();
                            }

                            string fileName = Path.GetFileName(new Uri(imgUrl).LocalPath);
                            string filePath = Path.Combine(folderPath, fileName);

                            try
                            {
                                webClient.DownloadFile(imgUrl, filePath);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to download {imgUrl}: {ex.Message}");
                            }
                        }

                        MessageBox.Show("Images downloaded successfully.");
                    }
                }
            }
        }

        
    }
}
