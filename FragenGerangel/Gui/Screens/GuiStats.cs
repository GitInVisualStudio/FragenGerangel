using FragenGerangel.GameBase;
using FragenGerangel.Utils;
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
    /// <summary>
    /// zeigt die statistik eines spielers
    /// </summary>
    public class GuiStats : GuiScreen
    {
        private Statistic stats;
        private FragenGerangel fragenGerangel;
        private Player player;

        /// <summary>
        /// spieler & game instanz für die informationen
        /// </summary>
        /// <param name="fragenGerangel"></param>
        /// <param name="player"></param>
        public GuiStats(FragenGerangel fragenGerangel, Player player = null) : base()
        {
            this.fragenGerangel = fragenGerangel;
            if (player == null)
                player = Globals.Player;
            else
                this.player = player;
            animation.Speed = 0.5f;
        }

        /// <summary>
        /// holt alle notwendigen informationen vom sever
        /// </summary>
        public override void Init()
        {

            Components.Add(new GuiButton("Ausloggen")
            {
                Location = new Vector(-17 - 20 - 100, 100),
                Size = new Vector(100, 50),
                BackColor = Color.LawnGreen,
                FontColor = Color.White,
                RX = 1.0f
            });
            GetComponent<GuiButton>("Ausloggen").OnClick += (object sender, Vector e) =>
            {
                Globals.Player = null;
                fragenGerangel.OpenScreen(new GuiLogin(fragenGerangel));
            };

            base.Init();

            Task<Statistic> task = Globals.APIManager.GetStatistics(player);
            task.Wait();
            stats = task.Result;
        }

        /// <summary>
        /// geht zurück wenn der escape gedrückt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Panel_OnKeyRelease(object sender, char e)
        {
            if (e == 27)
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));

            base.Panel_OnKeyRelease(sender, e);
        }

        /// <summary>
        /// Zeichnet die statistik
        /// </summary>
        public override void OnRender()
        {
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, offset / 2);
            StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X, offset * 3), c1, c2);
            Player localPlayer = player;
            StateManager.Push();
            StateManager.Translate(0, offset + offset * 3 / 2f);
            RenderUtils.DrawPlayer(localPlayer.Name, new Vector(100, 0), 100, false);

            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("Arial", 20));
            float height = StateManager.GetStringHeight(localPlayer.Name);
            StateManager.DrawString(localPlayer.Name, 200, -height/2);
            StateManager.DrawString("Elo: " + stats.ELO.ToString(), 200, height / 2);
            StateManager.FillRect(200 - 5, -height/2, 2, height * 2);
            StateManager.Pop();
            base.OnRender();

            float r = 200, margin = 50;
            StateManager.Push();
            StateManager.Translate(0, offset * 6 + 10);
            StateManager.SetColor(Color.Black);
            StateManager.SetFont(new Font("Arial", 15, FontStyle.Bold));
            StateManager.DrawString("Statistik", margin, -r/2);
            height = StateManager.GetStringHeight("Statistik");
            StateManager.FillRect(margin - 5, -r/2, 2, height);
            StateManager.Translate(0, height + 20);
            StateManager.SetColor(Color.LightGray);
            StateManager.FillRoundRect(10, -r/2 - 10, Size.X - 17 - 20, r + 50);
            //insgesamte spiele
            int games = stats.Losses + stats.Wins + stats.PerfectGames + stats.Draws;
            //prozentuale evrteilung
            int wins = (int)((stats.Wins / (float)games) * 100);
            int losses = (int)((stats.Losses/ (float)games) * 100);
            int draws = (int)((stats.Draws / (float)games) * 100);
            if (games == 0)
                wins = losses = draws = 0;
            StateManager.SetColor(Color.Black);
            float var2 = r + 10;
            
            StateManager.SetColor(Color.LawnGreen);
            StateManager.FillCircle(r / 2 + margin, 0, var2, wins * 3.6f * animation.Delta);
            StateManager.SetColor(Color.Black);
            StateManager.FillCircle(Size.X / 2, 0, var2, draws * 3.6f * animation.Delta);
            StateManager.SetColor(Color.Red);
            StateManager.FillCircle(Size.X - 17 - r / 2 - margin, 0, var2, losses * 3.6f * animation.Delta);

            StateManager.SetColor(Color.White);
            StateManager.FillCircle(r/2 + margin, 0, r);
            StateManager.FillCircle(Size.X / 2, 0, r);
            StateManager.FillCircle(Size.X - 17 - r / 2 - margin, 0, r);

            StateManager.SetColor(Color.Black);
            StateManager.DrawCenteredString(wins.ToString() + "%", r / 2 + margin, 0);
            StateManager.DrawCenteredString(draws.ToString() + "%", Size.X / 2, 0);
            StateManager.DrawCenteredString(losses.ToString() + "%", Size.X - 17 - r / 2 - margin, 0);

            StateManager.Translate(0, r / 2);
            StateManager.DrawCenteredString("Gewonnen", r / 2 + margin, 20);
            StateManager.DrawCenteredString("Unentschieden", Size.X / 2, 20);
            StateManager.DrawCenteredString("Verloren", Size.X - 17 - r / 2 - margin, 20);

            StateManager.Translate(0, 50);
            c1 = Color.FromArgb(100, 50, 50, 50);
            c2 = Color.FromArgb(0, 50, 50, 50);
            StateManager.SetColor(Color.White);
            StateManager.FillRect(0, 0, Size.X, 50);
            StateManager.SetColor(Color.Black);
            StateManager.DrawString("Spiele gespielt: ", 20, 12);
            float width = StateManager.GetStringWidth(games.ToString());
            StateManager.DrawString(games.ToString(), Size.X - 17 - 30 - width, 12);

            StateManager.Translate(0, 49);
            StateManager.SetColor(Color.White);
            StateManager.FillRect(0, 0, Size.X, 50);
            StateManager.FillGradientRect(new Vector(0, 0), new Vector(Size.X, 3), c1, c2, 90);
            StateManager.SetColor(Color.Black);
            StateManager.DrawString("Perfekte Spiele: ", 20, 12);
            width = StateManager.GetStringWidth(stats.PerfectGames.ToString());
            StateManager.DrawString(stats.PerfectGames.ToString(), Size.X - 17 - 30 - width, 12);

            StateManager.Pop();
        }
    }
}
