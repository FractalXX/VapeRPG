using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace VapeRPG.UI.Elements
{
    class UIButton : UITextPanel<string>
    {
        private bool toggled;
        public bool isToggleButton;

        public bool Toggled
        {
            get
            {
                return this.toggled;
            }
            set
            {
                if(this.isToggleButton)
                {
                    if (value)
                    {
                        this.TextColor = Color.Yellow;
                        this.BackgroundColor = new Color(80, 98, 163);
                    }
                    else
                    {
                        this.TextColor = Color.White;
                        this.BackgroundColor = new Color(60, 78, 143);
                    }

                    this.toggled = value;
                }
            }
        }

        public UIButton(string name, bool isToggle = false, float textScale = 1f) : base(name, textScale)
        {
            this.BackgroundColor = new Color(60, 78, 143);
            this.isToggleButton = isToggle;
            this.toggled = false;

            this.OnMouseOver += Event_MouseOver;
            this.OnMouseOut += Event_MouseOut;
            this.OnMouseDown += Event_MouseDown;
        }

        private void Event_MouseDown(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(10, -1, -1, 1, 1f, 0.0f);
            if (this.isToggleButton)
            {
                this.Toggled = !this.Toggled;
            }
        }

        private void Event_MouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!this.Toggled)
            {
                this.BackgroundColor = new Color(60, 78, 143);
            }
        }

        private void Event_MouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            this.BackgroundColor = new Color(80, 98, 163);
            Main.PlaySound(12, -1, -1, 1, 1f, 0.0f);
        }
    }
}
