using CoCSharp.Csv;
using CoCSharp.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Data
{
    /// <summary>
    /// Provides methods to manage Clash of Clans assets.
    /// </summary>
    public class AssetManager
    {
        #region Constants
        private const int Base = 1000000;
        #endregion

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
                throw new DirectoryNotFoundException("Could not find directory at '" + path + "'.");

            _tid2index = new Dictionary<string, int>();
            _arrayCsv = new object[30];
            _assetPath = path;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the default <see cref="AssetManager"/>.
        /// </summary>
        public static AssetManager DefaultInstance { get; set; }

        private readonly Dictionary<string, int> _tid2index;
        // Array of CsvDataCollection.
        private readonly object[] _arrayCsv;

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
        /// <summary>
        /// Gets the data ID of TownHalls.
        /// </summary>
        public int TownHallID
        {
            get
            {
                if (_thId == default(int))
                {
                    if (IsCsvLoaded<BuildingData>())
                        _thId = SearchCsv<BuildingData>("TID_BUILDING_TOWN_HALL", 0).ID;
                }

                return _thId;
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

        #region CSV Management
        #region IsLoaded
        /// <summary>
        /// Determines if the specified <typeparamref name="TCsvData"/> is loaded in memory.
        /// </summary>
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to check if its loaded.</typeparam>
        /// <returns><c>true</c> if its loaded; otherwise, false.</returns>
        public bool IsCsvLoaded<TCsvData>() where TCsvData : CsvData, new()
        {
            var instance = CsvData.GetInstance<TCsvData>();
            var index = GetIndex(instance.BaseDataID);
            return _arrayCsv[index] != null;
        }
        #endregion

        #region Load
        /// <summary>
        /// Loads the uncompressed CSV file at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// 
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
        /// <param name="path">Path relative to <see cref="AssetPath"/> pointing to the CSV file.</param>
        /// <returns>A <see cref="CsvDataCollection{TCsvData}"/> which contains the loaded <typeparamref name="TCsvData"/>.</returns>
        /// 
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
        /// Loads the compressed if specified, CSV file at the specified path relative to <see cref="AssetPath"/> in memory.
        /// </summary>
        /// 
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
        /// <param name="path">Path relative to <see cref="AssetPath"/> pointing to the CSV file.</param>
        /// <param name="compressed">If set to <c>true</c>, the CSV file will be decompressed; otherwise, no.</param>
        /// <returns>A <see cref="CsvDataCollection{TCsvData}"/> which contains the loaded <typeparamref name="TCsvData"/>.</returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null or whitespace.</exception>
        /// <exception cref="FileNotFoundException">File at <paramref name="path"/> not found.</exception>
        public CsvDataCollection<TCsvData> LoadCsv<TCsvData>(string path, bool compressed) where TCsvData : CsvData, new()
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            // Parameter 'path' must be relative to AssetPath.
            var fullPath = Path.Combine(_assetPath, path);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Could not find CSV file at '" + fullPath + "'.");

            var type = typeof(TCsvData);
            var table = new CsvTable(fullPath, compressed);
            var data = CsvConvert.DeserializeNew<TCsvData>(table);
            data.IsReadOnly = true;

            var index = GetIndex(CsvData.GetInstance<TCsvData>().BaseDataID);
            _arrayCsv[index] = data;
            return data;
        }
        #endregion

        #region Searching
        /// <summary>
        /// Searches for a <see cref="CsvDataSubCollection{TCsvData}"/> of <typeparamref name="TCsvData"/> with the specified data ID and
        /// returns the <see cref="CsvDataSubCollection{TCsvData}"/> of <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to look for.</typeparam>
        /// <param name="id">Specific data ID to look for.</param>
        /// <returns>
        /// A <see cref="CsvDataSubCollection{TCsvData}"/> of<typeparamref name="TCsvData"/> with the same data ID as specified if successful.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than the <see cref="Base"/>.</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TCsvData"/> was not loaded.</exception>
        public CsvDataSubCollection<TCsvData> SearchCsv<TCsvData>(int id) where TCsvData : CsvData, new()
        {
            // Silliness here.
            if (id < Base)
                throw new ArgumentOutOfRangeException("id", "id must be greater or equal to " + Base + ".");

            var index = GetIndex(id);
            var collection = (CsvDataCollection<TCsvData>)_arrayCsv[index];
            if (collection == null)
            {
                var type = typeof(TCsvData);
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");
            }

            return collection[id];
        }

        /// <summary>
        /// Searches for a <typeparamref name="TCsvData"/> with the specified data ID and level and
        /// returns the searched <typeparamref name="TCsvData"/>.
        /// </summary>
        /// 
        /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to look for.</typeparam>
        /// <param name="id">Specific data ID to look for.</param>
        /// <param name="level">Specific level to look for.</param>
        /// <returns>A <typeparamref name="TCsvData"/> with the same data ID and level as specified if successful.</returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than the <see cref="Base"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        /// <exception cref="InvalidOperationException"><typeparamref name="TCsvData"/> was not loaded.</exception>
        public TCsvData SearchCsv<TCsvData>(int id, int level) where TCsvData : CsvData, new()
        {
            // Silliness here.
            if (id < Base)
                throw new ArgumentOutOfRangeException("id", "id must be greater or equal to " + Base + ".");
            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

            var index = GetIndex(id);
            var collection = (CsvDataCollection<TCsvData>)_arrayCsv[index];
            if (collection == null)
            {
                var type = typeof(TCsvData);
                throw new InvalidOperationException("CsvData of type '" + type + "' was not loaded. Call LoadCsv<" + type.Name + ">() first.");
            }

            var subcollection = collection[id];
            if (subcollection == null)
                return null;

            return subcollection[level];
        }

        // Searches a CsvDataSubCollection without checks.
        internal CsvDataSubCollection<TCsvData> SearchCsvNoCheck<TCsvData>(int id) where TCsvData : CsvData, new()
        {
            var index = GetIndex(id);
            var collection = ((CsvDataCollection<TCsvData>)_arrayCsv[index]);
            if (collection == null)
                return null;

            return collection[id];
        }

        // Searches a CsvData without checks which is a little bit faster.
        internal TCsvData SearchCsvNoCheck<TCsvData>(int id, int level) where TCsvData : CsvData, new()
        {
            var index = GetIndex(id);
            var collection = ((CsvDataCollection<TCsvData>)_arrayCsv[index]);
            if (collection == null)
                return null;

            var subcollection = collection[id];
            if (subcollection == null)
                return null;

            return subcollection[level];
        }
        #endregion

        // Gets the index in _arrayCsv of a CsvDataCollection from a data ID.
        // Eg: 1000001  => 1
        //     12000003 => 12
        //     28000032 => 28
        private int GetIndex(int id)
        {
            return id / Base;
        }

        /// <summary>
        /// Searches for an array <typeparamref name="TCsvData"/> have the same specified TID.
        /// </summary>
        /// <typeparam name="TCsvData"></typeparam>
        /// <param name="tid"></param>
        /// <returns></returns>
        public TCsvData[] SearchCsv<TCsvData>(string tid) where TCsvData : CsvData, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searches for a <typeparamref name="TCsvData"/> have the same specified TID and level.
        /// </summary>
        /// <typeparam name="TCsvData"></typeparam>
        /// <param name="tid"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public TCsvData SearchCsv<TCsvData>(string tid, int level) where TCsvData : CsvData, new()
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
    }
}
