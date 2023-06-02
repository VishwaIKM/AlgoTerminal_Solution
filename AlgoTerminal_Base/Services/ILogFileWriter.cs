﻿using AlgoTerminal_Base.Structure;

namespace AlgoTerminal_Base.Services
{
    public interface ILogFileWriter
    {
        void DisplayLog(EnumDeclaration.EnumLogType Type, string Log);
        bool Start(string LogFileDirectory, string FileName);
        void Stop();
        void WriteLog(EnumDeclaration.EnumLogType type, string log);
    }
}