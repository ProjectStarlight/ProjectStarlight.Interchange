using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProjectStarlight.Interchange.Utilities;

namespace ProjectStarlight.Interchange
{
    // TODO: GIF looping
    public class TextureGIF
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool IsPaused { get; private set; }

        public bool HasEnded => FrameIndex >= Frames.Length && FrameTick >= TicksPerFrame && !ShouldLoop;

        public bool ShouldLoop { get; set; }

        public int TicksPerFrame { get; set; }

        public int FrameTick { get; private set; }

        public int FrameIndex { get; private set; }

        public Texture2D[] Frames { get; set; }

        public Texture2D CurrentFrame => HasEnded ? Frames.Last() : Frames[FrameIndex];

        /// <summary>
        ///     Creates a <see cref="TextureGIF"/> instance from an array of <see cref="Texture2D"/>s, where the <see cref="Texture2D"/>s serve as frames.
        /// </summary>
        internal TextureGIF(Texture2D[] frames, int ticksPerFrame)
        {
            FrameUtilities.VerifyGIFDimensions(frames, out int width, out int height);
            Width = width;
            Height = height;
            TicksPerFrame = ticksPerFrame;
            FrameIndex = 0;
            Frames = frames;
        }

        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        ///     Restarts (or officially starts, if not started previously) the GIF. Render the GIF with <see cref="Draw"/>.
        /// </summary>
        public void Play()
        {
            FrameTick = 0;
            FrameIndex = 0;

            if (IsPaused)
                SwitchPauseState();
        }

        /// <summary>
        ///     Completely stops a GIF. If you want to pause a GIF, use <see cref="SwitchPauseState"/>.
        /// </summary>
        public void Stop()
        {
            // This essentially sets HasEnded to true.
            FrameTick = Frames.Length - 1;
            FrameTick = TicksPerFrame;
            ShouldLoop = false;
        }

        /// <summary>
        ///     Attempts to change the array of <see cref="Texture2D"/>s to draw.
        /// </summary>
        /// <param name="frames">The <see cref="Texture2D"/>s to set.</param>
        /// <returns>Returns <c>true</c> if successful, otherwise <c>false</c>.</returns>
        public bool TryChangeGIF(Texture2D[] frames)
        {
            if (FrameIndex >= frames.Length)
                return false;

            try
            {
                FrameUtilities.VerifyGIFDimensions(frames, out int width, out int height);
                Width = width;
                Height = height;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Pauses and unpauses the GIF depending on its current state.
        /// </summary>
        public void SwitchPauseState() =>
            // TODO: Find out if there's anything that needs to be done when pausing and unpausing.
            IsPaused = !IsPaused;

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color) =>
            spriteBatch.Draw(CurrentFrame, position, color);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color) =>
            spriteBatch.Draw(CurrentFrame, position, sourceRectangle, color);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color,
            float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(CurrentFrame, position, sourceRectangle, color, rotation, origin, scale, effects,
                layerDepth);

        public void Draw(SpriteBatch spriteBatch, Vector2 position, Rectangle? sourceRectangle, Color color,
            float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(CurrentFrame, position, sourceRectangle, color, rotation, origin, scale, effects,
                layerDepth);

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Color color) =>
            spriteBatch.Draw(CurrentFrame, destinationRectangle, color);

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle,
            Color color) => spriteBatch.Draw(CurrentFrame, destinationRectangle, sourceRectangle, color);

        public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle, Rectangle? sourceRectangle,
            Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) =>
            spriteBatch.Draw(CurrentFrame, destinationRectangle, sourceRectangle, color, rotation, origin, effects,
                layerDepth);

        public void UpdateGIF()
        {
            if (!IsPaused && !HasEnded) 
                ForwardTicks(1);
        }

        public void ForwardTicks(int tickAmount)
        {
            for (int i = 0; i < tickAmount; i++)
            {
                FrameTick++;

                if (FrameTick < TicksPerFrame)
                    return;

                FrameTick = 0;

                if (FrameIndex < Frames.Length - 1) 
                    FrameIndex++;
                else if (ShouldLoop)
                    FrameIndex = 0;
            }
        }
    }
}