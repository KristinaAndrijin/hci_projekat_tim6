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
using HCI_Projekat.Commands;
using HCI_Projekat.Forms;
using HCI_Projekat.Model;
using HCI_Projekat.Service;

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for TripsTable.xaml
    /// </summary>
    public partial class TripsTable : Page
    {
        public List<Trip> TripsList { get; set; }
        public TripsTable()
        {   
            InitializeComponent();
            DataContext = this;
            Repository.AppContext dbContext = new Repository.AppContext();
            TripsList = dbContext.Trips.ToList();
        }

        private void AddNew(object sender, RoutedEventArgs e)
        {
            //placeholder prozor, ubaci svoj kad napravis formu
            TripForm tripForm = new TripForm();
            //festaurantForm.ItemAdded += Form_ItemAdded;
            tripForm.Show(); ;
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            Trip trip = TripService.GetAllAccomodations().FirstOrDefault(); 
            TripForm tripForm = new TripForm(trip);
            tripForm.Show(); ;
        }
    }
}
