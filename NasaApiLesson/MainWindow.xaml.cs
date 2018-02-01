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

        private async  void LoadButtonClick(object sender, RoutedEventArgs e)
        {
           await LoadImage();
        }

        private Task LoadImage()
        {
            return Task.Run(() =>
            {
                WebClient client = new WebClient();
                //client.BaseAddress = "https://api.nasa.gov/planetary/apod";
                //client.QueryString.Add("hd", "True");
                //client.QueryString.Add("api_key", "h7lH17irANgRnxTRT0H8KjFzB4SMD2FCIlyvH5lN");
                //client.QueryString.Add("date", "1994-11-08"); &date=1994-11-08

                using (Stream stream = client.OpenRead("https://api.nasa.gov/planetary/apod?api_key=h7lH17irANgRnxTRT0H8KjFzB4SMD2FCIlyvH5lN"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = reader.ReadToEnd();
                        NasaImageResponse response = JsonConvert.DeserializeObject<NasaImageResponse>(data);
                        client.DownloadFile( response.Hdurl, "universe.jpg");
                    }
                } ;

            }
               );
        }
    }
}
