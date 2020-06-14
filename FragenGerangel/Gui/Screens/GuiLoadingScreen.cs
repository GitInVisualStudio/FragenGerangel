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
    /// <summary>
    /// Screen zum wechseln zu anderen
    /// </summary>
    public class GuiLoadingScreen : GuiScreen
    {
        private GuiScreen next, current;
        private FragenGerangel fragenGerangel;
        private float time;
        private bool flag; //hat den anderen screen geöffnet

        /// <summary>
        /// momentaner & nächster screen + spiel instanz zum setzten
        /// </summary>
        /// <param name="nextScreen"></param>
        /// <param name="currentScreen"></param>
        /// <param name="fragenGerangel"></param>
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

        /// <summary>
        /// öffnen des ladebildschirms
        /// </summary>
        public override void Open()
        {
            Opend = true;
        }

        /// <summary>
        /// schließen des ladebildschirms
        /// </summary>
        public override void Close()
        {
            Opend = false;
        }

        /// <summary>
        /// Zeichnet den ladebildschirm und initialisiert den nächsten screen in einem anderen thread
        /// </summary>
        public override void OnRender()
        {
            base.OnRender();
            if (!Opend)
                return;
            //gucken ob der andere screen geschlossen ist
            if(animation.Incremental && flag && (current != null ? !current.Opend : true) && animation.Finished && fragenGerangel.currentScreen == current)
            {
                flag = false;
                new Thread(() =>
                {
                    while (Globals.APIManager == null && current != null) //warten auf die API => async
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

            //zeitreferenz
            time += StateManager.delta * 100 * 3;

            //zeichnen des kreises
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
