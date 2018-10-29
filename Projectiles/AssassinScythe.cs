using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Projectiles
{
    public class AssassinScythe : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.alpha = 100;
            projectile.light = 0.5f;
            projectile.aiStyle = 18;
            this.aiType = 18;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.magic = true;
            projectile.timeLeft = 20;
            projectile.hide = false;
        }

        public override void SetStaticDefaults()
        {
            this.DisplayName.SetDefault("Assassin Scythe");
        }

        public override bool PreAI()
        {
            projectile.position.X = Main.player[projectile.owner].position.X;
            projectile.position.Y = Main.player[projectile.owner].position.Y;
            return true;
        }

        public override void AI()
        {
            projectile.position.X = Main.player[projectile.owner].position.X;
            projectile.position.Y = Main.player[projectile.owner].position.Y;

            Main.player[projectile.owner].velocity.X = projectile.velocity.X * 2;
            Main.player[projectile.owner].velocity.Y = projectile.velocity.Y * 2;

            for (int i = 0; i < 5; i++)
            {
                int proj = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, 274, projectile.damage, 0f, projectile.owner);
                Main.projectile[proj].timeLeft = 120;
                Main.projectile[proj].scale = i / 2;
                Main.projectile[proj].penetrate = -1;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].velocity.X = 0;
            Main.player[projectile.owner].velocity.Y = 0;
        }

    }
}