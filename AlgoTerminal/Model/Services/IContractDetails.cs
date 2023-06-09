using AlgoTerminal.Model.Structure;
using System.Collections.Concurrent;

namespace AlgoTerminal.Model.Services
{
    public interface IContractDetails
    {
        ConcurrentDictionary<uint, ContractRecord.ContractData>? ContractDetailsToken { get; set; }

        ContractRecord.ContractData? GetContractDetailsByToken(uint Token);
        ContractRecord.ContractData? GetContractDetailsByTradingSymbol(string TradingSymbol);
        void LoadContractDetails();
    }
}