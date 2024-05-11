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

namespace Lab2
{
    public partial class Lab02_Bai01 : Form
    {
        public Lab02_Bai01()
        {
            InitializeComponent();
        }
        private string directoryPath;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                directoryPath = Path.GetDirectoryName(selectedFilePath);
                using (StreamReader sr = new StreamReader(selectedFilePath))
                {
                    richTextBox1.Text = sr.ReadToEnd();
                }
            }
            else
            {
                MessageBox.Show("File không hợp lệ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string FilePath = Path.Combine(directoryPath, "output1.txt");
            using (StreamWriter sw = new StreamWriter(FilePath))
            {
          
                string[] lines = richTextBox1.Lines;

                // Write each line to the file
                foreach (string line in lines)
                {
                    sw.WriteLine(line.ToUpper());
                }
                MessageBox.Show("Đã viết thành công vào output1.txt");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
