using FragenGerangel.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiTestScreen : GuiScreen
    {
        public GuiTestScreen() : base()
        {
            Components.Add(new GuiButton("Nutte")
            {
                Location = new Vector(100, 100),
                Size = new Vector(100, 100),
                BackColor = Color.Blue
            });
        }

        public override void OnRender()
        {
            base.OnRender();
        }
    }
}
