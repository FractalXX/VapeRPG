using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace VapeRPG
{
    enum SkillType { Melee, Magic, Ranged, Tank, Utility}

    class Skill
    {
        public string name;
        public string description;

        public int maxLevel;

        public Texture2D icon;
        public SkillType skillType;

        public Skill(string name, string description, int maxLevel, SkillType skillType, Texture2D icon)
        {
            this.name = name;
            this.description = description;
            this.icon = icon;
            this.maxLevel = maxLevel;
            this.skillType = skillType;
        }

        public Skill(string name, string description, int maxLevel, SkillType skillType) : this(name, description, maxLevel, skillType, ModLoader.GetTexture("VapeRPG/Textures/UI/SkillFrameEmpty")) { }
    }
}
