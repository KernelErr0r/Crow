using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crow.Repositories
{
    public interface IRepository
    {
        string Name { get; }
        List<IRepository> GetDependenciesByName(string name);
        Task<List<IRepository>> GetDependenciesByNameAsync(string name);
    }
}