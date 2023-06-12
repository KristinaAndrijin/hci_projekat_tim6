using HCI_Projekat.Commands;
using HCI_Projekat.Forms;
using HCI_Projekat.Model;
using HCI_Projekat.Repository;
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
using AppContext = HCI_Projekat.Repository.AppContext;

namespace HCI_Projekat.Pages.Tabele
{
    /// <summary>
    /// Interaction logic for RestaurantsTable.xaml
    /// </summary>
    public partial class RestaurantsTable : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Restaurant> _selectedItems;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Restaurant> SelectedItems
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
        public List<Restaurant> Restaurants { get; set; }
        public RestaurantsTable()
        {
            InitializeComponent();
            DataContext = this;
            AppContext dbContext = new AppContext();
            Restaurants = dbContext.Restaurants!.Where(a => !a.IsDeleted).ToList();
            DeleteSelectedItemsCommand = new RelayCommand<IEnumerable<Restaurant>>(ProcessSelectedItems, CanProcessSelectedItems);
            EditSelectedItemsCommand = new RelayCommand<IEnumerable<Restaurant>>(EditSelectedItems, CanProcessSelectedItems);

        }

        private void EditSelectedItems(IEnumerable<Restaurant> selectedItems)
        {
            foreach(Restaurant selectedItem in selectedItems)
            {
                RestaurantForm festaurantForm = new RestaurantForm(selectedItem);
                festaurantForm.Show();
            }

            using(var context = new AppContext())
            {
                DataGridRestorani.ItemsSource = context.Restaurants?.Where(a => !a.IsDeleted).ToList();
            }
        }

        private void ProcessSelectedItems(IEnumerable<Restaurant> selectedItems) //zapravo za brisanje, chatty me je zezno
        {
            var msgBox = new MessageBoxCustom("Da li sigurno zelite da obrisete to?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult!)
            {
                using (var context = new AppContext())
                {
                    DbSet<Restaurant> itemSet = context.Restaurants!;

                    foreach (Restaurant selectedItem in selectedItems)
                    {
                        var trackedItem = itemSet.Find(selectedItem.Id);
                        if (trackedItem != null)
                        {
                            trackedItem.IsDeleted = true;
                        }
                    }

                    context.SaveChanges();

                    var updatedItems = itemSet.ToList();

                    // Update the UI with the updated data
                    DataGridRestorani.ItemsSource = updatedItems;
                }
            }
        }

        private bool CanProcessSelectedItems(IEnumerable<Restaurant> selectedItems)
        {
            return selectedItems != null && selectedItems.Any();
        }

        private void YourDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new ObservableCollection<Restaurant>(DataGridRestorani.SelectedItems.Cast<Restaurant>());
        }

        private void DataGridRestorani_PreviewKeyDown(object sender, KeyEventArgs e)
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
            RestaurantForm festaurantForm = new RestaurantForm();
            festaurantForm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }
    }
}
