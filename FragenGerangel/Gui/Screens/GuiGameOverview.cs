using FragenGerangel.GameBase;
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
    public class GuiGameOverview : GuiScreen
    {
        private Game game;

        public GuiGameOverview(Game game) : base()
        {
            this.game = game;
            Components.Add(new GuiButton("SPIELEN")
            {
                Size = new Vector(200, 100),
                Location = new Vector(-100, -150),
                RX = 0.5f,
                RY = 1,
                FontColor = Color.White
            });
            animation.Speed = 0.6f;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Open()
        {
            base.Open();
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
            string score = "0 - 0";
            StateManager.DrawCenteredString(score, Size.X / 2, offset * 2 + offset / 2);

            float width = StateManager.GetStringWidth(score);

            StateManager.SetColor(Color.White);
            float var1 = Size.X / 2 - width;
            float var2 = Size.X / 2 + width;
            RenderUtils.DrawPlayer("player1", new Vector(var1 / 2 + 50, offset * 3), 100);
            RenderUtils.DrawPlayer("player2", new Vector(var2 + var1 / 2 - 50, offset * 3), 100);
            RenderTable();
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
                if(flag)
                    flag = RenderRound(new Round(-1, "asdasd", "asdasd", "asdawsd"));
                StateManager.Pop();

                Vector size = StateManager.GetStringSize(i.ToString());
                StateManager.SetColor(Color.White);
                StateManager.DrawString(i.ToString(), Size.X / 2 - size.X / 2 + 1, offset - size.Y / 2 + 1);
                offset += 50;
            }
        }

        private bool RenderRound(Round round)
        {
            if(round.Questions == null)
            {
                Vector size = StateManager.GetStringSize("DU BIST DRAN");
                StateManager.SetColor(Color.DarkOrange);
                StateManager.FillRoundRect(Size.X / 4 - size.X / 2, 25 / 2f, size.X, size.Y);
                StateManager.SetColor(Color.White);
                StateManager.DrawCenteredString("DU BIST DRAN", Size.X / 4, 25);
                return false;
            }

            for(int i = 0; i < round.Questions.Length; i++)
            {
                QuestionAnswer question = round.Questions[i];
                if (question.AnswerPlayer == -1)
                {
                    Vector size = StateManager.GetStringSize("DU BIST DRAN");
                    StateManager.FillRoundRect(Size.X / 4 - size.X / 2, 0, size.X, size.Y);
                    StateManager.DrawCenteredString("DU BIST DRAN", Size.X / 4, 0);
                    return false;
                }
            }
            for (int i = 0; i < round.Questions.Length; i++)
            {
                QuestionAnswer question = round.Questions[i];
                if (question.AnswerRemotePlayer == -1)
                {
                    StateManager.DrawCenteredString("Warte", Size.X / 4 * 3, 0);
                    return false;
                }
            }
            return true;
        }
    }
}
