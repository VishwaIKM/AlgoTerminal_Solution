using AlgoTerminal_Base.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal_Base.Services
{
    public interface IStraddleManager
    {
        ConcurrentDictionary<string, StrategyDetails>? Master_Straddle_Dictionary { get; set; }
        ConcurrentDictionary<string, double[]>? Option_Range_Breakout { get; set; }
        ConcurrentDictionary<string, ConcurrentDictionary<string, LegDetails>>? Straddle_LegDetails_Dictionary { get; set; }

        void CheckMovement();
    }
}