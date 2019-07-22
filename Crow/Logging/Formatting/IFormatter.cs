namespace Crow.Logging.Formatting
{
    public interface IFormatter
    {
        void Format(object input);
    }

    public interface IFormatter<T> : IFormatter { }
}