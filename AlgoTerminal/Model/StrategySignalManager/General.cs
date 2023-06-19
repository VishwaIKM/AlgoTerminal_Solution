using AlgoTerminal.Model.Request;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AlgoTerminal.Model.StrategySignalManager
{
    public class General : IGeneral
    {
      
        private List<string> TokenList = new List<string>();
        //MAIN portfolio dic
        public ConcurrentDictionary<string, PortfolioModel> Portfolios { get; set; } // key()=> stg {"name"}

        //Support 
        public ConcurrentDictionary<uint, List<InnerObject>> PortfolioLegByTokens { get; set; } //KEY()=>uint 

        public void AddToken(string token)
        {
            TokenList.Add(token);
        }
        public void RemoveToken(string token)
        {
            TokenList.Remove(token);
        }

        public bool IsTokenFound(string token)
        {
            return TokenList.Contains(token);
        }

    }
}
