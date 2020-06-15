using FragenGerangel.GameBase;
using FragenGerangel.Gui;
using FragenGerangel.Gui.Screens;
using FragenGerangel.Utils;
using FragenGerangel.Utils.API;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FragenGerangel
{
    public partial class FragenGerangel : Form
    {
        private System.Windows.Forms.Timer timer; //timer zum updaten
        public new Vector Size => new Vector(Width, Height); //größe als vektor
        public GuiScreen currentScreen;
        private GuiScreen loadingScreen;

        public FragenGerangel()
        {
            InitializeComponent();
            Init();
        }

        /// <summary>
        /// initialisiert das spiel
        /// </summary>
        public void Init()
        {
            DoubleBuffered = true; //sonst falckert es
            Width = 1280;
            Height = 720;
            Text = "FragenGerangel";
            AddEvents();

            timer = new System.Windows.Forms.Timer()
            {
                Interval = (int)(1000.0f / 120.0f)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            FontUtils.Init(this);
            //öffnet den loginscreen falls keine accountdaten vorhanden sind
            string[] account;
            if ((account = GetAccount()) == null)
                OpenScreen(new GuiLogin(this));
            else
            {
                Globals.APIManager = new APIManager(account[0], account[1]);
                while (Globals.APIManager == null)
                    Thread.Sleep(100);
                OpenScreen(new GuiMainScreen(this));
            }
            StateManager.Push();
        }

        /// <summary>
        /// gibt die account daten
        /// </summary>
        /// <returns></returns>
        private string[] GetAccount()
        {
            return null;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/login.dat";
            if (!File.Exists(path))
                return null;
            return File.ReadAllLines(path);
        }

        /// <summary>
        /// pro timertick wird das spiel aktualisiert
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// setzt alle notwendigen events für den screen damit die eingabe verarbeitet werden kann
        /// </summary>
        private void AddEvents()
        {
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                if (loadingScreen != null)
                    return;
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnClick(location);
            };
            MouseUp += (object sender, MouseEventArgs e) =>
            {
                if (loadingScreen != null)
                    return;
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnRelease(location);
            };
            KeyPress += (object sender, KeyPressEventArgs args) =>
            {
                if (loadingScreen != null)
                    return;
                currentScreen?.Component_OnKeyPress(args.KeyChar);
            };
            KeyUp += (object sender, KeyEventArgs args) =>
            {
                if (loadingScreen != null)
                    return;
                currentScreen?.Component_OnKeyRelease((char)args.KeyValue);
            };
            MouseMove += (object sender, MouseEventArgs e) =>
            {
                if (loadingScreen != null)
                    return;
                Vector location = new Vector(e.X, e.Y);
                currentScreen?.Component_OnMove(location);
            };
            MouseWheel += (object sender, MouseEventArgs e) =>
            {
                currentScreen?.OnSroll(-e.Delta);
            };
            Resize += (object sender, EventArgs args) =>
            {
                currentScreen?.Component_OnResize(Size);
                loadingScreen?.Component_OnResize(Size);
            };
        }

        /// <summary>
        /// öffnet einen neuen screen via loadingscreen
        /// </summary>
        /// <param name="next"></param>
        public void OpenScreen(GuiScreen next)
        {
            loadingScreen = new GuiLoadingScreen(next, currentScreen, this);
            loadingScreen.SetLocationAndSize(this, Size);
            loadingScreen.Init();
            if(currentScreen != null)
                if(currentScreen.Opend)
                    currentScreen.Close();
            loadingScreen.Open();
        }

        /// <summary>
        /// beendet alle threads wenn das form geschlossen wird
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            if(currentScreen != null)
                currentScreen.Close();
            loadingScreen = null;
        }

        /// <summary>
        /// wird aufgerufen wenn das fenster aktualisiert wird
        /// und zeichnet alle komponenten 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            StateManager.Push();
            StateManager.Update(e.Graphics);//setzen der graphics instanz für das zeichnen
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            #region drawing
            currentScreen?.OnRender();
            if(loadingScreen != null) //rendern des loadingscreens falls einer vorhanden ist
                if (loadingScreen.Opend)
                    loadingScreen?.OnRender();
                else
                    loadingScreen = null;
            #endregion stopDrawing
            AnimationManager.Update(); //updaten der animationen
            StateManager.Pop();
        }
    }
}
