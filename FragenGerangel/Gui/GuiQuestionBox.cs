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
    public class GuiQuestionBox : GuiPanel
    {

        public override void Init()
        {
            //Components.Add(new GuiButton("TEST")
            //{
            //    Location = new Vector(5, 5),
            //    Size = Size / 2
            //});
            //Components.Add(new GuiButton("TEST")
            //{
            //    Location = new Vector(5, 5) + Size / 2,
            //    Size = Size / 2
            //});
            //Components.Add(new GuiButton("TEST")
            //{
            //    Location = new Vector(5 + Size.X / 2, 5),
            //    Size = Size / 2
            //});
            //Components.Add(new GuiButton("TEST")
            //{
            //    Location = new Vector(5, 5 + Size.Y / 2),
            //    Size = Size / 2
            //});
            base.Init();
        }

        public override void OnRender()
        {
            base.OnRender();
            StateManager.SetColor(Color.Black);
            StateManager.FillRect(Location, Size);
        }

    }
}
