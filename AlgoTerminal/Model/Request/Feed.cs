using AlgoTerminal.Model.Response;
using AlgoTerminal.Model.Services;
using System;

namespace AlgoTerminal.Model.Request
{
    public class Feed : IFeed
    {
        #region Members
        public FeedC.Feed_Ikm? FeedC { get; set; }//FO
        public FeedCM.FeedCMIdxC? FeedCM { get; set; }//Capital
        private readonly ushort cNetId = 2; // This is Fix for FO == 2 and for curruncy CD ==6;

        //Instance 
        private FeedCB_C _C;
        private FeedCB_CM _CM;
        private IContractDetails _ContractDetails;


        #endregion

        #region Methods
        public Feed(FeedCB_C c, FeedCB_CM cM, IContractDetails contractDetails)
        {
            _C = c;
            _CM = cM;
            _ContractDetails = contractDetails;
        }
        /// <summary>
        /// SEND REQUEST TO ALL FEED DLL TO STOP THE FEED
        /// </summary>
        public bool FeedToStop()
        {
            try
            {
                FeedC?.Stop_Feed();
                FeedCM?.Stop_Feed();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// SEND REQUEST TO FEED DLL FEEDC(FO) AND FEEDCM(CAPITAL) TO START 
        /// </summary>
        public bool InitializeFeedDll()
        {
            bool status;
            try
            {
                //save then in global static request object
                FeedC = new FeedC.Feed_Ikm(_C);
                FeedCM = new FeedCM.FeedCMIdxC(_CM);

                //SEND REQUEST TO DLL FEED-->C
                status = FeedC.Init("233.1.2.5", "", "192.168.1.50", 34330, cNetId);
                status = FeedCM.Init("233.1.2.5", "", "192.168.1.50", 34074);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
            return status;
        }
        #endregion


    }
}
