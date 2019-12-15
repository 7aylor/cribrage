using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace cribrage
{
    class Button
    {
        public string Text { get; set; }
        public bool IsEnabled { get; set; } = false;
        public int DrawX { get; set; }
        public int DrawY { get; set; }
        public Color Color { get; set; } //The highlight color of the button
        public Texture2D Texture { get; set; }
        public bool Highlighted { get; set; } = false;

        public Button(string text, int x, int y, Color c, Texture2D texture)
        {
            Text = text;
            DrawX = x;
            DrawY = y;
            Color = c;
            Texture = texture;
        }

        public bool IsMouseHovering(Vector2 mPos)
        {
            if (mPos.X >= DrawX &&
               mPos.X <= DrawX + Texture.Width &&
               mPos.Y >= DrawY &&
               mPos.Y <= DrawY + Texture.Height)
            {
                Mouse.SetCursor(MouseCursor.Hand);
                return true;
            }
            Mouse.SetCursor(MouseCursor.Arrow);
            return false;
        }
    }
}
