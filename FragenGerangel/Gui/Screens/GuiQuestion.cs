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
    public class GuiQuestion : GuiScreen
    {
        public GuiQuestion() : base()
        {

        }

        public override void Init()
        {
            Components.Add(new GuiButton("Weiter")
            {
                Size = new Vector(-27, 100),
                Location = new Vector(5, -150),
                RY = 1,
                RWidth = 1,
                FontColor = Color.White
            });
            base.Init();
        }

        public override void OnRender()
        {
            base.OnRender();

            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);

            float height = Size.Y / 5;
            StateManager.FillGradientRect(Location, new Vector(Size.X, height), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.FillRoundRect(5, height / 2, Size.X - 27, Size.Y / 5 * 2);

            height = height / 2 + Size.Y / 5 * 2;
            float diff = Components[0].Location.Y - height;
            StateManager.FillRoundRect(5, height + 5, (Size.X - 10) / 2, diff / 2 - 10);
            StateManager.FillRoundRect(5, height + diff / 2 + 5, (Size.X - 10) / 2, diff / 2 - 10);
        }

    }
}
