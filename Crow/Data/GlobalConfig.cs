using System;
using System.ComponentModel;

namespace Crow.Data
{
    [Serializable]
    public struct GlobalConfig
    {
        [DefaultValue(false)]
        public bool StorePreviousBuilds { get; set; }
    }
}