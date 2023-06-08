namespace AlgoTerminal_Base.Structure
{
    public class EnumDeclaration
    {
        public enum EnumStrategyStatus { Added, Stopped, Running, Waiting };
        public enum EnumLogType { Warning, Error, Success, Info, Buy, Sell, Response };
        public enum EnumEntryAndExit : int { TimeBased, SignalBased, OnlyEntrySignalBased }

        public enum EnumSignalType : int
        {
            //Sample
            NONE,
            RSI,
            EMA
        }
        public enum EnumIndex : int
        {
            NONE = 0,
            NIFTY = 1,
            BANKNIFTY = 2,
            FINNIFTY = 3
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
            ITM20 = -20,
            ITM19 = -19,
            ITM18 = -18,
            ITM17 = -17,
            ITM16 = -16,
            ITM15 = -15,
            ITM14 = -14,
            ITM13 = -13,
            ITM12 = -12,
            ITM11 = -11,
            ITM10 = -10,
            ITM9 = -9,
            ITM8 = -8,
            ITM7 = -7,
            ITM6 = -6,
            ITM5 = -5,
            ITM4 = -4,
            ITM3 = -3,
            ITM2 = -2,
            ITM1 = -1,
            ATM = 0,
            OTM1 = 1,
            OTM2 = 2,
            OTM3 = 3,
            OTM4 = 4,
            OTM5 = 5,
            OTM6 = 6,
            OTM7 = 7,
            OTM8 = 8,
            OTM9 = 9,
            OTM10 = 10,
            OTM11 = 11,
            OTM12 = 12,
            OTM13 = 13,
            OTM14 = 14,
            OTM15 = 15,
            OTM16 = 16,
            OTM17 = 17,
            OTM18 = 18,
            OTM19 = 19,
            OTM20 = 20
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
            Points,
            PointPercentage,
            UnderlyingPoints,
            UnderlyingPointPercentage
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
