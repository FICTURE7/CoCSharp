namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a factory that produces strongly typed objects.
    /// </summary>
    /// <typeparam name="T">Type of object to produce.</typeparam>
    public interface IFactory<T> : IFactory
    {
        /// <summary>
        /// Returns an object produced by the <see cref="IFactory{T}"/>.
        /// </summary>
        /// <returns>Object produced by the <see cref="IFactory{ThisAssembly}"/>.</returns>
        new T Create();
    }
}
