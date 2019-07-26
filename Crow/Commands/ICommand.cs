namespace Crow.Commands
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }

        void Invoke(params string[] arguments);
    }
}