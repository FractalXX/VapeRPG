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
    enum SkillType { OnHit, OnKill }

    class Skill
    {
        public string name;
        public string description;

        public int maxLevel;

        public Texture2D icon;

        internal List<Skill> Prerequisites { get; private set; }
        internal List<Skill> Children { get; private set; }

        public SkillType type;
        public bool needsAllPrerequisites;

        public Skill(string name, string description, int maxLevel, SkillType type, Texture2D icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.maxLevel = maxLevel;

            this.needsAllPrerequisites = false;

            this.Prerequisites = new List<Skill>();
            this.Children = new List<Skill>();

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
            else if (this.name == "Reaver")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Exploding Rage"));
            }
            else if (this.name == "Soul Reaver")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Reaver"));
                this.Prerequisites.Add(VapeRPG.GetSkill("Energizing Kills"));
                this.needsAllPrerequisites = true;
            }

            else if(this.name == "Energizing Kills")
            {
                this.Prerequisites.Add(VapeRPG.GetSkill("Mana Addict"));
            }
        }

        internal void AddChildren()
        {
            if (this.name == "Excitement")
            {
                this.Children.Add(VapeRPG.GetSkill("Bloodlust"));
                this.Children.Add(VapeRPG.GetSkill("Mana Addict"));
            }
            if (this.name == "Bloodlust")
            {
                this.Children.Add(VapeRPG.GetSkill("Exploding Rage"));
            }
            else if (this.name == "Exploding Rage")
            {
                this.Children.Add(VapeRPG.GetSkill("Reaver"));
            }
            else if (this.name == "Reaver")
            {
                this.Children.Add(VapeRPG.GetSkill("Soul Reaver"));
            }

            else if (this.name == "Mana Addict")
            {
                this.Children.Add(VapeRPG.GetSkill("Energizing Kills"));
            }
        }
    }
}
