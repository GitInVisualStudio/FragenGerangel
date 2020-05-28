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
        private Animation animation;
        private PointF[] points;
        private Vector point;

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

        private void GuiButton_OnClick(object sender, Vector e)
        {
            point = e;
            animation.Reset();
            animation.Fire();
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointF(point.X, point.Y);
            }
        }

        public override void Init()
        {            
            base.Init();
            OnClick += GuiButton_OnClick;
            animation = new Animation();
            animation.Speed /= 2;
            animation.Stop();
            CurrentColor = BackColor;
            points = new PointF[100];
            //OnEnter += (object sender, Vector location) =>
            //{
            //    CurrentColor = HoverColor;
            //};

            //OnLeave += (object sender, Vector location) =>
            //{
            //    CurrentColor = BackColor;
            //};
        }

        public override void OnRender()
        {
            //throw new NotImplementedException();
            //TODO: Render the shit
            if (CustomRender == null)
            {

                

                StateManager.SetColor(CurrentColor);
                StateManager.FillRoundRect(Location, Size, 5);
                StateManager.SetColor(FontColor);
                StateManager.DrawCenteredString(Name, Location + Size / 2);
                if (!animation.Finished)
                {
                    StateManager.Push();
                    //StateManager.Translate(Location - point + Size);
                    int k = 0;
                    for(float i = 0; i < 360 ; i+= 360.0f / points.Length, k++)
                    {
                        points[k] = new PointF(point.X + MathUtils.Sin(i) * Size.Length * animation.Delta, point.Y + MathUtils.Cos(i) * Size.Length * animation.Delta);
                        PointF[] a = StateManager.GetRoundRectPoints(Location.X, Location.Y, Size.X, Size.Y, 5, 100);
                        if (!MathUtils.IsInPolygon(a, points[k]))
                        {
                            if(k > 0)
                                points[k] = a[k];
                            //k--;
                        }
                    }

                    StateManager.FillPolygon(points);
                    StateManager.Pop();
                }
            }
            else
                CustomRender.Invoke();
        }
    }
}
