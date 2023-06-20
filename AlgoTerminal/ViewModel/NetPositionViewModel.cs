using AlgoTerminal.Model.Structure;
using System.Collections.ObjectModel;

namespace AlgoTerminal.ViewModel
{
    public class NetPositionViewModel : DockWindowViewModel
    {
        public static ObservableCollection<NetPositionModel> NetPositionCollection { get; set; }

        public NetPositionViewModel()
        {
            NetPositionCollection ??= new();
        }
    }
}
