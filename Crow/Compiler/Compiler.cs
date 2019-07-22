using System;
using System.Diagnostics;
using System.IO;

namespace Crow.Compiler
{
    public class Compiler
    {
        private Process process = new Process();
        private string filePath;
        private string arguments;

        public event EventHandler<int> Finished;

        public Compiler(string filePath, string arguments)
        {
            if(File.Exists(filePath))
            {
                this.filePath = filePath;
                this.arguments = arguments;

                process.Exited += (sender, args) => {
                    Finished?.Invoke(this, process.ExitCode);
                };
            } else
            {
                throw new FileNotFoundException(filePath);
            }
        }

        public void Compile(string[] files)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = filePath,
                Arguments = arguments.Replace("{0}", String.Join(' ', files))
            };

            process.StartInfo = processStartInfo;
            process.Start();
        }
    }
}