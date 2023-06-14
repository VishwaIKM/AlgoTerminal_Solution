namespace AlgoTerminal.Model.Structure
{
    public class EnumDeclaration
    {
        public enum EnumStrategyStatus { Added, Stopped, Running, Waiting,Complete,Error,None};
        public enum EnumLogType { Warning, Error, Success, Info, Buy, Sell, Response };
        public enum EnumEntryAndExit : int { TIMEBASED, SignalBased, OnlyEntrySignalBased }

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
            CASH = 1,
            FUTURES = 2
        }
        //Entry settings
        public enum EnumStrategyType : int
        {
            INTRADAY = 1,
            BTST = 2,
            Positional = 3
        }
        //Legwise SL settings
        public enum EnumSquareOff : int
        {
            PARTIAL = 1,
            COMPLETE = 2
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
            
            FUTURES,
            OPTIONS
        }

        public enum EnumOptiontype : int
        {
            CE,
            PE,
            XX
        }

        public enum EnumExpiry : int
        {
            WEEKLY,
            NEXTWEEKLY,
            MONTHLY
        }

        public enum EnumSelectStrikeCriteria : int
        {
            STRIKETYPE,
            PREMIUMRANGE,
            CLOSESTPREMIUM,
            PREMIUMGREATEROREQUAL,
            PREMIUMLESSOREQUAL,
            STRADDLEWIDTH
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
            BUY,
            SELL 
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
            HIGH,
            LOW
        }
        public enum EnumRangeBreakoutType : int
        {
            UNDERLYING,
            INSTRUMENT
        }
    }
}
