using System;
using System.ComponentModel;

namespace Crow.Data.Configs
{
    [Serializable]
    public struct GlobalConfig
    {
        [DefaultValue(false)]
        public bool StorePreviousBuilds { get; set; }
    }
}