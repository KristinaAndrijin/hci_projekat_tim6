using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using HCI_Projekat.Model;
using MaterialDesignThemes.Wpf;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
namespace HCI_Projekat.Pages
{
    public partial class TripDetailsPage : Page
    {
        public Trip Trip { get; set; }
        private DateTime selectedDate;
        Pushpin startPushpin { get; set; }
        Pushpin endPushpin { get; set; }
        const string BING_API_KEY = "Ah8LozC7khuISaCoOppLN2Vm_mhOD65qXBZVEDcQoZ34UApWABR9jxtuKdlYb7jV";
        

        public TripDetailsPage(Trip trip)
        {
            this.Trip = trip;
            InitializeComponent();
            Debug.WriteLine($"Clicked on trip: {trip.Name}, Price: {trip.Price}");
            DataContext = this;
            string streetStart = "";
            string cityStart = "";
            string streetEnd = "";
            string cityEnd = "";

            string addressToParse = trip.StartingAddress;
            string[] partsOfAddress = addressToParse.Split(", ");
            if (partsOfAddress.Length == 4)
            {
                streetStart = partsOfAddress[0];
                cityStart = partsOfAddress[2].Split(" ")[1];
            }
            else if (partsOfAddress.Length == 3)
            {
                streetStart = partsOfAddress[0];
                if (partsOfAddress[1].Split(" ").Length > 1)
                {
                    cityStart = partsOfAddress[1].Split(" ")[1];
                }
                else
                {
                    cityStart = partsOfAddress[1];
                }

            }

            addressToParse = trip.EndingAddress;
            partsOfAddress = addressToParse.Split(", ");
            if (partsOfAddress.Length == 4)
            {
                streetEnd = partsOfAddress[0];
                cityEnd = partsOfAddress[2].Split(" ")[1];
            }
            else if (partsOfAddress.Length == 3)
            {
                streetEnd = partsOfAddress[0];
                if (partsOfAddress[1].Split(" ").Length > 1)
                {
                    cityEnd = partsOfAddress[1].Split(" ")[1];
                }
                else
                {
                    cityEnd = partsOfAddress[1];
                }

            }
            
            putPinPls(streetStart + ", " + cityStart + ", Srbija", true);
            putPinPls(streetEnd + ", " + cityEnd + ", Srbija", false);
            ConnectPushpins();
        }
        
        private void putPinPls(string address, bool start)
        {
            try
            {
                string requestUrl = string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}", address, BING_API_KEY);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseJson = reader.ReadToEnd();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                        double latitude = data.resourceSets[0].resources[0].point.coordinates[0];
                        double longitude = data.resourceSets[0].resources[0].point.coordinates[1];

                        Microsoft.Maps.MapControl.WPF.Location location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        Pushpin pin = new Pushpin { Location = location, Background = new SolidColorBrush(Colors.Blue) };
                        Map.Children.Add(pin);
                        if (start)
                        {
                            startPushpin = pin;
                        }
                        else
                        {
                            endPushpin = pin;
                        }
                        Map.Center = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
            }
        }
        
        private void ConnectPushpins()
        {

            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 2;

            LocationCollection locations = new LocationCollection();
            locations.Add(startPushpin.Location);
            locations.Add(endPushpin.Location);

            polyline.Locations = locations;
            Map.Children.Add(polyline);
        }


        private void OpenReservationDialog(object sender, RoutedEventArgs e)
        {
            ReservationDialog.Visibility = Visibility.Visible;
        }

        private void CloseReservationDialog(object sender, RoutedEventArgs e)
        {
            ReservationDialog.Visibility = Visibility.Collapsed;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            bool isFutureDate = selectedDate > DateTime.Now.Date;

            btnRezervisi.IsEnabled = isFutureDate;
        }

        private void BtnKupi_Click(object sender, RoutedEventArgs e)
        {
            PrintSelectedTime();
            CloseReservationDialog(sender, e);
            btnRezervisiOpenDialog.IsEnabled = false;
        }

        private void BtnRezervisi_Click(object sender, RoutedEventArgs e)
        {
            PrintSelectedTime();
            CloseReservationDialog(sender, e);
            btnRezervisiOpenDialog.IsEnabled = false;
        }

        private void PrintSelectedTime()
        {
            Debug.WriteLine($"Selected time: {selectedDate}");
        }
    }
}