using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Buffs
{
    class Strengthened : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Strengthened");
            Description.SetDefault("You take reduced damage the next time you get hit.");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<VapePlayer>().strengthened = true;
        }
    }
}
