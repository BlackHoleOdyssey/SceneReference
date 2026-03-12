using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OnirysGames.SceneReference.Editor
{
    [CustomPropertyDrawer(typeof(SceneReference), true)]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        private const float LINE_HEIGHT = 20;
        private const float SPACING = 2f;
        private bool foldout;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty guidProperty = property.FindPropertyRelative("guid");
            SerializedProperty assetProperty = property.FindPropertyRelative("sceneAsset");

            string currentPath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(currentPath);

            Rect objectField = new Rect(position.x, position.y, position.width, LINE_HEIGHT);

            EditorGUI.BeginChangeCheck();
            SceneAsset newAsset = EditorGUI.ObjectField(objectField, label, sceneAsset, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                if (newAsset != null)
                {
                    string newPath = AssetDatabase.GetAssetPath(newAsset);
                    guidProperty.stringValue = AssetDatabase.AssetPathToGUID(newPath);
                    assetProperty.objectReferenceValue = newAsset;
                }
                else
                {
                    guidProperty.stringValue = string.Empty;
                    assetProperty.objectReferenceValue = null;
                }
            }

            if (!string.IsNullOrEmpty(guidProperty.stringValue))
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);
                GUI.enabled = false;

                Rect foldoutRect = new Rect(position.x, position.y + LINE_HEIGHT + SPACING, position.width, LINE_HEIGHT);
                foldout = EditorGUI.Foldout(foldoutRect, foldout, "Scene Info", true);

                if (foldout)
                {
                    EditorGUI.indentLevel++;

                    float y = position.y + (LINE_HEIGHT + SPACING) * 2;

                    int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
                    string buildIndexLabel = buildIndex != -1
                        ? buildIndex.ToString()
                        : "Not in Build Settings";
                    
                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Guid", guidProperty.stringValue);
                    y += LINE_HEIGHT + SPACING;

                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Scene Path", scenePath);
                    y += LINE_HEIGHT + SPACING;

                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Build Index", buildIndexLabel);

                    EditorGUI.indentLevel--;
                }

                GUI.enabled = true;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty guidProperty = property.FindPropertyRelative("guid");
            bool hasAsset = !string.IsNullOrEmpty(guidProperty.stringValue);

            if (!hasAsset)
            {
                return LINE_HEIGHT;
            }

            float height = LINE_HEIGHT * 2 + SPACING;
            if (foldout)
            {
                height += (LINE_HEIGHT + SPACING) * 3; // 3 lines of info
            }

            return height;
        }
    }
}