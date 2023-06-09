using System;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.Structure
{
    public class PortfolioModel
    {
        public string Name { get; set; }
        public string UserID { get; set; }
        public EnumStrategyStatus Status { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public double M2M { get; set; }
        public double StopLoss { get; set; }
        public double TargetProfit { get; set; }
        public bool ReEntrySL { get; set; }
        public bool ReEntryTP { get; set; }
    }
}