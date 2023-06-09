using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Windows.Data;

namespace AlgoTerminal.Model.StrategySignalManager
{
    public class StraddleManager : IStraddleManager
    {
      
        private readonly IStraddleDataBaseLoadFromCsv straddleDataBaseLoad;
        private readonly ILogFileWriter logFileWriter;
        public StraddleManager(IStraddleDataBaseLoadFromCsv straddleDataBaseLoad, ILogFileWriter logFileWriter)
        {
            this.straddleDataBaseLoad = straddleDataBaseLoad;
            this.logFileWriter = logFileWriter;
        }

        public bool StraddleStartUP()
        {
            try
            {
                if (File.Exists(App.straddlePath))
                {
                    straddleDataBaseLoad.LoadStaddleStratgy(App.straddlePath);
                }
                else
                {
                    logFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, " Straddle DataBase File not found on given Path " + App.straddlePath);
                }

                return true;
            }
            catch (Exception ex)
            {
                logFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, " Application StartUp Block Complete. ");
                logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, " Application StartUp Block Complete. " + ex.StackTrace);
                return false;
            }
            finally
            {
                logFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, " Application StartUp Block Complete. ");
            }
        }
    }
}
