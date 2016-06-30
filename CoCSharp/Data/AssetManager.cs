using CoCSharp.Csv;
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
        #region Constants
        /// <summary>
        /// Default path pointing to the asset directory.
        /// </summary>
        public const string DefaultAssetPath = "assets";
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetManager"/> class with <see cref="DefaultAssetPath"/>
        /// as <see cref="AssetPath"/>.
        /// </summary>
        public AssetManager()
            : this(DefaultAssetPath)
        {
            // Space
        }

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
                throw new DirectoryNotFoundException("Could not find directory at '" + path + "'.");

            _dataCsv = new Dictionary<Type, object>();
            _assetPath = path;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the default <see cref="AssetManager"/>.
        /// </summary>
        public static AssetManager Default { get; set; }

        // Dictionary associating Types with 2 dimensional arrays of that type.
        private readonly Dictionary<Type, object> _dataCsv;

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
        #endregion

        #region Methods

        #region SC Loading
        /// <summary>
        /// Not implemented yet.
        /// Loads the SC file at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// <typeparam name="TSc">Type of SC file.</typeparam>
        /// <param name="path">Path relative to <see cref="AssetPath"/> pointing to the SC file.</param>
        public void LoadSc<TSc>(string path)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region CSV Loading
        /// <summary>
        /// Determines if the specified <typeparamref name="TCsvData"/> is loaded in memory.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to check if its loaded.</typeparam>
        /// <returns><c>true</c> if its loaded; otherwise, false.</returns>
        public bool IsCsvLoaded<TCsvData>()
        {
            var type = typeof(TCsvData);
            return _dataCsv.ContainsKey(type);
        }

        /// <summary>
        /// Determines if the specified <see cref="Type"/> is loaded in memory.
        /// </summary>
        /// <param name="type">Type of <see cref="CsvData"/> to check if its loaded.</param>
        /// <returns><c>true</c> if its loaded; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public bool IsCsvLoaded(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return _dataCsv.ContainsKey(type);
        }

        /// <summary>
        /// Loads the uncompressed CSV file at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
        /// <param name="path">Path relative to <see cref="AssetPath"/> pointing to the CSV file.</param>
        /// <returns>A <see cref="CsvDataCollection{TCsvData}"/> which contains the loaded <typeparamref name="TCsvData"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">File at <paramref name="path"/> not found.</exception>
        public CsvDataCollection<TCsvData> LoadCsv<TCsvData>(string path) where TCsvData : CsvData, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            // Load CSV assuming its not compressed.
            return LoadCsv<TCsvData>(path, false);
        }

        /// <summary>
        /// Loads the compressed if specified CSV file at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
        /// <param name="path">Path relative to <see cref="AssetPath"/> pointing to the CSV file.</param>
        /// <param name="compressed">If set to <c>true</c>, the CSV file will be decompressed; otherwise, no.</param>
        /// <returns>A <see cref="CsvDataCollection{TCsvData}"/> which contains the loaded <typeparamref name="TCsvData"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">File at <paramref name="path"/> not found.</exception>
        public CsvDataCollection<TCsvData> LoadCsv<TCsvData>(string path, bool compressed) where TCsvData : CsvData, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            // Parameter 'path' must be relative to AssetPath.
            var fullPath = Path.Combine(_assetPath, path);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Could not find file at '" + fullPath + "'.");

            var type = typeof(TCsvData);
            var table = new CsvTable(fullPath, compressed);
            var data = CsvConvert.DeserializeNew<TCsvData>(table);
            // data.ToArray() is sort of resource intensive,
            // because it creates a lot of new object arrays.
            var dataArray = data.ToArray();

            // If we already have a CSV data array of this type,
            // we update it.
            if (_dataCsv.ContainsKey(type))
            {
                _dataCsv.Add(typeof(TCsvData), dataArray);
            }
            else
            {
                _dataCsv[type] = dataArray;
            }

            return data;
        }
        #endregion

        #region CSV Searching
        /// <summary>
        /// Searches through the loaded CSV data and searches for the specified data ID.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to search.</typeparam>
        /// <param name="id">Data ID to search for.</param>
        /// <returns>An array of <typeparamref name="TCsvData"/> with the specified data ID.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 0.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> of type <typeparamref name="TCsvData"/> not loaded.</exception>
        public TCsvData[] SearchCsv<TCsvData>(int id) where TCsvData : CsvData, new()
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("id", "id must be non-negative.");

            var type = typeof(TCsvData);
            if (!_dataCsv.ContainsKey(type))
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");

            var collection = (TCsvData[][])_dataCsv[type];
            for (int i = 0; i < collection.Length; i++)
            {
                var subArray = collection[i];
                Debug.Assert(subArray != null, "CsvDataCollection<TCsvData>.ToArray() returned a null sub array.");

                // Make sure it contains 1 element at least.
                if (subArray.Length == 0)
                    continue;

                // Check if the first element has the same ID.
                // We ignore the rest because they should all have the same ID.
                if (subArray[0].ID == id)
                    return subArray;
            }

            return null;
        }

        /// <summary>
        /// Searches through the loaded CSV data and searches for the specified data ID and level.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to search.</typeparam>
        /// <param name="id">Data ID to search for.</param>
        /// <param name="level">Level to search for.</param>
        /// <returns>A <typeparamref name="TCsvData"/> with the specified data ID and level.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 0.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> of type <typeparamref name="TCsvData"/> not loaded.</exception>
        public TCsvData SearchCsv<TCsvData>(int id, int level) where TCsvData : CsvData, new()
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("id", "id must be non-negative.");
            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

            var type = typeof(TCsvData);
            if (!_dataCsv.ContainsKey(type))
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");

            var collection = SearchCsv<TCsvData>(id);
            for (int i = 0; i < collection.Length; i++)
            {
                var element = collection[i];
                if (element.Level == level)
                    return element;
            }

            return null;
        }

        /// <summary>
        /// Searches through the loaded CSV data and searches for the specified text ID.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to search.</typeparam>
        /// <param name="tid">Text ID to search for.</param>
        /// <returns>An array of <typeparamref name="TCsvData"/> with the specified text ID.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tid"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> of type <typeparamref name="TCsvData"/> not loaded.</exception>
        public TCsvData[] SearchCsv<TCsvData>(string tid) where TCsvData : CsvData, new()
        {
            if (string.IsNullOrWhiteSpace(tid))
                throw new ArgumentNullException("tid");

            var type = typeof(TCsvData);
            if (!_dataCsv.ContainsKey(type))
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");

            var collection = (TCsvData[][])_dataCsv[type];
            for (int i = 0; i < collection.Length; i++)
            {
                var subArray = collection[i];
                Debug.Assert(subArray != null, "CsvDataCollection<TCsvData>.ToArray() returned a null sub array.");

                // Make sure it contains an element at least.
                if (subArray.Length == 0)
                    continue;

                // Check if the first element has the same TID.
                // We ignore the rest because they should all have the same TID.
                if (subArray[0].TID == tid)
                    return subArray;
            }

            return null;
        }

        /// <summary>
        /// Searches through the loaded CSV data and searches for the specified text ID and level.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to search.</typeparam>
        /// <param name="tid">Text ID to search for.</param>
        /// <param name="level">Level to search for.</param>
        /// <returns>A <typeparamref name="TCsvData"/> with the specified text ID and level.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tid"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CsvData"/> of type <typeparamref name="TCsvData"/> not loaded.</exception>
        public TCsvData SearchCsv<TCsvData>(string tid, int level) where TCsvData : CsvData, new()
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");
            if (string.IsNullOrWhiteSpace(tid))
                throw new ArgumentNullException("tid");

            var type = typeof(TCsvData);
            if (!_dataCsv.ContainsKey(type))
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");

            var collection = SearchCsv<TCsvData>(tid);
            for (int i = 0; i < collection.Length; i++)
            {
                var element = collection[i];
                if (element.Level == level)
                    return element;
            }

            return null;
        }

        internal CsvData InternalSearchCsv(Type type, int id, int level)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("id", "id must be non-negative.");
            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

            if (!_dataCsv.ContainsKey(type))
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");

            var collection = (CsvData[][])_dataCsv[type];
            for (int i = 0; i < collection.Length; i++)
            {
                var subArray = collection[i];
                Debug.Assert(subArray != null, "CsvDataCollection<TCsvData>.ToArray() returned a null sub array.");

                // Make sure it contains 1 element at least.
                if (subArray.Length == 0)
                    continue;

                // Check if the first element has the same ID.
                // We ignore the rest because they should all have the same ID.
                if (subArray[0].ID == id)
                {
                    for (int j = 0; j < subArray.Length; j++)
                    {
                        var element = subArray[j];
                        if (element.Level == level)
                            return element;
                    }
                    return null;
                }
            }
            return null;
        }
        #endregion
        #endregion
    }
}
