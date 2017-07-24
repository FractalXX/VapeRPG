using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VapeRPG.UI.Elements
{
    class UIVapeButton : UIElement
    {
        private bool pressed;

        private Texture2D pressedTexture;
        private Texture2D texture;

        public new event Action OnClick;

        public bool visible;

        public UIVapeButton(Texture2D texture, Texture2D pressedTexture)
        {
            this.pressedTexture = pressedTexture;
            this.texture = texture;

            this.pressed = false;
            this.visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            // Custom click code
            MouseState ms = Mouse.GetState();
            if (this.ContainsPoint(new Vector2(ms.X, ms.Y)))
            {
                Main.LocalPlayer.mouseInterface = true;
                if (ms.LeftButton == ButtonState.Pressed && !this.pressed && this.visible)
                {
                    this.pressed = true;
                    Main.PlaySound(SoundLoader.customSoundType, -1, -1, Main.player[Main.myPlayer].GetModPlayer<VapePlayer>().mod.GetSoundSlot(SoundType.Custom, "Sounds/UI/ButtonPress"));
                }
                if (ms.LeftButton == ButtonState.Released && this.pressed)
                {
                    this.pressed = false;
                    try
                    {
                        this.OnClick();
                    }
                    catch(Exception e)
                    {
                        Main.NewText(e.ToString(), Color.Red);
                    }
                }
            }
            this.visible = false;

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Drawing the UIElement itself
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            if (!this.pressed)
            {
                spriteBatch.Draw(this.texture, new Rectangle(point1.X, point1.Y, (int)this.Width.Pixels, height), Color.White);
            }
            else
            {
                spriteBatch.Draw(this.pressedTexture, new Rectangle(point1.X, point1.Y, (int)this.Width.Pixels, height), Color.White);
            }
            this.visible = true;
        }
    }
}
