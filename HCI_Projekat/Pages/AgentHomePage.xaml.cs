using HCI_Projekat.Pages.Tabele;
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

namespace HCI_Projekat.Pages
{
    /// <summary>
    /// Interaction logic for AgentHomePage.xaml
    /// </summary>
    public partial class AgentHomePage : Page
    {
        public AgentHomePage()
        {
            InitializeComponent();
            AgentFrame.Navigate(new TripsTable());
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            string selectedText = selectedItem.Content.ToString();

            if(selectedText == "Putovanja")
            {
                if(AgentFrame != null)
                {
                    var table = new TripsTable();
                    AgentFrame.Navigate(table);
                    table.Focus();

                }
            }
            else if(selectedText == "Atrakcije")
            {
                if (AgentFrame != null)
                {
                    var table = new AttractionsTable();
                    AgentFrame.Navigate(table);
                    table.Focus();


                }
            }
            else if (selectedText == "Restorani")
            {
                if (AgentFrame != null)
                {
                    var table = new RestaurantsTable();
                    AgentFrame.Navigate(table);
                    table.Focus();
                    
                }
            }
            else if (selectedText == "Smestaj")
            {
                if (AgentFrame != null)
                {
                    var table = new AccomodationTable();
                    AgentFrame.Navigate(table);
                    table.Focus();

                }
            }
            else if (selectedText == "Pregled za mesec")
            {
                if (AgentFrame != null)
                {
                    var table = new MonthlyTable();
                    AgentFrame.Navigate(table);
                    table.Focus();

                }
            }
        }
    }
}
