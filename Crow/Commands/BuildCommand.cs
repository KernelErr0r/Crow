using System;
using System.Collections.Generic;
using System.IO;
using Crow.Api;
using Crow.Compiler;
using Crow.Data;
using Hjson;
using Newtonsoft.Json;
using Raven;

namespace Crow.Commands
{
    [Command("build", "build", "")]
    public class BuildCommand
    {
        [Default]
        public void Default()
        {
            if (Directory.Exists(".Crow"))
            {
                if (File.Exists(".Crow/build.crow"))
                {
                    var json = HjsonValue.Load(".Crow/build.crow").ToString();
                    var buildConfig = JsonConvert.DeserializeObject<BuildConfig>(json);

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