using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ID;

using VapeRPG.UI.States;

namespace VapeRPG
{
    enum VapeRPGMessageType : byte
    {
        ServerTransformChaosNPC,
        ClientSyncXp,
        ClientSyncStats,
        ClientSyncLevel
    };

    class VapeRPG : Mod
    {
        public const int MaxLevel = 200; // Self-explanatory
        public int[] XpNeededForLevel { get; private set; }
        public int[] XpNeededForChaosRank { get; private set; }
        public ExpUIState ExpUI { get; private set; } // For the level/xp/chaos rank panel
        public CharUIState CharUI { get; private set; } // For the character panel
        private UserInterface expUserInterface;
        private UserInterface charUserInterface;

        //public static Texture2D itemQualityFrame;

        public static string[] BaseStats =
        {
            "Strength",
            "Magic power",
            "Dexterity",
            "Agility",
            "Intellect",
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
            "Movement Speed",
            "Dodge Chance",
            "Block Chance"
        };

        public static List<Skill> Skills { get; private set; } // Add new skills to that list under Load()

        public static ModHotKey CharWindowHotKey;

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
                new Skill("Excitement", "Killing enemies grants you a shine buff for 6 seconds.", 1, SkillType.OnKill),

                new Skill("Bloodlust", "Killing enemies grants you a regeneration buff for 10 seconds.", 1, SkillType.OnKill),
                new Skill("Exploding Rage", "Killing enemies with melee has a chance (10%/20%/30%) to result in their bodies exploding dealing 10% of their maximum life in a medium sized area.", 3, SkillType.OnKill),
                new Skill("Reaver", "Killing enemies has a chance (5%/10%/15%) to summon an ancient wraith to crush your enemies for 30 seconds. Wraith damage is 15 summon damage * by skill level.", 3, SkillType.OnKill),
                new Skill("Soul Reaver", "Your reaver minion applies a slow debuff on hit, and grants you the effect of the hunter and night vision potion while it lasts. Additionally, you can now maintain two wraiths at once.", 1, SkillType.OnKill),

                new Skill("Rage", "Killing enemies grants you the Rage buff, increasing your melee damage (3%/level) for 10 seconds.", 3, SkillType.OnKill),
                new Skill("Overkill", "Killing enemies with critical hits restore a small percent of your life (3%/level).", 3, SkillType.OnKill),
                new Skill("Fury", "The Rage buff also increases your melee speed by the same amount.", 1, SkillType.OnKill),

                new Skill("Mana Addict", "Kills give you a mana regen buff for 10 seconds.", 1, SkillType.OnKill),
                new Skill("Energizing Kills", "Killing enemies with magic or summon damage grants you a stack of 'Energized'. After getting 20 stacks you receive a movement speed increase and release low-damaging sparks with mediocre knockback on being hit for 10 seconds.", 1, SkillType.OnKill),

                new Skill("Magic Sparks", "Magic kills release little magic sparks which reduces the life of nearby enemies by 4%/level.", 2, SkillType.OnKill),
                new Skill("Overkill Charge", "Killing enemies with critical hits restore a small percent of your mana (4%/level).", 3, SkillType.OnKill),
                new Skill("Spectral Sparks", "Magic sparks also reduce the defense of enemies by 5%.", 1, SkillType.OnKill),

                //Shredder Tree
                new Skill("X-Ray Hits", "Hitting enemies has a chance to apply a spelunker buff on you for 3 seconds.", 1, SkillType.GeneralWeapon),

                new Skill("Leftover Supply", "Magic weapons have a chance to not consume mana. (2%/level)", 2, SkillType.GeneralWeapon),
                new Skill("Bounce", "Magic hits have a chance (10%/level) to spawn a spark that bounces to another enemy. The spark's damage is 10%/20%/30% of the original damage.", 3, SkillType.GeneralWeapon),

                new Skill("Confusion", "Hits have a chance to confuse your enemy. (5%/level)", 3, SkillType.GeneralWeapon),
                new Skill("Confusion Field", "Confusion applies to multiple enemies in a short range.", 1, SkillType.GeneralWeapon),

                new Skill("High Five", "Critical hits further increase your crit and damage by 5%. Resets on non-crits.", 1, SkillType.GeneralWeapon),
                new Skill("Hawk Eye", "Your critical chance increases with distance to your enemy.", 1, SkillType.GeneralWeapon),
                new Skill("Powerful Eye", "Critical hits have increased knockback.", 1, SkillType.GeneralWeapon),
                new Skill("Dead Eye", "Critical hits outside the screen deal 4x damage instead of 2x.", 1, SkillType.GeneralWeapon),

                new Skill("Reconsolidation", "Increase your chance to not consume ammo (4%/level).", 2, SkillType.GeneralWeapon),
                new Skill("Close Combat Specialist", "Your ranged damage increases as you get closer to the enemy.", 1, SkillType.GeneralWeapon),
                new Skill("Close Combat Veteran", "Your critical chance increases as you get closer to the enemy.", 1, SkillType.GeneralWeapon),

                //Power Tree
                new Skill("Warmth", "Increase your HP/MP regen by 10%", 1, SkillType.General),

                new Skill("Kickstart", "You have 5%/10% more critical chance against enemies above 70% health.", 2, SkillType.General),
                new Skill("Execution", "If your enemy is under 20% health you can't crit them but you deal 20%/30%/40% increased damage to them.", 3, SkillType.General),
                new Skill("First Touch", "If your enemy is at its max health, your first hit instantly damages it for 10% of their health.", 1, SkillType.General),

                new Skill("Aggro", "Enemies are more likely to target you.", 1, SkillType.General),

                new Skill("Reflection", "You reflect 5%/10% of melee damage done by your enemy.", 2, SkillType.General),
                new Skill("Strengthen", "After a dodge or block, the next time you take damage, the amount is reduced by 5%/level.", 2, SkillType.General),

                new Skill("Damage to Defense", "Your damage is reduced by 10%/level but your defense increases by the same amount.", 3, SkillType.General),
                new Skill("Vital Supplies", "Damage to Defense increases life as well.", 1, SkillType.General),
                new Skill("Hardened Skin", "Melee hits deal 5%/10% less damage to you.", 2, SkillType.General),

                new Skill("Longer Flight", "Your flight time is increased by 20%/level.", 3, SkillType.General),
                new Skill("Angel", "You deal increased damage (5%/level) while using your wing. Doesn't apply on jumps.", 2, SkillType.General)
            };


            foreach(Skill skill in Skills)
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

                this.expUserInterface = new UserInterface();
                this.expUserInterface.SetState(this.ExpUI);

                this.charUserInterface = new UserInterface();
                this.charUserInterface.SetState(this.CharUI);

                ExpUIState.visible = true;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
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
    }
}
