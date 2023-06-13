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
using HCI_Projekat.Model;
using HCI_Projekat.Service;
using MaterialDesignThemes.Wpf;

namespace HCI_Projekat.Pages
{
    public partial class TripDetailsPage : Page
    {
        public Trip Trip { get; set; }
        private DateTime selectedDate;

        public TripDetailsPage(Trip trip)
        {
            this.Trip = trip;
            InitializeComponent();
            Debug.WriteLine($"Clicked on trip: {trip.Name}, Price: {trip.Price}");
            DataContext = this;
        }

        private void BtnKupi_Click(object sender, RoutedEventArgs e)
        {
            if (HasBoughtTrip(Trip))
            {
                ShowMessageBox("Ovo putovanje je već rezervisano za Vas.", MessageType.Warning, MessageButtons.Ok);
                return;
            }

            using (var dbContext = new Repository.AppContext())
            {
                BoughtTrip boughtTrip = new BoughtTrip(UserService.CurrentlyLoggedIn, Trip, DateTime.Now, Trip.Price);
                dbContext.BoughtTrips.Add(boughtTrip);
                dbContext.SaveChanges();
            }

            ShowMessageBox("Putovanje uspešno kupljeno", MessageType.Success, MessageButtons.Ok);
            CloseReservationDialog(sender, e);
        }

        private void BtnRezervisi_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDate == DateTime.MinValue || selectedDate <= DateTime.Now.Date)
            {
                ShowMessageBox("Uneseni datum je u prošlosti, molimo pokušajte ponovo.", MessageType.Warning, MessageButtons.Ok);
                return;
            }

            if (HasBoughtTrip(Trip))
            {
                ShowMessageBox("Ovo putovanje je već rezervisano za Vas.", MessageType.Warning, MessageButtons.Ok);
                return;
            }

            using (var dbContext = new Repository.AppContext())
            {
                BoughtTrip boughtTrip = new BoughtTrip(UserService.CurrentlyLoggedIn, Trip, selectedDate, Trip.Price);
                dbContext.BoughtTrips.Add(boughtTrip);
                dbContext.SaveChanges();
            }

            ShowMessageBox("Putovanje uspešno rezervisano", MessageType.Success, MessageButtons.Ok);
            CloseReservationDialog(sender, e);
        }

        private bool HasBoughtTrip(Trip trip)
        {
            using (var dbContext = new Repository.AppContext())
            {
                int userId = UserService.CurrentlyLoggedIn.Id; 
                int tripId = trip.Id; 
                return dbContext.BoughtTrips.Any(bt => bt.User.Id == userId && bt.Trip.Id == tripId);
            }
        }

        private void ShowMessageBox(string message, MessageType type, MessageButtons buttons)
        {
            MessageBoxCustom messageBox = new MessageBoxCustom(message, type, buttons);
            messageBox.Owner = Application.Current.MainWindow;
            messageBox.ShowDialog();
        }

        private void OpenReservationDialog(object sender, RoutedEventArgs e)
        {
            if (UserService.CurrentlyLoggedIn == null)
            {
                ShowMessageBox("Morate se prijaviti da biste rezervisali putovanje.", MessageType.Warning, MessageButtons.Ok);
                return;
            }


            ReservationDialog.Visibility = Visibility.Visible;
        }

        private void CloseReservationDialog(object sender, RoutedEventArgs e)
        {
            ReservationDialog.Visibility = Visibility.Collapsed;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            btnRezervisi.IsEnabled = selectedDate != DateTime.MinValue;
        }
    }
}
