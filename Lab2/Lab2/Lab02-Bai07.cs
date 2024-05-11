using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{
    public partial class Lab02_Bai07 : Form
    {
        public Lab02_Bai07()
        {
            InitializeComponent();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                TreeNode node = new TreeNode(drive.Name);
                node.Tag = drive.RootDirectory;
                treeView1.Nodes.Add(node);
                LoadDirectories(node);
            }
        }

        private void LoadDirectories(TreeNode parentNode)
        {
            try
            {
                DirectoryInfo[] directories = ((DirectoryInfo)parentNode.Tag).GetDirectories();
                foreach (DirectoryInfo directory in directories)
                {
                    TreeNode node = new TreeNode(directory.Name);
                    node.Tag = directory;
                    parentNode.Nodes.Add(node);
                    node.Nodes.Add(""); // Dummy node to show "+" for expandable nodes
                }

                FileInfo[] files = ((DirectoryInfo)parentNode.Tag).GetFiles();
                foreach (FileInfo file in files)
                {
                    TreeNode fileNode = new TreeNode(file.Name);
                    fileNode.Tag = file;
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore if access to directory is denied
            }
        }


        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is DirectoryInfo directory)
            {
                try
                {
                    foreach (TreeNode node in e.Node.Nodes)
                    {
                        node.Remove();
                    }
                    foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                    {
                        TreeNode node = new TreeNode(subDirectory.Name);
                        node.Tag = subDirectory;
                        e.Node.Nodes.Add(node);
                        node.Nodes.Add(""); // Dummy node to show "+" for expandable nodes
                    }

                    FileInfo[] files = directory.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        TreeNode fileNode = new TreeNode(file.Name);
                        fileNode.Tag = file;
                        e.Node.Nodes.Add(fileNode);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignore if access to directory is denied
                }
            }

            else if (e.Node.Tag is FileInfo file)
            {
                try
                {
                    if (IsImage(file.FullName))
                    {
                        // Handle image file
                        DisplayImage(file.FullName);
                    }
                    else
                    {
                        DisplayTextContent(file.FullName);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    // Ignore if access to file is denied
                }
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count == 1 && e.Node.Nodes[0].Text == "")
            {
                e.Node.Nodes.Clear();
                LoadDirectories(e.Node);
            }
        }



        private bool IsImage(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            string extension = Path.GetExtension(filePath);
            foreach (string ext in imageExtensions)
            {
                if (extension.Equals(ext, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void DisplayImage(string imagePath)
        {
            try
            {
                richTextBox1.Visible = false;
                pictureBox1.Visible = true;
                //Load the image from file
                Image image = Image.FromFile(imagePath);

                // Display the image in PictureBox
                pictureBox1.Image = image;

                // Adjust the size mode of the PictureBox
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DisplayTextContent(string filePath)
        {
            try
            {
                // Read the text content from the file
                string content = File.ReadAllText(filePath);

                // Display the text content in the TextBox
                richTextBox1.Text = content;

                // Make sure GroupBox is visible
                richTextBox1.Visible = true;
                pictureBox1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying text content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
