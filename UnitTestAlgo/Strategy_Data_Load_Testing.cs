
using AlgoTerminal.FileManager;
using AlgoTerminal.Services;
using AlgoTerminal.ViewModel;
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.FileManager;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Response;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.StrategySignalManager;
using Xunit.Abstractions;

namespace UnitTestAlgo
{

    public class Strategy_Data_Load_Testing
    {
        private static readonly IContractDetails ctr = new ContractDetails();
        private static readonly IGeneral general = new General(); 
        static FeedCB_C _C = new(general);
        static FeedCB_CM _CM = new(general);
        private static readonly IFeed feed = new Feed(_C, _CM, ctr);
        private static readonly IAlgoCalculation algo = new AlgoCalculation(ctr, feed);
        private readonly ITestOutputHelper output;
        private static  readonly IStraddleManager straddleManager = new StraddleManager();
        private static readonly ILogFileWriter logWriter = new LogFileWriter();
        private static readonly IStraddleDataBaseLoadFromCsv _data = new StraddleDataBaseLoadFromCsv(straddleManager, logWriter);

        public Strategy_Data_Load_Testing(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DataLoading()
        {
            logWriter.Start(@"D:\Development Vishwa\AlgoTerminal_Solution\UnitTest_Resources", "Log.txt");
            var data = _data.LoadStaddleStratgy(@"D:\Development Vishwa\AlgoTerminal_Solution\UnitTest_Resources\DataBaseStrategy.csv");
            output.WriteLine(data.ToString() +" : Status of File");
            Assert.True(data);
        }
    }
}