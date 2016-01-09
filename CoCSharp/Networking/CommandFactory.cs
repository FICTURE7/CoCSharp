using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to create <see cref="Command"/> instances.
    /// </summary>
    public static class CommandFactory
    {
        static CommandFactory()
        {
            s_commandType = typeof(Command);
            CommandDictionary = new Dictionary<int, Type>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (type.IsSubclassOf(s_commandType))
                {
                    var suppressAttribute = type.GetCustomAttributes<CommandFactorySuppressAttribute>().FirstOrDefault();
                    if (suppressAttribute != null && suppressAttribute.Suppress)
                        continue; // check if the class has the MessageFactorySuppressAttribute

                    var instance = (Command)Activator.CreateInstance(type);
                    if (CommandDictionary.ContainsKey(instance.ID))
                        throw new CommandException("A Command type with the same ID: " + instance.ID + " was already added to the dictionary.", instance);

                    CommandDictionary.Add(instance.ID, type);
                }
            }
        }

        private static readonly Type s_commandType;

        /// <summary>
        /// Gets the dictionary that associates <see cref="Command"/> types with
        /// there ID.
        /// </summary>
        public static Dictionary<int, Type> CommandDictionary { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="Command"/> with the specified
        /// command ID.
        /// </summary>
        /// <param name="id">The command ID.</param>
        /// <returns>The instance of the <see cref="Command"/>.</returns>
        public static Command Create(int id)
        {
            var type = (Type)null;
            if (!CommandDictionary.TryGetValue(id, out type))
                throw new NotSupportedException("Command with ID: " + id + " does not exists or is not implemented.");
            return (Command)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to create a new instance of a <see cref="Command"/> with the specified
        /// command ID. Returns <c>true</c> if the instance was created successfully.
        /// </summary>
        /// <param name="id">The command ID.</param>
        /// <param name="command">The instance of the <see cref="Command"/>.</param>
        /// <returns>Returns <c>true</c> if the instance was created successfully.</returns>
        public static bool TryCreate(int id, out Command command)
        {
            var type = (Type)null;
            if (!CommandDictionary.TryGetValue(id, out type))
            {
                command = null;
                return false;
            }
            command = (Command)Activator.CreateInstance(type);
            return true;
        }
    }
}
