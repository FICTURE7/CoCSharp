namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Interface representing a factory manager that manages <see cref="IFactory"/>.
    /// </summary>
    public interface IFactoryManager
    {
        /// <summary>
        /// Gets the <see cref="IServer"/> instance which owns this <see cref="IFactoryManager"/>.
        /// </summary>
        IServer Server { get; }

        /// <summary>
        /// Returns an <see cref="IFactory"/> of the specified type registered to the <see cref="IFactoryManager"/>.
        /// </summary>
        /// <typeparam name="TFactory">Type of <see cref="IFactory"/> to return.</typeparam>
        /// <returns>An <see cref="IFactory"/> of the specified type registered to the <see cref="IFactoryManager"/>.</returns>
        TFactory GetFactory<TFactory>() where TFactory : IFactory, new();

        /// <summary>
        /// Adds an <see cref="IFactory"/> of the specified type to the <see cref="IFactoryManager"/>.
        /// </summary>
        /// <typeparam name="TFactory">Type of <see cref="IFactory"/> to add.</typeparam>
        void RegisterFactory<TFactory>() where TFactory : IFactory, new();
    }
}
