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
        public RegistrationPage()
        {
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
    }
}
