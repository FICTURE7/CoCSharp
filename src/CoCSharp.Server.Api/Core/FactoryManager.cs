using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Api.Core
{
    /// <summary>
    /// Provides methods to manage <see cref="IFactory"/>.
    /// </summary>
    public class FactoryManager : IFactoryManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryManager"/> class from the specified <see cref="IServer"/>.
        /// </summary>
        /// <param name="server"><see cref="IServer"/> which owns this <see cref="FactoryManager"/>.</param>
        public FactoryManager(IServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
            _factories = new Dictionary<Type, IFactory>();
        }
        #endregion

        #region Fields & Properties
        private readonly IServer _server;
        private readonly Dictionary<Type, IFactory> _factories;

        /// <summary>
        /// Gets the <see cref="IServer"/> instance which owns this <see cref="IFactoryManager"/>.
        /// </summary>
        public IServer Server => _server;
        #endregion

        #region Methods
        /// <summary>
        /// Returns an <see cref="IFactory"/> of the specified type registered to the <see cref="IFactoryManager"/>.
        /// </summary>
        /// <typeparam name="TFactory">Type of <see cref="IFactory"/> to return.</typeparam>
        /// <returns>An <see cref="IFactory"/> of the specified type registered to the <see cref="IFactoryManager"/>.</returns>
        public TFactory GetFactory<TFactory>() where TFactory : IFactory, new()
        {
            var fact = default(IFactory);
            if (!_factories.TryGetValue(typeof(TFactory), out fact))
                return default(TFactory);

            return (TFactory)fact;
        }

        /// <summary>
        /// Adds an <see cref="IFactory"/> of the specified type to the <see cref="IFactoryManager"/>.
        /// </summary>
        /// <typeparam name="TFactory">Type of <see cref="IFactory"/> to add.</typeparam>
        public void RegisterFactory<TFactory>() where TFactory : IFactory, new()
        {
            var type = typeof(TFactory);
            if (_factories.ContainsKey(type))
                throw new InvalidOperationException("FactoryManager instance already contains IFactory of specified type.");

            var fact = new TFactory();
            fact.Manager = this;
            _factories.Add(type, fact);
        }
        #endregion
    }
}
