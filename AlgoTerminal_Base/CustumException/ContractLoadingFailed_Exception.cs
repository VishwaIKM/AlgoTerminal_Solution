using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal_Base.CustumException
{
    public class ContractLoadingFailed_Exception : Exception
    {
        public ContractLoadingFailed_Exception() { }

        public ContractLoadingFailed_Exception(string name)
       : base(String.Format("Contract Loading Failed: {0}", name))
        {

        }
    }
}
