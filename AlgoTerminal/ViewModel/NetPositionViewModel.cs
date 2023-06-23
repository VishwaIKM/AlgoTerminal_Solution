using AlgoTerminal.Model;
using AlgoTerminal.Model.Request;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AlgoTerminal.ViewModel
{
    public sealed class NetPositionViewModel : DockWindowViewModel
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public static ObservableCollection<NetPositionModel> NetPositionCollection { get; set; }
        private readonly IFeed feed;
        public NetPositionViewModel(IFeed feed)
        {
            NetPositionCollection ??= new();
            StartNetPositionUpdateTask();
            this.feed = feed;
        }

        private void StartNetPositionUpdateTask()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();
        }

        private async void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            #region Netpostion

            foreach (var item in OrderManagerModel.NetPosition_Dicc_By_Token) 
            {
                if (feed.FeedC != null)
                {
                    if (feed.FeedC.dcFeedData.TryGetValue((ulong)item.Key, out FeedC.ONLY_MBP_DATA_7208 oNLY_MBP_DATA_7208))
                    {
                        item.Value.LTP = Math.Round(Convert.ToDouble(oNLY_MBP_DATA_7208.LastTradedPrice) / 100.00, 2);
                        item.Value.MTM = Math.Round(item.Value.NetValue + item.Value.NetQuantity * item.Value.LTP, 2);
                    }
                }
            
            }
            await Task.Delay(101);
            #endregion
        }
    }
}
