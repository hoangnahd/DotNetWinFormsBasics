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
    public partial class Lab03_Bai01 : Form
    {
        public Lab03_Bai01()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UDP_Server_bai1 uDP_Server_Bai1 = new UDP_Server_bai1();
            uDP_Server_Bai1.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UDP_Client_bai1 uDP_Client_Bai1 = new UDP_Client_bai1();
            uDP_Client_Bai1.ShowDialog();
        }
    }
}
