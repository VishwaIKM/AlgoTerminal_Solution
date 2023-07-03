using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace AlgoTerminal.Model.FileManager
{

    public class OtherMethods
    {
        public static T DeepCopy<T>(T other)
        {
            using MemoryStream ms = new();
            BinaryFormatter formatter = new()
            {
                Context = new StreamingContext(StreamingContextStates.Clone)
            };
#pragma warning disable SYSLIB0011
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return (T)formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011
        }

        public static string GetNewName(string OldName)
        {
            string NewName = "NotDefine";

            if (OldName.Contains('.'))
            {
                var data = OldName.Split('.');
                var LastName = double.TryParse(data[1], out double value) ? value : 0;
                if (LastName != 0)
                {
                    LastName /= 100.00;
                    LastName += 0.01;
                    NewName = data[0] + LastName;
                }
                else
                {
                    //BUG
                    NewName = OldName + ".01";
                }
            }
            else
            {
                NewName = OldName + ".01";
            }
            return NewName;
        }
    }
}
