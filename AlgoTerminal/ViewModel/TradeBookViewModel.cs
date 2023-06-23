using AlgoTerminal.Model.Structure;
using System.Collections.ObjectModel;

namespace AlgoTerminal.ViewModel
{
    public sealed class TradeBookViewModel : DockWindowViewModel
    {
        public static ObservableCollection<TradeBookModel> TradeDataCollection { get; set; }
        public TradeBookViewModel()
        {
            TradeDataCollection ??= new();
        }
    }
}
