using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTerminal_Base.Structure;

namespace AlgoTerminal_Base
{
    public class StraddleManager : IStraddleManager
    {
        #region Members

        public ConcurrentDictionary<string, StraddleRecords>? Master_Straddle_Dictionary { get; set; }

        #endregion

        private void LoadAllAvailableStratgy()
        {

        }
    }
}
