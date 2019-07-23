using Crow.Dependencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crow.Repositories
{
    public interface IRepository
    {
        string Name { get; }
        List<IDependency> GetDependenciesByName(string name);
        Task<List<IDependency>> GetDependenciesByNameAsync(string name);
    }
}