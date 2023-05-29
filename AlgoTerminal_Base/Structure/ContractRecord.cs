using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal_Base.Structure
{
    public class ContractRecord
    {
        public record ContractData
        {
            public string? Expiry;
            public string? InstrumentType;
            public int lotSize;
            public string? trdSymbol;
            public double strike;
            public string? symbol;
            public int TokenID;
            public string? opttype;
            public int freezeQnty;
        }
    }
}
