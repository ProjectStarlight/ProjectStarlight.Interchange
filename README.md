# ProjectStarlight.Interchange
ProjectStarlight.Interchange (hereby referred to as "Interchange") is a simple library for loading and rendering GIFs inside of XNA (and, by extension, FNA and MonoGame, as they doesn't change anything important).

Interchange adds a `TextureGIF` class, which is, frankly, a little misleading, as it acts as a container for an array of `Texture2D`s and proper frame management, nothing exclusive to GIFs.

The `GIFBuilder` class provides methods for creating a `TextureGIF` with an array of `Texture2D`s, a string that points to the path of a GIF, or a FileStream of an opened GIF file.

You can load any file with frames and extract an array of `Texture2D`s to use to create an instance of a `TextureGIF`, even if it isn't a GIF!

## aight bro but what's the name mean
GIF :troll: ![image](https://user-images.githubusercontent.com/27323911/116934434-d608e880-ac19-11eb-9ebe-99853584ebe2.png)
