using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using VapeRPG.Items;

namespace VapeRPG
{
    static class SaveVersionHandler
    {
        internal static void Load(VapePlayer modPlayer, TagCompound tag)
        {
            TagCompound stats = tag.GetCompound("BaseStats");
            TagCompound skillLevels = tag.GetCompound("SkillLevels");
            TagCompound skillScrolls = tag.GetCompound("SkillScrolls");

            string key;

            modPlayer.statPoints = tag.GetAsInt("StatPoints");
            modPlayer.skillPoints = tag.GetAsInt("SkillPoints");

            // Unboxing values into the proper dictionaries
            foreach (var stat in stats)
            {
                key = stat.Key;
                // Intellect was removed in v0.3.1
                if (key == "Intellect")
                {
                    modPlayer.statPoints += (int)stat.Value;
                    continue;
                }
                // Agility was renamed to Haste in v0.3.1
                else if (key == "Agility")
                {
                    key = "Haste";
                }
                modPlayer.BaseStats[key] = (int)stat.Value;
            }

            foreach (var skillLevel in skillLevels)
            {
                modPlayer.SetSkillLevel(Type.GetType("VapeRPG.Skills." + GetTypeNameFromOldSave(skillLevel.Key)), (int)skillLevel.Value);
            }

            VapeRPG vapeMod = (modPlayer.mod as VapeRPG);
            foreach (var pair in skillScrolls)
            {
                Item item = skillScrolls.Get<Item>(pair.Key);
                if (!(item.modItem is ScrollBase))
                {
                    item.TurnToAir();
                }
                vapeMod.SkillBarUI.SkillSlots[int.Parse(pair.Key)].item = item;
            }

            modPlayer.xp = tag.GetAsLong("Xp");
            Vector2 expUIPos = tag.Get<Vector2>("expUIPos");
            Vector2 skillUIPos = tag.Get<Vector2>("skillUIPos");

            vapeMod.ExpUI.SetPosition(expUIPos);
            vapeMod.ExpUI.Recalculate();
            vapeMod.SkillBarUI.SetPosition(skillUIPos);
            vapeMod.SkillBarUI.Recalculate();
        }

        private static string GetTypeNameFromOldSave(string key)
        {
            if (key.Equals("Damage to Defense", StringComparison.CurrentCultureIgnoreCase))
            {
                return "DamageToDefense";
            }
            return key.Replace(" ", "").Replace("-", "");
        }
    }
}
