using AlgoTerminal.Model.FileManager;
using AlgoTerminal.Model.NNAPI;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static AlgoTerminal.Model.Structure.EnumDeclaration;

namespace AlgoTerminal.Model.StrategySignalManager
{
    public class StraddleManager : IStraddleManager
    {
        #region var & filed
        private readonly IStraddleDataBaseLoadFromCsv straddleDataBaseLoad;
        private readonly ILogFileWriter logFileWriter;
        private readonly IAlgoCalculation algoCalculation;
        DispatcherTimer dispatcherTimer = new();
        #endregion

        #region Methods & const.
        public StraddleManager(IStraddleDataBaseLoadFromCsv straddleDataBaseLoad,
            ILogFileWriter logFileWriter,
            IAlgoCalculation algoCalculation
           )
        {
            General.Portfolios ??= new();
            General.PortfolioLegByTokens ??= new();
            this.straddleDataBaseLoad = straddleDataBaseLoad;
            this.logFileWriter = logFileWriter;
            this.algoCalculation = algoCalculation;
        }

        #region Update The TRAIL SL ==> SL HIT ==> TP HIT ==> RE-ENTRY COMMAND:
        /// <summary>
        /// Time Init Method TIMESPAN()=> MILISECOND
        /// </summary>
        private void StartMonitoringCommand()
        {
            dispatcherTimer.Tick += new EventHandler(MonitoringThread);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();
        }
        /// <summary>
        /// Checking the Portfolio SL TP Trail SL and ReEntry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitoringThread(object? sender, EventArgs e)
        {

            //TO DO:
            //1. IS SL HIT SQUARE OFF CHECK IF RENTRY ON SL |(LEG/STG) => Decrement the RE Count
            //2. IS TP HIT SQUARE OFF CHECK IF RENTRY ON TP |(LEG/STG) => Decrement the RE Count
            //3. IS TRAIL SL AMOUNT HIT => Update TARIL AND SL FOR LEG or STG
            //4. Partail leg squre off or full stg if any leg sl hit
            try
            {
                if (straddleDataBaseLoad.Master_Straddle_Dictionary == null)
                    throw new Exception("Master STG is Empty Function from MonitoringThread.");

                if (straddleDataBaseLoad.Straddle_LegDetails_Dictionary == null)
                    throw new Exception("Master LEG is Empty Function from MonitoringThread.");

                if (General.Portfolios == null)
                    throw new Exception("General Portfolio is Empty Function from MonitoringThread.");


                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {//ALL STG
                    var Parent = Task.Factory.StartNew((Action)(async () =>
                    {

                        bool IsStgSquareOffRequestSend = false;
                        var stg_setting_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        var Portfolio_value = General.Portfolios[stg_key];
                        var leg_value = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[stg_key];
                        foreach (string Leg in leg_value.Keys)
                        {
                            #region Leg watcher
                            var leg_Details = leg_value[Leg];
                            var portfolio_leg_value = Portfolio_value.InnerObject.Where(xxx => xxx.Name == Leg).FirstOrDefault() ?? throw new Exception("Leg was not Loaded in GUI or Portfolios.");

                            try
                            {
                                if (portfolio_leg_value.ExitPrice == 0 && (portfolio_leg_value.Status != EnumStrategyStatus.Added) && portfolio_leg_value.IsLegInMonitoringQue) // Exit price != 0 means this leg is Complete
                                {

                                    portfolio_leg_value.IsLegInMonitoringQue = false;

                                    #region TRAIL SL FOR Leg Check and Update

                                    if (leg_Details.IsTrailSlEnable == true)
                                    {
                                        algoCalculation.UpdateLegSLTrail_IF_HIT(portfolio_leg_value, leg_Details);
                                    }
                                    #endregion

                                    #region Check SL/ TP for LEG
                                    bool SL_HIT = false, TP_HIT = false;
                                    if (leg_Details.IsStopLossEnable == true && portfolio_leg_value.StopLoss > 0)
                                    {
                                        SL_HIT = algoCalculation.Get_if_SL_is_HIT(portfolio_leg_value.StopLoss,
                                                                                        leg_Details.SettingStopLoss,
                                                                                        leg_Details.OptionType,
                                                                                        leg_Details.Position,
                                                                                        stg_setting_value.Index,
                                                                                        portfolio_leg_value.Token);

                                        if (SL_HIT)
                                        {
                                            logFileWriter.DisplayLog(EnumLogType.Info, "SL HIT for Leg :" + Leg + "  in the Stg: " + stg_key);
                                            portfolio_leg_value.Status = EnumStrategyStatus.CompleteBySL;
                                        }
                                    }

                                    if (leg_Details.IsTargetProfitEnable == true && portfolio_leg_value.TargetProfit > 0)
                                    {
                                        TP_HIT = algoCalculation.Get_if_TP_is_HIT(portfolio_leg_value.TargetProfit,
                                                                                        leg_Details.SettingStopLoss,
                                                                                        leg_Details.OptionType,
                                                                                        leg_Details.Position,
                                                                                        stg_setting_value.Index,
                                                                                        portfolio_leg_value.Token);
                                        if (TP_HIT)
                                        {
                                            logFileWriter.DisplayLog(EnumLogType.Info, "TP HIT for Leg :" + Leg + "  in the Stg: " + stg_key);
                                            portfolio_leg_value.Status = EnumStrategyStatus.CompleteByTP;
                                        }
                                    }
                                    //Check if partial squre of enable or complete

                                    if (stg_setting_value.SquareOff == EnumSquareOff.PARTIAL && (SL_HIT || TP_HIT))
                                    {
                                        //leg squareOff
                                        await SquareOffStraddle920Leg(portfolio_leg_value);
                                        ReEntryAndPlaceOrderCommon(SL_HIT, TP_HIT, portfolio_leg_value, leg_Details, stg_setting_value, Portfolio_value, leg_value);

                                    }
                                    else if (stg_setting_value.SquareOff == EnumSquareOff.COMPLETE && (SL_HIT || TP_HIT))
                                    {
                                        IsStgSquareOffRequestSend = true;
                                        //all Leg Square off
                                        await SquareOffStraddle920(Portfolio_value);
                                        //Need to find Simple Solution  =================> MARK HERE
                                        foreach (string ChildLeg in leg_value.Keys)
                                        {
                                            var child_leg_Details = leg_value[Leg];
                                            var child_portfolio_leg_value = Portfolio_value.InnerObject.Where(xxx => xxx.Name == ChildLeg).FirstOrDefault() ?? throw new Exception("Leg was not Loaded in GUI or Portfolios.");
                                            if (child_portfolio_leg_value.ExitPrice == 0 && child_portfolio_leg_value.Status != EnumStrategyStatus.Added)
                                            {
                                                await SquareOffStraddle920Leg(child_portfolio_leg_value);
                                                ReEntryAndPlaceOrderCommon(SL_HIT, TP_HIT, child_portfolio_leg_value, child_leg_Details, stg_setting_value, Portfolio_value, leg_value);
                                            }
                                        }
                                    }

                                    #endregion


                                }
                            }
                            catch { }

                            portfolio_leg_value.IsLegInMonitoringQue = true;
                            #endregion Leg watcher End

                        }
                        #region STG Watcher
                        if (!IsStgSquareOffRequestSend) //if square of  then no need to check SL AND TP OR RE-ENTRY
                        {
                            #region Trailing Options Check and Update

                            if(stg_setting_value.IsOverallTrallingOptionEnable == true)
                            {
                                algoCalculation.CheckAndUpdateOverallTrailingOption(Portfolio_value, stg_setting_value);
                            }

                            #endregion




                            #region SL AND TP HIT AND REENTRY
                            bool Overall_SL_HIT = false, Overall_TP_HIT = false;
                            if (stg_setting_value.IsOverallStopLossEnable == true && Portfolio_value.StopLoss > 0)
                            {
                                Overall_SL_HIT = algoCalculation.Is_overall_sl_hit(stg_setting_value, Portfolio_value);
                                if (Overall_SL_HIT)
                                {
                                    logFileWriter.DisplayLog(EnumLogType.Info, "SL HIT for the Stg: " + stg_key);
                                    await SquareOffStraddle920(Portfolio_value, EnumStrategyStatus.CompletedByOverallSL);
                                }

                            }
                            if (stg_setting_value.IsOverallReEntryOnTgtEnable == true && Portfolio_value.TargetProfit > 0)
                            {

                                Overall_TP_HIT = algoCalculation.Is_overall_tp_hit(stg_setting_value, Portfolio_value);
                                if (Overall_TP_HIT)
                                {
                                    logFileWriter.DisplayLog(EnumLogType.Info, "TP HIT for the Stg: " + stg_key);
                                    await SquareOffStraddle920(Portfolio_value, EnumStrategyStatus.CompletedByOverallTP);
                                }
                            }

                            if ((Overall_SL_HIT && Portfolio_value.ReEntrySL > 0) || Overall_TP_HIT && Portfolio_value.ReEntryTP > 0)
                            {
                                OverallStrategyReEntry(Overall_SL_HIT, Overall_TP_HIT, Portfolio_value, stg_key);
                            }
                            #endregion
                        }
                        #endregion Watcher end
                    }));

                }
            }
            catch (Exception ex) { logFileWriter.WriteLog(EnumLogType.Error, ex.Message + "  " + ex.StackTrace); }
        }


        private void OverallStrategyReEntry(bool Overall_SL_HIT, bool Overall_TP_HIT, PortfolioModel Portfolio_value, string old_stg_key)
        {
            try
            {
                //Add to dic
                //Master STG DIC + ALL LEG TO REENTER in Master leg DIC
                //New Portfolio Dic
                //New inner Object



                //Clone the Master Dic <T> Value
                var clone_stg_setting_value = OtherMethods.DeepCopy(straddleDataBaseLoad.Master_Straddle_Dictionary[old_stg_key]);
                var clone_leg_value = OtherMethods.DeepCopy(straddleDataBaseLoad.Straddle_LegDetails_Dictionary[old_stg_key]);

                //Decrement in Count of reEntry
                if (Overall_SL_HIT)
                    clone_stg_setting_value.OverallReEntryOnSL--;
                else
                    clone_stg_setting_value.OverallReEntryOnTgt--;



                //Change The Name 
                string new_stg_key = OtherMethods.GetNewName(old_stg_key);

                //Add TO Master Table/Dic
                if (straddleDataBaseLoad.Master_Straddle_Dictionary.ContainsKey(new_stg_key))
                {
                    //BUG
                    logFileWriter.WriteLog(EnumLogType.Error, new_stg_key + "This key already added in Master_Straddle_Dictionary");
                }
                else
                {
                    straddleDataBaseLoad.Master_Straddle_Dictionary.TryAdd(new_stg_key, clone_stg_setting_value);
                }
                if (straddleDataBaseLoad.Straddle_LegDetails_Dictionary.ContainsKey(new_stg_key))
                {
                    //BUG
                    logFileWriter.WriteLog(EnumLogType.Error, new_stg_key + "This key already added in Straddle_LegDetails_Dictionary");
                }
                else
                {
                    straddleDataBaseLoad.Straddle_LegDetails_Dictionary.TryAdd(new_stg_key, clone_leg_value);
                }

                //Add New Portfolio
                PortfolioModel portfolioModel = new();
                portfolioModel.Name = new_stg_key;
                portfolioModel.Index = clone_stg_setting_value.Index;
                portfolioModel.EntryTime = DateTime.Now;
                portfolioModel.ExitTime = clone_stg_setting_value.ExitTime;
                portfolioModel.UserID = clone_stg_setting_value.UserID;
                portfolioModel.InnerObject ??= new();

                if (!General.Portfolios.ContainsKey(portfolioModel.Name))
                {
                    General.Portfolios.TryAdd(portfolioModel.Name, portfolioModel);

                    //Initial leg load in DataBase and GUI INIT
                    foreach (var Leg in clone_leg_value.Keys)
                    {
                        try
                        {
                            var leg_value = clone_leg_value[Leg];


                            //Reverce the Position
                            if (((clone_stg_setting_value.SettingOverallReEntryOnSL == EnumOverallReEntryOnSL.REREVASAP || clone_stg_setting_value.SettingOverallReEntryOnSL == EnumOverallReEntryOnSL.REREVMOMENTUM) && Overall_SL_HIT) ||
                                ((clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REREVASAP || clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REREVMOMENTUM) && Overall_TP_HIT))
                            {
                                leg_value.Position = leg_value.Position == EnumPosition.BUY ? EnumPosition.SELL : EnumPosition.BUY;
                            }
                            //--------------------------------------------------------------------------------------------------------

                            InnerObject innerObject = new();
                            innerObject.StgName = new_stg_key;
                            innerObject.Name = Leg;
                            innerObject.BuySell = leg_value.Position;
                            innerObject.Status = EnumStrategyStatus.Added;
                            innerObject.TradingSymbol = "Loading ...";
                            innerObject.Qty = leg_value.Lots;
                            innerObject.enumUnderlyingFromForLeg = clone_stg_setting_value.UnderlyingFrom;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {

                                portfolioModel.InnerObject.Add(innerObject);

                            }), DispatcherPriority.Background, null);

                            //ADD TO GUI
                            General.Portfolios.TryUpdate(portfolioModel.Name, portfolioModel, General.Portfolios[portfolioModel.Name]);
                        }
                        catch (Exception ex)
                        {
                            logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                        }
                    }
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {

                        PortfolioViewModel.StrategyDataCollection.Add(portfolioModel);

                    }), DispatcherPriority.Background, null);



                    //Place The Order according to Setting
                    double TotalPremium = 0;

                    var Papa = Task.Factory.StartNew((Action)(async () =>
                    {
                        List<Task> tasks = new();
                        var stg_setting_value = straddleDataBaseLoad.Master_Straddle_Dictionary[new_stg_key];
                        var Portfolio_value = General.Portfolios[new_stg_key];
                        //
                        //GUI 
                        var leg_value = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[new_stg_key];
                        foreach (string Leg in leg_value.Keys)
                        {//ALL LEG

                            var bacha = Task.Factory.StartNew(async () =>
                            {
                                var leg_Details = leg_value[Leg];
                                var portfolio_leg_value = Portfolio_value.InnerObject.Where(xxx => xxx.Name == Leg).FirstOrDefault() ?? throw new Exception("Leg was not Loaded in GUI or Portfolios.");
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

                                    uint Token = EnumSegments.OPTIONS == leg_Details.SelectSegment ? ContractDetails.GetTokenByContractValue(Expiry, leg_Details.OptionType, stg_setting_value.Index, StrikeForLeg) :
                                ContractDetails.GetTokenByContractValue(Expiry, EnumOptiontype.XX, stg_setting_value.Index);
                                    string TradingSymbol = ContractDetails.GetContractDetailsByToken(Token).TrdSymbol ?? throw new Exception("for " + Token + " Trading Symbol was not Found in Contract Details.");



                                    portfolio_leg_value.Qty *= (int)ContractDetails.GetContractDetailsByToken(Token).LotSize;
                                    //Porfolio leg Update
                                    portfolio_leg_value.Token = Token;
                                    portfolio_leg_value.TradingSymbol = TradingSymbol;
                                    portfolio_leg_value.Status = EnumStrategyStatus.Waiting;
                                    portfolio_leg_value.ReEntrySL = leg_Details.ReEntryOnSL;
                                    portfolio_leg_value.ReEntryTP = leg_Details.ReEntryOnTgt;




                                    //Simple Movement or RanageBreak Out Enable

                                    if (leg_Details.IsSimpleMomentumEnable == true && leg_Details.IsRangeBreakOutEnable == true)
                                        throw new Exception("Simple Momentum and Range Break Out both are Enable");


                                    double ORBPrice_OR_SimpleMovementum = 0;

                                    if (leg_Details.IsSimpleMomentumEnable && ((clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REREVMOMENTUM || clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REREVMOMENTUM) || (clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REMOMENTUM || clone_stg_setting_value.SettingOverallReEntryOnTgt == EnumOverallReEntryOnTarget.REREVMOMENTUM)))
                                    {
                                        ORBPrice_OR_SimpleMovementum = await algoCalculation.GetLegMomentumlock(leg_Details.SettingSimpleMomentum,
                                                                                                   leg_Details.SimpleMomentum,
                                                                                                   stg_setting_value.Index,
                                                                                                   Token, portfolio_leg_value);
                                    }

                                    if (leg_Details.IsStopLossEnable == true)
                                    {
                                        portfolio_leg_value.StopLoss = Math.Round(algoCalculation.GetLegStopLoss(leg_Details.SettingStopLoss,
                                                                                                        leg_Details.OptionType,
                                                                                                        leg_Details.Position,
                                                                                                        leg_Details.StopLoss,
                                                                                                        leg_Details.SelectSegment,
                                                                                                        stg_setting_value.Index,
                                                                                                        Token, portfolio_leg_value), 2);
                                    }

                                    if (leg_Details.IsTargetProfitEnable == true)
                                    {
                                        portfolio_leg_value.TargetProfit = Math.Round(algoCalculation.GetLegTargetProfit(leg_Details.SettingTargetProfit,
                                                                                                                            leg_Details.OptionType,
                                                                                                                            leg_Details.Position,
                                                                                                                            leg_Details.TargetProfit,
                                                                                                                            leg_Details.SelectSegment,
                                                                                                                            stg_setting_value.Index,
                                                                                                                            Token, portfolio_leg_value), 2);
                                    }
                                    double _currentLTP = algoCalculation.GetStrikePriceLTP(Token);



                                    //Place the Order Using NNAPI 
                                    int OrderID = OrderManagerModel.GetOrderId();//Get the client unique ID
                                    OrderManagerModel.Portfolio_Dicc_By_ClientID.TryAdd(OrderID, portfolio_leg_value);
                                    LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)Token, price1: _currentLTP, orderQty: portfolio_leg_value.Qty,
                                         Buysell: portfolio_leg_value.BuySell, OrderType.LIMIT, 0, OrderID); //Here the orderID and the StgID both are Same . will use same stg id in other updation
                                                                                                             //GUI
                                    portfolio_leg_value.STG_ID = OrderID;
                                    portfolio_leg_value.EntryPrice = _currentLTP;
                                    portfolio_leg_value.Status = EnumStrategyStatus.OrderPlaced;
                                    portfolio_leg_value.EntryTime = DateTime.Now;

                                    TotalPremium += (portfolio_leg_value.Qty * portfolio_leg_value.EntryPrice);


                                    General.PortfolioLegByTokens.AddOrUpdate(Token, new List<InnerObject>() { portfolio_leg_value }, (key, list) =>
                                    {
                                        list.Add(portfolio_leg_value);
                                        return list;
                                    });


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
                        //StopLoss
                        Portfolio_value.TotalEntryPremiumPaid = TotalPremium;
                        Portfolio_value.InitialMTM = Portfolio_value.MTM;

                        await Task.Delay(1000);
                        if (stg_setting_value.IsOverallStopLossEnable)
                        {
                            Portfolio_value.StopLoss = Math.Round(algoCalculation.GetOverallStopLossValue(TotalPremium,
                                                                                                            Portfolio_value.MTM,
                                                                                                            stg_setting_value.SettingOverallStopLoss,
                                                                                                            stg_setting_value.OverallStopLoss)
                                                                                                             , 2);

                            Portfolio_value.ReEntrySL = stg_setting_value.OverallReEntryOnSL;
                        }
                        //TP
                        if (stg_setting_value.IsOverallTargetEnable)
                        {
                            Portfolio_value.TargetProfit = Math.Round(algoCalculation.GetOverallTargetProfitValue(TotalPremium,
                                                                                                           Portfolio_value.MTM,
                                                                                                           stg_setting_value.SettingOverallTarget,
                                                                                                           stg_setting_value.OverallTarget)
                                                                                                            , 2);

                            Portfolio_value.ReEntryTP = stg_setting_value.OverallReEntryOnTgt;
                        }

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
                                if ((Portfolio_value.SellTradedQty - Portfolio_value.BuyTradedQty) != 0)
                                    await SquareOffStraddle920(Portfolio_value);
                            }
                        }

                    }));
                }

            }
            catch (Exception ex)
            {
                logFileWriter.WriteLog(EnumLogType.Error, ex.Message + "  " + ex.StackTrace);
            }
        }



        /// <summary>
        ///  Make Re-Entry for the Order
        /// </summary>
        /// <param name="SL_HIT"></param>
        /// <param name="TP_HIT"></param>
        /// <param name="portfolio_leg_value"></param>
        /// <param name="leg_Details"></param>
        /// <param name="stg_setting_value"></param>
        /// <param name="Portfolio_value"></param>
        /// <param name="leg_value"></param>
        private async void ReEntryAndPlaceOrderCommon(bool SL_HIT, bool TP_HIT, InnerObject portfolio_leg_value, LegDetails leg_Details, StrategyDetails stg_setting_value, PortfolioModel Portfolio_value, ConcurrentDictionary<string, LegDetails> leg_value)
        {
            try
            {
                InnerObject innerObject = null;
                //check re-entry of Leg {Place =>if true}
                if (leg_Details.IsReEntryOnSLEnable == true && SL_HIT)
                {
                    if (portfolio_leg_value.ReEntrySL > 0)
                    {
                        portfolio_leg_value.ReEntrySL--;
                        //CODE HERE
                        innerObject = algoCalculation.GetLegDetailsForRentry_SLHIT(leg_Details, portfolio_leg_value, stg_setting_value);

                        //calculate new StopLoss For Leg

                    }
                }
                if (leg_Details.IsReEntryOnTgtEnable == true && TP_HIT)
                {
                    if (portfolio_leg_value.ReEntryTP > 0)
                    {
                        portfolio_leg_value.ReEntryTP--;
                        //CODE HERE
                        innerObject = algoCalculation.GetLegDetailsForRentry_TPHIT(leg_Details, portfolio_leg_value, stg_setting_value);
                    }
                }

                if (innerObject != null)
                {
                    leg_value.TryAdd(innerObject.Name, leg_Details);
                    General.PortfolioLegByTokens.AddOrUpdate(innerObject.Token, new List<InnerObject>() { innerObject }, (key, list) =>
                    {
                        list.Add(innerObject);
                        return list;
                    });
                    await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Portfolio_value.InnerObject.Add(innerObject);
                    }), DispatcherPriority.Background, null);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    General.Portfolios.TryUpdate(Portfolio_value.Name, Portfolio_value, General.Portfolios[Portfolio_value.Name]);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    //Place the Order Using NNAPI 
                    int OrderID = OrderManagerModel.GetOrderId();//Get the client unique ID
                    OrderManagerModel.Portfolio_Dicc_By_ClientID.TryAdd(OrderID, innerObject);
                    if (leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.RECOST || leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.REREVCOST
                    || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.RECOST || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.REREVCOST)
                    {
                        //wait for the Price and then Fire
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Task.Run(new Action(async () =>
                        {
                            innerObject.Message = "Wating for Price " + innerObject.EntryPrice;
                            var data = await algoCalculation.IsMyPriceHITforCost(SL_HIT, TP_HIT, innerObject.EntryPrice, innerObject.Token);
                            if (data == true)
                            {
                                LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)innerObject.Token, price1: innerObject.EntryPrice, orderQty: innerObject.Qty,
                            Buysell: innerObject.BuySell, OrderType.LIMIT, 0, OrderID);
                                innerObject.Status = EnumStrategyStatus.OrderPlaced;
                            }
                        }));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }
                    else if (leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.REASAP || leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.REREVASAP
                    || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.REASAP || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.REREVASAP)
                    {
                        LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)innerObject.Token, price1: innerObject.EntryPrice, orderQty: innerObject.Qty,
                             Buysell: innerObject.BuySell, OrderType.LIMIT, 0, OrderID);
                        innerObject.Status = EnumStrategyStatus.OrderPlaced;
                    }
                    else if (leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.REMOMENTUM || leg_Details.SettingReEntryOnSL == EnumLegReEntryOnSL.REREVMOMENTUM
                    || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.REMOMENTUM || leg_Details.SettingReEntryOnTgt == EnumLegReEntryOnTarget.REREVMOMENTUM)
                    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        if (leg_Details.IsSimpleMomentumEnable == false)
                        {
                            LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)innerObject.Token, price1: innerObject.EntryPrice, orderQty: innerObject.Qty,
                            Buysell: innerObject.BuySell, OrderType.LIMIT, 0, OrderID);
                            innerObject.Status = EnumStrategyStatus.OrderPlaced;
                        }
                        else
                        {
                            Task.Run(new Action(async () =>
                            {
                                innerObject = algoCalculation.IsSimpleMovementumHitForRentry(innerObject, leg_Details, stg_setting_value);

                                LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)innerObject.Token, price1: innerObject.EntryPrice, orderQty: innerObject.Qty,
                        Buysell: innerObject.BuySell, OrderType.LIMIT, 0, OrderID);
                                innerObject.Status = EnumStrategyStatus.OrderPlaced;

                            }));
                        }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }

                    innerObject.STG_ID = OrderID;

                    innerObject.EntryTime = DateTime.Now;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    General.PortfolioLegByTokens.AddOrUpdate(innerObject.Token, new List<InnerObject>() { innerObject }, (key, list) =>
                    {
                        list.Add(innerObject);
                        return list;
                    });
                }
            }
            catch (Exception ex)
            {
                logFileWriter.WriteLog(EnumLogType.Error, ex.StackTrace + ex.Message);
            }
        }


        #endregion


        #region SquareOff Straddle 
        /// <summary>
        /// Partial Square off using Straddle #0920 using strategy key and Leg Key
        /// </summary>
        /// <param name="portfolio_leg_value"></param>
        /// <returns></returns>
        private async Task SquareOffStraddle920Leg(InnerObject portfolio_leg_value, EnumStrategyStatus enumStrategyStatus = EnumStrategyStatus.None)
        {
            if (portfolio_leg_value.ExitPrice == 0)
            {
                try
                {
                    double _currentLTP = algoCalculation.GetStrikePriceLTP(portfolio_leg_value.Token);
                    EnumPosition enumPosition = portfolio_leg_value.BuySell == EnumPosition.BUY ? EnumPosition.SELL : EnumPosition.BUY;
                    int OrderID = OrderManagerModel.GetOrderId();//Get the client unique ID
                    OrderManagerModel.Portfolio_Dicc_By_ClientID.TryAdd(OrderID, portfolio_leg_value);
                    LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)portfolio_leg_value.Token, price1: _currentLTP, orderQty: portfolio_leg_value.Qty,
                         Buysell: enumPosition, OrderType.LIMIT, 0, portfolio_leg_value.STG_ID);
                    portfolio_leg_value.ExitPrice = _currentLTP;
                    portfolio_leg_value.ExitTime = DateTime.Now;
                    if (enumStrategyStatus != EnumStrategyStatus.None)
                    {
                        portfolio_leg_value.Status = enumStrategyStatus;
                    }


                }
                catch (Exception ex)
                {
                    logFileWriter.DisplayLog(EnumLogType.Error, " Something went wrong while Squared off the Leg with TOKEN: " + portfolio_leg_value.Token + ex.Message + ex.StackTrace);

                }
            }
        }
        /// <summary>
        /// Strategy Square off using Strategy Key
        /// </summary>
        /// <param name="KeyOfStraddle"></param>
        /// <returns></returns>
        public async Task<bool> SquareOffStraddle920(PortfolioModel PM, EnumStrategyStatus enumStrategyStatus = EnumStrategyStatus.None)
        {

            var _totalLeg = PM.InnerObject;
            foreach (var leg in _totalLeg)
            {
                if (leg.ExitPrice == 0)
                {
                    if (enumStrategyStatus == EnumStrategyStatus.None)
                        await SquareOffStraddle920Leg(leg);
                    else
                        await SquareOffStraddle920Leg(leg, enumStrategyStatus);


                    await Task.Delay(50);
                }
            }


            return true;
        }

        #endregion

        #region INIT>>>
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
        /// Load the Config Data From Master to Portfolio and inner Object .
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
                        portfolioModel.InnerObject ??= new();

                        //ADD in Portfolio for GUI
                        if (!General.Portfolios.ContainsKey(portfolioModel.Name))
                        {
                            General.Portfolios.TryAdd(portfolioModel.Name, portfolioModel);
                            if (PortfolioViewModel.StrategyDataCollection == null)
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
                                        innerObject.StgName = stg_key;
                                        innerObject.Name = Leg;
                                        innerObject.BuySell = leg_value.Position;
                                        innerObject.Status = EnumStrategyStatus.Added;
                                        innerObject.TradingSymbol = "Loading ...";
                                        innerObject.Qty = leg_value.Lots;
                                        innerObject.enumUnderlyingFromForLeg = stg_value.UnderlyingFrom;
                                        await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {

                                            portfolioModel.InnerObject.Add(innerObject);

                                        }), DispatcherPriority.Background, null);

                                        //ADD TO GUI
                                        General.Portfolios.TryUpdate(portfolioModel.Name, portfolioModel, General.Portfolios[portfolioModel.Name]);
                                        if (PortfolioViewModel.StrategyDataCollection == null)
                                            throw new Exception("THE PortFolio VIEW -> MODEL not initiated");
                                    }
                                    catch (Exception ex)
                                    {
                                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                                    }
                                }
                                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {

                                    PortfolioViewModel.StrategyDataCollection.Add(portfolioModel);

                                }), DispatcherPriority.Background, null);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logFileWriter.WriteLog(EnumDeclaration.EnumLogType.Error, ex.ToString());
                    }

                }
                StartMonitoringCommand(); // Thread to watch
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
            General.PortfolioLegByTokens ??= new();
            if (straddleDataBaseLoad.Master_Straddle_Dictionary == null || straddleDataBaseLoad.Straddle_LegDetails_Dictionary == null)
                throw new Exception("Master dic not loadded posibility The Stg file not loaded.");

            try
            {
                foreach (string stg_key in straddleDataBaseLoad.Master_Straddle_Dictionary.Keys)
                {//ALL STG
                    double TotalPremium = 0;

                    var Papa = Task.Factory.StartNew((Action)(async () =>
                    {
                        List<Task> tasks = new();
                        var stg_setting_value = straddleDataBaseLoad.Master_Straddle_Dictionary[stg_key];
                        var Portfolio_value = General.Portfolios[stg_key];
                        //waiting for Entry Time
                        if (stg_setting_value.EntryTime >= DateTime.Now)
                        {
                            int milisecond = (int)(stg_setting_value.EntryTime - DateTime.Now).TotalMilliseconds;
                            await Task.Delay(milisecond);



                            //
                            //GUI 
                            var leg_value = straddleDataBaseLoad.Straddle_LegDetails_Dictionary[stg_key];
                            foreach (string Leg in leg_value.Keys)
                            {//ALL LEG

                                var bacha = Task.Factory.StartNew(async () =>
                                {
                                    var leg_Details = leg_value[Leg];
                                    var portfolio_leg_value = Portfolio_value.InnerObject.Where(xxx => xxx.Name == Leg).FirstOrDefault() ?? throw new Exception("Leg was not Loaded in GUI or Portfolios.");
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

                                        uint Token = EnumSegments.OPTIONS == leg_Details.SelectSegment ? ContractDetails.GetTokenByContractValue(Expiry, leg_Details.OptionType, stg_setting_value.Index, StrikeForLeg) :
                                    ContractDetails.GetTokenByContractValue(Expiry, EnumOptiontype.XX, stg_setting_value.Index);
                                        string TradingSymbol = ContractDetails.GetContractDetailsByToken(Token).TrdSymbol ?? throw new Exception("for " + Token + " Trading Symbol was not Found in Contract Details.");



                                        portfolio_leg_value.Qty *= (int)ContractDetails.GetContractDetailsByToken(Token).LotSize;
                                        //Porfolio leg Update
                                        portfolio_leg_value.Token = Token;
                                        portfolio_leg_value.TradingSymbol = TradingSymbol;
                                        portfolio_leg_value.Status = EnumStrategyStatus.Waiting;
                                        portfolio_leg_value.ReEntrySL = leg_Details.ReEntryOnSL;
                                        portfolio_leg_value.ReEntryTP = leg_Details.ReEntryOnTgt;




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
                                                                                                       Token, portfolio_leg_value);
                                        }
                                        if (leg_Details.IsStopLossEnable == true)
                                        {
                                            portfolio_leg_value.StopLoss = Math.Round(algoCalculation.GetLegStopLoss(leg_Details.SettingStopLoss,
                                                                                                            leg_Details.OptionType,
                                                                                                            leg_Details.Position,
                                                                                                            leg_Details.StopLoss,
                                                                                                            leg_Details.SelectSegment,
                                                                                                            stg_setting_value.Index,
                                                                                                            Token, portfolio_leg_value), 2);
                                        }

                                        if (leg_Details.IsTargetProfitEnable == true)
                                        {
                                            portfolio_leg_value.TargetProfit = Math.Round(algoCalculation.GetLegTargetProfit(leg_Details.SettingTargetProfit,
                                                                                                                                leg_Details.OptionType,
                                                                                                                                leg_Details.Position,
                                                                                                                                leg_Details.TargetProfit,
                                                                                                                                leg_Details.SelectSegment,
                                                                                                                                stg_setting_value.Index,
                                                                                                                                Token, portfolio_leg_value), 2);
                                        }
                                        double _currentLTP = algoCalculation.GetStrikePriceLTP(Token);



                                        //Place the Order Using NNAPI 
                                        int OrderID = OrderManagerModel.GetOrderId();//Get the client unique ID
                                        OrderManagerModel.Portfolio_Dicc_By_ClientID.TryAdd(OrderID, portfolio_leg_value);
                                        LoginViewModel.NNAPIRequest.PlaceOrderRequest((int)Token, price1: _currentLTP, orderQty: portfolio_leg_value.Qty,
                                             Buysell: portfolio_leg_value.BuySell, OrderType.LIMIT, 0, OrderID); //Here the orderID and the StgID both are Same . will use same stg id in other updation
                                        //GUI
                                        portfolio_leg_value.STG_ID = OrderID;
                                        portfolio_leg_value.EntryPrice = _currentLTP;
                                        portfolio_leg_value.Status = EnumStrategyStatus.OrderPlaced;
                                        portfolio_leg_value.EntryTime = DateTime.Now;

                                        TotalPremium += (portfolio_leg_value.Qty * portfolio_leg_value.EntryPrice);



                                        //Bind to Dic responsibile for Feed load ....
                                        //if (General.PortfolioLegByTokens.TryGetValue(Token, out List<InnerObject> value))
                                        //{
                                        //    var legs = value;
                                        //    legs.Add(portfolio_leg_value);
                                        //    General.PortfolioLegByTokens[Token] = legs;
                                        //}
                                        //else
                                        //{
                                        //    List<InnerObject> legs = new()
                                        //    {
                                        //        portfolio_leg_value
                                        //    };
                                        //    General.PortfolioLegByTokens.TryAdd(Token, legs);
                                        //}
                                        // below function is more safe as above is missed to add in case of multi thread
                                        General.PortfolioLegByTokens.AddOrUpdate(Token, new List<InnerObject>() { portfolio_leg_value }, (key, list) =>
                                             {
                                                 list.Add(portfolio_leg_value);
                                                 return list;
                                             });


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
                            //StopLoss
                            Portfolio_value.TotalEntryPremiumPaid = TotalPremium;
                            Portfolio_value.InitialMTM = Portfolio_value.MTM;

                            await Task.Delay(1000);
                            if (stg_setting_value.IsOverallStopLossEnable)
                            {
                                Portfolio_value.StopLoss = Math.Round(algoCalculation.GetOverallStopLossValue(TotalPremium,
                                                                                                                Portfolio_value.MTM,
                                                                                                                stg_setting_value.SettingOverallStopLoss,
                                                                                                                stg_setting_value.OverallStopLoss)
                                                                                                                 , 2);

                                Portfolio_value.ReEntrySL = stg_setting_value.OverallReEntryOnSL;
                            }
                            //TP
                            if (stg_setting_value.IsOverallTargetEnable)
                            {
                                Portfolio_value.TargetProfit = Math.Round(algoCalculation.GetOverallTargetProfitValue(TotalPremium,
                                                                                                               Portfolio_value.MTM,
                                                                                                               stg_setting_value.SettingOverallTarget,
                                                                                                               stg_setting_value.OverallTarget)
                                                                                                                , 2);

                                Portfolio_value.ReEntryTP = stg_setting_value.OverallReEntryOnTgt;
                            }

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
                                    if ((Portfolio_value.SellTradedQty - Portfolio_value.BuyTradedQty) != 0)
                                        await SquareOffStraddle920(Portfolio_value);
                                }
                            }
                        }
                        else
                        {
                            logFileWriter.DisplayLog(EnumLogType.Info, "Can not placed the Streatgy. Time is Already passed NameOfThe Stg : " + stg_key.ToString());
                        }

                    }));
                }

            }
            catch (Exception ex)
            {
                logFileWriter.WriteLog(EnumLogType.Error, ex.ToString());
            }
        }

        #endregion
        #endregion
    }
}
