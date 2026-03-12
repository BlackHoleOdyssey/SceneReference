using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OnirysGames.SceneReference
{
    public static class SceneRegistry
    {
        private static Dictionary<string, int> scenes = new();
        private static readonly string FilePath = Path.Combine(Application.streamingAssetsPath, "SceneRegistry.json");
        
        public static Dictionary<string, int> Scenes => scenes;
        
        static SceneRegistry()
        {
            Load();
        }


        public static void Add(string guid, int buildIndex)
        {
            scenes.TryAdd(guid, buildIndex);
        }

        public static void Remove(string guid)
        {
            scenes.Remove(guid);
        }

        public static void Clear()
        {
            scenes.Clear();
        }
        
        public static string GetName(string guid)
        {
            return string.Empty;
        }

        public static string GetPath(string guid)
        {
            return string.Empty;
        }

        public static int GetBuildIndex(string guid)
        {
            return scenes.GetValueOrDefault(guid, -1);
        }

        public static void Load()
        {
            if (!File.Exists(FilePath)) return;

            string json = File.ReadAllText(FilePath);
            SceneRegistryData data = JsonUtility.FromJson<SceneRegistryData>(json);
            scenes = data.ToDictionary();
        }
        
        #if UNITY_EDITOR
        public static void Save()
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);

            string json = JsonUtility.ToJson(SceneRegistryData.FromDictionary(scenes), true);
            File.WriteAllText(FilePath, json);

            UnityEditor.AssetDatabase.Refresh();
        }
        #endif
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
                dict[guids[i]] = buildIndexes[i];
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
