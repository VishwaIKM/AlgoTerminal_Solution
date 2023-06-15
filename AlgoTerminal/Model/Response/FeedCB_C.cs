using AlgoTerminal.Model.Services;
using FeedC;
using System;

namespace AlgoTerminal.Model.Response
{
    public class FeedCB_C : IFeedResp
    {
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
                    leg.LTP = Math.Round(Convert.ToDouble(stFeed.LastTradedPrice)/100.00,2);
                }
            }
        }

        public void Messages(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
