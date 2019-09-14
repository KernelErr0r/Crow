using System.Collections.Generic;

namespace Crow.Api.Dependencies
{
    public interface IDependencyManager
    {
        IReadOnlyList<IDependency> Dependencies { get; }

        void AddDependency(IDependency dependency);
        void RemoveDependency(IDependency dependency);
    }
}