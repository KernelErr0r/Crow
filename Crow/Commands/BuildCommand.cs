using System.IO;
using Raven;

namespace Crow.Commands
{
    [Command("build", "build [build script]", "")]
    public class BuildCommand
    {
        [Default]
        public void Default(string file)
        {
            if (File.Exists(file))
            {
                Crow.Instance.Engine = Crow.Instance.Engine.Execute(File.ReadAllText(file));

                var configure = Crow.Instance.Engine.GetValue("configure");
                var build = Crow.Instance.Engine.GetValue("build");

                configure.Invoke();
            }
            else
            {
                throw new FileNotFoundException(file);
            }
        }
    }
}