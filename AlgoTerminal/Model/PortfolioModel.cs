﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal.Model
{
    public class PortfolioModel
    {
        public string Name { get; set; }
        public EnumStrategyStatus Status { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }  
        public double M2M { get; set; }
        public double StopLoss { get; set; }
        public double TargetProfit { get; set; }
        public bool ReEntrySL { get; set; }
        public bool ReEntryTP { get; set;}
    }
}