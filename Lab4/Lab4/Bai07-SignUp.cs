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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab4
{
    public partial class Bai07_SignUp : Form
    {
        public Bai07_SignUp()
        {
            InitializeComponent();
        }
        public class User
        {
            public string username { get; set; }
            public string email { get; set; }
            public string password { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public int sex { get; set; }
            public string birthday { get; set; }
            public string language { get; set; }
            public string phone { get; set; }
        }

        private async void subBtn_Click(object sender, EventArgs e)
        {

            var url = "https://nt106.uitiot.vn/api/v1/user/signup";
            
            using (var client = new HttpClient())
            {
                // Get the access token
                var user = new User
                {
                    username = username.Text.Trim(),
                    password = password.Text.Trim(),
                    email = email.Text.Trim(),
                    first_name = fname.Text.Trim(),
                    last_name = lname.Text.Trim(),
                    sex = male.Checked ? 1 : 0,
                    birthday = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                    language = selectLang.SelectedText.Trim(),
                    phone = phone.Text.Trim(),

                };
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseString);
                if (!response.IsSuccessStatusCode)
                {
                    var detail = responseObject["detail"].ToString();
                    MessageBox.Show($"Detail: {detail}");
                    return;
                }
                MessageBox.Show("Đăng ký thành công!");
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

        private void fmale_CheckedChanged(object sender, EventArgs e)
        {
            if(fmale.Checked)
                male.Checked = false;
        }
        private void male_CheckedChanged(object sender, EventArgs e)
        {

            if (male.Checked)
                fmale.Checked = false;
        }
    }
}
