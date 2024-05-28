using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Lab4.Lab04_Bai07;

namespace Lab4
{

    public partial class Bai07_ChiTietMonAn : Form
    {

        public Bai07_ChiTietMonAn(Lab04_Bai07 bai7)
        {
            InitializeComponent();
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 100;
            listBox1.Items.Add(random_food);
            this.Text ="Ăn "+ random_food.ten_mon_an+" đi!!!!";
        }


        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            // Get the food_info item to be drawn
            food_info item = random_food;

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
                e.Graphics.DrawString("Giá:" + "           " + item.gia, e.Font, textBrush, textRect.X, textRect.Y + 25);
                e.Graphics.DrawString("Địa chỉ:" + "     " + item.dia_chi, e.Font, textBrush, textRect.X, textRect.Y + 45);
                e.Graphics.DrawString("Đóng góp:" + "  " + item.nguoi_dong_gop, e.Font, textBrush, textRect.X, textRect.Y + 65);
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
    }
}
