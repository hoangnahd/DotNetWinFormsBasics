using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab2
{

    public partial class Lab02_Bai04 : Form
    {
        public Lab02_Bai04()
        {
            InitializeComponent();
        }
        private List<StudentInfo> students = new List<StudentInfo>();
        private List<StudentInfo> deserializedPeopleJson = new List<StudentInfo>();
        private int counter = 0;

        private void button2_Click(object sender, EventArgs e)
        {


            string student = "";

            if (idWrite.Text.Length != 8)
            {
                MessageBox.Show("Ma so sinh vien khong hop le!");
                idWrite.Text = "";
                return;
            }

            if (phoneWrite.Text.Length != 10 || !phoneWrite.Text.StartsWith("0"))
            {
                MessageBox.Show("So dien thoai khong hop le!");
                phoneWrite.Text = "";
                return;
            }

            if (float.Parse(mon1Write.Text.Trim()) < 0 || float.Parse(mon1Write.Text.Trim()) > 10)
            {
                MessageBox.Show("Diem mon 1 khong hop le!");
                mon1Write.Text = "";
                return;
            }
            if (float.Parse(mon2Write.Text.Trim()) < 0 || float.Parse(mon2Write.Text.Trim()) > 10)
            {
                MessageBox.Show("Diem mon 2 khong hop le!");
                mon2Write.Text = "";
                return;
            }
            if (float.Parse(mon3Write.Text.Trim()) < 0 || float.Parse(mon3Write.Text.Trim()) > 10)
            {
                MessageBox.Show("Diem mon 3 khong hop le!");
                mon3Write.Text = "";
                return;
            }


            students.Add(new StudentInfo(
            nameWrite.Text,
            idWrite.Text.Trim(),
            phoneWrite.Text.Trim(),
            float.Parse(mon1Write.Text.Trim()),
            float.Parse(mon2Write.Text.Trim()),
            float.Parse(mon3Write.Text.Trim())
            ));
            student += nameWrite.Text + '\n' + idWrite.Text + '\n' + phoneWrite.Text + '\n' +
                                mon1Write.Text + '\n' + mon2Write.Text + '\n' + mon2Write.Text +
                                "\n\n";
            richTextBox1.Text += student;
            nameWrite.Text = "";
            idWrite.Text = "";
            phoneWrite.Text = "";
            mon1Write.Text = "";
            mon2Write.Text = "";
            mon3Write.Text = "";

        }

        class StudentInfo
        {
            public string Name { get; set; }

            public string Id { get; set; }

            public string Phone { get; set; }

            public float Course1 { get; set; }

            public float Course2 { get; set; }

            public float Course3 { get; set; }

            public float average { get; set; }



            public StudentInfo(string name, string id, string phone,
                                float course1, float course2, float course3)
            {
                Name = name;
                Id = id;
                Phone = phone;
                this.Course1 = course1;
                this.Course2 = course2;
                this.Course3 = course3;
                average = (course1 + course2 + course3) / 3;
            }
            public override string ToString()
            {
                return $"Name: {this.Name}, id: {this.Id}, Phone: {this.Phone}, Course1: {this.Course1}, Course2: {this.Course2}, Course3: {this.Course3}, Average: {this.average}";
            }
        }
        static void SerializeToFileJson<T>(string filePath, T obj)
        {
            try
            {
                // Serialize the object to JSON format
                string json = JsonSerializer.Serialize(obj);
                // Write the JSON data to the file
                File.WriteAllText(filePath, json);
                Console.WriteLine($"Serialized data successfully written to{filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while serializing data:{ex.Message}");
            }
        }

        static T DeserializeFromFileJson<T>(string filePath)
        {
            try
            {
                // Read the JSON data from the file
                string json = File.ReadAllText(filePath);
                // Deserialize the JSON data to the specified type
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deserializing data:{ex.Message}");
                return default;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SerializeToFileJson("input4.json", students);
            MessageBox.Show("Ghi file thanh cong");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            deserializedPeopleJson = DeserializeFromFileJson<List<StudentInfo>>("input4.json");
            if (deserializedPeopleJson != null && deserializedPeopleJson.Count > 0)
            {
                nameShow.Text = deserializedPeopleJson[0].Name;
                idShow.Text = deserializedPeopleJson[0].Id;
                phoneShow.Text = deserializedPeopleJson[0].Phone;
                course1Show.Text = deserializedPeopleJson[0].Course1.ToString();
                course2Show.Text = deserializedPeopleJson[0].Course2.ToString();
                course3Show.Text = deserializedPeopleJson[0].Course3.ToString();
                averageShow.Text = deserializedPeopleJson[0].average.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (counter > 0)
            {
                counter -= 1;
                nameShow.Text = deserializedPeopleJson[counter].Name;
                idShow.Text = deserializedPeopleJson[counter].Id;
                phoneShow.Text = deserializedPeopleJson[counter].Phone;
                course1Show.Text = deserializedPeopleJson[counter].Course1.ToString();
                course2Show.Text = deserializedPeopleJson[counter].Course2.ToString();
                course3Show.Text = deserializedPeopleJson[counter].Course3.ToString();
                averageShow.Text = deserializedPeopleJson[counter].average.ToString();
                page.Text = (counter + 1).ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (counter < deserializedPeopleJson.Count - 1)
            {
                counter += 1;
                nameShow.Text = deserializedPeopleJson[counter].Name;
                idShow.Text = deserializedPeopleJson[counter].Id;
                phoneShow.Text = deserializedPeopleJson[counter].Phone;
                course1Show.Text = deserializedPeopleJson[counter].Course1.ToString();
                course2Show.Text = deserializedPeopleJson[counter].Course2.ToString();
                course3Show.Text = deserializedPeopleJson[counter].Course3.ToString();
                averageShow.Text = deserializedPeopleJson[counter].average.ToString();
                page.Text = (counter + 1).ToString();
            }
        }
    }
}
