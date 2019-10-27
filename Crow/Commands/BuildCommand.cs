using System;
using System.Collections.Generic;
using System.IO;
using Crow.Api;
using Crow.Compiler;
using Crow.Data;
using Raven;
using YamlDotNet.Serialization;

namespace Crow.Commands
{
    [Command("build", "build", "")]
    public class BuildCommand
    {
        private Deserializer deserializer = new Deserializer();
        
        [Default]
        public void Default()
        {
            if (Directory.Exists(".Crow"))
            {
                if (File.Exists(".Crow/build.crow"))
                {
                    var buildConfig = deserializer.Deserialize<BuildConfig>(File.ReadAllText(".Crow/build.crow"));

                    foreach (var compiler in buildConfig.Compilers)
                    {
                        var compilerExecutable = compiler.Executable;

                        if (!File.Exists(compilerExecutable))
                        {
                            var path = Environment.GetEnvironmentVariable("Path");
                            var directories = path.Split(';');

                            var exit = false;
                            
                            foreach (var directory in directories)
                            {
                                if (exit)
                                    break;
                                
                                if (!string.IsNullOrWhiteSpace(directory))
                                {
                                    if (Directory.Exists(directory))
                                    {
                                        foreach (var file in Directory.GetFiles(directory))
                                        {
                                            if (string.Equals(compiler.Executable, Path.GetFileName(file), StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                compilerExecutable = Path.GetFullPath(file);

                                                exit = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            
                            if(!File.Exists(compilerExecutable))
                                throw new ArgumentException("Executable");
                        }
                        
                        if(string.IsNullOrWhiteSpace(compiler.Arguments))
                            throw new ArgumentException("Arguments");

                        if (compiler.FileTypes.Length == 0)
                            throw new ArgumentException("FileTypes");
                        
                        CrowApi.CompilerManager.AddCompiler(new CustomCompiler(compilerExecutable, compiler.Arguments, compiler.FileTypes));
                    }

                    var files = new List<string>();
                    
                    //TODO
                    foreach (var file in Directory.GetFiles(Environment.CurrentDirectory))
                    {
                        if (CrowApi.CompilerManager.GetCompiler(Path.GetExtension(file)) is { })
                        {
                            files.Add(file);
                        }
                    }

                    if (files.Count > 0)
                    {
                        var compiler = CrowApi.CompilerManager.GetCompiler(Path.GetExtension(files[0]));
                        var directories = Directory.GetDirectories(".Crow/builds/");
                        var directory = "";
                        
                        if (!Crow.Instance.GlobalConfig.StorePreviousBuilds || directories.Length == 0)
                        {
                            directory = ".Crow/builds/0";

                            if (Directory.Exists(directory))
                            {
                                Directory.Delete(directory, true);
                            }
                        }
                        else
                        {
                            int index;

                            if (Int32.TryParse(directories[^1].Split('/')[^1], out index))
                            {
                                directory = $".Crow/builds/{index + 1}";
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
    }
}