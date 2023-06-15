using AlgoTerminal.Model.Structure;
using System;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IAlgoCalculation
    {
        double GetExpenses(uint Token, EnumDeclaration.EnumSegments enumSegments);
        DateTime GetLegExpiry(EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumOptiontype enumOptiontype);
        Task<double> GetLegMomentumlock(EnumDeclaration.EnumLegSimpleMomentum enumLegSimpleMomentum, double momentumPrice, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetLegStopLoss(EnumDeclaration.EnumLegSL enumLegSL, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double StopLoss, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetLegTargetProfit(EnumDeclaration.EnumLegTargetProfit enumLegTP, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double TargetProfit, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetMTM(double BuyAvg_Price, double SellAvg_Price, double Exp_, int BuyTrdqty, int SellTrdQty, uint Token);
        Task<double> GetRangeBreaKOut(EnumDeclaration.EnumRangeBreakout enumRangeBreakout, EnumDeclaration.EnumRangeBreakoutType enumRangeBreakoutType, DateTime RangeBreakOutEndTime, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetStrike(EnumDeclaration.EnumSelectStrikeCriteria _strike_criteria, EnumDeclaration.EnumStrikeType _strike_type, double _premium_lower_range, double _premium_upper_range, double _premium_or_StraddleValue, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumUnderlyingFrom enumUnderlyingFrom, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetStrikePriceLTP(uint Token);
        bool GetTrailSLHit(EnumDeclaration.EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount_Percentage, double ltp);
    }
}