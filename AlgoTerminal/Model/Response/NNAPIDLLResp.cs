using AlgoTerminal.Model.FileManager;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.StrategySignalManager;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.ViewModel;
using System;
using System.Collections.Generic;

namespace AlgoTerminal.Model.Response
{
    public class NNAPIDLLResp : IRespNNAPI
    {
        private readonly ILogFileWriter _logFileWriter;
        public NNAPIDLLResp(ILogFileWriter logFileWriter)
        {
            _logFileWriter = logFileWriter;
        }
        public void ChangePasswordResponse(bool Success, int UserID, int NewPassword, string MessageText)
        {
            // throw new NotImplementedException();
        }

        public void EndGetPosition()
        {
           // throw new NotImplementedException();
        }

        public void EndOpenOrderHistory()
        {
            //throw new NotImplementedException();
        }

        public void EndOrderHistory()
        {
            //throw new NotImplementedException();
        }

        public void EndTradeHistory()
        {
            //throw new NotImplementedException();
        }

        public void ErrorNotification(string ErrorDescription)
        {
            
           LoginViewModel.login.ErrorResponse(ErrorDescription);
            if (ErrorDescription.Contains("Unknown Message Received. Message Header: 0"))
                FeedCB_C._dashboard._connected = false;
        }

        public void GetPosition(int Token, int BuyTradedQty, long BuyTradedValue, int SellTradedQty, long SellTradedValue)
        {
            //throw new NotImplementedException();
        }

        public void LoginResponse(bool LoginSuccess, string MessageText)
        {
           LoginViewModel.login.LoginResponse(LoginSuccess, MessageText);
        }

        public void OpenOrderHistory(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType, string Time, string Status, string StatusDetail, string RejectionReason)
        {
            //throw new NotImplementedException();
        }

        public void OrderConfirmation(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, 
            ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType,
            string Time, string Status, string StatusDetail, string RejectionReason)
        {
            try
            {
                var contract = ContractDetails.GetContractDetailsByToken((uint)Token);
                if (contract == null)
                    throw new Exception("Did not Find Token Details in Contract File. = " +Token);
                //Two Things Need to Update Order=>forManual Order Validation as well as Portfolio Screen

                //OrderBook will remain common for auto order as well as manual order.
                #region OrderBook Managment
                if (OrderManagerModel.OrderBook_Dicc_By_ClientID.ContainsKey(iUserData))
                {
                    OrderBookModel orderBookModel = OrderManagerModel.OrderBook_Dicc_By_ClientID[iUserData];
                    orderBookModel.OrderQty = Qty;
                    orderBookModel.TradedQty = Qty - PendingQty;
                    orderBookModel.TriggerPrice = TriggerPrice;
                    orderBookModel.ModeratorID = AdminOrderID;
                    orderBookModel.ExchangeID = ExchOrdId;
                    orderBookModel.UpdateTime = Time;
                    orderBookModel.RejectionReason = RejectionReason;


                    if(!(Status == "modify" || Status == "modified" || Status == "open" || Status == "open pending" 
                        || Status == "Trigger Pending" || Status == "put order request") && RejectionReason.Contains("NSE Error"))
                    {
                        if (OrderBookViewModel.OpenOrderBook.Contains(orderBookModel))
                            OrderBookViewModel.OpenOrderBook.Remove(orderBookModel);

                        if(!OrderBookViewModel.CloseOrderBook.Contains(orderBookModel))
                            OrderBookViewModel.CloseOrderBook.Add(orderBookModel);
                    }
                  
                }
                else
                {
                    OrderBookModel orderBookModel = new();
                    orderBookModel.ClientID = iUserData;
                    orderBookModel.Status = Status;
                    orderBookModel.TradingSymbol = contract.TrdSymbol;
                    orderBookModel.BuySell = BuySell == "B" ? EnumDeclaration.EnumPosition.BUY : EnumDeclaration.EnumPosition.SELL;
                    orderBookModel.Price = Price;
                    orderBookModel.OrderQty = Qty;
                    orderBookModel.TradedQty = Qty- PendingQty;
                    orderBookModel.TriggerPrice = TriggerPrice;
                    orderBookModel.ModeratorID = AdminOrderID;
                    orderBookModel.ExchangeID = ExchOrdId;
                    orderBookModel.UpdateTime = Time;
                    orderBookModel.RejectionReason = RejectionReason;

                    //Add to ViewModel Obser
                    OrderManagerModel.OrderBook_Dicc_By_ClientID.TryAdd(iUserData, orderBookModel);
                    OrderBookViewModel.OpenOrderBook.Add(orderBookModel);

                }

                #endregion
                
            }
            catch(Exception ex) { _logFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Check Log! Recived Unhandle Error in OrderConfirmation from Moderatot for TOKEN: " + Token); }

        }

        public void OrderHistory(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType, string Time, string Status, string StatusDetail, string RejectionReason)
        {
            //throw new NotImplementedException();
        }

        public void ReceiveOrderType(string Description, string DefaultValue, List<string> lstOrderTypes)
        {
           // throw new NotImplementedException();
        }

        public void StartGetPosition()
        {
           // throw new NotImplementedException();
        }

        public void StartOpenOrderHistory()
        {
            //throw new NotImplementedException();
        }

        public void StartOrderHistory()
        {
            //throw new NotImplementedException();
        }

        public void StartTradeHistory()
        {
           // throw new NotImplementedException();
        }

        public void Trade(int Token, int TradeQty, int TradePrice, string BuySell, int TradeID, ulong ExchOrdId, long AdminOrderID, string TradeTime, int iUserData, string StrUserData)
        {
            try
            {
                //add to tradeBookViewModel from MOD resp
                TradeBookModel tradeBookModel = new();
                var cc = ContractDetails.GetContractDetailsByToken((uint)Token);
                tradeBookModel.TradingSymbol = cc.TrdSymbol;
                tradeBookModel.Time = TradeTime;
                tradeBookModel.Quantity = TradeQty;
                tradeBookModel.Price = TradePrice;
                tradeBookModel.BuySell = BuySell == "B" ? EnumDeclaration.EnumPosition.BUY : EnumDeclaration.EnumPosition.SELL;
                tradeBookModel.OptionType = cc.Opttype;
                tradeBookModel.Strike = cc.Strike;
                tradeBookModel.Expiry = cc.Expiry;
                tradeBookModel.ClientId = iUserData;
                tradeBookModel.TradeID = TradeID;
                tradeBookModel.ModeratorID = AdminOrderID;
                tradeBookModel.ExchnageID = ExchOrdId;
                TradeBookViewModel.TradeDataCollection ??= new ();
                TradeBookViewModel.TradeDataCollection.Add(tradeBookModel);


                //Portfolio Screen only process automated order. Manaul Order should not be processed in Porfolio as they are not part of STg.
                if (OrderManagerModel.Portfolio_Dicc_By_ClientID.ContainsKey(iUserData))
                {
                    var leg_Details = OrderManagerModel.Portfolio_Dicc_By_ClientID[iUserData];
                    leg_Details.Status = EnumDeclaration.EnumStrategyStatus.Running;

                }
            }
            catch (Exception ex)
            {

            }
           

        }

        public void TradeHistory(int Token, int TradeQty, int TradePrice, string BuySell, int TradeID, ulong ExchOrdId, long AdminOrderID, string TradeTime, int iUserData, string StrUserData)
        {
            //throw new NotImplementedException();
        }
    }
}
