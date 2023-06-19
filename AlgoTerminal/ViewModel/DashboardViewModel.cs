using AlgoTerminal.Model.Structure;
using System.Collections.Generic;

namespace AlgoTerminal.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        #region Members and Prop..

        public DockManagerViewModel DockManagerViewModel { get; private set; }
        public IDashboardModel DashboardModel { get; private set; }
        public MenuViewModel MenuViewModel { get; private set; }

        private readonly PortfolioViewModel _portfolioViewModel;
        private readonly NetPositionViewModel _netPositionViewModel;
        private readonly LoggerViewModel _loggerViewModel;
        private readonly TradeBookViewModel _tradeBookViewModel;
        private readonly OrderBookViewModel _orderBookViewModel;

        public DashboardViewModel(OrderBookViewModel orderBookViewModel, PortfolioViewModel portfolioViewModel, NetPositionViewModel netPositionViewModel, LoggerViewModel loggerViewModel, TradeBookViewModel tradeBookViewModel ,IDashboardModel dashboardModel)
        {
            _netPositionViewModel = netPositionViewModel;
            _loggerViewModel = loggerViewModel;
            _tradeBookViewModel = tradeBookViewModel;
            _portfolioViewModel = portfolioViewModel;
            _orderBookViewModel = orderBookViewModel;
            var documents = new List<DockWindowViewModel>();
            _portfolioViewModel.Title = "Portfolio";
            _tradeBookViewModel.Title = "Trade Book";
            _loggerViewModel.Title = "Log Record";
            _netPositionViewModel.Title = "Net Position";
            _orderBookViewModel.Title = "Order Book";
            documents.Add(_portfolioViewModel);
            documents.Add(_tradeBookViewModel);
            documents.Add(_loggerViewModel);
            documents.Add(_netPositionViewModel);
            documents.Add(_orderBookViewModel);
            this.DockManagerViewModel = new DockManagerViewModel(documents);
            this.MenuViewModel = new MenuViewModel(documents);
            this.DashboardModel = dashboardModel;
        }


        #endregion
    }
}
