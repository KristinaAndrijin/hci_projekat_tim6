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
using HCI_Projekat.Model;
using MaterialDesignThemes.Wpf;

using System.Diagnostics;
using System.Windows.Controls;

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


        private void OpenReservationDialog(object sender, RoutedEventArgs e)
        {
            ReservationDialog.Visibility = Visibility.Visible;
        }

        private void CloseReservationDialog(object sender, RoutedEventArgs e)
        {
            ReservationDialog.Visibility = Visibility.Collapsed;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePicker = (DatePicker)sender;
            selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            bool isFutureDate = selectedDate > DateTime.Now.Date;

            btnRezervisi.IsEnabled = isFutureDate;
        }

        private void BtnKupi_Click(object sender, RoutedEventArgs e)
        {
            PrintSelectedTime();
            CloseReservationDialog(sender, e);
            btnRezervisiOpenDialog.IsEnabled = false;
        }

        private void BtnRezervisi_Click(object sender, RoutedEventArgs e)
        {
            PrintSelectedTime();
            CloseReservationDialog(sender, e);
            btnRezervisiOpenDialog.IsEnabled = false;
        }

        private void PrintSelectedTime()
        {
            Debug.WriteLine($"Selected time: {selectedDate}");
        }
    }
}