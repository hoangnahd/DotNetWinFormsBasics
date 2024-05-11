using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Lab02_Bai02 : Form
    {
        public Lab02_Bai02()
        {
            InitializeComponent();
        }
        private string directoryPath;
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                int lineCount = 0;
                int wordCount = 0;
                int charCount = 0;

                string selectedFilePath = openFileDialog.FileName;
                string fileName = openFileDialog.SafeFileName.ToString();
                textBox1.Text = fileName;
                textBox3.Text = selectedFilePath;

                FileInfo fileInfo = new FileInfo(selectedFilePath);
                long fileSize = fileInfo.Length;
                textBox2.Text = fileSize.ToString() + " bytes";
                richTextBox2.Text = File.ReadAllText(selectedFilePath);
                directoryPath = Path.GetDirectoryName(selectedFilePath);
                using (StreamReader sr = new StreamReader(selectedFilePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;

                        // Split words, considering punctuation as word separators
                        string[] words = line.Split(new[] { ' ', '\t', ',', '.', ';', ':', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                        wordCount += words.Length;

                        // Count characters (excluding whitespace)
                        charCount += line.Where(c => !Char.IsWhiteSpace(c)).Count();
                    }                
                }
                textBox4.Text = lineCount.ToString();
                textBox5.Text = wordCount.ToString();
                textBox6.Text = charCount.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
