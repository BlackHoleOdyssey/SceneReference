// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace BHO.SceneReference
{
    [Serializable]
    public class SceneRef
    {
        [SerializeField] private string guid = "";

#if UNITY_EDITOR
        [SerializeField] private SceneAsset sceneAsset;
#endif

        public SceneRef() { }
        public SceneRef(string guid) { this.guid = guid; }

        public string Guid => guid;
        public string SceneName => SceneRegistry.GetName(guid);
        public string ScenePath => SceneRegistry.GetPath(guid);
        public int BuildIndex => SceneRegistry.GetBuildIndex(guid);

        public static implicit operator int(SceneRef sceneRef) => sceneRef.BuildIndex;
    }
}