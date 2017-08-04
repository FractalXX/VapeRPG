using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VapeRPG.UI.Elements
{
    class UIStatInfo : UIText
    {
        private UIVapeButton button;
        private UIText bonusText;

        private bool isMinorStat;

        public string stat;
        public float statValue;

        public float bonusValue;

        public UIStatInfo(string stat, float width, float height, bool isMinorStat = false, bool button = true) : base(stat)
        {
            this.stat = stat;
            this.statValue = 0;
            this.bonusValue = 0;
            this.isMinorStat = isMinorStat;

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            if(button)
            {
                this.button = new UIVapeButton(ModLoader.GetTexture("VapeRPG/Textures/UI/AddButton"), ModLoader.GetTexture("VapeRPG/Textures/UI/AddButtonPressed"));
                this.button.Width.Set(15, 0);
                this.button.Height.Set(15, 0);
                this.button.Top.Set(0, 0);

                Action onClick;

                if (this.isMinorStat)
                {
                    this.button.Left.Set(width + this.button.Width.Pixels + 50, 0);
                    onClick = delegate ()
                    {
                        VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
                        if (vp.chaosPoints > 0)
                        {
                            float value = 0.02f;
                            if (this.stat.Contains("Crit"))
                            {
                                value = 1;
                            }
                            if(this.stat.Contains("Dodge"))
                            {
                                value = 0.005f;
                            }
                            vp.ChaosBonuses[this.stat] += value;
                            vp.chaosPoints--;
                        }
                    };
                }
                else
                {
                    this.button.Left.Set(width + this.button.Width.Pixels, 0);

                    onClick = delegate ()
                    {
                        VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
                        if (vp.statPoints > 0)
                        {
                            vp.BaseStats[this.stat]++;
                            vp.statPoints--;
                        }
                    };
                }
                this.button.OnClick += onClick;

                this.Append(this.button);
            }

            if(!this.isMinorStat)
            {
                this.bonusText = new UIText("+ 0");
                this.bonusText.Left.Set(this.button.Left.Pixels + this.button.Width.Pixels * 2, 0);
                this.bonusText.Top.Set(0, 0);
                this.bonusText.Width.Set(20, 0);
                this.bonusText.Height.Set(this.Height.Pixels, 0);
                this.bonusText.TextColor = Color.LimeGreen;

                this.Append(this.bonusText);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.isMinorStat)
            {
                this.SetText(String.Format("{0}: {1:0.00}%", this.stat, this.statValue));
            }
            else
            {
                this.SetText(String.Format("{0}: {1}", this.stat, this.statValue));

                if (this.bonusValue > 0)
                {
                    this.bonusText.SetText(String.Format("+ {0}", this.bonusValue));
                }
                else
                {
                    this.bonusText.SetText("");
                }
            }

            base.Update(gameTime);
        }
    }
}
