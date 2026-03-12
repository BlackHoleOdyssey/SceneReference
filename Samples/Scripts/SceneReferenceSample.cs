using OnirysGames.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReferenceSample : MonoBehaviour
{
    [SerializeField] private SceneReference sceneReference;

    private void Awake()
    {
        SceneManager.LoadScene(sceneReference);
    }
}
