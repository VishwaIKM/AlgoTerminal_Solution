using AlgoTerminal.Model.Structure;
using System;
using System.Collections.Concurrent;

namespace AlgoTerminal.Model.Services
{
    public interface IContractDetails
    {
        ConcurrentDictionary<uint, ContractRecord.ContractData>? ContractDetailsToken { get; set; }

        ContractRecord.ContractData GetContractDetailsByToken(uint Token);
        ContractRecord.ContractData GetContractDetailsByTradingSymbol(string TradingSymbol);
        uint GetTokenByContractValue(DateTime exp, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumIndex enumIndex, double selectedStrike);
        uint GetTokenByContractValue(DateTime expiry, EnumDeclaration.EnumOptiontype xX, EnumDeclaration.EnumIndex index);
        void LoadContractDetails();
    }
}