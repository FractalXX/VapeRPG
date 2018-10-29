using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    class ExpUIState : DraggableUI
    {
        public static bool visible = false;

        private bool minimized = false;

        private UIVapeProgressBar xpBar;
        private UIVapeProgressBar chaosXpBar;
        private UIImageButton minimizeButton;
        private UIText levelText;

        public override Vector2 DefaultPosition => new Vector2(10, Main.screenHeight - 160);
        public override Vector2 DefaultSize => new Vector2(220, 140);

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!this.minimized)
            {
                base.Draw(spriteBatch);
            }
            else
            {
                this.minimizeButton.Draw(spriteBatch);
            }
        }

        public void SetPanelPosition(Vector2 position)
        {
            this.container.Left.Set(position.X, 0);
            this.container.Top.Set(position.Y, 0);

            this.Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (this.container.ContainsPoint(MousePosition) && !this.minimized || this.minimizeButton.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            base.Update(gameTime);
        }

        public void UpdateChaosXpBar(float value, float minValue, float maxValue)
        {
            this.chaosXpBar.value = value;
            this.chaosXpBar.minValue = minValue;
            this.chaosXpBar.maxValue = maxValue;
        }

        public void UpdateLevel(int newLevel, int newChaosRank)
        {
            this.levelText.SetText(String.Format("Level: {0}\nChaos rank: {1}", newLevel, newChaosRank));
        }

        public void UpdateXpBar(float value, float minValue, float maxValue)
        {
            this.xpBar.value = value;
            this.xpBar.minValue = minValue;
            this.xpBar.maxValue = maxValue;
        }

        protected override void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (this.container.ContainsPoint(evt.MousePosition) && !this.minimized || this.minimizeButton.ContainsPoint(evt.MousePosition))
            {
                base.DragStart(evt, listeningElement);
            }
        }

        protected override UIElement CreateContainer()
        {
            UIPanel container = new UIPanel();
            container.SetPadding(0);
            container.BackgroundColor = Color.SkyBlue;
            return container;
        }

        protected override void Construct()
        {
            this.xpBar = new UIVapeProgressBar(1, 0, 100, Color.Green, Color.Lime);
            this.xpBar.SetPadding(0);
            this.xpBar.Left.Set(10, 0);
            this.xpBar.Top.Set(this.container.Height.Pixels - 60, 0);
            this.xpBar.Width.Set(200, 0);
            this.xpBar.Height.Set(20, 0);
            this.xpBar.strokeThickness = 2;
            this.container.Append(this.xpBar);

            this.chaosXpBar = new UIVapeProgressBar(0, 0, 100, Color.Purple, Color.Violet);
            this.chaosXpBar.SetPadding(0);
            this.chaosXpBar.Left.Set(10, 0);
            this.chaosXpBar.Top.Set(this.container.Height.Pixels - 30, 0);
            this.chaosXpBar.Width.Set(200, 0);
            this.chaosXpBar.Height.Set(20, 0);
            this.chaosXpBar.strokeThickness = 2;
            this.container.Append(this.chaosXpBar);

            this.levelText = new UIText("Level: 1\nChaos rank: 0");
            this.levelText.Width.Set(this.container.Width.Pixels / 2, 0);
            this.levelText.Height.Set(5, 0f);
            this.levelText.Left.Set(this.container.Width.Pixels / 2 - this.levelText.Width.Pixels / 2, 0);
            this.levelText.Top.Set(20, 0);
            this.container.Append(this.levelText);

            this.minimizeButton = new UIImageButton(ModLoader.GetTexture("VapeRPG/Textures/UI/Button/MinimizeButton"));
            this.minimizeButton.Width.Set(50, 0f);
            this.minimizeButton.Height.Set(15, 0f);
            this.minimizeButton.HAlign = 0.5f;
            this.minimizeButton.Top.Set(0, 0f);

            this.minimizeButton.OnMouseUp += (evt, element) =>
            {
                this.minimized = !this.minimized;
                this.minimizeButton.SetImage(ModLoader.GetTexture("VapeRPG/Textures/UI/Button/" + (this.minimized ? "MinimizeButtonFlipped" : "MinimizeButton")));
            };

            this.container.Append(this.minimizeButton);
        }
    }
}
