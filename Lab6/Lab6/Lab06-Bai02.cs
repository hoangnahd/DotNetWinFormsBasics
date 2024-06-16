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
    public partial class Lab06_Bai02 : Form
    {
        public Lab06_Bai02()
        {
            InitializeComponent();
        }
        public static string Encrypt(string plaintext, string key)
        {
            plaintext = plaintext.ToUpper(); // Convert plaintext to uppercase
            string normalizedKey = NormalizeKey(plaintext, key.ToUpper()); // Normalize key to match plaintext length
            string ciphertext = "";

            for (int i = 0; i < plaintext.Length; i++)
            {
                char plainChar = plaintext[i];
                char keyChar = normalizedKey[i];
                if (plainChar == ' ') // Handle spaces by adding them directly to ciphertext
                {
                    ciphertext += ' ';
                    continue;
                }
                int offset = 'A'; // Start of ASCII uppercase letters
                int encryptedChar = (plainChar + keyChar - 2 * offset) % 26 + offset;
                ciphertext += (char)encryptedChar;
            }

            return ciphertext;
        }

        public static string Decrypt(string ciphertext, string key)
        {
            ciphertext = ciphertext.ToUpper(); // Convert ciphertext to uppercase
            string normalizedKey = NormalizeKey(ciphertext, key.ToUpper()); // Normalize key to match ciphertext length
            string plaintext = "";

            for (int i = 0; i < ciphertext.Length; i++)
            {
                char cipherChar = ciphertext[i];
                char keyChar = normalizedKey[i];
                if (cipherChar == ' ') // Handle spaces by adding them directly to plaintext
                {
                    plaintext += ' ';
                    continue;
                }
                int offset = 'A'; // Start of ASCII uppercase letters
                int decryptedChar = (cipherChar - keyChar + 26) % 26 + offset;
                plaintext += (char)decryptedChar;
            }

            return plaintext;
        }

        private static string NormalizeKey(string text, string key)
        {
            int textLength = text.Length;
            int keyLength = key.Length;
            string normalizedKey = "";

            for (int i = 0; i < textLength; i++)
            {
                char keyChar = key[i % keyLength];
                normalizedKey += keyChar;
            }

            return normalizedKey;
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
                richTextBox2.AppendText(Encrypt(richTextBox1.Text.ToUpper(), textBox1.Text.ToUpper()));
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
                richTextBox3.AppendText(Decrypt(richTextBox2.Text, textBox1.Text.ToUpper()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
