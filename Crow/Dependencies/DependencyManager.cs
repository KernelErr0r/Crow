using System.Collections.Generic;
using Crow.Api.Dependencies;

namespace Crow.Dependencies
{
    public class DependencyManager : IDependencyManager
    {
        public IReadOnlyList<IDependency> Dependencies => dependencies;
        private List<IDependency> dependencies = new List<IDependency>();

        public void AddDependency(IDependency dependency) 
            => dependencies.Add(dependency);

        public void RemoveDependency(IDependency dependency) 
            => dependencies.Remove(dependency);
    }
}