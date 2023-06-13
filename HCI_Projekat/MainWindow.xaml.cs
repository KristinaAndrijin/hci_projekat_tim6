using HCI_Projekat.Model;
using HCI_Projekat.Pages;
using HCI_Projekat.Service;
using HCI_Projekat.Forms;
using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xceed.Wpf.Toolkit;

namespace HCI_Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //private ObservableCollection<Item> Options { get; set; }
        //string connStr = "SERVER=localhost;USER=root;DATABASE=hcisql;PASSWORD=root";
        public MainWindow()
        {

            InitializeComponent();
            //var restaurants = RestaurantService.GetAllRestaurants();

            //Options = new ObservableCollection<Item>();
            //foreach (var restaurant in restaurants)
            //{
            //    Options.Add(new Item
            //    {
            //        Name = restaurant.Name,
            //        Id = restaurant.Id,
            //        Selected = true
            //    });
            //}

            //_combo.ItemsSource = Options;

            if (UserService.HasLoggedIn)
            {
                Login.Visibility = Visibility.Collapsed;
                Register.Visibility = Visibility.Collapsed;
                Logout.Visibility = Visibility.Visible;
            }
            else
            {
                Login.Visibility = Visibility.Visible;
                Register.Visibility = Visibility.Visible;
                Logout.Visibility = Visibility.Collapsed;
            }
            MainFrame.Content = new AgentHomePage();
            //MainFrame.Content = new TripForm();
        }

        private void BtnClickP1(object sender, RoutedEventArgs e)
        {
            //ignore
            MainFrame.Content = new AgentHomePage();
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            loginPage.MainWindowInstance = this;
            MainFrame.Navigate(loginPage);
        }

        private void NavigateBack(object sender, RoutedEventArgs e)
        {
            if (MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
        }

        private void GoToRegister(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RegistrationPage());
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            if (UserService.HasLoggedIn)
            {
                if (UserService.CurrentlyLoggedIn.Role == Role.Agent)
                {
                    MainFrame.Content = new HomePage();
                }
            }
            UserService.Logout();
            Login.Visibility = Visibility.Visible;
            Register.Visibility = Visibility.Visible;
            Logout.Visibility = Visibility.Collapsed;
        }

        private void GoToMape(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MapPage());
        }

        //private void _combo_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        //{
        //    CheckComboBox checkComboBox = (CheckComboBox)sender;

        //    // Get the selected items
        //    ObservableCollection<Object> selectedItems = (ObservableCollection<Object>) checkComboBox.SelectedItems;

        //    string textneki = "";
        //    // Access the selected items
        //    foreach (Item selectedItem in selectedItems)
        //    {
        //        // Perform some action with the selected item
        //        // You may need to cast the item to the appropriate type
        //        string selectedLanguage = selectedItem.Name;
        //        textneki += selectedLanguage;
        //        // Do something with the selected language
        //    }
        //    blockie.Text = textneki;
        //}
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