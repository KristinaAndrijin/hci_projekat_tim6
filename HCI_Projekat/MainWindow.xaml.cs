using HCI_Projekat.Model;
using HCI_Projekat.Pages;
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
            //UserService.Register("djole", "1234", "Djordje", "Djordjevic", "User");
            //User? u = UserService.Login("djole", "1234");
            //if (u != null)
            //{
            //    Console.WriteLine("pozdrav, " + u.Name);
            //}
            MainFrame.Content = new HomePage();
            // connectToMySql();
        }

        /*private void connectToMySql()
        {
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                using (MySqlConnection connection = new MySqlConnection(connStr))
                {
                    connection.Open();

                    // string createTableQuery = "CREATE TABLE IF NOT EXISTS Users (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(50), Surname VARCHAR(50), Email VARCHAR(50), Password VARCHAR(50))";
                    // using (MySqlCommand command = new MySqlCommand(createTableQuery, connection))
                    // {
                    //     command.ExecuteNonQuery();
                    // }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }*/

        private void BtnClickP1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new HomePage();
        }

        private void GoToLogin(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoginPage());
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
    }
}
