using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using FeedC;
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
        //lock needed ... (After implementation need to verify if static object have any bad effect)
        /// <summary>
        /// Data Update to View of Portfolio Screen
        /// </summary>
        /// <returns></returns>
        public bool DataUpdateRequest()
        {
            //Feed Data to Update The Str Status
            return true;
        }

        /// <summary>
        /// Feed FO Utilization and Updation
        /// </summary>
        /// <param name="FeedLogTime"></param>
        /// <param name="stFeed"></param>
        public void DataUpdateFrom_FO(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed)
        {

        }

        /// <summary>
        /// FEED CM Utilization and Updation
        /// </summary>
        /// <param name="FeedLogTime"></param>
        /// <param name="IndexName"></param>
        public void DataUpdateFrom_CM(uint FeedLogTime, string IndexName)
        {

        }

        /// <summary>
        /// Start the Process for #920 
        /// </summary>
        /// <returns></returns>
        public bool StartStraddle920()
        {
            return true;
        }

        /// <summary>
        /// Strategy Square off using Strategy Key
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <returns></returns>
        public bool SquareOffStraddle920(string KeyOfStraddle)
        {
            return true;
        }

        /// <summary>
        /// Partial Square off using Straddle #0920 using strategy key and Leg Key
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <param name="LegKey"></param>
        /// <returns></returns>
        public bool SquareOffStraddle920Leg(string KeyOfStraddle,string LegKey)
        {
            return true;
        }

        /// <summary>
        /// Re Entry Straddle #0920  Using Key Entry
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <returns></returns>
        public bool StrategyReEntryStraddle920(string KeyOfStraddle)
        {
            return false;
        }


    }
}
