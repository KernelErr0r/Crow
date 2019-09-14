using System;
using System.Diagnostics;
using System.IO;
using Crow.Api.Compiler;

namespace Crow.Compiler
{
    public class CustomCompiler : ICompiler
    {
        public event EventHandler<int> Finished;

        public char Separator { get; set; } = ' ';

        private Process process = new Process();
        private string filePath;
        private string arguments;

        public CustomCompiler(string filePath, string arguments)
        {
            if (File.Exists(filePath))
            {
                this.filePath = filePath;
                this.arguments = arguments;

                process.Exited += (sender, args) =>
                {
                    Finished?.Invoke(this, process.ExitCode);
                };
            }
            else
            {
                throw new FileNotFoundException(filePath);
            }
        }

        public void Compile(string output, string[] files, string[] libraries)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = filePath,
                Arguments = String.Format(arguments, output, String.Join(' ', files), String.Join(' ', libraries))
            };

            process.StartInfo = processStartInfo;
            process.Start();
        }
    }
}