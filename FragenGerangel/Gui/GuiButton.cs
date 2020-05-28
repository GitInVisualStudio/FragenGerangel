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
    public class GuiButton : GuiComponent
    {
        private const float DEFAULT_WIDTH = 100; 
        private const float DEFAULT_HEIGHT = 20;
        private Color hoverColor;
        private Action customRender = null;
        private Color currentColor;

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

        //public Color HoverColor { get => hoverColor; set => hoverColor = value; }

        //public override Color BackColor { get => base.BackColor; set => currentColor = base.BackColor = value; }
        //public Action CustomRender { get => customRender; set => customRender = value; }

        //TODO: den kack mit den Size fixen lol idk wie ich es machen soll
        public GuiButton(string name) : base(0, 0)
        {
            Name = name;
        }

        public GuiButton(string name, Action<Vector> click) : base(0, 0)
        {
            Name = name;
            OnClick += (object sender, Vector location) => click.Invoke(location); //Like wtf ?? OnClick += click;
        }


        public override void Init()
        {            
            base.Init();
            OnEnter += (object sender, Vector location) =>
            {
                CurrentColor = HoverColor;
            };

            OnLeave += (object sender, Vector location) =>
            {
                CurrentColor = BackColor;
            };
        }

        public override void OnRender()
        {
            //throw new NotImplementedException();
            //TODO: Render the shit
            if (CustomRender == null)
            {
                StateManager.SetColor(CurrentColor);
                StateManager.FillRect(Location, Size);
                StateManager.SetColor(FontColor);
                StateManager.DrawCenteredString(Name, Location + Size / 2);
            }
            else
                CustomRender.Invoke();
        }
    }
}
