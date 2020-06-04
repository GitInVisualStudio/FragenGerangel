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
            Components.Add(new GuiButton("Antwort1")
            {
                Size = new Vector(0, 0),
                Location = new Vector(5, 5),
                RY = 0.5f,
                RWidth = 0.5f,
                RHeight = 0.15f,
                FontColor = Color.White
            });
            Components.Add(new GuiButton("Antwort2")
            {
                Size = new Vector(0, 0),
                Location = new Vector(5, 10),
                RY = 0.65f,
                RWidth = 0.5f,
                RHeight = 0.15f,
                FontColor = Color.White
            });
            //Components.Add(new GuiButton("Antwort2")
            //{
            //    Size = new Vector(-27, 100),
            //    Location = new Vector(5, -150),
            //    RY = 1,
            //    RWidth = 1,
            //    FontColor = Color.White
            //});
            //Components.Add(new GuiButton("Antwort2")
            //{
            //    Size = new Vector(-27, 100),
            //    Location = new Vector(5, -150),
            //    RY = 1,
            //    RWidth = 1,
            //    FontColor = Color.White
            //});
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
        }

    }
}
