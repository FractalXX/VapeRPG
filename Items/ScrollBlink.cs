using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VapeRPG.Items
{
    class ScrollBlink : ScrollBase
    {
        public override void CastSpell(Player player, Vector2 mousePosition)
        {
            player.position = mousePosition;
        }

        public override void SetDefaults()
        {
            this.cooldown = 120;
            base.SetDefaults();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll (Blink)");
            Tooltip.SetDefault("Teleports the caster to the mouse's position.");
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
