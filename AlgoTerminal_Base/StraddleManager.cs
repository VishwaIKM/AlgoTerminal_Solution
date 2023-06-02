using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Structure;

namespace AlgoTerminal_Base
{
    public class StraddleManager : IStraddleManager
    {
        #region DataBase of Straddle

        public ConcurrentDictionary<string, StrategyDetails>? Master_Straddle_Dictionary { get; set; }

        //For LegDetails First Key is Stratgy send is Leg Key
        public ConcurrentDictionary<string, ConcurrentDictionary<string, LegDetails>>? Straddle_LegDetails_Dictionary { get; set; }

        #endregion

        public void CheckMovement()
        {

        }

    }
}
