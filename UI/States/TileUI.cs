using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using VapeRPG.UI.Elements;
using VapeRPG.Util;

namespace VapeRPG.UI.States
{
    internal abstract class TileUI : DraggableUI
    {
        public override Vector2 DefaultPosition => new Vector2(Main.screenWidth / 2 - this.DefaultSize.X / 2, Main.screenHeight / 2 - this.DefaultSize.Y);
        public bool visible;

        protected override sealed UIElement CreateContainer()
        {
            return new UIPanel();
        }

        protected override void Construct()
        {
            UIImageButton exitButton = new UIImageButton(Textures.UI.EXIT_BUTTON);
            exitButton.Width.Set(15f, 0f);
            exitButton.Height.Set(15f, 0f);
            exitButton.Left.Set(-exitButton.Width.Pixels, 1f);

            exitButton.OnClick += (evt, element) =>
            {
                this.visible = false;
            };

            this.container.Append(exitButton);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if(this.visible)
            {
                base.DrawSelf(spriteBatch);
            }
        }
    }
}
