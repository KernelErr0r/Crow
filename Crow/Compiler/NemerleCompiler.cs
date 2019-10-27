using System;
using System.Collections.Generic;
using System.IO;
using Crow.Api.Compiler;
using Nemerle.Collections;
using Nemerle.Compiler;

namespace Crow.Compiler
{
    public class NemerleCompiler : ICompiler
    {
        public event EventHandler<int> Finished;
        
        private LogWriter logWriter = new LogWriter(new MemoryStream());

        public void Compile(string output, string[] sourceFiles)
        {
            var sources = new List<ISource>();
            var result = 0;

            foreach (var sourceFile in sourceFiles)
                sources.Add(new FileSource(sourceFile, false, false));
            
            var compilationOptions = new CompilationOptions
            {
                Sources = sources.ToNList(),
                TargetIsLibrary = true,
                OutputPath = output
            };
            var manager = new ManagerClass(compilationOptions);
            
            manager.ErrorOccured += (loc, msg) => { result = 1; };
            manager.InitOutput(logWriter);
            manager.Run();

            Finished?.Invoke(this, result);
        }

        public void Compile(string output, string[] sourceFiles, string[] libraries)
        {
            var sources = new List<ISource>();
            var result = 0;

            foreach (var sourceFile in sourceFiles)
                sources.Add(new FileSource(sourceFile, false, false));
            
            var compilationOptions = new CompilationOptions
            {
                Sources = sources.ToNList(),
                ReferencedLibraries = libraries.ToNList(),
                TargetIsLibrary = true,
                OutputPath = output
            };
            var manager = new ManagerClass(compilationOptions);
            
            manager.ErrorOccured += (loc, msg) => { result = 1; };
            manager.InitOutput(logWriter);
            manager.Run();

            Finished?.Invoke(this, result);
        }
    }
}