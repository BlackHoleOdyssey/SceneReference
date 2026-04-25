# Scene Reference

Scene Reference is a Unity package that replaces scene references by name or index with GUIDs, eliminating broken references when scenes are renamed or reorganized. The registry updates automatically whenever your Build Settings change, and is fully configured from the Project Settings.

## Requirements

- Unity 6000.0.66f2 or newer

## Installation

### Via Git URL
1. Open the Package Manager (Window → Package Manager)
2. Click the + button and select "Add package from git URL"
3. Enter: `https://github.com/BlackHoleOdyssey/SceneReference.git`

### Via Unity Package
1. Go to the [Releases](https://github.com/BlackHoleOdyssey/SceneRegistry/releases) page
2. Download the latest `.unitypackage` file
3. In Unity, go to **Assets → Import Package → Custom Package**
4. Select the downloaded file and click Import


## Getting Started

1. Create a SceneRegistryConfig via **Create > BHO > Scene Registry Config**
2. Configure the storage mode from **Edit → Project Settings → Black Hole Odyssey → Scene Reference**
3. Add your scenes to the Build Settings — the registry updates automatically

## Usage

Add a `SceneReference` field to your MonoBehaviour:

```csharp
[SerializeField] private SceneReference myScene;
```

Load the scene:

```csharp
SceneManager.LoadScene(myScene); //implicit cast
```

Access scene information:

```csharp
myScene.SceneName   // Name of the scene
myScene.ScenePath   // Full path of the scene asset
myScene.BuildIndex  // Build index resolved from the registry
myScene.Guid        // Unique identifier of the scene
```

## Storage Modes

**Auto-Generated** generates a C# file compiled directly into your build. This is the recommended mode: no external files, no encryption key, and the data is as protected as the rest of your code. It is possible to use this mode for DLC, but it requires a game update in parallel.

**StreamingAssets** saves the registry in an encrypted JSON file accessible in the build files. This mode is intended for projects that need to update the registry without recompiling. The encryption key is stored in a ScriptableObject that can be extracted from the build using third-party tools, making it less secure.

## Disclaimer

Scene Reference is a scene referencing tool, not a security system. Any Unity build is by nature decompilable, with or without Scene Registry. The developer declines all responsibility in the event of cheating, data tampering, or unauthorized access in your project.

## License

Copyright (c) 2026 Black Hole Odyssey  
Licensed under the MIT License. See [LICENSE](LICENSE) for more information.

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for the full changelog.