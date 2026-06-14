namespace Contellation.Custom.Interfaces.Control.Dragablz
{
    public interface IManualInterTabClient : IInterTabClient
    {
        void Add(object item);
        void Remove(object item);
    }
}
