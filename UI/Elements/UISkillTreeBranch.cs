using System;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VapeRPG.UI.Elements
{
    class UISkillTreeBranch : UIElement
    {
        private Texture2D texture;

        public UISkillTreeBranch()
        {
            this.texture = ModLoader.GetTexture("VapeRPG/Textures/UI/Skills/Line");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(this.texture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
        }
    }
}
