using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Lab4.Lab04_Bai07;

namespace Lab4
{
    public partial class Bai07_DelFood : Form
    {
        private string accessToken;
        private string tokenType;
        public Bai07_DelFood(Bai07_Login login)
        {
            InitializeComponent();
            this.accessToken = login.accessToken;
            this.tokenType = login.tokenType;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                // Set the authorization header
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                // Make the DELETE request
                var response = await client.DeleteAsync("https://nt106.uitiot.vn/api/v1/monan/" + del_food.id);

                // Ensure a successful response
                if (response.IsSuccessStatusCode)
                {
                    // Assuming you need to set the authorization header again with tokenType and accessToken
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, accessToken);
                    MessageBox.Show("Đã xóa thành công!!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công!!");
                    this.Close();
                }
            }
        }
    }
}
