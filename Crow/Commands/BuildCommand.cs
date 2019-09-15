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
                Crow.Instance.engine = Crow.Instance.engine.Execute(File.ReadAllText(file));

                var configure = Crow.Instance.engine.GetValue("configure");
                var build = Crow.Instance.engine.GetValue("build");

                configure.Invoke();
            }
            else
            {
                throw new FileNotFoundException(file);
            }
        }
    }
}