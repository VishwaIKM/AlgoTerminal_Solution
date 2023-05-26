using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.Model
{
    public class EnumDeclaration
    {
        //Instrument settings
        public enum EnumIndex : int
        {
            None = 0,
            Nifty = 1,
            BankNifty = 2,
            FinNifty = 3
        }

        public enum EnumUnderlyingFrom : int
        {
            Cash = 1,
            Futures = 2
        }
        //Entry settings
        public enum EnumStrategyType : int
        {
            Intraday = 1,
            BTST = 2,
            Positional = 3
        }
        //Legwise SL settings
        public enum EnumSquareOff : int
        {
            Partial = 1,
            Complete = 2
        }

        public enum EnumTrailSLtoBreakEvenPrice : int
        {
            AllLegs = 1,
            SlLegs = 2
        }

        //OVERALL STRG SETTING
        public enum EnumOverallStopLoss :int
        {
            MTM,
            TotalPremiumPercentage
        }

        public enum EnumOverallReEntryOnSL : int
        {
            RE_ASAP,
            RE_RevASAP,
            RE_MOMENTUM,
            RE_RevMOMENTUM

        }
        public enum EnumOverallTarget : int
        {
            MTM,
            TotalPremiumPercentage
        }
        public enum EnumOverallReEntryOnTarget : int
        {
            RE_ASAP,
            RE_RevASAP,
            RE_MOMENTUM,
            RE_RevMOMENTUM

        }

        public enum EnumTrailingOption : int
        {
            Lock,
            LockAndTrail,
            OverallTrailAndSL
        }
        public enum EnumTrailingOptionOverallTrailAndSLSelected : int
        {
            MTM,
            TotalPremiumPercentage
        }

        //Legs
        //Leg Builder
        public enum EnumSegments : int
        {
            Futures,
            Options
        }

        public enum EnumOptiontype : int
        {
            Call,
            Put
        }

        public enum EnumExpiry : int
        {
            Weekly,
            NextWeekly,
            Monthly
        }

        public enum EnumSelectStrikeCriteria : int
        {
            StrikeType,
            PremiumRange,
            ClosestPremium,
            PremiumGreaterOrEqual,
            PremiumLessOrEqual,
            StraddleWidth
        }

        public enum EnumStrikeType : int
        {
            ITM2,
            ITM1,
            ATM,
            OTM1,
            OTM2,
        }

        public enum EnumPosition : int
        {
            Buy,
            Sell
        }

        //Leg Setting 

        public enum EnumLegTargetProfit : int 
        {
            Points,
            Underling,
            PointPercentage,
            underlingPercentage
        }
        public enum EnumLegSL : int
        {
            Points,
            Underling,
            PointPercentage,
            underlingPercentage
        }
        public enum EnumLegTrailSL : int
        {
            Points,
            PointPercentage,
        }
    }
}
