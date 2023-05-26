using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.Model
{
    public class StrategyStraddleModel
    {
        //Total Leg and there decl..
        //Save to a Generic Location..
    }
    public record InstrumentSettings
    {
        public int Index = (int)EnumDeclaration.EnumIndex.Nifty;
        public int UnderlyingFrom = (int)EnumDeclaration.EnumUnderlyingFrom.Cash;
    }

    public record EntrySettings
    {
        public int StartgyType = (int)EnumDeclaration.EnumStrategyType.Intraday;
        public DateTime EntryTime = DateTime.Today.AddHours(9).AddMinutes(35); //Defult
        public DateTime ExitTime = DateTime.Today.AddHours(15).AddMinutes(15); //Defult
    }

    public record LegwiseSLsettings
    {
        public int SquareOff = (int)EnumDeclaration.EnumSquareOff.Partial;
        
        public bool IsTrailSLtoBreakEvenPriceEnable = false;
        public int TrailSLtoBreakEvenPrice = (int)EnumDeclaration.EnumTrailSLtoBreakEvenPrice.AllLegs;
    }

    public record OverallStrategySettings
    {
        public bool IsOverallStopLossEnable = false;
        public int SettingOverallStopLoss = (int)1;
        public double OverallStopLoss = 0;

        public bool IsOverallReEntryOnSLEnable = false;
        public int SettingOverallReEntryOnSL = (int)1;
        public double OverallReEntryOnSL = 0;

        public bool IsOverallTargetEnable = false;
        public int SettingOverallTarget = (int)1;
        public double OverallTarget = 0;

        public bool IsOverallReEntryOnTgtEnable = false;
        public int SettingOverallReEntryOnTgt = (int)1;
        public double OverallReEntryOnTgt = 0;

        public bool IsOverallTrallingOptionEnable = false;
        public bool IsOverallTrallSLEnable = false;
        public int SettingOverallTrallSL = (int)1;
        public int SettingTrallingOption = (int)1;
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
        public int SettingTargetProfit = (int)1;
        public double TargetProfit = 0;

        public bool IsStopLossEnable = false;
        public int SettingStopLoss = (int)1;
        public double StopLoss = 0;
        
        public bool IsTrailSlEnable = false;
        public int SettingTrailEnable = (int)1;
        public double TrailSlAmount = 0;
        public double TrailSlStopLoss = 0;

        public bool IsReEntryOnTgtEnable = false;
        public int SettingReEntryOnTgt = (int)1;
        public double ReEntryOnTgt = 0;

        public bool IsReEntryOnSLEnable = false;
        public int SettingReEntryOnSL = (int)1;
        public double ReEntryOnSL = 0;

        public bool IsSimpleMomentumEnable = false;
        public int SettingSimpleMomentum = (int)1;
        public double SimpleMomentum = 0;

    }

    public record LegBuilder : LegSetting
    {
        public int SelectSegment = (int)EnumDeclaration.EnumSegments.Options;
        public int Lots=1;
        public int Position = (int)EnumDeclaration.EnumPosition.Buy;
        public int OptionType = (int)EnumDeclaration.EnumOptiontype.Call;
        public int Expiry = (int)EnumDeclaration.EnumExpiry.Weekly;
        public int StrikeCriteria = (int)EnumDeclaration.EnumSelectStrikeCriteria.StrikeType;
        public int StrikeType = (int)EnumDeclaration.EnumStrikeType.ATM;
        public double PremiumRangeLower =0;
        public double PremiumRangeUpper=0;
        public double Premium = 0;

    }
}
