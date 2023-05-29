using AlgoTerminal_Base;
using AlgoTerminal_Base.Services;

namespace UnitTestAlgo
{
    public class StraddleTest
    {
        private static readonly IAlgoCalculation algo =  new AlgoCalculation();
        [Fact]
        public void Test1()
        {
            algo.GetStrike()
        }
    }
}
