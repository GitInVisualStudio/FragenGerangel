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

namespace FragenGerangel.Gui
{
    /// <summary>
    /// Zeigt spielinformationen
    /// </summary>
    public class GuiGameInfo : GuiButton
    {
        private Game game;
        private bool remote; //true = remotes turn
        public event EventHandler<bool> InfoClick; //wenn information angeklickt wird

        /// <summary>
        /// game instanz für informationen über das spiel
        /// </summary>
        /// <param name="game"></param>
        public GuiGameInfo(Game game) : base("")
        {
            Game = game;
            OnClick += GuiGameInfo_OnClick;
            if (!game.Active)
                return;
            Remote = !game.YourTurn;
        }

        /// <summary>
        /// wird aufgerufen wenn die information gedrückt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuiGameInfo_OnClick(object sender, Vector e)
        {
            InfoClick?.Invoke(this, false);
        }

        public Game Game { get => game; set => game = value; }
        public bool Remote { get => remote; set => remote = value; }

        /// <summary>
        /// Zeichnet informationen zum spiel
        /// </summary>
        public override void OnRender()
        {
            var1 += (var2 - var1) * StateManager.delta * 10;
            int r = CurrentColor.R - (int)(CurrentColor.R * (var1));
            int g = CurrentColor.G - (int)(CurrentColor.G * (var1));
            int b = CurrentColor.B - (int)(CurrentColor.B * (var1));
            StateManager.SetFont(new Font("Arial", 12, FontStyle.Bold));
            StateManager.SetColor(0, 0, 0, 50);
            StateManager.FillRoundRect(Location, Size + new Vector(4, 4), 15);
            StateManager.SetColor(r, g, b);
            StateManager.FillRoundRect(Location, Size, 15);
            StateManager.SetColor(100, 0, 200, 150);
            StateManager.FillCircle(Location.X + 40, Location.Y + Size.Y / 2, 65);
            RenderUtils.DrawPlayer("", new Vector(Location.X + 40, Location.Y + Size.Y / 2), 50, false);
            StateManager.SetColor(Color.Black);
            float height = StateManager.GetStringHeight(game.RemotePlayer.Name);
            StateManager.DrawString(game.RemotePlayer.Name, Location.X + 90, Location.Y + Size.Y / 2 - height / 2);
            StateManager.DrawString(game.RemotePlayer.Name + " " + game.ScoreRemotePlayer + ":" + game.ScorePlayer + " " + Globals.Player.Name, Location.X + 90, Location.Y + Size.Y / 2 + height / 2);
            StateManager.FillRect(Location.X - 10 + 90, Location.Y + Size.Y / 2 - height / 2, 2, height * 2);
            string text = (Remote ? "Warte auf " + game.RemotePlayer.Name : "Du bist dran") + "!";
            if (!game.Active) //wenn spiel zu ende gibt es den gewinner aus
                text = game.ScoreRemotePlayer > game.ScorePlayer ? "Du hast verloren! :(" : game.ScorePlayer == game.ScoreRemotePlayer ? "Unentschieden!" : "Du hast gewonnen! :)";
            StateManager.DrawCenteredString(text, Location + Size / 2);
        }
    }
}
