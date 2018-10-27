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
            item.width = 32;
            item.height = 32;
            item.rare = 10;
            item.maxStack = 1;
            item.useAnimation = 15;
            item.useTime = 15;
            item.useStyle = 2;
            item.consumable = true;
            item.UseSound = SoundID.Item29;
            item.value = 1000000;
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
            int chaosPoints = 0;

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

            foreach (string bonus in VapeRPG.MinorStats)
            {
                if (bonus != "Block Chance")
                {
                    if (bonus.Contains("Crit"))
                    {
                        chaosPoints += (int)vp.ChaosBonuses[bonus];
                    }
                    else if (bonus.Contains("Dodge"))
                    {
                        chaosPoints += (int)(vp.ChaosBonuses[bonus] / 0.005f);
                    }
                    else
                    {
                        chaosPoints += (int)(vp.ChaosBonuses[bonus] / 0.02f);
                    }
                    vp.ChaosBonuses[bonus] = 0;
                }
            }

            vp.statPoints += statPoints;
            vp.skillPoints += skillPoints;
            vp.chaosPoints += chaosPoints;

            return true;
        }
    }
}
