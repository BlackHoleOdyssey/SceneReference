# Changelog

## [1.0.0] - 2026-04-25

### This is the first release of *SceneReference*.

- SceneReference field allowing scenes to be referenced by GUID instead of name or index, eliminating broken references when scenes are renamed or reorganized
- SceneRegistry that automatically syncs with Build Settings whenever scenes are added, removed, or reordered
- Auto-Generated storage mode that compiles the registry directly into the build as a C# file, with no external files or encryption key required
- StreamingAssets storage mode that saves the registry as an AES-encrypted JSON file, intended for projects that need to update the registry without recompiling such as DLC scenarios
- Custom Project Settings page under Edit → Project Settings → Black Hole Odyssey → Scene Registry
- SceneRegistryConfig ScriptableObject for configuring the storage mode and encryption key
- Automatic addition of SceneRegistryConfig to Preloaded Assets on creation
- First-time setup wizard displayed automatically on package import
- Custom property drawer for SceneReference with an "Add to scenes" button to quickly add a scene to Build Settings

## [1.0.1] - 2026-04-25

### Fixed

- Removed dependency on undefined Color constants (softRed, limeGreen) that caused compilation errors when installing the package in an external project
