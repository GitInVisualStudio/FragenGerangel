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
    public class GuiTextBox : GuiComponent
    {
        private Animation animation;
        private float time;
        private string text = "";
        private string token;

        public string Text { get => text; set => text = value; }

        public GuiTextBox(string name)
        {
            this.Name = name;
            OnKeyPress += GuiTextBox_OnKeyPress;
            animation = new Animation(3);
            animation.Fire();
        }

        private void GuiTextBox_OnKeyPress(object sender, char e)
        {
            if (e != 8)
            {
                if(char.IsLetterOrDigit(e) && e < 122)
                    text += e;
            }
            else if (text.Length >= 1)
                text = text.Substring(0, text.Length - 1);
        }

        public override void OnRender()
        {
            if (animation.Finished && Selected && animation.Incremental)
                animation.Reverse();
            if (animation.Finished && !Selected && !animation.Incremental && text.Length == 0)
                animation.Reverse();
            time += StateManager.delta;
            if (Selected && time >= 0.5)
            {
                time = 0;
                token = token == "_" ? " " : "_";
            }
            else if (!Selected)
                token = "";
            string renderString = text + token;
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            StateManager.SetColor(Color.White);
            StateManager.FillRoundRect(Location, Size, 10);

            Font font = new Font("Arial", 10.0f + animation.Delta * 2, FontStyle.Bold);
            Vector size = StateManager.GetStringSize(Name);
            Vector var1 = Location + new Vector(0, Size.Y / 2) * animation.Delta;
            StateManager.SetFont(font);
            StateManager.SetColor(Color.Gray);
            StateManager.DrawString(Name, var1.X + 5, var1.Y);
            StateManager.SetColor(Color.Black);
            font = new Font("Arial", 12, FontStyle.Bold);
            StateManager.SetFont(font);
            StateManager.DrawString(renderString, Location.X + 5, Location.Y + Size.Y / 2);
        }
    }
}
