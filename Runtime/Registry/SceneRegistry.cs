// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

namespace BHO.SceneReference
{
    public static class SceneRegistry
    {
        private static Dictionary<string, int> guidToIndex;
        private static Dictionary<int, string> indexToGuid;
        
        public static IReadOnlyDictionary<string, int> Registry => GuidToIndex;

        private static Dictionary<string, int> GuidToIndex => guidToIndex ??= new Dictionary<string, int>();

        private static Dictionary<int, string> IndexToGuid
        {
            get
            {
                if (indexToGuid != null)
                    return indexToGuid;

                indexToGuid = new Dictionary<int, string>();
                foreach (KeyValuePair<string, int> pair in GuidToIndex)
                    indexToGuid[pair.Value] = pair.Key;

                return indexToGuid;
            }
        }

        static SceneRegistry()
        {
            SceneRegistrySaveSystem.Load(out guidToIndex);
            guidToIndex ??= new Dictionary<string, int>();
        }

        public static void Add(string guid, int buildIndex)
        {
            if (GuidToIndex.TryAdd(guid, buildIndex))
                indexToGuid = null;
        }

        public static void Remove(string guid)
        {
            if (GuidToIndex.Remove(guid))
                indexToGuid = null;
        }

        public static void Clear()
        {
            GuidToIndex.Clear();
            indexToGuid = null;
        }

        public static string GetName(string guid)
        {
            string path = GetPath(guid);
            return string.IsNullOrEmpty(path) ? string.Empty : Path.GetFileNameWithoutExtension(path);
        }

        public static string GetPath(string guid)
        {
            int buildIndex = GetBuildIndex(guid);
            return buildIndex < 0 ? string.Empty : SceneUtility.GetScenePathByBuildIndex(buildIndex);
        }

        public static int GetBuildIndex(string guid) => GuidToIndex.GetValueOrDefault(guid, -1);

        public static int GetBuildIndex(Scenes scene) => (int)scene;

        public static SceneRef GetScene(Scenes scene)
        {
            return IndexToGuid.TryGetValue((int)scene, out string guid)
                ? new SceneRef(guid)
                : new SceneRef(string.Empty);
        }

#if UNITY_EDITOR
        public static void Reload()
        {
            Clear();

            int buildIndex = 0;
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled)
                    continue;

                Add(scene.guid.ToString(), buildIndex);
                buildIndex++;
            }

            SceneRegistrySaveSystem.Save(GuidToIndex);
        }
#endif
    }
}