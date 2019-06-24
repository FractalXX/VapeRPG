using System;
using Terraria;
using Terraria.ModLoader;

using Terraria.ID;

namespace VapeRPG.Items
{
    class VapersGlobe : ModItem
    {
        public override void SetDefaults()
        {
            this.item.width = 32;
            this.item.height = 32;
            this.item.rare = 10;
            this.item.maxStack = 1;
            this.item.useAnimation = 15;
            this.item.useTime = 15;
            this.item.useStyle = 2;
            this.item.consumable = true;
            this.item.UseSound = SoundID.Item29;
            this.item.value = 1000000;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vaper's Globe");
            Tooltip.SetDefault("Can be used to reset your stats and skills.");
        }

        public override bool UseItem(Player player)
        {
            VapePlayer vp = player.GetModPlayer<VapePlayer>();

            int statPoints = 0;
            int skillPoints = 0;

            foreach (string stat in VapeRPG.BaseStats)
            {
                statPoints += (vp.BaseStats[stat] - 1);
                vp.BaseStats[stat] = 1;
            }

            foreach (Skill skill in VapeRPG.Skills)
            {
                skillPoints += vp.GetSkillLevel(skill.GetType());
                vp.SetSkillLevel(skill.GetType(), 0);
            }

            vp.statPoints += statPoints;
            vp.skillPoints += skillPoints;

            return true;
        }
    }
}
