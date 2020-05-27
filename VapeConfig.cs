using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;

namespace VapeRPG
{
    public static class VapeConfig
    {
        // Common
        public static bool UIEnabled = true;
        public static float FinalMultiplierForXpGain = 0.5f;
        public static int ExperienceGainDistance = 3000;
        public static float GlobalXpMultiplier = 1f;

        // Stats
        public static int StatPointsPerLevel = 5;

        public static float MeleeDamageDivider = 500f;
        public static float MagicDamageDivider = 430f;
        public static float RangedDamageDivider = 465f;
        public static float MinionDamageDivider = 400f;
        public static float MagicDamageBySpiritDivider = 860f;

        public static float MeleeCritDivider = 10f;
        public static float MagicCritDivider = 7f;
        public static float RangedCritDivider = 8.5f;

        public static float MeleeSpeedDivider = 900f;
        public static float MoveSpeedDivider = 1800f;
        public static float DodgeDivider = 1800f;

        public static float SpiritPerMaxMinion = 50;
        public static float SpiritPerMaxTurret = 100;

        public static float MaxDodgeChance = 0.7f;

        public static float LifePerLevel = 5;
        public static float ManaPerLevel = 4;

        public static float LifePerVitality = 2;
        public static float ManaPerMagicPower = 0;

        public static float VitalityPerDefense = 10;
        public static float StrengthPerLife = 2;

        // Default stats
        public static float DefMeleeDamage = 0.7f;
        public static float DefMagicDamage = 0.9f;
        public static float DefRangedDamage = 0.8f;
        public static float DefMinionDamage = 0.8f;
        public static float DefThrownDamage = 0.8f;

        public static int DefMeleeCrit = 1;
        public static int DefMagicCrit = 1;
        public static int DefRangedCrit = 1;
        public static int DefThrownCrit = 1;

        public static float DefDodge = 0;
        public static float DefMeleeSpeed = 0.8f;

        public static int DefLife = 100;
        public static int DefMana = 20;

        // Xp gain overrides
        // Vanilla NPCs
        public static Dictionary<int, double> VanillaXpTable = new Dictionary<int, double>()
        {
            {NPCID.BlueSlime, 10 }
        };

        // Ignore lists
        public static List<int> IgnoredTypesForXpGain = new List<int>()
        {
            NPCID.DungeonGuardian,
            NPCID.Bunny,
            NPCID.BunnySlimed,
            NPCID.BunnyXmas,
            NPCID.GoldBunny,
            NPCID.PartyBunny,
            NPCID.Penguin,
            NPCID.PenguinBlack,
            NPCID.Bird,
            NPCID.GoldBird,
            NPCID.ScorpionBlack,
            NPCID.Buggy,
            NPCID.Duck,
            NPCID.Duck2,
            NPCID.DuckWhite,
            NPCID.DuckWhite2,
            NPCID.Frog,
            NPCID.GoldFrog,
            NPCID.Worm,
            NPCID.GoldWorm,
            NPCID.TruffleWorm,
            NPCID.Goldfish,
            NPCID.GoldfishWalker,
            NPCID.Grasshopper,
            NPCID.GoldGrasshopper,
            NPCID.LightningBug,
            NPCID.Mouse,
            NPCID.GoldMouse,
            NPCID.Squirrel,
            NPCID.SquirrelGold,
            NPCID.SquirrelRed,
            NPCID.Scorpion,
            NPCID.Sluggy,
            NPCID.Snail,
            NPCID.GlowingSnail,
            NPCID.SeaSnail,
            NPCID.Butterfly,
            NPCID.GoldButterfly,
            NPCID.Firefly
        };

        private static readonly string CommonConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Common.json");
        private static readonly string StatConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Stats.json");
        private static readonly string DefaultStatConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_DefaultStats.json");
        private static readonly string IgnoresConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Ignores.json");
        private static readonly string XpConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_XpOverrides.json");

        private static readonly Preferences CommonConfig = new Preferences(CommonConfigPath);
        private static readonly Preferences StatConfig = new Preferences(StatConfigPath);
        private static readonly Preferences DefaultStatsConfig = new Preferences(DefaultStatConfigPath);
        private static readonly Preferences IgnoresConfig = new Preferences(IgnoresConfigPath);
        private static readonly Preferences XpConfig = new Preferences(XpConfigPath);

        public static bool IsIgnoredType(NPC npc)
        {
            return IgnoredTypesForXpGain.Contains(npc.type) ||
                npc.TypeName.ToLower().Contains("pillar");
        }
        public static void Load()
        {
            if (!ReadConfig(CommonConfig))
            {
                CreateConfig(CommonConfig);
            }
            if (!ReadConfig(StatConfig))
            {
                CreateConfig(StatConfig);
            }
            if (!ReadConfig(DefaultStatsConfig))
            {
                CreateConfig(DefaultStatsConfig);
            }
            if (!ReadConfig(IgnoresConfig))
            {
                CreateConfig(IgnoresConfig);
            }
            if (!ReadConfig(XpConfig))
            {
                CreateConfig(XpConfig);
            }
        }

        private static bool ReadConfig(Preferences conf)
        {
            if (conf.Load())
            {
                /*FieldInfo[] fields = typeof(VapeConfig).GetFields(BindingFlags.Static);
                for(int i = 0; i < fields.Length; i++)
                {
                    if(conf.Contains(fields[i].Name))
                    {
                        Type type = fields[i].GetType();
                        object def = type.IsValueType ? Activator.CreateInstance(type) : null;
                        fields[i].SetValue(null, conf.Get(fields[i].Name, def));
                    }
                }*/
                if (conf == CommonConfig)
                {
                    conf.Get("EnableUI", ref UIEnabled);
                    conf.Get("FinalMultiplierForXpGain", ref FinalMultiplierForXpGain);
                    conf.Get("ExperienceGainDistance", ref ExperienceGainDistance);
                    conf.Get("GlobalXpMultiplier", ref GlobalXpMultiplier);
                }
                else if (conf == StatConfig)
                {
                    conf.Get("StatPointsPerLevel", ref StatPointsPerLevel);
                    conf.Get("LifePerLevel", ref LifePerLevel);
                    conf.Get("ManaPerLevel", ref ManaPerLevel);

                    conf.Get("LifePerVitality", ref LifePerVitality);
                    conf.Get("ManaPerMagicPower", ref ManaPerMagicPower);
                    conf.Get("VitalityPerDefense", ref VitalityPerDefense);
                    conf.Get("StrengthPerLife", ref StrengthPerLife);

                    conf.Get("MeleeDamageDivider", ref MeleeDamageDivider);
                    conf.Get("MagicDamageDivider", ref MagicDamageDivider);
                    conf.Get("RangedDamageDivider", ref RangedDamageDivider);
                    conf.Get("MinionDamageDivider", ref MinionDamageDivider);
                    conf.Get("MagicDamageBySpiritDivider", ref MagicDamageBySpiritDivider);

                    conf.Get("MeleeCritDivider", ref MeleeDamageDivider);
                    conf.Get("MagicCritDivider", ref MagicDamageDivider);
                    conf.Get("RangedCritDivider", ref RangedDamageDivider);

                    conf.Get("MeleeSpeedDivider", ref MeleeDamageDivider);
                    conf.Get("MoveSpeedDivider", ref MagicDamageDivider);
                    conf.Get("DodgeDivider", ref RangedDamageDivider);

                    conf.Get("SpiritPerMaxMinion", ref SpiritPerMaxMinion);
                    conf.Get("SpiritPerMaxTurret", ref SpiritPerMaxTurret);

                    conf.Get("MaxDodgeChance", ref MaxDodgeChance);
                }
                else if (conf == DefaultStatsConfig)
                {
                    conf.Get("DefMeleeDamage", ref DefMeleeDamage);
                    conf.Get("DefMagicDamage", ref DefMagicDamage);
                    conf.Get("DefRangedDamage", ref DefRangedDamage);
                    conf.Get("DefThrownDamage", ref DefThrownDamage);
                    conf.Get("DefMinionDamage", ref DefMinionDamage);

                    conf.Get("DefMeleeCrit", ref DefMeleeCrit);
                    conf.Get("DefMagicCrit", ref DefMagicCrit);
                    conf.Get("DefRangedCrit", ref DefRangedCrit);
                    conf.Get("DefThrownCrit", ref DefThrownCrit);

                    conf.Get("DefDodge", ref DefDodge);
                    conf.Get("DefMeleeSpeed", ref DefMeleeSpeed);
                }
                else if (conf == IgnoresConfig)
                {
                    conf.Get("IgnoredTypesForXpGain", ref IgnoredTypesForXpGain);
                }
                else if (conf == XpConfig)
                {
                    conf.Get("VanillaXpTable", ref VanillaXpTable);
                }
                return true;
            }
            return false;
        }

        private static void CreateConfig(Preferences conf)
        {
            conf.Clear();

            if (conf == CommonConfig)
            {
                conf.Put("EnableUI", UIEnabled);
                conf.Put("FinalMultiplierForXpGain", FinalMultiplierForXpGain);
                conf.Put("ExperienceGainDistance", ExperienceGainDistance);
                conf.Put("GlobalXpMultiplier", GlobalXpMultiplier);
            }
            else if (conf == StatConfig)
            {
                conf.Put("StatPointsPerLevel", StatPointsPerLevel);
                conf.Put("LifePerLevel", LifePerLevel);
                conf.Put("ManaPerLevel", ManaPerLevel);

                conf.Put("LifePerVitality", LifePerVitality);
                conf.Put("ManaPerMagicPower", ManaPerMagicPower);
                conf.Put("VitalityPerDefense", VitalityPerDefense);
                conf.Put("StrengthPerLife", StrengthPerLife);

                conf.Put("MeleeDamageDivider", MeleeDamageDivider);
                conf.Put("MagicDamageDivider", MagicDamageDivider);
                conf.Put("RangedDamageDivider", RangedDamageDivider);
                conf.Put("MinionDamageDivider", MinionDamageDivider);
                conf.Put("MagicDamageBySpiritDivider", MagicDamageBySpiritDivider);

                conf.Put("MeleeCritDivider", MeleeDamageDivider);
                conf.Put("MagicCritDivider", MagicDamageDivider);
                conf.Put("RangedCritDivider", RangedDamageDivider);

                conf.Put("MeleeSpeedDivider", MeleeDamageDivider);
                conf.Put("MoveSpeedDivider", MagicDamageDivider);
                conf.Put("DodgeDivider", RangedDamageDivider);

                conf.Put("SpiritPerMaxMinion", SpiritPerMaxMinion);
                conf.Put("SpiritPerMaxMinionTurret", SpiritPerMaxTurret);

                conf.Put("MaxDodgeChance", MaxDodgeChance);
            }
            else if (conf == DefaultStatsConfig)
            {
                conf.Put("DefMeleeDamage", DefMeleeDamage);
                conf.Put("DefMagicDamage", DefMagicDamage);
                conf.Put("DefRangedDamage", DefRangedDamage);
                conf.Put("DefThrownDamage", DefThrownDamage);
                conf.Put("DefMinionDamage", DefMinionDamage);

                conf.Put("DefMeleeCrit", DefMeleeCrit);
                conf.Put("DefMagicCrit", DefMagicCrit);
                conf.Put("DefRangedCrit", DefRangedCrit);
                conf.Put("DefThrownCrit", DefThrownCrit);

                conf.Put("DefDodge", DefDodge);
                conf.Put("DefMeleeSpeed", DefMeleeSpeed);
            }
            else if (conf == IgnoresConfig)
            {
                conf.Put("IgnoredTypesForXpGain", IgnoredTypesForXpGain);
            }
            else if (conf == XpConfig)
            {
                conf.Put("VanillaXpTable", VanillaXpTable);
            }

            conf.Save();
        }
    }
}