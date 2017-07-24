using System;
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
        private Skill skill;

        private UIVapeProgressBar levelBar;
        private UIVapeButton button;
        private UIImage icon;

        private Color skillColor;

        public UISkillInfo(Skill skill, float width, float height)
        {
            this.skill = skill;

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            switch (this.skill.skillType)
            {
                case SkillType.Melee:
                    this.skillColor = Color.Red;
                    break;

                case SkillType.Magic:
                    this.skillColor = Color.Cyan;
                    break;

                case SkillType.Ranged:
                    this.skillColor = Color.Orange;
                    break;

                case SkillType.Tank:
                    this.skillColor = Color.RosyBrown;
                    break;

                case SkillType.Utility:
                    this.skillColor = Color.LimeGreen;
                    break;

                default:
                    this.skillColor = Color.SkyBlue;
                    break;
            }

            this.icon = new UIImage(this.skill.icon);
            this.icon.SetPadding(0);
            this.icon.Width.Set(width, 0);
            this.icon.Height.Set(height - 30, 0);
            this.icon.Left.Set(0, 0);
            this.icon.Top.Set(0, 0);

            this.levelBar = new UIVapeProgressBar(0, 0, this.skill.maxLevel, Color.Gray, Color.LimeGreen);
            this.levelBar.SetPadding(0);
            this.levelBar.strokeThickness = 1;
            this.levelBar.Width.Set(width - 30, 0);
            this.levelBar.Height.Set(20, 0);
            this.levelBar.Left.Set(0, 0);
            this.levelBar.Top.Set(height - this.levelBar.Height.Pixels, 0);

            this.button = new UIVapeButton(ModLoader.GetTexture("VapeRPG/Textures/UI/AddButton"), ModLoader.GetTexture("VapeRPG/Textures/UI/AddButtonPressed"));
            this.button.SetPadding(0);
            this.button.Width.Set(20, 0);
            this.button.Height.Set(this.levelBar.Height.Pixels, 0);
            this.button.Left.Set(width - this.button.Width.Pixels, 0);
            this.button.Top.Set(height - this.button.Height.Pixels, 0);

            this.button.OnClick += delegate ()
            {
                VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();

                if (vp.skillPoints > 0 && vp.SkillLevels[this.skill.name] < this.skill.maxLevel)
                {
                    vp.SkillLevels[this.skill.name]++;
                    vp.skillPoints--;
                }
            };

            this.Append(this.levelBar);
            this.Append(this.button);
            this.Append(this.icon);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            this.DrawTooltip(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            VapePlayer vp = Main.player[Main.myPlayer].GetModPlayer<VapePlayer>();
            this.levelBar.value = vp.SkillLevels[this.skill.name];
            this.levelBar.maxValue = this.skill.maxLevel;

            base.Update(gameTime);
        }

        private void DrawTooltip(SpriteBatch spriteBatch)
        {
            MouseState ms = Mouse.GetState();
            Vector2 mousePosition = new Vector2(ms.X, ms.Y);

            if (this.icon.ContainsPoint(mousePosition))
            {
                Utils.DrawBorderString(spriteBatch, skill.name, new Vector2(Main.screenWidth / 2 - 200, 100), this.skillColor, 1.7f);
                Utils.DrawBorderString(spriteBatch, skill.description, new Vector2(Main.screenWidth / 2 - 200, 150), Color.White, 1.3f);
            }
        }
    }
}
