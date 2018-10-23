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

        public const int MaxLevel = 200; // Self-explanatory

        public int[] XpNeededForLevel { get; private set; }
        public int[] XpNeededForChaosRank { get; private set; }
        public ExpUIState ExpUI { get; private set; } // For the level/xp/chaos rank panel
        public CharUIState CharUI { get; private set; } // For the character panel
        public StatHelpUIState StatHelpUI { get; private set; }
        private UserInterface expUserInterface;
        private UserInterface charUserInterface;

        public static UserInterface ui;

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

        public static Skill GetSkill(string name)
        {
            return Skills.Find(x => x.name == name);
        }

        private static Texture2D GetSkillFrame(string name)
        {
            Texture2D frame;
            try
            {
                frame = ModLoader.GetTexture("VapeRPG/Textures/UI/Skills/" + name);
            }
            catch (Exception)
            {
                frame = ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrame");
            }

            return frame;
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
                        modPlayer.chaosXp = reader.ReadInt32();
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

            Skills = new List<Skill>()
            {
                //Reaper Tree
                new Skill("Excitement", "Killing enemies grants you a shine buff for 6 seconds.", 1, SkillTree.Reaper, GetSkillFrame("Excitement")),

                new Skill("Rage", "Killing enemies grants you the Rage buff, increasing your melee damage (3%/level) for 10 seconds.", 3, SkillTree.Reaper, GetSkillFrame("Rage")),

                new Skill("Bloodlust", "Killing enemies grants you a regeneration buff for 10 seconds.", 1, SkillTree.Reaper, GetSkillFrame("Bloodlust")),
                new Skill("Exploding Rage", "Killing enemies with melee has a chance (10%/20%/30%) to result in their bodies exploding dealing 10% of their maximum life in a medium sized area.", 3, SkillTree.Reaper, GetSkillFrame("ExplodingRage")),

                new Skill("Overkill", "Killing enemies with critical hits restore a small percent of your life (3%/level).", 3, SkillTree.Reaper, GetSkillFrame("Overkill")),
                new Skill("Fury", "The Rage buff also increases your melee speed by the same amount.", 1, SkillTree.Reaper, GetSkillFrame("Fury")),

                new Skill("Static Field", "Killing enemies has a chance (5%/10%/15%) to summon an electric shield around you. The shield does summon damage (20/level) and lasts 10 seconds.", 3, SkillTree.Reaper, GetSkillFrame("StaticField")),
                new Skill("High-Voltage Field", "Static Field applies slow debuff on hit and grants the night vision buff while it lasts. Upon taking fatal damage, your life gets restored to 50. This effect can only occur every 5 minutes. Duration also increases to 20 seconds.", 1, SkillTree.Reaper, GetSkillFrame("HighVoltageField")),

                new Skill("Mana Addict", "Kills give you a mana regen buff for 10 seconds.", 1, SkillTree.Reaper, GetSkillFrame("ManaAddict")),
                new Skill("Energizing Kills", "Killing enemies with magic or summon damage grants you a stack of 'Energized'. After getting 20 stacks you receive a movement speed increase and release low-damaging sparks with mediocre knockback on being hit for 10 seconds.", 1, SkillTree.Reaper, GetSkillFrame("EnergizingKills")),

                new Skill("Magic Sparks", "Magic kills have a chance (10%/level) to release little magic sparks which reduces the life of nearby enemies by 5%/level.", 2, SkillTree.Reaper, GetSkillFrame("MagicSparks")),
                new Skill("Overkill Charge", "Killing enemies with critical hits restore a small percent of your mana (4%/level).", 3, SkillTree.Reaper, GetSkillFrame("OverkillCharge")),
                new Skill("Spectral Sparks", "Magic sparks also reduce the defense of enemies by 15%.", 1, SkillTree.Reaper, GetSkillFrame("SpectralSparks")),

                //Shredder Tree
                new Skill("One Above All", "You have 2% chance to one hit kill any non-boss, non-pillar enemy.", 1, SkillTree.Shredder, GetSkillFrame("OneAboveAll")),

                new Skill("Bounce", "Magic hits have a chance (10%/level) to spawn a spark that bounces to another enemy. The spark's damage is 10%/20%/30% of the original damage.", 3, SkillTree.Shredder, GetSkillFrame("Bounce")),
                new Skill("Leftover Supply", "Magic weapons have a chance to not consume mana. (4%/level)", 2, SkillTree.Shredder, GetSkillFrame("LeftoverSupply")),

                new Skill("Confusion", "Hits have a chance to confuse your enemy. (5%/level)", 3, SkillTree.Shredder, GetSkillFrame("Confusion")),
                new Skill("Confusion Field", "Confusion applies to multiple enemies in a small radius.", 1, SkillTree.Shredder, GetSkillFrame("ConfusionField")),

                new Skill("High Five", "Critical hits further increase your crit and damage by 2%. Resets on non-crits or 50 stacks.", 1, SkillTree.Shredder, GetSkillFrame("HighFive")),
                new Skill("Titan Grip", "Critical hits have increased knockback (20%/level).", 2, SkillTree.Shredder, GetSkillFrame("TitanGrip")),
                new Skill("Hawk Eye", "Your critical chance increases with distance to your enemy.", 1, SkillTree.Shredder, GetSkillFrame("HawkEye")),

                new Skill("Close Combat Specialist", "You deal 1.5x damage to enemies within 10 tiles radius.", 1, SkillTree.Shredder, GetSkillFrame("CloseCombatSpecialist")),

                //Power Tree
                new Skill("Warmth", "Increases your HP/MP regen by 10%/level.", 2, SkillTree.Power, GetSkillFrame("Warmth")),

                new Skill("First Touch", "If your enemy is at its max health, your first hit (minions included) instantly damages it for 10% of their health.", 1, SkillTree.Power, GetSkillFrame("FirstTouch")),
                new Skill("Kickstart", "Your critical chance increases by 5%/10% against enemies above 70% health.", 2, SkillTree.Power, GetSkillFrame("Kickstart")),
                new Skill("Execution", "If your enemy is under 20% health you deal increased damage (10%/level) to them.", 3, SkillTree.Power, GetSkillFrame("Execution")),

                new Skill("Reflection", "You reflect 10%/20% of melee damage done by your enemy.", 2, SkillTree.Power, GetSkillFrame("Reflection")),
                new Skill("Strengthen", "After a dodge or block, the next time you take damage, the amount is reduced by 15%/level.", 2, SkillTree.Power, GetSkillFrame("Strengthen")),

                new Skill("Damage to Defense", "Your damage is reduced by 10%/level but your defense increases by the same amount.", 3, SkillTree.Power, GetSkillFrame("DamageToDefense")),
                new Skill("Vital Supplies", "Damage to Defense increases life as well.", 1, SkillTree.Power, GetSkillFrame("VitalSupplies")),
                new Skill("Hardened Skin", "Melee hits deal 5%/10% less damage to you.", 2, SkillTree.Power, GetSkillFrame("HardenedSkin")),

                new Skill("Longer Flight", "Your flight time is increased by 30%/level.", 3, SkillTree.Power, GetSkillFrame("LongerFlight")),
                new Skill("Angel", "You deal increased damage (5%/level) while using your wing. Doesn't apply on jumps.", 2, SkillTree.Power, GetSkillFrame("Angel"))
            };


            foreach (Skill skill in Skills)
            {
                skill.AddPrerequisites();
            }

            //itemQualityFrame = ModLoader.GetTexture("VapeRPG/Textures/UI/QualityFrame");

            CharWindowHotKey = RegisterHotKey("Character window", "C");

            if (Main.netMode != NetmodeID.Server)
            {
                this.ExpUI = new ExpUIState();
                this.ExpUI.Activate();

                this.CharUI = new CharUIState();
                this.CharUI.Activate();

                this.StatHelpUI = new StatHelpUIState();
                this.StatHelpUI.Activate();

                this.expUserInterface = new UserInterface();
                this.expUserInterface.SetState(this.ExpUI);

                this.charUserInterface = new UserInterface();
                this.charUserInterface.SetState(this.CharUI);
                ui = this.charUserInterface;

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
                    delegate
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
                    delegate
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
                    delegate
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
            }
        }
    }
}
