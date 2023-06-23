using AlgoTerminal.Model.Structure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.ViewModel
{
    public sealed class OrderBookViewModel : DockWindowViewModel
    {
        public static ObservableCollection<OrderBookModel>? OpenOrderBook { get; set; }
        public static ObservableCollection<OrderBookModel>? CloseOrderBook { get; set; }


        public OrderBookViewModel()
        {
            OpenOrderBook ??= new();
            CloseOrderBook ??= new();
        }
    }
}
