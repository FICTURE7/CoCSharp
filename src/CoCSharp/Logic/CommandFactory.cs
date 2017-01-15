using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides methods to create <see cref="Command"/> instances.
    /// </summary>
    public static class CommandFactory
    {
        static CommandFactory()
        {
            s_commandDictionary = new Dictionary<int, Type>();
            Initialize();
        }

        internal static void Initialize()
        {
            s_commandDictionary.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (type.IsSubclassOf(s_commandType))
                {
                    var suppressAttribute = type.GetCustomAttributes<CommandFactorySuppressAttribute>().FirstOrDefault();

                    // Ignore classes that has the CommandFactorySuppressAttribute.
                    if (suppressAttribute != null && suppressAttribute.Suppress)
                        continue;

                    // Create an instance to get command ID.
                    var instance = (Command)Activator.CreateInstance(type);

                    // A command with the same ID as instance.ID was already added to the dictionary.
                    Debug.Assert(!s_commandDictionary.ContainsKey(instance.Id), "s_commandDictionary already contains '" + instance.Id + "'.");
                    s_commandDictionary.Add(instance.Id, type);
                }
            }
        }

        private static readonly Type s_commandType = typeof(Command);
        private static readonly Dictionary<int, Type> s_commandDictionary;

        /// <summary>
        /// Creates a new instance of a <see cref="Command"/> with the specified
        /// command ID.
        /// </summary>
        /// <param name="id">The command ID.</param>
        /// <returns>
        /// The instance of the <see cref="Command"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">Could not find a <see cref="Command"/> with the specified command ID.</exception>"
        public static Command Create(int id)
        {
            var type = (Type)null;
            if (!s_commandDictionary.TryGetValue(id, out type))
                throw new NotSupportedException("Command with ID: " + id + " does not exists or is not implemented.");
            return (Command)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to create a new instance of a <see cref="Command"/> with the specified
        /// command ID. Returns <c>true</c> if the instance was created successfully; otherwise
        /// <c>false</c>.
        /// </summary>
        /// <param name="id">The command ID.</param>
        /// <param name="command">
        /// The instance of the <see cref="Command"/> created. If it returns <c>false</c> then
        /// the instance of the <see cref="Command"/> will be set to its default value.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the instance was created successfully; otherwise <c>false</c>.
        /// </returns>
        public static bool TryCreate(int id, out Command command)
        {
            var type = (Type)null;
            if (!s_commandDictionary.TryGetValue(id, out type))
            {
                command = default(Command);
                return false;
            }
            command = (Command)Activator.CreateInstance(type);
            return true;
        }
    }
}
