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
    class UIStatInfo : UIElement
    {
        public string stat;
        public float statValue;

        public float bonusValue;

        private UIImageButton button;
        private UIText bonusText;
        private UIText statText;

        private bool isMinorStat;

        public UIStatInfo(string stat, float width, float height, bool isMinorStat = false, bool button = true, float textScale = 1f)
        {
            this.stat = stat;
            this.statValue = 0;
            this.bonusValue = 0;
            this.isMinorStat = isMinorStat;

            this.statText = new UIText(stat, textScale);
            this.statText.Left.Set(0, 0);
            this.statText.Top.Set(0, 0);

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            if (button)
            {
                this.button = new UIImageButton(ModContent.GetTexture("VapeRPG/Textures/UI/Button/AddButton"));
                this.button.Width.Set(30, 0);
                this.button.Height.Set(30, 0);
                this.button.Top.Set(0, 0);

                MouseEvent onMouseDown;

                if (this.isMinorStat)
                {
                    onMouseDown = (x, y) =>
                    {
                        VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
                        if (this.stat.Contains("Max Minions"))
                        {
                            if (vp.chaosPoints >= 5)
                            {
                                vp.ChaosBonuses[this.stat] += 1;
                                vp.chaosPoints -= 5;
                            }
                        }
                        else
                        {
                            if (vp.chaosPoints > 0)
                            {
                                float value = 0.02f;
                                if (this.stat.Contains("Crit"))
                                {
                                    value = 1;
                                }
                                if (this.stat.Contains("Dodge"))
                                {
                                    value = 0.005f;
                                    if(vp.dodgeChance >= 0.7f)
                                    {
                                        return;
                                    }
                                }
                                vp.ChaosBonuses[this.stat] += value;
                                vp.chaosPoints--;
                            }
                        }
                    };
                    this.button.Left.Set(-50, 1f);
                }
                else
                {
                    onMouseDown = (x, y) =>
                    {
                        VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
                        if (vp.statPoints > 0)
                        {
                            vp.BaseStats[this.stat]++;
                            vp.statPoints--;
                        }
                    };
                    this.button.Left.Set(-80, 1f);
                }
                this.button.OnClick += onMouseDown;

                this.Append(this.button);
            }

            if (!this.isMinorStat)
            {
                this.bonusText = new UIText("+ 0");
                this.bonusText.Left.Set(-50, 1f);
                this.bonusText.Height.Set(0, 1f);
                this.bonusText.TextColor = Color.LimeGreen;

                this.Append(this.bonusText);
            }

            this.Append(this.statText);
        }

        public Color TextColor
        {
            get
            {
                return this.statText.TextColor;
            }
            set
            {
                this.statText.TextColor = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.stat.Contains("Max Minions"))
            {
                this.statText.SetText(String.Format("{0}: {1}", this.stat, (int)this.statValue));
            }
            else if(this.stat.Contains("Max Run Speed"))
            {
                this.statText.SetText(String.Format("{0}: {1:0.00}", this.stat, this.statValue));
            }
            else if (this.isMinorStat)
            {
                this.statText.SetText(String.Format("{0}: {1:0.00}%", this.stat, this.statValue));
            }
            else
            {
                this.statText.SetText(String.Format("{0}: {1}", this.stat, this.statValue));

                if (this.bonusValue > 0)
                {
                    this.bonusText.SetText(String.Format("+{0}", this.bonusValue));
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
