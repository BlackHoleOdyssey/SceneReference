// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using UnityEditor;

namespace BHO.SceneReference.Editor
{
    [InitializeOnLoad]
    public static class BuildSettingsWatcher
    {
        static BuildSettingsWatcher()
        {
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
            EditorApplication.quitting += () => SceneRegistrySaveSystem.Save(SceneRegistry.Scenes);
        }

        private static void OnSceneListChanged()
        {
            SceneRegistry.Clear();
            
            int buildIndex = 0;
            
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    SceneRegistry.Add(scene.guid.ToString(), buildIndex);
                    buildIndex++;
                }
            }
            
            SceneRegistrySaveSystem.Save(SceneRegistry.Scenes);
        }
    }
}
