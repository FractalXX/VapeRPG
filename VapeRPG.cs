using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ID;

using Microsoft.Xna.Framework.Graphics;

using VapeRPG.UI.States;
using System.Reflection;
using System.Linq;
using VapeRPG.Util;

namespace VapeRPG
{
    internal enum VapeRPGMessageType : byte
    {
        ServerTransformChaosNPC,
        ClientSyncXp,
        ClientSyncStats,
        ClientSyncLevel
    };

    class VapeRPG : Mod
    {
        public static Random rand = new Random();

        public static string[] BaseStats =
        {
            "Strength",
            "Magic power",
            "Dexterity",
            "Haste",
            "Vitality",
            "Spirit"
        };

        public static string[] MinorStats =
        {
            "Melee Damage",
            "Melee Crit",
            "Melee Speed",
            "Ranged Damage",
            "Ranged Crit",
            "Magic Damage",
            "Magic Crit",
            "Minion Damage",
            "Max Minions",
            "Max Run Speed",
            "Dodge Chance",
            "Block Chance"
        };

        public static List<Skill> Skills { get; private set; } // Add new skills to that list under Load()

        public static ModHotKey CharWindowHotKey;
        public static ModHotKey[] SkillHotKeys;

        public const int MaxLevel = 200; // Self-explanatory

        public int[] XpNeededForLevel { get; private set; }
        public int[] XpNeededForChaosRank { get; private set; }
        public ExpUIState ExpUI { get; private set; } // For the level/xp/chaos rank panel
        public CharUIState CharUI { get; private set; } // For the character panel
        public StatHelpUIState StatHelpUI { get; private set; }
        public SkillBarUIState SkillBarUI { get; private set; }

        private UserInterface expUserInterface;
        private UserInterface charUserInterface;
        private UserInterface skillBarUserInterface;

        private TileUI currentTileUI;
        private UserInterface tileUserInterface;

        public static UserInterface ui;

        static VapeRPG()
        {
            Skills = new List<Skill>();
        }

        public VapeRPG()
        {
            this.Properties = new ModProperties()
            {
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true
            };
        }

        public static string GetBuffDescription(int type)
        {
            string description = Lang.GetBuffDescription(type);

            if (type > 0)
            {
                if (type == 26 && Main.expertMode) description = Language.GetTextValue("BuffDescription.WellFed_Expert");
                if (type == 94)
                {
                    int num31 = (int)(Main.player[Main.myPlayer].manaSickReduction * 100f) + 1;
                    description += num31 + "%";
                }
            }
            return description;
        }

        public static Skill GetSkill<T>()
            where T : Skill
        {
            return Skills.Find(x => x.GetType() == typeof(T));
        }

        public static Skill GetSkill(Type type)
        {
            return Skills.Find(x => x.GetType() == type);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            VapeRPGMessageType msgType = (VapeRPGMessageType)reader.ReadByte();

            switch (msgType)
            {
                case VapeRPGMessageType.ServerTransformChaosNPC:
                    int chaosMultiplier = reader.ReadInt32();
                    int index = reader.ReadInt32();

                    NPC npc = Main.npc[index];
                    VapeGlobalNpc global = npc.GetGlobalNPC<VapeGlobalNpc>();

                    global.chaosMultiplier = chaosMultiplier;
                    global.isChaos = true;
                    break;

                case VapeRPGMessageType.ClientSyncStats:
                    Player player = Main.player[reader.ReadInt32()];

                    if (!player.Equals(Main.LocalPlayer) || Main.netMode == NetmodeID.Server)
                    {
                        VapePlayer modPlayer = player.GetModPlayer<VapePlayer>();

                        for (int i = 0; i < BaseStats.Length; i++)
                        {
                            string[] keyValuePair = reader.ReadString().Split(' ');
                            modPlayer.BaseStats[keyValuePair[0]] = int.Parse(keyValuePair[1]);
                        }

                        for (int i = 0; i < BaseStats.Length; i++)
                        {
                            string[] keyValuePair = reader.ReadString().Split(' ');
                            modPlayer.EffectiveStats[keyValuePair[0]] = int.Parse(keyValuePair[1]);
                        }
                    }

                    break;

                case VapeRPGMessageType.ClientSyncXp:
                    player = Main.player[reader.ReadInt32()];

                    if (Main.netMode == NetmodeID.Server || !player.Equals(Main.LocalPlayer))
                    {
                        VapePlayer modPlayer = player.GetModPlayer<VapePlayer>();

                        modPlayer.xp = reader.ReadInt32();
                    }
                    break;

                case VapeRPGMessageType.ClientSyncLevel:
                    player = Main.player[reader.ReadInt32()];

                    if (Main.netMode == NetmodeID.Server || !player.Equals(Main.LocalPlayer))
                    {
                        VapePlayer modPlayer = player.GetModPlayer<VapePlayer>();

                        modPlayer.level = reader.ReadInt32();
                    }
                    break;
            }
        }

        public override void Load()
        {
            VapeConfig.Load();

            XpNeededForLevel = new int[MaxLevel + 1];
            XpNeededForChaosRank = new int[MaxLevel + 1];

            XpNeededForLevel[0] = 0;
            XpNeededForLevel[1] = 0;

            XpNeededForChaosRank[0] = 0;
            XpNeededForChaosRank[1] = 40;

            for (int i = 2; i < XpNeededForLevel.Length; i++)
            {
                double value;
                value = 2 * (12 * Math.Pow(i, 2) + 1.486 * i * Math.Pow(i, 1.6 * Math.Sqrt(1 - 1 / i)) * Math.Log(i)) + XpNeededForLevel[i - 1];
                XpNeededForLevel[i] = (int)value;
                XpNeededForChaosRank[i] = (int)(value / 1.5f);
            }

            Type[] skillTypes = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "VapeRPG.Skills");
            foreach (Type type in skillTypes)
            {
                Skills.Add(Activator.CreateInstance(type) as Skill);
            }

            CharWindowHotKey = RegisterHotKey("Character window", "C");
            SkillHotKeys = new ModHotKey[SkillBarUIState.SKILL_SLOT_COUNT];
            SkillHotKeys[0] = RegisterHotKey("Use skill 1", "Y");
            SkillHotKeys[1] = RegisterHotKey("Use skill 2", "X");
            SkillHotKeys[2] = RegisterHotKey("Use skill 3", "C");
            SkillHotKeys[3] = RegisterHotKey("Use skill 4", "V");

            if (Main.netMode != NetmodeID.Server)
            {
                Textures.Load();

                this.ExpUI = new ExpUIState();
                this.ExpUI.Activate();

                this.CharUI = new CharUIState();
                this.CharUI.Activate();

                this.StatHelpUI = new StatHelpUIState();
                this.StatHelpUI.Activate();

                this.SkillBarUI = new SkillBarUIState();
                this.SkillBarUI.Activate();

                this.expUserInterface = new UserInterface();
                this.expUserInterface.SetState(this.ExpUI);

                this.charUserInterface = new UserInterface();
                this.charUserInterface.SetState(this.CharUI);
                ui = this.charUserInterface;

                this.skillBarUserInterface = new UserInterface();
                this.skillBarUserInterface.SetState(this.SkillBarUI);

                this.tileUserInterface = new UserInterface();

                ExpUIState.visible = true;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: StatHelp",
                    () =>
                    {
                        if (StatHelpUIState.visible)
                        {
                            StatHelpUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: StatWindow",
                    () =>
                    {
                        if (CharUIState.visible)
                        {
                            charUserInterface.Update(Main._drawInterfaceGameTime);
                            CharUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: ExperienceBar",
                    () =>
                    {
                        if (ExpUIState.visible)
                        {
                            expUserInterface.Update(Main._drawInterfaceGameTime);
                            ExpUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: SkillBar",
                    () =>
                    {
                        if (SkillBarUIState.visible)
                        {
                            this.skillBarUserInterface.Update(Main._drawInterfaceGameTime);
                            this.SkillBarUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                "VapeRPG: TileUI",
                () =>
                {
                    if (this.currentTileUI != null && this.currentTileUI.visible)
                    {
                        this.tileUserInterface.Update(Main._drawInterfaceGameTime);
                        this.currentTileUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI)
            );
            }
        }

        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public void SetTileUIState(TileUI tileUI)
        {
            this.tileUserInterface.SetState(tileUI);
            this.currentTileUI = tileUI;
        }
    }
}
