using AlgoTerminal.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.Model
{
    public class ApplicationManagerModel : IApplicationManagerModel
    {
        private readonly ILogFileWriter logFileWriter;
        private readonly IStraddleManager straddleManager;
        private readonly IFeed feed;
        private readonly IContractDetails contractDetails;
        public ApplicationManagerModel(ILogFileWriter logFileWriter, IStraddleManager straddleManager,IFeed feed,IContractDetails contractDetails)
        {
            this.feed = feed;
            this.logFileWriter = logFileWriter;
            this.straddleManager = straddleManager;
            this.contractDetails = contractDetails;
        }

        public async Task<bool> ApplicationStartUpRequirement()
        {
            try
            {
                contractDetails.LoadContractDetails();
                var feedStarted = feed.InitializeFeedDll();//Feed start
                await Task.Delay(3000);
                var daat = straddleManager.StraddleStartUP(); // File Load          
                await straddleManager.FirstTimeDataLoadingOnGUI();// GUI Load
                await Task.Delay(3000);
                await straddleManager.DataUpdateRequest();// Fire The Orders
                return true;
            }
            catch(Exception ex)
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

            }
            catch(Exception ex)
            {
                logFileWriter.WriteLog(Structure.EnumDeclaration.EnumLogType.Error, ex.ToString());
            }
        
        }
    }
}
