using System;
using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Buffs
{
    class HighFive : ModBuff
    {
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            VapePlayer vp = Main.LocalPlayer.GetModPlayer<VapePlayer>();
            tip += String.Format(" (Current: {0}%)", vp.highfiveStacks * 2);
            base.ModifyBuffTip(ref tip, ref rare);
        }

        public override void SetDefaults()
        {
            DisplayName.SetDefault("High Five");
            Description.SetDefault("Stacks critical chance and damage bonuses as long as you deal critical damage");
            Main.buffNoSave[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            vp.highfiveStacks++;
            if(vp.highfiveStacks >= 50)
            {
                vp.highfiveStacks = 0;
                player.ClearBuff(Type);
            }
            return base.ReApply(player, time, buffIndex);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            player.buffTime[buffIndex] = vp.highfiveStacks * 60;

            int critAmount = 2 * vp.highfiveStacks;
            float damageAmount = 2 * 0.01f * vp.highfiveStacks;

            player.meleeCrit += critAmount;
            player.meleeDamage += damageAmount;
            player.rangedCrit += critAmount;
            player.rangedDamage += damageAmount;
            player.magicCrit += critAmount;
            player.magicDamage += damageAmount;
            player.minionDamage += damageAmount;
            player.thrownDamage += damageAmount;
            player.thrownCrit += critAmount;
        }
    }
}
