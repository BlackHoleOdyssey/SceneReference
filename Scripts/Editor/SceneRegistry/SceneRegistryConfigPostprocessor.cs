// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace BHO.SceneReference.Editor
{
    public class SceneRegistryConfigPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string path in importedAssets)
            {
                SceneRegistryConfig config = AssetDatabase.LoadAssetAtPath<SceneRegistryConfig>(path);
                if (config != null)
                {
                    AddToPreloadedAssets(config);
                }
            }
        }

        private static void AddToPreloadedAssets(SceneRegistryConfig config)
        {
            List<Object> preloaded = PlayerSettings.GetPreloadedAssets().ToList();
            
            if (!preloaded.Contains(config))
            {
                preloaded.Add(config);
                PlayerSettings.SetPreloadedAssets(preloaded.ToArray());
                Debug.Log("[SceneRegistry] SceneRegistryConfig added to Preloaded Assets.");
            }
        }
    }
}