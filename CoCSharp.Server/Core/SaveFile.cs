using System;
using System.Collections;
using System.IO;

namespace CoCSharp.Server.Core
{
    public class SaveFile
    {
        public SaveFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            Path = path;
            _keys = new Hashtable();
            Load();
        }

        public string Path { get; private set; }

        private Hashtable _keys;

        public string GetKey(string name)
        {
            if (!_keys.ContainsKey(name))
                throw new Exception("Unknown key '" + name + "'.");

            return _keys[name] as string;
        }

        private void Load()
        {
            var keysLines = File.ReadAllLines(Path);
            for (int i = 0; i < keysLines.Length; i++)
            {
                var line = keysLines[i].Split('=');
                _keys.Add(line[0], line[1]);
            }
        }
    }
}
