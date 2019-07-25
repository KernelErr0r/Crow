using System;

namespace Crow.Compiler
{
    public interface ICompiler
    {
        event EventHandler<int> Finished;

        void Compile(string output, string[] sourceFiles, string[] libraries);
    }
}