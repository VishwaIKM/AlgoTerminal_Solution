using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal_Base.Request
{
    public class Feed
    {
        #region Members
        public static FeedC.Feed_Ikm S_FeedC;//FO
        public static FeedCM.FeedCMIdxC S_FeedCM;//Capital
        private static readonly ushort cNetId = 2; // This is Fix for FO == 2 and for curruncy CD ==6;

        //FOR FUTURE CALCULATION Need to find from contract file.
        public static int NiftyFutureToken = 0;
        public static int BankNiftyFutureToken = 0;
        public static int FinNiftyFutureToken = 0;
        #endregion

        #region Methods
        /// <summary>
        /// SEND REQUEST TO ALL FEED DLL TO STOP THE FEED
        /// </summary>
        public static void FeedToStop()
        {
            if (S_FeedC != null)
                S_FeedC.Stop_Feed();
            if (S_FeedCM != null)
                S_FeedCM.Stop_Feed();
        }
        /// <summary>
        /// FUT token initialization
        /// </summary>
        public static void InternalInitialization()
        {
            DateTime[] exp = Contract.S_Token_ID_ConDicc.Where(x => x.Value.symbol == "NIFTY" && x.Value.InstrumentType == "FUTIDX").Select(x => Convert.ToDateTime(x.Value.expiry)).ToArray();

            if (exp.Count() > 0)
            {
                Array.Sort(exp);
                string eNifty = string.Format("NIFTY{0}FUT", exp[0].ToString("yyMMM").ToUpper());
                string finNifty = string.Format("FINNIFTY{0}FUT", exp[0].ToString("yyMMM".ToUpper()));
                string eBank = string.Format("BANKNIFTY{0}FUT", exp[0].ToString("yyMMM").ToUpper());

                if (Contract.S_Token_TradeSymbol_ConDicc.ContainsKey(eNifty))
                    NiftyFutureToken = Convert.ToInt32(Contract.S_Token_TradeSymbol_ConDicc[eNifty].TokenID);
                if (Contract.S_Token_TradeSymbol_ConDicc.ContainsKey(eBank))
                    BankNiftyFutureToken = Convert.ToInt32(Contract.S_Token_TradeSymbol_ConDicc[eBank].TokenID);
                if (Contract.S_Token_TradeSymbol_ConDicc.ContainsKey(finNifty))
                    FinNiftyFutureToken = Convert.ToInt32(Contract.S_Token_TradeSymbol_ConDicc[finNifty].TokenID);

                if (NiftyFutureToken > 0 && !S_FeedC.dcFeedData.ContainsKey((ulong)NiftyFutureToken))
                    S_FeedC.dcFeedData.TryAdd((ulong)NiftyFutureToken, new FeedC.ONLY_MBP_DATA_7208());
                if (BankNiftyFutureToken > 0 && !S_FeedC.dcFeedData.ContainsKey((ulong)BankNiftyFutureToken))
                    S_FeedC.dcFeedData.TryAdd((ulong)BankNiftyFutureToken, new FeedC.ONLY_MBP_DATA_7208());
                if (FinNiftyFutureToken > 0 && !S_FeedC.dcFeedData.ContainsKey((ulong)FinNiftyFutureToken))
                    S_FeedC.dcFeedData.TryAdd((ulong)FinNiftyFutureToken, new FeedC.ONLY_MBP_DATA_7208());

            }
        }

        #endregion


    }
}
