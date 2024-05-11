using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab01
{
    public partial class Lab01_Bai04 : Form
    {
        public Lab01_Bai04()
        {
            InitializeComponent();
        }

        // Sự kiện load form
        private void Lab01_Bai04_Load(object sender, EventArgs e)
        {

        }

        // Dictionary để lưu trạng thái đã mua của ghế
        private Dictionary<string, bool> isBuy = new Dictionary<string, bool>();

        // Dictionary để ánh xạ phim với danh sách phòng chiếu
        private Dictionary<string, List<int>> phim2phong = new Dictionary<string, List<int>>();

        // Dictionary để ánh xạ phim, phòng chiếu và giá vé
        private Dictionary<string, Dictionary<int, Dictionary<string, float>>> giaVe = new Dictionary<string, Dictionary<int, Dictionary<string, float>>>();

        // Xử lý sự kiện khi người dùng chọn phim
        private void chon_phim_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Thêm thông tin phòng chiếu của phim được chọn vào combobox chọn phòng
            phim2phong["Đào, phở và piano"] = new List<int>() { 45000, 1, 2, 3 };
            phim2phong["Mai"] = new List<int>() { 100000, 2, 3 };
            phim2phong["Gặp lại chị bầu"] = new List<int>() { 90000, 1 };
            phim2phong["Tarot"] = new List<int>() { 90000, 3 };

            if (chon_phim.SelectedItem != null)
            {
                chonPhong.Enabled = true;
                chonPhong.Items.Clear();
                for (int i = 1; i < phim2phong[chon_phim.SelectedItem.ToString()].Count; i++)
                {
                    chonPhong.Items.Add(phim2phong[chon_phim.SelectedItem.ToString()][i]);
                }

            }
        }

        // Xử lý sự kiện khi người dùng chọn phòng
        private void chonPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chonPhong.SelectedItem != null)
            {
                chon_ghe.Enabled = true;
            }
        }

        // Xử lý sự kiện khi người dùng nhấn vào nút "Đặt vé"
        private void button46_Click(object sender, EventArgs e)
        {
            Button ten_ghe = new Button();

            if (chonTen.Text.Length > 0 && chonPhong.SelectedItem != null && chon_ghe.SelectedItem != null && chon_phim.SelectedItem != null)
            {
                // Tạo tên button ghế dựa trên phòng và vị trí ghế
                string buttonName = chon_ghe.Text.Split('-')[0] + chonPhong.Text + "_" + chon_ghe.Text.Split('-')[1];

                // Kiểm tra nếu ghế đã được mua
                if (isBuy.ContainsKey(buttonName) && isBuy[buttonName] == true)
                {
                    MessageBox.Show("Ghế đã được lựa chọn");
                }

                // Duyệt qua tất cả các control trên form
                foreach (Control control in this.Controls)
                {
                    // Nếu control là button và có tên trùng với tên của ghế được chọn
                    if (control is Button button && button.Name == buttonName)
                    {
                        // Lưu lại button ghế được chọn
                        ten_ghe = button;
                    }

                    // Tạo bảng giá vé cho mỗi phim và phòng chiếu
                    if (!giaVe.ContainsKey(chon_phim.SelectedItem.ToString()))
                    {
                        giaVe[chon_phim.SelectedItem.ToString()] = new Dictionary<int, Dictionary<string, float>>();
                    }
                    for (int i = 1; i < phim2phong[chon_phim.SelectedItem.ToString()].Count; i++)
                    {
                        if (!giaVe[chon_phim.SelectedItem.ToString()].ContainsKey(phim2phong[chon_phim.SelectedItem.ToString()][i]))
                        {

                            giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]] = new Dictionary<string, float>();
                        }

                        // Thiết lập giá vé cho từng loại ghế
                        float price = (float)phim2phong[chon_phim.SelectedItem.ToString()][0] / 4;

                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["A-1"] = price;
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["A-5"] = price;
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["C-1"] = price;
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["C-5"] = price;

                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["A-2"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["A-3"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["A-4"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["C-2"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["C-3"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["C-4"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["B-1"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["B-5"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0];

                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["B-2"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0] * 2;
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["B-3"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0] * 2;
                        giaVe[chon_phim.SelectedItem.ToString()][phim2phong[chon_phim.SelectedItem.ToString()][i]]["B-4"] = (float)phim2phong[chon_phim.SelectedItem.ToString()][0] * 2;
                    }
                }

                // Đánh dấu ghế đã được mua, thay đổi màu sắc của ghế và hiển thị thông tin vé
                if (ten_ghe.Name == buttonName && !isBuy.ContainsKey(buttonName))
                {
                    isBuy[buttonName] = true;
                    ten_ghe.BackColor = Color.Orange;
                    hoten.Text = chonTen.Text;
                    tien.Text = (giaVe[chon_phim.SelectedItem.ToString()][int.Parse(chonPhong.SelectedItem.ToString())][chon_ghe.SelectedItem.ToString()]).ToString() + "đ";
                    ve.Text = ten_ghe.Text;
                    phim.Text = chon_phim.SelectedItem.ToString();
                    phong.Text = chonPhong.SelectedItem.ToString();
                }

            }
            else
            {
                MessageBox.Show("Vui lòng chọn đầy đủ thông tin");
            }
        }


    }
}
