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
    public partial class Lab06_Bai03 : Form
    {
        public Lab06_Bai03()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bai03_Server bai03_Server = new Bai03_Server();
            bai03_Server.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai03_Client bai03_Client = new Bai03_Client();
            bai03_Client.ShowDialog();
        }
    }
}
