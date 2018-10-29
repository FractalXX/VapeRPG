using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace VapeRPG.UI.States
{
    internal abstract class DraggableUI : UIState
    {
        protected UIElement container;

        private bool isDragging = false;
        private Vector2 dragOffset;

        public abstract Vector2 DefaultPosition { get; }
        public abstract Vector2 DefaultSize { get; }

        public Vector2 Position
        {
            get
            {
                return new Vector2(this.container.Left.Pixels, this.container.Top.Pixels);
            }
        }

        public Vector2 Size
        {
            get
            {
                return new Vector2(this.container.Width.Pixels, this.container.Height.Pixels);
            }
        }

        public void SetPosition(Vector2 position)
        {
            this.container.Left.Set(position.X, 0);
            this.container.Top.Set(position.Y, 0);
        }

        public void SetSize(Vector2 size)
        {
            this.container.Width.Set(size.X, 0);
            this.container.Height.Set(size.Y, 0);
        }

        public void Reset()
        {
            this.SetPosition(this.DefaultPosition);
            this.SetSize(this.DefaultSize);
        }

        protected virtual void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            this.dragOffset = new Vector2(evt.MousePosition.X - this.container.Left.Pixels, evt.MousePosition.Y - this.container.Top.Pixels);
            this.isDragging = true;
        }

        protected virtual void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 end = evt.MousePosition;
            this.isDragging = false;

            this.container.Left.Set(end.X - dragOffset.X, 0f);
            this.container.Top.Set(end.Y - dragOffset.Y, 0f);

            this.Recalculate();
        }

        public override void OnInitialize()
        {
            this.container = this.CreateContainer();

            this.SetPosition(this.DefaultPosition);
            this.SetSize(this.DefaultSize);

            this.container.OnMouseDown += this.DragStart;
            this.container.OnMouseUp += this.DragEnd;

            this.Construct();

            base.Append(this.container);
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 mousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if(this.container.ContainsPoint(mousePosition))
            {
                Main.LocalPlayer.mouseInterface = this.container.ContainsPoint(mousePosition);
            }

            if (this.isDragging)
            {
                this.SetPosition(new Vector2(mousePosition.X - this.dragOffset.X, mousePosition.Y - this.dragOffset.Y));
                this.Recalculate();
            }

            base.Update(gameTime);
        }

        protected virtual void Construct() { }
        protected abstract UIElement CreateContainer();
    }
}
