using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTerminal.Model.Services;

namespace AlgoTerminal.Model.NNAPI
{
    public class ModeratorManagerModel : IModeratorManagerModel
    {
        #region Prop & var
        private int _orderId;
        private readonly object _locker = new();
        #endregion

        #region Methods
        public int GetOrderId()
        {
            lock (_locker)
            {
                return _orderId + 1;
            }
        }
        public void ResetOrderID(int orderId)
        {
            lock (_locker)
            {
                _orderId = orderId;
            }
        }
        #endregion
    }
}
