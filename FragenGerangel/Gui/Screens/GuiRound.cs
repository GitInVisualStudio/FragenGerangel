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

        private string question;
        private string category;
        private string[] answers;
        private bool[] stats;
        private int round, correct;
        private bool answered = false;

        public GuiRound(string category, string question, string[] answers, bool[] stats, int round, int correct) : base()
        {
            this.round = round;
            this.correct = correct;
            this.question = question;
            this.category = category;
            this.answers = answers;
            this.stats = stats;
        }

        public override void Init()
        {
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
            Components.Add(new GuiButton(answers[0])
            {
                Size = new Vector(-margin, -150 / 2f - margin * 2),
                Location = new Vector(margin, margin),
                RY = 1 / 2f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });
            Components.Add(new GuiButton(answers[1])
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
            Components.Add(new GuiButton(answers[2])
            {
                Size = new Vector(-margin, -150 / 2f - margin * 2),
                Location = new Vector(margin, -75 + margin),
                RY = 1 / 2f + 1 / 4f,
                RWidth = 1 / 2f,
                RHeight = 1 / 4f,
                FontColor = Color.White,
                BackColor = Color.FromArgb(30, 80, 150)
            });
            Components.Add(new GuiButton(answers[3])
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
            if (sender is GuiButton && !answered)
            {
                GuiButton button = (GuiButton)sender;
                if (!answers.Contains(button.Name))
                {
                    if(!answered)
                        return;
                    //TODO: go to the next round
                    return;
                }
                answered = true;
                if (button.Name == answers[correct])
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
            StateManager.FillGradientRect(Location, new Vector(Size.X, height), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.FillRoundRect(5, Size.Y / 10f, Size.X - 27, Size.Y / 5 * 2);
        }

    }
}
