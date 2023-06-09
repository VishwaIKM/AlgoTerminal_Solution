namespace AlgoTerminal.Model.Services
{
    public interface IGeneral
    {
        void AddToken(string token);
        bool IsTokenFound(string token);
        void RemoveToken(string token);
    }
}