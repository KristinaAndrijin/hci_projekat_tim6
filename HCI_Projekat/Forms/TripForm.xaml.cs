using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HCI_Projekat.Model;
using HCI_Projekat.Service;
using Microsoft.Maps.MapControl.WPF;
using Xceed.Wpf.Toolkit;

namespace HCI_Projekat.Forms
{
    /// <summary>
    /// Interaction logic for TripForm.xaml
    /// </summary>
    public partial class TripForm : Window
    {
        private ObservableCollection<Item> Options { get; set; }
        const string BING_API_KEY = "Ah8LozC7khuISaCoOppLN2Vm_mhOD65qXBZVEDcQoZ34UApWABR9jxtuKdlYb7jV";
        bool edit { get; set; }
        public event EventHandler ItemAdded;
        bool isStartAddressValid { get; set; }
        bool firstOpenStartAddress { get; set; }
        bool isStartCityValid { get; set; }
        bool firstOpenStartCity { get; set; }
        bool isNameValid { get; set; }
        bool firstOpenName { get; set; }
        bool isPriceValid { get; set; }
        bool firstOpenPrice { get; set; }
        Pushpin startPushpin { get; set; }
        string pinStartAddress { get; set; }
        bool isEndAddressValid { get; set; }
        bool firstOpenEndAddress { get; set; }
        bool isEndCityValid { get; set; }
        bool firstOpenEndCity { get; set; }
        Pushpin endPushpin { get; set; }
        string pinEndAddress { get; set; }

        bool editFirstStartAddress { get; set; }    
        bool editFirstEndAddress { get; set; }
        bool editFirstStartCity { get; set; }
        bool editFirstEndCity { get; set; }
        bool editFirstName { get; set; }
        bool editFirstPrice { get; set; }
        string nameEdit { get; set; }
        int priceEdit { get; set; }

        string addressStartEdit { get; set; }
        string addressEndEdit { get; set; }
        string cityStartEdit { get; set; }
        string cityEndEdit { get; set; }

        public TripForm()
        {
            InitializeComponent();
            PreviousButton.IsEnabled = false;
            Map.Center = new Microsoft.Maps.MapControl.WPF.Location(44.7866, 20.4489); // Beograd
            Map.ZoomLevel = 12;
            edit = false;
            isStartAddressValid = false;
            firstOpenStartAddress = true;
            isStartCityValid = false;
            firstOpenStartCity = true;
            isNameValid = false;
            firstOpenName = true;
            isEndAddressValid = false;
            firstOpenEndAddress = true;
            isEndCityValid = false;
            firstOpenEndCity = true;
            isPriceValid = false;
            firstOpenPrice = true;
            StartAddress.TextChanged += StartAddressTextChanged;
            StartCity.TextChanged += StartCityTextChanged;
            EndAddress.TextChanged += EndAddressTextChanged;
            EndCity.TextChanged += EndCityTextChanged;
            Price.TextChanged += PriceTextChanged;
            InitCombox();
            editFirstEndAddress = false;
            editFirstStartAddress = false;
            editFirstStartCity = false;
            editFirstEndCity = false;
            editFirstName = false;
            editFirstPrice = false;
        }

        public TripForm(Trip trip)
        {
            InitializeComponent();
            PreviousButton.IsEnabled = false;
            Map.Center = new Microsoft.Maps.MapControl.WPF.Location(44.7866, 20.4489); // Beograd
            Map.ZoomLevel = 12;
            edit = true;
            SubmitBtn.Content = "Izmeni putovanje";
            isStartAddressValid = true;
            firstOpenStartAddress = false;
            isStartCityValid = true;
            firstOpenStartCity = false;
            isNameValid = true;
            isEndAddressValid = true;
            firstOpenEndAddress = false;
            isEndCityValid = true;
            firstOpenEndCity = false;
            firstOpenName = false;
            isPriceValid = true;
            firstOpenPrice = false;
            InitCombox();
            editFirstEndAddress = true;
            editFirstStartAddress = true;
            editFirstStartCity = true;
            editFirstEndCity = true;
            editFirstName = true;
            editFirstPrice = true;


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

            StartAddress.Text = streetStart;
            addressStartEdit = streetStart;
            StartCity.Text = cityStart;
            cityStartEdit = cityStart;
            EndAddress.Text = streetEnd;
            addressEndEdit = streetEnd;
            EndCity.Text = cityEnd;
            cityEndEdit = cityEnd;

            Name.Text = trip.Name;
            nameEdit = trip.Name;
            Price.Text = trip.Price.ToString();
            priceEdit = trip.Price;

            putPinPls(streetStart + ", " + cityStart + ", Srbija", true);
            putPinPls(streetEnd + ", " + cityEnd + ", Srbija", false);

            StartAddress.TextChanged += StartAddressTextChanged;
            StartCity.TextChanged += StartCityTextChanged;
            EndAddress.TextChanged += EndAddressTextChanged;
            EndCity.TextChanged += EndCityTextChanged;
            Price.TextChanged += PriceTextChanged;
            
        }

        private void InitCombox()
        {
            var restaurants = RestaurantService.GetAllRestaurants();
            Options = new ObservableCollection<Item>();
            foreach (var restaurant in restaurants)
            {
                Options.Add(new Item
                {
                    Name = restaurant.Name,
                    Id = restaurant.Id,
                    Selected = true
                });
            }
            _combo_restaurant.ItemsSource = Options;

            var accomodations = AccomodationService.GetAllAccomodations();

            Options = new ObservableCollection<Item>();
            foreach (var accomodation in accomodations)
            {
                Options.Add(new Item
                {
                    Name = accomodation.Name,
                    Id = accomodation.Id,
                    Selected = true
                });
            }
            _combo_accomodation.ItemsSource = Options;

            var attractions = AttractionService.GetAllAttractions();
            Options = new ObservableCollection<Item>();
            foreach (var attraction in attractions)
            {
                Options.Add(new Item
                {
                    Name = attraction.Name,
                    Id = attraction.Id,
                    Selected = true
                });
            }
            _combo_attraction.ItemsSource = Options;
        }



        private void Next_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = tabControl.SelectedIndex;
            if (currentIndex < tabControl.Items.Count - 1)
            {
                tabControl.SelectedIndex = currentIndex + 1;
                UpdateButtonState();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = tabControl.SelectedIndex;
            if (currentIndex > 0)
            {
                tabControl.SelectedIndex = currentIndex - 1;
                UpdateButtonState();
            }
        }

        private void UpdateButtonState()
        {
            int currentIndex = tabControl.SelectedIndex;

            // Enable or disable the Previous button based on the current index
            PreviousButton.IsEnabled = currentIndex > 0;

            // Enable or disable the Next button based on the current index
            NextButton.IsEnabled = currentIndex < tabControl.Items.Count - 1;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                            pinStartAddress = address;
                        }
                        else
                        {
                            endPushpin = pin;
                            pinEndAddress = address;
                        }
                        Map.Center = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        UpdateNextButton();
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
            }
        }

        private void StartAddressTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstStartAddress)
            {
                StartAddress.Text = addressStartEdit;
                editFirstStartAddress = false;
                return;
            }
            string address = StartAddress.Text;
            isStartAddressValid = IsValidAddress(address);
            if (isStartAddressValid)
            {
                StartAddressBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                StartAddressBorder.BorderThickness = new Thickness(2);
                if (isStartCityValid)
                {
                    StartAddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateStartButton();
                UpdateNextButton();
            }
            else
            {
                StartAddressBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                StartAddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidAddress(string address)
        {
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$";
            return System.Text.RegularExpressions.Regex.IsMatch(address, regex);
        }

        private void EndAddressTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstEndAddress)
            {
                StartAddress.Text = addressEndEdit;
                editFirstEndAddress = false;
                return;
            }
            string address = EndAddress.Text;
            isEndAddressValid = IsValidAddress(address);
            if (isEndAddressValid)
            {
                EndAddressBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                EndAddressBorder.BorderThickness = new Thickness(2);
                if (isEndCityValid)
                {
                    EndAddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateEndButton();
                UpdateNextButton();
            }
            else
            {
                EndAddressBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                EndAddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private void StartCityTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstStartCity)
            {
                StartCity.Text = cityStartEdit;
                editFirstStartCity = false;
                return;
            }
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            string city = StartCity.Text;
            isStartCityValid = IsValidCity(city);
            if (isStartCityValid)
            {
                StartCityBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                StartCityBorder.BorderThickness = new Thickness(2);
                if (isStartAddressValid)
                {
                    StartAddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateStartButton();
                UpdateNextButton();
            }
            else
            {
                StartCityBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                StartAddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidCity(string city)
        {
            string regex = @"^[a-zA-Z\s]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(city, regex);
        }

        private void EndCityTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstEndCity)
            {
                EndCity.Text = cityEndEdit;
                editFirstEndCity = false;
                return;
            }
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            string city = EndCity.Text;
            isEndCityValid = IsValidCity(city);
            if (isEndCityValid)
            {
                EndCityBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                EndCityBorder.BorderThickness = new Thickness(2);
                if (isEndAddressValid)
                {
                    EndAddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateEndButton();
                UpdateNextButton();
            }
            else
            {
                EndCityBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                EndAddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private void ShowStart(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAddStart;
;
            if (startPushpin != null)
            {
                Map.Children.Remove(startPushpin);
                startPushpin = null;
            }
            try
            {
                String address = StartAddress.Text.Trim() + ", " + StartCity.Text.Trim() + ", Srbija";
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
                        startPushpin = pin;
                        pinStartAddress = address;
                        Map.Center = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        UpdateStartButton();
                        UpdateNextButton();
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
            }
        }

        private void ShowEnd(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAddEnd;
            if (endPushpin != null)
            {
                Map.Children.Remove(endPushpin);
                endPushpin = null;
            }
            try
            {
                String address = EndAddress.Text.Trim() + ", " + EndCity.Text.Trim() + ", Srbija";
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
                        endPushpin = pin;
                        pinEndAddress = address;
                        Map.Center = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        UpdateEndButton();
                        UpdateNextButton();
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
            }
        }

        private void AddStartPushpinClick(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAddStart;
            if (startPushpin != null)
            {
                Map.Children.Remove(startPushpin);
                startPushpin = null;
            }
        }

        private void AddEndPushpinClick(object sender, RoutedEventArgs e)
        {
            NextButton.IsEnabled = false;
            //NextButton.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAddEnd;
            if (endPushpin != null)
            {
                Map.Children.Remove(endPushpin);
                endPushpin = null;
            }
        }

        private void Map_MouseLeftButtonDownAddStart(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Microsoft.Maps.MapControl.WPF.Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                Pushpin pin = new Pushpin { Location = mouseLocation, Background = new SolidColorBrush(Colors.Red) };
                Map.Children.Add(pin);
                startPushpin = pin;
                try
                {
                    String res_address = GetAddressFromLocation(mouseLocation.Latitude, mouseLocation.Longitude);
                    if (res_address != null)
                    {
                        pin.ToolTip = res_address;
                        pinStartAddress = res_address;
                        //pushpin = pin;
                        UpdateStartButton();
                        UpdateNextButton();
                    }
                    else
                    {
                        var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                        msgBox.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: " + ex.Message, MessageType.Warning, MessageButtons.Ok);
                    msgBox.ShowDialog();
                }
            }
            Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownAddStart;
        }

        private void Map_MouseLeftButtonDownAddEnd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Microsoft.Maps.MapControl.WPF.Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                Pushpin pin = new Pushpin { Location = mouseLocation, Background = new SolidColorBrush(Colors.Red) };
                Map.Children.Add(pin);
                endPushpin = pin;
                try
                {
                    String res_address = GetAddressFromLocation(mouseLocation.Latitude, mouseLocation.Longitude);
                    if (res_address != null)
                    {
                        pin.ToolTip = res_address;
                        pinEndAddress = res_address;
                        //pushpin = pin;
                        UpdateEndButton();
                        UpdateNextButton();
                    }
                    else
                    {
                        var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                        msgBox.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                    msgBox.ShowDialog();
                }
            }
            Map.MouseLeftButtonDown -= Map_MouseLeftButtonDownAddEnd;
        }

        private string GetAddressFromLocation(double latitude, double longitude)
        {
            string requestUrl = $"https://dev.virtualearth.net/REST/v1/Locations/{latitude},{longitude}?key={BING_API_KEY}";

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(res.GetResponseStream()))
                    {
                        string responseJson = reader.ReadToEnd();

                        dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(responseJson);
                        //proba.Text = data.ToString();
                        var address = data["resourceSets"][0]["resources"][0]["address"];
                        return address["formattedAddress"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                var msgBox = new MessageBoxCustom("Adresa neuspešno unešena: ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
                return null;
            }
        }

        private void UpdateStartButton()
        {
            if (startPushpin != null)
            {
                StartShowBtn.IsEnabled = true;
                //StartShowBtn.Background = new SolidColorBrush(Colors.Blue);
            }
        }

        private void UpdateEndButton()
        {
            if (endPushpin != null)
            {
                EndShowBtn.IsEnabled = true;
                //EndShowBtn.Background = new SolidColorBrush(Colors.Blue);
            }
        }

        private void UpdateNextButton()
        {
            if (endPushpin != null && startPushpin != null)
            {
                NextButton.IsEnabled = true;
                //NextButton.Background = new SolidColorBrush(Colors.Blue);
            }
        }

        private void _combo_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            CheckComboBox checkComboBox = (CheckComboBox)sender;

            // Get the selected items
            ObservableCollection<Object> selectedItems = (ObservableCollection<Object>)checkComboBox.SelectedItems;

            string textneki = "";
            // Access the selected items
            foreach (Item selectedItem in selectedItems)
            {
                // Perform some action with the selected item
                // You may need to cast the item to the appropriate type
                string selectedLanguage = selectedItem.Name;
                textneki += selectedLanguage;
                // Do something with the selected language
            }
            //blockie.Text = textneki;
        }

        private void NameTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstName)
            {
                Name.Text = nameEdit;
                editFirstName = false;
                return;
            }
            string name = Name.Text;
            isNameValid = IsValidName(name);
            if (isNameValid)
            {
                NameBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                NameBorder.BorderThickness = new Thickness(2);
                NameValidationMessage.Visibility = Visibility.Collapsed;
                UpdateSubmitButton();
            }
            else
            {
                NameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                NameValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidName(string name)
        {
            string regex = @"^[a-zA-Z0-9\s]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(name, regex);
        }

        private void PriceTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstPrice)
            {
                Price.Text = priceEdit.ToString();
                editFirstPrice = false;
                return;
            }
            string price = Price.Text;
            isPriceValid = IsValidPrice(price);
            if (isPriceValid)
            {
                PriceBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                PriceBorder.BorderThickness = new Thickness(2);
                PriceValidationMessage.Visibility = Visibility.Collapsed;
                UpdateSubmitButton();
            }
            else
            {
                PriceBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                PriceValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidPrice(string price)
        {
            string regex = @"^[0-9]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(price, regex);
        }

        private void UpdateSubmitButton()
        {
            if (endPushpin != null && startPushpin != null && Name.Text != null && Price.Text !=null)
            {
                SubmitBtn.IsEnabled = true;
                SubmitBtn.Background = new SolidColorBrush(Colors.Blue);
                //NextButton.Background = new SolidColorBrush(Colors.Blue);
            }
        }

        private void AddTrip(object sender, RoutedEventArgs e)
        {
            int numPrice;
            bool success = int.TryParse(Price.Text, out numPrice);
            if (!success)
            {
                var msgBox = new MessageBoxCustom("Cena nije adekvatno unešena (samo brojevi) ", MessageType.Warning, MessageButtons.Ok);
                msgBox.ShowDialog();
                return;
            }
            else
            {
                TripService.Add(pinStartAddress, pinEndAddress, Name.Text, numPrice);
                var msgBox = new MessageBoxCustom("Uspešno dodato putovanje ", MessageType.Success, MessageButtons.Ok);
                msgBox.ShowDialog();
            }
        }

    }

    class Item : INotifyPropertyChanged
    {
        private bool _selected;
        private string _name;
        private int _id;

        public string Name
        {
            get => _name; set
            {
                _name = value;
                EmitChange(nameof(Name));
            }
        }
        public bool Selected
        {
            get => _selected; set
            {
                _selected = value;
                EmitChange(nameof(Selected));
            }
        }

        public int Id
        {
            get => _id; set
            {
                _id = value;
                EmitChange(nameof(Id));
            }
        }

        private void EmitChange(params string[] names)
        {
            if (PropertyChanged == null)
                return;
            foreach (var name in names)
                PropertyChanged(this,
                  new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler
                       PropertyChanged;
    }
}
