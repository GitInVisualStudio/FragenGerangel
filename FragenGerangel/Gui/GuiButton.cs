using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    /// <summary>
    /// button mit text welcher klickbar ist
    /// </summary>
    public class GuiButton : GuiComponent
    {
        private const float DEFAULT_WIDTH = 100; 
        private const float DEFAULT_HEIGHT = 20;
        private Color hoverColor;
        private Action customRender = null;
        private Color currentColor;
        private Animation animation;
        private Vector point;
        private bool prevSelected;
        protected float var1 = 0, var2 = 0;

        //individuelle render methode
        public Action CustomRender
        {
            get
            {
                return customRender;
            }

            set
            {
                customRender = value;
            }
        }

        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }

            set
            {
                currentColor = value;
            }
        }

        public Color HoverColor
        {
            get
            {
                return hoverColor;
            }

            set
            {
                hoverColor = value;
            }
        }

        /// <summary>
        /// text des buttons
        /// </summary>
        /// <param name="name"></param>
        public GuiButton(string name) : base(0, 0)
        {
            Name = name;
        }

        /// <summary>
        /// event beim drücken des button
        /// </summary>
        /// <param name="name"></param>
        /// <param name="click"></param>
        public GuiButton(string name, Action<Vector> click) : base(0, 0)
        {
            Name = name;
            OnClick += (object sender, Vector location) => click.Invoke(location); //Like wtf ?? OnClick += click;
        }

        /// <summary>
        /// initialisiert alle events
        /// </summary>
        public override void Init()
        {            
            base.Init();
            animation = new Animation();
            animation.Speed /= 2;
            animation.Stop();
            CurrentColor = BackColor;
            OnKeyPress += GuiButton_OnKeyPress;
            OnEnter += GuiButton_OnEnter;
            OnLeave += GuiButton_OnLeave;
        }

        /// <summary>
        /// animation beim überfahren des buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiButton_OnLeave(object sender, Vector e)
        {
            var2 = 0;
        }

        /// <summary>
        /// animation beim überfahren des buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiButton_OnEnter(object sender, Vector e)
        {
            var2 = 0.2f;
        }

        /// <summary>
        /// wenn enter gedrückt wird und der button ausgewählt ist, wird sein event aufgerufen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiButton_OnKeyPress(object sender, char e)
        {
            if (e == 13)
                Component_OnClick(new Vector(0, 0));
        }

        /// <summary>
        /// Zeichnen des buttons
        /// </summary>
        public override void OnRender()
        {
            if (Selected)
            {
                var2 = 0.2f;
                prevSelected = true;
            }
            else if (prevSelected)
            {
                var2 = 0f;
                prevSelected = false;
            }
            //übergang der fabe
            int r = CurrentColor.R + (int)((BackColor.R - CurrentColor.R) * StateManager.delta * 5);
            int g = CurrentColor.G + (int)((BackColor.G - CurrentColor.G) * StateManager.delta * 5);
            int b = CurrentColor.B + (int)((BackColor.B - CurrentColor.B) * StateManager.delta * 5);
            r = Math.Abs(r % 256);
            g = Math.Abs(g % 256);
            b = Math.Abs(b % 256);
            CurrentColor = Color.FromArgb(r, g, b);

            if (CustomRender == null)
            {
                var1 += (var2 - var1) * StateManager.delta * 10;
                r = CurrentColor.R - (int)(CurrentColor.R * (0.3f + var1));
                g = CurrentColor.G - (int)(CurrentColor.G * (0.3f + var1));
                b = CurrentColor.B - (int)(CurrentColor.B * (0.3f + var1));
                r = Math.Abs(r % 256);
                g = Math.Abs(g % 256);
                b = Math.Abs(b % 256);
                StateManager.SetColor(0, 0, 0, 50);
                StateManager.FillRoundRect(Location, Size + new Vector(2, 2), 10);
                StateManager.FillGradientRoundRect(Location, Size, CurrentColor, Color.FromArgb(r,g,b), 90, 10);
                StateManager.SetColor(FontColor);
                StateManager.SetFont(FontUtils.DEFAULT_FONT);
                StateManager.DrawCenteredString(Name, Location + Size / 2);
            }
            else
                CustomRender.Invoke();
        }
    }
}
