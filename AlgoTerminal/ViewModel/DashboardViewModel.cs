using System.Collections.Generic;

namespace AlgoTerminal.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Members and Prop..

        public DockManagerViewModel DockManagerViewModel { get; private set; }
        public MenuViewModel MenuViewModel { get; private set; }

        private readonly PortfolioViewModel _portfolioViewModel;
        private readonly NetPositionViewModel _netPositionViewModel;
        private readonly LoggerViewModel _loggerViewModel;
        private readonly TradeBookViewModel _tradeBookViewModel;

        public DashboardViewModel(PortfolioViewModel portfolioViewModel, NetPositionViewModel netPositionViewModel, LoggerViewModel loggerViewModel, TradeBookViewModel tradeBookViewModel)
        {
            _netPositionViewModel = netPositionViewModel;
            _loggerViewModel = loggerViewModel;
            _tradeBookViewModel = tradeBookViewModel;
            _portfolioViewModel = portfolioViewModel;
            var documents = new List<DockWindowViewModel>();
            _portfolioViewModel.Title = "Portfolio";
            _tradeBookViewModel.Title = "Trade Book";
            _loggerViewModel.Title = "Log Record";
            _netPositionViewModel.Title = "Net Position";
            documents.Add(_portfolioViewModel);
            documents.Add(_tradeBookViewModel);
            documents.Add(_loggerViewModel);
            documents.Add(_netPositionViewModel);
            this.DockManagerViewModel = new DockManagerViewModel(documents);
            this.MenuViewModel = new MenuViewModel(documents);
        }

        //SERVER
        private string _connected;
        public string Connected  
        {   get=>_connected; 
            set 
            {
                if (_connected != value)
                {
                    _connected = value;
                    OnPropertyChanged(nameof(Connected));
                }
            } 
        }
        //SPOT AND FUTURE DATA 
        private string _nifty50;
        public string Nifty50 { get => _nifty50;
            set
            {
                if (_nifty50 != value)
                {
                    _nifty50 = value;
                    OnPropertyChanged(nameof(Nifty50));
                }
               
            }
        }

        private string _niftyfut;
        public string NiftyFut
        {
            get => _niftyfut;
            set
            {
                if (_niftyfut != value)
                {
                    _niftyfut = value;
                    OnPropertyChanged(nameof(NiftyFut));
                }

            }
        }

        private string _banknifty;
        public string BankNifty
        {
            get => _banknifty;
            set
            {
                if (_banknifty != value)
                {
                    _banknifty = value;
                    OnPropertyChanged(nameof(BankNifty));
                }

            }
        }
        private string _bankniftyfut;
        public string BankNiftyFut
        {
            get => _bankniftyfut;
            set
            {
                if (_bankniftyfut != value)
                {
                    _bankniftyfut = value;
                    OnPropertyChanged(nameof(BankNiftyFut));
                }

            }
        }

        private string _finnifty;
        public string FinNifty
        {
            get => _finnifty;
            set
            {
                if (_finnifty != value)
                {
                    _finnifty = value;
                    OnPropertyChanged(nameof(FinNifty));
                }

            }
        }
        private string _finniftyfut;
        public string FinNiftyFut
        {
            get => _finniftyfut;
            set
            {
                if (_finniftyfut != value)
                {
                    _finniftyfut = value;
                    OnPropertyChanged(nameof(FinNiftyFut));
                }

            }
        }

        #endregion
    }
}
