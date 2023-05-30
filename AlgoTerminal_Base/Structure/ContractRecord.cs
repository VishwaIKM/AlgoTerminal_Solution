using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.Structure
{
    public class ContractRecord
    {
        public record ContractData
        {
            public DateTime Expiry;
            public string? InstrumentType;
            public uint LotSize;
            public string? TrdSymbol;
            public double Strike;
            public string? Symbol;
            public uint TokenID;
            public EnumOptiontype Opttype;
            public int FreezeQnty;
        }
    }
}
