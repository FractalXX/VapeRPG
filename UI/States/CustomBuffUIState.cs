using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    class CustomBuffUIState : UIState
    {
        public static bool visible = false;

        public void UpdateBuffs(int[] buffTypes)
        {
            base.Elements.Clear();

            int activeBuffs = 0;

            if(buffTypes.Length > 0)
            {
                foreach (int i in buffTypes)
                {
                    Texture2D texture = Main.buffTexture[i];
                    if(texture != null)
                    {
                        UIBuffIcon buffImage = new UIBuffIcon(i);
                        buffImage.Left.Set(20 + activeBuffs * (texture.Width + 5), 0);
                        buffImage.Top.Set(75, 0);
                        activeBuffs++;

                        base.Append(buffImage);
                    }
                }
            }
        }
    }
}
