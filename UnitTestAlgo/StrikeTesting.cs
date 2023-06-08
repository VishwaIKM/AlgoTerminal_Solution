using AlgoTerminal.Services;
using AlgoTerminal_Base;
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.FileManager;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Response;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.StrategySignalManager;
using AlgoTerminal_Base.UnitTest_Resource;
using Xunit.Abstractions;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace UnitTestAlgo
{
    public class StrikeTesting
    {
        //
        //NOTE:- The Below Test Case will run on OLD Database of Contract and Feed Already loaded in ==>()==> D:\Development Vishwa\AlgoTerminal_Solution\UnitTest_Resources
        // The Above Case can be run in 
        // Live Feed also use the Comment Code
        //
        private static readonly IContractDetails ctr = new ContractDetails();
        private static readonly IGeneral general = new General();
        static readonly FeedCB_C _C = new(general);
        static readonly FeedCB_CM _CM = new(general);
        private static readonly IFeed feed = new Feed(_C, _CM, ctr);
        private static readonly IAlgoCalculation algo = new AlgoCalculation(ctr, feed);
        private readonly ITestOutputHelper output;
        private readonly IFeedLoaderToXml feedLoader = new FeedLoaderToXml(feed, _C, _CM);

        public StrikeTesting(ITestOutputHelper output)
        {
            this.output = output;
        }

        /// <summary>
        /// The Below Function To restore The Feed Data Incase Remove NOTE===>()==> all calculation need to be done again if Contract and feed data Updated
        /// </summary>
        //
        //[Fact]
        //public void Feed_DataStore()
        //{
        //    var feedStart = feed.InitializeFeedDll();
        //    output.WriteLine("Feed Start Success : " + feedStart.ToString());
        //    Thread.Sleep(100000);
        //    feedLoader.SaveDicData();
        //    var feedstop = feed.FeedToStop();
        //    output.WriteLine("Feed Stop Success : " + feedstop.ToString());
        //}

        [Fact]
        public void Feed_START_STOP_TEST()
        {
            var feedStart = feed.InitializeFeedDll();
            var feedstop = feed.FeedToStop();
            output.WriteLine("Feed Start Success : " + feedStart.ToString());
            output.WriteLine("Feed Stop Success : " + feedstop.ToString());
            Assert.True(feedStart);
            Assert.True(feedstop);
        }
        [Fact]
        public void GetExpiryFunctionTest()
        {
            ctr.LoadContractDetails();//Load Contract
            var date = algo.GetLegExpiry(EnumExpiry.Monthly,
                EnumIndex.BANKNIFTY, EnumSegments.Options,
                EnumOptiontype.CE);
            string expiry = date.ToString("dd MMM yyyy").ToUpper();
            output.WriteLine("Recived the Expiry: " + expiry);
            Assert.NotNull(expiry);

        }

        #region Strike Selection Function Test
        [Fact]
        public void GetStrikeFunctionTest_UsingStrikeType_ITM_OTM_ATM()
        {
            //Commented code for live Feed
            ctr.LoadContractDetails();
            //var feedStart = feed.InitializeFeedDll();
            // Thread.Sleep(10000);
            //output.WriteLine("Feed Start Success : " + feedStart.ToString());
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.

            double? data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
            0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            // var feedstop = feed.FeedToStop();
            //output.WriteLine("Feed Stop Success : " + feedstop.ToString());
            Assert.NotNull(data);
            Assert.Equal(18600, data);

        }

        [Fact]
        public void GetStrikeFunctionTest_UsingPremiumRange()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.PremiumRange,
            EnumStrikeType.ATM,
            30, 300, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Sell);
            output.WriteLine("Recived the Strike: " + data.ToString());
            Assert.NotNull(data);
            Assert.Equal(18400, data);
        }

        [Fact]
        public void GetStrikeFunctionTest_ClosestPremium()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.ClosestPremium,
            EnumStrikeType.ATM,
            0, 0, 25,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            Assert.Equal(18750, data);
            Assert.NotNull(data);
        }

        [Fact]
        public void GetStrikeFunctionTest_PremiumGreaterOrEqual()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.PremiumGreaterOrEqual,
            EnumStrikeType.ATM,
            0, 0, 100,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            Assert.Equal(18550, data);
            Assert.NotNull(data);
        }

        [Fact]
        public void GetStrikeFunctionTest_PremiumLessOrEqual()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.PremiumLessOrEqual,
            EnumStrikeType.ATM,
            0, 0, 100,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            Assert.NotNull(data);
            Assert.Equal( 18600, data);
        }

        [Fact]
        public void GetStrikeFunctionTest_StraddleWidth()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();//Load the specific feed stored for unit Test.
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.StraddleWidth,
            EnumStrikeType.ATM,
            0, 0, -4.9,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            Assert.NotNull(data);
            Assert.Equal(17850,data);
        }

        #endregion

        #region By using Strike Get the Momentum Price...

        [Fact]
        public void GetMomentumLock_GetLegSimpleMomentum_Points()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();

            double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
             0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());


            double MomentumData = algo.GetLegMomentumlock(EnumLegSimpleMomentum.Points,
            20,
            EnumIndex.NIFTY,
            EnumExpiry.Weekly,
            data,
            EnumOptiontype.CE,
            EnumSegments.Options);

            output.WriteLine("Momentum price Recived Using Points +20: " + MomentumData.ToString());
            Assert.Equal(115.15, MomentumData);
            
        }
        [Fact]
        public void GetMomentumLock_GetLegSimpleMomentum_PointPercentage()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();

            double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
             0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());


            double MomentumData = algo.GetLegMomentumlock(EnumLegSimpleMomentum.PointPercentage,
            10,
            EnumIndex.NIFTY,
            EnumExpiry.Weekly,
            data,
            EnumOptiontype.CE,
            EnumSegments.Options);

            output.WriteLine("Momentum price Recived Using Points percentage +10: " + MomentumData.ToString());
            Assert.Equal(104.665, MomentumData);

        }

        [Fact]
        public void GetMomentumLock_GetLegSimpleMomentum_UnderlyingPoints()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();

            double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
             0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());


            double MomentumData = algo.GetLegMomentumlock(EnumLegSimpleMomentum.UnderlyingPoints,
            20,
            EnumIndex.NIFTY,
            EnumExpiry.Weekly,
            data,
            EnumOptiontype.CE,
            EnumSegments.Options);

            output.WriteLine("Momentum price Recived Using underlying Points +20: " + MomentumData.ToString());
            Assert.Equal(18639.650000000001, MomentumData);

        }

        [Fact]
        public void GetMomentumLock_GetLegSimple_UnderlyingPointPercentage()
        {
            ctr.LoadContractDetails();
            feedLoader.LoadFromXml();

            double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
             0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());


            double MomentumData = algo.GetLegMomentumlock(EnumLegSimpleMomentum.UnderlyingPointPercentage,
            10,
            EnumIndex.NIFTY,
            EnumExpiry.Weekly,
            data,
            EnumOptiontype.CE,
            EnumSegments.Options);

            output.WriteLine("Momentum price Recived Using underlyingPercentage +10: " + MomentumData.ToString());
            Assert.Equal(20481.615000000002, MomentumData);

        }

        #endregion

        #region Option Range BreakOut testing ORB

        [Fact]

        public void GetRangeBreakOut_FUTURE_High()
        {
            ctr.LoadContractDetails();

            var feedStart = feed.InitializeFeedDll();
            Thread.Sleep(10000);
            output.WriteLine("Feed Start Success : " + feedStart.ToString());

            // double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            // EnumStrikeType.ATM,
            // 0, 0, 0,
            // EnumIndex.NIFTY,
            // EnumUnderlyingFrom.Futures,
            // EnumSegments.Futures,
            // EnumExpiry.Weekly,
            // EnumOptiontype.XX,
            // EnumPosition.Buy);
            //output.WriteLine("Recived the Strike: " + data.ToString());
            double data = -0.01;
            var Price_Of_RangeBreak_Out = algo.GetRangeBreaKOut(EnumRangeBreakout.High,
                EnumRangeBreakoutType.Instrument,
                DateTime.Today.AddHours(14).AddMinutes(3),
                EnumIndex.NIFTY,
                EnumSegments.Futures,
                EnumExpiry.Weekly,
                EnumOptiontype.XX,
                data).Result;

            output.WriteLine(Price_Of_RangeBreak_Out.ToString() + " :Range BreakOut ..");
            var feedstop = feed.FeedToStop();
            output.WriteLine("Feed Stop Success : " + feedstop.ToString());

            Assert.NotNull(Price_Of_RangeBreak_Out);
        }
        [Fact]
        public void GetRangeBreakOut_Underlying_High()
        {
            ctr.LoadContractDetails();

            var feedStart = feed.InitializeFeedDll();
            Thread.Sleep(10000);
            output.WriteLine("Feed Start Success : " + feedStart.ToString());

            double data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.ATM,
            0, 0, 0,
            EnumIndex.NIFTY,
            EnumUnderlyingFrom.Futures,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " + data.ToString());

            var Price_Of_RangeBreak_Out = algo.GetRangeBreaKOut(EnumRangeBreakout.High,
                EnumRangeBreakoutType.Instrument,
                DateTime.Today.AddHours(14).AddMinutes(5),
                EnumIndex.NIFTY,
                EnumSegments.Options,
                EnumExpiry.Weekly,
                EnumOptiontype.CE,
                data).Result;

            output.WriteLine(Price_Of_RangeBreak_Out.ToString() + " :Range BreakOut ..");
            var feedstop = feed.FeedToStop();
            output.WriteLine("Feed Stop Success : " + feedstop.ToString());

            Assert.NotNull(Price_Of_RangeBreak_Out);
        }

        #endregion
    }
}
