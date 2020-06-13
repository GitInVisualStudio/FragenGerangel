using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    public class GuiSearch : GuiTextBox
    {
        public event EventHandler<string> OnTextChange;

        public GuiSearch(string titel) : base(titel)
        {

        }

        protected override void GuiTextBox_OnKeyPress(object sender, char e)
        {
            base.GuiTextBox_OnKeyPress(sender, e);
            OnTextChange?.Invoke(this, Text);
        }

        public override void OnRender()
        {
            base.OnRender();
        }
    }
}
