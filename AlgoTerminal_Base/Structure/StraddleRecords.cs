using System;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.Structure
{
    public class StraddleRecords
    {
        public record InstrumentSettings
        {
            public int Index = (int)EnumIndex.Nifty;
            public int UnderlyingFrom = (int)EnumUnderlyingFrom.Cash;
        }

        public record EntrySettings
        {
            public int EntryAndExitSetting = (int)EnumEntryAndExit.TimeBased;
            public int StartgyType = (int)EnumStrategyType.Intraday;

            //TimeBased ==> EntryAndExitSetting
            public DateTime EntryTime = DateTime.Today.AddHours(9).AddMinutes(35); //Defult

            //TimeBased ==> EntryAndExitSetting || OnlyEntrySignalBased==> Below Time Will Be exit based
            public DateTime ExitTime = DateTime.Today.AddHours(15).AddMinutes(15); //Defult

            //SignalBases ==> otherSignal
            public int Signal = (int)EnumSignalType.RSI;
        }

        public record LegwiseSLsettings
        {
            public int SquareOff = (int)EnumSquareOff.Partial;

            public bool IsTrailSLtoBreakEvenPriceEnable = false;
            public int TrailSLtoBreakEvenPrice = (int)EnumTrailSLtoBreakEvenPrice.AllLegs;
        }

        public record OverallStrategySettings
        {
            public bool IsOverallStopLossEnable = false;
            public int SettingOverallStopLoss = (int)EnumOverallStopLoss.MTM;
            public double OverallStopLoss = 0;

            public bool IsOverallReEntryOnSLEnable = false;
            public int SettingOverallReEntryOnSL = (int)EnumOverallReEntryOnSL.RE_ASAP;
            public double OverallReEntryOnSL = 0;

            public bool IsOverallTargetEnable = false;
            public int SettingOverallTarget = (int)EnumOverallTarget.MTM;
            public double OverallTarget = 0;

            public bool IsOverallReEntryOnTgtEnable = false;
            public int SettingOverallReEntryOnTgt = (int)EnumOverallReEntryOnTarget.RE_ASAP;
            public double OverallReEntryOnTgt = 0;

            public bool IsOverallTrallingOptionEnable = false;
            public bool IsOverallTrallSLEnable = false;
            public int SettingOverallTrallSL = (int)EnumOverallTrailingOption.Lock;
            public int SettingTrallingOption = (int)EnumOverallTrailingOptionTrailAndSLSelected.MTM;
            public double IfProfitReach = 0;
            public double LockProfit = 0;
            public double ForEveryIncreaseInProfitBy = 0;
            public double Trailprofitby = 0;
            public double TrailAmountMove = 0;
            public double TrailSLMove = 0;

        }

        public record LegSetting
        {
            //Leg Setting..
            public bool IsTargetProfitEnable = false;
            public int SettingTargetProfit = (int)EnumLegTargetProfit.Points;
            public double TargetProfit = 0;

            public bool IsStopLossEnable = false;
            public int SettingStopLoss = (int)EnumLegSL.Points;
            public double StopLoss = 0;

            public bool IsTrailSlEnable = false;
            public int SettingTrailEnable = (int)EnumLegTrailSL.Points;
            public double TrailSlAmount = 0;
            public double TrailSlStopLoss = 0;

            public bool IsReEntryOnTgtEnable = false;
            public int SettingReEntryOnTgt = (int)EnumLegReEntryOnTarget.RE_ASAP;
            public double ReEntryOnTgt = 0;

            public bool IsReEntryOnSLEnable = false;
            public int SettingReEntryOnSL = (int)EnumLegReEntryOnSL.RE_ASAP;
            public double ReEntryOnSL = 0;

            public bool IsSimpleMomentumEnable = false;
            public int SettingSimpleMomentum = (int)EnumLegSimpleMomentum.UP_Points;
            public double SimpleMomentum = 0;

            public bool IsRangeBreakOutEnable = false;
            public DateTime RangeBreakOutEndTime = DateTime.Today.AddHours(9).AddMinutes(45);
            public int SettingRangeBreakOut = (int)EnumRangeBreakout.High;
            public int SettingRangeBreakOutType = (int)EnumRangeBreakoutType.Underlying;

        }

        public record LegBuilder : LegSetting
        {
            public int SelectSegment = (int)EnumSegments.Options;
            public int Lots = 1;
            public int Position = (int)EnumPosition.Buy;
            public int OptionType = (int)EnumOptiontype.CE;
            public int Expiry = (int)EnumExpiry.Weekly;
            public int StrikeCriteria = (int)EnumSelectStrikeCriteria.StrikeType;
            public int StrikeType = (int)EnumStrikeType.ATM;
            public double PremiumRangeLower = 0;
            public double PremiumRangeUpper = 0;
            public double Premium = 0;

        }
    }
}
