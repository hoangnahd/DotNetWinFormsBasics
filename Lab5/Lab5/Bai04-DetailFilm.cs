using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Bai04_DetailFilm : Form
    {
        public Bai04_DetailFilm()
        {
            InitializeComponent();
        }

        private void Bai04_DetailFilm_Load(object sender, EventArgs e)
        {
            webView21.Source = new Uri("https://betacinemas.vn" + Lab05_Bai04.film.Href);
            this.Text = "https://betacinemas.vn" + Lab05_Bai04.film.Href;
        }
    }
}
