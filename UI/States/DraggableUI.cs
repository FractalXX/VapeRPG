using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.UI;

namespace VapeRPG.UI.States
{
    internal abstract class DraggableUI : UIState
    {
        public const int DEFAULT_HEADER_HEIGHT = 40;

        public string Title { get; set; }

        protected UIElement container { get; private set; }
        protected UIElement header { get; private set; }

        private bool isDragging = false;
        private Vector2 dragOffset;

        public abstract Vector2 DefaultPosition { get; }
        public abstract Vector2 DefaultSize { get; }

        public bool HasHeader
        {
            get
            {
                return this.header != null;
            }
        }

        public Vector2 Position
        {
            get
            {
                Vector2 position = new Vector2(this.container.Left.Pixels, this.container.Top.Pixels);
                if(this.HasHeader)
                {
                    position.Y -= this.header.Height.Pixels;
                }
                return position;
            }
        }

        public Vector2 Size
        {
            get
            {
                Vector2 size = new Vector2(this.container.Width.Pixels, this.container.Height.Pixels);
                if(this.HasHeader)
                {
                    size.Y += this.header.Height.Pixels;
                }
                return size;
            }
        }

        public void SetPosition(Vector2 position)
        {
            this.container.Left.Set(position.X, 0);
            this.container.Top.Set(position.Y, 0);

            if (this.HasHeader)
            {
                this.header.Left.Set(position.X, 0);
                this.header.Top.Set(position.Y, 0);
                this.container.Top.Set(this.header.Top.Pixels + this.header.Height.Pixels, 0);
            }
        }

        public void SetSize(Vector2 size)
        {
            this.container.Width.Set(size.X, 0);
            this.container.Height.Set(size.Y, 0);

            if (this.header != null)
            {
                this.header.Width.Set(size.X, 0);
            }
        }

        public void Reset()
        {
            this.SetPosition(this.DefaultPosition);
            this.SetSize(this.DefaultSize);
        }

        protected virtual void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            this.dragOffset = new Vector2(evt.MousePosition.X - this.container.Left.Pixels, evt.MousePosition.Y - this.container.Top.Pixels);

            if (!this.HasHeader)
            {
                this.isDragging = true;
            }
            else
            {
                this.isDragging = this.header.ContainsPoint(evt.MousePosition);
                this.dragOffset = new Vector2(evt.MousePosition.X - this.header.Left.Pixels, evt.MousePosition.Y - this.header.Top.Pixels);
            }
        }

        protected virtual void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            this.isDragging = false;

            this.container.Left.Set(end.X - dragOffset.X, 0f);
            this.container.Top.Set(end.Y - dragOffset.Y, 0f);

            if (this.HasHeader)
            {
                this.header.Left.Set(end.X - dragOffset.X, 0f);
                this.header.Top.Set(end.Y - dragOffset.Y, 0f);
                this.container.Top.Set(this.header.Top.Pixels + this.header.Height.Pixels, 0f);
            }

            this.Recalculate();
        }

        public override void OnInitialize()
        {
            this.container = this.CreateContainer();
            this.header = this.CreateHeader();

            if (this.HasHeader)
            {
                this.Append(this.header);
                this.header.OnMouseDown += this.DragStart;
                this.header.OnMouseUp += this.DragEnd;
            }
            else
            {
                this.container.OnMouseDown += this.DragStart;
                this.container.OnMouseUp += this.DragEnd;
            }

            this.SetPosition(this.DefaultPosition);
            this.SetSize(this.DefaultSize);

            this.Construct();

            base.Append(this.container);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.container.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (this.isDragging)
            {
                Vector2 newPosition = new Vector2(Main.MouseScreen.X - this.dragOffset.X, Main.MouseScreen.Y - this.dragOffset.Y);
                if(newPosition.X < 0)
                {
                    newPosition.X = 0;
                }
                else if(newPosition.X >= Main.screenWidth - this.container.Width.Pixels)
                {
                    newPosition.X = Main.screenWidth - this.container.Width.Pixels;
                }

                if(newPosition.Y < 0)
                {
                    newPosition.Y = 0;
                }
                else if(newPosition.Y >= Main.screenHeight - this.container.Height.Pixels - (this.HasHeader ? this.header.Height.Pixels : 0))
                {
                    newPosition.Y = Main.screenHeight - this.container.Height.Pixels - (this.HasHeader ? this.header.Height.Pixels : 0);
                }

                this.SetPosition(newPosition);
                this.Recalculate();
            }

            base.Update(gameTime);
        }

        protected virtual void Construct() { }
        protected abstract UIElement CreateContainer();
        protected virtual UIElement CreateHeader()
        {
            return null;
        }
    }
}
