using System;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace VapeRPG.Buffs
{
    public class Hemorrhage : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hemorrhage");
            Description.SetDefault("Losing life over time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<VapeGlobalNpc>().hemorrhage = true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = "Losing life over time";
        }
    }
}
