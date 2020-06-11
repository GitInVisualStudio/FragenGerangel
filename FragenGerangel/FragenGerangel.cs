using FragenGerangel.GameBase;
using FragenGerangel.Gui;
using FragenGerangel.Utils;
using FragenGerangel.Utils.API;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FragenGerangel
{
    public partial class FragenGerangel : Form
    {
        private Timer timer;
        public new Vector Size => new Vector(Width, Height);
        private GuiScreen currentScreen;

        public FragenGerangel()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            DoubleBuffered = true;
            Width = 1280;
            Height = 720;
            AddEvents();
            timer = new Timer()
            {
                Interval = (int)(1000.0f / 60.0f)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            box = new GuiCheckBox()
            {
                Location = new Vector(100, 100)
            };

            StateManager.Push();

            //currentScreen.Init();
            APIManager k = new APIManager("kaminund", "12345");
            APIManager y = new APIManager("yamimiriam", "12345");
            y.StartDuel(new Player("kaminund")).Wait();
            k.StartDuel(new Player("yamimiriam")).Wait();
            while (true)
            {
                k.Test().Wait();
                y.Test().Wait();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void AddEvents()
        {
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnClick(location);
                box.Component_OnClick(location);
            };
            MouseUp += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnRelease(location);
            };
            KeyDown += (object sender, KeyEventArgs args) =>
            {
                currentScreen?.Component_OnKeyPress((char)args.KeyValue);
            };
            KeyUp += (object sender, KeyEventArgs args) =>
            {
                currentScreen?.Component_OnKeyRelease((char)args.KeyValue);
            };
            MouseMove += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnMove(location);
            };
            Resize += (object sender, EventArgs args) =>
            {
                currentScreen?.Component_OnResize(Size);
            };
        }

        public void OpenScreen()
        {

        }

        GuiCheckBox box;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            StateManager.Push();
            StateManager.Update(e.Graphics);
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            AnimationManager.Update();
            #region drawing
            currentScreen?.OnRender();
            StateManager.FillGradientRect(new Vector(100, 100), new Vector(500, 200), Color.Black, Color.Red, 0.0f);
            box.OnRender();
            #endregion stopDrawing
            StateManager.Pop();
        }
    }
}
