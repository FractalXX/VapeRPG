using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using VapeRPG.UI.Elements;
using VapeRPG.Util;

namespace VapeRPG.UI.States
{
    class StatMenuUIState : DraggableUI
    {
        public static bool visible = false;

        private UIStatInfo[] statControls;
        private UIText statPointsText;

        public override Vector2 DefaultPosition
        {
            get
            {
                return new Vector2(Main.screenWidth / 2 - this.Width.Pixels / 2, Main.screenHeight / 2 - this.Height.Pixels / 2);
            }
        }

        public override Vector2 DefaultSize
        {
            get
            {
                return new Vector2(350, 350);
            }
        }

        protected override UIElement CreateContainer()
        {
            UIPanel container = new UIPanel();
            container.BackgroundColor.A = 255;
            return container;
        }

        protected override void Construct()
        {
            this.statControls = new UIStatInfo[VapeRPG.BaseStats.Length];

            this.statPointsText = new UIText("Stat points: 0");
            this.statPointsText.Top.Set(10, 0);
            this.container.Append(this.statPointsText);

            UIPanel statListContainer = new UIPanel();
            statListContainer.Width.Set(0f, 1f);
            statListContainer.Height.Set(-50, 1f);
            statListContainer.Top.Set(50, 0f);

            this.container.Append(statListContainer);

            UIList statList = new UIList();
            statList.ListPadding = 5f;
            statList.Width.Set(0f, 1f);
            statList.Height.Set(0f, 1f);
            statList.Top.Set(30, 0);
            statListContainer.Append(statList);

            UIScrollbar statScrollBar = new UIScrollbar();
            statScrollBar.SetView(100f, 1000f);
            statScrollBar.Height.Set(0f, 1f);
            statScrollBar.HAlign = 1f;

            statListContainer.Append(statScrollBar);
            statList.SetScrollbar(statScrollBar);

            for (int i = 0; i < statControls.Length; i++)
            {
                this.statControls[i] = new UIStatInfo(VapeRPG.BaseStats[i], 2 * this.container.Width.Pixels / 3, 20);
                this.statControls[i].Top.Set(70 + 1.2f * i * this.statControls[i].Height.Pixels + 5, 0);
                this.statControls[i].TextColor = Color.Yellow;
                statList.Add(this.statControls[i]);
            }

            UIButton exitButton = new UIButton("X");
            exitButton.Width.Set(40, 0);
            exitButton.Height.Set(40, 0);
            exitButton.Left.Set(-40, 1f);
            exitButton.OnClick += (obj, args) => StatMenuUIState.visible = false;

            this.container.Append(exitButton);
        }

        public void UpdateStats(IDictionary<string, int> baseStats, IDictionary<string, int> effectiveStats, int statPoints)
        {
            foreach (UIStatInfo info in this.statControls)
            {
                info.statValue = baseStats[info.stat];
                info.bonusValue = effectiveStats[info.stat] - baseStats[info.stat];
            }

            this.statPointsText.SetText(String.Format("Stat points: {0}", statPoints));
        }
    }
}
