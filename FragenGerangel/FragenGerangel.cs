using FragenGerangel.Gui;
using FragenGerangel.Gui.Screens;
using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FragenGerangel
{
    public partial class FragenGerangel : Form
    {
        private System.Windows.Forms.Timer timer;
        public new Vector Size => new Vector(Width, Height);
        private GuiScreen currentScreen, loadingScreen;

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

            timer = new System.Windows.Forms.Timer()
            {
                Interval = (int)(1000.0f / 60.0f)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            FontUtils.Init(this);
            OpenScreen(new GuiQuestion());

            StateManager.Push();
            //currentScreen.Init();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void AddEvents()
        {
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnClick(location);
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
                loadingScreen?.Component_OnResize(Size);
            };
        }

        public void OpenScreen(GuiScreen next)
        {
            new Thread(() =>
            {
                loadingScreen = new GuiLoadingScreen(next, currentScreen);
                loadingScreen.SetLocationAndSize(this, Size);
                loadingScreen.Init();
                next.SetLocationAndSize(this, Size);
                currentScreen?.Close();
                loadingScreen.Open();
                next.Init();
                next.Open();
                while (loadingScreen != null && loadingScreen.Opend)
                    Thread.Sleep(100);
                currentScreen = next;
                loadingScreen = null;
            }).Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            loadingScreen = null;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            StateManager.Push();
            StateManager.Update(e.Graphics);
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            #region drawing
            currentScreen?.OnRender();
            loadingScreen?.OnRender();
            #endregion stopDrawing
            AnimationManager.Update();
            StateManager.Pop();
        }
    }
}
