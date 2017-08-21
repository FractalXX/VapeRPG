using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;

namespace VapeRPG
{
    static class SaveVersionHandler
    {
        internal static void Load(VapePlayer modPlayer, TagCompound tag)
        {
            TagCompound stats = tag.GetCompound("BaseStats");
            TagCompound skillLevels = tag.GetCompound("SkillLevels");
            TagCompound chaosBonuses = tag.GetCompound("ChaosBonuses");

            string key;

            modPlayer.statPoints = tag.GetAsInt("StatPoints");
            modPlayer.skillPoints = tag.GetAsInt("SkillPoints");
            modPlayer.chaosPoints = tag.GetAsInt("ChaosPoints");

            // Unboxing values into the proper dictionary
            foreach (var stat in stats)
            {
                key = stat.Key;
                // Intellect was removed in v0.3.1
                if (key == "Intellect")
                {
                    modPlayer.statPoints += (int)stat.Value;
                    continue;
                }
                modPlayer.BaseStats[key] = (int)stat.Value;
            }

            foreach (var skillLevel in skillLevels)
            {
                modPlayer.SkillLevels[skillLevel.Key] = (int)skillLevel.Value;
            }

            foreach (var chaosBonus in chaosBonuses)
            {
                key = chaosBonus.Key;
                // Movement Speed as a chaos bonus was replaced with Max Run Speed in v0.3.1
                if(key == "Movement Speed")
                {
                    key = "Max Run Speed";
                }
                modPlayer.ChaosBonuses[key] = (float)chaosBonus.Value;
            }

            modPlayer.chaosRank = tag.GetAsInt("ChaosRank");
            modPlayer.chaosXp = tag.GetAsLong("ChaosXp");

            modPlayer.xp = tag.GetAsLong("Xp");
            Vector2 expUIPos = tag.Get<Vector2>("expUIPos");

            VapeRPG vapeMod = (modPlayer.mod as VapeRPG);
            vapeMod.ExpUI.SetPanelPosition(expUIPos);
        }
    }
}
