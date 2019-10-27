using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Crow.Data
{
    [Serializable]
    public struct BuildConfig
    {
        [DefaultValue("Awesome project")]
        public string Name { get; set; }
        [DefaultValue("1.0.0")]
        public string Version { get; set; }
        [DefaultValue(".Crow/builds/{VERSION}/{ID}")]
        public string BuildPath { get; set; }
        
        public List<Compiler> Compilers { get; set; }
        public List<Repository> Repositories { get; set; }
        public List<Dependency> Dependencies { get; set; }
    }
}