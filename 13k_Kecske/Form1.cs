using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace _13k_Kecske
{
    public partial class Form1 : Form
    {
        List<Kecske> kecskek = new List<Kecske>();
        public Form1()
        {
            InitializeComponent();
            Start();
            createButton.Click += buttonClick;
        }
        async void Start() {
            HttpClient client = new HttpClient();
            string url = "http://127.1.1.1:3000/kecske";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string message = await response.Content.ReadAsStringAsync();
                List<KecskeClass> data = JsonConvert.DeserializeObject<List<KecskeClass>>(message);
                //listBox1.Items.Clear();
                
                foreach (KecskeClass item in data)
                {
                    //listBox1.Items.Add($"Kecske neve: {item.nev}, kora: {item.kor}, súlya: {item.suly}, magassága: {item.magas}, neme: {item.nem}");
                    kecskek.Add(new Kecske(item));
                    panel1.Controls.Add(kecskek.Last());
                    kecskek.Last().Left = (kecskek.Count-1) * (kecskek.Last().Width + 5);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        async void buttonClick(object s, EventArgs e) {
            HttpClient client = new HttpClient();
            string url = "http://127.1.1.1:3000/kecske";
            try
            {
                var jsonObject = new
                {
                    kor = int.Parse(ageTextBox.Text),
                    magas = int.Parse(heightTextBox.Text),
                    suly = int.Parse(weightTextBox.Text),
                    nem = genderTextBox.Text,
                    nev = nameTextBox.Text
                };
                string jsonString = JsonConvert.SerializeObject(jsonObject);
                StringContent sendThis = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, sendThis);
                response.EnsureSuccessStatusCode();
                Start();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }
    }
}
