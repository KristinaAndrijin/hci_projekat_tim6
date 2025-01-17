﻿using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data.Entity;
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
        internal DateTime PickedDate { get; set; }
        internal List<BoughtTrip> BoughtTrips { get; set; }
        public string TestText { get; set; }

        public MonthlyTable()
        {
            InitializeComponent();
            PickedDate = DateTime.Now;
            DataContext = this;
            TestText = PickedDate.ToShortDateString();
            var thrityDaysAgo = PickedDate.AddDays(-30);
            using (var context = new Repository.AppContext())
            {
                BoughtTrips = context.BoughtTrips.
                    Include(bt => bt.User)
                    .Include(bt => bt.Trip)
                    .Where(a => (a.Date < PickedDate && a.Date > thrityDaysAgo))
                    .ToList();
                DataGridMonthly.ItemsSource = BoughtTrips;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            PickedDate = DatePicker.SelectedDate!.Value;
            using (var context = new Repository.AppContext())
            {
                var thrityDaysAgo = PickedDate.AddDays(-30);
                BoughtTrips = context.BoughtTrips
                    .Include(bt => bt.User)
                    .Include(bt => bt.Trip)
                    .Where(a => (a.Date < PickedDate && a.Date > thrityDaysAgo))
                    .ToList();
                DataGridMonthly.ItemsSource = BoughtTrips;
            }
        }
    }
}
