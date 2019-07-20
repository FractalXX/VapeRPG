using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using VapeRPG.UI.Elements;
using VapeRPG.Util;

namespace VapeRPG.UI.States
{
    class CharUIState : DraggableUI
    {
        public static bool visible = false;
        private const int PANEL_PADDING = 10;

        private UIPanel statPanel;
        private UIPanel miscPanel;
        private UISkillPanel skillPanel;

        private UIStatInfo[] miscStatControls;
        private UIText pointsText;

        private UIRelativeProgressBar xpBar;
        private UIText levelText;

        public override Vector2 DefaultPosition => new Vector2(Main.screenWidth / 2 - this.DefaultSize.X / 2, Main.screenHeight / 2 - this.DefaultSize.Y / 2);
        public override Vector2 DefaultSize => new Vector2(800, 600);

        public void UpdateBonusPanel(float meleeDamage, float magicDamage, float rangedDamage, int meleeCrit, int magicCrit, int rangedCrit, float meleeSpeed, float moveSpeed, float dodgeChance, float blockChance, int maxMinions, float minionDamage)
        {
            foreach (UIStatInfo info in this.miscStatControls)
            {
                if (info.stat.Contains("Melee Damage"))
                {
                    info.statValue = meleeDamage * 100;
                    info.TextColor = Color.Red;
                }
                if (info.stat.Contains("Ranged Damage"))
                {
                    info.statValue = rangedDamage * 100;
                    info.TextColor = Color.Orange;
                }
                if (info.stat.Contains("Magic Damage"))
                {
                    info.statValue = magicDamage * 100;
                    info.TextColor = Color.Cyan;
                }

                if (info.stat.Contains("Melee Crit"))
                {
                    info.statValue = meleeCrit;
                    info.TextColor = Color.Red;
                }
                if (info.stat.Contains("Ranged Crit"))
                {
                    info.statValue = rangedCrit;
                    info.TextColor = Color.Orange;
                }
                if (info.stat.Contains("Magic Crit"))
                {
                    info.statValue = magicCrit;
                    info.TextColor = Color.Cyan;
                }

                if (info.stat.Contains("Minion Damage"))
                {
                    info.statValue = minionDamage * 100;
                }

                if (info.stat.Contains("Max Minions"))
                {
                    info.statValue = maxMinions;
                }

                if (info.stat.Contains("Melee Speed"))
                {
                    info.statValue = meleeSpeed * 100;
                    info.TextColor = Color.Red;
                }
                if (info.stat.Contains("Max Run Speed"))
                {
                    info.statValue = moveSpeed;
                    info.TextColor = Color.LimeGreen;
                }
                if (info.stat.Contains("Dodge Chance"))
                {
                    info.statValue = dodgeChance * 100;
                    info.TextColor = Color.LimeGreen;
                }
                if (info.stat.Contains("Block Chance"))
                {
                    info.statValue = blockChance * 100;
                    info.TextColor = Color.LimeGreen;
                }
            }
        }

        public void UpdateLevel(int newLevel)
        {
            this.levelText.SetText(String.Format("Level: {0}", newLevel));
        }

        public void UpdateSkillPoints(int skillPoints)
        {
            this.pointsText.SetText(String.Format("Skill points: {0}", skillPoints));
        }

        public void UpdateXpBar(float value, float minValue, float maxValue)
        {
            this.xpBar.value = value;
            this.xpBar.minValue = minValue;
            this.xpBar.maxValue = maxValue;
        }

        protected override UIElement CreateContainer()
        {
            UIPanel container = new UIPanel();
            container.SetPadding(PANEL_PADDING);
            container.BackgroundColor = new Color(20, 38, 103);
            return container;
        }

        protected override UIElement CreateHeader()
        {
            UIPanel header = new UIPanel();
            header.Width.Set(0, 1f);
            header.Height.Set(DEFAULT_HEADER_HEIGHT, 0f);
            header.BackgroundColor.A = 255;

            UIText title = new UIText("Character Stats");
            title.Left.Set(20, 0f);
            header.Append(title);
            return header;
        }

        protected override void Construct()
        {
            this.miscStatControls = new UIStatInfo[VapeRPG.MinorStats.Length];

            this.skillPanel = new UISkillPanel(2 * (this.container.Width.Pixels - 2 * PANEL_PADDING) / 3, this.container.Height.Pixels - 2 * PANEL_PADDING);
            this.skillPanel.SetPadding(0);
            this.skillPanel.Left.Set(0, 0);
            this.skillPanel.Top.Set(0, 0);

            this.statPanel = new UIPanel();
            this.statPanel.SetPadding(10);
            this.statPanel.Left.Set(2 * (this.container.Width.Pixels - 2 * PANEL_PADDING) / 3, 0);
            this.statPanel.Top.Set(0, 0);
            this.statPanel.Width.Set((this.container.Width.Pixels - 2 * PANEL_PADDING) / 3, 0);
            this.statPanel.Height.Set((this.container.Height.Pixels - 2 * PANEL_PADDING) / 2, 0);
            this.statPanel.BorderColor = Color.Black;
            this.statPanel.BackgroundColor = new Color(100, 118, 183);

            UIButton statListButton = new UIButton("Open Stats");
            statListButton.Width.Set(0, 0.66f);
            statListButton.Height.Set(30, 0f);
            statListButton.Top.Set(-statListButton.Height.Pixels / 2, 0.5f);
            statListButton.HAlign = 0.5f;
            statListButton.OnClick += (obj, args) =>
            {
                StatMenuUIState.visible = true;
            };
            this.statPanel.Append(statListButton);

            this.pointsText = new UIText("Skill points: 0", 0.8f);
            this.pointsText.HAlign = 0.5f;
            this.pointsText.Top.Set(-80, 1f);
            this.statPanel.Append(this.pointsText);

            UIButton resetXpUI = new UIButton("Reset status bar", false, 0.8f);
            resetXpUI.Top.Set(-40, 1f);
            resetXpUI.HAlign = 0.5f;
            resetXpUI.OnClick += (evt, element) =>
            {
                VapeRPG vapeMod = ModLoader.GetMod("VapeRPG") as VapeRPG;
                vapeMod.ExpUI.Reset();
            };
            this.statPanel.Append(resetXpUI);

            this.xpBar = new UIRelativeProgressBar(1, 0, 100, Color.Green, Color.Lime);
            this.xpBar.SetPadding(0);
            this.xpBar.Top.Set(30, 0);
            this.xpBar.Width.Set(0, 0.8f);
            this.xpBar.Height.Set(20, 0);
            this.xpBar.HAlign = 0.5f;
            this.xpBar.strokeThickness = 2;
            this.statPanel.Append(this.xpBar);

            this.levelText = new UIText("Level: 1", 0.8f);
            this.levelText.Top.Set(10, 0);
            this.levelText.HAlign = 0.5f;
            this.statPanel.Append(this.levelText);

            this.miscPanel = new UIPanel();
            this.miscPanel.SetPadding(10);
            this.miscPanel.Left.Set(2 * (this.container.Width.Pixels - 2 * PANEL_PADDING) / 3, 0);
            this.miscPanel.Top.Set((this.container.Height.Pixels - 2 * PANEL_PADDING) / 2, 0);
            this.miscPanel.Width.Set((this.container.Width.Pixels - 2 * PANEL_PADDING) / 3, 0);
            this.miscPanel.Height.Set((this.container.Height.Pixels - 2 * PANEL_PADDING) / 2, 0);
            this.miscPanel.BorderColor = Color.Black;
            this.miscPanel.BackgroundColor = new Color(100, 118, 183);

            UIPanel miscStatListContainer = new UIPanel();
            miscStatListContainer.Width.Set(0f, 1f);
            miscStatListContainer.Height.Set(this.miscPanel.Height.Pixels - 40, 0f);

            UIList miscStatList = new UIList();
            miscStatList.Width.Set(0f, 1f);
            miscStatList.Height.Set(0f, 1f);
            miscStatListContainer.Append(miscStatList);

            UIScrollbar miscStatScrollBar = new UIScrollbar();
            miscStatScrollBar.SetView(100f, 1000f);
            miscStatScrollBar.Height.Set(0f, 1f);
            miscStatScrollBar.HAlign = 1f;
            miscStatListContainer.Append(miscStatScrollBar);
            miscStatList.SetScrollbar(miscStatScrollBar);

            this.miscPanel.Append(miscStatListContainer);

            #region miscPanel texts

            for (int i = 0; i < this.miscStatControls.Length; i++)
            {
                this.miscStatControls[i] = new UIStatInfo(VapeRPG.MinorStats[i], this.miscPanel.Width.Pixels, 20, true, false, 0.8f);
                this.miscStatControls[i].Top.Set(i * this.miscStatControls[i].Height.Pixels + 5, 0);

                miscStatList.Add(this.miscStatControls[i]);
            }

            #endregion

            this.container.Append(this.statPanel);
            this.container.Append(this.miscPanel);
            this.container.Append(this.skillPanel);
        }
    }
}
