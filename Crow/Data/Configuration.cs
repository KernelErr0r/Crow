using System;
using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace Crow.Data
{
    [Serializable]
    public sealed class Configuration
    {
        [YamlMember(Alias = "name"), DefaultValue("project name")]
        public string Name { get; internal set; }
        [YamlMember(Alias = "version"), DefaultValue("1.0.0")]
        public string Version { get; internal set; }
        
        [YamlIgnore] public IReadOnlyList<Dependency> Dependencies => dependencies;
        [YamlIgnore] public IReadOnlyList<Repository> Repositories => repositories;
        [YamlIgnore] public IReadOnlyList<Compiler> Compilers => compilers;

        private List<Dependency> dependencies = new List<Dependency>();
        private List<Repository> repositories = new List<Repository>();
        private List<Compiler> compilers = new List<Compiler>();
    }
}