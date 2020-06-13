using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiLoadingScreen : GuiScreen
    {
        private GuiScreen next, current;
        private FragenGerangel fragenGerangel;
        //private Animation animation;
        private float time;
        private bool flag;

        public GuiLoadingScreen(GuiScreen nextScreen, GuiScreen currentScreen, FragenGerangel fragenGerangel) : base()
        {
            current = currentScreen;
            next = nextScreen;
            this.fragenGerangel = fragenGerangel;
            animation = new Animation(4);
            //animation.OnFinish += Animation_OnFinish;
            flag = true;
            animation.Fire();
        }

        public override void Open()
        {
            Opend = true;
        }

        public override void Close()
        {
            Opend = false;
        }

        public override void OnRender()
        {
            base.OnRender();
            if (!Opend)
                return;
            if(animation.Incremental && flag && (current != null ? !current.Opend : true) && animation.Finished && fragenGerangel.currentScreen == current)
            {
                flag = false;
                new Thread(() =>
                {
                    while (Globals.APIManager == null && current != null)
                        Thread.Sleep(100);
                    next.SetLocationAndSize(this, Size);
                    next.Init();
                    next.Open();
                    animation.Reverse();
                    fragenGerangel.currentScreen = next;
                }).Start();
            }
            else if(!animation.Incremental && animation.Finished)
            {
                Opend = false;
                return;
            }
            if (animation.Delta == 0)
                return;
            Color c1 = Color.FromArgb((int)(255 * animation.Delta), 2, 175, 230);
            Color c2 = Color.FromArgb((int)(255 * animation.Delta), 84, 105, 230);
            StateManager.FillGradientRect(Location, Size, c1, c2);

            time += StateManager.delta * 100 * 3;

            float var1 = Math.Abs(MathUtils.Sin(time * 0.35f) * 120);
            float var2 = Math.Abs(MathUtils.Sin(time * 0.35f) * 120);

            int start = (int)(time - var1);
            int end = (int)(time + var2) + 30;

            StateManager.Push();
            StateManager.Translate(Size / 2);
            StateManager.SetColor(0,0,0, (int)(255 * animation.Delta));
            Vector prevPoint = new Vector(MathUtils.Sin(start) * 50, MathUtils.Cos(start) * 50);
            for (int i = start; i < end; i+=5)
            {
                Vector newPoint = new Vector(MathUtils.Sin(i) * 50, MathUtils.Cos(i) * 50);
                StateManager.DrawLine(prevPoint, newPoint, 5);
                prevPoint = newPoint;
            }
            StateManager.Pop();
        }
    }
}
