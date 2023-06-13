using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace HCI_Projekat.Forms
{
    /// <summary>
    /// Interaction logic for TripForm.xaml
    /// </summary>
    public partial class TripForm : Window
    {
        private ObservableCollection<FormStep> formSteps;
        public TripForm()
        {
            InitializeComponent();
            PreviousButton.IsEnabled = false;
        }



        private void Next_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = tabControl.SelectedIndex;
            if (currentIndex < tabControl.Items.Count - 1)
            {
                tabControl.SelectedIndex = currentIndex + 1;
                UpdateButtonState();
            }
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = tabControl.SelectedIndex;
            if (currentIndex > 0)
            {
                tabControl.SelectedIndex = currentIndex - 1;
                UpdateButtonState();
            }
        }

        private void UpdateButtonState()
        {
            int currentIndex = tabControl.SelectedIndex;

            // Enable or disable the Previous button based on the current index
            PreviousButton.IsEnabled = currentIndex > 0;

            // Enable or disable the Next button based on the current index
            NextButton.IsEnabled = currentIndex < tabControl.Items.Count - 1;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

    public class FormStep
    {
        public string StepName { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        // Add more fields as needed
    }
}
