using AlgoTerminal_Base.Services;
using FeedC;
using System;

namespace AlgoTerminal_Base.Response
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
            if(_general.IsTokenFound(stFeed.Token.ToString()))
            {

            }
        }

        public void Messages(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
