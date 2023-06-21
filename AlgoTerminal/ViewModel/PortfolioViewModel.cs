using AlgoTerminal.Command;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using AvalonDock.Layout;
using System;
using System.Collections.ObjectModel;
using System.Net.Security;
using System.Windows;
using System.Windows.Input;

namespace AlgoTerminal.ViewModel
{
    public class PortfolioViewModel : DockWindowViewModel, IPortfolioViewModel
    {
        private ObservableCollection<PortfolioModel>? _strategyView;
        public ObservableCollection<PortfolioModel>? StrategyDataCollection { get => _strategyView; set { _strategyView = value; RaisePropertyChanged(nameof(StrategyDataCollection)); } }
        public PortfolioModel? SelectedItem {get;set;}

    public PortfolioViewModel()
        {
            StrategyDataCollection ??= new();
        }

        private RelayCommand2 stopCommand;
        public ICommand StopCommand => stopCommand ??= new RelayCommand2(Stop);

        private void Stop()
        {
            if (SelectedItem != null)
            {
                var result = MessageBox.Show("Do You Want to Stop the Strategy? " + SelectedItem.Name, "ALERT", MessageBoxButton.OKCancel);
                var result2 = SelectedItem;
            }
            else
                MessageBox.Show("Please Select One Stratrgy.");
        }
    }
}
