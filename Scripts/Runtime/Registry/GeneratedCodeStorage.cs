using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace BHO.SceneReference
{
    public class GeneratedCodeStorage : ISceneRegistryStorage
    {
#if UNITY_EDITOR
        private static string OUTPUT_PATH
        {
            get
            {
                string[] guids = AssetDatabase.FindAssets("SceneRegistryGenerated");
                if (guids.Length > 0)
                    return AssetDatabase.GUIDToAssetPath(guids[0]);
                
                string selfPath =AssetDatabase.FindAssets("GeneratedCodeStorage")[0];
                string directory = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(selfPath));
                return Path.Combine(directory, "SceneRegistryGenerated.cs");
            }
        }

        public void Save(Dictionary<string, int> scenes)
        {
            string directory = Path.GetDirectoryName(OUTPUT_PATH);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("// Auto-generated — do not edit manually");
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("namespace BHO.SceneReference {");
            stringBuilder.AppendLine("    public static class SceneRegistryGenerated {");
            stringBuilder.AppendLine("        public static readonly Dictionary<string, int> Scenes = new() {");

            foreach (var (key, value) in scenes)
            {
                stringBuilder.AppendLine($"            {{ \"{key}\", {value} }},");
            }

            stringBuilder.AppendLine("        };");
            stringBuilder.AppendLine("    }");
            stringBuilder.AppendLine("}");

            File.WriteAllText(OUTPUT_PATH, stringBuilder.ToString());
            UnityEditor.AssetDatabase.Refresh();
        }
#endif

        public void Load(out Dictionary<string, int> scenes)
        {
            scenes = SceneRegistryGenerated.Scenes;
        }
    }
}