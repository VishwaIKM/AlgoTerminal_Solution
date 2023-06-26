using AlgoTerminal.Model.Structure;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IStraddleManager
    {
        Task DataUpdateRequest();
        Task<bool> FirstTimeDataLoadingOnGUI();
        Task<bool> SquareOffStraddle920(PortfolioModel PM, EnumDeclaration.EnumStrategyStatus enumStrategyStatus = EnumDeclaration.EnumStrategyStatus.None);
        bool StraddleStartUP();
        Task<bool> StrategyReEntryStraddle920(string KeyOfStraddle);
    }
}