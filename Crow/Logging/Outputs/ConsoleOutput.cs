using System;

namespace Crow.Logging.Outputs
{
    public class ConsoleOutput : IOutput
    {
        private object lck = new object();

        public void WriteLine(string input)
        {
            lock (lck)
            {
                Console.WriteLine(input);
            }
        }
    }
}