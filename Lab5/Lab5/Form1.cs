using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Xml.Linq;
using MailKit.Net.Smtp;
using MimeKit;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Lab05_Bai04 lab05_Bai04 = new Lab05_Bai04();
            lab05_Bai04.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Lab05_Bai01 lab05_Bai01 = new Lab05_Bai01();
            lab05_Bai01.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lab05_Bai02 lab05_Bai02 = new Lab05_Bai02();
            lab05_Bai02.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Lab05_Bai05 lab05_Bai05 = new Lab05_Bai05();
            lab05_Bai05.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lab05_Bai06 lab05_Bai06 = new Lab05_Bai06();
            lab05_Bai06.ShowDialog();
        }
    }
}
