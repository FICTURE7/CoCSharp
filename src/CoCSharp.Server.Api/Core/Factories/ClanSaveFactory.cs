using System;
using CoCSharp.Server.Api.Db;
using System.Threading;

namespace CoCSharp.Server.Api.Core.Factories
{
    /// <summary>
    /// Provides methods to create <see cref="ClanSave"/> instances.
    /// </summary>
    public class ClanSaveFactory : IFactory<ClanSave>
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ClanSaveFactory"/> class.
        /// </summary>
        public ClanSaveFactory()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        public IFactoryManager Manager { get; set; }

        private int _count;
        #endregion

        #region Methods
        public ClanSave Create()
        {
            Manager.Server.Logs.Info($"Created ClanSave {Interlocked.Increment(ref _count)} instances.");
            return new ClanSave();
        }

        object IFactory.Create()
        {
            return Create();
        }
        #endregion
    }
}
