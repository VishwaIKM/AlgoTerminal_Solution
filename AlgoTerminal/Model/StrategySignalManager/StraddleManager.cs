using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.ViewModel;
using FeedC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.StrategySignalManager
{
    public class StraddleManager: IStraddleManager
    {

        public ConcurrentDictionary<string, PortfolioModel> Portfolios { get; set; } // key()=> stg {"name"} and {"stg(legname)"}

        private readonly IStraddleDataBaseLoadFromCsv straddleDataBaseLoad;
        private readonly ILogFileWriter logFileWriter;
        private readonly IAlgoCalculation algoCalculation;
        private readonly PortfolioViewModel portfolioViewModel;
        private readonly IContractDetails contractDetails;
        public StraddleManager(IStraddleDataBaseLoadFromCsv straddleDataBaseLoad,
            ILogFileWriter logFileWriter, 
            IAlgoCalculation algoCalculation,
            PortfolioViewModel portfolioViewModel,
            IContractDetails contractDetails)
        {
            this.Portfolios ??= new();
            this.straddleDataBaseLoad = straddleDataBaseLoad;
            this.logFileWriter = logFileWriter;
            this.algoCalculation = algoCalculation;
            this.portfolioViewModel = portfolioViewModel;
            this.contractDetails = contractDetails;
        }
        /// <summary>
        /// fILE lOADING
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> FirstTimeDataLoadingOnGUI()
        {
            try//MAIN
            {
                if (straddleDataBaseLoad.Master_Straddle_Dictionary == null)
                    throw new Exception("The strategy is Not loaded Correctly");

                //Strategy load
                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {
                    try//MID
                    {
                        var stg_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        PortfolioModel portfolioModel = new();
                        portfolioModel.Name = stg_key;
                        portfolioModel.EntryTime = stg_value.EntryTime;
                        portfolioModel.ExitTime = stg_value.ExitTime;
                        portfolioModel.Status = EnumDeclaration.EnumStrategyStatus.Added;
                        portfolioModel.UserID = stg_value.UserID;
                        portfolioModel.IsStrategyRow = true;


                        //ADD in Portfolio for GUI
                        if (!Portfolios.ContainsKey(portfolioModel.Name))
                        {
                            Portfolios.TryAdd(portfolioModel.Name, portfolioModel);
                            if (portfolioViewModel.StrategyDataCollection == null)
                                throw new Exception("THE PortFolio VIEW->MODEL not initiated");
                            portfolioViewModel.StrategyDataCollection.Add(portfolioModel);

                            //leg load

                            if (straddleDataBaseLoad.Straddle_LegDetails_Dictionary.ContainsKey(stg_key))
                            {
                                var LegDetails = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[stg_key];
                                foreach (var Leg in LegDetails.Keys)
                                {
                                    try
                                    {
                                        var leg_value = LegDetails[Leg];
                                        PortfolioModel portfolioModelLeg = new();
                                        portfolioModelLeg.Name = stg_key + "(" + Leg + ")";
                                        portfolioModelLeg.TradingSymbol = "Loading ...";

                                        //ADD TO GUI
                                        Portfolios.TryAdd(portfolioModelLeg.Name, portfolioModelLeg);
                                        if (portfolioViewModel.StrategyDataCollection == null)
                                            throw new Exception("THE PortFolio VIEW->MODEL not initiated");
                                        portfolioViewModel.StrategyDataCollection.Add(portfolioModelLeg);
                                    }
                                    catch (Exception ex)
                                    {
                                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                    }
                }


            }
            catch (Exception ex) { logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString()); }
            return false;
        }

        /// <summary>
        /// Data Update to ViewModel ()=>(ProtfolioViewModel) of Portfolio Screen
        /// </summary>
        /// <returns></returns>
        public async Task DataUpdateRequest()
        {
            try
            {
                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {//ALL STG

                    var Papa = Task.Factory.StartNew(() =>
                    {
                        List<Task> tasks = new();
                        var stg_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        var GUIUpdation = Portfolios[stg_key];
                        GUIUpdation.Status = EnumDeclaration.EnumStrategyStatus.Waiting;
                        GUIUpdation.ReEntryTP = stg_value.OverallReEntryOnTgt;
                        GUIUpdation.ReEntrySL = stg_value.OverallReEntryOnSL;
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => { CollectionViewSource.GetDefaultView(portfolioViewModel.StrategyDataCollection).Refresh(); }), DispatcherPriority.Background, null);
                        
                        //waiting for Entry Time
                        while(stg_value.EntryTime > DateTime.Now)
                        {
                            Thread.Sleep(2000);
                        }

                        //
                        //GUI 
                        var leg_value = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[stg_key];
                        foreach (string Leg in leg_value.Keys)
                        {//ALL LEG

                            var bacha = Task.Factory.StartNew(() =>
                            {
                                try
                                {
                                    var leg_Details = leg_value[Leg];
                                    var GUIUpdateForLeg = Portfolios[stg_key+"("+Leg+")"];
                                    GUIUpdateForLeg.Status = EnumDeclaration.EnumStrategyStatus.Waiting;


                                    //Get Trading Symbol and Token
                                    DateTime Expiry = algoCalculation.GetLegExpiry(leg_Details.Expiry,
                                                                                       stg_value.Index,
                                                                                       leg_Details.SelectSegment,
                                                                                       leg_Details.OptionType);
                                    double StrikeForLeg  = algoCalculation.GetStrike(leg_Details.StrikeCriteria,
                                                                                        leg_Details.StrikeType,
                                                                                        leg_Details.PremiumRangeLower, 
                                                                                        leg_Details.PremiumRangeUpper, 
                                                                                        leg_Details.Premium_or_StraddleWidth,
                                                                                        stg_value.Index,//NIFTY/BANKNIFTY/FINNIFTY
                                                                                        stg_value.UnderlyingFrom,
                                                                                        leg_Details.SelectSegment,
                                                                                        leg_Details.Expiry,
                                                                                        leg_Details.OptionType,
                                                                                        leg_Details.Position);
                                    uint Token = contractDetails.GetTokenByContractValue(Expiry, leg_Details.OptionType, stg_value.Index, StrikeForLeg);
                                    string? TradingSymbol = contractDetails.GetContractDetailsByToken(Token).TrdSymbol;


                                    //GUI
                                    GUIUpdateForLeg.Token = Token;
                                    GUIUpdateForLeg.TradingSymbol = TradingSymbol;
                                    GUIUpdateForLeg.ReEntryTP = leg_Details.ReEntryOnTgt;
                                    GUIUpdateForLeg.ReEntrySL = leg_Details.ReEntryOnSL;

                                    //Simple Movement or RanageBreak Out Enable

                                    if (leg_Details.IsSimpleMomentumEnable == true && leg_Details.IsRangeBreakOutEnable == true)
                                        throw new Exception("Simple Momentum and Range Break Out both are Enable");


                                    double Price;
                                    if (leg_Details.IsRangeBreakOutEnable)
                                    {
                                        Price = algoCalculation.GetRangeBreaKOut(leg_Details.SettingRangeBreakOut,
                                                                                   leg_Details.SettingRangeBreakOutType,
                                                                                   leg_Details.RangeBreakOutEndTime,
                                                                                   stg_value.Index,
                                                                                   leg_Details.SelectSegment,
                                                                                   leg_Details.Expiry,
                                                                                   EnumOptiontype.CE,
                                                                                   StrikeForLeg).Result;
                                    }
                                    if(leg_Details.IsSimpleMomentumEnable)
                                    {
                                        Price = algoCalculation.GetLegMomentumlock(EnumLegSimpleMomentum.Points,
                                                                                                    20,
                                                                                                    EnumIndex.NIFTY,
                                                                                                    EnumExpiry.WEEKLY,
                                                                                                    StrikeForLeg,
                                                                                                    EnumOptiontype.CE,
                                                                                                    EnumSegments.OPTIONS);
                                    }


                                    //Place the Order Using NNAPI 


                                    //GUI
                                   


                                }
                                catch (Exception ex)
                                {
                                    logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                                }

                            });
                            tasks.Add(bacha);
                        }
                        Task.WaitAll(tasks.ToArray());
                        //Now Update STG



                        Application.Current.Dispatcher.BeginInvoke(new Action(() => { CollectionViewSource.GetDefaultView(portfolioViewModel.StrategyDataCollection).Refresh(); }), DispatcherPriority.Background, null);
                    });
                }
                
            }
            catch(Exception ex)
            {
                logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
            }
        }

     

        /// <summary>
        /// Feed FO Utilization and Updation
        /// </summary>
        /// <param name="FeedLogTime"></param>
        /// <param name="stFeed"></param>
        public async Task DataUpdateFrom_FO(uint FeedLogTime, ONLY_MBP_DATA_7208 stFeed)
        {

        }

        /// <summary>
        /// FEED CM Utilization and Updation
        /// </summary>
        /// <param name="FeedLogTime"></param>
        /// <param name="IndexName"></param>
        public async Task DataUpdateFrom_CM(uint FeedLogTime, string IndexName)
        {

        }

        /// <summary>
        /// Start the Process for #920 
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartStraddle920()
        {
            return true;
        }

        /// <summary>
        /// Strategy Square off using Strategy Key
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <returns></returns>
        public async Task<bool> SquareOffStraddle920(string KeyOfStraddle)
        {
            return true;
        }

        /// <summary>
        /// Partial Square off using Straddle #0920 using strategy key and Leg Key
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <param name="LegKey"></param>
        /// <returns></returns>
        public async Task<bool> SquareOffStraddle920Leg(string KeyOfStraddle, string LegKey)
        {
            return true;
        }

        /// <summary>
        /// Re Entry Straddle #0920  Using Key Entry
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <returns></returns>
        public async Task<bool> StrategyReEntryStraddle920(string KeyOfStraddle)
        {
            return false;
        }


    }
}
