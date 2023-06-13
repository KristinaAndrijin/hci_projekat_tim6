using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace HCI_Projekat.Help
{
    /// <summary>
    /// Interaction logic for HelpViewer.xaml
    /// </summary>
    public partial class HelpViewer : Window
    {
        /*JavaScriptControlHelper ch;*/
        public HelpViewer(string key, MainWindow originator)
        {
            InitializeComponent();
            string helpFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Help");
            string filePath = System.IO.Path.Combine(helpFolderPath, "index.html");
            Debug.WriteLine("AAAAAAAAAAAA "+ filePath);
            if (!File.Exists(filePath))
            {
                filePath = System.IO.Path.Combine(helpFolderPath, "error.html");
            }
            Uri u = new Uri(filePath);
            Debug.WriteLine("AAAAAAAAAAAAAAA" + u);
            //ch = new JavaScriptControlHelper(originator);
            //wbHelp.ObjectForScripting = ch;
            wbHelp.Navigate(u);

        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoBack));
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbHelp != null) && (wbHelp.CanGoForward));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoBack();
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbHelp.GoForward();
        }

        private void wbHelp_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {

        }
    }
}
