using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Lab06_Bai01 lab06_Bai01 = new Lab06_Bai01();
            lab06_Bai01.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lab06_Bai02 lab06_Bai02 = new Lab06_Bai02();
            lab06_Bai02.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Lab06_Bai03 lab06_Bai03 = new Lab06_Bai03();
            lab06_Bai03.ShowDialog();
        }
    }
}
