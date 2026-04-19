using UnityEngine;

namespace BHO.SceneReference
{
    public enum KeyProviderType
    {
        Custom
    }
    
    [CreateAssetMenu(fileName = "SceneRegistryConfig", menuName = "BHO/SceneRegistryConfig")]
    public class SceneRegistryConfig : ScriptableObject
    {
        [SerializeField] private KeyProviderType keyProvider;
        [SerializeField] private string customKey = "";
        
        public KeyProviderType KeyProvider => keyProvider;
        public string CustomKey => customKey;

        public static SceneRegistryConfig Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadConfig()
        {
            if (Instance == null)
            {
                SceneRegistryConfig[] configs = Resources.FindObjectsOfTypeAll<SceneRegistryConfig>();
        
                if (configs.Length > 0)
                {
                    Instance = configs[0];
                }
            }
            
            Debug.Log($"[SceneRegistry] Loaded {Instance.name}.");
        }
    }
}
