using CoCSharp.Csv;
using CoCSharp.Data.Model;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans decoration (deco).
    /// </summary>
    public class Decoration : VillageObject
    {
        internal const int BaseGameID = 506000000;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class.
        /// </summary>
        public Decoration() 
            : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Decoration"/> class with the specified
        /// X coordinate and Y coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Decoration(int x, int y) 
            : base(x, y)
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="CsvData"/> expected
        /// by the <see cref="Decoration"/>.
        /// </summary>
        /// <remarks>
        /// This is needed to make sure that the user provides a proper <see cref="CsvData"/> type
        /// for the <see cref="VillageObject"/>.
        /// </remarks>
        protected override Type ExpectedDataType
        {
            get
            {
                return typeof(DecorationData);
            }
        }
        #endregion

        #region Methods
        internal override void ToJsonWriter(JsonWriter writer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("data");
            writer.WriteValue(DataID);

            writer.WritePropertyName("x");
            writer.WriteValue(X);

            writer.WritePropertyName("y");
            writer.WriteValue(Y);

            writer.WriteEndObject();
        }

        internal override void FromJsonReader(JsonReader reader)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = (string)reader.Value;
                    switch (propertyName)
                    {
                        case "data":
                            DataID = reader.ReadAsInt32().Value;
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
        }
        #endregion
    }
}
