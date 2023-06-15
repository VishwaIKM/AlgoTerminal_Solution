using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.ViewModel;
using FeedC;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class StraddleManager : IStraddleManager
    {

        private readonly IStraddleDataBaseLoadFromCsv straddleDataBaseLoad;
        private readonly ILogFileWriter logFileWriter;
        private readonly IAlgoCalculation algoCalculation;
        private readonly PortfolioViewModel portfolioViewModel;
        private readonly IContractDetails contractDetails;
        private readonly IModeratorManagerModel moderatorManagerModel;
        private readonly IGeneral general;
        public StraddleManager(IStraddleDataBaseLoadFromCsv straddleDataBaseLoad,
            ILogFileWriter logFileWriter,
            IAlgoCalculation algoCalculation,
            PortfolioViewModel portfolioViewModel,
            IContractDetails contractDetails,
            IModeratorManagerModel moderatorManagerModel,
            IGeneral general)
        {
            this.general = general;
            this.general.Portfolios ??= new();
            this.general.PortfolioLegByTokens ??= new();
            this.straddleDataBaseLoad = straddleDataBaseLoad;
            this.logFileWriter = logFileWriter;
            this.algoCalculation = algoCalculation;
            this.portfolioViewModel = portfolioViewModel;
            this.contractDetails = contractDetails;
            this.moderatorManagerModel = moderatorManagerModel;
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
                if (straddleDataBaseLoad.Master_Straddle_Dictionary == null || straddleDataBaseLoad.Straddle_LegDetails_Dictionary == null)
                    throw new Exception("The strategy is Not loaded Correctly");

                //Strategy load
                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {
                    try//MID
                    {
                        var stg_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        PortfolioModel portfolioModel = new();
                        portfolioModel.Name = stg_key;
                        portfolioModel.Index = stg_value.Index;
                        portfolioModel.EntryTime = stg_value.EntryTime;
                        portfolioModel.ExitTime = stg_value.ExitTime;
                        portfolioModel.UserID = stg_value.UserID;
                        portfolioModel.innerObject ??= new();

                        //ADD in Portfolio for GUI
                        if (!general.Portfolios.ContainsKey(portfolioModel.Name))
                        {
                            general.Portfolios.TryAdd(portfolioModel.Name, portfolioModel);
                            if (portfolioViewModel.StrategyDataCollection == null)
                                throw new Exception("THE PortFolio VIEW->MODEL not initiated");
                            //portfolioViewModel.StrategyDataCollection.Add(portfolioModel);

                            //leg load

                            if (straddleDataBaseLoad.Straddle_LegDetails_Dictionary.TryGetValue(stg_key, out ConcurrentDictionary<string, LegDetails> value))
                            {
                                var LegDetails = value;
                                foreach (var Leg in LegDetails.Keys)
                                {
                                    try
                                    {
                                        var leg_value = LegDetails[Leg];
                                        InnerObject innerObject = new();
                                        innerObject.Name = Leg;
                                        innerObject.BuySell = leg_value.Position;
                                        innerObject.Status = EnumStrategyStatus.Added;
                                        innerObject.TradingSymbol = "Loading ...";
                                        innerObject.Qty = leg_value.Lots;
                                        portfolioModel.innerObject.Add(innerObject);
                                        //ADD TO GUI
                                        general.Portfolios.TryUpdate(portfolioModel.Name, portfolioModel, general.Portfolios[portfolioModel.Name]);
                                        if (portfolioViewModel.StrategyDataCollection == null)
                                            throw new Exception("THE PortFolio VIEW -> MODEL not initiated");
                                    }
                                    catch (Exception ex)
                                    {
                                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                                    }
                                }
                                portfolioViewModel.StrategyDataCollection.Add(portfolioModel);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                    }

                }
                return true;

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
            general.PortfolioLegByTokens ??= new();
            if (straddleDataBaseLoad.Master_Straddle_Dictionary == null || straddleDataBaseLoad.Straddle_LegDetails_Dictionary == null)
                throw new Exception("Master dic not loadded posibility The Stg file not loaded.");

            try
            {
                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {//ALL STG

                    var Papa = Task.Factory.StartNew((Action)(async () =>
                    {
                        List<Task> tasks = new();
                        var stg_setting_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        var Portfolio_value = general.Portfolios[stg_key];

                        //waiting for Entry Time
                        if (stg_setting_value.EntryTime > DateTime.Now)
                        {
                            int milisecond = (int)(stg_setting_value.EntryTime - DateTime.Now).TotalMilliseconds;
                            await Task.Delay(milisecond);
                        }

                        //
                        //GUI 
                        var leg_value = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[stg_key];
                        foreach (string Leg in leg_value.Keys)
                        {//ALL LEG

                            var bacha = Task.Factory.StartNew(async () =>
                            {
                                var leg_Details = leg_value[Leg];
                                var portfolio_leg_value = Portfolio_value.innerObject.Where(xxx => xxx.Name == Leg).FirstOrDefault() ?? throw new Exception("Leg was not Loaded in GUI or Portfolios.");
                                try
                                {

                                    //  GUIUpdateForLeg.Status = EnumDeclaration.EnumStrategyStatus.Waiting;


                                    //Get Trading Symbol and Token
                                    DateTime Expiry = algoCalculation.GetLegExpiry(leg_Details.Expiry,
                                                                                       stg_setting_value.Index,
                                                                                       leg_Details.SelectSegment,
                                                                                       leg_Details.OptionType);


                                    double StrikeForLeg = EnumSegments.OPTIONS == leg_Details.SelectSegment ? algoCalculation.GetStrike(leg_Details.StrikeCriteria,
                                                                                       leg_Details.StrikeType,
                                                                                       leg_Details.PremiumRangeLower,
                                                                                       leg_Details.PremiumRangeUpper,
                                                                                       leg_Details.Premium_or_StraddleWidth,
                                                                                       stg_setting_value.Index,//NIFTY/BANKNIFTY/FINNIFTY
                                                                                       stg_setting_value.UnderlyingFrom,
                                                                                       leg_Details.SelectSegment,
                                                                                       leg_Details.Expiry,
                                                                                       leg_Details.OptionType,
                                                                                       leg_Details.Position) :
                                                                                       -0.01;

                                    uint Token = EnumSegments.OPTIONS == leg_Details.SelectSegment ? contractDetails.GetTokenByContractValue(Expiry, leg_Details.OptionType, stg_setting_value.Index, StrikeForLeg) :
                                contractDetails.GetTokenByContractValue(Expiry, EnumOptiontype.XX, stg_setting_value.Index);
                                    string TradingSymbol = contractDetails.GetContractDetailsByToken(Token).TrdSymbol ?? throw new Exception("for " + Token + " Trading Symbol was not Found in Contract Details.");




                                    //Porfolio leg Update
                                    portfolio_leg_value.Token = Token;
                                    portfolio_leg_value.TradingSymbol = TradingSymbol;
                                    portfolio_leg_value.Status = EnumStrategyStatus.Waiting;





                                    //Simple Movement or RanageBreak Out Enable

                                    if (leg_Details.IsSimpleMomentumEnable == true && leg_Details.IsRangeBreakOutEnable == true)
                                        throw new Exception("Simple Momentum and Range Break Out both are Enable");


                                    double ORBPrice_OR_SimpleMovementum = 0;
                                    if (leg_Details.IsRangeBreakOutEnable)
                                    {
                                        ORBPrice_OR_SimpleMovementum = await algoCalculation.GetRangeBreaKOut(leg_Details.SettingRangeBreakOut,
                                                                                  leg_Details.SettingRangeBreakOutType,
                                                                                  leg_Details.RangeBreakOutEndTime,
                                                                                  stg_setting_value.Index,
                                                                                  Token);
                                    }
                                    else if (leg_Details.IsSimpleMomentumEnable)
                                    {
                                        ORBPrice_OR_SimpleMovementum = await algoCalculation.GetLegMomentumlock(leg_Details.SettingSimpleMomentum,
                                                                                                   leg_Details.SimpleMomentum,
                                                                                                   stg_setting_value.Index,
                                                                                                   Token);
                                    }
                                    if (leg_Details.IsStopLossEnable == true)
                                    {
                                        portfolio_leg_value.StopLoss = algoCalculation.GetLegStopLoss(leg_Details.SettingStopLoss,
                                                                                                        leg_Details.OptionType,
                                                                                                        leg_Details.Position,
                                                                                                        leg_Details.StopLoss,
                                                                                                        leg_Details.SelectSegment,
                                                                                                        stg_setting_value.Index,
                                                                                                        Token);
                                    }

                                    if (leg_Details.IsTargetProfitEnable == true)
                                    {
                                        portfolio_leg_value.TargetProfit = algoCalculation.GetLegTargetProfit(leg_Details.SettingTargetProfit,
                                                                                                                            leg_Details.OptionType,
                                                                                                                            leg_Details.Position,
                                                                                                                            leg_Details.TargetProfit,
                                                                                                                            leg_Details.SelectSegment,
                                                                                                                            stg_setting_value.Index,
                                                                                                                            Token);
                                    }
                                    double _currentLTP = algoCalculation.GetStrikePriceLTP(Token);



                                    //Place the Order Using NNAPI 

                                    //GUI
                                    portfolio_leg_value.EntryPrice = _currentLTP;
                                    portfolio_leg_value.Status = EnumStrategyStatus.Running;
                                    portfolio_leg_value.EntryTime = DateTime.Now;



                                    //Bind to Dic responsibile for Feed Update ....
                                    if (general.PortfolioLegByTokens.TryGetValue(Token, out List<InnerObject> value))
                                    {
                                        var legs = value;
                                        legs.Add(portfolio_leg_value);
                                        general.PortfolioLegByTokens[Token] = legs;
                                    }
                                    else
                                    {
                                        List<InnerObject> legs = new()
                                        {
                                            portfolio_leg_value
                                        };
                                        general.PortfolioLegByTokens.TryAdd(Token, legs);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    portfolio_leg_value.Status = EnumStrategyStatus.Error;
                                    logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                                }

                            });
                            tasks.Add(bacha);
                        }
                        //STG Detail when any leg place--------------------------------------- any leg

                        Task.WaitAll(tasks.ToArray());

                        //STG DETAILS when all leg place ---------------------------------------for all

                        //Square OFF all Leg TIMEBASED if Stg is TIMEBASED
                        if (stg_setting_value.EntryAndExitSetting == EnumEntryAndExit.TIMEBASED)
                        {
                            int SquareofSeconds = (int)(stg_setting_value.ExitTime - DateTime.Now).TotalSeconds;
                            if (SquareofSeconds < 0)
                            {
                                logFileWriter.WriteLog(EnumLogType.Error, "Streatgy Name " + Portfolio_value.Name + "  ExitTime is invalid in Congif File.");
                            }
                            else
                            {
                                await Task.Delay(SquareofSeconds);
                                await SquareOffStraddle920(stg_key);
                            }
                        }

                    }));
                }

            }
            catch (Exception ex)
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
