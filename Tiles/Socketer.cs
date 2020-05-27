using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ObjectData;
using Terraria.ModLoader;

using VapeRPG.Items;
using VapeRPG.UI.States.TileStates;

namespace VapeRPG.Tiles
{
    internal class Socketer : InteractableTile
    {
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 36, 30, ModContent.ItemType<Items.Socketer>());
        }

        public override void SetDefaults()
        {
            Main.tileSolidTop[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = false;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            adjTiles = new int[] { TileID.WorkBenches };

            this.ui = new SocketerUI();
        }
    }
}