using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{
    public partial class Lab02_Bai05 : Form
    {
        public Lab02_Bai05()
        {
            InitializeComponent();
        }

        class Phim
        {
            public string ten { get; set; }

            public float giaVeChuan { get; set; }

            public string phong { get; set; }

            public float tongVe { get; set; }

            public float veThuong { get; set; }

            public float veVip { get; set; }

            public float veVot { get; set; }
            public float doanhThu { get; set; }

            public float veTon { get; set; }
            public float tyLeVeBan { get; set; }

            public Phim(string _ten, float _giaVeChuan, string _phong, float _tongVe, float _veThuong, float _veVip, float _veVot)
            {
                ten = _ten;
                giaVeChuan = _giaVeChuan;
                phong = _phong;
                tongVe = _tongVe;
                veThuong = _veThuong;
                veVip = _veVip;
                veVot = _veVot;
                doanhThu = veThuong * giaVeChuan + veVot * giaVeChuan / 4 + veVip * giaVeChuan * 2;
                veTon = tongVe - veThuong - veVip - veVot;
                tyLeVeBan = 1 - veTon / tongVe;
            }
        }
        private List<Phim> danhsachPhim = new List<Phim>();


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                using (StreamReader sr = new StreamReader(selectedFilePath))
                {
                    string line;
                    int count = 0;
                    List<string> currentPart = new List<string>();

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            currentPart.Add(line);
                        }

                        if (currentPart.Count == 7) // Check if we have a complete part
                        {
                            string ten = currentPart[0].ToString();
                            float giaVeChuan = float.Parse(currentPart[1]);
                            string phong = currentPart[2];
                            int tongVe = int.Parse(currentPart[3]);
                            int veThuong = int.Parse(currentPart[4]);
                            int veVip = int.Parse(currentPart[5]);
                            int veVot = int.Parse(currentPart[6]);

                            Phim phim = new Phim(ten, giaVeChuan, phong, tongVe, veThuong, veVip, veVot);
                            danhsachPhim.Add(phim);

                            // Reset currentPart for the next part
                            currentPart.Clear();
                        }
                    }
                }
                string directoryPath = Path.GetDirectoryName(selectedFilePath);
                string FilePath = Path.Combine(directoryPath, "output5.txt");
                using (StreamWriter sw = new StreamWriter(FilePath))
                {
                    danhsachPhim = danhsachPhim.OrderByDescending(p => p.doanhThu).ToList();
                    int i = 1;
                    foreach (Phim phim in danhsachPhim)
                    {
                        sw.WriteLine("Tên phim: " + phim.ten);
                        sw.WriteLine("Vé bán ra: "+(phim.tongVe - phim.veTon).ToString());
                        sw.WriteLine("Vé tồn: " + phim.veTon.ToString());
                        sw.WriteLine("Tỷ lệ vé bán ra: " + phim.tyLeVeBan.ToString() + "%");
                        sw.WriteLine("Tổng doanh thu: " + phim.doanhThu.ToString());
                        sw.WriteLine("Xếp hạng: " + i.ToString());
                        sw.WriteLine("\n");
                        i += 1;
                    }
                }


            }
        }


        private async void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;

            

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProgressDialogForm progressDialogForm = new ProgressDialogForm();
                progressDialogForm.ShowDialog();

                string selectedFilePath = openFileDialog.FileName;
                using (StreamReader sr = new StreamReader(selectedFilePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        richTextBox1.AppendText(line+'\n');
                    }
                }
            }
        }
    }
}
