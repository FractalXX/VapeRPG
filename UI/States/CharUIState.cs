using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    class CharUIState : UIState
    {
        public static bool visible = false;

        public bool dragging = false;

        private UIPanel mainPanel;
        private UIPanel statPanel;
        private UIPanel miscPanel;
        private VapeSkillPanel skillPanel;

        private UIStatInfo[] statControls;
        private UIStatInfo[] miscStatControls;
        private UIText pointsText;
        private UIText chaosPointsText;

        private UIImage statHelper;

        private float charPanelWidth;
        private float charPanelHeight;

        private Vector2 offset;

        public override void OnInitialize()
        {
            this.statControls = new UIStatInfo[VapeRPG.BaseStats.Length];
            this.miscStatControls = new UIStatInfo[VapeRPG.MinorStats.Length];

            this.charPanelWidth = 800;
            this.charPanelHeight = 600;
            float mainPanelPadding = 10;

            this.mainPanel = new UIPanel();
            this.mainPanel.SetPadding(mainPanelPadding);
            this.mainPanel.Width.Set(this.charPanelWidth, 0);
            this.mainPanel.Height.Set(this.charPanelHeight, 0);
            this.mainPanel.Left.Set(Main.screenWidth / 2 - this.charPanelWidth / 2, 0);
            this.mainPanel.Top.Set(Main.screenHeight / 2 - this.charPanelHeight / 2, 0);
            mainPanel.BackgroundColor = new Color(20, 38, 103);

            this.mainPanel.OnMouseDown += new MouseEvent(this.DragStart);
            this.mainPanel.OnMouseUp += new MouseEvent(this.DragEnd);

            this.skillPanel = new VapeSkillPanel(2 * (this.mainPanel.Width.Pixels - 2 * mainPanelPadding) / 3, this.mainPanel.Height.Pixels - 2 * mainPanelPadding);
            this.skillPanel.SetPadding(0);
            this.skillPanel.Left.Set(0, 0);
            this.skillPanel.Top.Set(0, 0);

            this.statPanel = new UIPanel();
            this.statPanel.SetPadding(10);
            this.statPanel.Left.Set(2 * (this.mainPanel.Width.Pixels - 2 * mainPanelPadding) / 3, 0);
            this.statPanel.Top.Set(0, 0);
            this.statPanel.Width.Set((this.mainPanel.Width.Pixels - 2 * mainPanelPadding) / 3, 0);
            this.statPanel.Height.Set((this.mainPanel.Height.Pixels - 2 * mainPanelPadding) / 2, 0);
            this.statPanel.BorderColor = Color.Black;
            this.statPanel.BackgroundColor = new Color(100, 118, 183);

            #region statPanel texts

            for (int i = 0; i < statControls.Length; i++)
            {
                this.statControls[i] = new UIStatInfo(VapeRPG.BaseStats[i], this.statPanel.Width.Pixels, 20);
                this.statControls[i].Top.Set(30 + 1.2f * i * this.statControls[i].Height.Pixels + 5, 0);
                this.statControls[i].TextColor = Color.Yellow;
                this.statPanel.Append(this.statControls[i]);
            }

            this.statHelper = new UIImage(ModLoader.GetTexture("VapeRPG/Textures/UI/Button/HelpButton"));
            this.statHelper.Width.Set(30, 0);
            this.statHelper.Height.Set(30, 0);
            this.statHelper.Left.Set(-30, 1f);
            this.statHelper.Top.Set(0, 0);
            this.statHelper.OnMouseOver += (x, y) => StatHelpUIState.visible = true;
            this.statHelper.OnMouseOut += (x, y) => StatHelpUIState.visible = false;

            this.statPanel.Append(this.statHelper);

            this.pointsText = new UIText("Stat points: 0\nSkill points: 0", 0.8f);
            this.pointsText.Width.Set(this.statPanel.Width.Pixels / 3, 0);
            this.pointsText.HAlign = 0.5f;
            this.pointsText.Top.Set(-this.pointsText.MinHeight.Pixels * 2 - 10, 1f);
            this.statPanel.Append(this.pointsText);
            #endregion

            this.miscPanel = new UIPanel();
            this.miscPanel.SetPadding(10);
            this.miscPanel.Left.Set(2 * (this.mainPanel.Width.Pixels - 2 * mainPanelPadding) / 3, 0);
            this.miscPanel.Top.Set((this.mainPanel.Height.Pixels - 2 * mainPanelPadding) / 2, 0);
            this.miscPanel.Width.Set((this.mainPanel.Width.Pixels - 2 * mainPanelPadding) / 3, 0);
            this.miscPanel.Height.Set((this.mainPanel.Height.Pixels - 2 * mainPanelPadding) / 2, 0);
            this.miscPanel.BorderColor = Color.Black;
            this.miscPanel.BackgroundColor = new Color(100, 118, 183);

            this.chaosPointsText = new UIText("Chaos points: 0");
            this.chaosPointsText.Top.Set(-10, 1f);
            this.chaosPointsText.HAlign = 0.5f;
            this.chaosPointsText.TextColor = Color.Violet;
            this.miscPanel.Append(this.chaosPointsText);

            #region miscPanel texts

            for (int i = 0; i < this.miscStatControls.Length; i++)
            {
                this.miscStatControls[i] = new UIStatInfo(VapeRPG.MinorStats[i], this.miscPanel.Width.Pixels, 20, true, !VapeRPG.MinorStats[i].Contains("Block Chance"), 0.8f);
                this.miscStatControls[i].Top.Set(i * this.miscStatControls[i].Height.Pixels + 5, 0);

                this.miscPanel.Append(this.miscStatControls[i]);
            }

            #endregion

            this.mainPanel.Append(this.statPanel);
            this.mainPanel.Append(this.miscPanel);
            this.mainPanel.Append(this.skillPanel);

            base.Append(this.mainPanel);
        }

        public void UpdateBonusPanel(int chaosPoints, float meleeDamage, float magicDamage, float rangedDamage, int meleeCrit, int magicCrit, int rangedCrit, float meleeSpeed, float moveSpeed, float dodgeChance, float blockChance, int maxMinions, float minionDamage)
        {
            foreach (UIStatInfo usi in miscStatControls)
            {
                if (usi.stat.Contains("Melee Damage"))
                {
                    usi.statValue = meleeDamage * 100;
                    usi.TextColor = Color.Red;
                }
                if (usi.stat.Contains("Ranged Damage"))
                {
                    usi.statValue = rangedDamage * 100;
                    usi.TextColor = Color.Orange;
                }
                if (usi.stat.Contains("Magic Damage"))
                {
                    usi.statValue = magicDamage * 100;
                    usi.TextColor = Color.Cyan;
                }

                if (usi.stat.Contains("Melee Crit"))
                {
                    usi.statValue = meleeCrit;
                    usi.TextColor = Color.Red;
                }
                if (usi.stat.Contains("Ranged Crit"))
                {
                    usi.statValue = rangedCrit;
                    usi.TextColor = Color.Orange;
                }
                if (usi.stat.Contains("Magic Crit"))
                {
                    usi.statValue = magicCrit;
                    usi.TextColor = Color.Cyan;
                }

                if (usi.stat.Contains("Minion Damage"))
                {
                    usi.statValue = minionDamage * 100;
                }

                if (usi.stat.Contains("Max Minions"))
                {
                    usi.statValue = maxMinions;
                }

                if (usi.stat.Contains("Melee Speed"))
                {
                    usi.statValue = meleeSpeed * 100;
                    usi.TextColor = Color.Red;
                }
                if (usi.stat.Contains("Max Run Speed"))
                {
                    usi.statValue = moveSpeed;
                    usi.TextColor = Color.LimeGreen;
                }
                if (usi.stat.Contains("Dodge Chance"))
                {
                    usi.statValue = dodgeChance * 100;
                    usi.TextColor = Color.LimeGreen;
                }
                if (usi.stat.Contains("Block Chance"))
                {
                    usi.statValue = blockChance * 100;
                    usi.TextColor = Color.LimeGreen;
                }

                this.chaosPointsText.SetText(String.Format("Chaos points: {0}", chaosPoints));
            }
        }

        public void UpdateStats(Dictionary<string, int> baseStats, Dictionary<string, int> effStats, int statPoints, int skillPoints)
        {
            foreach (UIStatInfo usi in statControls)
            {
                usi.statValue = baseStats[usi.stat];
                usi.bonusValue = effStats[usi.stat] - baseStats[usi.stat];
            }

            this.pointsText.SetText(String.Format("Stat points: {0}\nSkill points: {1}", statPoints, skillPoints));
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            dragging = false;

            this.mainPanel.Left.Set(end.X - offset.X, 0f);
            this.mainPanel.Top.Set(end.Y - offset.Y, 0f);

            this.Recalculate();
        }


        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - this.mainPanel.Left.Pixels, evt.MousePosition.Y - this.mainPanel.Top.Pixels);
            dragging = true;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (this.mainPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                this.mainPanel.Left.Set(MousePosition.X - offset.X, 0f);
                this.mainPanel.Top.Set(MousePosition.Y - offset.Y, 0f);
            }

            this.Recalculate();

            base.DrawSelf(spriteBatch);
        }
    }
}
