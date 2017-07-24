using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VapeRPG.UI.Elements
{
    class UIVapeProgressBar : UIElement
    {
        public float value;
        public float maxValue;
        public float minValue;

        private Color backgroundColor;
        private Color foregroundColor;
        private Texture2D backgroundTexture;

        private UIText statusText;

        private float foregroundWidth;

        public int strokeThickness;
        public Color strokeColor;

        public UIVapeProgressBar(float value, float minValue, float maxValue, Color backgroundColor, Color foregroundColor)
        {
            this.value = value;
            this.minValue = minValue;
            this.maxValue = maxValue;

            this.backgroundColor = backgroundColor;
            this.foregroundColor = foregroundColor;

            this.strokeThickness = 0;
            this.strokeColor = Color.Black;

            this.backgroundTexture = ModLoader.GetTexture("VapeRPG/Textures/UI/Blank");
        }

        public override void OnInitialize()
        {
            // Initializing the text for the progress bar
            this.statusText = new UIText("0/0");
            this.statusText.Width.Set(this.Width.Pixels / 2, 0f);
            this.statusText.Height.Set(this.Height.Pixels / 2, 0f);
            this.statusText.Left.Set(this.Width.Pixels / 2 - this.statusText.Width.Pixels / 2, 0f);
            this.statusText.Top.Set(this.Height.Pixels / 4, 0f);

            this.Append(statusText);
        }

        public override void Update(GameTime gameTime)
        {
            // Scaling xp values to be between 0 and 1
            float relativeValue = (this.value - this.minValue) / (this.maxValue - this.minValue);
            if(relativeValue > this.maxValue)
            {
                relativeValue = this.maxValue;
            }
            this.foregroundWidth = relativeValue * this.Width.Pixels;
            this.statusText.SetText(String.Format("{0}/{1}", this.value, this.maxValue));
            this.Recalculate();

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Drawing the UIElement itself
            CalculatedStyle dimensions = this.GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);
            spriteBatch.Draw(this.backgroundTexture, new Rectangle(point1.X - this.strokeThickness, point1.Y - this.strokeThickness, width + this.strokeThickness * 2, height + this.strokeThickness * 2), this.strokeColor);
            spriteBatch.Draw(this.backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), this.backgroundColor);
            spriteBatch.Draw(this.backgroundTexture, new Rectangle(point1.X, point1.Y, (int)foregroundWidth, height), this.foregroundColor);
        }
    }
}
