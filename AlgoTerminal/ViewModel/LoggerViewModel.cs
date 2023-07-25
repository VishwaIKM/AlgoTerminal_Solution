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
            _LogFileWriter.Start(App.logFilePath, "Log.txt");
            
        }


        public static ObservableCollection<LoggerModel>? LogDataCollection
        {
            get { return LogGrid; }
            set { LogGrid = value;}
        }
    
    }

}

