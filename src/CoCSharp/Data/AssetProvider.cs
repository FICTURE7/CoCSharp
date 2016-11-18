using System;

namespace CoCSharp.Data
{
    // Base AssetProvider class. Used to load assets and get/return them.
    internal abstract class AssetProvider : IDisposable
    {
        private bool _disposed;

        // Loads the asset for the specified type, at the specified path.
        public abstract void LoadAsset(Type type, string path);

        // Unloads the asset for the specified type.
        public abstract bool UnloadAsset(Type type);

        // Returns a loaded asset for the specified type.
        public abstract object GetAsset(Type type);

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Not much.
            _disposed = true;
        }
    }
}
