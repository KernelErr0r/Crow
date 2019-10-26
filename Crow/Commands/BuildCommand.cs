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

            }
            else
            {
                throw new FileNotFoundException(file);
            }
        }
    }
}