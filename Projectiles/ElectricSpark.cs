using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Projectiles
{
    public class ElectricSpark : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Electric Spark");
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.penetrate = 2;
            projectile.damage = 10;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.knockBack = 10f;
            projectile.light = 1f;
            projectile.scale = 1f;
            projectile.extraUpdates = 1;
            projectile.hide = false;
        }

        public override void AI()
        {
            this.CreateDust();
            projectile.ai[0] += 0.1f;
        }

        private void CreateDust()
        {
            Dust.NewDustPerfect(new Vector2(projectile.position.X + projectile.width / 2, projectile.position.Y + projectile.height / 2), 15, Scale: 1.5f);
        }
    }
}