using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VapeRPG.Items
{
    public class Socketer : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 30;
            item.maxStack = 99;

            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.rare = 5;
            item.value = 150;
            item.createTile = mod.TileType<Tiles.Socketer>();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Socketer");
            Tooltip.SetDefault("Used for inserting gems in weapons.");
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DirtBlock, 1);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
