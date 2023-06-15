using AlgoTerminal.Model.Structure;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AlgoTerminal.Model.Services
{
    public interface IGeneral
    {
        ConcurrentDictionary<uint, List<InnerObject>> PortfolioLegByTokens { get; set; }
        ConcurrentDictionary<string, PortfolioModel> Portfolios { get; set; }

        void AddToken(string token);
        bool IsTokenFound(string token);
        void RemoveToken(string token);
    }
}