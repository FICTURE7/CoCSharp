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
        // Constructor used to load the VillageObject from a JsonTextReader.
        internal Decoration() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with the specified
        /// <see cref="Village"/> instance and <see cref="DecorationData"/>.
        /// </summary>
        /// 
        /// <param name="village">
        /// <see cref="Village"/> instance which owns this <see cref="Decoration"/>.
        /// </param>
        /// 
        /// <param name="data"><see cref="DecorationData"/> representing the data of the <see cref="Decoration"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="village"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public Decoration(Village village, DecorationData data) : base(village, data)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        internal override int KindId => 6;
        #endregion

        #region Methods
        /// <summary/>
        protected internal override void Tick(int tick)
        {
            // Decoration does not have much ticking logic.
        }

        #region Json Reading/Writing
        /// <summary/>
        protected internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(Data.Id);

            writer.WritePropertyName("id");
            writer.WriteValue(Id);

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            writer.WriteEndObject();
        }

        /// <summary/>
        protected internal override void FromJsonReader(JsonReader reader)
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
                throw new InvalidOperationException($"Decoration JSON at {reader.Path} does not contain a 'data' field.");

            if (instance.InvalidDataID(dataId))
                throw new InvalidOperationException($"Decoration JSON at {reader.Path} contained an invalid DecorationData ID. {instance.GetArgsOutOfRangeMessage("Data ID")}");

            var tableCollections = Assets.Get<CsvDataTableCollection>();
            var dataRef = new CsvDataRowRef<DecorationData>(dataId);
            var row = dataRef.Get(tableCollections);
            if (row == null)
                throw new InvalidOperationException("Could not find CsvDataRow with ID '" + dataId + "'.");

            var data = row[0];
            if (data == null)
                throw new InvalidOperationException("Could not find DecorationData with ID '" + dataId + "'.");

            _data = data;
        }
        #endregion

        internal static Decoration GetInstance(Village village)
        {
            var obj = (VillageObject)null;
            if (!VillageObjectPool.TryPop(Kind, out obj))
                obj = new Decoration();

            obj.SetVillageInternal(village);
            return (Decoration)obj;
        }
        #endregion
    }
}
