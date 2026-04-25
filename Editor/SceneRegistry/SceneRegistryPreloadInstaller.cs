// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace BHO.SceneReference.Editor
{
    public class SceneRegistryPreloadInstaller : AssetPostprocessor
    {
       private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool configChange = importedAssets.Any(path => path.EndsWith(".asset")) || movedAssets.Any(path => path.EndsWith(".asset"));

            if (configChange)
            {
                PreloadSceneRegistry();
            }
        }

        private static void PreloadSceneRegistry()
        {
            string[] guids = AssetDatabase.FindAssets("t:SceneRegistryConfig");
            
            if (guids.Length == 0) 
                return;
            
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            SceneRegistryConfig config = AssetDatabase.LoadAssetAtPath<SceneRegistryConfig>(path);

            if (config == null)
                return;
            
            Object[] preloadedAssets = PlayerSettings.GetPreloadedAssets();

            if (!preloadedAssets.Contains(config))
            {
                List<Object> newList = preloadedAssets.ToList();
                newList.Add(config);
                
                PlayerSettings.SetPreloadedAssets(newList.ToArray());
                
                Debug.Log($"[SceneRegistry] Added {config.name} to preloaded assets.");
            }
        }
    }
}
