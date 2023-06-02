
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.FileManager;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Response;
using AlgoTerminal_Base.Services;
using Xunit.Abstractions;

namespace UnitTestAlgo
{

    public class Stratgy_Overall_Setting_Testing
    {
        private static readonly IContractDetails ctr = new ContractDetails();
        static FeedCB_C _C = new();
        static FeedCB_CM _CM = new();
        private static readonly IFeed feed = new Feed(_C, _CM, ctr);
        private static readonly IAlgoCalculation algo = new AlgoCalculation(ctr, feed);
        private readonly ITestOutputHelper output;

        public Stratgy_Overall_Setting_Testing(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
             
        }
    }
}