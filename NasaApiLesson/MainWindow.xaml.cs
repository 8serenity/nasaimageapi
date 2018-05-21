using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NasaApiLesson
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            imageHost.Source = null;
            loadingBar.IsIndeterminate = true;
            if(!userDatePicker.SelectedDate.HasValue || userDatePicker.SelectedDate >= DateTime.Now)
            {
                loadingBar.IsIndeterminate = false;
                return;
            }
            var result = await LoadImage(userDatePicker.SelectedDate.Value);
            loadingBar.IsIndeterminate = false;

            explanationImage.Text = result.Explanation;


            File.Copy("newImage.jpg", "copyImage.jpg", true);

            imageHost.Source = new BitmapImage(new Uri(@"C:\Users\Мухамедьян\Desktop\nasaapi\NasaApiLesson\bin\Debug\copyImage.jpg"));
            
        }

        private Task<NasaImageResponse> LoadImage(DateTime date)
        {
            return Task.Run(() =>
            {
                WebClient client = new WebClient();
                //client.QueryString.Add("api_key", "h7lH17irANgRnxTRT0H8KjFzB4SMD2FCIlyvH5lN");
                //client.QueryString.Add("hd", "True");
                //client.QueryString.Add("date", "1994-11-08");

                //client.DownloadFile("https://api.nasa.gov/planetary/apod", "jsonNasa.txt");

                //using (StreamReader reader = new StreamReader("jsonNasa.txt"))
                //{
                //    string data = reader.ReadToEnd();
                //    NasaImageResponse response = JsonConvert.DeserializeObject<NasaImageResponse>(data);
                //    client.DownloadFile(response.Hdurl, "universe.jpg");
                //}

                string url = "https://api.nasa.gov/planetary/apod?api_key=h7lH17irANgRnxTRT0H8KjFzB4SMD2FCIlyvH5lN&hd=true&date="
                + date.ToString("yyyy-MM-dd");

                using (Stream stream = client.OpenRead(url))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = reader.ReadToEnd();
                        NasaImageResponse response = JsonConvert.DeserializeObject<NasaImageResponse>(data);
                        client.DownloadFile(response.Hdurl, "newImage.jpg");
                        return response;
                    }
                };

            });
        }
    }
}
