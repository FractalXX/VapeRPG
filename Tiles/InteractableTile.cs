using Terraria.ModLoader;
using VapeRPG.UI.States;

namespace VapeRPG.Tiles
{
    internal abstract class InteractableTile : ModTile
    {
        protected TileUI ui;

        public override void RightClick(int i, int j)
        {
            VapeRPG vapeMod = this.mod as VapeRPG;
            vapeMod.SetTileUIState(this.ui);

            this.ui.visible = true;
            this.ui.Recalculate();
        }
    }
}
