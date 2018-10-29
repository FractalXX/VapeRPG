using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace VapeRPG.UI.States
{
    internal class SkillBarUIState : DraggableUI
    {
        public static bool visible = false;

        public override Vector2 DefaultPosition => new Vector2(Main.screenWidth - this.DefaultSize.X, Main.screenHeight - this.DefaultSize.Y);
        public override Vector2 DefaultSize => new Vector2(200, 60);

        protected override UIElement CreateContainer()
        {
            return new UIElement();
        }

        protected override void Construct()
        {
            
        }
    }
}
