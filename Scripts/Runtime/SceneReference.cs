using System;
using UnityEditor;
using UnityEngine;

namespace OnirysGames.SceneReference
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