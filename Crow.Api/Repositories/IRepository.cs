using System.Collections.Generic;
using System.Threading.Tasks;
using Crow.Api.Dependencies;

namespace Crow.Api.Repositories
{
    public interface IRepository
    {
        string Name { get; }
        List<IDependency> GetDependenciesByName(string name);
        Task<List<IDependency>> GetDependenciesByNameAsync(string name);
    }
}