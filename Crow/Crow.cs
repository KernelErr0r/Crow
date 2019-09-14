using Crow.Commands;
using Crow.Dependencies;
using Crow.Repositories;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Crow()
        {
            Instance = this;

            InitializeApi();
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