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
    public class GuiCategory : GuiScreen
    {
        private Game game;
        private Round round;
        private int category;

        public GuiCategory(Game game) : base()
        {
            Game = game;
            Round = game.LastRound;
            //categories = round.PossibleCategories;
        }

        public Round Round { get => round; set => round = value; }
        public int Category { get => category; set => category = value; }
        public Game Game { get => game; set => game = value; }

        public override void Init()
        {
            int offset = 150;
            for(int i = 0; i < round.PossibleCategories.Length; i++, offset += 110)
                Components.Add(new GuiButton(round.PossibleCategories[i])
                {
                    Location = new Vector(-200, offset),
                    Size = new Vector(400, 100),
                    BackColor = Color.White,
                    RX = 0.5f,
                    FontColor = Color.Black
                });
            Components.ForEach(x => x.OnClick += CategorySelected);
            base.Init();
        }

        private void CategorySelected(object sender, Vector e)
        {
            if(Opend)
                category = Components.FindIndex(x => x.Name == ((GuiButton)sender).Name);            
            Close();
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

            StateManager.SetFont(new Font("comfortaa", 15));
            string score = "Wähle eine Kategorie aus:";
            StateManager.DrawCenteredString(score, Size.X / 2, offset * 2 + offset / 2);

            float width = StateManager.GetStringWidth(score);

            StateManager.SetColor(Color.White);
            float var1 = Size.X / 2 - width;
            float var2 = Size.X / 2 + width;
            RenderUtils.DrawPlayer(Globals.Player.Name, new Vector(var1 / 2 + 50, offset * 3), 100);
            RenderUtils.DrawPlayer(game.RemotePlayer.Name, new Vector(var2 + var1 / 2 - 50, offset * 3), 100);

            //StateManager.SetColor(Color.White);
            //StateManager.FillCircle(Size.X / 2, Size.Y / 2, Size.Y / 2);
        }

    }
}
