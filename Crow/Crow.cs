using Crow.Commands;
using Crow.Dependencies;
using Crow.Repositories;
using Jint;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crow.Api;
using Crow.Api.Compiler;
using Salem;

namespace Crow
{
    public class Crow
    {
        public static Crow Instance { get; private set; }

        internal Engine engine;

        private Logger logger = new Logger("Crow");

        private List<Tuple<ICompiler, string>> compilers = new List<Tuple<ICompiler, string>>();
        private ConcurrentBag<IPlugin> plugins = new ConcurrentBag<IPlugin>();

        public Crow()
        {
            Instance = this;

            InitializeApi();
            LoadPlugins();
            InitializePlugins();
            InitializeEngine();
            RegisterCommands();
        }

        public void Start(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    CrowApi.CommandManager.Invoke(args[0], args.Length == 1 ? null : args.Skip(1).ToArray());
                }
                else
                {
                    logger.Log("Error", "Incorrect usage");
                }
            }
            catch (Exception exception)
            {
                logger.Log("Error", exception);
            }
        }

        private void InitializeApi()
        {
            CrowApi.CommandManager = new CommandManager();
            CrowApi.DependencyManager = new DependencyManager();
            CrowApi.RepositoryManager = new RepositoryManager();
        }

        private void LoadPlugins()
        {
            var executableFilePath = Process.GetCurrentProcess().MainModule?.FileName;
            var pluginsDirectory = Path.Combine(Path.GetDirectoryName(executableFilePath), "Plugins");
            var files = new ConcurrentBag<string>(Directory.GetFiles(pluginsDirectory));

            Parallel.ForEach(files, (file) =>
            {
                if (Path.GetExtension(file) == ".dll")
                {
                    var rawAssembly = File.ReadAllBytes(file);
                    var assembly = AppDomain.CurrentDomain.Load(rawAssembly);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            var plugin = Activator.CreateInstance(type) as IPlugin;
                        
                            plugins.Add(plugin);
                        }
                    }   
                }
            });
        }
        
        private void InitializePlugins()
        {
            Parallel.ForEach(plugins, async (plugin) =>
            {
                await logger.Log(() => plugin.PreInit(), $"Preinitializing {plugin.Name}", $"Successfully preinitialized {plugin.Name}", $"Couldn't preinitialize {plugin.Name}"); 
            });

            
            Parallel.ForEach(plugins, async (plugin) =>
            {
                await logger.Log(() => plugin.Init(), $"Initializing {plugin.Name}", $"Successfully initialized {plugin.Name}", $"Couldn't initialize {plugin.Name}");
            });
        }
        
        private void InitializeEngine()
        {
            engine = new Engine(config => config.AllowClr().CatchClrExceptions((exception) =>
            {
                logger.Log("Error", exception);

                return true;
            }));

            engine.SetValue("dependencyManager", CrowApi.DependencyManager);
            engine.SetValue("repositoryManager", CrowApi.RepositoryManager);
        }

        private void RegisterCommands()
        {
            CrowApi.CommandManager.Register(new SetupCommand());
        }
    }
}