using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal.Model
{
    public class LoggerModel
    {
        public DateTime Time { get; set; }
        public EnumLogType Category { get; set; }  // Type of log
        public string Message { get; set; }
    }
}
