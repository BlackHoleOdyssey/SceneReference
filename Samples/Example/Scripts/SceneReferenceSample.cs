// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHO.SceneReference.Sample
{
    public class SceneReferenceSample : MonoBehaviour
    {
        [SerializeField] private SceneRef sceneRefExample;
        [SerializeField] private SceneRef sceneRefExample2;

        public void LoadFirstScene()
        {
            SceneManager.LoadScene(sceneRefExample);
        }

        public void LoadSecondScene()
        {
            SceneManager.LoadScene(sceneRefExample2);
        }
    }
}