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
    class StatHelpUIState : UIState
    {
        private UIPanel helpPanel;
        private UIText[] statHelpTexts;

        public static bool visible = false;

        public override void OnInitialize()
        {
            this.statHelpTexts = new UIText[VapeRPG.BaseStats.Length];

            this.helpPanel = new UIPanel();
            this.helpPanel.Width.Set(600, 0);
            this.helpPanel.Height.Set(500, 0);
            this.helpPanel.HAlign = 0.5f;
            this.helpPanel.VAlign = 0.5f;
            this.helpPanel.BackgroundColor = new Color(60, 78, 143);

            for(int i = 0; i < statHelpTexts.Length; i++)
            {
                string stat = VapeRPG.BaseStats[i];
                string description = "";

                if(stat.Contains("Strength"))
                {
                    description = "Increases melee damage and melee crit.\nEvery second point increases life by one.";
                }
                else if(stat.Contains("Magic power"))
                {
                    description = "Increases magic damage and magic crit.";
                }
                else if(stat.Contains("Dexterity"))
                {
                    description = "Increases ranged damage and ranged crit.";
                }
                else if(stat.Contains("Agility"))
                {
                    description = "Increases melee speed.\nIncreases dodge chance by a minimal amount.";
                }
                else if(stat.Contains("Vitality"))
                {
                    description = "Every point increases life by 2. Every 5th point increases defense by one.";
                }
                else if(stat.Contains("Spirit"))
                {
                    description = "Increases minion damage. Increases magic damage by a minimal amount.\nEvery 50th point increases max minions by one.";
                }

                string text = String.Format("{0}: {1}", stat, description);
                this.statHelpTexts[i] = new UIText(text);
                this.statHelpTexts[i].Left.Set(0, 0);
                this.statHelpTexts[i].Height.Set(this.statHelpTexts[i].MinHeight.Pixels + 45, 0);
                this.statHelpTexts[i].Top.Set(this.statHelpTexts[i].Height.Pixels * i, 0);

                this.helpPanel.Append(this.statHelpTexts[i]);
            }

            base.Append(this.helpPanel);
        }
    }
}
