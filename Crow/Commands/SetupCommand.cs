using System;
using System.Diagnostics;
using System.IO;
using Crow.Api.Commands;
using Salem;

namespace Crow.Commands
{
    [Command("setup", "setup [template]", "")]
    public class SetupCommand
    {
        private Logger logger = new Logger("Setup");

        public void Invoke(string template = "Default")
        {
            var workingDirectory = Path.Combine(Environment.CurrentDirectory, ".Crow");

            if (!Directory.Exists(workingDirectory))
            {
                Directory.CreateDirectory(workingDirectory);
                logger.Log("Info", "Created a directory '.Crow'");
                
                CopyTemplate(Path.Combine(Crow.Instance.TemplatesDirectory, template), ".Crow");

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