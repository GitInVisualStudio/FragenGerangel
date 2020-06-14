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
    public class GuiGameInfo : GuiButton
    {
        private Game game;
        private bool remote; //true = remotes turn
        public event EventHandler<bool> InfoClick;

        public GuiGameInfo(Game game) : base("")
        {
            Game = game;
            OnClick += GuiGameInfo_OnClick;
            Round round = game.LastRound;
            Remote = true;
            if (round.Questions == null)
                Remote = false;
            else
            {
                foreach (QuestionAnswer question in round.Questions)
                    if (question.AnswerPlayer == -1)
                        Remote = false;
            }
        }

        private void GuiGameInfo_OnClick(object sender, Vector e)
        {
            InfoClick?.Invoke(this, false);
        }

        public Game Game { get => game; set => game = value; }
        public bool Remote { get => remote; set => remote = value; }

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
            if (!game.Active)
                text = game.ScoreRemotePlayer > game.ScorePlayer ? "Du hast verloren! :(" : game.ScorePlayer == game.ScoreRemotePlayer ? "Unentschieden!" : "Du hast gewonnen! :)";
            StateManager.DrawCenteredString(text, Location + Size / 2);
        }
    }
}
