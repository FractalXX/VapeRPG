using System;
using Terraria;
using System.Text;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace VapeRPG.UI.Elements
{
    class UISkillTooltip : UIPanel
    {
        private UITextPanel<string> skillNameText;
        private UITextPanel<string> tooltipText;
        private UITextPanel<string> requirementsText;

        public UISkillTooltip(Skill skill)
        {
            this.SetPadding(5);

            this.MaxWidth.Set(400, 0);
            this.MaxHeight.Set(400, 0);
            this.Width.Set(300, 0);
            this.Height.Set(400, 0);

            this.skillNameText = new UITextPanel<string>(skill.Name);
            this.skillNameText.Left.Set(0, 0);
            this.skillNameText.Top.Set(0, 0);
            this.skillNameText.Width.Set(0, 1f);
            this.skillNameText.TextColor = Color.Lime;
            this.skillNameText.BackgroundColor = new Color(80, 128, 163);

            var prerequisites = skill.GetPrerequisites();
            string[] requirements = new string[prerequisites.Count];

            StringBuilder sb = new StringBuilder();
            sb.Append("Requires: ");
            if (prerequisites.Count > 0)
            {
                for (int i = 0; i < prerequisites.Count; i++)
                {
                    requirements[i] = VapeRPG.GetSkill(prerequisites[i]).Name;
                    if (i > 0)
                    {
                        sb.Append(" and ");
                    }
                    sb.Append(requirements[i]);

                }
            }
            else
            {
                sb.Append("Nothing");
            }

            this.tooltipText = new UITextPanel<string>(skill.Description, 0.8f);
            this.tooltipText.Top.Set(this.skillNameText.MinHeight.Pixels, 0);
            this.tooltipText.Width.Set(0, 1f);
            this.tooltipText.MaxWidth.Set(0, 1f);
            this.tooltipText.MaxHeight.Set(0, 0.8f);
            this.tooltipText.Height.Set(this.Height.Pixels - this.skillNameText.MinHeight.Pixels, 0);
            this.tooltipText.BackgroundColor = new Color(60, 78, 143);

            this.requirementsText = new UITextPanel<string>(sb.ToString(), 0.8f);
            this.requirementsText.Width.Set(0, 1f);
            this.requirementsText.TextColor = Color.Yellow;
            this.requirementsText.BackgroundColor = new Color(100, 118, 163);

            sb.Clear();

            string line = "";
            int lines = 1;
            for (int i = 0; i < this.tooltipText.Text.Length; i++)
            {
                line += this.tooltipText.Text[i];

                if (Main.fontMouseText.MeasureString(line).X >= this.MaxWidth.Pixels - 60 && this.tooltipText.Text[i] == ' ')
                {
                    sb.AppendLine(line);
                    line = "";
                    lines++;
                }
                else if(i == this.tooltipText.Text.Length - 1)
                {
                    sb.AppendLine(line);
                }
            }

            string text = sb.ToString();

            this.tooltipText.Height.Set((16f * 0.8f + Main.fontMouseText.LineSpacing * 0.8f) * lines, 0);
            this.tooltipText.SetText(text);
            this.Height.Set(this.skillNameText.MinHeight.Pixels + this.tooltipText.Height.Pixels + this.requirementsText.MinHeight.Pixels + 10, 0);
            this.Width.Set(Main.fontMouseText.MeasureString(text).X, 0);

            this.requirementsText.Top.Set(-this.requirementsText.MinHeight.Pixels, 1f);

            this.BackgroundColor = new Color(40, 58, 123);

            this.Append(this.skillNameText);
            this.Append(this.tooltipText);
            this.Append(this.requirementsText);
            this.Recalculate();
        }
    }
}
