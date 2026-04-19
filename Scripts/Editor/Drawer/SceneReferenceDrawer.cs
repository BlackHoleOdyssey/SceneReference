// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHO.SceneReference.Editor
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

            float buttonWidth = 100;
            float y = position.y;
            
            Rect objectField = new Rect(position.x, y, position.width - buttonWidth - SPACING, LINE_HEIGHT);
            Rect addSceneButton = new Rect(position.x + position.width - buttonWidth, y, buttonWidth, LINE_HEIGHT);
            y += LINE_HEIGHT + SPACING;

            EditorGUI.BeginChangeCheck();
            SceneAsset newAsset = EditorGUI.ObjectField(objectField, label, sceneAsset, typeof(SceneAsset), false) as SceneAsset;

            if (GUI.Button(addSceneButton, "Add to scenes"))
            {
                if (sceneAsset != null)
                {
                    string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                    if (IsSceneInBuildSettings(scenePath))
                    {
                        Debug.LogWarning($"Scene '{sceneAsset.name}' is already in Build Settings.");
                        return;
                    }

                    AddSceneToBuildSettings(scenePath);
                    Debug.Log($"Scene '{sceneAsset.name}' added to Build Settings.");
                }
            }

            if (sceneAsset != null && !IsSceneInBuildSettings(AssetDatabase.GetAssetPath(sceneAsset)))
            {
                EditorGUI.HelpBox(new Rect(position.x, y, position.width, LINE_HEIGHT), "Scene is not in Build Settings!", MessageType.Warning);
                y += LINE_HEIGHT + SPACING;
            }

            if (!string.IsNullOrEmpty(guidProperty.stringValue))
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);

                foldout = EditorGUI.Foldout(new Rect(position.x, y, position.width, LINE_HEIGHT), foldout, "Scene Info", true);
                y += LINE_HEIGHT + SPACING;

                if (foldout)
                {
                    GUI.enabled = false;
                    EditorGUI.indentLevel++;

                    int buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
                    string buildIndexLabel = buildIndex != -1 ? buildIndex.ToString() : "Not in Build Settings";

                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Guid", guidProperty.stringValue);
                    y += LINE_HEIGHT + SPACING;
                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Scene Path", scenePath);
                    y += LINE_HEIGHT + SPACING;
                    EditorGUI.LabelField(new Rect(position.x, y, position.width, LINE_HEIGHT), "Build Index", buildIndexLabel);

                    EditorGUI.indentLevel--;
                    GUI.enabled = true;
                }
            }

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

            EditorGUI.EndProperty();
        }

        private void AddSceneToBuildSettings(string scenePath)
        {
            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
            EditorBuildSettingsScene newScene = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = existingScenes.Concat(new[] { newScene }).ToArray();
        }

        private bool IsSceneInBuildSettings(string scenePath)
        {
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene.path == scenePath)
                {
                    return true;
                }
            }

            return false;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty guidProperty = property.FindPropertyRelative("guid");
            bool hasAsset = !string.IsNullOrEmpty(guidProperty.stringValue);

            float height = LINE_HEIGHT + SPACING;

            if (!hasAsset)
            {
                return height;
            }

            string scenePath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);

            if (!IsSceneInBuildSettings(scenePath))
            {
                height += LINE_HEIGHT + SPACING;
            } 

            height += LINE_HEIGHT + SPACING;

            if (foldout)
            {
                height += (LINE_HEIGHT + SPACING) * 3;
            }

            return height;
        }
    }
}