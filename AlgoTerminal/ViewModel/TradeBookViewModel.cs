using AlgoTerminal.Model.Structure;
using System.Collections.ObjectModel;

namespace AlgoTerminal.ViewModel
{
    public class TradeBookViewModel : DockWindowViewModel
    {
        public static ObservableCollection<TradeBookModel> TradeDataCollection { get; set; }
    }
}
