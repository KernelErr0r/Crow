using Crow.Commands;
using Crow.Dependencies;
using Crow.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Crow.Api;
using Crow.Api.Compiler;
using Crow.Compiler;
using Crow.Data;
using Crow.Plugins;
using Hjson;
using Newtonsoft.Json;
using Raven;
using Salem;

namespace Crow
{
    public class Crow
    {
        public static Crow Instance { get; private set; }

        public GlobalConfig GlobalConfig { get; private set; }
        
        public string MainDirectory { get; } = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName);
        public string PluginsDirectory { get; private set; }
        public string LibrariesDirectory { get; private set; }
        public string ConfigsDirectory { get; private set; }
        public string TemplatesDirectory { get; private set; }

        private readonly Logger logger = new Logger("Crow");
        private readonly PluginManager pluginManager = new PluginManager();

        private List<Tuple<ICompiler, string>> compilers = new List<Tuple<ICompiler, string>>();

        public Crow()
        {
            Instance = this;

            InitializeDirectories();
            InitializeConfigs();
            InitializeApi();
            pluginManager.LoadPlugins();
            pluginManager.InitializePlugins();
            RegisterCommands();
        }

        public void Start(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    CrowApi.CommandManager.Invoke(args[0], args.Length == 1 ? new string[0] : args.Skip(1).ToArray());
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

        private void InitializeDirectories()
        {
            PluginsDirectory = Path.Combine(MainDirectory, "Plugins");
            LibrariesDirectory = Path.Combine(MainDirectory, "Libraries");
            ConfigsDirectory = Path.Combine(MainDirectory, "Configs");
            TemplatesDirectory = Path.Combine(MainDirectory, "Templates");
        
            if (!Directory.Exists(PluginsDirectory))
                Directory.CreateDirectory(PluginsDirectory);
            
            if (!Directory.Exists(LibrariesDirectory))
                Directory.CreateDirectory(LibrariesDirectory);
                            
            if (!Directory.Exists(ConfigsDirectory))
                Directory.CreateDirectory(ConfigsDirectory);
                
            if (!Directory.Exists(TemplatesDirectory))
                Directory.CreateDirectory(TemplatesDirectory);
        }

        private void InitializeConfigs()
        {
            var globalConfigFile = Path.Combine(ConfigsDirectory, "global.hjson");

            string json;
            string hjson;
            
            if (!File.Exists(globalConfigFile))
            {
                json = JsonConvert.SerializeObject(new GlobalConfig(), Formatting.Indented);
                hjson = JsonValue.Parse(json).ToString(Stringify.Hjson);
                
                File.WriteAllText(globalConfigFile, hjson);
            }

            hjson = File.ReadAllText(globalConfigFile);
            json = HjsonValue.Parse(hjson).ToString();

            GlobalConfig = JsonConvert.DeserializeObject<GlobalConfig>(json);
        }

        private void InitializeApi()
        {
            CrowApi.CommandManager = new CommandManager();
            CrowApi.CompilerManager = new CompilerManager();
            CrowApi.DependencyManager = new DependencyManager();
            CrowApi.RepositoryManager = new RepositoryManager();
        }
        
        private void RegisterCommands()
        {
            CrowApi.CommandManager.RegisterCommand(new InteractiveCommand());
            CrowApi.CommandManager.RegisterCommand(new SetupCommand());
            CrowApi.CommandManager.RegisterCommand(new BuildCommand());
        }
    }
}