using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal_Base.StrategySignalManager
{
    public class StraddleManager : IStraddleManager
    {
        #region DataBase of Straddle

        // For Strategy 
        public ConcurrentDictionary<string, StrategyDetails>? Master_Straddle_Dictionary { get; set; }

        //For LegDetails First Key is Stratgy send is Leg Key
        public ConcurrentDictionary<string, ConcurrentDictionary<string, LegDetails>>? Straddle_LegDetails_Dictionary { get; set; }

        // For ORB
        public ConcurrentDictionary<string, double[]>? Option_Range_Breakout { get; set; }
        #endregion

        public void CheckMovement()
        {

        }

    }
}
