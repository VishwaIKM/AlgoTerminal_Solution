using System.Collections.ObjectModel;
using AlgoTerminal.Model.Structure;
using AlgoTerminal.Model.Services;
using Microsoft.VisualBasic;
using System.Linq;

namespace AlgoTerminal.ViewModel
{
    public class LoggerViewModel : DockWindowViewModel
    {
        private static ObservableCollection<LoggerModel>? LogGrid;
        private readonly ILogFileWriter _LogFileWriter;
        public LoggerViewModel(ILogFileWriter logFileWriter)
        {
            LogDataCollection ??= new();
            _LogFileWriter = logFileWriter;
            _LogFileWriter.Start(@"D:\Development Vishwa\AlgoTerminal_Solution\UnitTest_Resources", "Log.txt");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Warning, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Error, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Buy, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Success, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");

            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
            _LogFileWriter.DisplayLog(EnumDeclaration.EnumLogType.Info, "Testing");
        }


        public static ObservableCollection<LoggerModel>? LogDataCollection
        {
            get { return LogGrid; }
            set { LogGrid = value;}
        }
    
    }

}

