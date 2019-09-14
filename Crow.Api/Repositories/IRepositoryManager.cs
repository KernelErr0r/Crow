using System.Collections.Generic;

namespace Crow.Api.Repositories
{
    public interface IRepositoryManager
    {
        IReadOnlyList<IRepository> Repositories { get; }

        void AddRepository(IRepository repository);
        void RemoveRepository(IRepository repository);
    }
}