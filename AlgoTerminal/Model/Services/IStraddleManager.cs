using AlgoTerminal.Model.Structure;
using FeedC;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IStraddleManager
    {
        Task DataUpdateFrom_CM(uint FeedLogTime, string IndexName);
        Task DataUpdateFrom_FO(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed);
        Task DataUpdateRequest();
        Task<bool> FirstTimeDataLoadingOnGUI();
        Task<bool> SquareOffStraddle920(PortfolioModel PM);
        Task<bool> SquareOffStraddle920Leg(string KeyOfStraddle, string LegKey);
        Task<bool> StartStraddle920();
        bool StraddleStartUP();
        Task<bool> StrategyReEntryStraddle920(string KeyOfStraddle);
    }
}