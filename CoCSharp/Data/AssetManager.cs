using CoCSharp.Csv;
using CoCSharp.Data.AssetProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CoCSharp.Data
{
    /// <summary>
    /// Provides methods to manage Clash of Clans assets.
    /// </summary>
    public class AssetManager
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetManager"/> class with the specified path
        /// pointing to the asset directory.
        /// </summary>
        /// <param name="path">Path pointing to the asset directory.</param>
        public AssetManager(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Could not find directory at '" + Path.GetFullPath(path) + "'.");

            _assetPath = path;
            _providers = new Dictionary<Type, AssetProvider>();

            // Register the AssetProviders.
            var dataProvider = new CsvDataTableAssetProvider();
            RegisterProvider(typeof(CsvDataRow<>), dataProvider);
            // Register typeof(CsvDataTable) to the same instance of the type CsvDataRow<>.
            // Which enables us to do things like this AssetManaget.Get<CsvDataTable>();
            RegisterProvider(typeof(CsvDataTable), dataProvider);
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the default <see cref="AssetManager"/> instance.
        /// </summary>
        public static AssetManager Default { get; set; }

        // Dictionary of AssetLoaders that will load assets of the specified type.
        private readonly Dictionary<Type, AssetProvider> _providers;

        private readonly string _assetPath;
        /// <summary>
        /// Gets the path pointing to the asset directory.
        /// </summary>
        public string AssetPath
        {
            get
            {
                return _assetPath;
            }
        }

        private int _thId;
        [Obsolete]
        internal int TownHallID
        {
            get
            {
                return _thId;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the specified asset type at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to load.</typeparam>
        /// <param name="path">Path to asset file relative to <see cref="AssetPath"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TAsset"/> is already loaded.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        /// <exception cref="FileNotFoundException">File at <paramref name="path"/> does not exists.</exception>
        public void Load<TAsset>(string path) where TAsset : new()
        {
            if (path == null)
                throw new ArgumentNullException("path");

            if (IsLoaded<TAsset>())
                throw new InvalidOperationException("Asset is already loaded.");

            // Path pointing to asset relative to _assetPath.
            var newPath = Path.Combine(_assetPath, path);
            if (!File.Exists(newPath))
                throw new FileNotFoundException("File at '" + Path.GetFullPath(newPath) + "' does not exists.");

            var type = typeof(TAsset);
            // Gets the AssetLoader associated with the type.
            var provider = GetProvider(type);

            Debug.WriteLine("Loading asset of type {0} with AssetProvider {1} at {2}", args: new object[] { type, provider, path });

            provider.LoadAsset(type, newPath);
        }

        /// <summary>
        /// Returns a value indicating whether the specified asset type is loaded.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to check whether its loaded.</typeparam>
        /// <returns><c>tru</c> if the asset is loaded; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        public bool IsLoaded<TAsset>()
        {
            var type = typeof(TAsset);
            var provider = GetProvider(type);
            return provider.GetAsset(type) != null;
        }

        /// <summary>
        /// Returns the asset of the specified type that was loaded.
        /// </summary>
        /// <typeparam name="TAsset">Type of asset to return.</typeparam>
        /// <returns><typeparamref name="TAsset"/> that was loaded.</returns>
        /// 
        /// <exception cref="InvalidOperationException">Type of <typeparamref name="TAsset"/> is not loaded.</exception>
        /// <exception cref="InvalidOperationException">Unknown asset type.</exception>
        public TAsset Get<TAsset>()
        {
            //if (!IsLoaded<T>())
            //    throw new InvalidOperationException("Asset is not loaded.");
            var type = typeof(TAsset);
            var provider = GetProvider(type);
            var asset = provider.GetAsset(type);
            if (asset == null)
                throw new InvalidOperationException("Asset is not loaded.");

            return (TAsset)asset;
        }

        // Returns the AssetLoader instance associated with the specified type.
        private AssetProvider GetProvider(Type type)
        {
            var loader = (AssetProvider)null;

            // If the type is generic, we use its type definition.
            if (type.IsGenericType)
                type = type.GetGenericTypeDefinition();

            if (!_providers.TryGetValue(type, out loader))
                throw new InvalidOperationException("Couldn't find loader for the specified type '" + type + "'.");

            return loader;
        }

        // Registers an AssetProvider for the specified type.
        private void RegisterProvider(Type type, AssetProvider loader)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (loader == null)
                throw new ArgumentNullException("loader");

            if (_providers.ContainsKey(type))
                throw new Exception("Already have a loader registered for the specified type.");

            _providers.Add(type, loader);
        }
        #endregion
    }
}
