using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

using VapeRPG.UI.Elements;

namespace VapeRPG
{
    enum SkillType { GeneralWeapon, OnKill, General }

    class Skill
    {
        public string name;
        public string description;

        public int maxLevel;

        public Texture2D icon;

        internal List<Skill> Prerequisites { get; private set; }

        public SkillType type;

        public Skill(string name, string description, int maxLevel, SkillType type, Texture2D icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.maxLevel = maxLevel;

            this.Prerequisites = new List<Skill>();

            this.type = type;
        }

        public Skill(string name, string description, int maxLevel, SkillType type) : this(name, description, maxLevel, type, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrame")) { }

        internal void AddPrerequisites()
        {
            if (this.name == "Bloodlust" || this.name == "Mana Addict")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Excitement"));
            }
            else if (this.name == "Exploding Rage")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Bloodlust"));
            }
            else if (this.name == "Static Field")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Exploding Rage"));
            }
            else if (this.name == "High-Voltage Field")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Static Field"));
                this.Prerequisites.Add(VapeRPG.GetSkill("Energizing Kills"));
            }
            else if(this.name == "Rage")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Bloodlust"));
            }
            else if (this.name == "Overkill")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Rage"));
            }
            else if (this.name == "Fury")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Overkill"));
            }

            else if(this.name == "Magic Sparks")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Mana Addict"));
            }
            else if(this.name == "Overkill Charge")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Magic Sparks"));
            }
            else if(this.name == "Spectral Sparks")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Overkill Charge"));
                this.Prerequisites.Add(VapeRPG.GetSkill("Energizing Kills"));
            }
            else if(this.name == "Energizing Kills")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Mana Addict"));
            }

            else if(this.name == "Leftover Supply" || this.name == "Confusion" || this.name == "High Five")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("X-Ray Hits"));
            }
            
            else if(this.name == "Bounce")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Leftover Supply"));
            }
            
            else if(this.name == "Confusion Field")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Confusion"));
            }

            else if (this.name == "Titan Grip")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("High Five"));
            }
            else if (this.name == "Hawk Eye")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Titan Grip"));
            }

            else if(this.name == "Ammo Hoarding")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("High Five"));
            }
            else if (this.name == "Close Combat Specialist")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Ammo Hoarding"));
            }

            else if(this.name == "Kickstart" || this.name == "Aggro" || this.name == "Longer Flight")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Warmth"));
            }

            else if(this.name == "Execution")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Kickstart"));
            }
            else if (this.name == "First Touch")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Execution"));
            }

            else if (this.name == "Reflection" || this.name == "Damage to Defense")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Aggro"));
            }

            else if (this.name == "Strengthen")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Reflection"));
            }

            else if (this.name == "Vital Supplies")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Damage to Defense"));
            }
            else if (this.name == "Hardened Skin")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Vital Supplies"));
                this.Prerequisites.Add(VapeRPG.GetSkill("Strengthen"));
            }

            else if (this.name == "Angel")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Longer Flight"));
            }
        }
    }
}
