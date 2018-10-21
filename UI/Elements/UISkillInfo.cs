using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UISkillInfo : UIElement
    {
        private const float defaultWidth = 64;
        private const float defaultHeight = 64;

        internal Skill skill;

        private UIImage icon;
        private UIText skillLevelText;

        private Texture2D skillShade;

        public UISkillInfo(Skill skill, float width = defaultWidth, float height = defaultHeight)
        {
            this.skillShade = ModLoader.GetTexture("VapeRPG/Textures/UI/Skills/SkillShade");

            this.skill = skill;

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            this.icon = new UIImage(this.skill.icon);
            this.icon.SetPadding(0);
            this.icon.Width.Set(width, 0);
            this.icon.Height.Set(height, 0);
            this.icon.Left.Set(0, 0);
            this.icon.Top.Set(0, 0);

            this.skillLevelText = new UIText("0/0");
            this.skillLevelText.Left.Set(this.Width.Pixels - this.skillLevelText.MinWidth.Pixels, 0);
            this.skillLevelText.Top.Set(this.Height.Pixels - this.skillLevelText.MinHeight.Pixels, 0);

            this.icon.OnClick += (x, y) =>
            {
                VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();

                if (vp.HasPrerequisiteForSkill(this.skill) && vp.skillPoints > 0 && vp.SkillLevels[this.skill.name] < this.skill.maxLevel)
                {
                    vp.SkillLevels[this.skill.name]++;
                    if(vp.player.name != "vp") vp.skillPoints--;
                }
            };
            
            this.Append(this.icon);
            this.Append(this.skillLevelText);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            VapePlayer vp = Main.LocalPlayer.GetModPlayer<VapePlayer>();
            if (this.skill.Prerequisites.Count > 0 && !vp.HasPrerequisiteForSkill(this.skill))
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                int width = (int)Math.Ceiling(dimensions.Width);
                int height = (int)Math.Ceiling(dimensions.Height);
                spriteBatch.Draw(this.skillShade, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            VapePlayer vp = Main.LocalPlayer.GetModPlayer<VapePlayer>();
            this.skillLevelText.SetText(String.Format("{0}/{1}", vp.SkillLevels[this.skill.name], this.skill.maxLevel));
        }
    }
}
