using HCI_Projekat.Pages.Tabele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_Projekat.Commands
{
    internal class DeleteRestaurantCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {

        }
        private void Delete()
        {

        }
    }
}
