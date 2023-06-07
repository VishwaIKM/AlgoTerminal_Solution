using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using AlgoTerminal.View;
using AlgoTerminal.Model;
using AlgoTerminal.ViewModel;
using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Calculation;
using AlgoTerminal_Base.FileManager;
using AlgoTerminal_Base.Request;
using AlgoTerminal_Base.StrategySignalManager;
using AlgoTerminal_Base.Response;

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
                    services.AddSingleton<IFeed,Feed>();
                    services.AddSingleton<IGeneral, General>();
                    services.AddSingleton<ILogFileWriter, LogFileWriter>();
                    services.AddSingleton<IRespNNAPI, NNAPIDLLResp>();
                    services.AddSingleton<IStraddleDataBaseLoadFromCsv, StraddleDataBaseLoadFromCsv>();
                    services.AddSingleton<IStraddleManager,StraddleManager>();

                    //ViewModel....
                    services.AddSingleton<DashboardViewModel>();
                    services.AddSingleton<PortfolioViewModel>();
                   
                    //View ....
                    services.AddSingleton<DashboardView>(x => new()
                    {
                        DataContext = x.GetRequiredService<DashboardViewModel>()
                    });
                    services.AddSingleton<PortfolioView>(x => new()
                    {
                        DataContext = x.GetRequiredService<PortfolioViewModel>()
                    });
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var _runTheWPF = AppHost!.Services.GetRequiredService<PortfolioView>();
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
