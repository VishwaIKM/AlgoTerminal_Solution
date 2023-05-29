using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgoTerminal_Base.Structure;

namespace AlgoTerminal_Base
{
    public class StraddleManager
    {
        #region Members

        public static ConcurrentDictionary<string, StraddleRecords> Master_Dictionary = new();

        #endregion

        public StraddleManager()
        {
            Master_Dictionary.Clear();
            LoadAllAvailableStratgy();
        }

        private void LoadAllAvailableStratgy()
        {
            StraddleRecords straddleRecords = new ();
            Master_Dictionary.TryAdd("Hello", straddleRecords);
        }
    }
}
