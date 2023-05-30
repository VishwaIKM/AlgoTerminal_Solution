using AlgoTerminal_Base.Structure;
using System;
using System.Collections.Generic;

namespace AlgoTerminal_Base.Services
{
    public interface IAlgoCalculation
    {
        DateTime GetLegExpiry(EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumOptiontype enumOptiontype);
        double GetLegMomentumlock(EnumDeclaration.EnumLegSimpleMomentum enumLegSimpleMomentum, double MomentumPrice, double CurrentPriceForStrike);
        double GetLegStopLoss(EnumDeclaration.EnumLegSL enumLegSL, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetLegTargetProfit(EnumDeclaration.EnumLegTargetProfit enumLegTP, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetRangeBreaKOut(EnumDeclaration.EnumRangeBreakout enumRangeBreakout, EnumDeclaration.EnumRangeBreakoutType enumRangeBreakoutType, List<double> DataSetOfPrice);
        double GetStrike(EnumDeclaration.EnumSelectStrikeCriteria _strike_criteria, EnumDeclaration.EnumStrikeType _strike_type, double _premium_lower_range, double _premium_upper_range, double _premium, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumUnderlyingFrom enumUnderlyingFrom, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumOptiontype enumOptiontype);
        double GetTrailSL(EnumDeclaration.EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount, double yStopLoss, double ltp, double CurrentStopLoss);
    }
}