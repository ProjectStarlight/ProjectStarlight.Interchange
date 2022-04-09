using Microsoft.Xna.Framework.Graphics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = Microsoft.Xna.Framework.Color;

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

        /// <summary>
        ///     Creates a <see cref="TextureGIF"/> from a <see cref="FileStream"/>, opened and read from the <paramref name="filePath"/>.
        /// </summary>
        public static async Task<TextureGIF> FromGIFFile(string filePath, GraphicsDevice graphicsDevice, int ticksPerFrame)
        {
            await using FileStream stream = File.OpenRead(filePath);
            TextureGIF gif = await FromGIFFile(stream, graphicsDevice, ticksPerFrame);

            return gif;
        }

        /// <summary>
        ///     Creates a <see cref="TextureGIF"/> from a <see cref="Stream"/>.
        /// </summary>
        public static async Task<TextureGIF> FromGIFFile(Stream stream, GraphicsDevice graphicsDevice, int ticksPerFrame)
        {
            using Image? gif = await Image.LoadAsync(stream);
            
            if (gif is null)
                throw new NullReferenceException("Image was null.");

            List<Texture2D> xnaFrames = new();

            foreach (ImageFrame<Rgba32>? frame in gif.Frames.Cast<ImageFrame<Rgba32>>())
            {
                List<Color> colors = new();

                Parallel.For(0, frame.Height, (y, state) =>
                    Parallel.For(0, frame.Width, (x, stateCol) =>
                    {
                        Rgba32 frameColor = frame[x, y];
                        colors.Add(new Color(frameColor.R, frameColor.G, frameColor.B, frameColor.A));
                    }));
                
                Texture2D trueFrame = new(graphicsDevice, frame.Width, frame.Height);
                trueFrame.SetData(colors.ToArray());
                xnaFrames.Add(trueFrame);
            }

            return new TextureGIF(xnaFrames.ToArray(), ticksPerFrame);
        }
    }
}