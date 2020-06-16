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
    /// wie textbox nur verdeckt
    /// </summary>
    public class GuiPasswordBox : GuiTextBox
    {
        public GuiPasswordBox(string name) : base(name)
        {
        }

        public override void OnRender()
        {
            if (animation.Finished && Selected && animation.Incremental)
                animation.Reverse();
            if (animation.Finished && !Selected && !animation.Incremental && text.Length == 0)
                animation.Reverse();
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
            font = new Font("Arial", 15);
            StateManager.SetFont(font);

            if (StateManager.GetStringWidth(text) + 10 > Size.X)
                text = text.Substring(0, text.Length - 1);

            string renderString = "";
            foreach (char c in text)//zeichnen eines gleichlangen verdeckten strings
                renderString += "*";

            StateManager.DrawString(renderString, Location.X + 5, Location.Y + Size.Y / 2);
        }
    }
}
