using Crow.Api.Commands;
using Crow.Api.Dependencies;
using Crow.Api.Repositories;

namespace Crow.Api
{
    public static class CrowApi
    {
        public static ICommandManager CommandManager { get; set; }
        public static IDependencyManager DependencyManager { get; set; }
        public static IRepositoryManager RepositoryManager { get; set; }
    }
}