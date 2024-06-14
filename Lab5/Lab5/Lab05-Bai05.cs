using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.Text.RegularExpressions;
using System.Net;

namespace Lab5
{
    public partial class Lab05_Bai05 : Form
    {
        public Lab05_Bai05()
        {
            InitializeComponent(); 
        }
        public class Food
        {
            public string name { get; set; }
            public string img { get; set; }
            public string contributor { get; set; }
            public Food(string name, string img, string contributor)
            {
                this.name = name;
                this.img = img;
                this.contributor = contributor;
            }
        }
        private List<Food> foods = new List<Food>();
        static public Food randomFood;
        public void CreateTables()
        {
            string databaseFilePath = "../../../data/monAn.db";

            // Check if the database file exists
            if (!File.Exists(databaseFilePath))
            {
                // Create a new SQLite database file
                SQLiteConnection.CreateFile(databaseFilePath);
                Console.WriteLine("Database file created successfully.");
            }
            else
            {
                Console.WriteLine("Database file already exists.");
            }

            // Create connection
            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=../../../data/monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Drop MonAn table if it exists
                string dropMonAnTableQuery = "DROP TABLE IF EXISTS MonAn";
                using (SQLiteCommand dropMonAnTableCommand = new SQLiteCommand(dropMonAnTableQuery, sqliteConnection))
                {
                    dropMonAnTableCommand.ExecuteNonQuery();
                }

                // Create MonAn table with ThoiGian attribute
                string createMonAnTableQuery = @"CREATE TABLE IF NOT EXISTS MonAn (
                                             IDMA INTEGER PRIMARY KEY AUTOINCREMENT,
                                             TenMonAn TEXT,
                                             HinhAnh TEXT,
                                             IDNCC INTEGER,
                                             ThoiGian DATETIME)";
                using (SQLiteCommand createMonAnTableCommand = new SQLiteCommand(createMonAnTableQuery, sqliteConnection))
                {
                    createMonAnTableCommand.ExecuteNonQuery();
                    Console.WriteLine("Table 'MonAn' created successfully with 'ThoiGian' attribute.");
                }

                // Create NguoiDung table if not exists
                string createNguoiDungTableQuery = @"CREATE TABLE IF NOT EXISTS NguoiDung (
                                                 IDNCC INTEGER PRIMARY KEY AUTOINCREMENT,
                                                 HoVaTen TEXT)";
                using (SQLiteCommand createNguoiDungTableCommand = new SQLiteCommand(createNguoiDungTableQuery, sqliteConnection))
                {
                    createNguoiDungTableCommand.ExecuteNonQuery();
                    Console.WriteLine("Table 'NguoiDung' created successfully.");
                }

                // Close connection
                sqliteConnection.Close();
            }
        }
        private void InsertSampleData(string tenMonAn, string tenNguoiDongGop, string imagePath, DateTime thoiGian)
        {
            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=../../../data/monAn.db;Version=3;"))
                {
                    sqliteConnection.Open();

                    // Validate inputs (basic validation)
                    if (string.IsNullOrWhiteSpace(tenMonAn) || string.IsNullOrWhiteSpace(tenNguoiDongGop))
                    {
                        MessageBox.Show("Tên món ăn và tên người đóng góp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Start transaction
                    using (SQLiteTransaction transaction = sqliteConnection.BeginTransaction())
                    {
                        int idNCC = -1;

                        try
                        {
                            // Check if tenNguoiDongGop exists in NguoiDung table
                            string findNguoiDungQuery = "SELECT IDNCC FROM NguoiDung WHERE HoVaTen = @TenNguoiDung";
                            using (SQLiteCommand findNguoiDungCommand = new SQLiteCommand(findNguoiDungQuery, sqliteConnection))
                            {
                                findNguoiDungCommand.Parameters.AddWithValue("@TenNguoiDung", tenNguoiDongGop);
                                object result = findNguoiDungCommand.ExecuteScalar();

                                if (result != null && result != DBNull.Value)
                                {
                                    idNCC = Convert.ToInt32(result);
                                }
                                else
                                {
                                    // tenNguoiDongGop does not exist, insert it
                                    string insertNguoiDungQuery = "INSERT INTO NguoiDung (HoVaTen) VALUES (@TenNguoiDung)";
                                    using (SQLiteCommand insertNguoiDungCommand = new SQLiteCommand(insertNguoiDungQuery, sqliteConnection))
                                    {
                                        insertNguoiDungCommand.Parameters.AddWithValue("@TenNguoiDung", tenNguoiDongGop);
                                        insertNguoiDungCommand.ExecuteNonQuery();

                                        idNCC = (int)sqliteConnection.LastInsertRowId;
                                    }
                                }
                            }
                            // Insert into MonAn table
                            string insertMonAnQuery = "INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC, ThoiGian) VALUES (@TenMonAn, @HinhAnh, @IDNCC, @ThoiGian)";
                            using (SQLiteCommand insertMonAnCommand = new SQLiteCommand(insertMonAnQuery, sqliteConnection))
                            {
                                insertMonAnCommand.Parameters.AddWithValue("@TenMonAn", tenMonAn);
                                insertMonAnCommand.Parameters.AddWithValue("@HinhAnh", imagePath); // Consider default image path if imagePath is empty
                                insertMonAnCommand.Parameters.AddWithValue("@IDNCC", idNCC);
                                insertMonAnCommand.Parameters.AddWithValue("@ThoiGian", thoiGian);

                                int rowsAffected = insertMonAnCommand.ExecuteNonQuery();

                                if (rowsAffected != 1)
                                {
                                    MessageBox.Show("Lỗi khi chèn dữ liệu món ăn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            // Commit transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction on error
                            transaction.Rollback();
                            MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        static public Image GetImageFromUrl(string url)
        {
            try
            {
                // Create a web request to the URL
                WebRequest request = WebRequest.Create(url);

                // Set user agent and other headers if required
                ((HttpWebRequest)request).UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";

                // Get the web response
                using (WebResponse response = request.GetResponse())
                {
                    // Get the response stream
                    using (Stream stream = response.GetResponseStream())
                    {
                        // Convert stream to Image
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching image from URL '{url}': {ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int index = random.Next(foods.Count);
            randomFood = foods[index];
            Bai05_DetailFood bai05_DetailFood = new Bai05_DetailFood();
            bai05_DetailFood.ShowDialog();
        }
        static (string part1, string part2) ProcessString(string input)
        {
            // Split the input string by ';'
            var parts = input.Split(';');

            // Check if the split results in at least two parts
            if (parts.Length >= 2)
            {
                var part1 = parts[0];
                // Join the rest of the parts to handle any additional ';' in the second part
                var part2 = string.Join(";", parts, 1, parts.Length - 1);
                return (part1, part2);
            }

            // If the input does not contain at least two parts, return empty strings
            return (string.Empty, string.Empty);
        }
        static string GetSenderName(MimeMessage message)
        {
            // Check if From field is available
            if (message.From.Count > 0)
            {
                // Get the first sender's name
                var senderName = message.From[0].Name;
                return senderName;
            }
            return null;
        }
        static bool FoodExists(string name, string contributorName, DateTime thoigian)
        {
            using (var connection = new SQLiteConnection("Data Source=../../../data/monAn.db;Version=3;"))
            {
                connection.Open();

                string query = @"
                SELECT COUNT(*)
                FROM MonAn ma
                JOIN NguoiDung nd ON ma.IDNCC = nd.IDNCC
                WHERE ma.TenMonAn = @name AND nd.HoVaTen = @contributorName AND ma.ThoiGian = @thoigian";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@contributorName", contributorName);
                    command.Parameters.AddWithValue("@thoigian", thoigian);

                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void GetAllFoodsFromDatabase()
        {
            using (var connection = new SQLiteConnection("Data Source=../../../data/monAn.db;Version=3;"))
            {
                connection.Open();

                string query = @"
                SELECT ma.TenMonAn, ma.HinhAnh, nd.HoVaTen, ma.ThoiGian
                FROM MonAn ma
                JOIN NguoiDung nd ON ma.IDNCC = nd.IDNCC";

                using (var command = new SQLiteCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["TenMonAn"].ToString();
                        string image = reader["HinhAnh"].ToString();
                        string contributorName = reader["HoVaTen"].ToString();
                        foods.Add(new Food(name, image, contributorName));
                    }
                }

                connection.Close();
            }
        }

        private void Lab05_Bai05_Load(object sender, EventArgs e)
        {
            CreateTables();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 100;
            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, true);
                    client.Authenticate("shopeebot0001@gmail.com", "bclh jrgg tmkp jjyq");

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    for (int i = 0; i < inbox.Count; i++)
                    {
                        try
                        {
                            var message = inbox.GetMessage(i);

                            if (message.Subject.IndexOf("Đóng góp món ăn", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                var contributorName = GetSenderName(message) ?? "Người ẩn danh";
                                var content = message.TextBody;
                                var thoigian = message.Date.DateTime; // Get the sent date

                                if (content != null)
                                {
                                    (string name, string image) = ProcessString(content);
                                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(image))
                                    {
                                        if (!FoodExists(name, contributorName, thoigian))
                                        {
                                            InsertSampleData(name, contributorName, image, thoigian);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error processing message at index {i}: {ex.Message}");
                        }
                    }
                    GetAllFoodsFromDatabase();
                    if (foods.Any())
                    {
                        foreach(var food in foods)
                            listBox1.Items.Add(food);
                    }
                    else
                    {
                        MessageBox.Show("No messages with subject 'Đóng góp món ăn' found.");
                    }

                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Check if the index is valid
                if (e.Index < 0 || e.Index >= foods.Count) return;

                // Get the food_info item to be drawn
                Food item = foods[e.Index];

                // Define padding and image size
                int imageSize = 95; // Adjust based on your image size

                // Draw the background of the item
                e.DrawBackground();

                // Calculate the bounds for the image
                Rectangle imageRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 5, imageSize, imageSize);

                // Draw the image
                e.Graphics.DrawImage(GetImageFromUrl(item.img), imageRect);

                // Calculate the bounds for the text
                Rectangle textRect = new Rectangle(e.Bounds.X + 4 + imageSize + 4, e.Bounds.Y + 4, e.Bounds.Width - imageSize - 3 * 4, e.Bounds.Height - 2 * 4);
                using (SolidBrush textBrush = new SolidBrush(Color.IndianRed))
                {
                    Label tenMonAn = new Label
                    {
                        Text = item.name,
                        Font = new Font(e.Font.FontFamily, 14) // Set the font size to 14
                    };

                    e.Graphics.DrawString(tenMonAn.Text, tenMonAn.Font, textBrush, textRect.X, textRect.Y);
                }
                // Draw the text
                using (Brush textBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString("Đóng góp:" + "           " + item.contributor, e.Font, textBrush, textRect.X, textRect.Y + 25);
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
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
            
        }
    }
}
