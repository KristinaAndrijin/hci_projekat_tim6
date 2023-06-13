using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
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
using HCI_Projekat.Commands;
using HCI_Projekat.Forms;
using HCI_Projekat.Model;
using HCI_Projekat.Service;

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for TripsTable.xaml
    /// </summary>
    public partial class TripsTable : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Trip> _selectedItems;
        public ObservableCollection<Trip> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                _selectedItems = value;
                OnPropertyChanged(nameof(SelectedItems));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand DeleteSelectedItemsCommand { get; }
        public ICommand EditSelectedItemsCommand { get; }
        public List<Trip> TripsList { get; set; }
        public TripsTable()
        {   
            InitializeComponent();
            DataContext = this;
            Repository.AppContext dbContext = new Repository.AppContext();
            DeleteSelectedItemsCommand = new RelayCommand<IEnumerable<Trip>>(DeleteSelectedItems, CanProcessSelectedItems);
            EditSelectedItemsCommand = new RelayCommand<IEnumerable<Trip>>(EditSelectedItems, CanProcessSelectedItems);
            TripsList = dbContext.Trips
                .Include(a => a.Restaurants)
                .Include(a => a.Accomodations)
                .Include(a => a.Attractions)
                .Where(a => !a.IsDeleted)
                .ToList();
        }
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void EditSelectedItems(IEnumerable<Trip> selectedItems)
        {
            foreach (Trip selectedItem in selectedItems)
            {
                using(var context = new Repository.AppContext())
                {
                    Trip t = context.Trips.Find(selectedItem.Id);
                    TripForm form = new TripForm(t);
                    form.ItemAdded += Form_ItemAdded;
                    form.Show();
                }
                
            }

            using (var context = new Repository.AppContext())
            {
                DataGridPutovanja.ItemsSource = context.Trips?.Where(a => !a.IsDeleted).ToList();
            }
        }

        private void DeleteSelectedItems(IEnumerable<Trip> selectedItems)
        {
            var msgBox = new MessageBoxCustom("Da li sigurno zelite da obrisete to?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult!)
            {
                using (var context = new Repository.AppContext())
                {
                    DbSet<Trip> itemSet = context.Trips!;

                    foreach (Trip selectedItem in selectedItems)
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
                    DataGridPutovanja.ItemsSource = updatedItems;
                }
            }
        }

        private bool CanProcessSelectedItems(IEnumerable<Trip> selectedItems)
        {
            return selectedItems != null && selectedItems.Any();
        }


        private void AddNew(object sender, RoutedEventArgs e)
        {
            OpenNewForm();
        }
        private void Form_ItemAdded(object sender, EventArgs e)
        {
            using (var context = new Repository.AppContext())
            {
                TripsList = context.Trips
                .Include(a => a.Restaurants)
                .Include(a => a.Accomodations)
                .Include(a => a.Attractions)
                .Where(a => !a.IsDeleted)
                .ToList();
                DataGridPutovanja.ItemsSource = TripsList;
            }
        }
        private void EditButton(object sender, RoutedEventArgs e)
        {
            //placeholder prozor, ubaci svoj kad napravis formu
            //Window window = new Window();
            //window.Show();
        }

        private void OpenNewForm()
        {
            //placeholder prozor, ubaci svoj kad napravis formu
            TripForm form = new TripForm();
            form.ItemAdded += Form_ItemAdded;
            form.Show(); ;
        }

        private void Page_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.E)
            {
                EditSelectedItemsCommand.Execute(SelectedItems);
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.N)
            {
                OpenNewForm();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteSelectedItemsCommand.Execute(SelectedItems);
            }
        }

        private void DataGridPutovanja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new ObservableCollection<Trip>(DataGridPutovanja.SelectedItems.Cast<Trip>());
        }

        private void DataGridPutovanja_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true; // Suppress the default delete behavior
            }
        }
    }
}
