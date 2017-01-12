using CoCSharp.Server.Api.Db;
using System.Threading;

namespace CoCSharp.Server.Api.Core.Factories
{
    /// <summary>
    /// Provides methods to create <see cref="LevelSave"/> instances.
    /// </summary>
    public class LevelSaveFactory : IFactory<LevelSave>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelSaveFactory"/> class.
        /// </summary>
        public LevelSaveFactory()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="IFactoryManager"/> which owns this <see cref="IFactory"/>.
        /// </summary>
        public IFactoryManager Manager { get; set; }

        private int _count;
        #endregion

        #region Methods
        public LevelSave Create()
        {
            Manager.Server.Logs.Info($"Created LevelSave {Interlocked.Increment(ref _count)} instances.");
            return new LevelSave();
        }

        object IFactory.Create()
        {
            return Create();
        }
        #endregion
    }
}
