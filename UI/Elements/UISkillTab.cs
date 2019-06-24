using System;
using System.Collections.Generic;
using Terraria.UI;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VapeRPG.UI.Elements
{
    class UISkillTab : UIElement
    {
        public string name;
        private List<UISkillInfo> skillInfos;

        private UISkillTooltip tooltip;

        private SkillTree skillTypes;

        public UISkillTab(string name, SkillTree skillTypes)
        {
            this.name = name;
            this.skillTypes = skillTypes;

            this.SetPadding(10);

            this.skillInfos = new List<UISkillInfo>();
        }

        public void AddSkillInfo(UISkillInfo skillInfo)
        {
            this.skillInfos.Add(skillInfo);
            this.Append(skillInfo);

            skillInfo.OnMouseOver += UISkillInfo_OnMouseOver;
            skillInfo.OnMouseOut += UISkillInfo_OnMouseOut;
        }

        private void CreateLineBetweenSkills(UISkillInfo parent, UISkillInfo child, List<UISkillInfo> skillInfos)
        {
            float childX = child.Left.Pixels;
            float childY = child.Top.Pixels;
            const int LINE_WIDTH = 4;

            UISkillTreeBranch verticalBranch = new UISkillTreeBranch();
            verticalBranch.Left.Set(parent.Left.Pixels + parent.Width.Pixels / 2 - LINE_WIDTH / 2, 0f);
            verticalBranch.Top.Set(parent.Top.Pixels + parent.Height.Pixels, 0);
            verticalBranch.Width.Set(LINE_WIDTH, 0);
            verticalBranch.MaxHeight.Set(child.Top.Pixels - (parent.Top.Pixels + parent.Height.Pixels / 2), 0);
            verticalBranch.Height.Set(verticalBranch.MaxHeight.Pixels, 0);
            this.Append(verticalBranch);

            bool underOccuppied = skillInfos.Find(x => x != parent && x.Left.Pixels == parent.Left.Pixels && x.Top.Pixels > parent.Top.Pixels) != null;

            if (childX != parent.Left.Pixels)
            {
                if (underOccuppied)
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch();
                    branch.Height.Set(4, 0);
                    branch.Top.Set(parent.Top.Pixels + parent.Height.Pixels / 2, 0);
                    branch.MaxWidth.Set(1000, 0);

                    if (childX > parent.Left.Pixels)
                    {
                        branch.Width.Set((child.Left.Pixels + child.Width.Pixels / 2) - (parent.Left.Pixels + parent.Width.Pixels), 0);
                        branch.Left.Set(parent.Left.Pixels + parent.Width.Pixels, 0f);
                        verticalBranch.Left.Set(parent.Left.Pixels + parent.Height.Pixels + branch.Width.Pixels, 0f);
                    }
                    else
                    {
                        branch.Width.Set(parent.Left.Pixels - (child.Left.Pixels + child.Width.Pixels / 2) + verticalBranch.Width.Pixels, 0);
                        branch.Left.Set(parent.Left.Pixels - branch.Width.Pixels, 0f);
                        verticalBranch.Left.Set(parent.Left.Pixels - branch.Width.Pixels, 0f);
                    }
                    verticalBranch.Top.Set(parent.Top.Pixels + parent.Height.Pixels / 2, 0);
                    this.Append(branch);
                }
                else
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch();
                    branch.Height.Set(4, 0);
                    branch.Top.Set(parent.Top.Pixels + parent.Height.Pixels + verticalBranch.Height.Pixels, 0);
                    branch.MaxWidth.Set(1000, 0);

                    if (childX > parent.Left.Pixels)
                    {
                        branch.Width.Set(child.Left.Pixels - (parent.Left.Pixels + parent.Width.Pixels / 2), 0);
                        branch.Left.Set(parent.Left.Pixels + parent.Width.Pixels / 2, 0);
                    }
                    else
                    {
                        branch.Width.Set((parent.Left.Pixels + parent.Width.Pixels / 2) - (child.Left.Pixels + child.Width.Pixels) + verticalBranch.Width.Pixels, 0);
                        branch.Left.Set(parent.Left.Pixels + parent.Width.Pixels / 2 - branch.Width.Pixels + verticalBranch.Width.Pixels, 0);
                    }
                    this.Append(branch);
                }
            }
        }

        public void InitializeSkillInfos()
        {
            // Temporary storage to ensure that the branch lines will be drawn under the icons
            List<UISkillInfo> skillInfos = new List<UISkillInfo>();
            foreach (Skill skill in VapeRPG.Skills.FindAll(x => x.Tree == this.skillTypes))
            {
                UISkillInfo skillInfo = new UISkillInfo(skill);
                skillInfos.Add(skillInfo);
                TreeHelper.AddSkillInfo(skillInfo);
            }

            foreach (var skillInfo in skillInfos)
            {
                var prerequisites = skillInfo.skill.GetPrerequisites();
                if(prerequisites.Count > 0)
                {
                    foreach(Type parentType in prerequisites)
                    {
                        CreateLineBetweenSkills(skillInfos.Find(y => y.skill.GetType() == parentType), skillInfo, skillInfos);
                    }
                }
            }

            foreach(var skillInfo in skillInfos)
            {
                this.AddSkillInfo(skillInfo);
            }
        }

        private void UISkillInfo_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            this.RemoveChild(this.tooltip);
        }

        private void UISkillInfo_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            UISkillInfo usi = listeningElement as UISkillInfo;
            this.tooltip = new UISkillTooltip(usi.skill);
            this.tooltip.Left.Set(usi.Left.Pixels + usi.Width.Pixels + 10, 0);
            this.tooltip.Top.Set(usi.Top.Pixels + usi.Height.Pixels + 10, 0);

            this.Append(this.tooltip);

            CalculatedStyle dimensions = this.tooltip.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);

            if (point1.Y + height > Main.screenHeight)
            {
                this.tooltip.Top.Set(usi.Top.Pixels - height - 10, 0);
            }
        }
    }
}
