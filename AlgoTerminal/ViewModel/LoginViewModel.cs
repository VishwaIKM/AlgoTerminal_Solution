using AlgoTerminal.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.View;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.StrategySignalManager;
using AlgoTerminal.Model.Request;
using System.Windows.Threading;

namespace AlgoTerminal.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {
        #region Members and Inst.
        public static NNAPIRequest nnapi = new NNAPIRequest();
        LoginModel _login = new();
        private bool _isloginvisibile = true;
        private string _loginStatusGUILbl;
        private readonly DashboardView dashboardView1;
        private readonly IApplicationManagerModel applicationManagerModel;
        #endregion

        #region Properties
        public LoginModel UserDetails
        {
            get => _login;
            set { _login = value; }
        }
        public int? UserID
        {
            get => UserDetails.UserID;
            set
            {
                if (UserDetails.UserID != value)
                {
                    UserDetails.UserID = value;
                    RaisePropertyChanged(nameof(UserID));
                }
            }
        }
        public int? Password
        {
            get => UserDetails.Password;
            set
            {
                if (UserDetails.Password != value)
                {
                    UserDetails.Password = value;
                    RaisePropertyChanged(nameof(Password));
                }
            }
        }
        public bool IsLoginButtonEnable
        {
            get => _isloginvisibile;
            set
            {
                if (_isloginvisibile != value)
                {
                    _isloginvisibile = value;
                    RaisePropertyChanged(nameof(IsLoginButtonEnable));
                }
            }
        }
        public string LoginStatusGUILbl
        {
            get => _loginStatusGUILbl;
            set
            {
                if (_loginStatusGUILbl != value)
                {
                    _loginStatusGUILbl = value;
                    RaisePropertyChanged(nameof(LoginStatusGUILbl));
                }
            }
        }
        #endregion

        #region Methods
        public LoginViewModel(DashboardView dashboardView1,IApplicationManagerModel applicationManagerModel)
        {
            this.dashboardView1 = dashboardView1;
            this.applicationManagerModel = applicationManagerModel;

            bool a = Convert.ToBoolean(nnapi.InitializeServer());
            if (!a)
                MessageBox.Show("Not able to connect the server.");

        }

        #endregion

        #region ICommand BUTTON Method 

        public void LoginResponse(bool loginSuccess, string messageText)
        {
           
            Application.Current.Dispatcher.BeginInvoke(new Action(async () => 
            {
                if (loginSuccess)
                {
                    dashboardView1.Show();
                    App.Current.MainWindow.Close();
                    App.Current.MainWindow = dashboardView1;

                    await applicationManagerModel.ApplicationStartUpRequirement();
                }
                else
                    MessageBox.Show(messageText);

            }), 
            DispatcherPriority.Background, 
            null);
           
        }
        async void LoginCommandMethodExcute()
        {
            IsLoginButtonEnable = false;
            try
            {

                nnapi.LoginRequest();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Not Able to Connect SQL Server. " + ex.ToString());
                IsLoginButtonEnable = true;
            }


            IsLoginButtonEnable = true;
        }
        void ExitCommandMethodExcute()
        {
            Application.Current.Shutdown();
        }
        bool CanThisMethodExecute() { return true; }

        #endregion

        #region ICommand
        public ICommand LoginCommand => new RelayCommand2(LoginCommandMethodExcute, CanThisMethodExecute);
        public ICommand ExitAppCommand => new RelayCommand2(ExitCommandMethodExcute, CanThisMethodExecute);




        #endregion
    }
}
