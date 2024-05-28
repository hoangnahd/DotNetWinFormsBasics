using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Lab4
{
    public partial class Bai07_themMonAn : Form
    {
        private Bai07_Login login;
        private string accessToken;
        private string tokenType;
        public Bai07_themMonAn(Bai07_Login Login)
        {
            InitializeComponent();
            this.login = Login;
            accessToken = login.accessToken;
            tokenType = login.tokenType;
        }

        class monAn
        {
            public string ten_mon_an { get; set; }
            public float gia { get; set; }
            public string mo_ta { get; set; }
            public string hinh_anh { get; set; }
            public string dia_chi { get; set; }
        }
        private async void subBtn_Click(object sender, EventArgs e)
        {
            if (tenMonAn.Text == null || Gia.Text == null || 
                diachi.Text == null || textBox1.Text.Trim() == null || 
                richTextBox1.Text == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }
            var url = "https://nt106.uitiot.vn/api/v1/monan/add";
            string imageString = textBox1.Text.Trim();


            using (var client = new HttpClient())
            {

                // Get the access token
                var monan = new monAn
                {
                    ten_mon_an = tenMonAn.Text,
                    gia = float.Parse(Gia.Text),
                    mo_ta = richTextBox1.Text,
                    hinh_anh = imageString,
                    dia_chi = diachi.Text,
                };
                var json = JsonConvert.SerializeObject(monan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new
                System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseString);
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, accessToken);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"error");
                    return;
                }
                MessageBox.Show("Thêm món ăn thành công!!");
                this.Close();
            }

        }
        private void ClearAllTextBoxes(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = string.Empty;
                }
                else if (c.HasChildren)
                {
                    ClearAllTextBoxes(c);
                }
            }
        }
        private void clearBtn_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxes(this);
        }
    }
}
