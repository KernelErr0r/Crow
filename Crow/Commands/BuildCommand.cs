using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crow.Api.Compiler;
using Crow.Api.Configs;
using Crow.Compiler;
using Crow.Data.Configs;
using Raven;

namespace Crow.Commands
{
    [Command("build", "build", "")]
    public class BuildCommand
    {
        private ICompilerManager compilerManager;
        private IConfigDeserializer configDeserializer;
    
        public BuildCommand(ICompilerManager compilerManager, IConfigDeserializer configDeserializer)
        {
            this.compilerManager = compilerManager;
            this.configDeserializer = configDeserializer;
        }
    
        [Default]
        public void Default()
        {
            if (Directory.Exists(".Crow"))
            {
                if (File.Exists(".Crow/build.crow"))
                {
                    var buildConfig = configDeserializer.DeserializeFromFile<BuildConfig>(".Crow/build.crow");

                    RegisterCompilers(buildConfig);

                    var files = GetCompilableFiles();

                    Build(files, buildConfig);
                }
                else
                {
                    throw new FileNotFoundException(".Crow/build.crow");
                }
            }
            else
            {
                throw new DirectoryNotFoundException(".Crow");
            }
        }

        private List<string> GetPathVariable()
        {
            return new List<string>(Environment.GetEnvironmentVariable("Path")?.Split(';') ?? new string[0]);
        }

        private string GetExecutableFromPath(string executable)
        {
            var path = GetPathVariable();
            var directories = from entry in path
                                where !string.IsNullOrWhiteSpace(entry) && Directory.Exists(entry)
                                select entry;

            foreach (var directory in directories)
            {
                foreach (var file in Directory.GetFiles(directory))
                {
                    if (string.Equals(executable, Path.GetFileName(file), StringComparison.Ordinal))
                    {
                        return Path.GetFullPath(file);
                    }
                }
            }

            if (!File.Exists(executable))
                throw new ArgumentException(nameof(executable));

            return null;
        }

        private List<string> GetCompilableFiles()
        {
            var files = new List<string>();
                    
            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories))
            {
                if (compilerManager.GetCompiler(Path.GetExtension(file)) is { })
                {
                    files.Add(file);
                }
            }

            return files;
        }

        private void RegisterCompilers(BuildConfig buildConfig)
        {
            foreach (var compiler in buildConfig.Compilers)
            {
                var compilerExecutable = File.Exists(compiler.Executable) ? compiler.Executable : GetExecutableFromPath(compiler.Executable);

                if(string.IsNullOrWhiteSpace(compiler.Arguments))
                    throw new ArgumentException(nameof(compiler.Arguments));

                if (compiler.FileTypes.Length == 0)
                    throw new ArgumentException(nameof(compiler.FileTypes));
                        
                compilerManager.AddCompiler(new CustomCompiler(compilerExecutable, compiler.Arguments, compiler.FileTypes));
            }
        }

        private void Build(List<string> files, BuildConfig buildConfig)
        {
            if (files.Count > 0)
            {
                var compiler = compilerManager.GetCompiler(Path.GetExtension(files[0]));
                        
                var version = buildConfig.Version;
                var id = 0;

                var directory = buildConfig.BuildPath
                    .Replace("{VERSION}", version);
                var tempDirectory = directory
                    .Replace("{ID}", $"{0}");

                if (!Crow.Instance.GlobalConfig.StorePreviousBuilds || !Directory.Exists(tempDirectory))
                {
                    directory = directory.Replace("{ID}", $"{0}");
                            
                    if (Directory.Exists(directory))
                    {
                        Directory.Delete(directory, true);
                    }
                }
                else
                {
                    tempDirectory += "/../";
                            
                    var directories = Directory.GetDirectories(tempDirectory);

                    if (Int32.TryParse(directories[^1].Split('/')[^1], out id))
                    {
                        directory = directory.Replace("{ID}", $"{id + 1}");
                    }
                    else
                    {
                        throw new InvalidSetupException();
                    }
                }

                Directory.CreateDirectory(directory);
                        
                compiler.Compile($"{directory}/{buildConfig.Name}-{buildConfig.Version}.exe", files.ToArray());
            }
        }
    }
}