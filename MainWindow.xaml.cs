using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "665d2c61ae145438263aa2214d07fcb3";

        private string requestUrl = "https://api.openweathermap.org/data/2.5/weather"; //?q={city name}&appid={apiKey}

        public MainWindow()
        {
            InitializeComponent();

            UpdateData("Bad Düben");
        }

        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();
            var finalUri = requestUrl + "?q=" + city + "&appid=" + apiKey + "&units=metric";
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
            return weatherMapResponse;
        }

        public void UpdateData(string city)
        {
            //WeatherMapResponse result = GetWeatherData("Bad Düben");
            //WeatherMapResponse result = GetWeatherData("Las Palmas");
            WeatherMapResponse result = GetWeatherData(city);

            string finalImage = "Sun.png";
            try
            { 
                string currentWeather = result.weather[0].main.ToLower();

                if (currentWeather.Contains("cloud"))
                {
                    finalImage = "Cloud.png";
                }
                else if (currentWeather.Contains("rain"))
                {
                    finalImage = "Rain.png";
                }
                else if (currentWeather.Contains("snow"))
                {
                    finalImage = "Snow.png";
                }

                backgroundImage.ImageSource = new BitmapImage(new Uri("Images/" + finalImage, UriKind.RelativeOrAbsolute));

                labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
                labelInfo.Content = result.weather[0].main;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                labelTemperature.Content = "";
                labelInfo.Content = "Ungültige\nEingabe";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;
            if (query == "")
            {
                query = "Bad Düben";
                textBoxQuery.Text = query;
            }
            UpdateData(query);
        }
    }
}
