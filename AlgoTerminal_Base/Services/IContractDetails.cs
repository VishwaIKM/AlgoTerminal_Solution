using AlgoTerminal_Base.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal_Base.Services
{
    public interface IContractDetails
    {
        ConcurrentDictionary<uint, ContractRecord.ContractData>? ContractDetailsToken { get; set; }

        ContractRecord.ContractData? GetContractDetailsByToken(uint Token);
        ContractRecord.ContractData? GetContractDetailsByTradingSymbol(string TradingSymbol);
        void LoadContractDetails();
    }
}