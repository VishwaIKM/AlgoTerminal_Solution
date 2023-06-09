using AlgoTerminal.Model.Structure;
using System;
using System.Collections.ObjectModel;

namespace AlgoTerminal.ViewModel
{
    public class PortfolioViewModel : DockWindowViewModel
    {

        public static ObservableCollection<PortfolioModel>? StrategyDataCollection { get; set; }
        public PortfolioViewModel()
        {
            StrategyDataCollection = new ObservableCollection<PortfolioModel>();

            //PortfolioModel portfolioModel = new()
            //{
            //    Name = "starddle920",
            //    Status = EnumDeclaration.EnumStrategyStatus.Stopped,
            //    EntryTime = DateTime.Now,
            //    ExitTime = DateTime.Now,
            //    M2M = 121,
            //    StopLoss = 121.12,
            //    TargetProfit = 170.23,
            //    ReEntrySL = true,
            //    ReEntryTP = false,
            //    UserID = "ULT001"

            //};
            //StrategyDataCollection.Add(portfolioModel);
            //PortfolioModel portfolioModel2 = new()
            //{
            //    Name = "starddle9202",
            //    Status = EnumDeclaration.EnumStrategyStatus.Stopped,
            //    EntryTime = DateTime.Now,
            //    ExitTime = DateTime.Now,
            //    M2M = 121,
            //    UserID = "ULT004",
            //    StopLoss = 121.12,
            //    TargetProfit = 170.23,
            //    ReEntrySL = false,
            //    ReEntryTP = false

            //};
            //StrategyDataCollection.Add(portfolioModel2);  
            //// RaisePropertyChanged(nameof(StrategyDataCollection));
        }

    }
}
