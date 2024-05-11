using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lab03_Bai01 lab03_Bai01 = new Lab03_Bai01();
            lab03_Bai01.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lab03_Bai02 lab03_Bai02 = new Lab03_Bai02();
            lab03_Bai02.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lab03_Bai03 lab03_Bai03 = new Lab03_Bai03();
            lab03_Bai03.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Lab03_Bai04 lab03_Bai04 = new Lab03_Bai04();
            lab03_Bai04.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lab03_Bai05 lab03_Bai05 = new Lab03_Bai05();
            lab03_Bai05.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Lab03_Bai06 lab03_Bai06 = new Lab03_Bai06();
            lab03_Bai06.ShowDialog();
        }
    }
}
