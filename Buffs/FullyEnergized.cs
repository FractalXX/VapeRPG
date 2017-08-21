using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Buffs
{
    class FullyEnergized : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Energized");
            Description.SetDefault("Increased movement speed and releasing electric sparks on being hit");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();
            vp.energized = true;
            player.moveSpeed *= 2f;
            player.maxRunSpeed += 2;
            Dust.NewDust(player.position, player.width, player.height, 15, Scale: 1.5f);
        }
    }
}
