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
        }


        public static void Load()
        {
            UI.SKILL_SHADE = ModLoader.GetTexture("VapeRPG/Textures/UI/Skills/SkillShade");
            UI.SQUARE_SHADE = ModLoader.GetTexture("VapeRPG/Textures/UI/TransparentSquare");
            UI.EXIT_BUTTON = ModLoader.GetTexture("VapeRPG/Textures/UI/Button/ExitButton");
            UI.ADD_BUTTON = ModLoader.GetTexture("VapeRPG/Textures/UI/Button/AddButton");
            UI.HELP_BUTTON = ModLoader.GetTexture("VapeRPG/Textures/UI/Button/HelpButton");
            UI.MINIMIZE_BUTTON = ModLoader.GetTexture("VapeRPG/Textures/UI/Button/MinimizeButton");
            UI.MINIMIZE_BUTTON_FLIPPED = ModLoader.GetTexture("VapeRPG/Textures/UI/Button/MinimizeButtonFlipped");
        }
    }
}
