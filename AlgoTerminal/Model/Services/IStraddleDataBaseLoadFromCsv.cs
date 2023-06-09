using AlgoTerminal.Model.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal.Model.Services
{
    public interface IStraddleDataBaseLoadFromCsv
    {
        ConcurrentDictionary<string, StrategyDetails>? Master_Straddle_Dictionary { get; set; }
        ConcurrentDictionary<string, ConcurrentDictionary<string, LegDetails>>? Straddle_LegDetails_Dictionary { get; set; }

        bool LoadStaddleStratgy(string path);
    }
}