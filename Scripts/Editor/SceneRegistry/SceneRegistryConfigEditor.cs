using UnityEditor;

namespace OnirysGames.SceneReference.Editor
{
    [CustomEditor(typeof(SceneRegistryConfig))]
    public class SceneRegistryConfigEditor : UnityEditor.Editor
    {
        private SerializedProperty keyProvider;
        private SerializedProperty customKeyProvider;
        private SerializedProperty customServerUrl;

        private void OnEnable()
        {
            keyProvider = serializedObject.FindProperty("keyProvider");
            customKeyProvider = serializedObject.FindProperty("customKey");
            customServerUrl = serializedObject.FindProperty("serverUrl");
        }

        public override void OnInspectorGUI()
        {
            SceneRegistryConfig config = (SceneRegistryConfig)target;
            
            keyProvider.enumValueIndex = EditorGUILayout.Popup("Key Provider", keyProvider.enumValueIndex, keyProvider.enumDisplayNames);

            if (keyProvider.enumValueIndex == (int)KeyProviderType.Custom)
            {
                customKeyProvider.stringValue = EditorGUILayout.TextField("Custom Key", config.CustomKey);
            }
            else if (keyProvider.enumValueIndex == (int)KeyProviderType.ServerUrl)
            {
                customServerUrl.stringValue = EditorGUILayout.TextField("Server URL", config.ServerUrl);
            }
        }
    }
}