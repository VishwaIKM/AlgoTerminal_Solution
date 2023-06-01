using AlgoTerminal_Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoTerminal_Base.DataImportFromFile
{
    public class StraddleDataBaseLoadFromCsv
    {
        private IStraddleManager _straddleManager;

        public StraddleDataBaseLoadFromCsv(IStraddleManager straddleManager)
        {
            _straddleManager = straddleManager;
        }

        public bool LoadStaddleStratgy(string path)
        {
            try
            {
                using FileStream _fs = new(path, FileMode.Open, FileAccess.Read);
                using StreamReader _reader = new(_fs);

                while(!_reader.EndOfStream)
                {
                    string key = "MasterKey";

                    if(!_straddleManager.Master_Straddle_Dictionary.ContainsKey(key))
                    {

                    }
                }

            }
            catch(Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
            return false;
        }
    }
}
