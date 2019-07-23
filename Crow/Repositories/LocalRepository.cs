using Crow.Dependencies;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Crow.Repositories
{
    class LocalRepository : IRepository
    {
        public string Name => Path.GetDirectoryName(path);

        private string path;

        public LocalRepository(string path)
        {
            if(Directory.Exists(path))
            {
                this.path = path;
            } else
            {
                throw new DirectoryNotFoundException(path);
            }
        }

        public List<IDependency> GetDependenciesByName(string name)
        {
            var list = new List<IDependency>();

            foreach(var file in Directory.GetFiles(path))
            {
                if(file.StartsWith(name + "."))
                {
                    list.Add(new LocalDependency(file, Name));
                }
            }

            return list;
        }

        public async Task<List<IDependency>> GetDependenciesByNameAsync(string name)
        {
            return await Task.Run(() =>
            {
                var list = new List<IDependency>();

                Parallel.ForEach(Directory.GetFiles(path), (file) =>
                {
                    if (file.StartsWith(name + "."))
                    {
                        list.Add(new LocalDependency(file, Name));
                    }
                });

                return list;
            });
        }
    }
}