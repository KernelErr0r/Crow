using System;
using System.Collections.Generic;

namespace Crow.Data
{
    [Serializable]
    public struct BuildConfig
    {
        public string Name { get; set; }
        public string Version { get; set; }
        
        public List<Compiler> Compilers { get; set; }
        public List<Repository> Repositories { get; set; }
        public List<Dependency> Dependencies { get; set; }
    }
}