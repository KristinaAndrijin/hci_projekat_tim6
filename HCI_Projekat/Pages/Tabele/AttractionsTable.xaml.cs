using HCI_Projekat.Commands;
using HCI_Projekat.Forms;
using HCI_Projekat.Model;
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
    /// Interaction logic for AttractionsTable.xaml
    /// </summary>
    public partial class AttractionsTable : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Attraction> _selectedItems;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Attraction> SelectedItems
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
        public List<Attraction> Attractions { get; set; }
        public AttractionsTable()
        {
            InitializeComponent();
            DataContext = this;
            Repository.AppContext dbContext = new Repository.AppContext();
            Attractions = dbContext.Attractions!.Where(a => !a.IsDeleted).ToList();
            DeleteSelectedItemsCommand = new RelayCommand<IEnumerable<Attraction>>(DeleteSelectedItems, CanProcessSelectedItems);
            EditSelectedItemsCommand = new RelayCommand<IEnumerable<Attraction>>(EditSelectedItems, CanProcessSelectedItems);
        }

        private void DataGridAtrakcije_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItems = new ObservableCollection<Attraction>(DataGridAtrakcije.SelectedItems.Cast<Attraction>());
        }
        private bool CanProcessSelectedItems(IEnumerable<Attraction> selectedItems)
        {
            return selectedItems != null && selectedItems.Any();
        }
        private void DeleteSelectedItems(IEnumerable<Attraction> selectedItems)
        {
            var msgBox = new MessageBoxCustom("Da li sigurno zelite da obrisete to?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult!)
            {
                using (var context = new Repository.AppContext())
                {
                    DbSet<Attraction> itemSet = context.Attractions!;

                    foreach (Attraction selectedItem in selectedItems)
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
                    DataGridAtrakcije.ItemsSource = updatedItems;
                }
            }
        }

        private void EditSelectedItems(IEnumerable<Attraction> selectedItems)
        {
            foreach (Attraction selectedItem in selectedItems)
            {
                //TODO: Staviti odgovarajucu formu!!!
                //AccomodationForm accomodationForm = new AccomodationForm();
                //accomodationForm.DataContext = selectedItem;
                //accomodationForm.Show();
            }

            using (var context = new Repository.AppContext())
            {
                DataGridAtrakcije.ItemsSource = context.Attractions?.Where(a => !a.IsDeleted).ToList();
            }
        }

        private void DataGridAtrakcije_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                e.Handled = true; // Suppress the default delete behavior
            }
        }
        private void AddNewItem()
        {
            //TODO: Staviti odgovarajucu formu!!!
            //AccomodationForm accomodationForm = new AccomodationForm();
            //accomodationForm.ItemAdded += AccomodationForm_ItemAdded;
            //accomodationForm.Show();
        }
        private void AccomodationForm_ItemAdded(object sender, EventArgs e)
        {
            using (var context = new Repository.AppContext())
            {
                DataGridAtrakcije.ItemsSource = context.Attractions?.Where(a => !a.IsDeleted).ToList();
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

        private void NovoButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewItem();
        }
    }
}
