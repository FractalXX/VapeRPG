using System;
using System.Text;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace VapeRPG.UI.Elements
{
    class UISkillTooltip : UIPanel
    {
        private UIText skillNameText;
        private UIText tooltipText;

        internal bool visible;

        public UISkillTooltip(Skill skill) : base()
        {
            this.visible = false;
            this.SetPadding(5);

            this.MaxWidth.Set(300, 0);
            this.MaxHeight.Set(200, 0);
            this.Width.Set(300, 0);
            this.Height.Set(200, 0);

            this.skillNameText = new UIText(skill.name);
            this.skillNameText.Left.Set(0, 0);
            this.skillNameText.Top.Set(0, 0);
            this.skillNameText.TextColor = Color.Green;

            this.tooltipText = new UIText(skill.description);
            this.tooltipText.Left.Set(0, 0);
            this.tooltipText.Top.Set(this.skillNameText.MinHeight.Pixels + 5, 0);
            this.tooltipText.TextColor = Color.White;
            this.tooltipText.MaxWidth.Set(this.Width.Pixels, 0);

            StringBuilder sb = new StringBuilder();
            int words = 0;
            for (int i = 0; i < this.tooltipText.Text.Length - 1; i++)
            {
                sb.Append(this.tooltipText.Text[i]);
                if(this.tooltipText.Text[i + 1].Equals(' '))
                {
                    words++;
                    if(words >= 6)
                    {
                        sb.AppendLine();
                        words = 0;
                    }
                }
            }
            sb.Append(this.tooltipText.Text[this.tooltipText.Text.Length - 1]);

            this.tooltipText.SetText(sb.ToString());

            this.BackgroundColor = Color.Gray;

            this.Append(this.skillNameText);
            this.Append(this.tooltipText);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                base.DrawSelf(spriteBatch);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.visible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
