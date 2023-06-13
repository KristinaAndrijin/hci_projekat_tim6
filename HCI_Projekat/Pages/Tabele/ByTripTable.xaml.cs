using HCI_Projekat.Model;
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
using System.Data.Entity;
using System.Windows.Shapes;

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for ByTripTable.xaml
    /// </summary>
    public partial class ByTripTable : Page
    {
        private List<string> itemList;
        private string SelectedName;
        private List<BoughtTrip> BoughtTrips;

        public ByTripTable()
        {
            InitializeComponent();
            DataContext = this;
            using(var dbContext = new Repository.AppContext())
            {
                itemList = dbContext.Trips.Where(trip => !trip.IsDeleted).Select(trip => trip.Name).ToList();
                BoughtTrips = dbContext.BoughtTrips!
                    .Include(bt => bt.User)
                    .Include(bt => bt.Trip)
                    .ToList();
            }
            ComboBox.ItemsSource = itemList;
            DataGridByTrip.ItemsSource = BoughtTrips;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ComboBox.SelectedItem != null)
            {
                SelectedName = ComboBox.SelectedItem.ToString();
            }
            using(var dbContext = new Repository.AppContext())
            {
                BoughtTrips = dbContext.BoughtTrips
                    .Include(bt => bt.User)
                    .Include(bt => bt.Trip)
                    .Where(trip => trip.Trip.Name == SelectedName)
                    .ToList();
            }
            DataGridByTrip.ItemsSource = BoughtTrips;
        }
    }
}
