using System.Collections.Generic;

namespace BHO.SceneReference
{
    public interface ISceneRegistryStorage
    {
        void Load(out Dictionary<string, int> scenes);
#if UNITY_EDITOR
        void Save(Dictionary<string, int> scenes);
#endif
    }
}
