using System.Threading.Tasks;

namespace AlgoTerminal.Model.Services
{
    public interface IApplicationManagerModel
    {
        Task<bool> ApplicationStartUpRequirement();
    }
}