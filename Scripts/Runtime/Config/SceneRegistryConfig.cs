// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using UnityEngine;

namespace BHO.SceneReference
{
    public enum KeyProviderType
    {
        Custom
    }

    [CreateAssetMenu(fileName = "SceneRegistryConfig", menuName = "BHO/SceneRegistryConfig")]
    public class SceneRegistryConfig : ScriptableObject
    {
        [SerializeField, HideInInspector] private KeyProviderType keyProvider;
        [SerializeField, HideInInspector] private string customKey = "";
        [HideInInspector] public StorageMode StorageMode;

        public KeyProviderType KeyProvider => keyProvider;
        public string CustomKey => customKey;

        public static SceneRegistryConfig Instance
        {
            get
            {
                if (instance == null)
                    instance = Load();
                return instance;
            }
            private set => instance = value;
        }

        private static SceneRegistryConfig instance;

        private static SceneRegistryConfig Load()
        {
#if UNITY_EDITOR
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:SceneRegistryConfig");
            if (guids.Length > 0)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                return UnityEditor.AssetDatabase.LoadAssetAtPath<SceneRegistryConfig>(path);
            }

            return null;
#else
        SceneRegistryConfig[] configs = Resources.FindObjectsOfTypeAll<SceneRegistryConfig>();
        return configs.Length > 0 ? configs[0] : null;
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadConfig()
        {
            Instance = Load();

            if (Instance != null)
            {
                Debug.Log($"[SceneRegistry] Loaded {Instance.name}.");
            }
            else
            {
                Debug.LogWarning("[SceneRegistry] No SceneRegistryConfig found.");
            }
        }
    }
}