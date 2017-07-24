using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Projectiles
{
    public class MagicSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Spark");
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.light = 1f;
            projectile.extraUpdates = 1;
            projectile.hide = false;
        }

        public override void AI()
        {        
            this.cooldownSlot = -1;
            Main.PlaySound(113, projectile.position);
            this.CreateDust();
            projectile.ai[0] += 0.1f;
        }

        private void CreateDust()
        {
            Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 130, 0, 0, 0).noGravity = true;
            Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y - 1), projectile.width/2, projectile.height/2, 131, 0, 0, 0).noGravity = true;
            Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y + 1), projectile.width/2, projectile.height/2, 132, 0, 0, 0).noGravity = true;
            Dust.NewDustDirect(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width/2, projectile.height/2, 133, 0, 0, 0).noGravity = true;
            Dust.NewDustDirect(new Vector2(projectile.position.X + 1, projectile.position.Y), projectile.width/2, projectile.height/2, 134, 0, 0, 0).noGravity = true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {

        }
    }
}