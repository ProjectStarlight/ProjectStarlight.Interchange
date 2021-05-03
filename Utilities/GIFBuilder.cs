using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

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

        public static TextureGIF FromGIFFile(string filePath, GraphicsDevice graphicsDevice, int ticksPerFrame)
        {
            TextureGIF gif;

            using (FileStream stream = File.OpenRead(filePath))
            {
                gif = FromGIFFile(stream, graphicsDevice, ticksPerFrame);
            }

            return gif;
        }

        // https://dejanstojanovic.net/aspnet/2018/march/getting-gif-image-information-using-c/
        public static TextureGIF FromGIFFile(FileStream stream, GraphicsDevice graphicsDevice, int ticksPerFrame)
        {
            using (Image image = Image.FromStream(stream))
            {
                if (!image.RawFormat.Equals(ImageFormat.Gif))
                    throw new Exception("Given file was not a GIF!");

                if (!ImageAnimator.CanAnimate(image))
                    throw new Exception("Can't animate the GIF file!");

                List<Image> frames = new List<Image>();
                List<Texture2D> trueFrames = new List<Texture2D>();

                FrameDimension dimension = new FrameDimension(image.FrameDimensionsList[0]);
                int frameCount = image.GetFrameCount(dimension);

                // Populate list of frames to read
                for (int i = 0; i < frameCount; i++)
                {
                    image.SelectActiveFrame(dimension, i);
                    frames.Add(image.Clone() as Image);
                }

                foreach (Image frame in frames)
                {
                    // finally use bitmaps yay
                    Bitmap gifFrame = new Bitmap(frame);
                    int width = gifFrame.Width;
                    int height = gifFrame.Height;
                    Color[,] framePixels = new Color[width, height];
                    int unflattenedSize = framePixels.Length;
                    Color[] flattenedPixels = new Color[unflattenedSize];

                    for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        framePixels[x, y] = gifFrame.GetPixel(x, y);

                    int flatIndex = 0;
                    for (int x = 0; x < framePixels.GetUpperBound(0); x++)
                    for (int y = 0; y < framePixels.GetUpperBound(1); y++)
                        flattenedPixels[flatIndex++] = framePixels[x, y];

                    XnaColor[] usableColors = new XnaColor[flattenedPixels.Length];
                    for (int i = 0; i < usableColors.Length; i++)
                    {
                        Color temp = flattenedPixels[i];
                        usableColors[i] = new XnaColor(temp.R, temp.G, temp.B, temp.A);
                    }

                    Texture2D trueFrame = new Texture2D(graphicsDevice, width, height);
                    trueFrame.SetData(usableColors);
                    trueFrames.Add(trueFrame);
                    frame.Dispose();
                }

                return new TextureGIF(trueFrames.ToArray(), ticksPerFrame);
            }
        }
    }
}
