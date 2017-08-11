using System;
using System.Collections.Generic;
using Terraria.UI;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UISkillTab : UIElement
    {
        public bool visible;
        public string name;
        private List<UISkillInfo> skillInfos;
        private List<UIElement> bars;

        private UISkillTooltip tooltip;

        private SkillType skillTypes;

        public UISkillTab(string name, SkillType skillTypes)
        {
            this.visible = false;
            this.name = name;
            this.skillTypes = skillTypes;

            this.skillInfos = new List<UISkillInfo>();
            this.bars = new List<UIElement>();
            this.tooltip = new UISkillTooltip(VapeRPG.GetSkill("Excitement"));
        }

        public void InitializeSkillInfos()
        {
            UISkillInfo usi = null;

            if (this.skillTypes == SkillType.OnKill)
            {
                usi = new UISkillInfo(VapeRPG.GetSkill("Excitement"));
                usi.Left.Set(this.Width.Pixels / 2 - usi.Width.Pixels / 2, 0);
                usi.Top.Set(20, 0);
            }
            else if (this.skillTypes == SkillType.OnHit)
            {
                usi = new UISkillInfo(VapeRPG.GetSkill("X-Ray Hits"));
                usi.Left.Set(this.Width.Pixels / 2 - usi.Width.Pixels / 2, 0);
                usi.Top.Set(20, 0);
            }

            this.AddSkillInfo(usi);

            foreach (Skill skill in VapeRPG.Skills.FindAll(x => x.type == this.skillTypes))
            {
                foreach (Skill child in skill.Children)
                {
                    usi = new UISkillInfo(child);

                    UISkillInfo firstParent = this.skillInfos.Find(x => x.skill == child.Prerequisites[0]);

                    if (firstParent != null)
                    {
                        float childX = firstParent.Left.Pixels;
                        float childY = firstParent.Top.Pixels + 94f;

                        if (this.skillInfos.Find(x => x.Left.Pixels == childX && x.Top.Pixels == childY) != null)
                        {
                            childX += 104;
                        }

                        usi.Left.Set(childX, 0);
                        usi.Top.Set(childY, 0);

                        CreateLineBetweenSkills(firstParent, usi);

                        if (child.Prerequisites.Count > 1)
                        {
                            UISkillInfo secondParent = this.skillInfos.Find(x => x.skill == child.Prerequisites[1]);
                            CreateLineBetweenSkills(secondParent, usi);
                        }

                        this.AddSkillInfo(usi);
                    }
                }
            }
        }

        private void CreateLineBetweenSkills(UISkillInfo parent, UISkillInfo child)
        {
            float childX = child.Left.Pixels;
            float childY = child.Top.Pixels;

            UISkillTreeBranch verticalBranch = new UISkillTreeBranch(BranchDirection.Vertical);
            verticalBranch.Left.Set(0, 0.5f);
            verticalBranch.Top.Set(0, 1f);
            verticalBranch.Width.Set(4, 0);
            verticalBranch.MaxHeight.Set(child.Top.Pixels - (parent.Top.Pixels + parent.Height.Pixels / 2), 0);
            verticalBranch.Height.Set(verticalBranch.MaxHeight.Pixels, 0);
            parent.Append(verticalBranch);

            bool underOccuppied = this.skillInfos.Find(x => x != parent && x.Left.Pixels == parent.Left.Pixels && x.Top.Pixels > parent.Top.Pixels) != null;

            if(childX != parent.Left.Pixels)
            {
                if (underOccuppied)
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch(BranchDirection.Horizontal);
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
                        branch.Width.Set((parent.Left.Pixels + parent.Width.Pixels / 2) - (child.Left.Pixels + child.Width.Pixels) + verticalBranch.Width.Pixels, 0);
                        branch.Left.Set(-branch.Width.Pixels + verticalBranch.Width.Pixels, 0.5f);
                        verticalBranch.Left.Set(-branch.Width.Pixels, 0f);
                    }
                    verticalBranch.Top.Set(0, 0.5f);
                    parent.Append(branch);
                }
                else
                {
                    UISkillTreeBranch branch = new UISkillTreeBranch(BranchDirection.Horizontal);
                    branch.Height.Set(4, 0);
                    branch.Top.Set(verticalBranch.Height.Pixels, 1f);
                    branch.MaxWidth.Set(1000, 0);

                    if (childX > parent.Left.Pixels)
                    {
                        branch.Width.Set((child.Left.Pixels + child.Width.Pixels / 2) - (parent.Left.Pixels + parent.Width.Pixels), 0);
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

        public void AddSkillInfo(UISkillInfo usi)
        {
            this.skillInfos.Add(usi);
            this.Append(usi);

            usi.OnMouseOver += UISkillInfo_OnMouseOver;
            usi.OnMouseOut += UISkillInfo_OnMouseOut;
        }

        private void UISkillInfo_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            this.tooltip.visible = false;
            this.RemoveChild(this.tooltip);
        }

        private void UISkillInfo_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            UISkillInfo usi = listeningElement as UISkillInfo;
            this.tooltip = new UISkillTooltip(usi.skill);
            this.tooltip.Left.Set(usi.Left.Pixels + usi.Width.Pixels + 10, 0);
            this.tooltip.Top.Set(usi.Top.Pixels + usi.Height.Pixels + 10, 0);

            if (this.tooltip.Top.Pixels + this.tooltip.Height.Pixels > Main.screenHeight)
            {
                this.tooltip.Top.Set(usi.Top.Pixels + 10, 0);
            }

            this.tooltip.visible = true;
            this.Append(this.tooltip);
        }
    }
}
