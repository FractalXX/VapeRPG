using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;

namespace VapeRPG.UI.States
{
    internal abstract class DraggableUI : UIState
    {
        public enum DragMode
        {
            Default,
            Header
        }

        public const int DEFAULT_HEADER_HEIGHT = 40;

        public string Title { get; set; }

        protected UIElement container { get; private set; }
        protected UIElement header { get; private set; }

        private bool isDragging = false;
        private Vector2 dragOffset;

        public abstract Vector2 DefaultPosition { get; }
        public abstract Vector2 DefaultSize { get; }

        public virtual bool HasHeader
        {
            get
            {
                return false;
            }
        }

        public DragMode dragMode { get; private set; }

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

        public void SetDragMode(DragMode dragMode)
        {
            if(dragMode == DragMode.Header && !this.HasHeader)
            {
                throw new System.Exception("DragMode.Header can only be set if the container has a header.");
            }

            this.dragMode = dragMode;
        }

        public void Reset()
        {
            this.SetPosition(this.DefaultPosition);
            this.SetSize(this.DefaultSize);
        }

        protected virtual void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            this.dragOffset = new Vector2(evt.MousePosition.X - this.container.Left.Pixels, evt.MousePosition.Y - this.container.Top.Pixels);

            if(this.dragMode == DragMode.Default)
            {
                this.isDragging = true;
            }
            else if(this.dragMode == DragMode.Header)
            {
                this.isDragging = this.header.ContainsPoint(evt.MousePosition);
            }
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
            this.header = this.CreateHeader();

            if(this.header == null && this.HasHeader)
            {
                throw new System.Exception("A header needs to be created for the DraggableUI if its HasHeader property is set.");
            }
            else if(this.header != null)
            {
                this.container.Append(this.header);
            }

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
        protected virtual UIElement CreateHeader()
        {
            return null;
        }
    }
}
