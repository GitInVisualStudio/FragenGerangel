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
using static System.Net.Mime.MediaTypeNames;

namespace FragenGerangel.Gui.Screens
{
    public class GuiMainScreen : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private Player[] gameRequests, friendRequests;
        private Game[] games;

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
            Update().Wait();
            base.Init();


            GuiList<GuiGameInfo> games = new GuiList<GuiGameInfo>("Aktive Spiele")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            GuiList<GuiGameInfo> gamesClosed = new GuiList<GuiGameInfo>("Vergangene Spiele")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };

            games.SetLocationAndSize(this, Size);
            gamesClosed.SetLocationAndSize(this, Size);
            foreach (Game g in this.games)
            {
                GuiGameInfo info = new GuiGameInfo(g)
                {
                    RWidth = 1,
                    Size = new Vector(-50, 100),
                    BackColor = Color.White,
                };
                info.InfoClick += ActiveGameClick;
                if (!g.Active)
                    gamesClosed.Add(info);
                else
                    games.Add(info);
            }
            gamesClosed.Init();
            games.Init();


            GuiList<GuiPlayerInfo> friendRequests = new GuiList<GuiPlayerInfo>("Freundschaftsanfragen")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            {
                friendRequests.SetLocationAndSize(this, Size);
                foreach(Player p in this.friendRequests)
                {
                    GuiPlayerInfo info = new GuiPlayerInfo(p, "möchte dein Freund sein", 2)
                    {
                        RWidth = 1,
                        Size = new Vector(-50, 100),
                        BackColor = Color.White,
                    };
                    info.InfoClick += HandleFriendRequest;
                    friendRequests.Add(info);
                }
                friendRequests.Init();
                if(friendRequests.Components.Count > 0)
                    Components.Add(friendRequests);
            }

            GuiList<GuiPlayerInfo> gameRequests = new GuiList<GuiPlayerInfo>("Spielanfragen")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            {
                gameRequests.SetLocationAndSize(this, Size);
                foreach (Player p in this.gameRequests)
                {
                    GuiPlayerInfo info = new GuiPlayerInfo(p, "möchte spielen", 2)
                    {
                        RWidth = 1,
                        Size = new Vector(-50, 100),
                        BackColor = Color.White,
                    };
                    info.InfoClick += HandleGameRequest; ;
                    gameRequests.Add(info);
                }
                gameRequests.Init();
                if (gameRequests.Components.Count > 0)
                    Components.Add(gameRequests);
            }

            if (games.Components.Count > 0)
                Components.Add(games);
            if (gamesClosed.Components.Count > 0)
                Components.Add(gamesClosed);
            SearchForUpdates();
        }

        private void HandleGameRequest(object sender, bool e)
        {
            //TODO: accept / reject request and refresh
            GuiPlayerInfo info = (GuiPlayerInfo)sender;
            if (e)
            {
                new Thread(() =>
                {
                    Globals.APIManager.StartDuel(info.Player).Wait();
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                }).Start();
            }
            else
            {

            }
        }

        private void HandleFriendRequest(object sender, bool e)
        {
            //TODO: accept / reject request and refresh
            GuiPlayerInfo info = (GuiPlayerInfo)sender;
            if (e)
            {
                new Thread(() =>
                {
                    Globals.APIManager.BefriendUser(info.Player.Name).Wait();
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                }).Start();
            }
            else
            {

            }
        }

        private void SearchForUpdates()
        {
            new Thread(() =>
            {
                while (true)
                {
                    //Gamerequests
                    Task<Player[]> gameRequests = Globals.APIManager.GetDuelRequests();
                    gameRequests.Wait();
                    if (gameRequests.Result.Length != this.gameRequests.Length)
                    {
                        fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                        return;
                    }
                    for (int i = 0; i < this.gameRequests.Length; i++)
                        if (this.gameRequests[i].Name != gameRequests.Result[i].Name)
                        {
                            fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                            return;
                        }
                    //Friends
                    Task<Player[]> friendRequests = Globals.APIManager.GetFriendRequests();
                    friendRequests.Wait();
                    if (friendRequests.Result.Length != this.friendRequests.Length)
                    {
                        fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                        return;
                    }
                    for (int i = 0; i < this.friendRequests.Length; i++)
                        if (this.friendRequests[i].Name != friendRequests.Result[i].Name)
                        {
                            fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                            return;
                        }
                    //Games
                    Task<Game[]> activeGames = Globals.APIManager.GetGames();
                    if(activeGames.Result.Length != this.games.Length)
                    {
                        fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                        return;
                    }
                    for(int i = 0; i < this.games.Length; i++)
                        if(!this.games[i].Equals(activeGames.Result[i]))
                        {
                            fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                            return;
                        }
                    Thread.Sleep(5000);
                    if (!Opend)
                        break;
                }
            }).Start();
        }

        private void ActiveGameClick(object sender, bool e)
        {
            GuiGameInfo info = (GuiGameInfo)sender;
            fragenGerangel.OpenScreen(new GuiGameOverview(fragenGerangel, info.Game));
        }

        private void ClickRequest(object sender, bool e)
        {
            new Thread(() =>
            {
                GuiPlayerInfo info = (GuiPlayerInfo)sender;
                Globals.APIManager.StartDuel(info.Player).Wait();
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            }).Start();
        }

        private async Task Update()
        {
            gameRequests = await Globals.APIManager.GetDuelRequests();
            games = await Globals.APIManager.GetGames();
            friendRequests = await Globals.APIManager.GetFriendRequests();
        }

        private void OnClick_NewGame(object sender, Vector e)
        {
            fragenGerangel.OpenScreen(new GuiFindOpponent(fragenGerangel));
        }

        protected override void Panel_OnClick(object sender, Vector e)
        {
            base.Panel_OnClick(sender, e);
            if(e.Y > 50 && e.Y < 150)
            {
                fragenGerangel.OpenScreen(new GuiStats(fragenGerangel, Globals.Player));
            }
        }

        public override void OnRender()
        {
            //base.OnRender();
            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            int offset = 50;
            StateManager.FillGradientRect(Location, new Vector(Size.X, offset), c1, c2);
            StateManager.SetColor(Color.White);
            StateManager.SetFont(new Font("comfortaa", 20));
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

            offset = 200;
            foreach(GuiComponent component in Components)
            {
                if (component is GuiButton)
                {
                    component.OnRender();
                    continue;
                }
                component.Location = new Vector(component.Location.X, offset);
                component.OnRender();
                offset += (int)component.Size.Y + 30;
            }
        }
    }
}
