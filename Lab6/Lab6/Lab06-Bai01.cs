using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6
{
    public partial class Lab06_Bai01 : Form
    {
        public Lab06_Bai01()
        {
            InitializeComponent();
        }
        private string CaesarEncrypt(string text, int shift)
        {
            char[] buffer = text.ToCharArray();
            for (int i = 0; i < buffer.Length; i++)
            {
                char letter = buffer[i];
                if (char.IsLetter(letter))
                {
                    // Determine if the letter is uppercase or lowercase for correct ASCII range
                    char offset = char.IsUpper(letter) ? 'A' : 'a';
                    letter = (char)((((letter + shift) - offset) % 26) + offset);

                    // Handle negative shifts
                    if (letter < offset)
                    {
                        letter = (char)(letter + 26);
                    }
                }
                buffer[i] = letter;
            }
            return new string(buffer);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin");
                return;
            }
            try
            {
                richTextBox2.Clear();
                richTextBox2.AppendText(CaesarEncrypt(richTextBox1.Text, int.Parse(textBox1.Text.Trim())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox3.Clear();
                richTextBox3.AppendText(CaesarEncrypt(richTextBox2.Text, -int.Parse(textBox1.Text.Trim())));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
