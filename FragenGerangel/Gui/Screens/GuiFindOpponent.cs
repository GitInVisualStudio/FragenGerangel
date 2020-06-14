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
        private Player[] newPlayer, friendList;
        private float timer;
        private bool searched = true;

        public GuiFindOpponent(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        private void GuiFindOpponent_OnTextChange(object sender, string e)
        {
            //TODO: search for new Player
            if (e == null || e.Length == 0)
                return;
            timer = 0;
            searched = false;
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
            friendList = var1.Result;
            players = var1.Result;


            OnResize += SetLocationAndSize;
            OnResize += Panel_OnResize;
            OnClick += Panel_OnClick;
            OnMove += Panel_OnMove;
            OnRelease += Panel_OnRelease;
            OnKeyPress += Panel_OnKeyPress;
            OnKeyRelease += Panel_OnKeyRelease;
            OnLeave += GuiPanel_OnLeave;
            SetLocationAndSize(this, Size);
            AddComponents();
        }

        private void AddComponents()
        {
            int offset = 170;
            if(players != null)
                foreach (Player p in players)
                {
                    GuiPlayerInfo info = new GuiPlayerInfo(p, "Versende eine Einladung", IsFriend(p) ? 0 : 1)
                    {
                        Location = new Vector(10, offset),
                        Size = new Vector(-50, 100),
                        RWidth = 1,
                        BackColor = Color.White
                    };
                    info.Component_OnResize(Size);
                    info.InfoClick += InfoOnClick;
                    offset += 110;
                    Components.Add(info);
                }
            Components.ForEach(x =>
            {
                x.Init();
                x.SetLocationAndSize(this, Size);
            });
        }

        private bool IsFriend(Player p)
        {
            foreach(Player player in friendList)
            {
                if (player.Name == p.Name)
                    return true;
            }
            return false;
        }

        private void InfoOnClick(object sender, bool e)
        {
            //TODO: start a new game
            GuiPlayerInfo guiPlayerInfo = (GuiPlayerInfo)sender;
            if (!e)
            {
                fragenGerangel.OpenScreen(new GuiStats(fragenGerangel, guiPlayerInfo.Player));
                return;
            }
            new Thread(() =>
            {
                if (!IsFriend(guiPlayerInfo.Player))
                {
                    Globals.APIManager.BefriendUser(guiPlayerInfo.Player.Name).Wait();
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                    return;
                }
                Globals.APIManager.StartDuel(guiPlayerInfo.Player).Wait();
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            }).Start();
        }

        public override void OnRender()
        {
            timer += StateManager.delta;
            if(timer > 1f && !searched)
            {
                new Thread(() =>
                {
                    Task<Player[]> task = Globals.APIManager.Search(GetComponent<GuiSearch>("Suche").Text);
                    task.Wait();
                    Player[] players = task.Result;
                    newPlayer = players;
                    updateList = true;
                    searched = true;
                }).Start();
            }
            if (updateList)
            {
                updateList = false;
                players = newPlayer.ToArray();
                GuiSearch search = GetComponent<GuiSearch>("Suche");
                Components.Clear();
                Components.Add(search);
                AddComponents();
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
