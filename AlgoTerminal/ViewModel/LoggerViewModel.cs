using AlgoTerminal_Base.Services;
using System.Collections.Concurrent;
using System;
using System.Data;
using static AlgoTerminal_Base.Structure.EnumDeclaration;
using System.IO;
using System.Threading.Tasks;
using System.Drawing;
using AlgoTerminal.Structure;
using System.Windows.Controls;
using System.Windows.Threading;
using AlgoTerminal.View;
using System.Windows;
using AlgoTerminal.Services;
using System.Collections.ObjectModel;
using AlgoTerminal.Model;
using AlgoTerminal.FileManager;

namespace AlgoTerminal.ViewModel
{
    public class LoggerViewModel : DockWindowViewModel
    {
        private static ObservableCollection<LoggerModel> _Row;
        private ILogFileWriter _LogFileWriter;
        public LoggerViewModel(ILogFileWriter logFileWriter)
        {
            LogDataCollection ??= new();
            _LogFileWriter = logFileWriter;
            _LogFileWriter.Start(@"D:\Development Vishwa\AlgoTerminal_Solution\UnitTest_Resources", "Log.txt");
            _LogFileWriter.DisplayLog(EnumLogType.Buy, "Error");
            LoggerModel loggerModel = new LoggerModel();
            loggerModel.Category = EnumLogType.Warning;
            loggerModel.Time = DateTime.Now;
            loggerModel.Message = "Hello";
            LogDataCollection.Add(loggerModel);
            LoggerModel loggerModel1 = new LoggerModel();
            loggerModel1.Category = EnumLogType.Error;
            loggerModel1.Time = DateTime.Now;
            loggerModel1.Message = "Hello2";
            LogDataCollection.Add(loggerModel1);
        }


        public static ObservableCollection<LoggerModel> LogDataCollection
        {
            get { return _Row; }
            set { _Row = value; }
        }
    
    }

}

