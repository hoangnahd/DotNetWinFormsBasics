using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Lab3
{
    public partial class Bai05_Client : Form
    {
        public Bai05_Client()
        {
            InitializeComponent();
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

        private string imagePath = "";
        private TcpClient tcpClient;
        private NetworkStream ns;
        List<food_data> all_data = new List<food_data>();
        List<random_data> random_food = new List<random_data>();
        private void Bai05_Client_Load(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);
            tcpClient.Connect(ipEndPoint);
            ns = tcpClient.GetStream();

            // Start a background thread to continuously receive data from the server
            Thread receiveThread = new Thread(() => ReceiveDataAsync());
            receiveThread.Start();
        }
        private async Task ReceiveDataAsync()
        {
            try
            {
                byte[] buffer = new byte[8192 * 10];

                while (true)
                {
                    int bytesRead = await ns.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        // If no bytes are read, the client has closed the connection
                        break;
                    }

                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (receivedData.Split('\n')[0] == "random_food")
                    {  
                        random_food = JsonConvert.DeserializeObject<List<random_data>>(receivedData.Split('\n')[1]);
                        // Retrieve the values from the query result
                        string tenMonAn = random_food[0].tenMonAn.ToString();
                        string tenNguoiDung = random_food[0].tenNguoiDung.ToString();
                        string compressImage = random_food[0].image;
                        // Display the tenNguoiDung and tenMonAn
                        DecompressAndShowImage(compressImage);
                        textBox3.Text = tenMonAn;
                        textBox4.Text = tenNguoiDung;
                    }
                    else
                    {
                        all_data = JsonConvert.DeserializeObject<List<food_data>>(receivedData);
                        listView1.Items.Clear();
                        listView1.Columns.Clear();
                        listView1.View = View.Details;
                        // Add columns with appropriate widths and headers
                        listView1.Columns.Add("ID Món Ăn", 100);
                        listView1.Columns.Add("Tên Món Ăn", 200);
                        listView1.Columns.Add("Tên Người Đóng Góp", 200); // New column for contributor name
                        listView1.Columns.Add("ID Người Đóng Góp", 100);

                        foreach (var data in all_data)
                        {
                            ListViewItem item = new ListViewItem(data.idMonAn);
                            item.SubItems.Add(data.tenMonAn);
                            item.SubItems.Add(data.tenNguoiDongGop); // Use the alias for the column name
                            item.SubItems.Add(data.idNguoiDongGop);
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                // Handle IOException
                MessageBox.Show("Error reading from client: " + ex.Message);
            }
        }


        public void DecompressAndShowImage(string base64Image)
        {
            // Convert Base64 string back to byte array
            byte[] compressedImageData = Convert.FromBase64String(base64Image);

            // Create a memory stream from the compressed image data
            using (MemoryStream compressedImageStream = new MemoryStream(compressedImageData))
            {
                // Load the compressed image from the memory stream
                using (Image compressedImage = Image.FromStream(compressedImageStream))
                {
                    // Set decompression quality back to 100
                    ImageCodecInfo jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

                    // Create a new Bitmap with enhanced quality
                    Bitmap enhancedBitmap = new Bitmap(compressedImage.Width, compressedImage.Height);

                    // Draw the decompressed image onto the enhanced Bitmap to enhance quality
                    using (Graphics g = Graphics.FromImage(enhancedBitmap))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(compressedImage, new Rectangle(0, 0, enhancedBitmap.Width, enhancedBitmap.Height));
                    }

                    // Display the enhanced image in the PictureBox
                    pictureBox1.Image = enhancedBitmap;
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string initialDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\data\"));

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Select Image File",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                FilterIndex = 1,
                RestoreDirectory = true,
                InitialDirectory = initialDirectory // Set initial directory to the desired directory
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                imagePath = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendData("delete");
            MessageBox.Show("Xóa thành công món ăn!!");
        }

        private void SendData(string Data)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(Data);
                ns.Write(data, 0, data.Length);
                // Optionally, you might want to flush the stream after writing
                ns.Flush();              
            }
            catch (IOException ex)
            {
                // Handle IOException
                MessageBox.Show("Error sending from client: " + ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SendData("community" + '/' + "textBox2.Text.Trim()" + '/' + "random");         
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 0)
            {
                SendData("personal" + '/' + textBox2.Text.Trim() + '/' + "random");
            }
                
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0 && imagePath.Length > 0)
            {
                // Convert the byte array to a Base64 string
                List<random_data> info = new List<random_data>();
                random_data data = new random_data();
                data.tenMonAn = textBox1.Text;
                data.tenNguoiDung = textBox2.Text;
                data.image = compressImage(imagePath);
                info.Add(data);

                string json_data = JsonConvert.SerializeObject(info);
                SendData(json_data);
                MessageBox.Show("Thêm món ăn thành công");
            }
            else
                MessageBox.Show("Vui lòng nhập đủ thông tin!!");
        }     

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
