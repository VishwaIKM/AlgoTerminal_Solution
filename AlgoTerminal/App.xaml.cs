using AlgoTerminal.Model;
using AlgoTerminal.Model.Calculation;
using AlgoTerminal.Model.FileManager;
using AlgoTerminal.Model.Request;
using AlgoTerminal.Model.Response;
using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.StrategySignalManager;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.View;
using AlgoTerminal.ViewModel;
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
        public static string? straddlePath;

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
                    //Path
                    straddlePath = hostContext.Configuration.GetConnectionString("StraddleFilePath");
                    //DBContext ...
                    //Model ...
                    services.AddSingleton<FeedCB_C>();
                    services.AddSingleton<FeedCB_CM>();
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
                    services.AddSingleton<IApplicationManagerModel,ApplicationManagerModel>();
                    //ViewModel....
                    services.AddSingleton<DashboardViewModel>();
                    services.AddSingleton<PortfolioViewModel>();
                    services.AddSingleton<LoggerViewModel>();
                    services.AddSingleton<TradeBookViewModel>();
                    services.AddSingleton<NetPositionViewModel>();
                    services.AddSingleton<LoginViewModel>();

                    //View ....
                    services.AddSingleton<DashboardView>(x => new()
                    {
                        DataContext = x.GetRequiredService<DashboardViewModel>()
                    });
                    services.AddSingleton<LoginView>(x=>new()
                    {
                        DataContext=x.GetRequiredService<LoginViewModel>()
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

            var _runTheWPF = AppHost!.Services.GetRequiredService<LoginView>();
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
