using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace VapeRPG.Buffs
{
    class StaticField : ModBuff
    {
        internal static int range = 200;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Static Field");
            Description.SetDefault("You're protected by a static field which damages enemies.");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            int dustCount = 360;
            for (int i = 0; i < dustCount; i += 2)
            {
                double angle = i * Math.PI / 180;
                Vector2 sparkPosition = new Vector2(player.position.X + player.width / 2 + range * (float)Math.Cos(angle), player.position.Y + player.height / 2 + range * (float)Math.Sin(angle));

                Dust dust = Dust.NewDustPerfect(sparkPosition, 15, Vector2.Zero);
                dust.noGravity = true;
            }
        }
    }
}
