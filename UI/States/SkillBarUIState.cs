using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    internal class SkillBarUIState : DraggableUI
    {
        public static bool visible = true;
        public const int SKILL_SLOT_COUNT = 4;

        public override Vector2 DefaultPosition => new Vector2(Main.screenWidth - this.DefaultSize.X, Main.screenHeight - this.DefaultSize.Y);
        public override Vector2 DefaultSize => new Vector2(200, 60);

        public UISkillScrollSlot[] SkillSlots { get; private set; }
        private UIText[] hotkeyTexts;

        protected override UIElement CreateContainer()
        {
            return new UIElement();
        }

        protected override void Construct()
        {
            this.SkillSlots = new UISkillScrollSlot[SKILL_SLOT_COUNT];
            this.hotkeyTexts = new UIText[SKILL_SLOT_COUNT];
            float slotWidth = this.DefaultSize.X / SKILL_SLOT_COUNT;
            for (int i = 0; i < SKILL_SLOT_COUNT; i++)
            {
                this.SkillSlots[i] = new UISkillScrollSlot();
                this.SkillSlots[i].Left.Set(i * slotWidth, 0);
                this.SkillSlots[i].Width.Set(slotWidth, 0);
                this.SkillSlots[i].Height.Set(this.DefaultSize.Y, 0);

                this.hotkeyTexts[i] = new UIText("");
                this.hotkeyTexts[i].HAlign = 0.9f;
                this.hotkeyTexts[i].TextColor = Color.Yellow;
                this.SkillSlots[i].Append(this.hotkeyTexts[i]);

                this.container.Append(this.SkillSlots[i]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < SKILL_SLOT_COUNT; i++)
            {
                string keyText = "";
                if (VapeRPG.SkillHotKeys[i].GetAssignedKeys().Count > 0)
                {
                    keyText = VapeRPG.SkillHotKeys[i].GetAssignedKeys()[0];
                }
                this.hotkeyTexts[i].SetText(keyText);
            }
            base.Update(gameTime);
        }
    }
}
