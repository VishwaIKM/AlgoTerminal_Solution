using AlgoTerminal.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.Structure
{
    public class PortfolioModel:BaseViewModel
    {
        private string _name = "Loading...";
        public string Name {
            get => _name; set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public EnumIndex Index { get; set; }
        public string? UserID { get; set; }
        private DateTime _entryTime;
        public DateTime EntryTime
        {
            get => _entryTime; set
            {
                if (_entryTime != value)
                {
                    _entryTime = value;
                    OnPropertyChanged(nameof(EntryTime));
                }
            }
        }
        private DateTime _exitTime;
        public DateTime ExitTime
        {
            get => _exitTime; set
            {
                if (_exitTime != value)
                {
                    _exitTime = value;
                    OnPropertyChanged(nameof(ExitTime));
                }
            }
        }
        private double _mtm;
        public double MTM { get=> _mtm; set
            {
                if(_mtm != value)
                {
                    _mtm = value;
                    OnPropertyChanged(nameof(MTM));   
                }
            }
        }
        private double _stopLoss;
        public double StopLoss
        {
            get => _stopLoss; set
            {
                if (_stopLoss != value)
                {
                    _stopLoss = value;
                    OnPropertyChanged(nameof(StopLoss));
                }
            }
        }
        private double _targetprofit;

        public double TargetProfit
        {
            get => _targetprofit; set
            {
                if (_targetprofit != value)
                {
                    _targetprofit = value;
                    OnPropertyChanged(nameof(TargetProfit));
                }
            }
        }
        private int _reEntrySl;
        public int ReEntrySL
        {
            get => _reEntrySl; set
            {
                if (_reEntrySl != value)
                {
                    _reEntrySl = value;
                    OnPropertyChanged(nameof(ReEntrySL));
                }
            }
        }
        private int _reEntryTP;

        public int ReEntryTP
        {
            get => _reEntryTP; set
            {
                if (_reEntryTP != value)
                {
                    _reEntryTP = value;
                    OnPropertyChanged(nameof(ReEntryTP));
                }
            }
        }

        public ObservableCollection<InnerObject> innerObject { get; set; } = new ObservableCollection<InnerObject>();
    }
    public class InnerObject : BaseViewModel
    {
        private string _name = "Loading...";
        public string Name
        {
            get => _name; set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public string? TradingSymbol { get; set; }    
        public EnumStrategyStatus Status { get; set; }
        public double EntryPrice { get; set; }
        public double ExitPrice { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public double MTM { get; set; }
        public EnumPosition BuySell { get; set; }
        public int Qty { get; set; }

        //Hidden INFO
        public uint Token { get; set; }
    }
}