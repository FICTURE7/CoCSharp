using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace CoCSharp.Logic
{
    public partial class Village
    {
        /// <summary>
        /// Returns a non-indented JSON string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A non-indented JSON string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Village"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Village"/>.</returns>
        public string ToJson(bool indent)
        {
            var textWriter = new StringWriter();
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                if (indent)
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 4;
                }

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("exp_ver");
                jsonWriter.WriteValue(ExperienceVersion);

                jsonWriter.WritePropertyName("buildings");
                WriteBuildingArray(jsonWriter);

                jsonWriter.WritePropertyName("obstacles");
                WriteObstacleArray(jsonWriter);

                jsonWriter.WritePropertyName("traps");
                WriteTrapArray(jsonWriter);

                jsonWriter.WritePropertyName("decos");
                WriteDecorationArray(jsonWriter);

                jsonWriter.WriteEndObject();
                return textWriter.ToString();
            }
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string with the default <see cref="Data.AssetManager"/> instance.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException"><see cref="AssetManager.Default"/> is null.</exception>
        /// 
        /// <remarks>
        /// The default <see cref="Data.AssetManager"/> instance is the value <see cref="AssetManager.Default"/>.
        /// </remarks>
        public static Village FromJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            if (AssetManager.Default == null)
                throw new InvalidOperationException("Default instance of AssetManager cannot be null.");

            //return FromJson(value, AssetManager.Default);
            throw new Exception();
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string with the specified <see cref="Data.AssetManager"/> instance.
        /// </summary>
        /// 
        /// <param name="value">
        /// JSON string which represents the <see cref="Village"/>.
        /// </param>
        /// <param name="level">
        /// <see cref="Data.AssetManager"/> from which data of <see cref="VillageObject"/> in the <see cref="Village"/> will be populated.
        /// </param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or whitespace.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="level"/> has not loaded <see cref="CsvData"/> type of either of the following;
        /// <see cref="BuildingData"/>, <see cref="ObstacleData"/>, <see cref="TrapData"/> or <see cref="DecorationData"/>.
        /// </exception>
        public static Village FromJson(string value, Level level)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            var village = new Village(level);

            var textReader = new StringReader(value);
            using (var jsonReader = new JsonTextReader(textReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var propertyName = (string)jsonReader.Value;
                        switch (propertyName)
                        {
                            case "exp_ver":
                                village.ExperienceVersion = jsonReader.ReadAsInt32().Value;
                                break;

                            case "buildings":
                                ReadBuildingArray(jsonReader, village);
                                break;

                            case "obstacles":
                                ReadObstacleArray(jsonReader, village);
                                break;

                            case "traps":
                                ReadTrapArray(jsonReader, village);
                                break;

                            case "decos":
                                ReadDecorationArray(jsonReader, village);
                                break;
                        }
                    }
                }
            }

            if (village._townhall == null)
                throw new InvalidOperationException("Village does not contain a Town Hall.");

            // Tick once to update/set VillageObject values and
            // finish construction that are finished.
            village.Update(0);

            return village;
        }

        #region Json Writing
        private void WriteBuildingArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var building in _villageObjects.GetRow(Building.Kind))
                building?.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteObstacleArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var obstacle in _villageObjects.GetRow(Obstacle.Kind))
                obstacle?.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteTrapArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var trap in _villageObjects.GetRow(Trap.Kind))
                trap?.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteDecorationArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var deco in _villageObjects.GetRow(Decoration.Kind))
                deco?.ToJsonWriter(writer);

            writer.WriteEndArray();
        }
        #endregion

        #region Json Reading
        private static void ReadBuildingArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var building = (Building)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    building = Building.GetInstance(village);
                    building.FromJsonReader(reader);

                    building = null;
                }
            }
        }

        private static void ReadObstacleArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var obstacle = (Obstacle)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    obstacle = Obstacle.GetInstance(village);
                    obstacle.FromJsonReader(reader);

                    obstacle = null;
                }
            }
        }

        private static void ReadTrapArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var trap = (Trap)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    trap = Trap.GetInstance(village);
                    trap.FromJsonReader(reader);

                    trap = null;
                }
            }
        }

        private static void ReadDecorationArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var decoration = (Decoration)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    decoration = Decoration.GetInstance(village);
                    decoration.FromJsonReader(reader);

                    decoration = null;
                }
            }
        }
        #endregion
    }
}
