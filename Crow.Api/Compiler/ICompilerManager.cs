using System.Collections.Generic;

namespace Crow.Api.Compiler
{
    public interface ICompilerManager
    {
        IReadOnlyList<ICompiler> Compilers { get; }
        
        void AddCompiler(ICompiler compiler);
        void RemoveCompiler(ICompiler compiler);
    }
}