using CoCSharp.Network.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides method to create instances of <see cref="StreamEntry"/>.
    /// </summary>
    public static class StreamEntryFactory
    {
        static StreamEntryFactory()
        {
            s_allianceStreamDictionary = new Dictionary<int, Type>();
            s_avatarStreamDictionary = new Dictionary<int, Type>();
            Initialize();
        }

        internal static void Initialize()
        {
            s_allianceStreamDictionary.Clear();
            s_avatarStreamDictionary.Clear();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (!type.IsSubclassOf(typeof(StreamEntry)) || type.IsAbstract)
                    continue;

                var suppressAttribute = type.GetCustomAttributes<StreamEntrySuppressAttribute>().FirstOrDefault();
                if (suppressAttribute != null && suppressAttribute.Suppress)
                    continue;

                var instance = (StreamEntry)Activator.CreateInstance(type);

                if (type.IsSubclassOf(typeof(AllianceStreamEntry)))
                {
                    Debug.Assert(!s_allianceStreamDictionary.ContainsKey(instance.Id), "s_allianceStreamDictionary already contains '" + instance.Id + "'.");
                    s_allianceStreamDictionary.Add(instance.Id, type);
                }
                else if (type.IsSubclassOf(typeof(AvatarStreamEntry)))
                {
                    Debug.Assert(!s_avatarStreamDictionary.ContainsKey(instance.Id), "s_avatarStreamDictionary already contains '" + instance.Id + "'.");
                    s_avatarStreamDictionary.Add(instance.Id, type);
                }
                else
                {
                    Debug.Fail("Unexpected type " + type);
                }
            }
        }

        private static readonly Dictionary<int, Type> s_allianceStreamDictionary;
        private static readonly Dictionary<int, Type> s_avatarStreamDictionary;

        /// <summary>
        /// Creates a new instance of an <see cref="AllianceStreamEntry"/> with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="AllianceStreamEntry"/>.</param>
        /// <returns>
        /// The instance of the <see cref="AllianceStreamEntry"/>. If no <see cref="AllianceStreamEntry"/> with the specified message ID was
        /// found then null will be returned.
        /// </returns>
        public static AllianceStreamEntry CreateAllianceStreamEntry(int id)
        {
            var type = (Type)null;
            if (!s_allianceStreamDictionary.TryGetValue(id, out type))
                return null;
            return (AllianceStreamEntry)Activator.CreateInstance(type);
        }

        /// <summary>
        /// Creates a new instance of an <see cref="AvatarStreamEntry"/> with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the <see cref="AvatarStreamEntry"/>.</param>
        /// <returns>
        /// The instance of the <see cref="AvatarStreamEntry"/>. If no <see cref="AvatarStreamEntry"/> with the specified message ID was
        /// found then null will be returned.
        /// </returns>
        public static AvatarStreamEntry CreateAvatarStreamEntry(int id)
        {
            var type = (Type)null;
            if (!s_avatarStreamDictionary.TryGetValue(id, out type))
                return null;
            return (AvatarStreamEntry)Activator.CreateInstance(type);
        }
    }
}
