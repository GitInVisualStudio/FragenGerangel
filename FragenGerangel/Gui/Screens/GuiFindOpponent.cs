using FragenGerangel.GameBase;
using FragenGerangel.Utils;
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
    public class GuiFindOpponent : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private Player[] players;

        public GuiFindOpponent(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
            Components.Add(new GuiSearch("Suche")
            {
                Location = new Vector(20, 75),
                RWidth = 1,
                Size = new Vector(-50, 50)
            });

            players = new Player[1];
            players[0] = new Player("Nutte");

            GetComponent<GuiSearch>("Suche").OnTextChange += GuiFindOpponent_OnTextChange;
        }

        private void GuiFindOpponent_OnTextChange(object sender, string e)
        {
            //TODO: search for new Player
            Task<Player[]> task = Globals.APIManager.Search(GetComponent<GuiSearch>("Suche").Text);
            task.Wait();
            Player[] p = task.Result;
            players = p;
        }

        protected override void Panel_OnKeyPress(object sender, char e)
        {
            if (e == 27)
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            base.Panel_OnKeyPress(sender, e);
        }

        public override void Init()
        {
            base.Init();
        }

        public override void OnRender()
        {
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
            //StateManager.SetFont(FontUtils.DEFAULT_FONT);
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, offset / 2);

            StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X,  offset * 2), c1, c2);
            base.OnRender();

            offset = offset * 3 + 20;
            for(int i = 0; i < players.Length; i++)
            {
                StateManager.SetColor(Color.White);
                StateManager.FillRoundRect(10, offset, Size.X - 50, 100, 15);
                StateManager.SetColor(100, 0, 200, 150);
                StateManager.FillCircle(50, offset + 50, 65);
                RenderUtils.DrawPlayer(players[i].Name, new Vector(50, offset + 50), 50, false);
                StateManager.SetColor(Color.Black);
                float height = StateManager.GetStringHeight(players[i].Name);
                StateManager.DrawString(players[i].Name, 100, offset + 50 - height / 2);
                StateManager.DrawString("Versende eine Einladung", 100, offset + 50 + height / 2);
                StateManager.FillRect(90, offset + 50 - height / 2, 2, height * 2);
                StateManager.SetColor(0, 0, 0, 150);
                StateManager.FillCircle(Size.X - 100, offset + 50, 51);
                StateManager.FillGradientCircle(Size.X - 100, offset + 50, 50, Color.Blue, Color.Cyan, 45);
                StateManager.SetColor(Color.White);
                float size = 12.5f;
                StateManager.DrawLine(Size.X - 100, offset + 50 - size, Size.X - 100, offset + 50 + size, 2);
                StateManager.DrawLine(Size.X - 100 - size, offset + 50, Size.X - 100 + size, offset + 50, 2);
                offset += 110;
            }
        }
    }
}
