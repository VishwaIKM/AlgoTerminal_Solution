using AlgoTerminal.Model.FileManager;
using AlgoTerminal.Model.Services;
using AlgoTerminal.ViewModel;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace AlgoTerminal.Model
{
    public class ApplicationManagerModel : IApplicationManagerModel
    {
        private readonly ILogFileWriter logFileWriter;
        private readonly IStraddleManager straddleManager;
        private readonly IFeed feed;

        public ApplicationManagerModel(ILogFileWriter logFileWriter, IStraddleManager straddleManager, IFeed feed)
        {
            this.feed = feed;
            this.logFileWriter = logFileWriter;
            this.straddleManager = straddleManager;

        }

        public async Task<bool> ApplicationStartUpRequirement()
        {
            try
            {
                ContractDetails.LoadContractDetails();
                var feedStarted = feed.InitializeFeedDll();//Feed start
                await Task.Delay(1000);
                var daat = straddleManager.StraddleStartUP(); // File Load          
                await straddleManager.FirstTimeDataLoadingOnGUI();// GUI Load
                await Task.Delay(1000);
                await straddleManager.DataUpdateRequest();// Fire The Orders
                return true;
            }
            catch (Exception ex)
            {
                logFileWriter.DisplayLog(Structure.EnumDeclaration.EnumLogType.Error, " Application StartUp Block failed.");
                logFileWriter.WriteLog(Structure.EnumDeclaration.EnumLogType.Error, ex.ToString());
                return false;
            }

        }
        public void ApplicationStopRequirement()
        {
            try
            {
                //Save ALL Data of Dicc
                //Code Here....



                var feedStarted = feed.FeedToStop();//Feed start
                LoginViewModel.NNAPIRequest.LogOutRequest();
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                logFileWriter.WriteLog(Structure.EnumDeclaration.EnumLogType.Error, ex.ToString());
            }

        }
    }
}
