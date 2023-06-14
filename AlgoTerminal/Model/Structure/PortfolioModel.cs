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
        private double _reEntrySl;
        public double ReEntrySL
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
        private double _reEntryTP;

        public double ReEntryTP
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

        //For Calculation

        public double BuyAveragePrice { get; set; }
        public double SellAveragePrice { get; set; }
        public int BuyTradedQty { get; set; }
        public int SellTradedQty { get; set;}
        public double Expenses { get; set;}



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
        private string _tradingSymbol = "Loading...";
        public string TradingSymbol
        {
            get => _tradingSymbol; set
            {
                if (_tradingSymbol != value)
                {
                    _tradingSymbol = value;
                    OnPropertyChanged(nameof(TradingSymbol));
                }
            }
        }
        private EnumStrategyStatus _status = EnumStrategyStatus.None;
        public EnumStrategyStatus Status
        {
            get => _status; set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private double _entryPrice;
        public double EntryPrice
        {
            get => _entryPrice; set
            {
                if (_entryPrice != value)
                {
                    _entryPrice = value;
                    OnPropertyChanged(nameof(EntryPrice));
                }
            }
        }
        private double _exitPrice;
        public double ExitPrice
        {
            get => _exitPrice; set
            {
                if (_exitPrice != value)
                {
                    _exitPrice = value;
                    OnPropertyChanged(nameof(ExitPrice));
                }
            }
        }
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
        public double MTM
        {
            get => _mtm; set
            {
                if (_mtm != value)
                {
                    _mtm = value;
                    OnPropertyChanged(nameof(MTM));
                }
            }
        }
        private EnumPosition _buysell;
        public EnumPosition BuySell
        {
            get => _buysell; set
            {
                if (_buysell != value)
                {
                    _buysell = value;
                    OnPropertyChanged(nameof(BuySell));
                }
            }
        }
        private int _qty = 0;
        public int Qty
        {
            get => _qty; set
            {
                if (_qty != value)
                {
                    _qty = value;
                    OnPropertyChanged(nameof(Qty));
                }
            }
        }

        private double _reEntrySl;
        public double ReEntrySL
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
        private double _reEntryTP;

        public double ReEntryTP
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
        //Hidden INFO
        public uint Token { get; set; }
        //For Calculation
        public double Expenses { get; set; }
    }
}