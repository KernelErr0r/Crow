using Crow.Commands;
using Crow.Compiler;
using Crow.Dependencies;
using Crow.Repositories;
using Jint;
using System;
using System.Collections.Generic;
using System.Linq;
using Salem;

namespace Crow
{
    public class Crow
    {
        public static Crow Instance { get; private set; }

        internal Engine engine;

        private CommandManager commandManager = new CommandManager();
        private DependencyManager dependencyManager = new DependencyManager();
        private RepositoryManager repositoryManager = new RepositoryManager();
        
        private Logger logger = new Logger("Crow");

        private List<Tuple<ICompiler, string>> compilers = new List<Tuple<ICompiler, string>>();

        public Crow()
        {
            Instance = this;

            InitializeEngine();
        }

        public void Start(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    commandManager.Invoke(args[0], args.Length == 1 ? null : args.Skip(1).ToArray());
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

        private void InitializeEngine()
        {
            engine = new Engine(config => config.AllowClr().CatchClrExceptions((exception) =>
            {
                logger.Log("Error", exception);

                return true;
            }));

            engine.SetValue("dependencyManager", dependencyManager);
            engine.SetValue("repositoryManager", repositoryManager);
        }
    }
}