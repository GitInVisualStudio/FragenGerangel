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
    public class GuiGameOverview : GuiScreen
    {
        public GuiGameOverview() : base()
        {
            Components.Add(new GuiButton("SPIELEN")
            {
                Size = new Vector(200, 100),
                Location = new Vector(-100, -150),
                RX = 0.5f,
                RY = 1
            });
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

            StateManager.DrawCenteredString("0 - 0", Size.X / 2, offset * 2 + offset / 2);

            float width = StateManager.GetStringWidth("0 - 0");

            StateManager.SetColor(Color.White);
            float var1 = Size.X / 2 - width;
            float var2 = Size.X / 2 + width;

            StateManager.FillCircle(var1 / 2, offset * 3, 100);
            StateManager.FillCircle(var2 + var1 / 2, offset * 3, 100);

            StateManager.SetColor(Color.Black);
            StateManager.SetFont(new Font("comfortaa", 15));
            StateManager.DrawCenteredString("player1", var1 / 2, offset * 3 + 65);
            StateManager.DrawCenteredString("player2", var2 + var1 / 2, offset * 3 + 65);

            RenderTable();
        }

        private void RenderTable()
        {
            int offset = 250;
            for(int i = 0; i < 6; i++)
            {
                Color c1 = Color.FromArgb(100, 50, 50, 50);
                Color c2 = Color.FromArgb(0, 50, 50, 50);

                StateManager.SetColor(Color.White);
                StateManager.FillRect(0, offset, Size.X, 50);
                StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X, 3), c1, c2, 90);
                StateManager.Push();
                StateManager.Translate(Size.X / 2, offset);
                StateManager.Rotate(45);
                StateManager.FillRoundRect(-12,-12, 24, 24, 8);
                StateManager.SetColor(Color.Gray);
                StateManager.FillRoundRect(-10, -10, 20, 20, 7);
                StateManager.Pop();
                Vector size = StateManager.GetStringSize(i.ToString());
                StateManager.DrawString(i.ToString(), Size.X / 2 - size.X / 2 + 1, offset - size.Y / 2 + 1);
                offset += 50;
            }
        }
    }
}
