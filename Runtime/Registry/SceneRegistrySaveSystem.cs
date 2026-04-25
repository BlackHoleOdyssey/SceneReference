// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace BHO.SceneReference
{
    public static class SceneRegistrySaveSystem
    {
        private static ISceneRegistryStorage storage;

        private static ISceneRegistryStorage Storage => storage ??= CreateStorage();

        private static ISceneRegistryStorage CreateStorage()
        {
            SceneRegistryConfig config = SceneRegistryConfig.Instance;
            if (config == null)
                return new GeneratedCodeStorage();

            return config.StorageMode switch
            {
                StorageMode.StreamingAssets => new StreamingAssetsStorage(),
                _ => new GeneratedCodeStorage()
            };
        }
        
        public static void Load(out Dictionary<string, int> scenes) => Storage.Load(out scenes);

#if UNITY_EDITOR
        public static void Save(Dictionary<string, int> scenes) => Storage.Save(scenes);
#endif
        public static void ResetStorage()
        {
            storage = null;
        }
    }

    [Serializable]
    internal class SceneRegistryData
    {
        public List<string> guids = new();
        public List<int> buildIndexes = new();

        public Dictionary<string, int> ToDictionary()
        {
            Dictionary<string, int> dict = new();
            for (int i = 0; i < guids.Count; i++)
            {
                dict[guids[i]] = buildIndexes[i];
            }

            return dict;
        }

        public static SceneRegistryData FromDictionary(Dictionary<string, int> dict)
        {
            SceneRegistryData data = new();
            foreach (var kvp in dict)
            {
                data.guids.Add(kvp.Key);
                data.buildIndexes.Add(kvp.Value);
            }

            return data;
        }
    }
}