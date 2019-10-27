using System;

namespace Crow.Api.Compiler
{
    public interface ICompiler
    {
        string[] FileTypes { get; }
        
        event EventHandler<int> Finished;

        void Compile(string output, string[] sourceFiles);
        void Compile(string output, string[] sourceFiles, string[] libraries);
    }
}