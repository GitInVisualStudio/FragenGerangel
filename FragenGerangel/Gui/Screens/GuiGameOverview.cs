using FragenGerangel.GameBase;
using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiGameOverview : GuiScreen
    {
        private Game game;
        private FragenGerangel fragenGerangel;
        private GuiRound round;
        private int index;

        public GuiGameOverview(FragenGerangel fragenGerangel, Game game) : base()
        {
            this.game = game;
            this.fragenGerangel = fragenGerangel;
        }

        protected override void Panel_OnKeyRelease(object sender, char e)
        {
            if (e == 27)
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));

            base.Panel_OnKeyRelease(sender, e);
        }

        private void Round_OnClose(object sender, int e)
        {
            if(index < 3)
                game.LastRound.Questions[index].AnswerPlayer = e;
            index++;
            if (index < 3)
            {
                round = new GuiRound(game.LastRound.Questions[index], game);
                round.OnRoundClose += Round_OnClose;
                fragenGerangel.OpenScreen(round);
            }
            else
            {
                GuiRound round = (GuiRound)sender;
                new Thread(() =>
                {
                    Globals.APIManager.GetGame(game).Wait();
                    fragenGerangel.OpenScreen(new GuiGameOverview(fragenGerangel, game));
                }).Start();
            }
        }

        public override void Init()
        {
            //Globals.APIManager.GetGame(game).Wait();
            if (!game.Active)
            {
                Components.Add(new GuiButton("Zurück")
                {
                    Size = new Vector(200, 100),
                    Location = new Vector(-100, -150),
                    RX = 0.5f,
                    RY = 1,
                    FontColor = Color.White
                });
                animation.Speed = 0.6f;
                base.Init();
                GetComponent<GuiButton>("Zurück").OnClick += (object sender, Vector e) =>
                {
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                };
                return;
            }
            Components.Add(new GuiButton(IsRemoteTurn() ? "Zurück" : "SPIELEN")
            {
                Size = new Vector(200, 100),
                Location = new Vector(-100, -150),
                RX = 0.5f,
                RY = 1,
                FontColor = Color.White,
                BackColor = IsRemoteTurn() ? Color.Gray : Color.LawnGreen
            });
            GetComponent<GuiButton>(IsRemoteTurn() ? "Zurück" : "SPIELEN").OnClick += (object sender, Vector e) =>
            {
                if (IsRemoteTurn())
                {
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                    return;
                }
                if (game.LastRound.Category == null)
                {
                    GuiCategory category = new GuiCategory(game);
                    bool flag = true;
                    category.OnClose += (object s, EventArgs args) =>
                    {
                        new Thread(() =>
                        {
                            if (!flag)
                                return;
                            flag = false;
                            category.animation.Finished = true;
                            category.animation.Incremental = true;
                            Globals.APIManager.ChooseCategory(game, category.Category + 1).Wait();
                            Task task = Globals.APIManager.GetGame(game);
                            task.Wait();
                            round = new GuiRound(game.LastRound.Questions[0], game);
                            round.OnRoundClose += Round_OnClose;
                            fragenGerangel.OpenScreen(round);
                        }).Start();
                    };
                    fragenGerangel.OpenScreen(category);
                    return;
                }
                int index = 0;
                for(int i = 0; i < game.LastRound.Questions.Length; i++)
                    if(game.LastRound.Questions[i].AnswerPlayer == -1)
                    {
                        this.index = i;
                        index = i;
                        break;
                    }
                round = new GuiRound(game.LastRound.Questions[index], game);
                round.OnRoundClose += Round_OnClose;
                fragenGerangel.OpenScreen(round);
            };
            animation.Speed = 0.6f;
            base.Init();
        }

        public override void OnRender()
        {
            base.OnRender();
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
            //StateManager.SetFont(FontUtils.DEFAULT_FONT);
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, offset / 2);

            StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X, offset * 2), c1, c2);

            StateManager.SetFont(new Font("comfortaa", 40));
            string score = game.ScorePlayer + " - " + game.ScoreRemotePlayer;
            StateManager.DrawCenteredString(score, Size.X / 2, offset * 2 + offset / 2);

            float width = StateManager.GetStringWidth(score);

            StateManager.SetColor(Color.White);
            float var1 = Size.X / 2 - width;
            float var2 = Size.X / 2 + width;
            RenderUtils.DrawPlayer(Globals.Player.Name, new Vector(var1 / 2 + 50, offset * 3), 100);
            RenderUtils.DrawPlayer(game.RemotePlayer.Name, new Vector(var2 + var1 / 2 - 50, offset * 3), 100);
            RenderTable();
        }

        private bool IsRemoteTurn()
        {
            if (game.LastRound.Category == null)
                return false;
            foreach (QuestionAnswer q in game.LastRound.Questions)
                if (q.AnswerPlayer == -1)
                    return false;
            return true;
        }

        private void RenderTable()
        {
            int offset = 250;
            bool flag = true;
            for(int i = 0; i < 6; i++)
            {
                StateManager.Push();
                StateManager.Translate(Size.X / 2, offset);
                float scale = animation.Delta * 2f - i * 0.1f;
                if (scale < 0)
                {
                    StateManager.Pop();
                    continue;
                }
                if (scale > 1)
                    scale = 1;
                StateManager.Scale(scale);
                StateManager.Translate(-Size.X / 2 * scale, -offset * scale);
                Color c1 = Color.FromArgb(100, 50, 50, 50);
                Color c2 = Color.FromArgb(0, 50, 50, 50);

                StateManager.SetColor(Color.White);
                StateManager.FillRect(0, offset, Size.X, 50);
                StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X, 3), c1, c2, 90);
                StateManager.Push();
                StateManager.Translate(Size.X / 2, offset);
                StateManager.Rotate(45);
                StateManager.FillRoundRect(-12,-12, 24, 24, 8);
                StateManager.SetColor(Color.Gray);
                StateManager.FillRoundRect(-10, -10, 20, 20, 7);
                StateManager.Pop();
                StateManager.Pop();
                StateManager.Push();
                StateManager.Translate(0, offset);
                bool var1 = true;
                if(flag)
                    var1 = RenderRound(game.Rounds[i]);
                StateManager.Pop();

                Vector size = StateManager.GetStringSize((i + 1).ToString());
                StateManager.SetColor(Color.White);
                StateManager.DrawString((i + 1).ToString(), Size.X / 2 - size.X / 2 + 1, offset - size.Y / 2 + 1);
                if (flag)
                {
                    StateManager.SetColor(Color.Black);
                    StateManager.DrawCenteredString(game.Rounds[i].Category, Size.X / 2 , offset + 23);
                }
                offset += 50;
                if(flag)
                    flag = var1;
            }
        }

        private bool RenderRound(Round round)
        {
            bool flag = true;
            if(round.Questions == null)
            {
                Vector size = StateManager.GetStringSize("DU BIST DRAN");
                StateManager.SetColor(Color.DarkOrange);
                StateManager.FillRoundRect(Size.X / 4 - size.X / 2, 25 / 2f, size.X, size.Y);
                StateManager.SetColor(Color.White);
                StateManager.DrawCenteredString("DU BIST DRAN", Size.X / 4, 25);
                return false;
            }
            float offset = 0;
            for(int i = 0; i < round.Questions.Length; i++)
            {
                QuestionAnswer question = round.Questions[i];
                if (question.AnswerPlayer == -1)
                {
                    Vector size = StateManager.GetStringSize("DU BIST DRAN");
                    StateManager.SetColor(Color.DarkOrange);
                    StateManager.FillRoundRect(Size.X / 4 - size.X / 2, 25 / 2f, size.X, size.Y);
                    StateManager.SetColor(Color.White);
                    StateManager.DrawCenteredString("DU BIST DRAN", Size.X / 4, 25);
                    flag = false;
                    break;
                }
                bool right = question.AnswerPlayer == 0;
                StateManager.SetColor(right ? Color.LawnGreen : Color.Red);
                float width = 25;
                StateManager.FillRoundRect(Size.X / 4 - width * 2 + offset, width / 2, width, width);
                offset += width;
            }
            offset = 0;
            for (int i = 0; i < round.Questions.Length; i++)
            {
                QuestionAnswer question = round.Questions[i];
                if (question.AnswerRemotePlayer == -1)
                {
                    if (!flag)
                        return false;
                    Vector size = StateManager.GetStringSize("Warte auf " + game.RemotePlayer.Name);
                    StateManager.SetColor(Color.DarkOrange);
                    StateManager.FillRoundRect(Size.X / 4 * 3- size.X / 2, 25 / 2f, size.X, size.Y);
                    StateManager.SetColor(Color.White);
                    StateManager.DrawCenteredString("Warte auf " + game.RemotePlayer.Name, Size.X / 4 * 3, 25);
                    return false;
                }
                if (question.AnswerPlayer == -1)
                    return false;
                bool right = question.AnswerRemotePlayer == 0;
                StateManager.SetColor(right ? Color.LawnGreen : Color.Red);
                float width = 25;
                StateManager.FillRoundRect(Size.X / 4 * 3 - width + offset, width / 2, width, width);
                offset += width;
            }
            return flag;
        }
    }
}
