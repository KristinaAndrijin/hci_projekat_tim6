using HCI_Projekat.Model;
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
        public MainWindow()
        {
            InitializeComponent();
            /*UserService.Register("djole", "1234", "Djordje", "Djordjevic", "User");
            User? u = UserService.Login("djole", "1234");
            if(u != null)
            {
                Console.WriteLine("pozdrav, " + u.Name);
            }*/

        }
    }
}
