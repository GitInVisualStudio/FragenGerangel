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
            StateManager.SetFont(new Font("Arial", 12, FontStyle.Bold));
            StateManager.SetColor(CurrentColor);
            StateManager.FillRoundRect(Location, Size, 15);
            StateManager.SetColor(100, 0, 200, 150);
            StateManager.FillCircle(Location.X + 40, Location.Y + Size.Y / 2, 65);
            RenderUtils.DrawPlayer("", new Vector(Location.X + 40, Location.Y + Size.Y / 2), 50, false);
            StateManager.SetColor(Color.Black);
            float height = StateManager.GetStringHeight(game.RemotePlayer.Name);
            StateManager.DrawString(game.RemotePlayer.Name, Location.X + 90, Location.Y + Size.Y / 2 - height / 2);
            StateManager.DrawString(game.RemotePlayer.Name + " " + game.ScoreRemotePlayer + ":" + game.ScorePlayer + " " + Globals.Player.Name, Location.X + 90, Location.Y + Size.Y / 2 + height / 2);
            StateManager.FillRect(Location.X - 10 + 90, Location.Y + Size.Y / 2 - height / 2, 2, height * 2);
            string text = Remote ? "Warte auf " + game.RemotePlayer.Name : "Du bist dran";
            StateManager.DrawCenteredString(text + "!", Location + Size / 2);
        }
    }
}
