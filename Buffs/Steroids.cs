using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace VapeRPG.Buffs
{
    class Steroids : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Steroids");
            Description.SetDefault("Movement speed is temporarily increased");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += player.GetModPlayer<VapePlayer>().SkillLevels["Steroids"] / 5;
        }
    }
}
