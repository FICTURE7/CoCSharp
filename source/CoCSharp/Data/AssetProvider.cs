using System;

namespace CoCSharp.Data
{
    // Base AssetProvider class. Used to load assets and get/return them.
    internal abstract class AssetProvider
    {
        // Loads the asset for the specified type, at the specified path.
        public abstract void LoadAsset(Type type, string path);

        // Returns a loaded asset for the specified type.
        public abstract object GetAsset(Type type);
    }
}
