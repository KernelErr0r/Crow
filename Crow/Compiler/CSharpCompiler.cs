using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using Crow.Api.Compiler;

namespace Crow.Compiler
{
    public class CSharpCompiler : ICompiler
    {
        public event EventHandler<int> Finished;

        public void Compile(string output, string[] source, string[] libraries)
        {
            if (source.Length > 0)
            {
                var syntaxTrees = new List<SyntaxTree>();
                var references = new List<MetadataReference>();

                foreach (var file in source)
                {
                    syntaxTrees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(file)));
                }

                foreach (var library in libraries)
                {
                    references.Add(MetadataReference.CreateFromFile(library));
                }

                var compilation = CSharpCompilation.Create(output, syntaxTrees, references);

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
                throw new Exception();
            }
        }
    }
}