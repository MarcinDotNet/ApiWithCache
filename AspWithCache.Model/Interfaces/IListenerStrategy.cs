namespace AspWithCache.Model.Interfaces
{
    public interface IListenerStrategy : IDisposable
    {
        void Start();

        void Stop();
    }
}