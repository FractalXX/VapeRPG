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
        private UIButton button;
        private UIText bonusText;
        private UIText statText;

        private bool isMinorStat;

        public string stat;
        public float statValue;

        public float bonusValue;

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
                this.button = new UIButton();
                this.button.Width.Set(30, 0);
                this.button.Height.Set(30, 0);
                this.button.Top.Set(0, 0);
                this.button.SetName("+");

                MouseEvent onMouseDown;

                this.button.Left.Set(-80, 1f);
                this.button.OnClick += (x, y) =>
                {
                    Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
                };

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
                                }
                                vp.ChaosBonuses[this.stat] += value;
                                vp.chaosPoints--;
                            }
                        }
                    };
                }
                else
                {
                    onMouseDown = (x, y) =>
                    {
                        VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
                        if (this.stat.Contains("Intellect") && vp.BaseStats["Intellect"] >= 300) return;
                        if (vp.statPoints > 0)
                        {
                            vp.BaseStats[this.stat]++;
                            vp.statPoints--;
                        }
                    };
                }
                this.button.OnClick += onMouseDown;

                this.Append(this.button);
                this.Append(this.statText);
            }

            if (!this.isMinorStat)
            {
                this.bonusText = new UIText("+ 0");
                this.bonusText.Left.Set(-35, 1f);
                this.bonusText.Height.Set(0, 1f);
                this.bonusText.TextColor = Color.LimeGreen;

                this.Append(this.bonusText);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.stat.Contains("Max Minions"))
            {
                this.statText.SetText(String.Format("{0}: {1}", this.stat, (int)this.statValue));
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
