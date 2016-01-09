using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CoCSharp.Networking.Messages;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to create <see cref="Message"/> instances.
    /// </summary>
    public static class MessageFactory
    {
        static MessageFactory()
        {
            s_messageType = typeof(Message);
            MessageDictionary = new Dictionary<ushort, Type>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (type.IsSubclassOf(s_messageType))
                {
                    var suppressAttribute = type.GetCustomAttributes<MessageFactorySuppressAttribute>().FirstOrDefault();
                    if (suppressAttribute != null && suppressAttribute.Suppress)
                        continue; // check if the class has the MessageFactorySuppressAttribute

                    var instance = (Message)Activator.CreateInstance(type);
                    if (MessageDictionary.ContainsKey(instance.ID))
                        throw new MessageException("A Message type with the same ID: " + instance.ID + " was already added to the dictionary.", instance);

                    MessageDictionary.Add(instance.ID, type);
                }
            }
        }

        private static readonly Type s_messageType;

        /// <summary>
        /// Gets the dictionary that associates <see cref="Message"/> types with
        /// there ID.
        /// </summary>
        public static Dictionary<ushort, Type> MessageDictionary { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="Message"/> with the specified
        /// message ID.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <returns>The instance of the <see cref="Message"/>.</returns>
        public static Message Create(ushort id)
        {
            var type = (Type)null;
            if (!MessageDictionary.TryGetValue(id, out type))
                return new UnknownMessage() { ID = id };
            return (Message)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to create a new instance of a <see cref="Message"/> with the specified
        /// message ID. Returns <c>true</c> if the instance was created successfully.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <param name="message">The instance of the <see cref="Message"/>.</param>
        /// <returns>Returns <c>true</c> if the instance was created successfully.</returns>
        public static bool TryCreate(ushort id, out Message message)
        {
            var type = (Type)null;
            if (!MessageDictionary.TryGetValue(id, out type))
            {
                message = null;
                return false;
            }
            message = (Message)Activator.CreateInstance(type);
            return true;
        }
    }
}
