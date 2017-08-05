using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    class StatusBarUIState : UIState
    {
        private UIVapeProgressBar hpBar;
        private UIVapeProgressBar mpBar;

        public static bool visible = true;

        public override void OnInitialize()
        {
            this.hpBar = new UIVapeProgressBar(1, 0, 100, Color.Gray, Color.Red);
            this.hpBar.Width.Set(200, 0);
            this.hpBar.Height.Set(20, 0);
            this.hpBar.strokeThickness = 2;
            this.hpBar.Left.Set(Main.screenWidth - this.hpBar.Width.Pixels - 50, 0);
            this.hpBar.Top.Set(10, 0);

            this.mpBar = new UIVapeProgressBar(1, 0, 100, Color.Gray, Color.Blue);
            this.mpBar.Width.Set(200, 0);
            this.mpBar.Height.Set(20, 0);
            this.mpBar.strokeThickness = 2;
            this.mpBar.Left.Set(Main.screenWidth - this.mpBar.Width.Pixels - 50, 0);
            this.mpBar.Top.Set(40, 0);

            base.Append(this.hpBar);
            base.Append(this.mpBar);
        }

        public void UpdateHpMp(float hp, float mp, float maxHp, float maxMp)
        {
            this.hpBar.value = hp;
            this.mpBar.value = mp;

            this.hpBar.maxValue = maxHp;
            this.mpBar.maxValue = maxMp;
        }
    }
}
