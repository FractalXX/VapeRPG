using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader.Config;

namespace VapeRPG
{
    public class VapeConfig : ModConfig{
        public override ConfigScope Mode => ConfigScope.ServerSide;
        //Common
        [Header("Vape RPG Module")]
        [Label("Chaos Chance")]
        [Tooltip("Chance for spawned enemy be a Chaos enemy, Default: 5%\nMUST RELOAD MODS")]
        [DefaultValue(5)]
        [Range(0, 100)]
        [ReloadRequired]//So that players do not abuse this
        public int ChaosChance;
        [Label("Minimum Chaos Multiplier")]
        [Tooltip("Defines the minimum strenght a Chaos mob can have. Default: 3x\nMUST RELOAD MODS")]
        [DefaultValue(3f)]
        [Range(1f, 10f)]
        [ReloadRequired]//So that players do not abuse this
        public int MinChaosMultiplier;
        [Label("Maximum Chaos Multiplier")]
        [Tooltip("Defines the maximum strenght a Chaos mob can have. Default: 6x\nMUST RELOAD MODS")]
        [DefaultValue(6f)]
        [Range(5f, 20f)]
        [ReloadRequired]//So that players do not abuse this
        public int MaxChaosMultiplier;
        [Label("Global Exp Rate")]//Applied for normal exp and chaos exp
        [Tooltip("Applies for both normal and chaos exp. Default: 1x\nMUST RELOAD MODS")]
        [DefaultValue(0.5f)]
        [Increment(0.5f)]
        [Range(0.5f, 10f)]
        [ReloadRequired]//So that players do not abuse this
        public float GlobalXpMultiplier;
        [Label("Experience Gain Distance")]
        [Tooltip("Maximum distance you need to be to acquire experience. Default: 3000 blocks")]
        [DefaultValue(3000)]
        [Range(1000, 10000)]
        public int ExperienceGainDistance;
        [Label("Exp Curve Multiplier")]//Applied for normal exp only
        [Tooltip("Changes how much experience you need to get per level. Default: 1.486x\nMUST RELOAD MODS")]
        [DefaultValue(1.486f)]
        [Increment(0.001f)]
        [Range(0.486f, 2.686f)]
        [ReloadRequired]//So that players do not abuse this
        public float ExperienceCurveMultiplier;

        // Stats
        [Header("Stats Module")]
        [Label("Stat Points per Level")]
        [Tooltip("Amount of stats points gained on level up. Default: 5\nMUST RELOAD MODS")]
        [DefaultValue(5)]
        [Increment(1)]
        [Range(0,10)]
        [ReloadRequired]//So that players do not abuse this
        public int StatPointsPerLevel = 5;
        [DefaultValue(500)]
        [JsonIgnore]
        public float MeleeDamageDivider = 500f;
        [DefaultValue(430f)]
        [JsonIgnore]
        public float MagicDamageDivider = 430f;
        [DefaultValue(465f)]
        [JsonIgnore]
        public float RangedDamageDivider = 465f;
        [DefaultValue(400f)]
        [JsonIgnore]
        public float MinionDamageDivider = 400f;
        [DefaultValue(860f)]
        [JsonIgnore]
        public float MagicDamageBySpiritDivider = 860f;

        [DefaultValue(10f)]
        [JsonIgnore]
        public float MeleeCritDivider = 10f;
        [DefaultValue(7f)]
        [JsonIgnore]
        public float MagicCritDivider = 7f;
        [DefaultValue(8.5f)]
        [JsonIgnore]
        public float RangedCritDivider = 8.5f;

        [DefaultValue(900)]
        [JsonIgnore]
        public float MeleeSpeedDivider = 900f;
        [DefaultValue(1800)]
        [JsonIgnore]
        public float MoveSpeedDivider = 1800f;
        [DefaultValue(1800)]
        [JsonIgnore]
        public float DodgeDivider = 1800f;
        [DefaultValue(50)]
        [JsonIgnore]
        public float SpiritPerMaxMinion = 50;
        [DefaultValue(100)]
        [JsonIgnore]
        public float SpiritPerMaxTurret = 100;

        [Label("Max Dodge Chance")]
        [Tooltip("Max value for dodge chance. Default: 0.7")]
        [DefaultValue(0.7f)]
        [Increment(0.05f)]
        [Range(0f,1f)]
        public float MaxDodgeChance = 0.7f;
        [Label("Life per Level")]
        [Tooltip("Amount of life adquired when leveled up. Default: 5\nMUST RELOAD MODS")]
        [DefaultValue(5f)]
        [Increment(1f)]
        [Range(0f,20f)]
        [ReloadRequired]//So that players do not abuse this
        public float LifePerLevel = 5;
        [Label("Mana per Level")]
        [Tooltip("Amount of mana adquired on level up. Default: 4\nMUST RELOAD MODS")]
        [DefaultValue(4f)]
        [Increment(1f)]
        [Range(0f,20f)]
        [ReloadRequired]//So that players do not abuse this
        public float ManaPerLevel = 4;
        [Label("Life per Vitality")]
        [Tooltip("Amount of life adquire per Vitality point. Default: 2\nMUST RELOAD MODS")]
        [DefaultValue(2f)]
        [Increment(1f)]
        [Range(0f,20f)]
        [ReloadRequired]//So that players do not abuse this
        public float LifePerVitality = 2;
        [Label("Mana per Magic Power")]
        [Tooltip(". Default: \nMUST RELOAD MODS")]
        [DefaultValue(0)]
        [Increment(1)]
        [Range(0,20)]
        [ReloadRequired]//So that players do not abuse this
        public float ManaPerMagicPower = 0;
        [Label("Vitality per Defense")]
        [Tooltip(". Default: \nMUST RELOAD MODS")]
        [DefaultValue(10)]
        [Increment(1)]
        [Range(0,20)]
        [ReloadRequired]//So that players do not abuse this
        public float VitalityPerDefense = 10;
        [Label("Strength per Life")]
        [Tooltip(". Default: \nMUST RELOAD MODS")]
        [DefaultValue(2)]
        [Increment(1)]
        [Range(0,10)]
        [ReloadRequired]//So that players do not abuse this
        public float StrengthPerLife = 2;

        // Default stats
        [Header("Initial Stats Module")]
        [Label("Initial Melee Damage")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.7f)]
        [Increment(0.05f)]
        [Range(0.1f, 1f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefMeleeDamage = 0.7f;
        [Label("Initial Magic Damage")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.9f)]
        [Increment(0.05f)]
        [Range(0.1f, 1f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefMagicDamage = 0.9f;
        [Label("Initial Ranged Damage")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.8f)]
        [Increment(0.05f)]
        [Range(0.1f, 1f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefRangedDamage = 0.8f;
        [Label("Initial Summoner Damage")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.8f)]
        [Increment(0.05f)]
        [Range(0.1f, 1f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefMinionDamage = 0.8f;
        [Label("Initial Thrower Damager")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.8f)]
        [Increment(0.05f)]
        [Range(0.1f, 1f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefThrownDamage = 0.8f;
        [Label("Default Melee Crit Chance")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(1)]
        [Increment(1)]
        [Range(1, 15)]
        [ReloadRequired]//So that players do not abuse this
        public int DefMeleeCrit = 1;
        [Label("Default Magic Crit Chance")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(1)]
        [Increment(1)]
        [Range(1, 15)]
        [ReloadRequired]//So that players do not abuse this
        public int DefMagicCrit = 1;
        [Label("Default Ranged Crit Chance")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(1)]
        [Increment(1)]
        [Range(1, 15)]
        [ReloadRequired]//So that players do not abuse this
        public int DefRangedCrit = 1;
        [Label("Default Thrower Crit Chance")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(1)]
        [Increment(1)]
        [Range(1, 15)]
        [ReloadRequired]//So that players do not abuse this
        public int DefThrownCrit = 1;
        [Label("Default Dodge Chance")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0f)]
        [Increment(0.1f)]
        [Range(0f, 10f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefDodge = 0;
        [Label("Default Melee Speed")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(0.8f)]
        [Increment(0.1f)]
        [Range(0.1f, 2f)]
        [ReloadRequired]//So that players do not abuse this
        public float DefMeleeSpeed = 0.8f;
        [Label("Initial Max Life")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(100)]
        [Increment(20)]
        [Range(1, 1000)]
        [ReloadRequired]//So that players do not abuse this
        public int DefLife = 100;
        [Label("Initial Max Mana")]
        [Tooltip("MUST RELOAD MODS")]
        [DefaultValue(20)]
        [Increment(20)]
        [Range(1, 1000)]
        [ReloadRequired]//So that players do not abuse this
        public int DefMana = 20;

        // Xp gain overrides
        // Vanilla NPCs
        [JsonIgnore]
        public Dictionary<int, double> VanillaXpTable = new Dictionary<int, double>()
        {
            {NPCID.BigMimicJungle, 10 }
        };

        // Ignore lists
        [JsonIgnore]
        public List<int> IgnoredTypesForXpGain = new List<int>()
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
        [JsonIgnore]
        public List<int> IgnoredTypesChaos = new List<int>()
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

        public bool IsIgnoredType(NPC npc)
        {
            return IgnoredTypesForXpGain.Contains(npc.type) ||
                npc.TypeName.ToLower().Contains("pillar");
        }

        public bool IsIgnoredTypeChaos(NPC npc)
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