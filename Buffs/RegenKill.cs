using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace VapeRPG.Buffs
{
    public class RegenKill : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Regen kill");
            Description.SetDefault("Mana regen is increased");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VapePlayer>().regenKill = true;
        }
    }
}
