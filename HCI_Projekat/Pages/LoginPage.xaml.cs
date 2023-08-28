using HCI_Projekat.Model;
using HCI_Projekat.Service;
using Microsoft.Win32;
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

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public MainWindow MainWindowInstance { get; set; }
        bool isValidPassword {  get; set; }
        bool isValidEmail { get; set; }
        bool firstOpenEmail { get; set; }
        bool firstOpenPassword { get; set; }

        public LoginPage()
        {
            isValidEmail = false;
            isValidPassword = false;
            firstOpenEmail = true;
            firstOpenPassword = true;
            InitializeComponent();
        }

        private void GoToRegister(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage();
            NavigationService.Navigate(registrationPage);
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;
            string password = Password.Password;
            User u = UserService.Login(email, password);
            if (u != null)
            {
                new MessageBoxCustom("Uspešno ste se ulogovali!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                MainWindowInstance.Login.Visibility = Visibility.Collapsed;
                MainWindowInstance.Register.Visibility = Visibility.Collapsed;
                MainWindowInstance.Logout.Visibility = Visibility.Visible;
                if(u.Role == Role.User)
                {
                    MainWindowInstance.Pregled.Visibility = Visibility.Visible;
                    if (MainFrame.NavigationService.CanGoBack)
                    {
                        MainFrame.NavigationService.GoBack();
                    }
                }
                else if(u.Role == Role.Agent)
                {
                    MainWindowInstance.MainFrame.NavigationService.Navigate(new AgentHomePage());
                }
                
            } else
            {
                new MessageBoxCustom("Prijava nije uspela!", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
            //OutputTextBlock.Text = "Email: " + email + "\nPassword: " + password;
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login(sender, e);
            }
        }

        private void EmailTextChanged(object sender, TextChangedEventArgs e)
        {
            string email = Email.Text;
            isValidEmail = IsValidEmail(email);
            if (isValidEmail)
            {
                EmailBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                EmailBorder.BorderThickness = new Thickness(2);
                EmailValidationMessage.Visibility = Visibility.Collapsed;
            } else
            {
                EmailBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                EmailValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateLoginButtonState();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string password = Password.Password;
            isValidPassword = IsValidPassword(password);
            if (isValidPassword)
            {
                PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                PasswordBorder.BorderThickness = new Thickness(2);
                PasswordValidationMessage.Visibility = Visibility.Collapsed;
            } else
            {
                PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                PasswordValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateLoginButtonState();
        }

        private void UpdateLoginButtonState()
        {
            if (isValidEmail && isValidPassword) 
            {
                LoginButton.Background = new SolidColorBrush(Colors.Blue);
                LoginButton.IsEnabled = true;
                //EmailBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                //PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
            } else
            {
                LoginButton.Background = new SolidColorBrush(Colors.Gray);
                LoginButton.IsEnabled = false;
            }
        }

        private bool IsValidEmail(string email)
        {
            string emailRegex = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidPassword(string password)
        {
            string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

            return System.Text.RegularExpressions.Regex.IsMatch(password, passwordRegex);
        }

        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenEmail)
            {
                firstOpenEmail = false;
                Email.TextChanged += EmailTextChanged;
                string email = Email.Text;
                isValidEmail = IsValidEmail(email);

                if (isValidEmail)
                {
                    EmailBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    EmailBorder.BorderThickness = new Thickness(2);
                    EmailValidationMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    EmailBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    EmailValidationMessage.Visibility = Visibility.Visible;
                }
                UpdateLoginButtonState();
            }
            
        }

        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenPassword)
            {
                firstOpenPassword = false;
                Password.PasswordChanged += Password_PasswordChanged;
                string password = Password.Password;
                isValidPassword = IsValidPassword(password);

                if (isValidPassword)
                {
                    PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    PasswordBorder.BorderThickness = new Thickness(2);
                    PasswordValidationMessage.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    PasswordValidationMessage.Visibility = Visibility.Visible;
                }
                UpdateLoginButtonState();
            }

        }
    }
}
