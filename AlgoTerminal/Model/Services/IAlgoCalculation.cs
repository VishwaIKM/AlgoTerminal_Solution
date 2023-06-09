using AlgoTerminal.Model.Structure;
using System;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IAlgoCalculation
    {
        DateTime GetLegExpiry(EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumOptiontype enumOptiontype);
        double GetLegMomentumlock(EnumDeclaration.EnumLegSimpleMomentum enumLegSimpleMomentum, double momentumPrice, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumExpiry enumExpiry, double selectedStrike, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumSegments enumSegments);
        double GetLegStopLoss(EnumDeclaration.EnumLegSL enumLegSL, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double StopLoss);
        double GetLegTargetProfit(EnumDeclaration.EnumLegTargetProfit enumLegTP, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double TargetProfit);
        Task<double> GetRangeBreaKOut(EnumDeclaration.EnumRangeBreakout enumRangeBreakout, EnumDeclaration.EnumRangeBreakoutType enumRangeBreakoutType, DateTime RangeBreakOutEndTime, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumOptiontype enumOptiontype, double selectedStrike);
        double GetStrike(EnumDeclaration.EnumSelectStrikeCriteria _strike_criteria, EnumDeclaration.EnumStrikeType _strike_type, double _premium_lower_range, double _premium_upper_range, double _premium_or_StraddleValue, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumUnderlyingFrom enumUnderlyingFrom, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        bool GetTrailSLHit(EnumDeclaration.EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount_Percentage, double ltp);
    }
}