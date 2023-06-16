using AlgoTerminal.Model.Request;
using AlgoTerminal.Model.Services;
using FeedC;
using System;
using System.Linq;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.Response
{
    public class FeedCB_C : IFeedResp
    {
        private const double STT_Opt = 0.00060;
        private const double Exp_Opt = 0.000572;
        private const double StampDuty_Opt = .00003;

        private readonly IGeneral _general;
        public FeedCB_C(IGeneral general)
        {
            _general = general;
        }
        public void Feed_CallBack(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed)
        {
           if(_general.PortfolioLegByTokens.ContainsKey(stFeed.Token))
            {
               var legs =  _general.PortfolioLegByTokens[stFeed.Token];
                foreach(var leg in legs)
                {  
                    //MTM//LTP when order is placed
                    if(leg.EntryPrice != 0 && leg.ExitPrice ==0)
                    {
                        //leg LTP Update ...
                        leg.LTP = Math.Round(Convert.ToDouble(stFeed.LastTradedPrice) / 100.00, 2);
                        double _exp = stFeed.BidPrice1 * STT_Opt + (stFeed.BidPrice1 + stFeed.AskPrice1) * Exp_Opt + stFeed.AskPrice1 * StampDuty_Opt;
                        uint Bid_Ask = leg.BuySell == EnumPosition.BUY ? stFeed.AskPrice1 : stFeed.BidPrice1;
                        leg.MTM = Math.Round(((leg.Qty*leg.EntryPrice) + Math.Abs(leg.Qty) * Bid_Ask - Math.Abs(leg.Qty)*_exp) /100.00,2);
                        double Pnl = leg.BuySell == EnumPosition.BUY ? (leg.LTP - leg.EntryPrice) : (leg.EntryPrice - leg.LTP);
                        leg.PNL = Math.Round((Pnl* leg.Qty - _exp)/100.00,2);

                        //stg Update 
                        var stg = _general.Portfolios.Where(x => x.Value.InnerObject.Contains(leg)).FirstOrDefault();
                        double finalLtp = 0;
                        foreach(var x in stg.Value.InnerObject)
                        {
                            finalLtp += x.PNL;
                        }
                        stg.Value.PNL = Math.Round(finalLtp,2);
                    }
                }
            }
        }

        public void Messages(string msg)
        {
            throw new NotImplementedException();
        }

    }
}
