using HCI_Projekat.Commands;
using HCI_Projekat.Model;
﻿using HCI_Projekat.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for AccomodationTable.xaml
    /// </summary>
    public partial class AccomodationTable : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Accomodation> _selectedItems;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Accomodation> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ICommand DeleteSelectedItemsCommand { get; }
        public ICommand EditSelectedItemsCommand { get; }
        public List<Accomodation> AccomodationList { get; set; }
        public AccomodationTable()
        {
            InitializeComponent();
            DataContext = this;
            Repository.AppContext dbContext = new Repository.AppContext();
            AccomodationList = dbContext.Accomodations!.Where(a => !a.IsDeleted).ToList();
            DeleteSelectedItemsCommand = new RelayCommand<IEnumerable<Accomodation>>(DeleteSelectedItems, CanProcessSelectedItems);
            EditSelectedItemsCommand = new RelayCommand<IEnumerable<Accomodation>>(EditSelectedItems, CanProcessSelectedItems);
        }

        private void DataGridSmestaj_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new ObservableCollection<Accomodation>(DataGridSmestaj.SelectedItems.Cast<Accomodation>());

        }

        private bool CanProcessSelectedItems(IEnumerable<Accomodation> selectedItems)
        {
            return selectedItems != null && selectedItems.Any();
        }

        private void DeleteSelectedItems(IEnumerable<Accomodation> selectedItems) 
        {
            var msgBox = new MessageBoxCustom("Da li sigurno zelite da obrisete to?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult!)
            {
                using (var context = new Repository.AppContext())
                {
                    DbSet<Accomodation> itemSet = context.Accomodations!;

                    foreach (Accomodation selectedItem in selectedItems)
                    {
                        var trackedItem = itemSet.Find(selectedItem.Id);
                        if (trackedItem != null)
                        {
                            trackedItem.IsDeleted = true;
                        }
                    }

                    context.SaveChanges();

                    var updatedItems = itemSet.Where(a => !a.IsDeleted).ToList();

                    // Update the UI with the updated data
                    DataGridSmestaj.ItemsSource = updatedItems;
                }
            }
        }
        private void EditSelectedItems(IEnumerable<Accomodation> selectedItems)
        {
            foreach (Accomodation selectedItem in selectedItems)
            {
                AccomodationForm accomodationForm = new AccomodationForm();
                accomodationForm.DataContext = selectedItem;
                accomodationForm.ItemAdded += AccomodationForm_ItemAdded;
                accomodationForm.Show();
            }

            using (var context = new Repository.AppContext())
            {
                DataGridSmestaj.ItemsSource = context.Accomodations?.Where(a => !a.IsDeleted).ToList();
            }
        }

        private void DataGridSmestaj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true; // Suppress the default delete behavior
            }
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.E)
            {
                EditSelectedItemsCommand.Execute(SelectedItems);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N)
            {
                AddNewItem();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteSelectedItemsCommand.Execute(SelectedItems);
            }
        }
        private void AddNewItem()
        {
            AccomodationForm accomodationForm = new AccomodationForm();
            accomodationForm.ItemAdded += AccomodationForm_ItemAdded;
            accomodationForm.Show();
        }

        private void AccomodationForm_ItemAdded(object sender, EventArgs e)
        {
            using (var context = new Repository.AppContext())
            {
                DataGridSmestaj.ItemsSource = context.Accomodations?.Where(a => !a.IsDeleted).ToList();
            }
        }

        private void NovoButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }

        private void AddNew(object sender, RoutedEventArgs e)
        {
           //kris funkcija, ne koristi
        }

        private void EditButton(object sender, RoutedEventArgs e)
        {
            //kris funkcija, ne koristi
            //AccomodationForm accomodationForm = new AccomodationForm("tntntn");
            //accomodationForm.Show();
        }
    }
}
