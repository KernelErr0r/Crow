using System;
using System.Diagnostics;
using System.IO;
using Salem;

namespace Crow.Commands
{
    public class SetupCommand : ICommand
    {
        public string Name => "setup";
        public string Description => "";
        
        private Logger logger = new Logger("Setup");

        public void Invoke(params string[] arguments)
        {
            var workingDirectory = Path.Combine(Environment.CurrentDirectory, ".Crow");
            var templatesDirectory = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), "Templates");
            
            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
                logger.Log("Info", "Created a directory '.Crow'");
                
                if ((arguments?.Length ?? 0) == 0)
                    CopyTemplate(Path.Combine(templatesDirectory, "Default"), ".Crow");
                else
                    CopyTemplate(Path.Combine(templatesDirectory, arguments[0]), ".Crow");

                Directory.CreateDirectory(Path.Combine(workingDirectory, "libs"));
                logger.Log("Info", "Created a directory '.Crow/libs'");

                Directory.CreateDirectory(Path.Combine(workingDirectory, "temp"));
                logger.Log("Info", "Created a directory '.Crow/temp'");
            }
            else
            {
                logger.Log("Error", $"Project '{Path.GetDirectoryName(Environment.CurrentDirectory)}' is already set up");
            }
        }

        private void CopyTemplate(string sourceDirectory, string destinationDirectory)
        {
            if (Directory.Exists(sourceDirectory))
            {
                foreach (var file in Directory.GetFiles(sourceDirectory))
                {
                    File.Copy(file, Path.Combine(destinationDirectory, Path.GetFileName(file)));

                    logger.Log("Info", $"Copied a file '{Path.GetFileName(file)}' to a directory '{destinationDirectory}'");
                }
            } 
            else 
            {
                logger.Log("Error", $"Directory '{sourceDirectory}' doesn't exist");
            }
        }
    }
}