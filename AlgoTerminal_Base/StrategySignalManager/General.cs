using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTerminal_Base.Services;

namespace AlgoTerminal_Base.StrategySignalManager
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
