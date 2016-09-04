using CoCSharp.Csv;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans decoration (deco).
    /// </summary>
    public class Decoration : VillageObject<DecorationData>
    {
        #region Constants
        internal const int Kind = 6;
        internal const int BaseGameID = 506000000;
        #endregion

        #region Constructors
        // Constructor that FromJsonReader method is going to use.
        internal Decoration(Village village) : base(village)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with the specified <see cref="Village"/> containing
        /// the <see cref="Decoration"/> and <see cref="DecorationData"/> which is associated with it.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> containing the <see cref="Decoration"/>.</param>
        /// <param name="data"><see cref="DecorationData"/> which is associated with this <see cref="Decoration"/>.</param>
        public Decoration(Village village, CsvDataCollectionRef<DecorationData> data) : base(village, data)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with the specified <see cref="Village"/> containing the <see cref="Decoration"/>
        /// and <see cref="DecorationData"/> which is associated with it, X coordinate and Y coordinate.
        /// </summary>
        /// 
        /// <param name="village"><see cref="Village"/> which contains the <see cref="Decoration"/>.</param>
        /// <param name="data"><see cref="DecorationData"/> which is associated with this <see cref="Decoration"/>.</param>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Decoration(Village village, CsvDataCollectionRef<DecorationData> data, int x, int y) : base(village, data, x, y)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        internal override int KindID
        {
            get
            {
                return 6;
            }
        }
        #endregion

        #region Methods
        // Decoration objects does not have a ticking logic.
        internal override void Tick(int tick)
        {
            // Space
        }

        #region Json Reading/Writing
        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            //writer.WriteValue(Data._OldID);

            writer.WritePropertyName("id");
            writer.WriteValue(ID);

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            writer.WriteEndObject();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            var instance = CsvData.GetInstance<DecorationData>();

            var dataId = -1;
            var dataIdSet = false;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = (string)reader.Value;
                    switch (propertyName)
                    {
                        case "id":
                            // Ignore for now.
                            break;

                        case "data":
                            dataId = reader.ReadAsInt32().Value;
                            dataIdSet = true;
                            break;

                        case "x":
                            X = reader.ReadAsInt32().Value;
                            break;

                        case "y":
                            Y = reader.ReadAsInt32().Value;
                            break;
                    }
                }
            }

            if (!dataIdSet)
                throw new InvalidOperationException("Decoration JSON does not contain a 'data' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException("Decoration JSON contained an invalid DecorationData ID. " + instance.GetArgsOutOfRangeMessage("Data ID"));

            //var data = AssetManager.SearchCsvNoCheck<DecorationData>(dataId, 0);
            var data = default(DecorationData);
            if (data == null)
                throw new InvalidOperationException("Could not find DecorationData with ID '" + dataId + "'.");

            _data = data;
        }
        #endregion

        internal static Decoration GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (VillageObjectPool.TryPop(Kind, out obj))
            {
                obj.SetVillageInternal(village);
                return (Decoration)obj;
            }

            return new Decoration(village);
        }
        #endregion
    }
}
