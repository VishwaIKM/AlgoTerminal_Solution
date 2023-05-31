using AlgoTerminal_Base;
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.Contract;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Response;
using AlgoTerminal_Base.Services;
using Xunit.Abstractions;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace UnitTestAlgo
{
    public class ConfigTesting
    {
       
        private static readonly IContractDetails ctr = new ContractDetails();
        static FeedCB_C _C = new();
        static FeedCB_CM _CM = new();
        private static readonly IFeed feed = new Feed(_C, _CM, ctr);
        private static readonly IAlgoCalculation algo = new AlgoCalculation(ctr, feed);
        private readonly ITestOutputHelper output;

        public ConfigTesting(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
           var feedStart =  feed.InitializeFeedDll();
           var feedstop= feed.FeedToStop();
            output.WriteLine("Feed Start Success : "+ feedStart.ToString());
            output.WriteLine("Feed Stop Success : "+feedstop.ToString());
            Assert.True(feedStart);
            Assert.True(feedstop);
        }
        [Fact]
        public void GetExpiryFunctionTest()
        {
            ctr.LoadContractDetails();//Load Contract
            var date = algo.GetLegExpiry(EnumExpiry.Monthly, 
                EnumIndex.FinNifty, EnumSegments.Options, 
                EnumOptiontype.CE);
            string expiry = date.ToString("dd MMM yyyy").ToUpper();
            output.WriteLine("Recived the Expiry: " + expiry);
            Assert.NotNull(expiry);

        }

        [Fact]
        public void GetStrikeFunctionTest_UsingStrikeType_ITM_OTM_ATM()
        {
            ctr.LoadContractDetails();
            var feedStart = feed.InitializeFeedDll();
            Thread.Sleep(10000);
            output.WriteLine("Feed Start Success : " + feedStart.ToString());
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.StrikeType,
            EnumStrikeType.OTM2,
            0, 0, 0,
            EnumIndex.Nifty,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Buy);
            output.WriteLine("Recived the Strike: " +  data.ToString());
            
            var feedstop = feed.FeedToStop();
            output.WriteLine("Feed Stop Success : " + feedstop.ToString());
            Assert.NotNull(data);   

        }

        [Fact]
        public void GetStrikeFunctionTest_UsingPremiumRange()
        {
            ctr.LoadContractDetails();
            var feedStart = feed.InitializeFeedDll();
            Thread.Sleep(10000);
            output.WriteLine("Feed Start Success : " + feedStart.ToString());
            double? data = algo.GetStrike(EnumSelectStrikeCriteria.PremiumRange,
            EnumStrikeType.ATM,
            30, 200, 0,
            EnumIndex.Nifty,
            EnumUnderlyingFrom.Cash,
            EnumSegments.Options,
            EnumExpiry.Weekly,
            EnumOptiontype.CE,
            EnumPosition.Sell);
            output.WriteLine("Recived the Strike: " + data.ToString());

            var feedstop = feed.FeedToStop();
            output.WriteLine("Feed Stop Success : " + feedstop.ToString());
            Assert.NotNull(data);
        }

    }
}
