using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace AlgoTerminal_Base.Contract
{
    public class ContractDetails
    {
        #region Find the latest avaliable Contract file in CON AKJ

        private static string DefultContractPath = "C:\\CON_AKJ\\NSE_FO_contract_" + DateTime.Now.ToString("ddMMyyyy") + ".csv";
        private static readonly DirectoryInfo Info = new DirectoryInfo("C:\\CON_AKJ\\");
        private static FileInfo[] filePaths = Info.GetFiles().OrderByDescending(p => p.CreationTime).Where(x => x.Name.Contains("NSE_FO_contract_") && x.Name.Contains(".csv")).ToArray();
        private static readonly string S_Contract_File_Path = filePaths.Count() <= 0 ? DefultContractPath : filePaths[0].FullName;

        #endregion

        private static ConcurrentDictionary<uint, Structure.ContractRecord>? S_ContractDetailsToken;
        public ContractDetails()
        {
            S_ContractDetailsToken = new();
            LoadContractDetails();
        }

        private void LoadContractDetails()
        {
            using (FileStream _fs = new(S_Contract_File_Path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader _sw = new(_fs))
                {
                    while(!_sw.EndOfStream)
                    {

                    }
                };
            };        
        }    
    }
}
