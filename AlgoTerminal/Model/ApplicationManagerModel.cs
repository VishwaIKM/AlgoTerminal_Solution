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
        private readonly IStraddleManager straddleManager;

        public ApplicationManagerModel(ILogFileWriter logFileWriter, IStraddleManager straddleManager)
        {
            this.logFileWriter = logFileWriter;
            this.straddleManager = straddleManager;
        }

        public bool ApplicationStartUpRequirement()
        {
            try
            {
                straddleManager.StraddleStartUP();
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
