using System.IO;

namespace Crow.Logging.Outputs
{
    public class FileOutput : IOutput
    {
        private StreamWriter streamWriter;
        private object lck = new object();

        public FileOutput(string file)
        {
            if (File.Exists(file))
            {
                streamWriter = new StreamWriter(file);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public void WriteLine(string input)
        {
            lock (lck)
            {
                streamWriter.WriteLine(input);
            }
        }
    }
}