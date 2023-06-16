﻿using AlgoTerminal.Model.NNAPI;
using AlgoTerminal.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.Model.Request
{
    public class NNAPIRequest
    {
                #region Members
        static readonly object _placeOrderLock = new object();
        static readonly object _modifyOrderLock = new object();
        static readonly object _cancelOrderLock = new object();
        #endregion

        #region instances
        private readonly IRespNNAPI Resp;
        private readonly NNAPI.NNAPI Nnapi; // = new NNAPI.NNAPI(S_ResponseObj);
        #endregion
        public NNAPIRequest(IRespNNAPI Resp)
        { 
            this.Resp = Resp;
            Nnapi ??= new(this.Resp);
        }
        #region Methods
        /// <summary>
        /// TO Connect with Server With parm IP and Port Required
        /// </summary>
        public int InitializeServer()
        {
            return 0;
            //return Nnapi.Init(Config.S_Moderator_IP, Config.S_Moderator_Port);
        }


        /// <summary>
        /// Login Request To API
        /// USER ID (INT)
        /// PSSWOARD (INT)
        /// </summary>
        public void LoginRequest()
        {
           // Nnapi.Login(LoginVM.S_User_Id, LoginVM.S_Password);

        }


        /// <summary>
        /// Log out --> Close Connection from server and abort the thread.
        /// </summary>
        public void LogOutRequest()
        {
            Nnapi.Logout();
        }


        /// <summary>
        /// PLACE ORDER TO MODRATOR THROUGH THE NNAPI
        /// </summary>
        /// <param name="OrderType"></param>
        /// <param name="tokenId"></param>
        /// <param name="price"></param>
        /// <param name="orderQty"></param>
        /// <param name="transType"></param>
        /// <param name="orderType"></param>
        /// <param name="triggerPrice"></param>
        /// <param name="marketWatch_OrderID"></param>
        internal void PlaceOrderRequest(string OrderType, int tokenId, int price, int orderQty, TransType transType, OrderType orderType, int triggerPrice, int marketWatch_OrderID,int strUserdata =-1)
        {
            lock (_placeOrderLock)
            {
                if (price > 0 && orderQty > 0)
                {
                    try
                    {
                        Nnapi.PlaceOrder(tokenId, price, orderQty, transType, orderType, triggerPrice, marketWatch_OrderID, strUserdata);

                        //if (transType == TransType.B)
                        //    General.S_Logger.DisplayLog(E_Log_Type.Success, "Order Type :" + orderType + " Order ID :" + marketWatch_OrderID + " TransType :B " +
                        //        " Token Id :" + tokenId + " Price :" + price + " Trigger Price: " + triggerPrice + " has been Placed.");
                        //else
                        //    General.S_Logger.DisplayLog(E_Log_Type.Success, "Order Type :" + orderType + " Order ID :" + marketWatch_OrderID + " TransType :S " +
                        //        " Token Id :" + tokenId + " Price :" + price + " Trigger Price: " + triggerPrice + " has been Placed.");
                    }
                    catch (Exception e)
                    {
                        //General.S_Logger.DisplayLog(E_Log_Type.Error, "Unable to Place Order To Server");
                        //General.S_Logger.WriteLog(E_Log_Type.Error, e.ToString());
                    }
                }
                else
                {
                    /*General.S_Logger.DisplayLog(E_Log_Type.Error, "Price or Quantity is 0. Order did not placed.");*/
                }

            }
        }


        /// <summary>
        /// Order Histroy Request to get the Previous Record of Order From MOD Server Through the NNAPI
        /// </summary>
        internal void GetOrderHistoryRequest()
        {
            Nnapi.GetOrderHistory();
        }


        /// <summary>
        /// Order OPEN Histroy Request to get the Previous Record of Order From MOD Server Through the NNAPI
        /// </summary>
        internal void GetOpenOrderHistoryRequest() { Nnapi.GetOpenOrderHistory(); }


        /// <summary>
        /// TRADE Histroy Request to get the Previous Record of TRADE From MOD Server Through the NNAPI
        /// </summary>
        internal void GetTradeHistoryRequest() { Nnapi.GetTradeHistory(); }


        /// <summary>
        /// POSITION Request to get the Previous Record of POSITION From MOD Server Through the NNAPI
        /// </summary>
        internal void GetPositionRequest() { Nnapi.GetPosition(); }


        /// <summary>
        /// Request for Cancel Order
        /// </summary>
        /// <param name="_adminOrderId"></param> 
        internal void CancelOrderRequest(long _adminOrderId)
        {
            lock (_cancelOrderLock)
            {
                Nnapi.CancelOrder(_adminOrderId);
                //General.S_Logger.DisplayLog(E_Log_Type.Success, "Cancel Order Send to Modrator with AdminId: " + _adminOrderId);
            }
        }

        /// <summary>
        /// Request For Modify Order
        /// </summary>
        /// <param name="token"></param>
        /// <param name="_adminOrderId"></param>
        /// <param name="_price"></param>
        /// <param name="orderQty"></param>
        /// <param name="transType"></param>
        /// <param name="orderType"></param>
        /// <param name="_triggerPrice"></param>
        internal void ModifyOrderRequest(int token, long _adminOrderId, int _price, int orderQty, TransType transType, OrderType orderType, int _triggerPrice)
        {
            lock (_modifyOrderLock)
            {
                if (_price > 0 || orderQty > 0)
                {
                    Nnapi.ModifyOrder(token, _adminOrderId, _price, orderQty, transType, orderType, _triggerPrice);
                    //General.S_Logger.DisplayLog(E_Log_Type.Success, "Modify Order Send to Modrator with AdminId: " + _adminOrderId + "");
                }
                else
                {
                    //General.S_Logger.DisplayLog(E_Log_Type.Error, "Price or Quantity is 0. Order did not Modify.");
                }
            }
        }

        #endregion
    }
}