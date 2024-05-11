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
    public partial class Lab03_Bai03 : Form
    {
        public Lab03_Bai03()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TCP_server tCP_Server = new TCP_server();
            tCP_Server.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TCP_client tCP_Client = new TCP_client();
            tCP_Client.ShowDialog();
        }
    }
}
