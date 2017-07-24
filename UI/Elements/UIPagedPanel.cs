using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UIPagedPanel : UITexturedPanel
    {
        private byte pageCount;

        public byte PageCount
        {
            get
            {
                return this.pageCount;
            }
            set
            {
                if(value < 1)
                {
                    throw new ArgumentOutOfRangeException("UIPagedPanel: PageCount must be at least one.");
                }

                this.pageCount = value;
            }
        }

        private UIVapeButton leftButton;
        private UIVapeButton rightButton;
        private UIText currentPageText;

        public List<UISkillPage> Pages { get; private set; }

        private byte currentPage;

        public UIPagedPanel(Texture2D backgroundTexture, int strokeThickness, byte pageCount, float width, float height) : base(backgroundTexture, strokeThickness)
        {
            this.Pages = new List<UISkillPage>();

            this.Width.Set(width, 0);
            this.Height.Set(height, 0);

            this.pageCount = pageCount;

            this.leftButton = new UIVapeButton(ModLoader.GetTexture("VapeRPG/Textures/UI/LeftButton"), ModLoader.GetTexture("VapeRPG/Textures/UI/LeftButtonPressed"));
            this.leftButton.Width.Set(30, 0);
            this.leftButton.Height.Set(30, 0);
            this.leftButton.Left.Set(this.Width.Pixels / 3, 0);
            this.leftButton.Top.Set(this.Height.Pixels - this.leftButton.Height.Pixels - 10, 0);

            this.rightButton = new UIVapeButton(ModLoader.GetTexture("VapeRPG/Textures/UI/RightButton"), ModLoader.GetTexture("VapeRPG/Textures/UI/RightButtonPressed"));
            this.rightButton.Width.Set(30, 0);
            this.rightButton.Height.Set(30, 0);
            this.rightButton.Left.Set(this.Width.Pixels - this.Width.Pixels / 3, 0);
            this.rightButton.Top.Set(this.Height.Pixels - this.rightButton.Height.Pixels - 10, 0);

            this.currentPageText = new UIText("1");
            this.currentPageText.Width.Set(30, 0);
            this.currentPageText.Height.Set(30, 0);
            this.currentPageText.Left.Set(this.Width.Pixels / 2, 0);
            this.currentPageText.Top.Set(this.leftButton.Top.Pixels, 0);

            this.currentPage = 0;

            for (int i = 0; i < pageCount; i++)
            {
                UISkillPage page = new UISkillPage(this.backgroundTexture, 0);
                page.Width.Set(this.Width.Pixels, 0);
                page.Height.Set(this.Height.Pixels, 0);
                page.Left.Set(0, 0);
                page.Top.Set(0, 0);
                if (i == this.currentPage) page.visible = true;

                this.Pages.Add(page);
                this.Append(page);
            }

            this.leftButton.OnClick += (() => this.GoToPage((byte)(this.currentPage - 1)));
            this.rightButton.OnClick += (() => this.GoToPage((byte)(this.currentPage + 1)));

            this.Append(this.leftButton);
            this.Append(this.rightButton);
            this.Append(this.currentPageText);
        }

        public override void Update(GameTime gameTime)
        {
            this.currentPageText.SetText(String.Format("{0}", this.currentPage+1));
            base.Update(gameTime);
        }

        private void GoToPage(byte page)
        {
            if(page >= 0 && page < this.pageCount)
            {
                this.Pages[currentPage].visible = false;
                this.Pages[page].visible = true;
                this.currentPage = page;
            }
        }
    }
}
