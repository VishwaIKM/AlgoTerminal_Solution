using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using System;
using System.Collections.ObjectModel;
using System.Net.Security;

namespace AlgoTerminal.ViewModel
{
    public class PortfolioViewModel : DockWindowViewModel, IPortfolioViewModel
    {
        private ObservableCollection<PortfolioModel>? _strategyView;
        public ObservableCollection<PortfolioModel>? StrategyDataCollection { get => _strategyView; set {   _strategyView = value; RaisePropertyChanged(nameof(StrategyDataCollection));  } }

        
        public PortfolioViewModel()
        {
            StrategyDataCollection ??= new();
        }
    }
}
