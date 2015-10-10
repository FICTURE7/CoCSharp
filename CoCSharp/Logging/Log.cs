using System.IO;

namespace CoCSharp.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Log
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Log(string name)
        {
            Name = name;
            Path = name + ".log";
            LogString = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public Log(string name, string path)
        {
            Name = name;
            Path = path;
            LogString = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        protected string LogString { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Save()
        {
            File.WriteAllText(Path, LogString);
        }
    }
}
