using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal_Base.Structure
{
    public class EnumDeclaration
    {
        public enum EnumEntryAndExit : int { TimeBased, SignalBased, OnlyEntrySignalBased }

        public enum EnumSignalType : int
        {
            //Sample
            RSI,
            EMA
        }
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
        public enum EnumOverallStopLoss : int
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

        public enum EnumOverallTrailingOption : int
        {
            Lock,
            LockAndTrail,
            OverallTrailAndSL
        }
        public enum EnumOverallTrailingOptionTrailAndSLSelected : int
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
            CE,
            PE,
            XX
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
            ITM2 = -2,
            ITM1 = -1,
            ATM = 0,
            OTM1 = 1,
            OTM2 = 2,
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

        public enum EnumLegReEntryOnTarget : int
        {
            RE_ASAP,
            RE_RevASAP,
            RE_MOMENTUM,
            RE_RevMOMENTUM,
            RE_COST,
            RE_RevCOST
        }
        public enum EnumLegReEntryOnSL : int
        {
            RE_ASAP,
            RE_RevASAP,
            RE_MOMENTUM,
            RE_RevMOMENTUM,
            RE_COST,
            RE_RevCOST
        }

        public enum EnumLegSimpleMomentum : int
        {
            UP_Points,
            DOWN_Points,
            UP_PointPercentage,
            DOWN_PointPercentage,
            UP_UnderlyingPoints,
            DOWN_UnderlyingPoints,
            UP_UnderlyingPointPercentage,
            DOWN_UnderlyingPointPercentage
        }

        public enum EnumRangeBreakout : int
        {
            High,
            Low
        }
        public enum EnumRangeBreakoutType : int
        {
            Underlying,
            Instrument
        }
    }
}
