using System.Collections.Concurrent;
using System.Threading;
using AlgoTerminal.Model.Structure;

namespace AlgoTerminal.Model
{
    public sealed class OrderManagerModel
    {
        #region Prop & var
        private static int _orderId;
        private static readonly object _locker = new();

        public static ConcurrentDictionary<int, OrderBookModel> OrderBook_Dicc_By_ClientID = new();
        public static ConcurrentDictionary<int, InnerObject> Portfolio_Dicc_By_ClientID = new(); //leg wise details
        #endregion

        #region Methods
        public static int GetOrderId()
        {
            lock (_locker)
            {
                return Interlocked.Increment(ref _orderId); ;
            }
        }
        public static void ResetOrderID(int orderId)
        {
            lock (_locker)
            {
                _orderId = orderId;
            }
        }
        #endregion
    }
}
