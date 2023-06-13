using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HCI_Projekat.Service;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for UserTripsOverviewPage.xaml
    /// </summary>
    public partial class UserTripsOverviewPage : Page
    {
        private List<BoughtTrip> BoughtTrips;
        public UserTripsOverviewPage()
        {
            InitializeComponent();
            DataContext = this;
            using (var dbContext = new Repository.AppContext())
            {
                BoughtTrips = dbContext.BoughtTrips!
                    .Include(bt => bt.User)
                    .Include(bt => bt.Trip)
                    .Where(bt => bt.User.Id == UserService.CurrentlyLoggedIn.Id)
                    .ToList();
            }
            DataGridUser.ItemsSource = BoughtTrips;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var msgBox = new MessageBoxCustom("Da li Ste sigurni da zelite da otkazete ovo putovanje?",MessageType.Confirmation,MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult)
            {
                Button button = (Button)sender;

                // Get the corresponding data item for the clicked row
                BoughtTrip item = (BoughtTrip)button.DataContext;

                if (item != null)
                {
                    using (var dbContext = new Repository.AppContext())
                    {
                        var observedItem = dbContext.BoughtTrips.Find(item.Id);

                        if (observedItem != null)
                        {
                            dbContext.BoughtTrips.Remove(observedItem);
                            dbContext.SaveChanges();
                        }
                        BoughtTrips = dbContext.BoughtTrips!
                        .Include(bt => bt.User)
                        .Include(bt => bt.Trip)
                        .Where(bt => bt.User.Id == UserService.CurrentlyLoggedIn.Id)
                        .ToList();
                    }
                    DataGridUser.ItemsSource = BoughtTrips;
                }
            }
        }
    }
}
