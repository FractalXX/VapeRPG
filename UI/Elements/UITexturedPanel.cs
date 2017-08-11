using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UITexturedPanel : UIPanel
    {
        protected Texture2D backgroundTexture;
        private int strokeThickness;

        public UITexturedPanel(Texture2D backgroundTexture, int strokeThickness)
        {
            this.backgroundTexture = backgroundTexture;
            this.strokeThickness = strokeThickness;
        }

        public UITexturedPanel(int strokeThickness) : this(ModLoader.GetTexture("VapeRPG/Textures/UI/Blank"), strokeThickness) { }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            if(strokeThickness > 0)
            {
                spriteBatch.Draw(ModLoader.GetTexture("VapeRPG/Textures/UI/Blank"), new Rectangle(point1.X - this.strokeThickness, point1.Y - this.strokeThickness, width + this.strokeThickness * 2, height + this.strokeThickness * 2), this.BorderColor);
            }
            spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), this.BackgroundColor);
        }
    }
}
