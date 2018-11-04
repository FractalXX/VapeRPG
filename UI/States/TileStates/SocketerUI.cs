using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.UI;
using VapeRPG.UI.Elements;

namespace VapeRPG.UI.States.TileStates
{
    internal class SocketerUI : TileUI
    {
        public override Vector2 DefaultSize => new Vector2(300, 200);

        protected override void Construct()
        {
            this.container.SetPadding(10f);

            UIInteractableItemPanel itemPanel = new UIInteractableItemPanel((item) => item.damage > 0);
            itemPanel.Width.Set(50f, 0f);
            itemPanel.Height.Set(50f, 0f);

            UIInteractableItemPanel socketPanel = new UIInteractableItemPanel(/* is socket? */);
            socketPanel.Width.Set(50f, 0f);
            socketPanel.Height.Set(50f, 0f);
            socketPanel.Left.Set(itemPanel.Left.Pixels + itemPanel.Width.Pixels + 2 * this.container.PaddingLeft, 0f);

            UIButton applyButton = new UIButton("Apply");
            applyButton.Width.Set(100f, 0f);
            applyButton.Height.Set(30f, 0f);
            applyButton.Top.Set(itemPanel.Top.Pixels + itemPanel.Height.Pixels + 2 * this.container.PaddingTop, 0f);

            this.container.Append(itemPanel);
            this.container.Append(socketPanel);
            this.container.Append(applyButton);

            base.Construct();
        }
    }
}
