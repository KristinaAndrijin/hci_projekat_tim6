using HCI_Projekat.Forms;
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
    /// Interaction logic for AccomodationTable.xaml
    /// </summary>
    public partial class AccomodationTable : Page
    {
        public AccomodationTable()
        {
            InitializeComponent();
        }

        private void AddNew(object sender, RoutedEventArgs e)
        {
            AccomodationForm accomodationForm = new AccomodationForm();
            accomodationForm.Show();
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            //AccomodationForm accomodationForm = new AccomodationForm("tntntn");
            //accomodationForm.Show();
        }
    }
}
