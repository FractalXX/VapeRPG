using System;
using System.Collections.Generic;
using Terraria.ModLoader.UI.Elements;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class VapeSkillPanel : UIPanel
    {
        private List<UISkillTab> tabs;
        private List<UIButton> buttons;

        private UIPanel buttonPanel;

        private byte currentTab;

        private static Dictionary<string, SkillType> tabNames = new Dictionary<string, SkillType>()
        {
            { "On Kill Effects", SkillType.OnKill },
            { "On Hit Effects", SkillType.OnHit }
        };

        public VapeSkillPanel(float width, float height)
        {
            this.tabs = new List<UISkillTab>();
            this.buttons = new List<UIButton>();

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            this.buttonPanel = new UIPanel();
            this.buttonPanel.SetPadding(5);
            this.buttonPanel.Width.Set(width, 0);
            this.buttonPanel.Height.Set(70, 0);
            this.buttonPanel.BackgroundColor = new Color(40, 58, 123);

            this.currentTab = 0;

            int i = 0;
            foreach(var x in tabNames)
            {
                UISkillTab tab = new UISkillTab(x.Key, x.Value);
                tab.Width.Set(this.Width.Pixels, 0);
                tab.Height.Set(this.Height.Pixels, 0);
                tab.Left.Set(0, 0);
                tab.Top.Set(60, 0);

                tab.InitializeSkillInfos();

                UIButton button = new UIButton(true);
                button.Width.Set((this.Width.Pixels - 10) / tabNames.Count, 0);
                button.Height.Set(60, 0);
                button.Left.Set(i * button.Width.Pixels, 0);
                button.Top.Set(0, 0);
                button.SetName(x.Key);
                
                this.buttons.Add(button);
                this.tabs.Add(tab);

                this.Append(tab);
                this.buttonPanel.Append(button);

                i++;
            }

            this.tabs[this.currentTab].visible = true;
            this.buttons[0].Toggled = true;

            foreach(var x in this.buttons)
            {
                x.OnMouseDown += (e, y) =>
                {
                    this.GoToTab((byte)this.buttons.IndexOf(x));
                };
            }

            this.Append(this.buttonPanel);
        }

        private void GoToTab(byte tab)
        {
            if(tab != this.currentTab)
            {
                if (tab < this.tabs.Count)
                {
                    this.buttons[this.currentTab].Toggled = false;
                    this.tabs[this.currentTab].visible = false;
                    this.tabs[tab].visible = true;
                    this.currentTab = tab;
                }
            }
            else
            {
                this.buttons[tab].Toggled = true;
            }
        }
    }
}
