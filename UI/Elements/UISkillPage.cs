using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UISkillPage : UITexturedPanel
    {
        public bool visible;

        public UISkillPage(Texture2D backgroundTexture, int strokeThickness) : base(backgroundTexture, strokeThickness)
        {
            this.visible = false;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if(this.visible)
            {
                base.DrawSelf(spriteBatch);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(this.visible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
