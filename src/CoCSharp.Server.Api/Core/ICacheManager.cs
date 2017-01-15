namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a cache manager that caches object instances.
    /// </summary>
    public interface ICacheManager
    {
        bool TryGet<T>(long id, out T obj);

        bool Register<T>(long id, T obj);

        void Unregister<T>(long id); 
    }
}
