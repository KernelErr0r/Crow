using System.Collections.Generic;
using Crow.Api.Dependencies;

namespace Crow.Dependencies
{
    public class DependencyManager
    {
        public IReadOnlyList<IDependency> Dependencies => dependencies;
        private List<IDependency> dependencies = new List<IDependency>();

        public void AddRepository(IDependency dependency) 
            => dependencies.Add(dependency);

        public void RemoveRepository(IDependency dependency) 
            => dependencies.Remove(dependency);
    }
}