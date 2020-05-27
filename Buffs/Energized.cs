using Terraria;
using Terraria.ModLoader;

using VapeRPG.Projectiles;

namespace VapeRPG.Buffs
{
    class Energized : ModBuff
    {
        internal static byte maxStacks = 20;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energized");
            Description.SetDefault("You feel overwhelmed with energy");
            Main.buffNoSave[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            vp.energizedStacks++;
            if (vp.energizedStacks >= maxStacks)
            {
                player.AddBuff(ModContent.BuffType<FullyEnergized>(), 600);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Energized"), player.position);
                vp.energizedStacks = 0;
                player.ClearBuff(Type);
            }
            else
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Charge"), player.position);
            }
            return base.ReApply(player, time, buffIndex);
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = player.GetModPlayer<VapePlayer>().energizedStacks * 60;
        }
    }
}
