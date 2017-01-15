﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using CoCSharp.Network.Messages;
using System.Diagnostics;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides methods to create <see cref="Message"/> instances.
    /// </summary>
    public static class MessageFactory
    {
        static MessageFactory()
        {
            s_messageDictionary = new Dictionary<ushort, Type>();
            Initialize();
        }

        internal static void Initialize()
        {
            s_messageDictionary.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (type.IsSubclassOf(s_messageType))
                {
                    var suppressAttribute = type.GetCustomAttributes<MessageFactorySuppressAttribute>().FirstOrDefault();

                    // Ignore classes that has the MessageFactorySuppressAttribute.
                    if (suppressAttribute != null && suppressAttribute.Suppress)
                        continue;

                    // Create an instance to get the message ID.
                    var instance = (Message)Activator.CreateInstance(type);

                    // A message with the same ID as instance.ID was already added to the dictionary.
                    Debug.Assert(!s_messageDictionary.ContainsKey(instance.Id), "s_messageDictionary already contains '" + instance.Id + "'.");
                    s_messageDictionary.Add(instance.Id, type);
                }
            }
        }

        private static readonly Type s_messageType = typeof(Message);
        private static readonly Dictionary<ushort, Type> s_messageDictionary;

        /// <summary>
        /// Creates a new instance of a <see cref="Message"/> with the specified
        /// message ID.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <returns>
        /// The instance of the <see cref="Message"/>. If no <see cref="Message"/> with the specified message ID was
        /// found then a new instance of the <see cref="UnknownMessage"/> will returned.
        /// </returns>
        public static Message Create(ushort id)
        {
            var type = (Type)null;
            if (!s_messageDictionary.TryGetValue(id, out type))
                return new UnknownMessage() { Id = id };
            return (Message)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Tries to create a new instance of a <see cref="Message"/> with the specified
        /// message ID. Returns <c>true</c> if the instance was created successfully; otherwise
        /// <c>false</c>.
        /// </summary>
        /// <param name="id">The message ID.</param>
        /// <param name="message">
        /// The instance of the <see cref="Message"/> created. If it returns <c>false</c> then
        /// the instance of the <see cref="Message"/> will be set to its default value.
        /// </param>
        /// <returns>
        /// Returns <c>true</c> if the instance was created successfully; otherwise <c>false</c>.
        /// </returns>
        public static bool TryCreate(ushort id, out Message message)
        {
            var type = (Type)null;
            if (!s_messageDictionary.TryGetValue(id, out type))
            {
                message = default(Message);
                return false;
            }
            message = (Message)Activator.CreateInstance(type);
            return true;
        }
    }
}
