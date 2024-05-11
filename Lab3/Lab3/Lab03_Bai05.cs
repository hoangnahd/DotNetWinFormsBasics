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
    public partial class Lab03_Bai05 : Form
    {
        public Lab03_Bai05()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bai05_Server server = new Bai05_Server();
            server.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bai05_Client client = new Bai05_Client();
            client.ShowDialog();
        }
    }
}
