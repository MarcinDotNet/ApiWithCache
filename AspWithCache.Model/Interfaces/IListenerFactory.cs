namespace AspWithCache.Model.Interfaces
{
    public interface IListenerFactory
    {
        IListenerStrategy GetListener(string listenerType);
    }
}