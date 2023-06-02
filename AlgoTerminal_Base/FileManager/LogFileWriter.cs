using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using AlgoTerminal_Base.Services;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.FileManager
{
    public class LogFileWriter : ILogFileWriter
    {
        BlockingCollection<Param>? _blocking_collection { get; set; }
        private StreamWriter? _back_log_writer;

        /// <summary>
        /// Begin the TASK
        /// </summary>
        /// <param name="LogFileDirectory"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public bool Start(string LogFileDirectory, string FileName)
        {
            _blocking_collection ??= new();

            _back_log_writer = new StreamWriter(LogFileDirectory + "\\" + FileName, true)
            {
                AutoFlush = true
            };

            Task.Factory.StartNew(() =>
            {
                foreach (Param p in _blocking_collection.GetConsumingEnumerable())
                {
                    switch (p.Ltype)
                    {
                        case EnumLogType.Info:
                            const string LINE_MSG = "[{0}] {1}";
                            _back_log_writer.WriteLine(String.Format(LINE_MSG, LogTimeStamp(), p.Msg));
                            break;
                        case EnumLogType.Warning:
                            const string WARNING_MSG = "[{1}][Warning] {0}";
                            _back_log_writer.WriteLine(String.Format(WARNING_MSG, p.Msg, LogTimeStamp()));
                            System.Diagnostics.Debug.WriteLine("[Warning]" + p.Msg);
                            break;
                        case EnumLogType.Error:
                            const string ERROR_MSG = "[{1}][Error] {0}";
                            _back_log_writer.WriteLine(String.Format(ERROR_MSG, p.Msg, LogTimeStamp()));
                            System.Diagnostics.Debug.WriteLine("[Error]" + p.Msg);
                            break;
                        default:
                            _back_log_writer.WriteLine(String.Format(LINE_MSG, LogTimeStamp(), p.Msg));
                            System.Diagnostics.Debug.WriteLine("[{0}] {1}", p.Ltype.ToString(), p.Msg);
                            break;
                    }

                    if (p.ShowInLogWindow)
                    {
                        string[] rowData = new string[3];
                        Color logColor = Color.LightCyan;
                        if (p.Ltype == EnumLogType.Success)
                        {
                            logColor = Color.White;
                        }
                        else if (p.Ltype == EnumLogType.Info)
                        {
                            logColor = Color.Gold;
                        }
                        else if (p.Ltype == EnumLogType.Error)
                        {
                            logColor = Color.Yellow;
                        }
                        else if (p.Ltype == EnumLogType.Buy)
                        {
                            logColor = Color.LightGreen;
                        }
                        else if (p.Ltype == EnumLogType.Sell)
                        {
                            logColor = Color.Red;
                        }
                        else if (p.Ltype == EnumLogType.Response)
                        {
                            logColor = Color.LavenderBlush;
                        }

                        rowData[0] = DateTime.Now.ToString("hh:mm:ss");
                        rowData[1] = p.Ltype.ToString();
                        rowData[2] = p.Msg;

                        //Need to code for .Net core after GUI Added

                        //ListViewItem LogRow = new ListViewItem(rowData);
                        //LogRow.BackColor = logColor;

                        //General.S_LogView_Form?.AddItem(LogRow);
                    }
                }

                _back_log_writer.Flush();
                _back_log_writer.Close();
            });

            return true;
        }

        /// <summary>
        /// Log add to Blocking collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="log"></param>
        public void WriteLog(EnumLogType type, string log)
        {
            if (!_blocking_collection.IsAddingCompleted)
            {
                Param p = new Param(type, log, false);
                _blocking_collection.Add(p);
            }
        }

        /// <summary>
        /// log add to Blocking collection also flag will be true so it will show in GUI
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Log"></param>
        public void DisplayLog(EnumLogType Type, string Log)
        {
            if (!_blocking_collection.IsAddingCompleted)
            {
                Param p = new Param(Type, Log, true);
                _blocking_collection.Add(p);
            }
        }

        /// <summary>
        /// Log Time Stamp according to System Time on The Machine
        /// </summary>
        /// <returns></returns>
        string LogTimeStamp()
        {
            DateTime now = DateTime.Now;
            return now.ToString("hh:mm:ss");
        }

        /// <summary>
        /// Dispose the instance 
        /// </summary>
        public void Stop()
        {
            _blocking_collection.CompleteAdding();
        }
    }
    /// <summary>
    /// Blocking collection data structure
    /// </summary>
    public class Param
    {
        public EnumLogType Ltype { get; set; }  // Type of log
        public string Msg { get; set; }     // Message
        public bool ShowInLogWindow { get; set; } //Whether to display in Log Window or not

        public Param()
        {
            Ltype = EnumLogType.Info;
            Msg = "";
            ShowInLogWindow = false;
        }

        public Param(EnumLogType logType, string logMsg, bool showInGui = true)
        {
            Ltype = logType;
            Msg = logMsg;
            ShowInLogWindow = showInGui;
        }
    }
}
