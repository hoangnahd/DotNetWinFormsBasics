using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Newtonsoft.Json;


namespace Lab3
{
    public partial class Bai04_Client : Form
    {
        public Bai04_Client()
        {
            InitializeComponent();
        }
        public class ticket_info
        {
            public string Phong { get; set; }
            public string Phim { get; set; }
            public string GiaVeChuan { get; set; }
            public List<Ghe_info> Ghe { get; set; }
        }

        public class Ghe_info
        {
            public string SoGhe { get; set; }
            public bool Dat { get; set; }
            public string heSo { get; set; }
        }

        private TcpClient tcpClient;
        private NetworkStream ns;
        List<ticket_info> tickets = new List<ticket_info>();
        private void Bai04_Client_Load(object sender, EventArgs e)
        {
            tcpClient = new TcpClient();
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 8080);
            tcpClient.Connect(ipEndPoint);
            ns = tcpClient.GetStream();

            // Start a background thread to continuously receive data from the server
            Thread receiveThread = new Thread(ReceiveData);
            receiveThread.Start();

        }

        private void ReceiveData()
        {
            try
            {

                byte[] buffer = new byte[4096];
                int bytesRead = ns.Read(buffer, 0, buffer.Length);

                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                tickets = JsonConvert.DeserializeObject<List<ticket_info>>(receivedData);
                chon_phim.Items.Clear();

                foreach( var ticket in tickets)
                {
                    chon_phim.Items.Add(ticket.Phim.ToString());
                    chonPhong.Items.Add(ticket.Phong.ToString());
                    if(chon_phim != null)
                    {
                        foreach (var ghe in ticket.Ghe)
                        {
                            if (ghe.Dat)
                                danhDauGheDaMua(ghe.SoGhe.Trim());
                            else
                                danhDauGhe(ghe.SoGhe.Trim());
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

        private void SendData()
        {
            try
            {
                // Assuming ns is your NetworkStream
                string json_data = JsonConvert.SerializeObject(tickets);

                byte[] data = Encoding.UTF8.GetBytes(json_data);
                ns.Write(data, 0, data.Length);
                // Optionally, you might want to flush the stream after writing
                ns.Flush();
                Bai04_Client_Load(this, EventArgs.Empty);
            }
            catch (IOException ex)
            {
                // Handle IOException
                MessageBox.Show("Error sending from client: " + ex.Message);
            }
        }


        private void danhDauGheDaMua(string name)
        {
            foreach (Control control in this.Controls)
            {
                // Nếu control là button và có tên trùng với tên của ghế được chọn
                if (control is Button button && button.Text.Trim() == name)
                {
                    // Lưu lại button ghế được chọn
                    button.BackColor = Color.Orange;
                }
            }
        }
        private void danhDauGhe(string name)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button button && button.Text.Trim() == name)
                {
                    // Lưu lại button ghế được chọn
                    button.BackColor = Color.White;
                }
            }
        }

        private void dat_mua_Click(object sender, EventArgs e)
        {
            string ten_ghe = "";
            float tien = 0;

            if (chonTen.Text.Length > 0 && chonPhong.SelectedItem != null && chon_ghe.SelectedItem != null && chon_phim.SelectedItem != null)
            {

                // Duyệt qua tất cả các control trên form
                

                foreach (var ticket in tickets)
                {
                    if (ticket.Phim == chon_phim.SelectedItem.ToString())
                    {
                        foreach (var ghe in ticket.Ghe)
                        {
                            if (ghe.SoGhe.Trim() == chon_ghe.SelectedItem.ToString().Trim())
                            {
                                if (!ghe.Dat)
                                {
                                    ghe.Dat = true;
                                    tien = float.Parse(ticket.GiaVeChuan.Trim()) * float.Parse(ghe.heSo.Trim());
                                }
                                else
                                {
                                    MessageBox.Show("Ghế đã được đặt vui lòng chọn lại!!");
                                    return;
                                }

                            }
                        }
                    }
                }
                hoten.Text = chonTen.Text;
                ve.Text = chon_ghe.SelectedItem.ToString();
                giaTien.Text = tien.ToString();
                phim.Text = chon_phim.SelectedItem.ToString();
                phong.Text = chonPhong.SelectedItem.ToString();

                SendData();

                MessageBox.Show("Vé đã được mua thành công!!");

            }
            else
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin");
            }

        }

        private void exit_Click(object sender, EventArgs e)
        {
            // Close the network stream and the TCP client
            ns.Close();
            tcpClient.Close();
            this.Close();
        }

        private void chon_phim_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chon_phim.SelectedItem != null)
            {
                chonPhong.Enabled = true;
                chonPhong.Items.Clear();
                foreach ( var ticket in tickets)
                {
                    if (ticket.Phim == chon_phim.SelectedItem)
                    {
                        chonPhong.Items.Add(ticket.Phong);
                        chon_ghe.Enabled = true;
                        chon_ghe.Items.Clear();
                        foreach(var ghe in ticket.Ghe)
                        {
                            chon_ghe.Items.Add(ghe.SoGhe);
                            if (ghe.Dat)
                                danhDauGheDaMua(ghe.SoGhe.Trim());
                            else
                                danhDauGhe(ghe.SoGhe.Trim());
                        }
                    }
                    
                }

            }
        }
    }
}
