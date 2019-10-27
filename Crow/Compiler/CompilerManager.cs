using System;
using System.Collections.Generic;
using Crow.Api.Compiler;

namespace Crow.Compiler
{
    public class CompilerManager : ICompilerManager
    {
        public IReadOnlyList<ICompiler> Compilers => compilers;

        private List<ICompiler> compilers = new List<ICompiler>();
        
        public ICompiler GetCompiler(string fileType)
        {
            foreach (var compiler in compilers)
            {
                foreach (var type in compiler.FileTypes)
                {
                    if (string.Equals(fileType, type, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return compiler;
                    }
                }
            }

            return null;
        }

        public void AddCompiler(ICompiler compiler)
            => compilers.Add(compiler);

        public void RemoveCompiler(ICompiler compiler)
            => compilers.Add(compiler);
    }
}