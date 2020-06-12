using FragenGerangel.Utils;
using FragenGerangel.Utils.API;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiLogin : GuiScreen
    {
        private FragenGerangel fragenGerangel;

        public GuiLogin(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        public override void Init()
        {
            Components.Add(new GuiButton("Login")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, 0),
                FontColor = Color.White
            });
            Components.Add(new GuiTextBox("Username")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, -110),
                FontColor = Color.White
            });
            Components.Add(new GuiTextBox("Passwort")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, -55),
                FontColor = Color.White
            });

            GetComponent<GuiButton>("Login").OnClick += GuiLogin_OnClick;

            base.Init();
        }

        private void GuiLogin_OnClick(object sender, Vector e)
        {
            string password = GetComponent<GuiTextBox>("Passwort").Text;
            string username = GetComponent<GuiTextBox>("Username").Text;
            if(password.Length >= 5 && username.Length >= 3)
            {
                Close();
                Globals.APIManager = new APIManager(username, password);
            }
        }

        public override void OnRender()
        {
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            StateManager.FillGradientRect(Location, Size, c1, c2);
            StateManager.SetFont(new Font("Arial", 25));
            StateManager.SetColor(Color.White);
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, 100);
            base.OnRender();
        }
    }
}
