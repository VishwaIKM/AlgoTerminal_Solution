using System;
using System.Collections.Generic;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Structure;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.Calculation
{
    public class AlgoCalculation : IAlgoCalculation
    {
        #region Strike Setting Only ==> As per EnumDeclaration.EnumSelectStrikeCriteria

        private readonly object SelectStrikeSettingLock = new();
        public int GetStrike(EnumSelectStrikeCriteria _strike_criteria,
            EnumStrikeType _strike_type,
            double _premium_lower_range, double _premium_upper_range, double _premium)
        {
            lock (SelectStrikeSettingLock)
            {
                return _strike_criteria switch
                {
                    EnumSelectStrikeCriteria.StrikeType => GetStrikeType(_strike_type),
                    EnumSelectStrikeCriteria.PremiumRange => GetPremiumRange(_premium_lower_range, _premium_upper_range),
                    EnumSelectStrikeCriteria.ClosestPremium => GetClosestPremium(_premium),
                    EnumSelectStrikeCriteria.PremiumGreaterOrEqual => GetPremiumGreaterOrEqual(_premium),
                    EnumSelectStrikeCriteria.PremiumLessOrEqual => GetPremiumLessOrEqual(_premium),
                    EnumSelectStrikeCriteria.StraddleWidth => GetStraddleWidth(_premium),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private int GetStraddleWidth(double premium)
        {
            throw new NotImplementedException();
        }

        private int GetPremiumLessOrEqual(double premium)
        {
            throw new NotImplementedException();
        }

        private int GetPremiumGreaterOrEqual(double premium)
        {
            throw new NotImplementedException();
        }

        private int GetClosestPremium(double premium)
        {
            throw new NotImplementedException();
        }

        private int GetPremiumRange(double premium_lower_range, double premium_upper_range)
        {
            throw new NotImplementedException();
        }

        private int GetStrikeType(EnumStrikeType strike_type)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Get StopLoss for Leg

        private readonly object _sllock = new object();
        public double GetLegStopLoss(EnumLegSL enumLegSL,
            double entryPrice,
            EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            lock (_sllock)
            {
                return enumLegSL switch
                {
                    EnumLegSL.Points => GetLegPointSL(entryPrice, enumOptiontype, enumPosition),
                    EnumLegSL.PointPercentage => GetLegPointPercentageSL(entryPrice, enumOptiontype, enumPosition),
                    EnumLegSL.Underling => GetLegUnderling(entryPrice, enumOptiontype, enumPosition),
                    EnumLegSL.underlingPercentage => GetLegunderlingPercentage(entryPrice, enumOptiontype, enumPosition),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double GetLegunderlingPercentage(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegUnderling(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegPointPercentageSL(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegPointSL(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Get Target Profit for Leg

        private readonly object _trlock = new object();
        public double GetLegTargetProfit(EnumLegTargetProfit enumLegTP,
            double entryPrice,
            EnumOptiontype enumOptiontype,
            EnumPosition enumPosition)
        {
            lock (_trlock)
            {
                return enumLegTP switch
                {
                    EnumLegTargetProfit.Points => GetLegPointTP(entryPrice, enumOptiontype, enumPosition),
                    EnumLegTargetProfit.PointPercentage => GetLegPointPercentageTP(entryPrice, enumOptiontype, enumPosition),
                    EnumLegTargetProfit.Underling => GetLegUnderlingTP(entryPrice, enumOptiontype, enumPosition),
                    EnumLegTargetProfit.underlingPercentage => GetLegunderlingPercentageTP(entryPrice, enumOptiontype, enumPosition),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double GetLegunderlingPercentageTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegUnderlingTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegPointPercentageTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        private double GetLegPointTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Get Taril SL Value
        private readonly object _traillock = new();
        /// <summary>
        /// Provide the New StopLoss When SetAmount Move in Favour OtherWise Return 0;
        /// </summary>
        /// <param name="enumLegTrailSL"></param>
        /// <param name="entryPrice"></param>
        /// <param name="xAmount"></param>
        /// <param name="yStopLoss"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetTrailSL(EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount,
            double yStopLoss, double ltp,
            double CurrentStopLoss)
        {
            lock (_traillock)
            {
                return enumLegTrailSL switch
                {
                    EnumLegTrailSL.Points => GetLegPointTailSL(entryPrice, xAmount, yStopLoss, ltp),
                    EnumLegTrailSL.PointPercentage => GetLegPointPercentageTrailSL(entryPrice, xAmount, yStopLoss, ltp),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double GetLegPointPercentageTrailSL(double entryPrice, double xAmount, double yStopLoss, double ltp)
        {
            if (entryPrice + entryPrice * xAmount / 100 >= ltp)
            {

            }
            throw new NotImplementedException();
        }

        private double GetLegPointTailSL(double entryPrice, double xAmount, double yStopLoss, double ltp)
        {
            if (entryPrice + xAmount >= ltp)
            {

            }
            throw new NotImplementedException();
        }

        #endregion

        #region Get Expiry
        private readonly object _expiry = new();

        public DateTime GetLegExpiry(EnumExpiry enumExpiry, EnumIndex enumIndex)
        {
            lock (_expiry)
            {
                if (enumExpiry == EnumExpiry.Weekly)
                {

                }
                else if (enumExpiry == EnumExpiry.Monthly)
                {

                }
                else
                {

                }
                return DateTime.MinValue;
            }
        }

        #endregion

        #region Get Simple Momentum
        private readonly object _mvlock = new();
        public double GetLegMomentumlock(EnumLegSimpleMomentum enumLegSimpleMomentum,
            double MomentumPrice,
            double CurrentPriceForStrike)
        {
            lock (_mvlock)
            {
                return enumLegSimpleMomentum switch
                {
                    EnumLegSimpleMomentum.UP_Points => GetLegSimpleMomentumUP_Points(MomentumPrice, CurrentPriceForStrike),
                    EnumLegSimpleMomentum.DOWN_Points => GetLegSimpleMomentumDOWN_Points(MomentumPrice, CurrentPriceForStrike),
                    EnumLegSimpleMomentum.UP_PointPercentage => GetLegSimpleMomentumUP_PointPercentage(MomentumPrice, CurrentPriceForStrike),
                    EnumLegSimpleMomentum.DOWN_PointPercentage => GetLegSimpleMomentumDOWN_PointPercentage(MomentumPrice, CurrentPriceForStrike),
                    EnumLegSimpleMomentum.UP_UnderlyingPoints => GetLegSimpleMomentumUP_UnderlyingPoints(MomentumPrice, CurrentPriceForStrike),
                    EnumLegSimpleMomentum.DOWN_UnderlyingPointPercentage => GetLegSimpleDOWN_UnderlyingPointPercentage(MomentumPrice, CurrentPriceForStrike),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double GetLegSimpleDOWN_UnderlyingPointPercentage(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        private double GetLegSimpleMomentumUP_UnderlyingPoints(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        private double GetLegSimpleMomentumDOWN_PointPercentage(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        private double GetLegSimpleMomentumUP_PointPercentage(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        private double GetLegSimpleMomentumDOWN_Points(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        private double GetLegSimpleMomentumUP_Points(double momentumPrice, double currentPriceForStrike)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Get RangeBreakOut

        private readonly object _RBOlock = new();

        /// <summary>
        /// Range Required for find High and Low price for strike
        /// </summary>
        /// <param name="enumRangeBreakout"></param>
        /// <param name="enumRangeBreakoutType"></param>
        /// <param name="DataSetOfPrice"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public double GetRangeBreaKOut(EnumRangeBreakout enumRangeBreakout, EnumRangeBreakoutType enumRangeBreakoutType, List<double> DataSetOfPrice)
        {
            lock (_RBOlock)
            {
                return enumRangeBreakoutType switch
                {
                    EnumRangeBreakoutType.Underlying => GetRangeBreaKOutUnderlying(enumRangeBreakout, DataSetOfPrice),
                    EnumRangeBreakoutType.Instrument => GetRangeBreaKOutInstrument(enumRangeBreakout, DataSetOfPrice),
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private double GetRangeBreaKOutInstrument(EnumRangeBreakout enumRangeBreakout, List<double> dataSetOfPrice)
        {
            throw new NotImplementedException();
        }

        private double GetRangeBreaKOutUnderlying(EnumRangeBreakout enumRangeBreakout, List<double> dataSetOfPrice)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
