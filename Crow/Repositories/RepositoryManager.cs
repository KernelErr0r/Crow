using System.Collections.Generic;

namespace Crow.Repositories
{
    public class RepositoryManager
    {
        public IReadOnlyList<IRepository> Repositories => repositories;
        private List<IRepository> repositories = new List<IRepository>();

        public void AddRepository(IRepository repository) 
            => repositories.Add(repository);

        public void RemoveRepository(IRepository repository) 
            => repositories.Remove(repository);
    }
}