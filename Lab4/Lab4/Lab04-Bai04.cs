using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Newtonsoft.Json;
using static Lab4.Lab04_Bai07;


namespace Lab4
{
    public partial class Lab04_Bai04 : Form
    {
        public Lab04_Bai04()
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


        private void Lab04_Bai04_Load(object sender, EventArgs e)
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

            // Get the food_info item to be drawn
            FilmInfo item = filmInfos[e.Index];

            // Define padding and image size
            int imageSize = 145; // Adjust based on your image size

            // Draw the background of the item
            e.DrawBackground();

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
            using (SolidBrush textBrush = new SolidBrush(Color.IndianRed))
            {
                Label tenMonAn = new Label
                {
                    Text = item.Title,
                    Font = new Font(e.Font.FontFamily, 14) // Set the font size to 14
                };

                e.Graphics.DrawString(tenMonAn.Text, tenMonAn.Font, textBrush, textRect.X, textRect.Y);
            }
            // Draw the text
            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {
                e.Graphics.DrawString("https://betacinemas.vn"+item.Href, e.Font, textBrush, textRect.X, textRect.Y + 25);
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
                    film = listBox1.Items[index] as FilmInfo;
                    detailFilm detailFilm = new detailFilm();
                    detailFilm.ShowDialog();
                }
            }
        }
    }
}
