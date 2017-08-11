using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace VapeRPG.UI.Elements
{
    class UIButton : UIPanel
    {
        private UIText nameText;

        public bool isToggle;
        private bool toggled;

        public bool Toggled
        {
            get
            {
                return this.toggled;
            }
            set
            {
                if(this.isToggle)
                {
                    if (value)
                    {
                        this.nameText.TextColor = Color.Yellow;
                        this.BackgroundColor = new Color(80, 98, 163);
                    }
                    else
                    {
                        this.nameText.TextColor = Color.White;
                        this.BackgroundColor = new Color(60, 78, 143);
                    }

                    this.toggled = value;
                }
            }
        }

        public UIButton(bool isToggle = false)
        {
            this.BackgroundColor = new Color(60, 78, 143);
            this.isToggle = isToggle;
            this.toggled = false;

            this.OnMouseOver += Event_MouseOver;
            this.OnMouseOut += Event_MouseOut;
            this.OnMouseDown += Event_MouseDown;
        }

        public void SetName(string name)
        {
            this.nameText = new UIText(name);
            this.nameText.HAlign = 0.5f;
            this.nameText.VAlign = 0.5f;
            this.Append(this.nameText);
        }

        private void Event_MouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            this.BackgroundColor = new Color(80, 98, 163);
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
        }

        private void Event_MouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!this.Toggled)
            {
                this.BackgroundColor = new Color(60, 78, 143);
            }
        }

        private void Event_MouseDown(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(10, -1, -1, 1, 1f, 0.0f);
            if (this.isToggle)
            {
                this.Toggled = !this.Toggled;
            }
        }
    }
}
