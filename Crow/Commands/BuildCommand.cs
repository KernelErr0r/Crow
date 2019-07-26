using System;
using System.IO;

namespace Crow.Commands
{
    public class BuildCommand : ICommand
    {
        public string Name => "build";

        public string Description => "";

        public void Invoke(params string[] arguments)
        {
            if(arguments.Length > 0)
            {
                if(File.Exists(arguments[0]))
                {
                    Crow.Instance.engine = Crow.Instance.engine.Execute(File.ReadAllText(args[0]));

                    var configure = Crow.Instance.engine.GetValue("configure");
                    var build = Crow.Instance.engine.GetValue("build");

                    configure.Invoke();
                } else
                {
                    throw new FileNotFoundException(arguments[0]);
                }
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}