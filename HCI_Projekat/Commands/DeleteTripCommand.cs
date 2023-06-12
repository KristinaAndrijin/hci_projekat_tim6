using HCI_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HCI_Projekat.Commands
{
    public class DeleteTripCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            /*var selectedRow = parameter as Trip;
            if (selectedRow != null)
            {
                
            }*/
            var msgBox = new MessageBoxCustom("Da li ste sigurni da zelite to da obrisete?", MessageType.Confirmation, MessageButtons.YesNo);
            msgBox.ShowDialog();
            if ((bool)msgBox.DialogResult)
            {
                new MessageBoxCustom("obrisato", MessageType.Info, MessageButtons.Ok).ShowDialog();
            }
            else
            {
                new MessageBoxCustom("nije obrisato", MessageType.Info, MessageButtons.Ok).ShowDialog();

            }
        }
    }
}
