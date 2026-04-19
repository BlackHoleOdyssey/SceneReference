// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using UnityEditor;
using UnityEngine;

namespace BHO.SceneReference
{
    [Serializable]
    public class SceneReference
    {
        [SerializeField] private string guid = "";

        public string Guid => guid;
        
        public string SceneName => SceneRegistry.GetName(guid);
        public string ScenePath => SceneRegistry.GetPath(guid);
        public int BuildIndex => SceneRegistry.GetBuildIndex(guid);
        
        #if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;
        #endif
        
        public static implicit operator int(SceneReference sceneReference) => sceneReference.BuildIndex;
    }
}