using AlgoTerminal.ViewModel;

namespace AlgoTerminal.Model.Structure
{
    public sealed class NetPositionModel:BaseViewModel
    {
        public string TradingSymbol { get; set; }
        private int _buyQty;
        public int BuyQuantity { 
            get => _buyQty;
            set
            {
                if (value != _buyQty)
                {
                    _buyQty = value;
                    OnPropertyChanged(nameof(BuyQuantity));
                }
            }
        }

        private int _sellQty;
        public int SellQuantity
        {
            get => _sellQty;
            set
            {
                if (value != _sellQty)
                {
                    _sellQty = value;
                    OnPropertyChanged(nameof(SellQuantity));
                }
            }
        }

        private double _buyPrice;
        public double BuyAvgPrice
        {
            get => _buyPrice;
            set
            {
                if (value != _buyPrice)
                {
                    _buyPrice = value;
                    OnPropertyChanged(nameof(BuyAvgPrice));
                }
            }
        }
        private double _sellPrice;
        public double SellAvgPrice
        {
            get => _sellPrice;
            set
            {
                if (value != _sellPrice)
                {
                    _sellPrice = value;
                    OnPropertyChanged(nameof(SellAvgPrice));
                }
            }
        }

        private int _netQuantity;
        public int NetQuantity
        {
            get => _netQuantity;
            set
            {
                if (value != _netQuantity)
                {
                    _netQuantity = value;
                    OnPropertyChanged(nameof(NetQuantity));
                }
            }
        }
    }
}
