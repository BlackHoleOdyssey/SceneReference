using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHO.SceneReference.Sample
{
    public class SceneReferenceSample : MonoBehaviour
    {
        [SerializeField] private SceneReference sceneReference;

        private void Awake()
        {
            SceneManager.LoadScene(sceneReference);
        }
    }
}
