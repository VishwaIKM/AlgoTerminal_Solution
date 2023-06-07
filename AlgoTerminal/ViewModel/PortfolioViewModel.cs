using AlgoTerminal.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlgoTerminal.ViewModel
{
    public class PortfolioViewModel:BaseViewModel
    {
        public ObservableCollection<PortfolioModel> StrategyDataCollection { get; set; }
        public PortfolioViewModel()
        {
            StrategyDataCollection = new ObservableCollection<PortfolioModel>();

            PortfolioModel portfolioModel = new()
            {
                Name = "starddle920",
                Status = AlgoTerminal_Base.Structure.EnumDeclaration.EnumStrategyStatus.Stopped,
                EntryTime = DateTime.Now,
                ExitTime = DateTime.Now,
                M2M = 121,
                StopLoss = 121.12,
                TargetProfit = 170.23,
                ReEntrySL = true,
                ReEntryTP = false

            };
            StrategyDataCollection.Add(portfolioModel);
            PortfolioModel portfolioModel2 = new()
            {
                Name = "starddle9202",
                Status = AlgoTerminal_Base.Structure.EnumDeclaration.EnumStrategyStatus.Stopped,
                EntryTime = DateTime.Now,
                ExitTime = DateTime.Now,
                M2M = 121,
                StopLoss = 121.12,
                TargetProfit = 170.23,
                ReEntrySL = false,
                ReEntryTP = false

            };
           
            StrategyDataCollection.Add(portfolioModel2);
           // RaisePropertyChanged(nameof(StrategyDataCollection));
        }

    }
}
