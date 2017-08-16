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
        private List<UIElement> bars;

        private UISkillTooltip tooltip;

        private SkillType skillTypes;

        public UISkillTab(string name, SkillType skillTypes)
        {
            this.name = name;
            this.skillTypes = skillTypes;

            this.SetPadding(10);

            this.skillInfos = new List<UISkillInfo>();
            this.bars = new List<UIElement>();
            this.tooltip = new UISkillTooltip(VapeRPG.GetSkill("Excitement"));
        }

        public void InitializeSkillInfos()
        {
            foreach (Skill skill in VapeRPG.Skills.FindAll(x => x.type == this.skillTypes))
            {
                UISkillInfo usi = new UISkillInfo(skill);
                this.AddSkillInfo(usi);
                TreeHelper.AddSkillInfo(usi);
            }

            foreach (var x in this.skillInfos)
            {
                if(x.skill.Prerequisites.Count > 0)
                {
                    foreach(Skill parent in x.skill.Prerequisites)
                    {
                        CreateLineBetweenSkills(this.skillInfos.Find(y => y.skill == parent), x);
                    }
                }
            }
        }

        private void CreateLineBetweenSkills(UISkillInfo parent, UISkillInfo child)
        {
            float childX = child.Left.Pixels;
            float childY = child.Top.Pixels;

            UISkillTreeBranch verticalBranch = new UISkillTreeBranch();
            verticalBranch.Left.Set(0, 0.5f);
            verticalBranch.Top.Set(0, 1f);
            verticalBranch.Width.Set(4, 0);
            verticalBranch.MaxHeight.Set(child.Top.Pixels - (parent.Top.Pixels + parent.Height.Pixels / 2), 0);
            verticalBranch.Height.Set(verticalBranch.MaxHeight.Pixels, 0);
            parent.Append(verticalBranch);

            bool underOccuppied = this.skillInfos.Find(x => x != parent && x.Left.Pixels == parent.Left.Pixels && x.Top.Pixels > parent.Top.Pixels) != null;

            if (childX != parent.Left.Pixels)
            {
                if (underOccuppied)
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch();
                    branch.Height.Set(4, 0);
                    branch.Top.Set(0, 0.5f);
                    branch.MaxWidth.Set(1000, 0);

                    if (childX > parent.Left.Pixels)
                    {
                        branch.Width.Set((child.Left.Pixels + child.Width.Pixels / 2) - (parent.Left.Pixels + parent.Width.Pixels), 0);
                        branch.Left.Set(0, 1f);
                        verticalBranch.Left.Set(branch.Width.Pixels, 1f);
                    }
                    else
                    {
                        branch.Width.Set(parent.Left.Pixels - (child.Left.Pixels + child.Width.Pixels / 2) + verticalBranch.Width.Pixels, 0);
                        branch.Left.Set(-branch.Width.Pixels, 0f);
                        verticalBranch.Left.Set(-branch.Width.Pixels, 0f);
                    }
                    verticalBranch.Top.Set(0, 0.5f);
                    parent.Append(branch);
                }
                else
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch();
                    branch.Height.Set(4, 0);
                    branch.Top.Set(verticalBranch.Height.Pixels, 1f);
                    branch.MaxWidth.Set(1000, 0);

                    if (childX > parent.Left.Pixels)
                    {
                        branch.Width.Set(child.Left.Pixels - (parent.Left.Pixels + parent.Width.Pixels / 2), 0);
                        branch.Left.Set(0, 0.5f);
                    }
                    else
                    {
                        branch.Width.Set((parent.Left.Pixels + parent.Width.Pixels / 2) - (child.Left.Pixels + child.Width.Pixels) + verticalBranch.Width.Pixels, 0);
                        branch.Left.Set(-branch.Width.Pixels + verticalBranch.Width.Pixels, 0.5f);
                    }
                    parent.Append(branch);
                }
            }
        }

        public void AddSkillInfo(UISkillInfo usi)
        {
            this.skillInfos.Add(usi);
            this.Append(usi);

            usi.OnMouseOver += UISkillInfo_OnMouseOver;
            usi.OnMouseOut += UISkillInfo_OnMouseOut;
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
