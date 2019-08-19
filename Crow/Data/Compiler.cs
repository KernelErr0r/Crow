using System;

namespace Crow.Data
{
    [Serializable]
    public struct Compiler
    {
        public string Executable { get; set; }
        public string Arguments { get; set; }
    }
}