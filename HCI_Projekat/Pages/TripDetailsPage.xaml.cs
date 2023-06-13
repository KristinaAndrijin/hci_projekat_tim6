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

        public TripDetailsPage(Trip trip)
        {
            this.Trip = trip;
            InitializeComponent();
            Debug.WriteLine($"Clicked on trip: {trip.Name}, Price: {trip.Price}");
            DataContext = this;
        }
    }
}
