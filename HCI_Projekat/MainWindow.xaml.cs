using HCI_Projekat.Model;
using HCI_Projekat.Pages;
using HCI_Projekat.Service;
using HCI_Projekat.Forms;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System.Net;
using System.IO;
using System.Data;
using System.Xml.Linq;
using System.ComponentModel;
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
                if(UserService.CurrentlyLoggedIn!.Role == Role.User)
                {
                    Pregled.Visibility = Visibility.Visible;
                }
                
            }
            else
            {
                Login.Visibility = Visibility.Visible;
                Register.Visibility = Visibility.Visible;
                Logout.Visibility = Visibility.Collapsed;
                Pregled.Visibility = Visibility.Collapsed;
            }
            MainFrame.Content = new HomePage();
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
                if(MainFrame.Content is UserTripsOverviewPage)
                {
                    MainFrame.NavigationService.GoBack();
                }
            }
            UserService.Logout();
            Login.Visibility = Visibility.Visible;
            Register.Visibility = Visibility.Visible;
            Logout.Visibility = Visibility.Collapsed;
            Pregled.Visibility = Visibility.Collapsed;
        }

        private void GoToMape(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MapPage());
        }

        private void GoToUserTripOverview(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserTripsOverviewPage());
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