using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States
{
    class CharUIState : UIState
    {
        private UITexturedPanel charPanel;
        private UITexturedPanel statPanel;
        private UITexturedPanel miscPanel;
        private UIPagedPanel skillPanel;

        private UIStatInfo[] statControls;
        private UIStatInfo[] miscStatControls;
        private UIText pointsText;
        private UIText chaosPointsText;

        private UIVapeButton[] statAdderButtons;

        public static bool visible = false;

        private float charPanelWidth;
        private float charPanelHeight;

        public override void OnInitialize()
        {
            this.statControls = new UIStatInfo[VapeRPG.baseStats.Length];
            this.miscStatControls = new UIStatInfo[VapeRPG.minorStats.Length];

            this.charPanelWidth = 800;
            this.charPanelHeight = 600;

            this.charPanel = new UITexturedPanel(ModLoader.GetTexture("VapeRPG/Textures/UI/VapeGUI"), 0);
            this.charPanel.SetPadding(0);
            this.charPanel.Left.Set(Main.screenWidth / 2 - this.charPanelWidth / 2, 0);
            this.charPanel.Top.Set(Main.screenHeight / 2 - this.charPanelHeight / 2, 0);
            this.charPanel.Width.Set(this.charPanelWidth, 0);
            this.charPanel.Height.Set(this.charPanelHeight, 0);
            this.charPanel.BackgroundColor = Color.White;
            this.charPanel.BorderColor = Color.Black;

            this.statPanel = new UITexturedPanel(ModLoader.GetTexture("VapeRPG/Textures/UI/VapeGUI"), 0);
            this.statPanel.SetPadding(0);
            this.statPanel.Left.Set(2 * this.charPanel.Width.Pixels / 3, 0);
            this.statPanel.Top.Set(0, 0);
            this.statPanel.Width.Set(this.charPanel.Width.Pixels / 3, 0);
            this.statPanel.Height.Set(this.charPanel.Height.Pixels / 2, 0);
            this.statPanel.BackgroundColor = Color.LightBlue;
            this.statPanel.BorderColor = Color.Black;

            #region statPanel texts

            for (int i = 0; i < statControls.Length; i++)
            {
                this.statControls[i] = new UIStatInfo(VapeRPG.baseStats[i], this.statPanel.Width.Pixels / 2, 20);
                this.statControls[i].Left.Set(this.statPanel.Width.Pixels / 8, 0);
                this.statControls[i].Top.Set(20 + 1.2f * i * this.statControls[i].Height.Pixels + 5, 0);
                this.statControls[i].TextColor = Color.LightGray;
                this.statPanel.Append(this.statControls[i]);
            }

            this.pointsText = new UIText("Stat points: 0\nSkill points: 0");
            this.pointsText.Width.Set(this.statPanel.Width.Pixels / 3, 0);
            this.pointsText.Height.Set(this.statPanel.Height.Pixels / 5, 0);
            this.pointsText.Left.Set(this.statPanel.Width.Pixels / 2 - this.pointsText.Width.Pixels / 2, 0);
            this.pointsText.Top.Set(this.statPanel.Height.Pixels * 0.75f, 0);
            this.statPanel.Append(this.pointsText);
            #endregion

            this.charPanel.Append(this.statPanel);

            this.miscPanel = new UITexturedPanel(ModLoader.GetTexture("VapeRPG/Textures/UI/VapeGUI"), 0);
            this.miscPanel.SetPadding(0);
            this.miscPanel.Left.Set(2 * this.charPanel.Width.Pixels / 3, 0);
            this.miscPanel.Top.Set(this.charPanel.Height.Pixels / 2, 0);
            this.miscPanel.Width.Set(this.charPanel.Width.Pixels / 3, 0);
            this.miscPanel.Height.Set(this.charPanel.Height.Pixels / 2, 0);
            this.miscPanel.BackgroundColor = Color.LightBlue;
            this.miscPanel.BorderColor = Color.Black;

            this.chaosPointsText = new UIText("Chaos points: 0");
            this.chaosPointsText.Width.Set(this.miscPanel.Width.Pixels / 3, 0);
            this.chaosPointsText.Height.Set(this.miscPanel.Height.Pixels / 6, 0);
            this.chaosPointsText.Left.Set(this.miscPanel.Width.Pixels / 2 - this.chaosPointsText.Width.Pixels / 2, 0);
            this.chaosPointsText.Top.Set(this.miscPanel.Height.Pixels - this.chaosPointsText.Height.Pixels, 0);
            this.miscPanel.Append(this.chaosPointsText);

            #region miscPanel texts

            for(int i = 0; i < this.miscStatControls.Length; i++)
            {
                this.miscStatControls[i] = new UIStatInfo(VapeRPG.minorStats[i], this.miscPanel.Width.Pixels / 2, 20, true);
                this.miscStatControls[i].Left.Set(this.miscPanel.Width.Pixels / 8, 0);
                this.miscStatControls[i].Top.Set(20 + 1.2f * i * this.miscStatControls[i].Height.Pixels + 5, 0);
                this.miscStatControls[i].TextColor = Color.LightGray;
                this.miscPanel.Append(this.miscStatControls[i]);
            }

            #endregion

            this.charPanel.Append(this.miscPanel);

            this.skillPanel = new UIPagedPanel(ModLoader.GetTexture("VapeRPG/Textures/UI/VapeGUI"), 0, (byte)Math.Ceiling((double)VapeRPG.skills.Count / 9), 2 * this.charPanel.Width.Pixels / 3, this.charPanel.Height.Pixels);
            this.skillPanel.SetPadding(0);
            this.skillPanel.Left.Set(0, 0);
            this.skillPanel.Top.Set(0, 0);

            int currentRow = 0;
            int currentColumn = 0;
            int currentPage = 0;

            #region skills

            foreach (Skill skill in VapeRPG.skills)
            {
                UISkillInfo usi = new UISkillInfo(skill, 150, 130);

                float padding = 10;

                int numberOfColumns = (int)(this.skillPanel.Pages[currentPage].Width.Pixels / (usi.Width.Pixels + padding));
                int numberOfRows = (int)((this.skillPanel.Pages[currentPage].Height.Pixels - 60) / (usi.Height.Pixels + padding));

                usi.Left.Set(currentColumn * (padding + this.skillPanel.Pages[currentPage].Width.Pixels) / numberOfColumns, 0);
                usi.Top.Set(currentRow * (padding + this.skillPanel.Pages[currentPage].Height.Pixels) / numberOfRows, 0);

                this.skillPanel.Pages[currentPage].Append(usi);

                currentColumn++;

                if (currentColumn >= numberOfColumns)
                {
                    currentColumn = 0;
                    currentRow++;

                    if (currentRow >= numberOfRows)
                    {
                        currentPage++;
                        currentRow = 0;
                    }
                }
            }

            this.charPanel.Append(this.skillPanel);

            #endregion

            base.Append(this.charPanel);
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

        public void UpdateBonusPanel(int chaosPoints, float meleeDamage, float magicDamage, float rangedDamage, int meleeCrit, int magicCrit, int rangedCrit, float meleeSpeed, float moveSpeed, float dodgeChance)
        {
            foreach (UIStatInfo usi in miscStatControls)
            {
                if(usi.stat.Contains("Melee Damage"))
                {
                    usi.statValue = (int)(meleeDamage * 100);
                }
                if (usi.stat.Contains("Ranged Damage"))
                {
                    usi.statValue = (int)(rangedDamage * 100);
                }
                if (usi.stat.Contains("Magic Damage"))
                {
                    usi.statValue = (int)(magicDamage * 100);
                }

                if (usi.stat.Contains("Melee Crit"))
                {
                    usi.statValue = meleeCrit;
                }
                if (usi.stat.Contains("Ranged Crit"))
                {
                    usi.statValue = rangedCrit;
                }
                if (usi.stat.Contains("Magic Crit"))
                {
                    usi.statValue = magicCrit;
                }

                if(usi.stat.Contains("Melee Speed"))
                {
                    usi.statValue = (int)(meleeSpeed * 100);
                }
                if(usi.stat.Contains("Movement Speed"))
                {
                    usi.statValue = (int)(moveSpeed * 100);
                }
                if(usi.stat.Contains("Dodge Chance"))
                {
                    usi.statValue = (int)(dodgeChance * 100);
                }

                this.chaosPointsText.SetText(String.Format("Chaos points: {0}", chaosPoints));
            }
        }
    }
}
