using FragenGerangel.Utils;
using FragenGerangel.Utils.API;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiLogin : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private string displayText = "";

        public GuiLogin(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        public override void Init()
        {
            Components.Add(new GuiTextBox("Username")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, -110),
                FontColor = Color.White
            });
            Components.Add(new GuiPasswordBox("Passwort")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, -55),
                FontColor = Color.White
            });
            Components.Add(new GuiButton("Login")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, 0),
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
                displayText = "loggin in...";
                new Thread(() =>
                {
                    File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/login.dat", new string[] { username, password });
                    Globals.APIManager = new APIManager(username, password);
                }).Start();
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            }
            else
            {
                displayText = "Username/Passwort zu kruz";
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
            StateManager.SetFont(new Font("Arial", 12));
            StateManager.DrawCenteredString(displayText, Size.X / 2, 200);
            base.OnRender();
        }
    }
}
