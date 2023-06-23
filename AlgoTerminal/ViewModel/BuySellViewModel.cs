using AlgoTerminal.Command;
using AlgoTerminal.View;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AlgoTerminal.ViewModel
{
    public sealed class BuySellViewModel:BaseViewModel
    {

        private RelayCommand2 exitCommand;
        private RelayCommand2 submitCommand;
        public ICommand ExitCommand => exitCommand ??= new RelayCommand2(Exit);
        public ICommand SubmitCommand => submitCommand ??= new RelayCommand2(Submit);

        private void Submit()
        {
        }
        private void Exit()
        {
            BuySellView buySellView = App.AppHost!.Services.GetRequiredService<BuySellView>();
            buySellView.Close();    
        }
    }
}
