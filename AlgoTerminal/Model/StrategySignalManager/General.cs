using AlgoTerminal.Model.Services;
using System.Collections.Generic;

namespace AlgoTerminal.Model.StrategySignalManager
{
    public class General : IGeneral
    {
        private List<string> TokenList = new List<string>();

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
