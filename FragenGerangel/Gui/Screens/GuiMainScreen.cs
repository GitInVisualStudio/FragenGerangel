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
    /// <summary>
    /// Der hauptbildschirm des spieles
    /// </summary>
    public class GuiMainScreen : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private Player[] gameRequests, friendRequests;
        private Game[] games;
        private bool update;
        private int scroll;
        private float scrollDelta;
        private bool shouldScroll;

        /// <summary>
        /// Spiel instanz für aktualisierungen
        /// </summary>
        /// <param name="fragenGerangel"></param>
        public GuiMainScreen(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        /// <summary>
        /// Erstellt alle notwendigen komponenten
        /// </summary>
        public override void Init()
        {
            update = true;
            Components.Add(new GuiButton("Suche")
            {
                Location = new Vector(-100, 155),
                RX = 0.5f,
                Size = new Vector(200, 50),
                BackColor = Color.LawnGreen,
                FontColor = Color.White
            });
            GetComponent<GuiButton>("Suche").OnClick += OnClick_NewGame;
            //holt alle notwendigen informationen vom server
            Update().Wait();
            base.Init();

            //liste von aktiven spielen
            GuiList<GuiGameInfo> games = new GuiList<GuiGameInfo>("Aktive Spiele")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            //liste von vergangenen spielen
            GuiList<GuiGameInfo> gamesClosed = new GuiList<GuiGameInfo>("Vergangene Spiele")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };

            games.SetLocationAndSize(this, Size);
            gamesClosed.SetLocationAndSize(this, Size);
            //hinzufügen der spiele
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
                {
                    gamesClosed.Add(info);
                }
                else
                    games.Add(info);
            }

            gamesClosed.Init();
            games.Init();

            //liste von anfragen
            GuiList<GuiPlayerInfo> friendRequests = new GuiList<GuiPlayerInfo>("Freundschaftsanfragen")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            {
                friendRequests.SetLocationAndSize(this, Size);
                //hinzufügen der anfragen
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
            //liste von anfragen
            GuiList<GuiPlayerInfo> gameRequests = new GuiList<GuiPlayerInfo>("Spielanfragen")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            {
                gameRequests.SetLocationAndSize(this, Size);
                //hinzufügen der anfragen
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
            //fügt die listen hinzu falls diese elemente beinhalten
            if (games.Components.Count > 0)
                Components.Add(games);
            if (gamesClosed.Components.Count > 0)
                Components.Add(gamesClosed);
            SearchForUpdates();
        }

        /// <summary>
        /// Wird aufgeruft wenn gescrollt wird
        /// </summary>
        /// <param name="direction"></param>
        public override void OnSroll(int direction)
        {
            base.OnSroll(direction);
            if(shouldScroll && direction > 0)
                scroll += direction;
            else if(direction < 0)
                scroll += direction;
            if (scroll < 0)
                scroll = 0;
        }

        /// <summary>
        /// akzeptiert oder lehnt spielfragen ab und aktualisiert mit dem sevrer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                new Thread(() =>
                {
                    Globals.APIManager.DeclineDuelRequest(info.Player).Wait();
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                }).Start();
            }
        }

        /// <summary>
        /// akzeptiert oder lehnt freundschaftsanfragen ab und aktualisiert mit dem sevrer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                new Thread(() =>
                {
                    Globals.APIManager.DeclineFriendRequest(info.Player).Wait();
                    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                }).Start();
            }
        }

        /// <summary>
        /// schließt den update thread wenn der screen geschlossen wird
        /// </summary>
        public override void Close()
        {
            update = false;
            base.Close();
        }

        /// <summary>
        /// guckt jede minute ob eine aktualisierung vorliegt und verarbeitet diese wenn vorhanden
        /// </summary>
        private void SearchForUpdates()
        {
            new Thread(() =>
            {
                int time = 0;
                while (update)
                {
                    Thread.Sleep(1000);
                    time++;
                    if (time > 60)
                    {
                        time = 0;
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
                        if (activeGames.Result.Length != this.games.Length)
                        {
                            fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                            return;
                        }
                        for (int i = 0; i < this.games.Length; i++)
                            if (!this.games[i].Equals(activeGames.Result[i]))
                            {
                                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                                return;
                            }
                    }
                }
            }).Start();
        }

        /// <summary>
        /// öffnet das spiel um die übersicht zu sehen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActiveGameClick(object sender, bool e)
        {
            GuiGameInfo info = (GuiGameInfo)sender;
            fragenGerangel.OpenScreen(new GuiGameOverview(fragenGerangel, info.Game));
        }

        /// <summary>
        /// holt alle informationen des spielers vom server
        /// </summary>
        /// <returns></returns>
        private async Task Update()
        {
            gameRequests = await Globals.APIManager.GetDuelRequests().ConfigureAwait(false);
            games = await Globals.APIManager.GetDuelIDs().ConfigureAwait(false);
            friendRequests = await Globals.APIManager.GetFriendRequests().ConfigureAwait(false);
        }

        /// <summary>
        /// öffnet die suche
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick_NewGame(object sender, Vector e)
        {
            fragenGerangel.OpenScreen(new GuiFindOpponent(fragenGerangel));
        }

        /// <summary>
        /// passt die position des klick events mit dem scrolling an
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Panel_OnClick(object sender, Vector e)
        {
            if(e.Y > 50 && e.Y < 150)
            {
                fragenGerangel.OpenScreen(new GuiStats(fragenGerangel, Globals.Player));
            }
            e = new Vector(e.X, e.Y + scrollDelta);
            base.Panel_OnClick(sender, e);
        }

        /// <summary>
        /// passt die position des move events mit dem scrolling an
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Panel_OnMove(object sender, Vector e)
        {
            e = new Vector(e.X, e.Y + scrollDelta);
            base.Panel_OnMove(sender, e);
        }

        /// <summary>
        /// Zeichnet alle komponenten
        /// </summary>
        public override void OnRender()
        {
            StateManager.Push();
            scrollDelta += (scroll - scrollDelta) * StateManager.delta * 10;
            StateManager.Translate(0, -scrollDelta); //versetzt um das scrolling
            int offset = 200;
            shouldScroll = false;
            foreach (GuiComponent component in Components)
            {
                if (component is GuiButton)
                {
                    component.OnRender();
                    continue;
                }
                component.Location = new Vector(component.Location.X, offset);
                component.OnRender();
                offset += (int)component.Size.Y + 30;
                if (offset - scroll > Size.Y)
                    shouldScroll = true;
            }
            StateManager.Pop();

            Color c1 = Color.FromArgb(255, 2, 175, 230);
            Color c2 = Color.FromArgb(255, 84, 105, 230);
            offset = 50;
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
        }
    }
}
