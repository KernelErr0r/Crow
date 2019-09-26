using System.IO;
using Crow.Api.Commands;

namespace Crow.Commands
{
    [Command("build", "build [build script]", "")]
    public class BuildCommand
    {
        public void Invoke(string file)
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