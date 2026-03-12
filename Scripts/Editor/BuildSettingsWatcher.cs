using UnityEditor;
using UnityEngine;

namespace OnirysGames.SceneReference.Editor
{
    [InitializeOnLoad]
    public static class BuildSettingsWatcher
    {
        static BuildSettingsWatcher()
        {
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        private static void OnSceneListChanged()
        {
            SceneRegistry.Clear();
            
            int buildIndex = 0;
            
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    Debug.Log($"Adding scene {scene.guid.ToString()} to registry: " + scene.path);
                    SceneRegistry.Add(scene.guid.ToString(), buildIndex);
                }
                
                buildIndex++;
            }
            
            foreach (var keyValuePair in SceneRegistry.Scenes)
            {
                Debug.Log($"GUID: {keyValuePair.Key}, Build Index: {keyValuePair.Value}");
            }
            
            SceneRegistry.Save();
        }
    }
}
