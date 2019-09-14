using System;

namespace Crow.Api.Compiler
{
    public interface ICompiler
    {
        event EventHandler<int> Finished;

        void Compile(string output, string[] sourceFiles, string[] libraries);
    }
}