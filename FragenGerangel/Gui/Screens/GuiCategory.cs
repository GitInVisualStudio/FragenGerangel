using FragenGerangel.GameBase;
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
    public class GuiCategory : GuiScreen
    {
        private string[] categories;

        public GuiCategory(Round round) : base()
        {
            //categories = round.PossibleCategories;
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OnRender()
        {
            base.OnRender();
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
            //StateManager.SetFont(FontUtils.DEFAULT_FONT);
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, offset / 2);

            StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X, offset * 2), c1, c2);

            StateManager.SetFont(new Font("comfortaa", 40));
            string score = "0 - 0";
            StateManager.DrawCenteredString(score, Size.X / 2, offset * 2 + offset / 2);

            float width = StateManager.GetStringWidth(score);

            StateManager.SetColor(Color.White);
            float var1 = Size.X / 2 - width;
            float var2 = Size.X / 2 + width;
            RenderUtils.DrawPlayer("player1", new Vector(var1 / 2 + 50, offset * 3), 100);
            RenderUtils.DrawPlayer("player2", new Vector(var2 + var1 / 2 - 50, offset * 3), 100);

            StateManager.SetColor(Color.White);
            StateManager.FillCircle(Size.X / 2, Size.Y / 2, Size.Y / 2);
        }

    }
}
