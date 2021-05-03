using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectStarlight.Interchange.Utilities
{
    /// <summary>
    ///     Provides methods for creating <see cref="TextureGIF"/>s.
    /// </summary>
    public static class GIFBuilder
    {
        /// <summary>
        ///     Creates a <see cref="TextureGIF"/> from an array of <see cref="Texture2D"/>s.
        /// </summary>
        public static TextureGIF FromArray(Texture2D[] frames, int ticksPerFrame) =>
            new TextureGIF(frames, ticksPerFrame);

        public static TextureGIF FromGIFFile(byte[] data, int ticksPerFrameOverride = -1) =>
            throw new NotImplementedException();

        public static TextureGIF FromGIFFile(string filePath, int ticksPerFrameOverride = -1) =>
            throw new NotImplementedException();

        public static TextureGIF FromGIFFile(FileStream stream, int ticksPerFrameOverride = -1) =>
            throw new NotImplementedException();
    }
}
