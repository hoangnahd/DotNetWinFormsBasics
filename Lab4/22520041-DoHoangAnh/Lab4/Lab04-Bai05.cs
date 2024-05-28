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
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace Lab4
{
    public partial class Lab04_Bai05 : Form
    {
        public Lab04_Bai05()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            var url = textBox1.Text.Trim();
            var username = textBox2.Text.Trim(); // replace with your username
            var password = textBox3.Text.Trim(); // replace with your password
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
                    richTextBox1.AppendText($"Detail: {detail}");
                    return;
                }

                var tokenType = responseObject["token_type"].ToString();
                var accessToken = responseObject["access_token"].ToString();
                richTextBox1.AppendText($"Token Type: {tokenType}\n");
                richTextBox1.AppendText($"Access Token: {accessToken}\n");
                richTextBox1.AppendText("\nĐăng nhập thành công");

                // Use the access token to authenticate a GET request
                client.DefaultRequestHeaders.Authorization = new
                System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var getUserUrl = "https://nt106.uitiot.vn/api/v1/user/me";
                var getUserResponse = await client.GetAsync(getUserUrl);
                var getUserResponseString = await
                getUserResponse.Content.ReadAsStringAsync();
                Console.WriteLine(getUserResponseString);
            }
        }
    }
}
