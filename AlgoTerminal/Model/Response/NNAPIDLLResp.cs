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
      
        public void ChangePasswordResponse(bool Success, int UserID, int NewPassword, string MessageText)
        {
            throw new NotImplementedException();
        }

        public void EndGetPosition()
        {
            throw new NotImplementedException();
        }

        public void EndOpenOrderHistory()
        {
            throw new NotImplementedException();
        }

        public void EndOrderHistory()
        {
            throw new NotImplementedException();
        }

        public void EndTradeHistory()
        {
            throw new NotImplementedException();
        }

        public void ErrorNotification(string ErrorDescription)
        {
            
           LoginViewModel.login.ErrorResponse(ErrorDescription);
            if (ErrorDescription.Contains("Unknown Message Received. Message Header: 0"))
                FeedCB_C._dashboard._connected = false;
        }

        public void GetPosition(int Token, int BuyTradedQty, long BuyTradedValue, int SellTradedQty, long SellTradedValue)
        {
            throw new NotImplementedException();
        }

        public void LoginResponse(bool LoginSuccess, string MessageText)
        {
           LoginViewModel.login.LoginResponse(LoginSuccess, MessageText);
        }

        public void OpenOrderHistory(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType, string Time, string Status, string StatusDetail, string RejectionReason)
        {
            throw new NotImplementedException();
        }

        public void OrderConfirmation(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType, string Time, string Status, string StatusDetail, string RejectionReason)
        {
            throw new NotImplementedException();
        }

        public void OrderHistory(int Token, int Price, int Qty, int PendingQty, string BuySell, long AdminOrderID, ulong ExchOrdId, int iUserData, string StrUserData, int TriggerPrice, int TradedQty, long TradedValue, string OrderType, string Time, string Status, string StatusDetail, string RejectionReason)
        {
            throw new NotImplementedException();
        }

        public void ReceiveOrderType(string Description, string DefaultValue, List<string> lstOrderTypes)
        {
            throw new NotImplementedException();
        }

        public void StartGetPosition()
        {
            throw new NotImplementedException();
        }

        public void StartOpenOrderHistory()
        {
            throw new NotImplementedException();
        }

        public void StartOrderHistory()
        {
            throw new NotImplementedException();
        }

        public void StartTradeHistory()
        {
            throw new NotImplementedException();
        }

        public void Trade(int Token, int TradeQty, int TradePrice, string BuySell, int TradeID, ulong ExchOrdId, long AdminOrderID, string TradeTime, int iUserData, string StrUserData)
        {
            throw new NotImplementedException();
        }

        public void TradeHistory(int Token, int TradeQty, int TradePrice, string BuySell, int TradeID, ulong ExchOrdId, long AdminOrderID, string TradeTime, int iUserData, string StrUserData)
        {
            throw new NotImplementedException();
        }
    }
}
