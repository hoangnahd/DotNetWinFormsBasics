using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab4
{
    public partial class Lab04_Bai07 : Form
    {
        private Bai07_Login login;
        private string accessToken;
        private string tokenType;
        private List<food_info> foods = new List<food_info>();
        private bool displayAll = true;
        static public food_info del_food;
        public class food_info
        {
            public string ten_mon_an { get; set; }
            public Image hinh_anh { get; set; }
            public string gia { get; set; }
            public string dia_chi { get; set; }
            public string nguoi_dong_gop { get; set; }
            public string id { get; set; }

            public food_info(Image _img, string _ten_mon_an, string _gia, string _dia_chi, string _nguoi_dong_gop, string _id)
            {
                hinh_anh = _img;
                ten_mon_an = _ten_mon_an;
                gia = _gia;
                dia_chi = _dia_chi;
                nguoi_dong_gop = _nguoi_dong_gop;
                id = _id;
            }
        }
        static public food_info random_food;

        public Lab04_Bai07(Bai07_Login Login)
        {
            InitializeComponent();
            
            this.login = Login;
            accessToken = login.accessToken;
            tokenType = login.tokenType;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 100;
            comboBox1.Enabled = false;
            
        }
        private async void DisplayPage(string url)
        {
            using (var client = new HttpClient())
            {
                var content = new
                {
                    current = int.Parse(comboBox1.SelectedItem.ToString()),
                    pageSize = int.Parse(comboBox2.SelectedItem.ToString())
                };
                var httpContent = new StringContent(
                                                    JsonConvert.SerializeObject(content),
                                                    Encoding.UTF8,
                                                    "application/json"
                                                    );
                client.DefaultRequestHeaders.Authorization = new
                System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.PostAsync(url, httpContent);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JObject.Parse(responseString);
                client.DefaultRequestHeaders.Authorization = new
                    System.Net.Http.Headers.AuthenticationHeaderValue(tokenType, accessToken);
                listBox1.Items.Clear();
                int dataSize = responseObject["data"].Type == JTokenType.Array
                   ? responseObject["data"].Count()
                   : responseObject["data"].Children().Count();
                int size = Math.Min(dataSize, int.Parse(comboBox2.SelectedItem.ToString()));
                foods.Clear();
                for (int i = 0; i < size; i++)
                {
                    try
                    {
                        // Extract the information from the response object
                        string imageUrl = responseObject["data"][i]["hinh_anh"].ToString();
                        string tenMonAn = responseObject["data"][i]["ten_mon_an"].ToString();
                        string gia = responseObject["data"][i]["gia"].ToString();
                        string diaChi = responseObject["data"][i]["dia_chi"].ToString();
                        string nguoiDongGop = responseObject["data"][i]["nguoi_dong_gop"].ToString();
                        string id = responseObject["data"][i]["id"].ToString();
                        // Create an Image object from the file path
                        Image image = null;
                        using (var webClient = new WebClient())
                        {
                            using (var stream = webClient.OpenRead(imageUrl))
                            {
                                image = Image.FromStream(stream);
                            }
                        }
                        // Create a new food_info object
                        food_info foodItem = new food_info(image, tenMonAn, gia, diaChi, nguoiDongGop, id); ;
                        // Add the food_info object to the list
                        listBox1.Items.Add(foodItem);
                        foods.Add(foodItem);
                    }
                    catch { }
                    
                }

            }
        }
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Check if the index is valid
            if (e.Index < 0 || e.Index >= foods.Count) return;

            // Get the food_info item to be drawn
            food_info item = foods[e.Index];
            
            // Define padding and image size
            int imageSize = 95; // Adjust based on your image size

            // Draw the background of the item
            e.DrawBackground();

            // Calculate the bounds for the image
            Rectangle imageRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 5, imageSize, imageSize);

            // Draw the image
            e.Graphics.DrawImage(item.hinh_anh, imageRect);

            // Calculate the bounds for the text
            Rectangle textRect = new Rectangle(e.Bounds.X + 4 + imageSize + 4, e.Bounds.Y + 4, e.Bounds.Width - imageSize - 3 * 4, e.Bounds.Height - 2 * 4);
            using (SolidBrush textBrush = new SolidBrush(Color.IndianRed))
            {
                Label tenMonAn = new Label
                {
                    Text = item.ten_mon_an,
                    Font = new Font(e.Font.FontFamily, 14) // Set the font size to 14
                };

                e.Graphics.DrawString(tenMonAn.Text, tenMonAn.Font, textBrush, textRect.X, textRect.Y);
            }
            // Draw the text
            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {         
                e.Graphics.DrawString("Giá:"+"           "+item.gia, e.Font, textBrush, textRect.X, textRect.Y + 25);
                e.Graphics.DrawString("Địa chỉ:"+"     "+item.dia_chi, e.Font, textBrush, textRect.X, textRect.Y + 45);
                e.Graphics.DrawString("Đóng góp:"+"  "+item.nguoi_dong_gop, e.Font, textBrush, textRect.X, textRect.Y + 65);
            }

            // Calculate the bounds for the border with padding
            Rectangle borderRect = new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 5, e.Bounds.Height - 4);

            // Draw the border around the item with padding
            using (Pen borderPen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawRectangle(borderPen, borderRect);
            }

            // Draw the focus rectangle if the item has focus
            e.DrawFocusRectangle();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Bai07_themMonAn bai07_ThemMonAn = new Bai07_themMonAn(this.login);
            bai07_ThemMonAn.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.White;
            button3.BackColor = Color.Gray;
            displayAll = true;
            if (comboBox2?.SelectedItem != null && comboBox1?.SelectedItem != null)
                DisplayPage("https://nt106.uitiot.vn/api/v1/monan/all");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.Gray;
            button3.BackColor = Color.White;
            displayAll = false;
            if (comboBox2?.SelectedItem != null && comboBox1?.SelectedItem != null)
                DisplayPage("https://nt106.uitiot.vn/api/v1/monan/my-dishes");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (displayAll)
                DisplayPage("https://nt106.uitiot.vn/api/v1/monan/all");
            else if(comboBox2?.SelectedItem != null)
                DisplayPage("https://nt106.uitiot.vn/api/v1/monan/my-dishes");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int comboBox2Value;
            if (comboBox2?.SelectedItem != null && int.TryParse(comboBox2.SelectedItem.ToString(), out comboBox2Value))
            {
                int size = 160 / comboBox2Value + 1;
                comboBox1.Items.Clear(); // Clear existing items
                for (int i = 1; i <= size; i++)
                {
                    comboBox1.Items.Add(i);
                }
                comboBox1.Enabled = true;

                if (displayAll && comboBox1.SelectedItem != null)
                {
                    DisplayPage("https://nt106.uitiot.vn/api/v1/monan/all");
                }
                else if (comboBox1.SelectedItem != null)
                {
                    DisplayPage("https://nt106.uitiot.vn/api/v1/monan/my-dishes");
                }
            }

        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) // Check if left mouse button is clicked
            {
                // Get the index of the clicked item
                int index = listBox1.IndexFromPoint(e.Location);

                // Check if the index is valid and if the click was on an item
                if (index != ListBox.NoMatches)
                {
                    // Get the food_info object associated with the clicked item
                    del_food = listBox1.Items[index] as food_info;

                    // Now you can do whatever you want with the clicked food_info object
                    // For example, you can display a new form with detailed information about the clicked item
                    // Assuming you have a method named ShowDetailsForm in your code that takes a food_info object
                    Bai07_DelFood bai07_DelFood = new Bai07_DelFood(this.login);
                    bai07_DelFood.ShowDialog();
                }
            }
        }

        private void ShowDetailsForm()
        {
            // Create a new instance of the form to show details
            Bai07_ChiTietMonAn detailsForm = new Bai07_ChiTietMonAn(this);

            // Show the details form
            detailsForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Create an instance of the Random class
            Random random = new Random();

            // Generate a random number between 1 and 100 (inclusive)
            int randomNumber = random.Next(0, foods.Count); // .Next(maxValue) is exclusive, so we use 101 to include 100

            // Output the random number
            random_food = foods[randomNumber];
            ShowDetailsForm();
        }
    }
}
