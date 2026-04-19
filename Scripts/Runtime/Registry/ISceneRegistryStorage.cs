// Copyright (c) 2026 Black Hole Odyssey
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

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
