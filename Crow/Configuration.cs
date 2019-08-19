using System;
using System.Collections.Generic;
using System.ComponentModel;
using Crow.Compiler;
using Crow.Dependencies;
using Crow.Repositories;
using YamlDotNet.Serialization;

namespace Crow
{
    [Serializable]
    public sealed class Configuration
    {
        [YamlMember(Alias = "name"), DefaultValue("project name")]
        public string Name { get; internal set; }
        [YamlMember(Alias = "version"), DefaultValue("1.0.0")]
        public string Version { get; internal set; }
        
        [YamlIgnore] public IReadOnlyList<IDependency> Dependencies => dependencies;
        [YamlIgnore] public IReadOnlyList<IRepository> Repositories => repositories;
        [YamlIgnore] public IReadOnlyList<ICompiler> Compilers => compilers;

        private List<IDependency> dependencies = new List<IDependency>();
        private List<IRepository> repositories = new List<IRepository>();
        private List<ICompiler> compilers = new List<ICompiler>();
    }
}