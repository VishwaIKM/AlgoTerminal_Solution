using AlgoTerminal.Model.Structure;
using System;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IAlgoCalculation
    {
        double GetExpenses(uint Token, EnumDeclaration.EnumSegments enumSegments);
        InnerObject GetLegDetailsForRentry_SLHIT(LegDetails leg_Details, InnerObject OldLegDetails, StrategyDetails stg_setting_value);
        InnerObject GetLegDetailsForRentry_TPHIT(LegDetails leg_Details, InnerObject OldLegDetails, StrategyDetails stg_setting_value);
        DateTime GetLegExpiry(EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumOptiontype enumOptiontype);
        Task<double> GetLegMomentumlock(EnumDeclaration.EnumLegSimpleMomentum enumLegSimpleMomentum, double momentumPrice, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetLegStopLoss(EnumDeclaration.EnumLegSL enumLegSL, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double StopLoss, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumIndex enumIndex, uint Token, InnerObject legDetails);
        double GetLegTargetProfit(EnumDeclaration.EnumLegTargetProfit enumLegTP, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, double TargetProfit, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumIndex enumIndex, uint Token, InnerObject legDetails);
        double GetMTM(double BuyAvg_Price, double SellAvg_Price, double Exp_, int BuyTrdqty, int SellTrdQty, uint Token);
        double GetOverallStopLossValue(double TotalPremium, double TotalMTM, EnumDeclaration.EnumOverallStopLoss enumOverallStopLoss, double stopLossValue);
        double GetOverallTargetProfitValue(double TotalPremium, double TotalMTM, EnumDeclaration.EnumOverallTarget enumOverallTargetProfit, double TargetPofit);
        Task<double> GetRangeBreaKOut(EnumDeclaration.EnumRangeBreakout enumRangeBreakout, EnumDeclaration.EnumRangeBreakoutType enumRangeBreakoutType, DateTime RangeBreakOutEndTime, EnumDeclaration.EnumIndex enumIndex, uint Token);
        double GetStrike(EnumDeclaration.EnumSelectStrikeCriteria _strike_criteria, EnumDeclaration.EnumStrikeType _strike_type, double _premium_lower_range, double _premium_upper_range, double _premium_or_StraddleValue, EnumDeclaration.EnumIndex enumIndex, EnumDeclaration.EnumUnderlyingFrom enumUnderlyingFrom, EnumDeclaration.EnumSegments enumSegments, EnumDeclaration.EnumExpiry enumExpiry, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition);
        double GetStrikePriceLTP(uint Token);
        bool Get_if_SL_is_HIT(double CurrentStopLossValue, EnumDeclaration.EnumLegSL enumLegSL, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, EnumDeclaration.EnumIndex enumIndex, uint Token);
        bool Get_if_TP_is_HIT(double CurrentTargetProfitValue, EnumDeclaration.EnumLegSL enumLegSL, EnumDeclaration.EnumOptiontype enumOptiontype, EnumDeclaration.EnumPosition enumPosition, EnumDeclaration.EnumIndex enumIndex, uint Token);
        Task<bool> IsMyPriceHITforCost(bool sL_HIT, bool tP_HIT, double entryPrice, uint token);
        bool Is_overall_sl_hit(double TotalPremium, double PnL, double OverallStopLoss);
        bool Is_overall_tp_hit(double TotalPremium, double PnL, double OverallTargetProfit);
       
        void UpdateLegSLTrail_IF_HIT(InnerObject portfolio_leg_value, LegDetails leg_Details);
    }
}