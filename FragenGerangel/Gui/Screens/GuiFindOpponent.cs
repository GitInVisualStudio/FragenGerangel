using FragenGerangel.GameBase;
using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FragenGerangel.Gui.Screens
{
    public class GuiFindOpponent : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private Player[] players;
        private bool updateList;
        private List<Player> newPlayer;

        public GuiFindOpponent(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        private void GuiFindOpponent_OnTextChange(object sender, string e)
        {
            //TODO: search for new Player
            List<Player> result = new List<Player>();
            foreach (Player p in players)
                if (p.Name.ToLower().StartsWith(e.ToLower()))
                    result.Add(p);
            newPlayer = result;
            updateList = true;
        }

        protected override void Panel_OnKeyPress(object sender, char e)
        {
            if (e == 27)
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            base.Panel_OnKeyPress(sender, e);
        }

        public override void Init()
        {
            Components.Add(new GuiSearch("Suche")
            {
                Location = new Vector(20, 75),
                RWidth = 1,
                Size = new Vector(-50, 50)
            });

            GetComponent<GuiSearch>("Suche").OnTextChange += GuiFindOpponent_OnTextChange;

            Task<Player[]> var1 = Globals.APIManager.GetFriends();
            var1.Wait();
            players = var1.Result;
            int offset = 170;
            foreach(Player p in players)
            {
                GuiPlayerInfo info = new GuiPlayerInfo(p, "Versende eine Einladung", 0)
                {
                    Location = new Vector(10, offset),
                    Size = new Vector(-50, 100),
                    RWidth = 1,
                    BackColor = Color.White
                };
                info.InfoClick += InfoOnClick;
                Components.Add(info);
            }
            base.Init();
        }

        private void InfoOnClick(object sender, bool e)
        {
            //TODO: start a new game
            new Thread(() =>
            {
                GuiPlayerInfo guiPlayerInfo = (GuiPlayerInfo)sender;
                Globals.APIManager.StartDuel(guiPlayerInfo.Player).Wait();
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            }).Start();
        }

        public override void OnRender()
        {
            if (updateList)
            {
                updateList = false;
                players = newPlayer.ToArray();
            }
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
            StateManager.DrawCenteredString("FragenGerangel", Size.X / 2, offset / 2);

            StateManager.FillGradientRect(new Vector(0, offset), new Vector(Size.X,  offset * 2), c1, c2);
            base.OnRender();
        }
    }
}
