using System;

namespace Crow.Data
{
    [Serializable]
    public struct Repository
    {
        public string Name { get; set; }
        public string Provider { get; set; }
        public string Uri { get; set; }
    }
}