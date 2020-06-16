using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FragenGerangel.Gui
{
    /// <summary>
    /// eine box in welche man schreiben kann
    /// </summary>
    public class GuiTextBox : GuiComponent
    {
        protected Animation animation;
        protected float time;
        protected string text = "";
        protected string token;
        public event EventHandler<string> OnTextChange;

        /// <summary>
        /// geschriebener text
        /// </summary>
        public string Text { get => text; set => text = value; }

        /// <summary>
        /// name der box
        /// </summary>
        /// <param name="name"></param>
        public GuiTextBox(string name)
        {
            this.Name = name;
            OnKeyPress += GuiTextBox_OnKeyPress;
            animation = new Animation(3);
            animation.Fire();
        }

        /// <summary>
        /// verarbeitet den input und verändert den text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GuiTextBox_OnKeyPress(object sender, char e)
        {
            string prev = text;
            if (e != 8)
            {
                if (char.IsLetterOrDigit(e) && e < 122)
                    text += e;
            }
            else if (text.Length >= 1)
                text = text.Substring(0, text.Length - 1);
            if(prev != text)
                OnTextChange?.Invoke(this, Text);
        }

        /// <summary>
        /// zeichnet die box
        /// </summary>
        public override void OnRender()
        {
            if (animation.Finished && Selected && animation.Incremental)
                animation.Reverse();
            if (animation.Finished && !Selected && !animation.Incremental && text.Length == 0)
                animation.Reverse();
            time += StateManager.delta;
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

            if (StateManager.GetStringWidth(text) + 10 > Size.X)
                text = text.Substring(0, text.Length - 1);

            if (Selected && time >= 0.5)//jede 0.5 sek wird '_' dem text hinzugefügt
            {
                time = 0;
                token = token == "_" ? " " : "_";
            }
            else if (!Selected)
                token = "";
            string renderString = text + token;

            StateManager.DrawString(renderString, Location.X + 5, Location.Y + Size.Y / 2);
        }
    }
}
