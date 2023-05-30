using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Structure;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.Calculation
{
    public class AlgoCalculation : IAlgoCalculation
    {
        private IContractDetails _contractDetails;
        private IFeed _feed;

        public AlgoCalculation(IContractDetails contractDetails, IFeed feed)
        {
            _contractDetails = contractDetails;
            _feed = feed;
        }

        #region Strike Setting Only ==> As per EnumDeclaration.EnumSelectStrikeCriteria

        private readonly object SelectStrikeSettingLock = new();
        public double GetStrike(EnumSelectStrikeCriteria _strike_criteria,
            EnumStrikeType _strike_type,
            double _premium_lower_range, double _premium_upper_range, double _premium,
            EnumIndex enumIndex,
            EnumUnderlyingFrom enumUnderlyingFrom,
            EnumSegments enumSegments,
            EnumExpiry enumExpiry,
            EnumOptiontype enumOptiontype)
        {
           
            lock (SelectStrikeSettingLock)
            {

                if (_contractDetails.ContractDetailsToken != null)
                {
                    uint Token = _contractDetails
                           .ContractDetailsToken
                           .Where(x => x.Value.Symbol == enumIndex.ToString().ToUpper()
                       && x.Value.Opttype == enumOptiontype
                       && x.Value.Expiry == GetLegExpiry(enumExpiry, enumIndex, enumSegments, enumOptiontype))
                           .Select(x => x.Key)
                           .FirstOrDefault();

                    return _strike_criteria switch
                    {
                        EnumSelectStrikeCriteria.StrikeType => GetStrikeType(_strike_type, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype,Token),
                        EnumSelectStrikeCriteria.PremiumRange => GetPremiumRange(_premium_lower_range, _premium_upper_range, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype),
                        EnumSelectStrikeCriteria.ClosestPremium => GetClosestPremium(_premium, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype),
                        EnumSelectStrikeCriteria.PremiumGreaterOrEqual => GetPremiumGreaterOrEqual(_premium, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype),
                        EnumSelectStrikeCriteria.PremiumLessOrEqual => GetPremiumLessOrEqual(_premium, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype),
                        EnumSelectStrikeCriteria.StraddleWidth => GetStraddleWidth(_premium),//Need to Check StraddleWidth
                        _ => throw new NotImplementedException(),
                    };
                }
                else
                    throw new Exception("Contract Not Loaded");
            }
        }

        private double GetPremiumLessOrEqual(double premium, EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom, EnumSegments enumSegments, EnumExpiry enumExpiry, EnumOptiontype enumOptiontype)
        {
            throw new NotImplementedException();
        }

        private double GetStraddleWidth(double premium)
        {
            throw new NotImplementedException();
        }

        private double GetPremiumGreaterOrEqual(double premium, EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom, EnumSegments enumSegments, EnumExpiry enumExpiry, EnumOptiontype enumOptiontype)
        {
            throw new NotImplementedException();
        }

        private double GetClosestPremium(double premium, EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom, EnumSegments enumSegments, EnumExpiry enumExpiry, EnumOptiontype enumOptiontype)
        {
            throw new NotImplementedException();
        }

        private double GetPremiumRange(double premium_lower_range, double premium_upper_range, EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom, EnumSegments enumSegments, EnumExpiry enumExpiry, EnumOptiontype enumOptiontype)
        {
            throw new NotImplementedException();
        }

        private double GetStrikeType(EnumStrikeType strike_type,
            EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom,
            EnumSegments enumSegments, EnumExpiry enumExpiry,
            EnumOptiontype enumOptiontype, uint token)
        {
            //Call option = Spot Price – Strike Price ()
            //Put option  = Strike Price – Spot price (OPS Direction to Call)
            if (_contractDetails.ContractDetailsToken !=null && _feed.FeedCM != null && _feed.FeedC != null && _feed.FeedC.dcFeedData != null && enumSegments == EnumSegments.Options)
            {
                double ATMStrike = 0, SpotPrice = 0, FinalStrike = 0;
                string Symbol = enumIndex.ToString().ToUpper();
                if (enumUnderlyingFrom == EnumUnderlyingFrom.Futures)
                {
                   DateTime exp =  GetLegExpiry(enumExpiry, enumIndex, EnumSegments.Futures, enumOptiontype);
                   uint FUTToken =  _contractDetails.ContractDetailsToken.Where(x=>x.Value.Expiry == exp 
                   && x.Value.Opttype == EnumOptiontype.XX 
                   && x.Value.Symbol == Symbol)
                        .Select(s=>s.Key)
                        .FirstOrDefault();
                    if (FUTToken != 0)
                        SpotPrice = _feed.FeedC.dcFeedData[FUTToken].LastTradedPrice/100;
                }
                else //underlying Fut
                {
                    string? SpotString;
                    if (enumIndex == EnumIndex.Nifty) SpotString = "Nifty 50";
                    else if (enumIndex == EnumIndex.BankNifty) SpotString = "Nifty Bank";
                    else if (enumIndex == EnumIndex.FinNifty) SpotString = "Nifty Fin Service";
                    else SpotString = null;
                    if (SpotString == null)
                        throw new Exception();

                    if (_feed.FeedCM.dcFeedDataIdx.TryGetValue(SpotString, out FeedCM.MULTIPLE_INDEX_BCAST_REC_7207 valueCM))
                    {
                        var feeddata = valueCM;
                        SpotPrice = feeddata.IndexValue;
                    }
                }

                var list = _contractDetails.ContractDetailsToken.Where(y=>y.Value.Symbol == Symbol &&
                y.Value.Opttype == enumOptiontype).Select(x=>x.Value.Strike).ToList().Distinct();
                //ATM Strike
                ATMStrike = list.Aggregate((x, y) => Math.Abs(x - SpotPrice) < Math.Abs(y - SpotPrice) ? x : y);

                if(strike_type == EnumStrikeType.ATM)
                    return ATMStrike;

                double[] sorteddata = list.ToArray();
                Array.Sort(sorteddata);

                int _atmIndex = Array.IndexOf(sorteddata, ATMStrike);
                int _indexNeedToReturn = _atmIndex + (int)strike_type;
                FinalStrike = sorteddata[_indexNeedToReturn];
                return FinalStrike;
            }
            else
                throw new Exception("Feed is not Initialized");

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

        public DateTime GetLegExpiry(EnumExpiry enumExpiry, EnumIndex enumIndex, EnumSegments enumSegments, EnumOptiontype enumOptiontype)
        {
            lock (_expiry)
            {
                string Symbol = enumIndex.ToString().ToUpper();

                if (_contractDetails.ContractDetailsToken == null)
                    throw new Exception("Contract data Not Found");

                if (enumSegments == EnumSegments.Futures)
                {
                    DateTime[] data = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == Symbol && x.Value.Opttype == EnumOptiontype.XX).Select(x => x.Value.Expiry).ToArray();
                    Array.Sort(data);
                    return data[0];
                }
                DateTime[] data1 = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == Symbol && x.Value.Opttype == enumOptiontype).Select(x => x.Value.Expiry).Distinct().ToArray();
                if (data1.Count() > 0)
                {
                    Array.Sort(data1);
                    if (enumExpiry == EnumExpiry.Weekly)
                        return data1[0];
                    else if (enumExpiry == EnumExpiry.NextWeekly)
                        return data1[1];
                    else
                        return data1[2];
                }
                else
                    throw new Exception("Expiry Not Found");
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
