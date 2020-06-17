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
    /// <summary>
    /// Screen zum einloggen und registrieren
    /// </summary>
    public class GuiLogin : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private string displayText = "";//info text

        /// <summary>
        /// spiel instanz für das setzten des spielers
        /// </summary>
        /// <param name="fragenGerangel"></param>
        public GuiLogin(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        /// <summary>
        /// erstellt die notwendigen komponenten
        /// </summary>
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
            Components.Add(new GuiButton("Registration")
            {
                RX = 0.5f,
                RY = 0.5f,
                BackColor = Color.LawnGreen,
                Size = new Vector(300, 50),
                Location = new Vector(-150, 55),
                FontColor = Color.White
            });
            GetComponent<GuiButton>("Login").OnClick += GuiLogin_OnClick;
            GetComponent<GuiButton>("Registration").OnClick += OnRegister;

            base.Init();
        }

        private void OnRegister(object sender, Vector e)
        {
            fragenGerangel.OpenScreen(new GuiRegister(fragenGerangel));
        }

        /// <summary>
        /// wird aufgerufen wenn auf login gekickt wird
        /// speichert die logindaten und loggt sich ein
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiLogin_OnClick(object sender, Vector e)
        {
            string password = GetComponent<GuiTextBox>("Passwort").Text;
            string username = GetComponent<GuiTextBox>("Username").Text;
            if(password.Length >= 5 && username.Length >= 3)
            {
                displayText = "loggin in...";
                new Thread(() =>
                {
                    Globals.APIManager = new APIManager();
                    try
                    {
                        Globals.APIManager.Login(username, password).Wait(); //TODO: das try catchen
                    }
                    catch (Exception exc)
                    {
                        displayText = "Zugangsdaten nicht korrekt";
                        return;
                    }
                    File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/login.dat", new string[] { username, password });
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                }).Start();
            }
            else
            {
                displayText = "Username/Passwort zu kurz";
            }
        }

        /// <summary>
        /// Zeichnet die komponenten
        /// </summary>
        public override void OnRender()
        {
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            StateManager.FillGradientRect(Location, Size, c1, c2);
            StateManager.SetFont(new Font("Arial", 25));
            StateManager.SetColor(Color.White);
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, 100);
            StateManager.SetFont(new Font("Arial", 12));
            StateManager.DrawCenteredString(displayText, Size.X / 2, 150);
            base.OnRender();
        }
    }
}
