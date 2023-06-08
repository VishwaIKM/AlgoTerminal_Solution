using AlgoTerminal.FileManager;
using AlgoTerminal.Model;
using AlgoTerminal.Services;
using AlgoTerminal.View;
using AlgoTerminal.ViewModel;
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.FileManager;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.Response;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.StrategySignalManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace AlgoTerminal
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }


        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(x =>
                {
                    x.AddJsonFile("appsettings.json");
                    x.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    //DBContext ...
                    //Model ...
                    services.AddSingleton<ControlCenterModel>();
                    services.AddSingleton<DashboardModel>();
                    services.AddSingleton<PortfolioModel>();
                    //Services ....
                    services.AddSingleton<IAlgoCalculation, AlgoCalculation>();
                    services.AddSingleton<IContractDetails, ContractDetails>();
                    services.AddSingleton<IFeed, Feed>();
                    services.AddSingleton<IGeneral, General>();
                    services.AddSingleton<ILogFileWriter, LogFileWriter>();
                    services.AddSingleton<IRespNNAPI, NNAPIDLLResp>();
                    services.AddSingleton<IStraddleDataBaseLoadFromCsv, StraddleDataBaseLoadFromCsv>();
                    services.AddSingleton<IStraddleManager, StraddleManager>();

                    //ViewModel....
                    services.AddSingleton<DashboardViewModel>();
                    services.AddSingleton<PortfolioViewModel>();
                    services.AddSingleton<LoggerViewModel>();
                    services.AddSingleton<TradeBookViewModel>();
                    services.AddSingleton<NetPositionViewModel>();


                    //View ....
                    services.AddSingleton<DashboardView>(x => new()
                    {
                        DataContext = x.GetRequiredService<DashboardViewModel>()
                    });

                    //USERCONTROL'S
                    services.AddSingleton<PortfolioView>(x => new()
                    {
                        DataContext = x.GetRequiredService<PortfolioViewModel>()
                    });
                    services.AddSingleton<LoggerView>(x => new()
                    {
                        DataContext = x.GetRequiredService<LoggerViewModel>()
                    });
                    services.AddSingleton<TradeBookView>(x => new()
                    {
                        DataContext = x.GetRequiredService<TradeBookViewModel>()
                    });
                    services.AddSingleton<NetPositionView>(x => new()
                    {
                        DataContext = x.GetRequiredService<NetPositionViewModel>()
                    });

                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var _runTheWPF = AppHost!.Services.GetRequiredService<DashboardView>();
            this.MainWindow = _runTheWPF;
            _runTheWPF.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
