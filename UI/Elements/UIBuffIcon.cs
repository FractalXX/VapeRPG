using System;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VapeRPG.UI.Elements
{
    class UIBuffIcon : UIImage
    {
        public int buffType;
        private UIText timerText;

        public UIBuffIcon(int buffType) : base(Main.buffTexture[buffType])
        {
            this.buffType = buffType;
            this.timerText = new UIText("100s");
            this.timerText.Top.Set(this.Height.Pixels + 5, 0);
            this.Append(timerText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);

            int index = 0;

            if (!Main.vanityPet[this.buffType] && !Main.lightPet[this.buffType] && !Main.buffNoTimeDisplay[this.buffType] && (!Main.LocalPlayer.honeyWet || this.buffType != 48) && (!Main.LocalPlayer.wet || !Main.expertMode || this.buffType != 46))
            {
                for(int i = 0; i < Main.LocalPlayer.buffType.Length; i++)
                {
                    if(Main.LocalPlayer.buffType[i] == this.buffType)
                    {
                        index = i;
                    }       
                }
                this.timerText.SetText(String.Format("{0}s", (int)Math.Round(Main.LocalPlayer.buffTime[index] / 60f, 1)));
                this.CenterTimerText();
            }

            MouseState ms = Mouse.GetState();
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);

            if (this.ContainsPoint(mousePosition))
            {
                if (this.buffType == 147) Main.bannerMouseOver = true;
                Utils.DrawBorderString(spriteBatch, VapeRPG.GetBuffDescription(this.buffType), new Vector2(ms.X, ms.Y + 20), Color.White);

                if(ms.RightButton == ButtonState.Pressed && !Main.debuff[this.buffType])
                {
                    MethodInfo mTryRemovingBuff = typeof(Main).GetMethod("TryRemovingBuff", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod);
                    mTryRemovingBuff.Invoke(Main.instance, new object[] { index, this.buffType });
                }
            }
        }

        private void CenterTimerText()
        {
            this.timerText.Left.Set(this.Width.Pixels / 2 - this.timerText.Width.Pixels / 2, 0f);
        }
    }
}
