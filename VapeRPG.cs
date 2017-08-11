using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;
using Terraria.ID;

using VapeRPG.UI;
using VapeRPG.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

using VapeRPG.DataStructures;

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
        public static TreeNode<Skill> OnKillEffectTree;
        public static TreeNode<Skill> OnHitEffectTree;

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
                //On Kill Effects
                new Skill("Excitement", "Killing enemies grants you a shine buff for 6 seconds.", 1, SkillType.OnKill),
                new Skill("X-Ray Hits", "Hitting enemies has a chance to apply a spelunker buff on you.", 1, SkillType.OnHit),

                new Skill("Mana Addict", "Kills give you a minor mana regen bonus.", 1, SkillType.OnKill),
                new Skill("Energizing Kills", "Killing enemies with magic or summon damage grants you a stack of 'Energized'. After getting 20 stacks you receive a movement speed steroid and release low-damaging sparks with mediocre knockback on being hit for 5 seconds.", 1, SkillType.OnKill),

                new Skill("Bloodlust", "Killing enemies grants you a regeneration buff for 10 seconds.", 1, SkillType.OnKill),
                new Skill("Exploding Rage", "Killing enemies with melee has a chance (10%/20%/30%) to result in their bodies exploding blood and gore dealing 10% of their maximum life in a medium sized area.", 3, SkillType.OnKill),
                new Skill("Reaver", "Killing enemies has a chance (5%/10%/15%) to summon an ancient wraith(the one we all hate when we go to pwnhammer some demon altars) to crush your enemies for 30 seconds. Wraith damage is 15 summon damage * by skill level.", 3, SkillType.OnKill),
                new Skill("Soul Reaver", "Your reaver minion applies a slow debuff on hit, and grants you effect of the hunter/night vision potion while it lasts. Additionally, you can now maintain two wraiths at ones.", 1, SkillType.OnKill),
            };


            foreach(Skill skill in Skills)
            {
                skill.AddPrerequisites();
                skill.AddChildren();
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

        /*private void InitializeSkillTrees()
        {
            OnKillEffectTree = new TreeNode<Skill>(GetSkill("Excitement"));
            OnHitEffectTree = new TreeNode<Skill>(GetSkill("X-Ray Kills"));

            foreach(Skill skill in Skills)
            {
                if(skill.type == SkillType.OnKill)
                {
                    if(skill.Prerequisites.Count > 0)
                    {
                        Skill prerequisite = skill.Prerequisites[0];
                        TreeNode<Skill> prerequisiteNode = OnKillEffectTree.Children.Find(x => x.Value == prerequisite);
                        if(prerequisiteNode != null)
                        {
                            prerequisiteNode.AddChild(new TreeNode<Skill>(skill));
                        }
                    }
                }
            }
        }*/

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
