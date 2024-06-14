using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Lab5
{
    public partial class Lab05_Bai04 : Form
    {
        public Lab05_Bai04()
        {
            InitializeComponent();
        }
        public class FilmInfo
        {
            public string Title { get; set; }
            public string Genre { get; set; }
            public string Duration { get; set; }
            public string Href { get; set; }
            public string ImgUrl { get; set; } = "Image not found!";
        }
        private List<FilmInfo> filmInfos = new List<FilmInfo>();
        static public FilmInfo film = new FilmInfo();
        private void SaveFilmInfosToJson()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "films.json",
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Save JSON File",
                InitialDirectory = Path.GetFullPath("../../../data/")
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Serialize the list of FilmInfo objects to JSON
                    string jsonString = JsonConvert.SerializeObject(filmInfos, Formatting.Indented);

                    // Save the JSON string to the selected file
                    File.WriteAllText(saveFileDialog.FileName, jsonString);
                    MessageBox.Show("Film information saved successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }
        private async void FetchFilmInfo(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var filmInfoNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'film-info film-xs-info')]");
            var divFilmImgNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'pi-img-wrapper')]");
            if (filmInfoNodes == null) return;

            foreach (var node in filmInfoNodes)
            {
                FilmInfo filmInfo = new FilmInfo();
                var titleNode = node.SelectSingleNode(".//h3/a");
                var genreNode = node.SelectSingleNode(".//li[1]");
                var durationNode = node.SelectSingleNode(".//li[2]");

                filmInfo.Title = titleNode?.InnerText.Trim();
                filmInfo.Genre = genreNode?.InnerText.Replace("Thể loại:", "").Trim();
                filmInfo.Duration = durationNode?.InnerText.Replace("Thời lượng:", "").Trim();
                filmInfo.Href = titleNode?.GetAttributeValue("href", "").Trim();

                filmInfos.Add(filmInfo);
            }

            if (divFilmImgNodes != null && divFilmImgNodes.Count > 0)
            {
                for (int i = 0; i < divFilmImgNodes.Count && i < filmInfos.Count; i++)
                {
                    var imgNode = divFilmImgNodes[i].SelectSingleNode(".//img");
                    if (imgNode != null)
                    {
                        string srcValues = imgNode.GetAttributeValue("src", string.Empty).Trim();
                        filmInfos[i].ImgUrl = srcValues;
                    }
                    listBox1.Items.Add(filmInfos[i]);
                }
            }
        }
       
        private void Lab05_Bai04_Load(object sender, EventArgs e)
        {
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 150;

            FetchFilmInfo("https://betacinemas.vn/phim.htm");
            SaveFilmInfosToJson();
        }
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Check if the index is valid
            if (e.Index < 0 || e.Index >= filmInfos.Count) return;

            // Get the filmInfo item to be drawn
            FilmInfo item = filmInfos[e.Index];

            // Define padding and image size
            int imageSize = 145; // Adjust based on your image size

            // Draw the background of the item
            e.Graphics.FillRectangle(Brushes.White, e.Bounds);

            // Calculate the bounds for the image
            Rectangle imageRect = new Rectangle(e.Bounds.X + 6, e.Bounds.Y + 5, imageSize, imageSize);

            // Draw the image
            Image image = null;
            using (var webClient = new WebClient())
            {
                using (var stream = webClient.OpenRead(item.ImgUrl))
                {
                    image = Image.FromStream(stream);
                }
            }
            e.Graphics.DrawImage(image, imageRect);

            // Calculate the bounds for the text
            Rectangle textRect = new Rectangle(e.Bounds.X + 4 + imageSize + 4, e.Bounds.Y + 4, e.Bounds.Width - imageSize - 3 * 4, e.Bounds.Height - 2 * 4);

            // Draw the title text
            using (SolidBrush textBrush = new SolidBrush(Color.IndianRed))
            {
                e.Graphics.DrawString(item.Title, new Font(e.Font.FontFamily, 14), textBrush, textRect.X, textRect.Y);
            }

            // Draw the URL text
            using (Brush textBrush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString("https://betacinemas.vn" + item.Href, e.Font, textBrush, textRect.X, textRect.Y + 25);
            }

            // Draw "Book Ticket" button
            Rectangle bookButtonRect = new Rectangle(textRect.X + 5, textRect.Y + 70, 100, 30);
            e.Graphics.FillRectangle(Brushes.White, bookButtonRect);
            e.Graphics.DrawRectangle(Pens.Black, bookButtonRect);
            e.Graphics.DrawString("Đặt vé", new Font(e.Font, FontStyle.Bold), Brushes.Black, bookButtonRect.X + 20, bookButtonRect.Y + 8);

            // Draw "View Detail" button
            Rectangle detailButtonRect = new Rectangle(textRect.X + 115, textRect.Y + 70, 100, 30);
            e.Graphics.FillRectangle(Brushes.White, detailButtonRect);
            e.Graphics.DrawRectangle(Pens.Black, detailButtonRect);
            e.Graphics.DrawString("Chi tiết", new Font(e.Font, FontStyle.Bold), Brushes.Black, detailButtonRect.X + 20, detailButtonRect.Y + 8);

            // Calculate the bounds for the border with padding
            Rectangle borderRect = new Rectangle(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 5, e.Bounds.Height - 4);

            // Draw the border around the item with padding
            using (Pen borderPen = new Pen(Color.Black, 1))
            {
                e.Graphics.DrawRectangle(borderPen, borderRect);
            }

            // Prevent default focus rectangle
            e.DrawFocusRectangle();
        }


        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the clicked item
            int index = (sender as ListBox).IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                // Calculate the bounds for the buttons
                Rectangle itemBounds = (sender as ListBox).GetItemRectangle(index);
                Rectangle imageRect = new Rectangle(itemBounds.X + 6, itemBounds.Y + 5, 145, 145);
                Rectangle textRect = new Rectangle(itemBounds.X + 4 + 145 + 4, itemBounds.Y + 4, itemBounds.Width - 145 - 3 * 4, itemBounds.Height - 2 * 4);
                Rectangle bookButtonRect = new Rectangle(textRect.X + 5, textRect.Y + 70, 100, 30);
                Rectangle detailButtonRect = new Rectangle(textRect.X + 115, textRect.Y + 70, 100, 30);

                // Check if the click was on the "Book Ticket" button
                if (bookButtonRect.Contains(e.Location))
                {
                    film = listBox1.Items[index] as FilmInfo;
                    Bai04_BuyTicket bai04_BuyTicket = new Bai04_BuyTicket();
                    bai04_BuyTicket.ShowDialog();
                }

                // Check if the click was on the "View Detail" button
                if (detailButtonRect.Contains(e.Location))
                {
                    film = listBox1.Items[index] as FilmInfo;
                    Bai04_DetailFilm detailFilm = new Bai04_DetailFilm();
                    detailFilm.ShowDialog();
                }

            }
        }
        // Custom ListBox class to prevent selection fill

    }
}
