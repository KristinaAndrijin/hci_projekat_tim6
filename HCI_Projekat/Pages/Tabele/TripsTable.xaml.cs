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

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for TripsTable.xaml
    /// </summary>
    public partial class TripsTable : Page
    {
        public TripsTable()
        {
            InitializeComponent();
        }

        private void AddNew(object sender, RoutedEventArgs e)
        {
            //placeholder prozor, ubaci svoj kad napravis formu
            Window window = new Window();
            window.Show();
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            //placeholder prozor, ubaci svoj kad napravis formu
            Window window = new Window();
            window.Show();
        }

        private void DeleteButton(object sender, RoutedEventArgs e)
        {
            var msgBox = new MessageBoxCustom("Da li ste sigurni da zelite to da obrisete?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult)
            {
                new MessageBoxCustom("obrisato", MessageType.Info, MessageButtons.Ok).ShowDialog();
            }
            else
            {
                new MessageBoxCustom("nije obrisato", MessageType.Info, MessageButtons.Ok).ShowDialog();

            }
        }
    }
}
