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
    public class GuiRound : GuiScreen
    {
        private int correct;
        private string[] answers;
        private string[] original;
        private bool answered = false;
        private string category;
        private bool renderEnemyAnswer;
        private string question;
        private QuestionAnswer questionAnswer;

        public event EventHandler<int> OnRoundClose;
        private Game game;
        public string[] Answers { get => answers; set => answers = value; }
        public int Answer;
        public bool Answered { get => answered; set => answered = value; }
        public QuestionAnswer QuestionAnswer { get => questionAnswer; set => questionAnswer = value; }

        public GuiRound(QuestionAnswer questions, Game game) : base()
        {
            this.game = game;
            QuestionAnswer = questions;
            answers = questions.Question.Answers;
            question = questions.Question.Q;
            category = questions.Question.Category;
        }

        public override void Init()
        {
            List<string> var1 = new List<string>();
            Random ra = new Random();
            for (int i = 0; i < 4; i++)
            {
                string var2 = null;
                while (var1.Count < 4)
                {
                    if (!var1.Contains(var2 = answers[ra.Next(4)]))
                        var1.Add(var2);
                }
            }
            original = answers;
            answers = var1.ToArray();
            correct = var1.IndexOf(answers[0]);
            Components.Add(new GuiButton("Weiter")
            {
                Size = new Vector(-27, 100),
                Location = new Vector(5, -150),
                RY = 1f,
                RWidth = 1,
                FontColor = Color.White,
                BackColor = Color.Gray
            });
            float margin = 5f;
            Components.Add(new GuiButton(Answers[0])
            {
                Size = new Vector(-margin, -150 / 2f - margin * 2),
                Location = new Vector(margin, margin),
                RY = 1 / 2f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });
            Components.Add(new GuiButton(Answers[1])
            {
                Size = new Vector(-27 - margin, -150 / 2f - margin * 2),
                Location = new Vector(margin * 2, margin),
                RY = 1 / 2f,
                RX = 1 / 2f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });
            Components.Add(new GuiButton(Answers[2])
            {
                Size = new Vector(-margin, -150 / 2f - margin * 2),
                Location = new Vector(margin, -75 + margin),
                RY = 1 / 2f + 1 / 4f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });
            Components.Add(new GuiButton(Answers[3])
            {
                Size = new Vector(-27 - margin, -150 / 2f - margin * 2),
                Location = new Vector(margin * 2, -75 + margin),
                RY = 1 / 2f + 1 / 4f,
                RX = 1 / 2f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });

            foreach(GuiComponent component in Components)
                component.OnClick += ScreenClicked;

            base.Init();
        }

        private void ScreenClicked(object sender, Vector e)
        {
            if(((GuiButton)sender).Name == "Weiter")
            {
                if (answered)
                {
                    OnRoundClose?.Invoke(this, Answer);
                    return;
                }
                return;
            }
            if (sender is GuiButton && !Answered)
            {
                renderEnemyAnswer = true;
                GuiButton button = (GuiButton)sender;
                Answer = original.ToList().IndexOf(button.Name);
                QuestionAnswer.AnswerPlayer = Answer;
                Globals.APIManager.UploadQuestionAnswer(QuestionAnswer).Wait();
                Answered = true;
                if (button.Name == original[correct])
                {
                    button.BackColor = Color.LawnGreen;
                }
                else
                {
                    button.BackColor = Color.Red;
                }
                if (QuestionAnswer.AnswerRemotePlayer != -1)
                {
                    button = GetComponent<GuiButton>(original[QuestionAnswer.AnswerRemotePlayer]);
                    if (button.Name == original[correct])
                    {
                        button.BackColor = Color.LawnGreen;
                    }
                    else
                    {
                        button.BackColor = Color.Red;
                    }
                }
                GetComponent<GuiButton>("Weiter").BackColor = Color.LawnGreen;
            }
        }

        public override void OnRender()
        {
            base.OnRender();

            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);

            float height = Size.Y / 5;
            StateManager.SetColor(Color.White);
            StateManager.FillRoundRect(5, Size.Y / 10f, Size.X - 27, Size.Y / 5 * 2);
            StateManager.SetColor(Color.Black);
            StateManager.DrawCenteredString(question, Size.X / 2, Size.Y / 10 + Size.Y / 5);
            float width = StateManager.GetStringWidth(category);
            StateManager.SetColor(Color.MediumPurple);
            StateManager.FillRoundRect(Size.X / 2 - width / 2 - 20, height - 5, width + 40, 50, 5);
            StateManager.SetColor(Color.White);
            StateManager.DrawCenteredString(category, Size.X / 2, height + 20);
            StateManager.FillGradientRect(Location, new Vector(Size.X, height), c1, c2);

            StateManager.Push();
            StateManager.Translate(Size.X / 2, height / 2);
            StateManager.Rotate(45);
            width = 70;
            StateManager.FillRoundRect(width / -2, width / -2, width, width, 15);
            StateManager.Rotate(-45);
            StateManager.SetColor(c1);
            StateManager.SetFont(new Font("Arial", 30, FontStyle.Bold));
            StateManager.DrawCenteredString(game.Rounds.ToList().IndexOf(game.LastRound).ToString(), 0, 0);
            StateManager.Pop();
            StateManager.SetColor(c1.R, c1.G, c1.B, 100);
            StateManager.FillCircle(Size.X - 150, height / 2, 70);
            RenderUtils.DrawPlayer(game.RemotePlayer.Name, new Vector(Size.X - 150, height / 2), 60);

            if (renderEnemyAnswer)
            {
                if(QuestionAnswer.AnswerRemotePlayer != -1)
                {
                    GuiButton button = GetComponent<GuiButton>(original[QuestionAnswer.AnswerRemotePlayer]);
                    RenderUtils.DrawPlayer(game.RemotePlayer.Name, new Vector(button.Location.X + button.Size.X / 3, button.Location.Y), 35, false);
                    StateManager.SetColor(Color.White);
                    StateManager.SetFont(FontUtils.DEFAULT_FONT);
                    StateManager.DrawCenteredString(game.RemotePlayer.Name, button.Location.X + button.Size.X / 3, button.Location.Y + 25);
                }
            }

        }

    }
}
