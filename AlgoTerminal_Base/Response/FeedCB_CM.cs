using AlgoTerminal_Base.Services;
using FeedCM;
using System;

namespace AlgoTerminal_Base.Response
{
    public class FeedCB_CM : IFeedRespCMIdx
    {
        private readonly IGeneral _general;

        public FeedCB_CM(IGeneral general)
        {
            _general = general;
        }

        public void Feed_CMIdx_CallBack(uint FeedLogTime, string IndexName)
        {
            if(_general.IsTokenFound(IndexName))
            {

            }
        }

        public void Feed_CM_CallBack(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed)
        {
            //NOT IN USE
        }

        public void Messages(string msg)
        {
           //FOR LOGGER
        }
    }
}
