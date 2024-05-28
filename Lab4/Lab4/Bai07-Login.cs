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

namespace Lab4
{
    public partial class Bai07_Login : Form
    {
        public Bai07_Login()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Bai07_SignUp bai07_Signup = new Bai07_SignUp();
            bai07_Signup.ShowDialog();
        }
        public string accessToken { get; private set; }
        public string tokenType { get; private set; }
        private async void button1_Click(object sender, EventArgs e)
        {
            var url = "https://nt106.uitiot.vn/auth/token";
            var username = textBox1.Text; // replace with your username
            var password = textBox2.Text; // replace with your password
            using (var client = new HttpClient())
            {
                // Get the access token
                var content = new MultipartFormDataContent
                {
                     { new StringContent(username), "username" },
                     { new StringContent(password), "password" }
                };
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseString);
                if (!response.IsSuccessStatusCode)
                {
                    var detail = responseObject["detail"].ToString();
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu");
                    return;
                }

                tokenType = responseObject["token_type"].ToString();
                accessToken = responseObject["access_token"].ToString();
                Lab04_Bai07 lab04_Bai07 = new Lab04_Bai07(this);
                lab04_Bai07.ShowDialog();
                this.Close();
            }
        }
    }
}
