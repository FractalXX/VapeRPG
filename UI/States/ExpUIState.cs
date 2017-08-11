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
    class ExpUIState : UIState
    {
        private UIVapeProgressBar xpBar;
        private UIVapeProgressBar chaosXpBar;
        private UIPanel levelPanel;
        private UIText levelText;

        public static bool visible = false;

        private const float width = 220;
        private const float height = 140;

        public override void OnInitialize()
        {
            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            this.levelPanel = new UIPanel();
            this.levelPanel.SetPadding(0);
            this.levelPanel.Left.Set(10, 0);
            this.levelPanel.Top.Set(Main.screenHeight - 80, 0);
            this.levelPanel.Width.Set(width, 0);
            this.levelPanel.Height.Set(height, 0);
            this.levelPanel.BackgroundColor = Color.SkyBlue;

            this.levelPanel.OnMouseDown += new MouseEvent(this.DragStart);
            this.levelPanel.OnMouseUp += new MouseEvent(this.DragEnd);

            this.xpBar = new UIVapeProgressBar(1, 0, 100, Color.Green, Color.Lime);
            this.xpBar.SetPadding(0);
            this.xpBar.Left.Set(10, 0);
            this.xpBar.Top.Set(this.levelPanel.Height.Pixels - 60, 0);
            this.xpBar.Width.Set(200, 0);
            this.xpBar.Height.Set(20, 0);
            this.xpBar.strokeThickness = 2;
            this.levelPanel.Append(this.xpBar);

            this.chaosXpBar = new UIVapeProgressBar(0, 0, 100, Color.Purple, Color.Violet);
            this.chaosXpBar.SetPadding(0);
            this.chaosXpBar.Left.Set(10, 0);
            this.chaosXpBar.Top.Set(this.levelPanel.Height.Pixels - 30, 0);
            this.chaosXpBar.Width.Set(200, 0);
            this.chaosXpBar.Height.Set(20, 0);
            this.chaosXpBar.strokeThickness = 2;
            this.levelPanel.Append(this.chaosXpBar);

            this.levelText = new UIText("Level: 1\nChaos rank: 0");
            this.levelText.Width.Set(this.levelPanel.Width.Pixels / 2, 0);
            this.levelText.Height.Set(5, 0f);
            this.levelText.Left.Set(this.levelPanel.Width.Pixels / 2 - this.levelText.Width.Pixels / 2, 0);
            this.levelText.Top.Set(20, 0);
            this.levelPanel.Append(this.levelText);

            base.Append(this.levelPanel);
        }

        public void UpdateXpBar(float value, float minValue, float maxValue)
        {
            this.xpBar.value = value;
            this.xpBar.minValue = minValue;
            this.xpBar.maxValue = maxValue;
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

        private Vector2 offset;
        public bool dragging = false;

        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - this.levelPanel.Left.Pixels, evt.MousePosition.Y - this.levelPanel.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            this.levelPanel.Left.Set(end.X - offset.X, 0f);
            this.levelPanel.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (this.levelPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                this.levelPanel.Left.Set(MousePosition.X - offset.X, 0f);
                this.levelPanel.Top.Set(MousePosition.Y - offset.Y, 0f);
            }

            this.Recalculate();

            base.DrawSelf(spriteBatch);
        }

        public void SetPanelPosition(Vector2 position)
        {
            this.levelPanel.Left.Set(position.X, 0);
            this.levelPanel.Top.Set(position.Y, 0);

            this.Recalculate();
        }

        public Vector2 GetPanelPosition()
        {
            return new Vector2(this.levelPanel.Left.Pixels, this.levelPanel.Top.Pixels);
        }
    }
}
