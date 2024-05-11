using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Lab02_Bai03 : Form
    {
        public Lab02_Bai03()
        {
            InitializeComponent();
        }
        private string directoryPath;

        static double EvaluateExpression(string expressionString)
        {
            // Create a parameterless lambda expression representing the provided expression string
            string normalizedLine = expressionString.Replace('–', '-');
            Expression<Func<double>> expression = () => Evaluate(normalizedLine);

            // Compile and execute the lambda expression
            Func<double> func = expression.Compile();
            return func();
        }

        static double Evaluate(string expression)
        {
            // Use DataTable's Compute method to evaluate the expression
            var dataTable = new System.Data.DataTable();
            var result = dataTable.Compute(expression, "");
            return Convert.ToDouble(result);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                directoryPath = Path.GetDirectoryName(selectedFilePath);
                string content = "";
                using (StreamReader sr = new StreamReader(selectedFilePath))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!string.IsNullOrEmpty(line.Trim()))
                        {
                            content += line + " = " + EvaluateExpression(line.Trim()).ToString() + '\n';
                        }
                    }
                    richTextBox1.Text = content;
                }
            }
            else
            {
                MessageBox.Show("File không hợp lệ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            string FilePath = Path.Combine(directoryPath, "output3.txt");
            using (StreamWriter sw = new StreamWriter(FilePath))
            {

                string[] lines = richTextBox1.Lines;

                // Write each line to the file
                foreach (string line in lines)
                {
                    sw.WriteLine(line.ToUpper());
                }
                MessageBox.Show("Đã viết thành công vào output3.txt");
            }
        }
    }
}
