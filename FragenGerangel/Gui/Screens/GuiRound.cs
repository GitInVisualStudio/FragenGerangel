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
    public class GuiRound : GuiScreen
    {
        private int correct;
        private string[] answers;
        private string[] original;
        private bool answered = false;
        private string category;
        private string question;
        private QuestionAnswer questionAnswer;

        public event EventHandler<int> OnRoundClose;

        public string[] Answers { get => answers; set => answers = value; }
        public int Answer;
        public bool Answered { get => answered; set => answered = value; }
        public QuestionAnswer QuestionAnswer { get => questionAnswer; set => questionAnswer = value; }

        public GuiRound(QuestionAnswer questions) : base()
        {
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
                GuiButton button = (GuiButton)sender;
                Answer = original.ToList().IndexOf(button.Name);
                QuestionAnswer.AnswerPlayer = Answer;
                Globals.APIManager.UploadQuestionAnswer(QuestionAnswer).Wait();
                Answered = true;
                if (button.Name == original[correct])
                {
                    button.CurrentColor = Color.LawnGreen;
                }
                else
                {
                    button.CurrentColor = Color.Red;
                }
                GetComponent<GuiButton>("Weiter").CurrentColor = Color.LawnGreen;
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
        }

    }
}
