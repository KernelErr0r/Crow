using Crow.Api.Compiler;
using Crow.Api.Dependencies;
using Crow.Api.Repositories;
using Raven;

namespace Crow.Api
{
    public static class CrowApi
    {
        public static ICommandManager CommandManager { get; set; }
        public static ICompilerManager CompilerManager { get; set; }
        public static IDependencyManager DependencyManager { get; set; }
        public static IRepositoryManager RepositoryManager { get; set; }
    }
}