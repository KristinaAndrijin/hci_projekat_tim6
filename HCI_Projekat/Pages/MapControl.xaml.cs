using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for MapPage.xaml
    /// </summary>
    public partial class MapControl : UserControl
    {
        private Pushpin currentPushpin;
        private bool thereIsPinToMove;

        private bool isMapMouseLeftButtonDownAddHandled; // dodavanje pinova na klik
        private bool isMapMouseLeftButtonDownMoveHandled; // pomeranje pinova na klik
        private List<Pushpin> allPushpins;

        // RightButtonDown za Pushpin i mapu je uvek ok


        // List<bool> cuva redom: Pushpin +
        // + MouseLeftButtonDownMove - default false
        // + MouseLeftButtonDownDelete - default false
        private Dictionary<Pushpin, List<bool>> pushpinReactions;
        public MapControl()
        {
            InitializeComponent();
            Map.Center = new Location(44.7866, 20.4489); // Beograd
            Map.ZoomLevel = 12;
            allPushpins = new List<Pushpin>();
            pushpinReactions = new Dictionary<Pushpin, List<bool>>();
            thereIsPinToMove = false;
        }

        private void SearchAndAddPushpins_Click(object sender, RoutedEventArgs e)
        {
            List<string> addresses = new List<string>
            {
                "Belgrade, Serbia",
                "Bačka 92, Zrenjanin, Srbija"
            };

            SearchAndAddPushpinsWithRequestUrl(addresses);
        }

        private void SearchAndAddPushpinsWithRequestUrl(List<string> addresses)
        {
            foreach (string address in addresses)
            {
                try
                {
                    string requestUrl = string.Format("http://dev.virtualearth.net/REST/v1/Locations?q={0}&key={1}", address, "Ah8LozC7khuISaCoOppLN2Vm_mhOD65qXBZVEDcQoZ34UApWABR9jxtuKdlYb7jV");

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseJson = reader.ReadToEnd();

                            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                            double latitude = data.resourceSets[0].resources[0].point.coordinates[0];
                            double longitude = data.resourceSets[0].resources[0].point.coordinates[1];

                            Location location = new Location(latitude, longitude);

                            // Pushpin pushpin = new Pushpin();
                            Pushpin pushpin = new Pushpin { Location = location, Background = new SolidColorBrush(Colors.Blue) };
                            // pushpin.Background = new SolidColorBrush(Colors.Blue);
                            //pushpin.Tag = "Moj prvi pun!!!";
                            //pushpin.ToolTip = "Moj prvi pin!!";
                            // pushpin.Location = location;
                            // MapLayer.SetPosition(pushpin, location);
                            Map.Children.Add(pushpin);

                            allPushpins.Add(pushpin);
                            pushpinReactions[pushpin] = new List<bool>() { false, false };
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

        }

        private void Map_MouseLeftButtonDownAdd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                Pushpin pin = new Pushpin { Location = mouseLocation, Background = new SolidColorBrush(Colors.Red) };
                Map.Children.Add(pin);
                allPushpins.Add(pin);
                pushpinReactions[pin] = new List<bool>() { false, false };
            }
        }

        private void Map_MouseLeftButtonDownMove(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (thereIsPinToMove && e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                // Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                currentPushpin.Location = mouseLocation;
                Map.Children.Add(currentPushpin);
                allPushpins.Add(currentPushpin);
            }
        }

        private void Pushpin_MouseLeftButtonDownDelete(object sender, MouseButtonEventArgs e)
        {
            Pushpin toDelete = (Pushpin)sender;
            allPushpins.Remove(toDelete);
            Map.Children.Remove(toDelete);
        }

        private void Pushpin_MouseLeftButtonDownMove(object sender, MouseButtonEventArgs e)
        {
            currentPushpin = (Pushpin)sender;
            allPushpins.Remove(currentPushpin);
            Map.Children.Remove(currentPushpin);
            thereIsPinToMove = true;
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownMove;
        }

        private void AddPushpinClick(object sender, RoutedEventArgs e)
        {
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAdd;
            isMapMouseLeftButtonDownAddHandled = true;
            if (isMapMouseLeftButtonDownMoveHandled)
            {
                Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownMove;
                isMapMouseLeftButtonDownMoveHandled = false;
            }
        }

        private void MovePushpin(object sender, RoutedEventArgs e)
        {
            foreach (var pin in allPushpins)
            {
                pushpinReactions[pin] = new List<bool>() { true, false };
                pin.MouseLeftButtonDown += Pushpin_MouseLeftButtonDownMove;
            }
            isMapMouseLeftButtonDownMoveHandled = true;
            if (isMapMouseLeftButtonDownAddHandled)
            {
                Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownAdd;
                isMapMouseLeftButtonDownAddHandled = false;
            }
        }


        private void ConnectPushpins(object sender, RoutedEventArgs e)
        {
            if (allPushpins.Count <= 2)
            {
                // At least 2 pushpins are required to draw a line
                return;
            }

            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new SolidColorBrush(Colors.Blue);
            polyline.StrokeThickness = 2;

            LocationCollection locations = new LocationCollection();
            foreach (Pushpin pushpin in allPushpins)
            {
                locations.Add(pushpin.Location);
            }

            polyline.Locations = locations;
            Map.Children.Add(polyline);
        }


        private void DeletePushpins(object sender, RoutedEventArgs e)
        {
            foreach (var pin in allPushpins)
            {
                pushpinReactions[pin] = new List<bool>() { false, true };
                pin.MouseLeftButtonDown += Pushpin_MouseLeftButtonDownDelete;
            }


        }

        private void NormalView(object sender, RoutedEventArgs e)
        {
            if (isMapMouseLeftButtonDownAddHandled)
            {
                Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownAdd;
            }

            if (isMapMouseLeftButtonDownMoveHandled)
            {
                Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownMove;
            }

            foreach (var pin in allPushpins)
            {
                List<bool> permissions = pushpinReactions[pin];
                if (permissions[0])
                {
                    pin.MouseLeftButtonDown -= Pushpin_MouseLeftButtonDownMove;
                }

                if (permissions[1])
                {
                    pin.MouseLeftButtonDown -= Pushpin_MouseLeftButtonDownDelete;
                }
                pushpinReactions[pin] = new List<bool>() { false, false };
            }
        }
    }
}
