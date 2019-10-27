using System.Collections.Generic;

namespace Crow.Api.Compiler
{
    public interface ICompilerManager
    {
        IReadOnlyList<ICompiler> Compilers { get; }
        
        ICompiler GetCompiler(string fileType);
        void AddCompiler(ICompiler compiler);
        void RemoveCompiler(ICompiler compiler);
    }
}