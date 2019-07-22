using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crow.Repositories
{
    public interface IRepository
    {
        List<IRepository> GetDependenciesByName(string name);
        Task<List<IRepository>> GetDependenciesByNameAsync(string name);
    }
}