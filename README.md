# ProjectStarlight.Interchange
ProjectStarlight.Interchange (hereby referred to as "Interchange") is a simple library for loading and rendering GIFs inside of XNA and FNA.

Interchange adds a `TextureGIF` class, which is, frankly, a little misleading, as it acts as a container for an array of `Texture2D`s and proper frame management, nothing exclusive to GIFs.

The `GIFBuilder` class provides methods for creating a `TextureGIF` with an array of `Texture2D`s, a string that points to the path of a GIF, or a `FileStream` of an opened GIF file (while the `Texture2D` array overload works with any `Texture2D`s, `FileStream` and path overloads REQUIRE the file to be a GIF).

You can load any file with frames and extract an array of `Texture2D`s to use to create an instance of a `TextureGIF`, even if it isn't a GIF!

## Using
### Normal Projects
If you want to use this in a normal project, simply download the DLL for whatever framework you're using and add it as an assembly reference.

### tModLoader
If you want to use this in a tModLoader mod, it's a little more complicated. You need to download the XNA and FNA files, add them both to your `lib` folder, keep the XNA DLL as the default name, and append `.FNA` (i.e. `ProjectStarlight.Interchange.FNA.dll`), add `ProjectStarlight.Interchange` to `dllReferences` in your `build.txt` file, and reference the XNA (`ProjectStarlight.Interchange.dll`) assembly in your mod.
