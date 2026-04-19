using UnityEditor;

namespace BHO.SceneReference.Editor
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
                    SceneRegistry.Add(scene.guid.ToString(), buildIndex);
                }
                
                buildIndex++;
            }
            
            SceneRegistrySaveSystem.Save(SceneRegistry.Scenes);
        }
    }
}
