using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.Localization;

using VapeRPG.UI;
using VapeRPG.UI.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG
{
    class VapeRPG : Mod
    {
        public const int maxLevel = 200;
        public int[] XpNeededForLevel { get; private set; }
        public int[] XpNeededForChaosRank { get; private set; }
        public ExpUIState ExpUI { get; private set; }
        public CharUIState CharUI { get; private set; }
        public CustomBuffUIState BuffUI { get; private set; }
        private UserInterface userInterface;

        public static Texture2D itemQualityFrame;

        public static string[] baseStats =
        {
            "Strength",
            "Magic power",
            "Dexterity",
            "Agility",
            "Intellect",
            "Vitality"
        };

        public static string[] minorStats =
        {
            "Melee Damage",
            "Magic Damage",
            "Ranged Damage",
            "Melee Crit",
            "Magic Crit",
            "Ranged Crit",
            "Melee Speed",
            "Movement Speed",
            "Dodge Chance"
        };

        public static List<Skill> skills { get; private set; }

        public static ModHotKey charWindowHotKey;

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
            XpNeededForLevel = new int[maxLevel + 1];
            XpNeededForChaosRank = new int[maxLevel + 1];

            XpNeededForLevel[0] = 0;
            XpNeededForLevel[1] = 0;

            XpNeededForChaosRank[0] = 0;
            XpNeededForChaosRank[1] = 20;

            for (int i = 2; i < XpNeededForLevel.Length; i++)
            {
                double value;
                value = 12 * Math.Pow(i, 2) + 1.486 * i * Math.Pow(i, 1.6 * Math.Sqrt(1 - 1 / i)) * Math.Log(i);
                XpNeededForLevel[i] = (int)value;
                XpNeededForChaosRank[i] = (int)(value / 2);
            }

            skills = new List<Skill>()
            {
                //Tank
                new Skill("Snail armor", "Movement speed decreases but armor is increased.", 10, SkillType.Tank, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameSnailArmor")),
                new Skill("Damage to defense", "Convert part of your (melee) damage to defense.", 10, SkillType.Tank, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameDamageToDefense")),
                new Skill("Thorns", "Enemies attacking you take damage.", 10, SkillType.Tank, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameThorns")),

                //Melee
                new Skill("Hemorrhage", "Additional damage is dealt to your enemies over time (after hit).", 10, SkillType.Melee, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameHemorrhage")),
                new Skill("Sacrifice", "Trade part of your life for additional melee damage.", 10, SkillType.Melee, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameSacrifice")),

                //Magic
                new Skill("Magic clusters", "Weapon hits explode into additional magic stars.", 10, SkillType.Magic, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameMagicClusters")),

                //Ranged
                new Skill("Explosive shots", "Your weapons fire explosive projectiles.", 10, SkillType.Ranged, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameExplosiveShots")),
                new Skill("Incendiary shots", "Your shots ignite your enemies.", 1, SkillType.Ranged, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameIncendiaryShots")),

                //Utility
                new Skill("Longer invulnerability", "You remain invulnerable longer after getting hit.", 10, SkillType.Utility, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameLongerInvulnerability")),
                new Skill("Longer flight", "You can fly longer.", 10, SkillType.Utility, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameLongerFlight")),
                new Skill("Steroids", "Taking damage increases your movement speed.", 10, SkillType.Utility, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameSteroids"))
            };

            itemQualityFrame = ModLoader.GetTexture("VapeRPG/Textures/UI/QualityFrame");

            charWindowHotKey = RegisterHotKey("Character window", "C");

            this.ExpUI = new ExpUIState();
            this.ExpUI.Activate();

            this.CharUI = new CharUIState();
            this.CharUI.Activate();

            this.BuffUI = new CustomBuffUIState();
            this.BuffUI.Activate();

            this.userInterface = new UserInterface();
            this.userInterface.SetState(this.ExpUI);

            ExpUIState.visible = true;
            CustomBuffUIState.visible = true;
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                //Remove Resource bars
                if (layers[i].Name.Contains("Resource Bars"))
                {
                    layers.RemoveAt(i);
                }
            }

            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: StatWindow",
                    delegate
                    {
                        if (CharUIState.visible)
                        {
                            CharUI.Update(Main._drawInterfaceGameTime);
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
                            ExpUI.Update(Main._drawInterfaceGameTime);
                            ExpUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
                layers.Insert(MouseTextIndex, new LegacyGameInterfaceLayer(
                    "VapeRPG: CustomBuffUI",
                    delegate
                    {
                        if (CustomBuffUIState.visible)
                        {
                            BuffUI.Update(Main._drawInterfaceGameTime);
                            BuffUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
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
    }
}
