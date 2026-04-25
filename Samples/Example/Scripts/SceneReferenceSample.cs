// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHO.SceneReference.Sample
{
    public class SceneReferenceSample : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneReferenceExample;
        [SerializeField] private SceneReference sceneReferenceExample2;

        public void LoadFirstScene()
        {
            SceneManager.LoadScene(sceneReferenceExample);
        }

        public void LoadSecondScene()
        {
            SceneManager.LoadScene(sceneReferenceExample2);
        }
    }
}
