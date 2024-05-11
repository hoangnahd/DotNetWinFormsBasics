using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Lab3
{
    public partial class Bai05_Server : Form
    {
        public Bai05_Server()
        {
            InitializeComponent();
            File.Delete("monAn.db");
            CreateTables();

            string image = compressImage("../../../data/banhcanh.jpg");

            InsertSampleData("banhcanh", "Hoang Anh", image);
            loadData();
            jsonData = JsonConvert.SerializeObject(all_data);
        }

        class food_data
        {
            public string tenMonAn { get; set; }
            public string idMonAn { get; set; }
            public string tenNguoiDongGop { get; set; }
            public string idNguoiDongGop { get; set; }  
        }
        class random_data
        {
            public string tenMonAn { get; set; }
            public string image { get; set; }
            public string tenNguoiDung { get; set; }
        }
        private bool stopServer = false;
        private Socket listenerSocket;
        
        private List<food_data> all_data = new List<food_data>();
        private string jsonData = "";
        private List<random_data> random_food = new List<random_data>();
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Waiting for a connection...");
            Thread serverThread = new Thread(new ThreadStart(() => StartServer()));
            serverThread.Start();
        }
        string receivedString = "";
        static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        public string compressImage(string imagePath)
        {
            using (Bitmap bitmap = new Bitmap(imagePath)) // Use using for automatic disposal
            {
                // Compress image as JPEG with specified quality
                ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 20L); // Adjust quality as needed

                using (MemoryStream compressedImageStream = new MemoryStream()) // Use using for disposal
                {
                    bitmap.Save(compressedImageStream, jpegEncoder, encoderParams);
                    byte[] compressedImageData = compressedImageStream.ToArray();

                    // Encode the compressed image as Base64
                    string base64Image = Convert.ToBase64String(compressedImageData);
                    return base64Image;
                }
            }
        }

        private async Task StartServer()
        {
            try
            {
                byte[] buffer = new byte[8192 * 5];
                listenerSocket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp
                );
                IPEndPoint ipepServer = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
                listenerSocket.Bind(ipepServer);
                listenerSocket.Listen(-1);

                List<Socket> connectedClients = new List<Socket>();

                while (!stopServer)
                {
                    // Accept incoming client connections asynchronously
                    Socket clientSocket = await listenerSocket.AcceptAsync();
                    connectedClients.Add(clientSocket);

                    // Process each client in a separate task
                    Task.Run(async () =>
                    {
                        try
                        {
                            while (true)
                            {
                                richTextBox1.AppendText("Connection accepted from: " + clientSocket.RemoteEndPoint + "\n");
                                byte[] data = Encoding.UTF8.GetBytes(jsonData);

                                // Send data to all connected clients
                                foreach (var connectedClient in connectedClients)
                                {
                                    await connectedClient.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                                }

                                richTextBox1.AppendText("Data is sent to all clients\n");

                                // Receive data from client
                                richTextBox1.AppendText("Waiting data from client\n");
                                int receivedBytes = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                                
                                if (receivedBytes > 0)
                                {
                                    receivedString = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                                    if (receivedString == "delete")
                                    {
                                        delete_food();
                                        jsonData = JsonConvert.SerializeObject(all_data);                                       
                                    }
                                    else if (receivedString.Split('/')[2] == "random")
                                    {

                                        getRandDomFood(receivedString.Split('/')[0], receivedString.Split('/')[1]);
                                        jsonData ="random_food\n" + JsonConvert.SerializeObject(random_food);
                                    }
                                    else
                                    {
                                        List<random_data> receivedData = JsonConvert.DeserializeObject<List<random_data>>(receivedString);
                                        InsertSampleData(receivedData[0].tenMonAn, receivedData[0].tenNguoiDung, receivedData[0].image);
                                        jsonData = JsonConvert.SerializeObject(all_data);
                                    }
                                    richTextBox1.AppendText("Received data from client\n");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            richTextBox1.AppendText("Error: " + ex.Message + "\n");
                        }
                        finally
                        {
                            // Close connection on error or when the client disconnects
                            if (clientSocket != null && clientSocket.Connected)
                            {
                                clientSocket.Shutdown(SocketShutdown.Both);
                                clientSocket.Close();
                                connectedClients.Remove(clientSocket); // Remove client from the list of connected clients
                                richTextBox1.AppendText("Connection closed\n");
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                // Close listener socket on exit
                if (listenerSocket != null)
                {
                    listenerSocket.Close();
                }
            }
        }


        private void loadData()
        {
            all_data = new List<food_data>();
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
                            food_data data = new food_data();
                            data.idMonAn = reader["IDMA"].ToString();
                            data.tenMonAn = reader["TenMonAn"].ToString();
                            data.tenNguoiDongGop = reader["TenNguoiDongGop"].ToString(); // Use the alias for the column name
                            data.idNguoiDongGop = reader["IDNCC"].ToString();
                            all_data.Add(data);
                        }
                    }
                }

                // Close the connection
                sqliteConnection.Close();
            }
        }
        private void CreateTables()
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

        private void InsertSampleData(string tenMonAn, string tenNguoiDung, string imagePath)
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

                            insertMonAnCommand.Parameters.AddWithValue("@IDNCC", idNCC);
                            int rowsAffected = insertMonAnCommand.ExecuteNonQuery();

                            if (rowsAffected != 1) // Handle success or error based on rows affected
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
                loadData();
            }
            catch (Exception ex)
            {
                // Handle general exceptions (e.g., connection errors)
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void delete_food()
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
            loadData();
        }

        private void getRandDomFood(string option, string name)
        {
            random_food.Clear();
            using (SQLiteConnection sqliteConnection = new SQLiteConnection("Data Source=monAn.db;Version=3;"))
            {
                sqliteConnection.Open();

                // Query to retrieve a random MonAn along with the associated tenNguoiDung
                string query = @"SELECT MonAn.*, NguoiDung.HoVaTen AS TenNguoiDung 
                         FROM MonAn 
                         INNER JOIN NguoiDung ON MonAn.IDNCC = NguoiDung.IDNCC";

                // If the option is set to "personal", filter by name
                if (option == "personal")
                {
                    query += " WHERE NguoiDung.HoVaTen = @name";
                }

                query += " ORDER BY RANDOM() LIMIT 1";

                using (SQLiteCommand command = new SQLiteCommand(query, sqliteConnection))
                {
                    // If filtering by name, add the parameter to the command
                    if (option == "personal" && !string.IsNullOrEmpty(name))
                    {
                        command.Parameters.AddWithValue("@name", name);
                    }

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            random_data data = new random_data();
                            // Retrieve the values from the query result
                            data.tenMonAn = reader["TenMonAn"].ToString();
                            data.tenNguoiDung = reader["TenNguoiDung"].ToString();
                            data.image = reader["HinhAnh"].ToString();
                            random_food.Add(data);
                        }
                    }
                }
                sqliteConnection.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            stopServer = true;
            if (listenerSocket != null)
            {
                listenerSocket.Close();
            }
            foreach (Form form in Application.OpenForms)
            {
                form.Close();
            }
        }
    }
}
