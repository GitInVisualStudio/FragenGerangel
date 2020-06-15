using FragenGerangel.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    /// <summary>
    /// Oberklasse aller komponenten
    /// beinhaltet informationen zu größe, position usw.
    /// und behandelt alle events
    /// </summary>
    public abstract class GuiComponent
    {
        private bool hovering;
        private Vector prevScreenSize = new Vector(0, 0);
        private Vector location = new Vector(0, 0);
        private Vector size = new Vector(0,0);
        private string name;
        private bool selected;
        private Color backColor = Color.Gray;
        private Color fontColor;
        private float rX = -1, rY = -1, rWidth = -1, rHeight = -1;//Relative Location und Size
        public event EventHandler<Vector> OnResize;
        public event EventHandler<Vector> OnClick;
        public event EventHandler<Vector> OnMove;
        public event EventHandler<Vector> OnEnter;
        public event EventHandler<Vector> OnLeave;
        public event EventHandler<Vector> OnRelease;
        public event EventHandler<char> OnKeyPress;
        public event EventHandler<char> OnKeyRelease;

        public Vector Location
        {
            get
            {
                return location;
            }

            set
            {
                location = value;
            }
        }

        public Vector Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                selected = value;
            }
        }

        public Color BackColor
        {
            get
            {
                return backColor;
            }

            set
            {
                backColor = value;
            }
        }

        public Color FontColor
        {
            get
            {
                return fontColor;
            }

            set
            {
                fontColor = value;
            }
        }

        public float RX
        {
            get
            {
                return rX;
            }

            set
            {
                rX = value;
            }
        }

        public float RY
        {
            get
            {
                return rY;
            }

            set
            {
                rY = value;
            }
        }

        public float RWidth
        {
            get
            {
                return rWidth;
            }

            set
            {
                rWidth = value;
            }
        }

        public float RHeight
        {
            get
            {
                return rHeight;
            }

            set
            {
                rHeight = value;
            }
        }

        public void Component_OnResize(Vector size) => OnResize?.Invoke(this, size);
        public void Component_OnClick(Vector location) => OnClick?.Invoke(this, location);
        public void Component_OnRelease(Vector location) => OnRelease?.Invoke(this, location);
        public void Component_OnMove(Vector location) => OnMove?.Invoke(this, location);
        public void Component_OnEnter(Vector location) => OnEnter?.Invoke(this, location);
        public void Component_OnLeave(Vector location) => OnLeave?.Invoke(this, location);
        public void Component_OnKeyPress(char keyChar) => OnKeyPress?.Invoke(this, keyChar);
        public void Component_OnKeyRelease(char keyChar) => OnKeyRelease?.Invoke(this, keyChar);

        /// <summary>
        /// gibt zurück ob der mauscursor über der komponente ist
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public virtual bool OnHover(Vector location)
        {
            if(location.X > this.location.X && location.X < this.location.X + size.X && this.location.Y < location.Y && this.location.Y + size.Y > location.Y)
            {
                if (hovering == false)
                    OnEnter?.Invoke(this, location);
                return hovering = true;
            }
            if (hovering == true)
                OnLeave?.Invoke(this, location);
            return hovering = false;
        }

        /// <summary>
        /// Gundkonstruktoren mit position und größe
        /// flaot = absolut, double = relativ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GuiComponent(float x, float y) { Location = new Vector(x, y); }

        public GuiComponent(float x, float y, float width, float height)
        {
            Location = new Vector(x, y);
            Size = new Vector(width, height);
        }

        public GuiComponent(double x, double y, double width, double height)
        {
            RX = (float)x;
            RY = (float)y;
            RWidth = (float)width;
            RHeight = (float)height;
        }

        public GuiComponent(float x, float y, double width, double height)
        {
            RWidth = (float)width;
            RHeight = (float)height;
            Location = new Vector(x, y);
        }


        public GuiComponent(Vector location)
        {
            Location = location;
            Size = size;
        }

        public GuiComponent()
        {

        }

        /// <summary>
        /// fügt die events hinzu
        /// </summary>
        public virtual void Init()
        {
            OnResize += SetLocationAndSize;
        }
        
        /// <summary>
        /// methode zum zeichnen der komponente
        /// </summary>
        public abstract void OnRender();

        /// <summary>
        /// passt größe und position relativ zur fenster größe an
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="screenSize"></param>
        public virtual void SetLocationAndSize(object sender, Vector screenSize)
        {
            Location = new Vector(RX != -1 ?
                screenSize.X * RX + Location.X - prevScreenSize.X * RX :
                Location.X,
                RY != -1 ?
                screenSize.Y * RY + Location.Y - prevScreenSize.Y * RY:
                Location.Y );
            Size = new Vector(RWidth != -1 ?
                screenSize.X * RWidth + Size.X - prevScreenSize.X * RWidth:
                Size.X,
                RHeight != -1 ?
                screenSize.Y * RHeight + Size.Y - prevScreenSize.Y * RHeight:
                Size.Y);
            prevScreenSize = screenSize;
        }
    }
}
