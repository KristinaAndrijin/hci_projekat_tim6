using HCI_Projekat.Model;
using HCI_Projekat.Repository;
using HCI_Projekat.Service;
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
using static System.Net.Mime.MediaTypeNames;
using AppContext = HCI_Projekat.Repository.AppContext;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        //private readonly AppContext dbContext;
        bool isNameValid { get; set; }
        bool firstOpenName { get; set; }
        bool isSurnameValid { get; set; }
        bool firstOpenSurname { get; set; }
        bool isValidEmail { get; set; }
        bool firstOpenEmail { get; set; }
        bool isValidPassword { get; set; }
        bool firstOpenPassword { get; set; }
        bool isValidRepeatPassword { get; set; }
        bool firstOpenRepeatPassword { get; set; }
        public RegistrationPage()
        {
            isNameValid = false;
            firstOpenName = true;
            isSurnameValid = false;
            firstOpenSurname = true;
            isValidEmail = false;
            firstOpenEmail = true;
            isValidPassword = false;
            firstOpenPassword = true;
            isValidRepeatPassword = false;
            firstOpenRepeatPassword = true;
            InitializeComponent();
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage();
            NavigationService.Navigate(loginPage);
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            string email = Email.Text;
            string password = Password.Password;
            string repeatPassword = RepeatPassword.Password;
            string name = Name.Text;
            string surname = Surname.Text;
            if (password != repeatPassword)
            {
                new MessageBoxCustom("Lozinke se ne poklapaju!", MessageType.Warning, MessageButtons.Ok).ShowDialog();
            }
            else
            {
                //OutputTextBlock.Text = "Email: " + email + "\n" + "Password: " + password + "\n" +
                //"Repeat: " + repeatPassword + "\n"
                //+ "Name: " + name + "\n"
                //+ "Surname: " + surname;
                UserService.Register(email, password, name, surname, "User");
                new MessageBoxCustom("Uspešno ste se registrovali!", MessageType.Success, MessageButtons.Ok).ShowDialog();
            }
        }

        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Register(sender, e);
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenName)
            {
                firstOpenName = false;
                Name.TextChanged += NameTextChanged;
                string name = Name.Text;
                isNameValid = IsValidName(name);

                if (isNameValid)
                {
                    NameBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    NameBorder.BorderThickness = new Thickness(2);
                    if (isSurnameValid)
                    {
                        NameValidationMessage.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    NameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    NameValidationMessage.Visibility = Visibility.Visible;
                }
                UpdateRegistartionButtonState();
            }

        }

        private void NameTextChanged(object sender, TextChangedEventArgs e)
        {
            string name = Name.Text;
            isNameValid = IsValidName(name);
            if (isNameValid)
            {
                NameBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                NameBorder.BorderThickness = new Thickness(2);
                if (isSurnameValid)
                {
                    NameValidationMessage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                NameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                NameValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateRegistartionButtonState();
        }

        private bool IsValidName(string name)
        {
            string regex = @"^[a-zA-Z]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(name, regex);
        }

        private void Surname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenSurname)
            {
                firstOpenSurname = false;
                Surname.TextChanged += SurnameTextChanged;
                string surname = Surname.Text;
                isSurnameValid = IsValidSurname(surname);

                if (isSurnameValid)
                {
                    SurnameBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    SurnameBorder.BorderThickness = new Thickness(2);
                    if (isNameValid)
                    {
                        NameValidationMessage.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    SurnameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    NameValidationMessage.Visibility = Visibility.Visible;
                }
                UpdateRegistartionButtonState();
            }

        }

        private void SurnameTextChanged(object sender, TextChangedEventArgs e)
        {
            string surname = Surname.Text;
            isSurnameValid = IsValidSurname(surname);
            if (isSurnameValid)
            {
                SurnameBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                SurnameBorder.BorderThickness = new Thickness(2);
                if (isNameValid)
                {
                    NameValidationMessage.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                SurnameBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                NameValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateRegistartionButtonState();
        }

        private bool IsValidSurname(string surname)
        {
            string regex = @"^[a-zA-Z]+$";

            return System.Text.RegularExpressions.Regex.IsMatch(surname, regex);
        }

        private void UpdateRegistartionButtonState()
        {
            if (isNameValid && isSurnameValid && isValidEmail && isValidPassword && isValidRepeatPassword)
            {
                RegisterButton.Background = new SolidColorBrush(Colors.Blue);
                RegisterButton.IsEnabled = true;
                //EmailBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                //PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else
            {
                RegisterButton.Background = new SolidColorBrush(Colors.Gray);
                RegisterButton.IsEnabled = false;
            }
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
                UpdateRegistartionButtonState();
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
            }
            else
            {
                EmailBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                EmailValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateRegistartionButtonState();
        }

        private bool IsValidEmail(string email)
        {
            string emailRegex = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";

            return System.Text.RegularExpressions.Regex.IsMatch(email, emailRegex);
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
                UpdateRegistartionButtonState();
            }

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
            }
            else
            {
                PasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                PasswordValidationMessage.Visibility = Visibility.Visible;
            }
            UpdateRegistartionButtonState();
        }

        private bool IsValidPassword(string password)
        {
            string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";

            return System.Text.RegularExpressions.Regex.IsMatch(password, passwordRegex);
        }

        private void RepeatPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (firstOpenRepeatPassword)
            {
                firstOpenRepeatPassword = false;
                RepeatPassword.PasswordChanged += RepeatPassword_PasswordChanged;
                string repeatPassword = RepeatPassword.Password;
                string password = Password.Password;
                isValidRepeatPassword = IsValidRepeatPassword(password, repeatPassword);

                if (isValidPassword && isValidRepeatPassword)
                {
                    RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                    RepeatPasswordBorder.BorderThickness = new Thickness(2);
                    RepeatPasswordValidationMessage.Visibility = Visibility.Collapsed;
                }
                else if (!isValidPassword)
                {
                    RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    RepeatPasswordValidationMessage.Visibility = Visibility.Visible;
                    RepeatPasswordValidationMessage.Text = "Lozinka nije validna";
                } 
                else
                {
                    RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                    RepeatPasswordValidationMessage.Visibility = Visibility.Visible;
                    RepeatPasswordValidationMessage.Text = "Lozinke se ne poklapaju";
                }
                UpdateRegistartionButtonState();
            }

        }

        private void RepeatPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string repeatPassword = RepeatPassword.Password;
            string password = Password.Password;
            isValidRepeatPassword = IsValidRepeatPassword(password, repeatPassword);

            if (isValidPassword && isValidRepeatPassword)
            {
                RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Green);
                RepeatPasswordBorder.BorderThickness = new Thickness(2);
                RepeatPasswordValidationMessage.Visibility = Visibility.Collapsed;
            }
            else if (!isValidPassword)
            {
                RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                RepeatPasswordValidationMessage.Visibility = Visibility.Visible;
                RepeatPasswordValidationMessage.Text = "Lozinka nije validna";
            }
            else
            {
                RepeatPasswordBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                RepeatPasswordValidationMessage.Visibility = Visibility.Visible;
                RepeatPasswordValidationMessage.Text = "Lozinke se ne poklapaju";
            }
            UpdateRegistartionButtonState();
        }

        private bool IsValidRepeatPassword(string password, string repeatPassword)
        {
            return isValidPassword && password == repeatPassword;
        }
    }
}
