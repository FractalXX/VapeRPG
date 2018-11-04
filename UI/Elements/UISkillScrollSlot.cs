using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using VapeRPG.Items;
using VapeRPG.Util;

namespace VapeRPG.UI.Elements
{
    internal class UISkillScrollSlot : UIInteractableItemPanel
    {
        private ScrollBase Scroll
        {
            get
            {
                return this.item.modItem as ScrollBase;
            }
        }

        public UISkillScrollSlot() : base((item) => item.modItem is ScrollBase) { }

        public void UseSkill(Player player)
        {
            this.Scroll?.Use(player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.Scroll?.cooldownRemaining > 0)
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point position = new Point((int)dimensions.X, (int)dimensions.Y);
                int width = (int)Math.Ceiling(dimensions.Width);
                int height = (int)Math.Ceiling(dimensions.Height);
                spriteBatch.Draw(Textures.UI.SQUARE_SHADE, new Rectangle(position.X, position.Y + (int)(height * (1 - (float)this.Scroll.cooldownRemaining / this.Scroll.cooldown)), width, (int)(height * (float)this.Scroll.cooldownRemaining / this.Scroll.cooldown)), Color.White);
            }
        }
    }
}
