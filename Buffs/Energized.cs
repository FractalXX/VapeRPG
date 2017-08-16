using Terraria;
using Terraria.ModLoader;

using VapeRPG.Projectiles;

namespace VapeRPG.Buffs
{
    class Energized : ModBuff
    {
        internal static byte maxStacks = 2;
        internal byte stacks = 1;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energized");
            Description.SetDefault("You feel overwhelmed with energy");
            Main.buffNoSave[Type] = true;
            Main.persistentBuff[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex)
        {
            this.stacks++;
            if (this.stacks >= maxStacks)
            {
                player.AddBuff(mod.BuffType<FullyEnergized>(), 600);
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Energized"), player.position);
                this.stacks = 1;
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
            player.buffTime[buffIndex] = this.stacks * 60;
        }
    }
}
