using AlgoTerminal.Model.Services;
using AlgoTerminal.Model.Structure;
using System.Collections.ObjectModel;

namespace AlgoTerminal.NewViewModel
{
    public class LoggerUcViewModel : GenericViewModel
    {
        private static ObservableCollection<LoggerModel>? LogGrid;
        private readonly ILogFileWriter _LogFileWriter;
        public LoggerUcViewModel(ILogFileWriter logFileWriter)
        {
            LogDataCollection ??= new();
            _LogFileWriter = logFileWriter;
            _LogFileWriter.Start(App.logFilePath, "Log.txt");

        }
        public LoggerUcViewModel()
        {
            Title = "LOGGER";
        }


        public static ObservableCollection<LoggerModel>? LogDataCollection
        {
            get { return LogGrid; }
            set { LogGrid = value; }
        }
    }
}
