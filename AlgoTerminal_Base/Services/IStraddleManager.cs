using AlgoTerminal_Base.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal_Base.Services
{
    public interface IStraddleManager
    {
        ConcurrentDictionary<string, StraddleRecords>? Master_Straddle_Dictionary { get; set; }
    }
}