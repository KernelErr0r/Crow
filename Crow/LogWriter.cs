using System.IO;
using System.Text;
using Salem;

namespace Crow
{
    public class LogWriter : StreamWriter
    {
        private Logger logger = new Logger();
        
        public LogWriter(Stream stream) : base(stream) { }
        public LogWriter(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public LogWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize) {}
        public LogWriter(Stream stream, Encoding encoding = null, int bufferSize = -1, bool leaveOpen = false) : base(stream, encoding, bufferSize, leaveOpen) { }
        public LogWriter(string path) : base(path) { }
        public LogWriter(string path, bool append) : base(path, append) { }
        public LogWriter(string path, bool append, Encoding encoding) : base(path, append, encoding) { }

        public LogWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
        { }

        public new void WriteLine(string input)
        {
            logger.Log("Info", input);
        }
    }
}