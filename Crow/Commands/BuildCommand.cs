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

                    List<string> compilableFiles = new List<string>();
                    
                    if (string.Equals(buildConfig.Compiler, "custom", StringComparison.OrdinalIgnoreCase))
                    {
                        if (compilerManager.Compilers.All(compiler => compiler.FileTypes != buildConfig.CustomCompiler.FileTypes))
                        {
                            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories))
                            {
                                foreach (var extension in buildConfig.CustomCompiler.FileTypes)
                                {
                                    if (string.Equals(Path.GetExtension(file), extension))
                                    {
                                        compilableFiles.Add(file);
                                    }
                                }
                            }
                            
                            RegisterCompiler(buildConfig);
                        }
                    }
                    else
                    {
                        //TODO
                    }

                    Build(compilableFiles, buildConfig);
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

        private void RegisterCompiler(BuildConfig buildConfig)
        {
            var compilerExecutable = File.Exists(buildConfig.CustomCompiler.Executable) ? buildConfig.CustomCompiler.Executable : GetExecutableFromPath(buildConfig.CustomCompiler.Executable);
            
            if(string.IsNullOrWhiteSpace(buildConfig.CustomCompiler.Arguments))
                throw new ArgumentException(nameof(buildConfig.CustomCompiler.Arguments));

            if (buildConfig.CustomCompiler.FileTypes.Length == 0)
                throw new ArgumentException(nameof(buildConfig.CustomCompiler.FileTypes));
                        
            compilerManager.AddCompiler(new CustomCompiler(compilerExecutable, buildConfig.CustomCompiler.Arguments, buildConfig.CustomCompiler.FileTypes));
        }

        private void Build(List<string> files, BuildConfig buildConfig)
        {
            if (files.Count > 0)
            {
                var compiler = compilerManager.GetCompiler(Path.GetExtension(files[0]));
                
                if(compiler == null)
                    throw new ArgumentNullException(nameof(compiler));

                if (buildConfig.Version == null)
                    throw new ArgumentNullException(nameof(buildConfig.Version));

                if (buildConfig.BuildPath == null)
                    throw new ArgumentNullException(nameof(buildConfig.BuildPath));

                var directory = GetOutputDirectory(buildConfig);

                Directory.CreateDirectory(directory);
                        
                compiler.Compile($"{directory}/{buildConfig.Name}-{buildConfig.Version}.exe", files.ToArray());
            }
        }

        private string GetOutputDirectory(BuildConfig buildConfig)
        {
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

            return directory;
        }
    }
}