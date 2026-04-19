using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace BHO.SceneReference
{
    public class StreamingAssetsStorage : ISceneRegistryStorage
    {
        private readonly string FilePath = Path.Combine(Application.streamingAssetsPath, "SceneRegistry.json");

        public void Load(out Dictionary<string, int> scenes)
        {
            scenes = new();
            if (!File.Exists(FilePath)) return;

            string cipher = File.ReadAllText(FilePath);
            string json = Decrypt(cipher);
            SceneRegistryData data = JsonUtility.FromJson<SceneRegistryData>(json);
            scenes = data.ToDictionary();
        }

        private string Decrypt(string cipherBytes)
        {
            byte[] fullBytes = Convert.FromBase64String(cipherBytes);

            using Aes aes = Aes.Create();
            aes.Key = DeriveKey();

            byte[] iv = new byte[16];
            Array.Copy(fullBytes, 0, iv, 0, 16);
            aes.IV = iv;

            using MemoryStream memoryStream = new(fullBytes, 16, fullBytes.Length - 16);
            using CryptoStream cryptoStream = new(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEnd();
        }

        private byte[] DeriveKey()
        {
            SceneRegistryConfig config = SceneRegistryConfig.Instance;
            string keyToUse = "";

            if (config != null)
            {
                if (config.KeyProvider == KeyProviderType.Custom)
                {
                    keyToUse = SceneRegistryConfig.Instance.CustomKey;
                }
            }

            using SHA256 sha256 = SHA256.Create();
            return sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(keyToUse));
        }

#if UNITY_EDITOR
        public void Save(Dictionary<string, int> scenes)
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);

            string json = JsonUtility.ToJson(SceneRegistryData.FromDictionary(scenes), true);
            string cipher = Encrypt(json);
            File.WriteAllText(FilePath, cipher);

            UnityEditor.AssetDatabase.Refresh();
        }

        private string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKey();
            aes.GenerateIV();

            using MemoryStream memoryStream = new();

            memoryStream.Write(aes.IV, 0, 16);


            using (CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }
#endif
    }
}
