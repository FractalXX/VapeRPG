using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace VapeRPG.Util
{
    public static class Textures
    {
        public static class UI
        {
            public static Texture2D SQUARE_SHADE;
            public static Texture2D SKILL_SHADE;

            public static Texture2D EXIT_BUTTON;
            public static Texture2D ADD_BUTTON;
            public static Texture2D HELP_BUTTON;
            public static Texture2D MINIMIZE_BUTTON;
            public static Texture2D MINIMIZE_BUTTON_FLIPPED;

            public static Texture2D SKILL_BRANCH_LINE;
            public static Texture2D SKILL_ICON_DEFAULT;
        }


        public static void Load()
        {
            UI.SKILL_SHADE = ModContent.GetTexture("VapeRPG/Textures/UI/Skills/SkillShade");
            UI.SQUARE_SHADE = ModContent.GetTexture("VapeRPG/Textures/UI/TransparentSquare");
            UI.EXIT_BUTTON = ModContent.GetTexture("VapeRPG/Textures/UI/Button/ExitButton");
            UI.ADD_BUTTON = ModContent.GetTexture("VapeRPG/Textures/UI/Button/AddButton");
            UI.HELP_BUTTON = ModContent.GetTexture("VapeRPG/Textures/UI/Button/HelpButton");
            UI.MINIMIZE_BUTTON = ModContent.GetTexture("VapeRPG/Textures/UI/Button/MinimizeButton");
            UI.MINIMIZE_BUTTON_FLIPPED = ModContent.GetTexture("VapeRPG/Textures/UI/Button/MinimizeButtonFlipped");
            UI.SKILL_BRANCH_LINE = ModContent.GetTexture("VapeRPG/Textures/UI/Skills/Line");
            UI.SKILL_ICON_DEFAULT = ModContent.GetTexture("VapeRPG/Textures/UI/SkillFrame");
        }

        public static void Unload()
        {
            UI.SKILL_SHADE = null;
            UI.SQUARE_SHADE = null;
            UI.EXIT_BUTTON = null;
            UI.ADD_BUTTON = null;
            UI.HELP_BUTTON = null;
            UI.MINIMIZE_BUTTON = null;
            UI.MINIMIZE_BUTTON_FLIPPED = null;
            UI.SKILL_BRANCH_LINE = null;
            UI.SKILL_ICON_DEFAULT = null;
        }
    }
}
