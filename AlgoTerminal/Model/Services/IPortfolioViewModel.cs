using AlgoTerminal.Model.Structure;
using System.Collections.ObjectModel;

namespace AlgoTerminal.Model.Services
{
    public interface IPortfolioViewModel
    {
        ObservableCollection<PortfolioModel>? StrategyDataCollection { get; set; }
    }
}