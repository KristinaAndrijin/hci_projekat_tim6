using HCI_Projekat.Model;
using HCI_Projekat.Pages.Tabele;
using HCI_Projekat.Service;
using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace HCI_Projekat.Forms
{
    /// <summary>
    /// Interaction logic for AccomodationForm.xaml
    /// </summary>
    public partial class tripForm : Window
    {
        bool isAddressValid { get; set; }
        bool firstOpenAddress { get; set; }
        bool isCityValid { get; set; }
        bool firstOpenCity { get; set; }
        bool isNameValid { get; set; }
        bool firstOpenName { get; set; }
        Pushpin pushpin { get; set; }
        RestaurantType type { get; set; }
        string pin_address { get; set; }
        bool edit { get; set; }
        int restaurant_id { get; set; }
        bool editFirstAddress { get; set; }
        bool editFirstCity { get; set; }
        bool editFirstName { get; set; }
        string addressEdit { get; set; }
        string cityEdit { get; set; }
        string nameEdit { get; set; }
        public event EventHandler ItemAdded;//event koji hvatam u tabeli


        const string BING_API_KEY = "Ah8LozC7khuISaCoOppLN2Vm_mhOD65qXBZVEDcQoZ34UApWABR9jxtuKdlYb7jV";
        public tripForm()
        {
            InitializeComponent();
            Map.Center = new Location(44.7866, 20.4489); // Beograd
            Map.ZoomLevel = 12;
            isAddressValid = false;
            firstOpenAddress = true;
            isCityValid = false;
            firstOpenCity = true;
            isNameValid = false;
            firstOpenName = true;
            edit = false;
            Address.TextChanged += AddressTextChanged;
            City.TextChanged += CityTextChanged;
            Name.TextChanged += NameTextChanged;
            editFirstAddress = false;
            editFirstCity = false;
            editFirstName = false;
        }

        public tripForm(Restaurant restaurant)
        {
            //Address.TextChanged -= AddressTextChanged;
            //City.TextChanged -= CityTextChanged;
            //Name.TextChanged -= NameTextChanged;
            InitializeComponent();
            SubmitBtn.Content = "Izmeni restoran";
            edit = true;
            Map.Center = new Location(44.7866, 20.4489); // Beograd
            Map.ZoomLevel = 12;
            isAddressValid = true;
            firstOpenAddress = false;
            isCityValid = true;
            firstOpenCity = false;
            isNameValid = true;
            firstOpenName = false;
            string street = "";
            string city = "";
            string addressToParse = restaurant.Address;
            string[] partsOfAddress = addressToParse.Split(", ");
            if (partsOfAddress.Length == 4)
            {
                street = partsOfAddress[0];
                city = partsOfAddress[2].Split(" ")[1];
            }
            else if (partsOfAddress.Length == 3)
            {
                street = partsOfAddress[0];
                if (partsOfAddress[1].Split(" ").Length > 1) {
                    city = partsOfAddress[1].Split(" ")[1];
                } else
                {
                    city = partsOfAddress[1];
                }

            }
            Address.Text = street;
            addressEdit = street;
            City.Text = city;
            cityEdit = city;
            RestaurantType t = restaurant.Type;
            if (t == RestaurantType.Ethno)
            {
                Combo.SelectedIndex = 0;
            }
            else if (t == RestaurantType.Modern)
            {
                Combo.SelectedIndex = 1;
            }
            else
            {
                Combo.SelectedIndex = 2;
            }
            Name.Text = restaurant.Name;
            nameEdit = restaurant.Name;
            putPinPls(street + ", " + city + ", Srbija");
            restaurant_id = restaurant.Id;
            editFirstAddress = true;
            editFirstCity = true;
            editFirstName = true;
            Address.TextChanged += AddressTextChanged;
            City.TextChanged += CityTextChanged;
            Name.TextChanged += NameTextChanged;
        }

        private void putPinPls(string address)
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

                        Location location = new Location(latitude, longitude);
                        Pushpin pin = new Pushpin { Location = location, Background = new SolidColorBrush(Colors.Blue) };
                        Map.Children.Add(pin);
                        pushpin = pin;
                        pin_address = address;
                        Map.Center = new Location(latitude, longitude);
                        UpdateButton();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adresa neuspešno unešena: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Address_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenAddress)
            {
                firstOpenAddress = false;
                Address.TextChanged += AddressTextChanged;
                string address = Address.Text;
                isAddressValid = IsValidAddress(address);

                if (isAddressValid)
                {
                    AddressBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    AddressBorder.BorderThickness = new Thickness(2);
                    if (isCityValid)
                    {
                        AddressValidationMessage.Visibility = Visibility.Collapsed;
                    }
                    UpdateButton();
                }
                else
                {
                    AddressBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    AddressValidationMessage.Visibility = Visibility.Visible;
                }
            }

        }

        private void AddressTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstAddress)
            {
                Address.Text = addressEdit;
                editFirstAddress = false;
                return;
            }
            string address = Address.Text;
            isAddressValid = IsValidAddress(address);
            if (isAddressValid)
            {
                AddressBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                AddressBorder.BorderThickness = new Thickness(2);
                if (isCityValid)
                {
                    AddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateButton();
            }
            else
            {
                AddressBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                AddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidAddress(string address)
        {
            string regex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$";
            return System.Text.RegularExpressions.Regex.IsMatch(address, regex);
        }

        private void City_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenCity)
            {
                firstOpenCity = false;
                City.TextChanged += CityTextChanged;
                string city = City.Text;
                isCityValid = IsValidCity(city);

                if (isCityValid)
                {
                    CityBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    CityBorder.BorderThickness = new Thickness(2);
                    if (isAddressValid)
                    {
                        AddressValidationMessage.Visibility = Visibility.Collapsed;
                    }
                    UpdateButton();
                }
                else
                {
                    CityBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    AddressValidationMessage.Visibility = Visibility.Visible;
                }
            }

        }

        private void CityTextChanged(object sender, TextChangedEventArgs e)
        {
            if (editFirstCity)
            {
                City.Text = cityEdit;
                editFirstCity = false;
                return;
            }
            SubmitBtn.IsEnabled = false;
            SubmitBtn.Background = new SolidColorBrush(Colors.Gray);
            string city = City.Text;
            isCityValid = IsValidCity(city);
            if (isCityValid)
            {
                CityBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                CityBorder.BorderThickness = new Thickness(2);
                if (isAddressValid)
                {
                    AddressValidationMessage.Visibility = Visibility.Collapsed;
                }
                UpdateButton();
            }
            else
            {
                CityBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                AddressValidationMessage.Visibility = Visibility.Visible;
            }
        }

        private bool IsValidCity(string city)
        {
            string regex = @"^[a-zA-Z\s]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(city, regex);
        }

        private void Show(object sender, RoutedEventArgs e)
        {
            SubmitBtn.IsEnabled = false;
            SubmitBtn.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAdd;
            if (pushpin != null)
            {
                Map.Children.Remove(pushpin);
                pushpin = null;
            }
            try
            {
                String address = Address.Text.Trim() + ", " + City.Text.Trim() + ", Srbija";
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

                        Location location = new Location(latitude, longitude);
                        Pushpin pin = new Pushpin { Location = location, Background = new SolidColorBrush(Colors.Blue) };
                        Map.Children.Add(pin);
                        pushpin = pin;
                        pin_address = address;
                        Map.Center = new Location(latitude, longitude);
                        UpdateButton();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adresa neuspešno unešena: " + ex.Message);
            }
        }

        private void AddPushpinClick(object sender, RoutedEventArgs e)
        {
            SubmitBtn.IsEnabled = false;
            SubmitBtn.Background = new SolidColorBrush(Colors.Gray);
            Map.MouseLeftButtonDown += Map_MouseLeftButtonDownAdd;
            if (pushpin != null)
            {
                Map.Children.Remove(pushpin);
                pushpin = null;
            }
        }

        private void Map_MouseLeftButtonDownAdd(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                Location mouseLocation = Map.ViewportPointToLocation(e.GetPosition(Map));
                Pushpin pin = new Pushpin { Location = mouseLocation, Background = new SolidColorBrush(Colors.Red) };
                Map.Children.Add(pin);
                pushpin = pin;
                try
                {
                    String res_address = GetAddressFromLocation(mouseLocation.Latitude, mouseLocation.Longitude);
                    if (res_address != null)
                    {
                        pin.ToolTip = res_address;
                        pin_address = res_address;
                        //pushpin = pin;
                        UpdateButton();
                    }
                    else
                    {
                        MessageBox.Show("Adresa neuspešno pročitana");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Adresa neuspešno pročitana: " + ex.Message);
                }
            }
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
                MessageBox.Show("Adresa neuspešno pročitana: " + ex.Message);
                return null;
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            string selectedText = selectedItem.Content.ToString();

            if (selectedText == "Etno")
            {
                type = RestaurantType.Ethno;
            }
            else if (selectedText == "Moderan")
            {
                type = RestaurantType.Modern;
            }
            else
            {
                type = RestaurantType.FastFood;
            }
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
                UpdateButton();
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

        private void AddAccomodation(object sender, RoutedEventArgs e)
        {
            if (!edit)
            {
                RestaurantService.Add(pin_address, type, Name.Text);
                new MessageBoxCustom("Restoran je uspešno dodat", MessageType.Success, MessageButtons.Ok).ShowDialog();
                ItemAdded?.Invoke(this, EventArgs.Empty);//event koji hvatam u tabeli
                Address.Text = null;
                City.Text = null;
                Name.Text = null;
            }
            else
            {
                RestaurantService.Edit(restaurant_id, pin_address, type, Name.Text);
                new MessageBoxCustom("Restoran je uspešno izmenjen", MessageType.Success, MessageButtons.Ok).ShowDialog();
                ItemAdded?.Invoke(this, EventArgs.Empty);//event koji hvatam u tabeli
                this.Close();
            }
        }

        private void UpdateButton()
        {
            if (isNameValid && pushpin != null)
            {
                SubmitBtn.IsEnabled = true;
                SubmitBtn.Background = new SolidColorBrush(Colors.Blue);
            }
        }

    }
}
