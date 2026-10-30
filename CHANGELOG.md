# Changelog

## [1.1.0] - 2026-10-30

### Added

- Auto-generated `Scenes` enum, built from the scenes enabled in Build Settings. Each value matches the scene's build index, so `(int)Scenes.Level1` gives the build index directly, with no registry lookup
- `SceneRegistry.GetScene(Scenes)` returns a `SceneLink` from the enum name, so a scene can be referenced from any script without an inspector field or a hard-coded name or index
- `SceneRegistry.GetBuildIndex(Scenes)` overload returns the build index as an `int` without any allocation
- Enum regeneration is triggered automatically when a `.unity` file is imported, deleted or moved, and when the scene list in Build Settings changes (added, removed, reordered, enabled or disabled)
- `Tools → BHO → Regenerate Scenes Enum` menu item to force a regeneration manually
- Scene names are sanitized into valid C# identifiers (invalid characters, leading digits and reserved keywords are handled), and duplicate names are suffixed with a warning in the console
- The generated file is only rewritten when its content actually changes, which avoids unnecessary recompilations

### Changed

- **Breaking:** Renamed `SceneReference` to `SceneLink`, to avoid the ambiguity with the `BHO.SceneReference` namespace (`SceneReference.SceneReference`). Fields of this type serialized in existing scripts, prefabs and scenes must be reassigned after updating
- `SceneRegistry` now keeps a cached reverse lookup (build index → GUID), which is invalidated whenever the registry changes
- `SceneRegistry.Scenes` is now exposed as the read-only `SceneRegistry.Registry`, to avoid the name clash with the `Scenes` enum. Modifications go through `Add`, `Remove` and `Clear`
- `SceneRegistry.Reload()` now saves the registry after rebuilding it
- The generated enum is now written next to the runtime scripts, outside the `Editor` folder, so it can be used from runtime code

### Fixed

- Fixed a potential `NullReferenceException` in `SceneRegistry` when the registry was accessed before it had been initialized
- Fixed the generated enum containing empty entries when some scenes were disabled in Build Settings
- Wrapped the `UnityEditor` using directive in `SceneLink` with `#if UNITY_EDITOR` to avoid build errors

### Improved

- Added icons to the Editor scripts that did not have one, so they are easier to identify in the Project window

## [1.0.2] - 2026-10-28

### Fixed

- Fixed `SceneRegistry.GetName` and `SceneRegistry.GetPath` returning empty strings for scenes that are not currently loaded. Both methods now resolve the path through `SceneUtility.GetScenePathByBuildIndex`, which works at runtime for any scene in the Build Settings
- Wrapped the `UnityEditor` using directive in `SceneRegistry` with `#if UNITY_EDITOR` to avoid build errors

## [1.0.1] - 2026-04-25

### Fixed

- Removed dependency on undefined Color constants (softRed, limeGreen) that caused compilation errors when installing the package in an external project

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
