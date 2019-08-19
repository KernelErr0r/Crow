using Crow.Commands;
using Crow.Compiler;
using Crow.Dependencies;
using Crow.Repositories;
using Jint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Salem;

namespace Crow
{
    public class Crow
    {
        public static Crow Instance { get; private set; }

        internal Engine engine;

        private CommandManager commandManager = new CommandManager();
        private Logger logger = new Logger("Crow");

        private List<IRepository> repositories = new List<IRepository>();
        private List<IDependency> dependencies = new List<IDependency>();
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

            Action<string, string, string> addCompiler = (extension, arguments, executable) => AddCompiler(extension, arguments, executable);
            Action<string, string> addLocalRepository = (name, path) => AddLocalRepository(name, path);
            Action<string, string> addLocalDependency = (repository, dependency) => AddLocalDependency(repository, dependency);
            Action addCSharpCompiler = () => AddCSharpCompiler();
            Action addVisualBasicCompiler = () => AddVisualBasicCompiler();

            engine.SetValue("addCompiler", addCompiler);
            engine.SetValue("addCSharpCompiler", addCSharpCompiler);
            engine.SetValue("addVisualBasicCompiler", addVisualBasicCompiler);
            engine.SetValue("addLocalRepository", addLocalRepository);
            engine.SetValue("addLocalDependency", addLocalDependency);
        }

        private void AddCompiler(string fileExtension, string arguments, string executable)
        {
            if (File.Exists(executable))
            {
                compilers.Add(new Tuple<ICompiler, string>(new CustomCompiler(executable, arguments), fileExtension));
            }
            else
            {
                throw new FileNotFoundException(executable);
            }
        }

        private void AddCSharpCompiler() =>
            compilers.Add(new Tuple<ICompiler, string>(new CSharpCompiler(), ".cs"));

        private void AddVisualBasicCompiler() =>
            compilers.Add(new Tuple<ICompiler, string>(new VisualBasicCompiler(), ".vb"));

        private void AddLocalRepository(string name, string path) =>
            repositories.Add(new LocalRepository(path, name));

        private void AddLocalDependency(string repository, string dependency) =>
            dependencies.Add(new LocalDependency(dependency, repository));
    }
}