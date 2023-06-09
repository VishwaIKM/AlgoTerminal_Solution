using AlgoTerminal.Model.Structure;
using System.Windows.Controls;

namespace AlgoTerminal.Model.Services
{
    public interface ILoggerViewModel
    {
        DataGrid LogDataCollection { get; set; }

        void DisplayLog(EnumDeclaration.EnumLogType Type, string Log);
        bool Start(string LogFileDirectory, string FileName);
        void Stop();
        void WriteLog(EnumDeclaration.EnumLogType type, string log);
    }
}