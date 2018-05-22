using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace VapeRPG
{
    public static class VapeConfig
    {
        // Common
        public static bool UIEnabled = true;
        public static int ChaosChance = 5;
        public static int MinChaosMultiplier = 3;
        public static int MaxChaosMultiplier = 6;
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

        public static int SpiritPerMaxMinion = 50;
        public static int SpiritPerMaxTurret = 100;

        public static float MaxDodgeChance = 0.7f;

        public static int LifePerLevel = 5;
        public static int ManaPerLevel = 4;

        public static int LifePerVitality = 2;
        public static int ManaPerMagicPower = 0;

        public static int VitalityPerDefense = 10;
        public static int StrengthPerLife = 2;

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

        public static List<int> IgnoredTypesChaos = new List<int>()
        {
            NPCID.EaterofWorldsBody,
            NPCID.EaterofWorldsHead,
            NPCID.EaterofWorldsTail,
            NPCID.DevourerBody,
            NPCID.DevourerHead,
            NPCID.DevourerTail,
            NPCID.GiantWormBody,
            NPCID.GiantWormHead,
            NPCID.GiantWormTail,
            NPCID.DuneSplicerBody,
            NPCID.DuneSplicerHead,
            NPCID.DuneSplicerTail,
            NPCID.LeechBody,
            NPCID.LeechHead,
            NPCID.LeechTail,
            NPCID.StardustWormBody,
            NPCID.StardustWormHead,
            NPCID.StardustWormTail,
            NPCID.SolarCrawltipedeBody,
            NPCID.SolarCrawltipedeHead,
            NPCID.SolarCrawltipedeTail,
            NPCID.SeekerBody,
            NPCID.SeekerHead,
            NPCID.SeekerTail,
            NPCID.DiggerBody,
            NPCID.DiggerHead,
            NPCID.DiggerTail,
            NPCID.TheDestroyerBody,
            NPCID.TheDestroyerTail,
            NPCID.WyvernBody,
            NPCID.WyvernBody2,
            NPCID.WyvernBody3,
            NPCID.WyvernHead,
            NPCID.WyvernLegs,
            NPCID.WyvernTail,
            NPCID.TombCrawlerBody,
            NPCID.TombCrawlerHead,
            NPCID.TombCrawlerTail,
            NPCID.BoneSerpentBody,
            NPCID.BoneSerpentHead,
            NPCID.BoneSerpentTail,
            NPCID.PrimeCannon,
            NPCID.PrimeLaser,
            NPCID.PrimeSaw,
            NPCID.PrimeVice,
            NPCID.CultistDragonBody1,
            NPCID.CultistDragonBody2,
            NPCID.CultistDragonBody3,
            NPCID.CultistDragonBody4,
            NPCID.CultistDragonHead,
            NPCID.CultistDragonTail,
            NPCID.GolemHead,
            NPCID.GolemHeadFree,
            NPCID.MoonLordHead,
            NPCID.SkeletronHead
        };

        private static string CommonConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Common.json");
        private static string StatConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Stats.json");
        private static string DefaultStatConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_DefaultStats.json");
        private static string IgnoresConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_Ignores.json");
        private static string XpConfigPath = Path.Combine(Main.SavePath, "Mod Configs/VapeRPG", "VapeRPG_XpOverrides.json");

        private static Preferences CommonConfig = new Preferences(CommonConfigPath);
        private static Preferences StatConfig = new Preferences(StatConfigPath);
        private static Preferences DefaultStatsConfig = new Preferences(DefaultStatConfigPath);
        private static Preferences IgnoresConfig = new Preferences(IgnoresConfigPath);
        private static Preferences XpConfig = new Preferences(XpConfigPath);

        public static void Load()
        {
            if (!ReadConfig(CommonConfig))
            {
                ErrorLogger.Log("Failed to read config file: VapeRPG_Common.json! Recreating config...");
                CreateConfig(CommonConfig);
            }
            if (!ReadConfig(StatConfig))
            {
                ErrorLogger.Log("Failed to read config file: VapeRPG_Stats.json! Recreating config...");
                CreateConfig(StatConfig);
            }
            if (!ReadConfig(DefaultStatsConfig))
            {
                ErrorLogger.Log("Failed to read config file: VapeRPG_StartingStats.json! Recreating config...");
                CreateConfig(DefaultStatsConfig);
            }
            if (!ReadConfig(IgnoresConfig))
            {
                ErrorLogger.Log("Failed to read config file: VapeRPG_Ignores.json! Recreating config...");
                CreateConfig(IgnoresConfig);
            }
            if (!ReadConfig(XpConfig))
            {
                ErrorLogger.Log("Failed to read config file: VapeRPG_ExperienceTable.json! Recreating config...");
                CreateConfig(XpConfig);
            }
        }

        //Returns "true" if the config file was found and successfully loaded.
        static bool ReadConfig(Preferences conf)
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
                if(conf == CommonConfig)
                {
                    conf.Get("EnableUI", ref UIEnabled);
                    conf.Get("ChaosChance", ref ChaosChance);
                    conf.Get("MinChaosMultiplier", ref MinChaosMultiplier);
                    conf.Get("MaxChaosMultiplier", ref MaxChaosMultiplier);
                    conf.Get("FinalMultiplierForXpGain", ref FinalMultiplierForXpGain);
                    conf.Get("ExperienceGainDistance", ref ExperienceGainDistance);
                    conf.Get("GlobalXpMultiplier", ref GlobalXpMultiplier);
                }
                else if(conf == StatConfig)
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
                else if(conf == DefaultStatsConfig)
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
                else if(conf == IgnoresConfig)
                {
                    conf.Get("IgnoredTypesForXpGain", ref IgnoredTypesForXpGain);
                    conf.Get("IgnoredTypesChaos", ref IgnoredTypesChaos);
                }
                else if(conf == XpConfig)
                {
                    conf.Get("VanillaXpTable", ref VanillaXpTable);
                }
                return true;
            }
            return false;
        }

        //It would make more sense to call this method SaveConfig(), but since we don't have an in-game editor or anything, this will only be called if a config file wasn't found or it's invalid.
        static void CreateConfig(Preferences conf)
        {
            conf.Clear();

            if (conf == CommonConfig)
            {
                conf.Put("EnableUI", UIEnabled);
                conf.Put("ChaosChance", ChaosChance);
                conf.Put("MinChaosMultiplier", MinChaosMultiplier);
                conf.Put("MaxChaosMultiplier", MaxChaosMultiplier);
                conf.Put("FinalMultiplierForXpGain", FinalMultiplierForXpGain);
                conf.Put("ExperienceGainDistance", ExperienceGainDistance);
                conf.Put("GlobalXpMultiplier", GlobalXpMultiplier);
            }
            else if(conf == StatConfig)
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
            else if(conf == IgnoresConfig)
            {
                conf.Put("IgnoredTypesForXpGain", IgnoredTypesForXpGain);
                conf.Put("IgnoredTypesChaos", IgnoredTypesChaos);
            }
            else if(conf == XpConfig)
            {
                conf.Put("VanillaXpTable", VanillaXpTable);
            }

            conf.Save();
        }

        public static bool IsIgnoredType(NPC npc)
        {
            return IgnoredTypesForXpGain.Contains(npc.type) ||
                npc.TypeName.ToLower().Contains("pillar");
        }

        public static bool IsIgnoredTypeChaos(NPC npc)
        {
            return IgnoredTypesForXpGain.Contains(npc.type) ||
                    IgnoredTypesChaos.Contains(npc.type) ||
                    npc.TypeName.ToLower().Contains("head") ||
                    npc.TypeName.ToLower().Contains("body") ||
                    npc.TypeName.ToLower().Contains("tail") ||
                    npc.TypeName.ToLower().Contains("pillar") ||
                    npc.FullName.ToLower().Contains("head") ||
                    npc.FullName.ToLower().Contains("body") ||
                    npc.FullName.ToLower().Contains("tail") ||
                    npc.FullName.ToLower().Contains("pillar") ||
                    npc.GivenName.ToLower().Contains("head") ||
                    npc.GivenName.ToLower().Contains("body") ||
                    npc.GivenName.ToLower().Contains("tail") ||
                    npc.GivenName.ToLower().Contains("pillar") ||
                    npc.aiStyle == 6;
        }
    }
}
