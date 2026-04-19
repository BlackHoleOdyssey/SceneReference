using UnityEditor;

namespace BHO.SceneReference.Editor
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
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            keyProvider.enumValueIndex = EditorGUILayout.Popup("Key Provider", keyProvider.enumValueIndex, keyProvider.enumDisplayNames);
            customKeyProvider.stringValue = EditorGUILayout.TextField("Custom Key", customKeyProvider.stringValue);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}