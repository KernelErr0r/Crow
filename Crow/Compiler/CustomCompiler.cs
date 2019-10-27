using System;
using System.Diagnostics;
using System.IO;
using Crow.Api.Compiler;

namespace Crow.Compiler
{
    public class CustomCompiler : ICompiler
    {
        public string[] FileTypes { get; }
        public char Separator { get; set; } = ' ';
        public event EventHandler<int> Finished;

        private Process process = new Process();
        private string filePath;
        private string arguments;

        public CustomCompiler(string filePath, string arguments, string[] fileTypes)
        {
            if (File.Exists(filePath))
            {
                this.FileTypes = fileTypes;
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

        public void Compile(string output, string[] sourceFiles)
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = filePath,
                Arguments = String.Format(arguments, output, String.Join(' ', sourceFiles))
            };

            process.StartInfo = processStartInfo;
            process.Start();
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