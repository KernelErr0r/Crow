using System;

namespace Crow.Data
{
    [Serializable]
    public struct Dependency
    {
        public string Repository { get; set; }
        public string Uri { get; set; }
    }
}