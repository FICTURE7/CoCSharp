namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a factory that produces objects.
    /// </summary>
    public interface IFactory
    {
        /// <summary>
        /// Gets or sets the <see cref="IFactoryManager"/> which owns this <see cref="IFactory"/>.
        /// </summary>
        IFactoryManager Manager { get; set; }

        /// <summary>
        /// Returns an object produced by the <see cref="IFactory"/>.
        /// </summary>
        /// <returns>Object produced by the <see cref="IFactory"/>.</returns>
        object Create();
    }
}
