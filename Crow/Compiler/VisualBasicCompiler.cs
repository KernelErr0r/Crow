using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using Crow.Api.Compiler;

namespace Crow.Compiler
{
    public class VisualBasicCompiler : ICompiler
    {
        public event EventHandler<int> Finished;

        public void Compile(string output, string[] sourceFiles)
        {
            if (sourceFiles.Length > 0)
            {
                var syntaxTrees = new List<SyntaxTree>();

                foreach (var file in sourceFiles)
                {
                    syntaxTrees.Add(VisualBasicSyntaxTree.ParseText(File.ReadAllText(file)));
                }

                var compilation = VisualBasicCompilation.Create(output, syntaxTrees);

                using (var memoryStream = new MemoryStream())
                {
                    var result = compilation.Emit(memoryStream);

                    if (result.Success)
                    {
                        using (var streamWriter = new StreamWriter(output))
                        {
                            streamWriter.Write(memoryStream.ToArray());
                        }

                        Finished?.Invoke(this, 0);
                    }
                    else
                    {
                        Finished?.Invoke(this, 1);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public void Compile(string output, string[] sourceFiles, string[] libraries)
        {
            if (sourceFiles.Length > 0)
            {
                var syntaxTrees = new List<SyntaxTree>();
                var references = new List<MetadataReference>();

                foreach (var file in sourceFiles)
                {
                    syntaxTrees.Add(VisualBasicSyntaxTree.ParseText(File.ReadAllText(file)));
                }

                foreach (var library in libraries)
                {
                    references.Add(MetadataReference.CreateFromFile(library));
                }

                var compilation = VisualBasicCompilation.Create(output, syntaxTrees, references);

                using (var memoryStream = new MemoryStream())
                {
                    var result = compilation.Emit(memoryStream);

                    if (result.Success)
                    {
                        using (var streamWriter = new StreamWriter(output))
                        {
                            streamWriter.Write(memoryStream.ToArray());
                        }

                        Finished?.Invoke(this, 0);
                    }
                    else
                    {
                        Finished?.Invoke(this, 1);
                    }
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}