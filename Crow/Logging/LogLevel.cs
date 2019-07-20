using System.Drawing;

namespace Crow.Logging
{
    public class LogLevel
    {
        public string Name { get; private set; }
        public string Icon { get; private set; }
        public Color Color { get; private set; }

        public LogLevel(string name, string icon, Color color)
        {
            Name = name;
            Icon = icon;
            Color = color;
        }
    }
}