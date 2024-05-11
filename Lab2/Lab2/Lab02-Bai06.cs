using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Lab02_Bai06 : Form
    {
        public Lab02_Bai06()
        {
            InitializeComponent();
            File.Delete("monAn.db");
            CreateTables();
            loadData();
            // Add columns to the ListView


        }

        private string imagePath;

        private void loadData()
        {
            listView1.View = View.Details;
            // Add columns with appropriate widths and headers
            listView1.Columns.Add("ID Món Ăn", 100);
            listView1.Columns.Add("Tên Món Ăn", 200);
            listView1.Columns.Add("Tên Người Đóng Góp", 200); // New column for contributor name
            listView1.Columns.Add("ID Người Đóng Góp", 100);

            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Query to retrieve data from the MonAn and NguoiDung tables using a join
                string query = @"SELECT MonAn.IDMA, MonAn.TenMonAn, NguoiDung.HoVaTen AS TenNguoiDongGop, MonAn.IDNCC
                     FROM MonAn
                     INNER JOIN NguoiDung ON MonAn.IDNCC = NguoiDung.IDNCC";

                // Create command
                using (SQLiteCommand command = new SQLiteCommand(query, sqliteConnection))
                {
                    // Execute command
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Append data rows to the ListView
                        while (reader.Read())
                        {
                            ListViewItem item = new ListViewItem(reader["IDMA"].ToString());
                            item.SubItems.Add(reader["TenMonAn"].ToString());
                            item.SubItems.Add(reader["TenNguoiDongGop"].ToString()); // Use the alias for the column name
                            item.SubItems.Add(reader["IDNCC"].ToString());
                            listView1.Items.Add(item);
                        }
                    }
                }

                // Close the connection
                sqliteConnection.Close();
            }
        }
        private void CreateTables()
        {

            string databaseFilePath = "monAn.db";

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
            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Create MonAn table if not exists
                string createMonAnTableQuery = @"CREATE TABLE IF NOT EXISTS MonAn (
                                                 IDMA INTEGER PRIMARY KEY AUTOINCREMENT,
                                                 TenMonAn TEXT,
                                                 HinhAnh TEXT,
                                                 IDNCC INTEGER)";
                using (SQLiteCommand createMonAnTableCommand = new SQLiteCommand(createMonAnTableQuery, sqliteConnection))
                {
                    createMonAnTableCommand.ExecuteNonQuery();
                }

                // Create NguoiDung table if not exists
                string createNguoiDungTableQuery = @"CREATE TABLE IF NOT EXISTS NguoiDung (
                                                     IDNCC INTEGER PRIMARY KEY AUTOINCREMENT,
                                                     HoVaTen TEXT,
                                                     QuyenHan TEXT)";
                using (SQLiteCommand createNguoiDungTableCommand = new SQLiteCommand(createNguoiDungTableQuery, sqliteConnection))
                {
                    createNguoiDungTableCommand.ExecuteNonQuery();
                }

                // Close connection
                sqliteConnection.Close();
            }
        }

        private void InsertSampleData(string tenMonAn, string tenNguoiDung)
        {
            int idNCC = -1;

            try
            {
                using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
                {
                    sqliteConnection.Open();

                    // Validate user input (consider regular expressions or whitelisting)

                    // Find IDNCC based on tenNguoiDung
                    string findNguoiDungQuery = "SELECT IDNCC FROM NguoiDung WHERE HoVaTen = @TenNguoiDung";
                    using (SQLiteCommand findNguoiDungCommand = new SQLiteCommand(findNguoiDungQuery, sqliteConnection))
                    {
                        findNguoiDungCommand.Parameters.AddWithValue("@TenNguoiDung", tenNguoiDung);
                        object result = findNguoiDungCommand.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            idNCC = Convert.ToInt32(result);
                        }
                        else
                        {
                            // tenNguoiDung does not exist, insert it
                            string insertNguoiDungQuery = "INSERT INTO NguoiDung (HoVaTen, QuyenHan) VALUES (@TenNguoiDung, @QuyenHan)";
                            using (SQLiteCommand insertNguoiDungCommand = new SQLiteCommand(insertNguoiDungQuery, sqliteConnection))
                            {
                                insertNguoiDungCommand.Parameters.AddWithValue("@TenNguoiDung", tenNguoiDung);
                                insertNguoiDungCommand.Parameters.AddWithValue("@QuyenHan", "admin");
                                insertNguoiDungCommand.ExecuteNonQuery();
                            }

                            idNCC = (int)sqliteConnection.LastInsertRowId; // Use for error handling
                        }
                    }

                    if (idNCC != -1)
                    {
                        string insertMonAnQuery = "INSERT INTO MonAn (TenMonAn, HinhAnh, IDNCC) VALUES (@TenMonAn, @HinhAnh, @IDNCC)";
                        using (SQLiteCommand insertMonAnCommand = new SQLiteCommand(insertMonAnQuery, sqliteConnection))
                        {
                            insertMonAnCommand.Parameters.AddWithValue("@TenMonAn", tenMonAn);

                            // Check or provide a default image path before adding
                            insertMonAnCommand.Parameters.AddWithValue("@HinhAnh", imagePath);
                            imagePath = "";

                            insertMonAnCommand.Parameters.AddWithValue("@IDNCC", idNCC);
                            int rowsAffected = insertMonAnCommand.ExecuteNonQuery();

                            if (rowsAffected == 1) // Handle success or error based on rows affected
                            {
                                ListViewItem item = new ListViewItem(sqliteConnection.LastInsertRowId.ToString());
                                item.SubItems.Add(tenMonAn);
                                item.SubItems.Add(tenNguoiDung);
                                item.SubItems.Add(idNCC.ToString());
                                listView1.Items.Add(item);
                                textBox1.Text = "";
                                textBox2.Text = "";
                            }
                            else
                            {
                                // Handle insertion failure (e.g., display error message)
                                MessageBox.Show("Failed to insert dish data.");
                            }
                        }
                    }

                    else
                    {
                        // Handle invalid user input (e.g., display error message)
                        MessageBox.Show("Invalid user input detected.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle general exceptions (e.g., connection errors)
                MessageBox.Show("An error occurred: " + ex.Message);
            }


        }


        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Select Image File",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                imagePath = openFileDialog1.FileName;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(imagePath))
            {
                MessageBox.Show("Vui long chon day du thong tin");
            }
            else
            {
                InsertSampleData(textBox1.Text, textBox3.Text);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Find the ID of the last row
                string findLastIDQuery = "SELECT MAX(IDMA) FROM MonAn";
                using (SQLiteCommand findLastIDCommand = new SQLiteCommand(findLastIDQuery, sqliteConnection))
                {
                    int lastID = Convert.ToInt32(findLastIDCommand.ExecuteScalar());

                    if (lastID != -1) // Check if any rows exist
                    {
                        // Delete the row with the last ID
                        string deleteQuery = "DELETE FROM MonAn WHERE IDMA = @IDMA";
                        using (SQLiteCommand deleteCommand = new SQLiteCommand(deleteQuery, sqliteConnection))
                        {
                            deleteCommand.Parameters.AddWithValue("@IDMA", lastID);
                            deleteCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Handle no rows scenario (e.g., display message)
                        MessageBox.Show("There are no rows to delete.");
                    }
                }

                sqliteConnection.Close();
            }
            listView1.Clear();
            loadData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Query to retrieve a random MonAn along with the associated tenNguoiDung
                string query = @"SELECT MonAn.*, NguoiDung.HoVaTen AS TenNguoiDung 
                             FROM MonAn 
                             INNER JOIN NguoiDung ON MonAn.IDNCC = NguoiDung.IDNCC 
                             ORDER BY RANDOM() LIMIT 1";

                using (SQLiteCommand command = new SQLiteCommand(query, sqliteConnection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the query result
                            string tenMonAn = reader["TenMonAn"].ToString();
                            string tenNguoiDung = reader["TenNguoiDung"].ToString();
                            string imagePath = reader["HinhAnh"].ToString();

                            // Load the image and display it in the PictureBox control
                            pictureBox1.Image = Image.FromFile(imagePath);

                            // Display the tenNguoiDung and tenMonAn
                            textBox4.Text = tenNguoiDung;
                            textBox2.Text = tenMonAn;
                        }
                    }
                }

                sqliteConnection.Close();
            }
        }

        private void Lab02_Bai06_Load(object sender, EventArgs e)
        {

        }
    }
}
