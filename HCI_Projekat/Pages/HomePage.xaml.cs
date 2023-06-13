using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Data.Entity;


namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {

        public List<Trip> TripsList { get; set; }
        public HomePage()
        {
            using (var context = new Repository.AppContext())
            {
                TripsList = context.Trips
                    .Include(t => t.Attractions)
                    .Include(t => t.Accomodations)
                    .Include(t => t.Restaurants)
                    .Where(a => !a.IsDeleted)
                    .ToList();

            }
            InitializeComponent();

        }

        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var card = (sender as FrameworkElement)?.DataContext as Trip;
            if (card != null)
            {
                Debug.WriteLine($"Clicked on trip: {card.Name}, Price: {card.Price}");
            }
        }

    }
}
