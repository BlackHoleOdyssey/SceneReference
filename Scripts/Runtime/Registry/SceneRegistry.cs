using System.Collections.Generic;
using UnityEditor;

namespace BHO.SceneReference
{
    public static class SceneRegistry
    {
        private static Dictionary<string, int> scenes;

        public static Dictionary<string, int> Scenes => scenes;

        static SceneRegistry()
        {
            SceneRegistrySaveSystem.Load(out scenes);
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

#if UNITY_EDITOR
        public static void Reload()
        {
            Clear();
            int buildIndex = 0;
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    Add(scene.guid.ToString(), buildIndex);
                    buildIndex++;
                }
            }
        }
#endif
    }
}