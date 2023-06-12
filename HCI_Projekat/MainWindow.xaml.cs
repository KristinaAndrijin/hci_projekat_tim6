using HCI_Projekat.Model;
using HCI_Projekat.Pages;
using HCI_Projekat.Service;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
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
using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System.Net;
using System.IO;
using System.Data;
using System.Xml.Linq;

namespace HCI_Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string connStr = "SERVER=localhost;USER=root;DATABASE=hcisql;PASSWORD=root";
        public MainWindow()
        {
            InitializeComponent();

            TripServicec.Add();
            TripServicec.Add();
            TripServicec.Add();
            TripServicec.Add();
            TripServicec.Add();
            TripServicec.Add();

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
        }

        private void BtnClickP1(object sender, RoutedEventArgs e)
        {
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
                if(UserService.CurrentlyLoggedIn.Role == Role.Agent)
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
    }
}
