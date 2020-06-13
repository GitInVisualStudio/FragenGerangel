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
    public class GuiMainScreen : GuiScreen
    {
        private FragenGerangel fragenGerangel;

        public GuiMainScreen(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        public override void Init()
        {
            Components.Add(new GuiButton("Neues Spiel")
            {
                Location = new Vector(-100, 155),
                RX = 0.5f,
                Size = new Vector(200, 50),
                BackColor = Color.LawnGreen,
                FontColor = Color.White
            });
            GetComponent<GuiButton>("Neues Spiel").OnClick += OnClick_NewGame;
            base.Init();
        }

        private void OnClick_NewGame(object sender, Vector e)
        {
            fragenGerangel.OpenScreen(new GuiFindOpponent(fragenGerangel));
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

            Player localPlayer = Globals.Player;
            RenderUtils.DrawPlayer(localPlayer.Name, new Vector(40, offset * 2), 60, false);
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("Arial", 12));
            float height = StateManager.GetStringHeight(localPlayer.Name);
            StateManager.DrawString(localPlayer.Name, 100, offset * 1.5f);
            StateManager.SetFont(new Font("Arial", 10));
            StateManager.DrawString("Deine Statistiken >", 100, offset * 1.5f + height);
            StateManager.FillRect(100 - 5, offset * 1.5f, 2, height * 2);

            RenderYourTurn();
        }

        private void RenderYourTurn()
        {
            StateManager.SetFont(new Font("Arial", 12, FontStyle.Bold));
            StateManager.SetColor(Color.Black);
            StateManager.DrawString("Du bist dran", 50, 200);
            StateManager.SetColor(Color.DarkOrange);
        }
    }
}
