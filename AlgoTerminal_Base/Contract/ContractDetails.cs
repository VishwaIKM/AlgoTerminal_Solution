using AlgoTerminal_Base.Services;
using AlgoTerminal_Base.Structure;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using static AlgoTerminal_Base.Structure.EnumDeclaration;

namespace AlgoTerminal_Base.Contract
{
    public class ContractDetails : IContractDetails
    {
        #region Find the latest avaliable Contract file in CON AKJ

        private static readonly string DefultContractPath = "C:\\CON_AKJ\\NSE_FO_contract_" + DateTime.Now.ToString("ddMMyyyy") + ".csv";
        private static readonly DirectoryInfo Info = new DirectoryInfo("C:\\CON_AKJ\\");
        private static readonly FileInfo[] filePaths = Info.GetFiles().OrderByDescending(p => p.CreationTime).Where(x => x.Name.Contains("NSE_FO_contract_") && x.Name.Contains(".csv")).ToArray();
        private static readonly string S_Contract_File_Path = filePaths.Count() <= 0 ? DefultContractPath : filePaths[0].FullName;

        #endregion

        public ConcurrentDictionary<uint, ContractRecord.ContractData>? ContractDetailsToken { get; set; }
        public void LoadContractDetails()
        {
            //Exception will handle in Invoke Method LvL
            if (!File.Exists(S_Contract_File_Path))
            {
                throw new FileNotFoundException(DefultContractPath);
            }
            if (ContractDetailsToken != null)
            {
                ContractDetailsToken.Clear();
                ContractDetailsToken = null;
            }
            using (FileStream _fs = new(S_Contract_File_Path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader _sw = new(_fs))
                {
                    _sw.ReadLine();
                    while (!_sw.EndOfStream)
                    {
                        ContractRecord.ContractData cntrInfo = new();
                        string? line = _sw.ReadLine();

                        if (line == null)
                            continue;

                        string[] arrline = line.Split(',');

                        if (string.IsNullOrEmpty(arrline[18]) || string.IsNullOrWhiteSpace(arrline[18]))
                            continue;

                        cntrInfo.TokenID = Convert.ToUInt32(arrline[0].Trim());

                        if (cntrInfo.TokenID < 1)
                            continue;

                        DateTime dt = Convert.ToDateTime("1/1/1980 12:00:00 AM");//.Add(diff);
                        dt = dt.AddSeconds(Convert.ToInt32(arrline[4]));//cols[28]));//
                        cntrInfo.Expiry = dt;

                        cntrInfo.Symbol = arrline[3].Trim();
                        cntrInfo.Strike = Convert.ToDouble(arrline[5].Trim())/100;
                        cntrInfo.LotSize = Convert.ToUInt32(arrline[8].Trim());

                        cntrInfo.FreezeQnty = (int)Convert.ToDouble(arrline[40]);

                        cntrInfo.Opttype = 0;
                        if (arrline[6].Trim() == EnumOptiontype.CE.ToString())
                            cntrInfo.Opttype = EnumOptiontype.CE;
                        if (arrline[6].Trim() == EnumOptiontype.PE.ToString())
                            cntrInfo.Opttype = EnumOptiontype.PE;
                        if (arrline[6].Trim() == EnumOptiontype.XX.ToString())
                            cntrInfo.Opttype = EnumOptiontype.XX;

                        cntrInfo.InstrumentType = arrline[2].Trim();
                        cntrInfo.TrdSymbol = arrline[18];

                        ContractDetailsToken ??= new();


                        if (!ContractDetailsToken.ContainsKey(cntrInfo.TokenID))
                        {
                            ContractDetailsToken.TryAdd(cntrInfo.TokenID, cntrInfo);
                        }
                    }
                };
            };
        }

        public ContractRecord.ContractData? GetContractDetailsByToken(uint Token)
        {
            if (ContractDetailsToken == null)
                throw new Exception("Contract File Not Loaded!");

            if (ContractDetailsToken.TryGetValue(Token, out ContractRecord.ContractData? value))
                return value;

            return null;
        }

        public ContractRecord.ContractData? GetContractDetailsByTradingSymbol(string TradingSymbol)
        {
            if (ContractDetailsToken == null)
                throw new Exception("Contract File Not Loaded!");

            ContractRecord.ContractData value = ContractDetailsToken.Where(x => x.Value.TrdSymbol == TradingSymbol).Select(x => x.Value).First();
            return value;
        }

    }
}
