using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Lab5.Lab05_Bai05;

namespace Lab5
{
    public partial class Bai05_DetailFood : Form
    {
        public Bai05_DetailFood()
        {
            InitializeComponent();     
        }

        private void Bai05_DetailFood_Load(object sender, EventArgs e)
        {
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 100;
            listBox1.Items.Add(randomFood);
            this.Text = "Ăn " + randomFood.name + " đi!!!!";
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {

                // Get the food_info item to be drawn
                Food item = randomFood;

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
