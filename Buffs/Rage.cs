using System;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Buffs
{
    public class Rage : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rage");
            Description.SetDefault("Increased melee damage");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<VapePlayer>().rageBuff = true;
        }

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            VapePlayer vp = Main.LocalPlayer.GetModPlayer<VapePlayer>();
            if(vp.HasSkill("Fury"))
            {
                tip += String.Format(" and melee speed");
            }
            tip += String.Format(" ({0}%)", vp.SkillLevels["Rage"] * 3);
            base.ModifyBuffTip(ref tip, ref rare);
        }
    }
}
