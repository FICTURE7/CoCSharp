using CoCSharp.Data.Slots;
using CoCSharp.Logic;
using System.Collections.Generic;
using System.IO;

namespace CoCSharp.Server.Core
{
    public class NpcManager // : INpcManager
    {
        private const string NpcVillageFilePrefix = "npc";

        public NpcManager()
        {
            _loadedNpc = new Dictionary<int, Village>();
            _completeNpcStartList = GetCompleteNpcStart();
        }

        public NpcStarSlot[] CompleteNpcStarList
        {
            get { return _completeNpcStartList; }
        }

        private NpcStarSlot[] _completeNpcStartList;
        private Dictionary<int, Village> _loadedNpc;

        public Village LoadNpc(int id)
        {
            if (_loadedNpc.ContainsKey(id))
                return _loadedNpc[id];

            // Adding 1 to skip tutorial levels.
            var index = id - (17000000 + 1);
            var files = Directory.GetFiles(DirectoryPaths.Npc);
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                if (fileName == NpcVillageFilePrefix + index)
                {
                    var villageJson = File.ReadAllText(files[i]);
                    var village = Village.FromJson(villageJson);
                    _loadedNpc.Add(id, village);
                    return village;
                }
            }
            return null;
        }

        // Generates a complete list of NPCs.
        private NpcStarSlot[] GetCompleteNpcStart()
        {
            var prefixLength = NpcVillageFilePrefix.Length;
            var list = new List<NpcStarSlot>();
            var files = Directory.GetFiles(DirectoryPaths.Npc);
            for (int i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                if (fileName.Length <= prefixLength)
                    continue;

                var indexStr = fileName.Substring(prefixLength, fileName.Length - prefixLength);
                var index = int.Parse(indexStr);
                var id = 17000000 + index;

                list.Add(new NpcStarSlot(id, 3));
            }

            return list.ToArray();
        }
    }
}
