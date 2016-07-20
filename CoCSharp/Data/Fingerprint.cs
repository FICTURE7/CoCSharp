using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a Clash of Clans fingerprint.
    /// </summary>
    public class Fingerprint : ICollection<FingerprintFile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class.
        /// </summary>
        public Fingerprint()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Fingerprint"/> class
        /// from the specified path to the fingerprint.json.
        /// </summary>
        /// <param name="path">Path to fingerprint.json</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/>'s length is 0.</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        public Fingerprint(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (path.Length == 0)
                throw new ArgumentException("Empty path is not valid.");
            if (!File.Exists(path))
                throw new FileNotFoundException("Could not find file '" + Path.GetFullPath(path) + "'.");

            _files = new List<FingerprintFile>();
            var json = File.ReadAllText(path);
            var fingerprint = FromJson(json);

            _files = fingerprint._files;
            MasterHash = fingerprint.MasterHash;
            Version = fingerprint.Version;
        }

        /// <summary>
        /// Gets or sets the <see cref="FingerprintFile"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The <see cref="FingerprintFile"/> at the specified index.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is invalid.</exception>
        public FingerprintFile this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", "index must be greater or equal to 0.");
                if (index > _files.Count - 1)
                    throw new ArgumentOutOfRangeException("index", "index must be less or equal to Count - 1.");

                return _files[index];
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("Fingerprint object is readonly.");
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", "index must be greater or equal to 0.");
                if (index > _files.Count - 1)
                    throw new ArgumentOutOfRangeException("index", "index must be less or equal to Count - 1.");

                _files[index] = value;
            }
        }

        private byte[] _masterHash;
        /// <summary>
        /// Gets or sets the SHA-1 master hash of the <see cref="Fingerprint"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public byte[] MasterHash
        {
            get
            {
                return _masterHash;
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("Fingerprint object is readonly.");

                _masterHash = value;
            }
        }

        private string _version;
        /// <summary>
        /// Gets or sets the version of the <see cref="Fingerprint"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("Fingerprint object is readonly.");

                _version = value;
            }
        }

        /// <summary>
        /// Gets the number of <see cref="FingerprintFile"/> in the <see cref="Fingerprint"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _files.Count;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Fingerprint"/> is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        private List<FingerprintFile> _files;

        /// <summary>
        /// Computes the master hash of the overall fingerprint file.
        /// </summary>
        /// <returns>Master hash of the overall fingerprint file.</returns>
        public byte[] ComputeMasterHash()
        {
            var hashes = new StringBuilder();

            // Appends all hashes of the FingerprintFiles into hex-strings.
            for (int i = 0; i < _files.Count; i++)
            {
                var hash = InternalUtils.BytesToString(_files[i].Hash);
                hashes.Append(hash);
            }

            var hashesBytes = Encoding.UTF8.GetBytes(hashes.ToString());
            using (var sha1 = SHA1.Create())
                return sha1.ComputeHash(hashesBytes);
        }

        /// <summary>
        /// Returns a non-indented JSON string that represents the current <see cref="Fingerprint"/>.
        /// </summary>
        /// <returns>A non-indented JSON string that represents the current <see cref="Fingerprint"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Fingerprint"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Fingerprint"/>.</returns>
        public string ToJson(bool indent)
        {
            var jsonStr = string.Empty;

            var textWriter = new StringWriter();
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                // Turn on indentation if "indent" is set to true.
                if (indent)
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 4;
                }

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("files");
                jsonWriter.WriteStartArray();

                for (int i = 0; i < _files.Count; i++)
                {
                    jsonWriter.WriteStartObject();

                    jsonWriter.WritePropertyName("sha");
                    jsonWriter.WriteValue(InternalUtils.BytesToString(_files[i].Hash));

                    jsonWriter.WritePropertyName("path");
                    jsonWriter.WriteValue(_files[i].Path);

                    jsonWriter.WriteEndObject();
                }

                jsonWriter.WriteEndArray();

                jsonWriter.WritePropertyName("sha");
                jsonWriter.WriteValue(InternalUtils.BytesToString(MasterHash));

                jsonWriter.WritePropertyName("version");
                jsonWriter.WriteValue(Version);

                jsonWriter.WriteEndObject();

                jsonStr = textWriter.ToString();
            }

            //return indent == true ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this);
            return jsonStr;
        }

        /// <summary>
        /// Returns a <see cref="Fingerprint"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Fingerprint"/>.</param>
        /// <returns>A <see cref="Fingerprint"/> object that is deserialized from the specified JSON string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> is empty.</exception>
        public static Fingerprint FromJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            var fingerprint = new Fingerprint();
            var fingerprintFiles = new List<FingerprintFile>();
            var file = new FingerprintFile();

            // Determines if we're inside the "files" array.
            var inFilesArray = false;

            using (var txtReader = new StringReader(value))
            using (var jsonReader = new JsonTextReader(txtReader))
            {
                // Keep on reading the next JSON token.
                while (jsonReader.Read())
                {
                    switch (jsonReader.TokenType)
                    {
                        // If we hit a token of type PropertyName we read its value
                        // and determine where to assign it.
                        case JsonToken.PropertyName:
                            var propertyName = (string)jsonReader.Value;
                            switch (propertyName)
                            {
                                case "files":
                                    if (jsonReader.Read())
                                    {
                                        if (jsonReader.TokenType != JsonToken.StartArray)
                                        {
                                            // Throw an exception here maybe?
                                        }
                                        else
                                        {
                                            inFilesArray = true;
                                        }
                                    }
                                    else
                                    {
                                        // Throw an exception here maybe?
                                        break;
                                    }
                                    break;

                                case "file":
                                    if (!inFilesArray)
                                        break;

                                    file.Path = jsonReader.ReadAsString();
                                    break;

                                case "sha":
                                    // Convert the "sha" string into a byte array.
                                    var str = jsonReader.ReadAsString();
                                    if (str == null || str.Length != 40)
                                        return null;

                                    var bytes = new byte[str.Length / 2];
                                    for (int i = 0; i < bytes.Length; i++)
                                        bytes[i] = byte.Parse(str.Substring(i * 2, 2), NumberStyles.HexNumber);

                                    // If we're inside the "files" array assign it to the
                                    // FingerprintFile.
                                    if (inFilesArray)
                                        file.Hash = bytes;
                                    // If not assign it to the Fingerprint itself.
                                    else
                                        fingerprint.MasterHash = bytes;
                                    break;

                                case "version":
                                    fingerprint.Version = jsonReader.ReadAsString();
                                    break;
                            }
                            break;

                        case JsonToken.EndObject:
                            // Reset the file object when we hit EndObject token
                            // to allow new creation of FingerprintFile.
                            if (inFilesArray)
                            {
                                fingerprintFiles.Add(file);
                                file = new FingerprintFile();
                            }
                            break;

                        case JsonToken.EndArray:
                            inFilesArray = false;
                            break;
                    }
                }
            }

            fingerprint._files = fingerprintFiles;
            return fingerprint;
        }

        /// <summary>
        /// Creates a new <see cref="Fingerprint"/> for the specified directory path.
        /// </summary>
        /// <param name="path">Path pointing to the directory.</param>
        /// <returns>A <see cref="Fingerprint"/> representing the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is empty or white space.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> does not exits.</exception>
        public static Fingerprint Create(string path)
        {
            return Create(path, null);
        }

        /// <summary>
        /// Creates a new <see cref="Fingerprint"/> for the specified directory path and specified version.
        /// </summary>
        /// <param name="path">Path pointing to the directory.</param>
        /// <param name="version">Version of the <see cref="Fingerprint"/>.</param>
        /// <returns>A <see cref="Fingerprint"/> representing the specified directory.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is empty or white space.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> does not exits.</exception>
        public static Fingerprint Create(string path, string version)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Empty path is not valid.", "path");
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException("Could not find directory at '" + path + "'.");

            var fingerprint = new Fingerprint();
            var fingerprintFiles = new List<FingerprintFile>();
            var filePaths = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

            using (var sha1 = SHA1.Create())
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    var filePath = RemovePathRootDir(filePaths[i]);
                    var fileBytes = File.ReadAllBytes(filePaths[i]);
                    var fingerprintFile = new FingerprintFile();

                    fingerprintFile.Path = filePath;
                    fingerprintFile.Hash = sha1.ComputeHash(fileBytes);

                    fingerprintFiles.Add(fingerprintFile);
                }
            }

            fingerprint._files = fingerprintFiles;
            fingerprint.MasterHash = fingerprint.ComputeMasterHash();
            fingerprint.Version = version;

            return fingerprint;
        }

        /// <summary>
        /// Adds a <see cref="FingerprintFile"/> to the <see cref="Fingerprint"/>.
        /// </summary>
        /// <param name="item">The <see cref="FingerprintFile"/> to add to the <see cref="Fingerprint"/>.</param>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public void Add(FingerprintFile item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Fingerprint object is readonly.");
            if (item == null)
                throw new ArgumentNullException("item");

            _files.Add(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific <see cref="FingerprintFile"/> from the <see cref="Fingerprint"/>.
        /// </summary>
        /// <param name="item">The <see cref="FingerprintFile"/> to remove from the <see cref="Fingerprint"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="item"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Remove(FingerprintFile item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Fingerprint object is readonly.");
            if (item == null)
                throw new ArgumentNullException("item");

            return _files.Remove(item);
        }

        /// <summary>
        /// Removes all <see cref="FingerprintFile"/> from the <see cref="Fingerprint"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Fingerprint object is readonly.");

            _files.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="Fingerprint"/> contains a specific <see cref="FingerprintFile"/>.
        /// </summary>
        /// <param name="item">The <see cref="FingerprintFile"/> to locate in the <see cref="Fingerprint"/>.</param>
        /// <returns><c>true</c> if the <paramref name="item"/> was found in the <see cref="Fingerprint"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Contains(FingerprintFile item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return _files.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="Fingerprint"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="Fingerprint"/>.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> exceeds the bounds of <paramref name="array"/>.</exception>
        public void CopyTo(FingerprintFile[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0 || arrayIndex > array.Length - 1)
                throw new ArgumentOutOfRangeException("arrayIndex exceeds the bounds of array.", "arrayIndex");

            _files.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Fingerprint"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{FingerprintFile}"/> for the <see cref="Fingerprint"/>.</returns>
        public IEnumerator<FingerprintFile> GetEnumerator()
        {
            return new Enumerator(_files.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<FingerprintFile>
        {
            public Enumerator(FingerprintFile[] files)
            {
                _index = -1;
                _files = files;
            }

            private int _index;
            private FingerprintFile[] _files;

            public FingerprintFile Current
            {
                get
                {
                    return _files[_index];
                }
                set
                {
                    _files[_index] = value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                _index++;
                return (_index < _files.Length);
            }

            public void Reset()
            {
                _index = -1;
            }

            // Nothing to dispose.
            public void Dispose()
            {
                // Space
            }
        }

        // Removes the first directory from a path string.
        private static string RemovePathRootDir(string path)
        {
            // Tries if the path contains a separator character.
            var separatorIndex = path.IndexOf(Path.DirectorySeparatorChar);

            // If does not contain a separator character then
            // tries the alternative separator character.
            if (separatorIndex == -1)
            {
                separatorIndex = path.IndexOf(Path.AltDirectorySeparatorChar);

                // If still does not contain a separator character then
                // returns the original path.
                if (separatorIndex == -1)
                    return path;
            }

            return path.Remove(0, separatorIndex + 1);
        }
    }
}
