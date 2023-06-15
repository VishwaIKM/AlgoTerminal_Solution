namespace AlgoTerminal.Model.Services
{
    public interface IModeratorManagerModel
    {
        int GetOrderId();
        void ResetOrderID(int orderId);
    }
}