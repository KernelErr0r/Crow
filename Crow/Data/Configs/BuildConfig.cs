using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Crow.Data.Configs
{
    [Serializable]
    public struct BuildConfig
    {
        [JsonProperty("name"), DefaultValue("Awesome_project")]
        public string Name { get; set; }
        [JsonProperty("version"), DefaultValue("1.0.0")]
        public string Version { get; set; }
        [JsonProperty("build-path"), DefaultValue(".Crow/builds/{VERSION}/{ID}")]
        public string BuildPath { get; set; }
        [JsonProperty("compiler"), DefaultValue("Custom")]
        public string Compiler { get; set; }
        
        [JsonProperty("custom-compiler")]
        public Compiler CustomCompiler { get; set; }
        [JsonProperty("repositories")]
        public List<Repository> Repositories { get; set; }
        [JsonProperty("dependencies")]
        public List<Dependency> Dependencies { get; set; }
    }
}