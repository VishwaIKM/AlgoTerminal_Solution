using FeedCM;
using System;

namespace AlgoTerminal_Base.Response
{
    public class FeedCB_CM : IFeedRespCMIdx
    {
        public void Feed_CMIdx_CallBack(uint FeedLogTime, string IndexName)
        {
            throw new NotImplementedException();
        }

        public void Feed_CM_CallBack(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed)
        {
            throw new NotImplementedException();
        }

        public void Messages(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
