using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace VapeRPG.Items
{
    class ScrollAnsatsu : ScrollBase
    {
        public override void CastSpell(Player player, Vector2 mousePosition)
        {
            Vector2 speed = new Vector2(Main.mouseX - Main.screenWidth / 2, Main.mouseY - Main.screenHeight / 2);
            speed *= 30 / speed.Length();

            Projectile projectile = Projectile.NewProjectileDirect(player.position, speed, mod.ProjectileType<Projectiles.AssassinScythe>(), (int)(damage * player.meleeDamage), 1f, Main.myPlayer);
            projectile.scale = 2.5f;

            player.velocity = speed;
        }

        public override void SetDefaults()
        {
            this.cooldown = 30;
            this.damage = 50;
            base.SetDefaults();
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
