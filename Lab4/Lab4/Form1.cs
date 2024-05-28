using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lab04_Bai01 lab04_Bai01 = new Lab04_Bai01();
            lab04_Bai01.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lab04_Bai02 lab04_Bai02 = new Lab04_Bai02();
            lab04_Bai02.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lab04_Bai03 lab04_Bai03 = new Lab04_Bai03();
            lab04_Bai03.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Lab04_Bai04 lab04_Bai04 = new Lab04_Bai04();
            lab04_Bai04.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Lab04_Bai05 lab04_Bai05 = new Lab04_Bai05();
            lab04_Bai05.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Lab04_Bai06 lab04_Bai06 = new Lab04_Bai06();
            lab04_Bai06.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bai07_Login lab04_Bai07 = new Bai07_Login();
            lab04_Bai07.ShowDialog();
        }
    }
}
