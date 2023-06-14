﻿using AlgoTerminal.Model.Request;
using AlgoTerminal.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.Calculation
{
    public class AlgoCalculation : IAlgoCalculation
    {
        private readonly IContractDetails _contractDetails;
        private readonly IFeed _feed;

        private const double STT_Opt = 0.00060;
        private const double Stt_Fut = 0.0001;
        private const double Exp_Opt = 0.000572;
        private const double Exp_Fut = 0.0000238;
        private const double StampDuty_Fut = .00002;
        private const double StampDuty_Opt = .00003;
        private double UnderLingValue(EnumIndex enumIndex)
        {
            if (_feed.FeedCM == null)
                throw new Exception("Feed for Captial is NULL");
            string? SpotString;
            if (enumIndex == EnumIndex.NIFTY) SpotString = "Nifty 50";
            else if (enumIndex == EnumIndex.BANKNIFTY) SpotString = "Nifty Bank";
            else if (enumIndex == EnumIndex.FINNIFTY) SpotString = "Nifty Fin Service";
            else SpotString = null;
            if (SpotString == null)
                throw new Exception();

            return _feed.FeedCM.dcFeedDataIdx[SpotString].IndexValue;
        }
        private double GetInstrumentPrice(uint Token)
        {
            if (_feed.FeedC == null)
                throw new Exception("Feed for F&O is NULL");
            return Convert.ToDouble(_feed.FeedC.dcFeedData[Token].LastTradedPrice) / 100.00;
        }
        public AlgoCalculation(IContractDetails contractDetails, IFeed feed)
        {
            _contractDetails = contractDetails;
            _feed = feed;
        }

        #region Strike Setting Only ==> As per EnumDeclaration.EnumSelectStrikeCriteria

        /// <summary>
        ///  Provide the strike Value according to the Strike Criteria .
        /// </summary>
        /// <param name="_strike_criteria"></param>
        /// <param name="_strike_type"></param>
        /// <param name="_premium_lower_range"></param>
        /// <param name="_premium_upper_range"></param>
        /// <param name="_premium_or_StraddleValue"></param>
        /// <param name="enumIndex"></param>
        /// <param name="enumUnderlyingFrom"></param>
        /// <param name="enumSegments"></param>
        /// <param name="enumExpiry"></param>
        /// <param name="enumOptiontype"></param>
        /// <param name="enumPosition"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="CustumException">Can give Erorr if Contract not loaded, Feed is not initiated or Feed Dic do not have any token Details</exception>
        public double GetStrike(EnumSelectStrikeCriteria _strike_criteria,
            EnumStrikeType _strike_type,
            double _premium_lower_range, double _premium_upper_range, double _premium_or_StraddleValue,
            EnumIndex enumIndex,
            EnumUnderlyingFrom enumUnderlyingFrom,
            EnumSegments enumSegments,
            EnumExpiry enumExpiry,
            EnumOptiontype enumOptiontype,
            EnumPosition enumPosition)
        {

            return _strike_criteria switch
            {
                EnumSelectStrikeCriteria.STRIKETYPE => GetStrikeType(_strike_type, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype),
                EnumSelectStrikeCriteria.PREMIUMRANGE => CommonFunctionForPremium(_premium_lower_range, _premium_upper_range, _premium_or_StraddleValue, enumIndex, enumSegments, enumExpiry, enumOptiontype, enumPosition, _strike_criteria),
                EnumSelectStrikeCriteria.CLOSESTPREMIUM => CommonFunctionForPremium(_premium_lower_range, _premium_upper_range, _premium_or_StraddleValue, enumIndex, enumSegments, enumExpiry, enumOptiontype, enumPosition, _strike_criteria),
                EnumSelectStrikeCriteria.PREMIUMGREATEROREQUAL => CommonFunctionForPremium(_premium_lower_range, _premium_upper_range, _premium_or_StraddleValue, enumIndex, enumSegments, enumExpiry, enumOptiontype, enumPosition, _strike_criteria),
                EnumSelectStrikeCriteria.PREMIUMLESSOREQUAL => CommonFunctionForPremium(_premium_lower_range, _premium_upper_range, _premium_or_StraddleValue, enumIndex, enumSegments, enumExpiry, enumOptiontype, enumPosition, _strike_criteria),
                EnumSelectStrikeCriteria.STRADDLEWIDTH => GetStraddleWidth(_premium_or_StraddleValue, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype, enumPosition, _strike_criteria),
                _ => throw new NotImplementedException(),
            };
        }

        private double GetStraddleWidth(double premium_or_StraddleValue, EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom,
            EnumSegments enumSegments, EnumExpiry enumExpiry, EnumOptiontype enumOptiontype, EnumPosition enumPosition,
            EnumSelectStrikeCriteria strike_criteria)
        {
            //ATM Strike +/- (value) * (Call LTP + Put LTP)

            double StraddleWidth;
            if (_contractDetails.ContractDetailsToken == null || _feed.FeedC == null)
                throw new Exception("Contract not Loaded or Feed not Init");
            //Set Spot according to underlying Form
            double ATMStrike = GetStrikeType(EnumStrikeType.ATM, enumIndex, enumUnderlyingFrom, enumSegments, enumExpiry, enumOptiontype);
            DateTime exp = GetLegExpiry(enumExpiry, enumIndex, enumSegments, enumOptiontype);

            //GET CE AND PE TOKEN
            uint[] Token = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == enumIndex.ToString().ToUpper()
                 && x.Value.Expiry == exp
                 && x.Value.Strike == ATMStrike)
                    .Select(x => x.Key)
                    .ToArray();
            if (Token.Length > 2)
                throw new Exception("Call and Put Can not have muplitpile value -> logic fail :) ");

            double _ATMStraddleprice = Convert.ToDouble(_feed.FeedC.dcFeedData[Token[0]].LastTradedPrice + _feed.FeedC.dcFeedData[Token[1]].LastTradedPrice) / 100.00;
            StraddleWidth = ATMStrike + premium_or_StraddleValue * _ATMStraddleprice;
            uint nearest = _contractDetails.ContractDetailsToken[Token[0]].LotSize;
            double StraddleWidthNearestStrike = (double)Math.Round(StraddleWidth / nearest) * nearest;
            return StraddleWidthNearestStrike;
        }
        private double GetStrikeType(EnumStrikeType strike_type,
            EnumIndex enumIndex, EnumUnderlyingFrom enumUnderlyingFrom,
            EnumSegments enumSegments, EnumExpiry enumExpiry,
            EnumOptiontype enumOptiontype)
        {
            //Call option = Spot Price – Strike Price ()
            //Put option  = Strike Price – Spot price (OPS Direction to Call)
            if (_contractDetails.ContractDetailsToken != null && _feed.FeedCM != null && _feed.FeedC != null && _feed.FeedC.dcFeedData != null && enumSegments == EnumSegments.OPTIONS)
            {
                double ATMStrike = 0, SpotPrice = 0, FinalStrike = 0;
                string Symbol = enumIndex.ToString().ToUpper();
                if (enumUnderlyingFrom == EnumUnderlyingFrom.FUTURES)
                {
                    // Setting to EnumSegments.Futures => get Future Expiry first Month
                    DateTime exp = GetLegExpiry(enumExpiry, enumIndex, EnumSegments.FUTURES, enumOptiontype);
                    uint FUTToken = _contractDetails.ContractDetailsToken.Where(x => x.Value.Expiry == exp
                    && x.Value.Opttype == EnumOptiontype.XX
                    && x.Value.Symbol == Symbol)
                         .Select(s => s.Key)
                         .FirstOrDefault();
                    if (FUTToken != 0)
                        SpotPrice = _feed.FeedC.dcFeedData[FUTToken].LastTradedPrice / 100;
                }
                else
                {
                    string? SpotString;
                    if (enumIndex == EnumIndex.NIFTY) SpotString = "Nifty 50";
                    else if (enumIndex == EnumIndex.BANKNIFTY) SpotString = "Nifty Bank";
                    else if (enumIndex == EnumIndex.FINNIFTY) SpotString = "Nifty Fin Service";
                    else SpotString = null;
                    if (SpotString == null)
                        throw new Exception();

                    if (_feed.FeedCM.dcFeedDataIdx.TryGetValue(SpotString, out FeedCM.MULTIPLE_INDEX_BCAST_REC_7207 valueCM))
                    {
                        var feeddata = valueCM;
                        SpotPrice = feeddata.IndexValue;
                    }
                }

                var list = _contractDetails.ContractDetailsToken.Where(y => y.Value.Symbol == Symbol &&
                y.Value.Opttype == enumOptiontype).Select(x => x.Value.Strike).ToList().Distinct();
                //ATM Strike
                ATMStrike = list.Aggregate((x, y) => Math.Abs(x - SpotPrice) < Math.Abs(y - SpotPrice) ? x : y);

                if (strike_type == EnumStrikeType.ATM)
                    return ATMStrike;

                double[] sorteddata = list.ToArray();
                Array.Sort(sorteddata);

                int _atmIndex = Array.IndexOf(sorteddata, ATMStrike);
                int _indexNeedToReturn;
                if (enumOptiontype == EnumOptiontype.CE)
                    _indexNeedToReturn = _atmIndex + (int)strike_type;
                else
                    _indexNeedToReturn = _atmIndex - (int)strike_type;

                FinalStrike = sorteddata[_indexNeedToReturn];
                return FinalStrike;
            }
            else
                throw new Exception("Feed is not Initialized");

        }

        private double CommonFunctionForPremium(double premium_lower_range,
            double premium_upper_range,
            double _premium, EnumIndex enumIndex,
            EnumSegments enumSegments, EnumExpiry enumExpiry,
            EnumOptiontype enumOptiontype, EnumPosition enumPosition,
            EnumSelectStrikeCriteria enumSelectStrikeCriteria)
        {
            //Closest to High Pre => SELL
            //Closest to Low Pre => BUY
            //Range will be diff for call and put

            if (_contractDetails.ContractDetailsToken == null) throw new Exception("Contract is Initialize");
            if (_feed.FeedC == null) throw new Exception("Feed is Not Initialize");
            //expiry
            DateTime exp = GetLegExpiry(enumExpiry, enumIndex, enumSegments, enumOptiontype);

            //valid Token
            uint[] TokenList = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == enumIndex.ToString().ToUpper() &&
                 x.Value.Opttype == enumOptiontype
                 && x.Value.Expiry == exp)
                    .Select(x => x.Key)
                    .ToArray();

            //Feed Avaliable for Token in Feed Dic
            uint[] FeedAvaliableTokenInDic = _feed.FeedC.dcFeedData.Select(x => Convert.ToUInt32(x.Key)).ToArray();

            //Intersection 
            var FinalTokenSetAfterIntersection = TokenList.Intersect(FeedAvaliableTokenInDic);

            //All Premium
            double[] premium = FinalTokenSetAfterIntersection.Select(xx => Convert.ToDouble(_feed.FeedC.dcFeedData[xx].LastTradedPrice) / 100.00).ToArray();
            uint Token; int index = -1;
            //Valid Premium in Range

            if (enumSelectStrikeCriteria == EnumSelectStrikeCriteria.PREMIUMRANGE)
            {
                double[] PremiumInRange = premium.Where(x => x >= premium_lower_range && x <= premium_upper_range).ToArray();
                if (PremiumInRange.Length <= 0) throw new Exception("No Strike Found with in provided Range");
                //GetToken =>Note need to handle if same Price find in multipile strike
                if (enumPosition == EnumPosition.BUY) index = Array.IndexOf(premium, PremiumInRange.Min());
                else if (enumPosition == EnumPosition.SELL) index = Array.IndexOf(premium, PremiumInRange.Max());
            }
            else if (enumSelectStrikeCriteria == EnumSelectStrikeCriteria.CLOSESTPREMIUM)
            {
                List<double> list = premium.ToList();
                index = Array.IndexOf(premium, list.Aggregate((x, y) => Math.Abs(x - _premium) < Math.Abs(y - _premium) ? x : y));
            }
            else if (enumSelectStrikeCriteria == EnumSelectStrikeCriteria.PREMIUMGREATEROREQUAL)
            {
                double[] PremiumInRange = premium.Where(x => x >= _premium).ToArray();
                if (PremiumInRange.Length <= 0) throw new Exception("No Strike Found with in provided Range");
                index = Array.IndexOf(premium, PremiumInRange.Min());
            }
            else if (enumSelectStrikeCriteria == EnumSelectStrikeCriteria.PREMIUMLESSOREQUAL)
            {
                double[] PremiumInRange = premium.Where(x => x <= _premium).ToArray();
                if (PremiumInRange.Length <= 0) throw new Exception("No Strike Found with in provided Range");
                index = Array.IndexOf(premium, PremiumInRange.Max());
            }
            else
            {
                throw new Exception("Invalid Option for Select Strike Option");
            }
            if (index >= 0 && _contractDetails != null)
            {
                Token = FinalTokenSetAfterIntersection.ElementAt(index);
                return _contractDetails.ContractDetailsToken[Token].Strike;
            }
            throw new Exception("Some Calculation is Missed ==> GetPremiumRange Function");
        }
        #endregion

        #region Get StopLoss for Leg
        public double GetLegStopLoss(EnumLegSL enumLegSL,
            EnumOptiontype enumOptiontype, EnumPosition enumPosition,
            double StopLoss,
            EnumSegments enumSegments,
            EnumIndex enumIndex, uint Token)
        {
            double entryPrice;
            if ((enumLegSL == EnumLegSL.Underling || enumLegSL == EnumLegSL.underlingPercentage))
                entryPrice = UnderLingValue(enumIndex);
            else
                entryPrice = GetInstrumentPrice(Token);

            if (enumSegments == EnumSegments.FUTURES)
                enumOptiontype = EnumOptiontype.XX;

            return enumLegSL switch
            {
                EnumLegSL.Points => GetLegPoint_UnderlyingSL(entryPrice, enumOptiontype, enumPosition, StopLoss),
                EnumLegSL.PointPercentage => GetLegPointPercentage_underlyingSL(entryPrice, enumOptiontype, enumPosition, StopLoss),
                EnumLegSL.underlingPercentage => GetLegPointPercentage_underlyingSL(entryPrice, enumOptiontype, enumPosition, StopLoss),
                EnumLegSL.Underling => GetLegPoint_UnderlyingSL(entryPrice, enumOptiontype, enumPosition, StopLoss),
                _ => throw new NotImplementedException(),
            };
        }

        private double GetLegPointPercentage_underlyingSL(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition, double StopLoss)
        {
            if ((enumPosition == EnumPosition.BUY && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumPosition == EnumPosition.SELL && enumOptiontype == EnumOptiontype.PE))
                return entryPrice - entryPrice * StopLoss / 100.00;
            else if ((enumPosition == EnumPosition.SELL && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumOptiontype == EnumOptiontype.PE && enumPosition == EnumPosition.BUY))
                return entryPrice + entryPrice * StopLoss / 100.00;
            else
                throw new NotImplementedException("Invalid Option");
        }

        private double GetLegPoint_UnderlyingSL(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition, double StopLoss)
        {
            if ((enumPosition == EnumPosition.BUY && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumPosition == EnumPosition.SELL && enumOptiontype == EnumOptiontype.PE))
                return entryPrice - StopLoss;
            else if ((enumPosition == EnumPosition.SELL && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumOptiontype == EnumOptiontype.PE && enumPosition == EnumPosition.BUY))
                return entryPrice + StopLoss;
            else
                throw new NotImplementedException("Invalid Option");
        }
        #endregion

        #region Get Target Profit for Leg
        public double GetLegTargetProfit(EnumLegTargetProfit enumLegTP,
            double entryPrice,
            EnumOptiontype enumOptiontype,
            EnumPosition enumPosition,
            double TargetProfit)
        {
            return enumLegTP switch
            {
                EnumLegTargetProfit.Points => GetLegPointTP(entryPrice, enumOptiontype, enumPosition, TargetProfit),
                EnumLegTargetProfit.PointPercentage => GetLegPointPercentageTP(entryPrice, enumOptiontype, enumPosition, TargetProfit),
                EnumLegTargetProfit.Underling => GetLegPointTP(entryPrice, enumOptiontype, enumPosition, TargetProfit),
                EnumLegTargetProfit.underlingPercentage => GetLegPointPercentageTP(entryPrice, enumOptiontype, enumPosition, TargetProfit),
                _ => throw new NotImplementedException(),
            };
        }

        private double GetLegPointPercentageTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition, double TargetProfit)
        {
            if ((enumPosition == EnumPosition.BUY && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumPosition == EnumPosition.SELL && enumOptiontype == EnumOptiontype.PE))
                return entryPrice + entryPrice * TargetProfit / 100.00;
            else if ((enumPosition == EnumPosition.SELL && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumOptiontype == EnumOptiontype.PE && enumPosition == EnumPosition.BUY))
                return entryPrice - entryPrice * TargetProfit / 100.00;
            else
                throw new NotImplementedException("Invalid Option");
        }

        private double GetLegPointTP(double entryPrice, EnumOptiontype enumOptiontype, EnumPosition enumPosition, double TargetProfit)
        {
            if ((enumPosition == EnumPosition.BUY && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumPosition == EnumPosition.SELL && enumOptiontype == EnumOptiontype.PE))
                return entryPrice + TargetProfit;
            else if ((enumPosition == EnumPosition.SELL && (enumOptiontype == EnumOptiontype.CE || enumOptiontype == EnumOptiontype.XX)) || (enumOptiontype == EnumOptiontype.PE && enumPosition == EnumPosition.BUY))
                return entryPrice - TargetProfit;
            else
                throw new NotImplementedException("Invalid Option");
        }

        #endregion

        #region Get Taril SL HIT Value
        /// <summary>
        /// Provide the New StopLoss When SetAmount Move in Favour OtherWise Return 0;
        /// </summary>
        /// <param name="enumLegTrailSL"></param>
        /// <param name="entryPrice"></param>
        /// <param name="xAmount_Percentage"></param>
        /// <param name="yStopLoss"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool GetTrailSLHit(EnumLegTrailSL enumLegTrailSL, double entryPrice, double xAmount_Percentage, double ltp)
        {

            return enumLegTrailSL switch
            {
                EnumLegTrailSL.Points => GetLegPointTailSL(entryPrice, xAmount_Percentage, ltp),
                EnumLegTrailSL.PointPercentage => GetLegPointPercentageTrailSL(entryPrice, xAmount_Percentage, ltp),
                _ => throw new NotImplementedException(),
            };
        }

        private bool GetLegPointPercentageTrailSL(double entryPrice, double xAmount_Percentage, double ltp)
        {
            return entryPrice + entryPrice * xAmount_Percentage / 100 >= ltp;
        }

        private bool GetLegPointTailSL(double entryPrice, double xAmount_Percentage, double ltp)
        {
            return entryPrice + xAmount_Percentage >= ltp;
        }

        #endregion

        #region Get Expiry
        public DateTime GetLegExpiry(EnumExpiry enumExpiry, EnumIndex enumIndex, EnumSegments enumSegments, EnumOptiontype enumOptiontype)
        {
            string Symbol = enumIndex.ToString().ToUpper();

            if (_contractDetails.ContractDetailsToken == null)
                throw new Exception("Contract data Not Found");

            if (enumSegments == EnumSegments.FUTURES || enumExpiry == EnumExpiry.MONTHLY)
            {
                DateTime[] data = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == Symbol && x.Value.Opttype == EnumOptiontype.XX).Select(x => x.Value.Expiry).ToArray();
                Array.Sort(data);
                return data[0];
            }
            DateTime[] data1 = _contractDetails.ContractDetailsToken.Where(x => x.Value.Symbol == Symbol && x.Value.Opttype == enumOptiontype).Select(x => x.Value.Expiry).Distinct().ToArray();
            if (data1.Count() > 0)
            {
                Array.Sort(data1);
                if (enumExpiry == EnumExpiry.WEEKLY)
                    return data1[0];
                else if (enumExpiry == EnumExpiry.NEXTWEEKLY)
                    return data1[1];
                else
                    throw new Exception("Invalid Expiry Selected");
            }
            else
                throw new Exception("Expiry Not Found");
        }

        #endregion

        #region Get Simple Momentum Initial Value Only
        /// <summary>
        /// Simple Momentum => . This function will not hold the order  
        /// </summary>
        /// <param name="enumLegSimpleMomentum"></param>
        /// <param name="momentumPrice"></param>
        /// <param name="enumIndex"></param>
        /// <param name="enumExpiry"></param>
        /// <param name="selectedStrike"></param>
        /// <param name="enumOptiontype"></param>
        /// <param name="enumSegments"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<double> GetLegMomentumlock(EnumLegSimpleMomentum enumLegSimpleMomentum,
            double momentumPrice,
            EnumIndex enumIndex
            , uint Token)
        {
            double _current_Price;
            if (enumLegSimpleMomentum == EnumLegSimpleMomentum.Points || enumLegSimpleMomentum == EnumLegSimpleMomentum.PointPercentage)
                _current_Price = GetInstrumentPrice(Token);
            else if (enumLegSimpleMomentum == EnumLegSimpleMomentum.UnderlyingPoints || enumLegSimpleMomentum == EnumLegSimpleMomentum.UnderlyingPointPercentage)
                _current_Price = UnderLingValue(enumIndex);
            else
                throw new Exception("Invalid Option Selected");

            if (_current_Price <= 0)
                throw new Exception("Invalid Price Fetched from Feed");



            var value = enumLegSimpleMomentum switch
            {
                EnumLegSimpleMomentum.Points => GetLegSimpleMomentum_UnderlyingPoints(momentumPrice, _current_Price),
                EnumLegSimpleMomentum.PointPercentage => GetLegSimple_UnderlyingPointPercentage(momentumPrice, _current_Price),
                EnumLegSimpleMomentum.UnderlyingPoints => GetLegSimpleMomentum_UnderlyingPoints(momentumPrice, _current_Price),
                EnumLegSimpleMomentum.UnderlyingPointPercentage => GetLegSimple_UnderlyingPointPercentage(momentumPrice, _current_Price),
                _ => throw new NotImplementedException(),
            };

            if (enumLegSimpleMomentum == EnumLegSimpleMomentum.Points || enumLegSimpleMomentum == EnumLegSimpleMomentum.PointPercentage)
            {
                if (momentumPrice > 0)
                {
                    while (value > GetInstrumentPrice(Token))
                        await Task.Delay(300);
                }
                else
                {
                    while (value < GetInstrumentPrice(Token))
                        await Task.Delay(300);
                }
                return GetInstrumentPrice(Token);
            }
            else if (enumLegSimpleMomentum == EnumLegSimpleMomentum.UnderlyingPoints || enumLegSimpleMomentum == EnumLegSimpleMomentum.UnderlyingPointPercentage)
            {
                if (momentumPrice > 0)
                {
                    while (value > UnderLingValue(enumIndex))
                        await Task.Delay(300);
                }
                else
                {
                    while (value < UnderLingValue(enumIndex))
                        await Task.Delay(300);
                }
            }
            return UnderLingValue(enumIndex);
        }

        private double GetLegSimple_UnderlyingPointPercentage(double momentumPrice, double currentPriceForStrike)
        {

            return currentPriceForStrike + currentPriceForStrike * momentumPrice / 100;
        }

        private double GetLegSimpleMomentum_UnderlyingPoints(double momentumPrice, double currentPriceForStrike)
        {
            return currentPriceForStrike + momentumPrice;
        }


        #endregion

        #region Get RangeBreakOut


        /// <summary>
        /// This function will take time Run it using async only...
        /// </summary>
        /// <param name="enumRangeBreakout"></param>
        /// <param name="enumRangeBreakoutType"></param>
        /// <param name="DataSetOfPrice"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<double> GetRangeBreaKOut(EnumRangeBreakout enumRangeBreakout,
            EnumRangeBreakoutType enumRangeBreakoutType,
            DateTime RangeBreakOutEndTime,
            EnumIndex enumIndex
            , uint Token)
        {
            if (_contractDetails.ContractDetailsToken == null)
                throw new Exception("Contract is Null");

            if (_feed.FeedC == null)
                throw new Exception("Feed FO is NULL");

            if (_feed.FeedCM == null)
                throw new Exception("Feed CM is NULL");


           
            List<double> SetOfPrice = new();
            if (enumRangeBreakoutType == EnumRangeBreakoutType.INSTRUMENT)
            {

                //add Price in the Set 
                while (RangeBreakOutEndTime >= DateTime.Now)
                {
                    await Task.Delay(300);
                    double price = Convert.ToDouble(_feed.FeedC.dcFeedData[Token].LastTradedPrice / 100.00);
                    if (!SetOfPrice.Contains(price))
                        SetOfPrice.Add(price);
                }
            }
            else if (enumRangeBreakoutType == EnumRangeBreakoutType.UNDERLYING)
            {
                string? SpotString;
                if (enumIndex == EnumIndex.NIFTY) SpotString = "Nifty 50";
                else if (enumIndex == EnumIndex.BANKNIFTY) SpotString = "Nifty Bank";
                else if (enumIndex == EnumIndex.FINNIFTY) SpotString = "Nifty Fin Service";
                else SpotString = null;
                if (SpotString == null)
                    throw new Exception();

                //add Price in the Set 
                while (RangeBreakOutEndTime >= DateTime.Now)
                {
                    await Task.Delay(300);
                    double price = _feed.FeedCM.dcFeedDataIdx[SpotString].IndexValue;
                    if (!SetOfPrice.Contains(price))
                        SetOfPrice.Add(price);
                }
            }
            else
                throw new Exception("Invalid Option Selected");


            return enumRangeBreakout switch
            {
                EnumRangeBreakout.HIGH => GetRangeBreaKOutHigh(SetOfPrice),
                EnumRangeBreakout.LOW => GetRangeBreaKOutILow(SetOfPrice),
                _ => throw new NotImplementedException(),
            };
        }

        private double GetRangeBreaKOutILow(List<double> _setOfPrice)
        {
            return _setOfPrice.Min();
        }

        private double GetRangeBreaKOutHigh(List<double> _setOfPrice)
        {
            return _setOfPrice.Max();
        }

        #endregion

        #region Get MTM
        public double GetMTM(double BuyAvg_Price, double SellAvg_Price, double Exp_, int BuyTrdqty, int SellTrdQty, uint Token)
        {
            if (_feed.FeedC == null)
                throw new Exception("Feed for F&O not init.");
            uint Bid_Ask = BuyTrdqty - SellTrdQty > 0 ? _feed.FeedC.dcFeedData[Token].AskPrice1 : _feed.FeedC.dcFeedData[Token].BidPrice1;
            return (BuyAvg_Price * BuyTrdqty) + (SellAvg_Price * SellTrdQty) + Math.Abs(BuyTrdqty - SellTrdQty) * Bid_Ask - Math.Max(BuyTrdqty, SellTrdQty) * Exp_;
        }
        #endregion

        #region Get Expence
        public double GetExpenses(uint Token, EnumSegments enumSegments)
        {
            if (_feed.FeedC == null)
                throw new Exception("Feed for F&O not init.");
            if (EnumSegments.OPTIONS == enumSegments)
                return _feed.FeedC.dcFeedData[Token].BidPrice1 * STT_Opt + (_feed.FeedC.dcFeedData[Token].BidPrice1 + _feed.FeedC.dcFeedData[Token].AskPrice1) * Exp_Opt + _feed.FeedC.dcFeedData[Token].AskPrice1 * StampDuty_Opt;
            else
                return _feed.FeedC.dcFeedData[Token].BidPrice1 * Stt_Fut + (_feed.FeedC.dcFeedData[Token].BidPrice1 + _feed.FeedC.dcFeedData[Token].AskPrice1) * Exp_Fut + _feed.FeedC.dcFeedData[Token].AskPrice1 * StampDuty_Fut;
        }
        #endregion

        #region Get Stike Price using Token LTP

        public double GetStrikePriceLTP(uint Token)
        {
            if (_feed.FeedC == null)
                throw new Exception("Feed for F&O not init.");
            return Convert.ToDouble(_feed.FeedC.dcFeedData[Token].LastTradedPrice) / 100.00;

        }
        #endregion
    }
}
