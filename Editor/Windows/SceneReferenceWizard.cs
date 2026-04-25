// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BHO.SceneReference.Editor
{
    [InitializeOnLoad]
    public static class SceneReferenceWizard
    {
        private const string WIZARD_SHOWN_KEY = "BHO.SceneReference.WizardShown";
        
        static SceneReferenceWizard()
        {
            if (!EditorPrefs.GetBool(WIZARD_SHOWN_KEY, false))
            {
                EditorApplication.update += ShowWizard;
            }
        }

        private static void ShowWizard()
        {
            EditorApplication.update -= ShowWizard;
            EditorPrefs.SetBool(WIZARD_SHOWN_KEY, true);
            SceneRegistryWizardWindow.Open();
        }
    }

    public class SceneRegistryWizardWindow : EditorWindow
    {
        [MenuItem("BHO/Scene Reference")]
        public static void Open()
        {
            SceneRegistryWizardWindow window = GetWindow<SceneRegistryWizardWindow>("Scene Reference Setup");
            window.minSize = new Vector2(500, 265);
            window.maxSize = new Vector2(500.01f, 265.01f);
            window.ShowUtility();
        }

        private void OnGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, new Color32(167, 239, 241, 255));
            
            GUILayout.Label("Welcome to Scene Reference!", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label("Scene Reference is a Unity package that replaces scene references by name or index with GUIDs, eliminating broken references when scenes are renamed or reorganized. The registry updates automatically whenever your Build Settings change, and is fully configured from the Project Settings.", EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);
            
            if (GUILayout.Button("Create a SceneRegistryConfig"))
            {
                string path = EditorUtility.SaveFilePanelInProject("Create SceneRegistryConfig", "SceneRegistryConfig", "asset", "Select a location to save the SceneRegistryConfig asset.");
                
                if (!string.IsNullOrEmpty(path))
                {
                    SceneRegistryConfig config = CreateInstance<SceneRegistryConfig>();
                    AssetDatabase.CreateAsset(config, path);
                    AssetDatabase.SaveAssets();
                    EditorUtility.FocusProjectWindow();
                    Selection.activeObject = config;
                }
            }
            
            if (GUILayout.Button("Open Project Settings"))
            {
                SettingsService.OpenProjectSettings("Project/Black Hole Odyssey/Scene Reference");
                Close();
            }

            if (GUILayout.Button("Open sample scene"))
            {
                string[] guids = AssetDatabase.FindAssets("SceneReferenceExample t:Scene");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
                
                Close();
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Send Report"))
            {
                BugReporterWindow.ShowWindow();
                Close();
            }
        }
    }
}
