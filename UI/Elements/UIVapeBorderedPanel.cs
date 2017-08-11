using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UIVapeBorderedPanel : UIPanel
    {
        protected Texture2D textureBottom;
        protected Texture2D textureMiddle;
        protected Texture2D textureTop;

        private static int originalWidth = 1000;

        public bool Visible;

        internal UIVapeBorderedPanel(string baseTextureName)
        {
            string panelPath = "VapeRPG/Textures/UI/Panel/" + baseTextureName;

            this.textureBottom = ModLoader.GetTexture(panelPath + "Bottom");
            this.textureMiddle = ModLoader.GetTexture(panelPath + "Middle");
            this.textureTop = ModLoader.GetTexture(panelPath + "Top");

            this.Visible = true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if(this.Visible)
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                int width = (int)Math.Ceiling(dimensions.Width);
                int height = (int)Math.Ceiling(dimensions.Height);

                float widthFactor = this.Width.Pixels / originalWidth;
                int relativeHeight = (int)(this.textureBottom.Height * widthFactor);
                float relativeMiddleHeight = this.Height.Pixels - 2 * relativeHeight;

                spriteBatch.Draw(this.textureTop, new Rectangle(point1.X, point1.Y, width, (int)relativeHeight), Color.White);
                spriteBatch.Draw(this.textureMiddle, new Rectangle(point1.X, (int)(point1.Y + relativeHeight), width, (int)relativeMiddleHeight), Color.White);
                spriteBatch.Draw(this.textureBottom, new Rectangle(point1.X, (int)(point1.Y + relativeHeight + relativeMiddleHeight), width, (int)relativeHeight), Color.White);
            }
        }
    }
}
