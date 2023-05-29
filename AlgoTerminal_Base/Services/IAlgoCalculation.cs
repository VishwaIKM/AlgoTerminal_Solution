using System;
using System.Collections.Generic;
using AlgoTerminal_Base.Structure;

namespace AlgoTerminal_Base.Services
{
    public interface IAlgoCalculation
    {
        DateTime GetLegExpiry(EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumIndex enumIndex);
        double GetLegMomentumlock(EnumDeclaration.EnumLegSimpleMomentum enumLegSimpleMomentum, double MomentumPrice, double CurrentPriceForStrike);
        double GetLegStopLoss(EnumDeclaration.EnumLegSL enumLegSL, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetLegTargetProfit(EnumDeclaration.EnumLegTargetProfit enumLegTP, double entryPrice, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetRangeBreaKOut(EnumDeclaration.EnumRangeBreakout enumRangeBreakout, EnumDeclaration.EnumRangeBreakoutType enumRangeBreakoutType, List<double> DataSetOfPrice);
        int GetStrike(EnumDeclaration.EnumSelectStrikeCriteria _strike_criteria, EnumDeclaration.EnumStrikeType _strike_type, double _premium_lower_range, double _premium_upper_range, double _premium);
        double GetTrailSL(EnumDeclaration.EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount, double yStopLoss, double ltp, double CurrentStopLoss);
    }
}