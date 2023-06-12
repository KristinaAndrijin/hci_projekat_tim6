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
using System.Windows.Shapes;

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for MonthlyTable.xaml
    /// </summary>
    public partial class MonthlyTable : Page
    {
        internal List<BoughtTrip> boughtTrips { get; set; }
        public MonthlyTable()
        {
            InitializeComponent();

        }
    }
}
