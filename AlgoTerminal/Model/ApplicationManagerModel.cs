using AlgoTerminal.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal.Model
{
    public class ApplicationManagerModel : IApplicationManagerModel
    {
        private readonly ILogFileWriter logFileWriter;
        public ApplicationManagerModel(ILogFileWriter logFileWriter)
        {
            this.logFileWriter = logFileWriter;

            ApplicationStartUpRequirement();
        }

        public bool ApplicationStartUpRequirement()
        {
            try
            {
                return true;
            }
            catch
            {
                logFileWriter.DisplayLog(Structure.EnumDeclaration.EnumLogType.Info, " Application StartUp Block Complete. ");
                return false;
            }
            finally
            {
                logFileWriter.DisplayLog(Structure.EnumDeclaration.EnumLogType.Info, " Application StartUp Block Complete. ");
            }
        }
    }
}
