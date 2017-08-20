using Terraria;
using Terraria.ModLoader;

namespace VapeRPG.Buffs
{
    class FieldSickness : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Field Sickness");
            Description.SetDefault("Can't be resurrected by high-voltage field.");
            Main.buffNoSave[Type] = true;
        }
    }
}
