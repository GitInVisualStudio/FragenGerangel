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
        private Player[] gameRequests;
        private Game[] activeGames;

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
            GuiList<GuiGameInfo> games = new GuiList<GuiGameInfo>("Aktive Spiele")
            {
                Location = new Vector(10, 200),
                RWidth = 1,
                Size = new Vector(0, 0)
            };
            Components.Add(games);
            base.Init();

            games.SetLocationAndSize(this, Size);
            foreach (Game g in activeGames)
            {
                if (!g.Active)
                    continue;
                GuiGameInfo info = new GuiGameInfo(g)
                {
                    RWidth = 1,
                    Size = new Vector(-50, 100),
                    BackColor = Color.White,
                };
                info.InfoClick += ActiveGameClick;
                games.Add(info);
            }
            games.Init();
            //int offset = 210;
            //for(int i = 0; i < gameRequests.Length; i++)
            //{
            //    Player p = gameRequests[i];
            //    GuiPlayerInfo info = new GuiPlayerInfo(p, "möchte spielen!", 1)
            //    {
            //        RWidth = 1,
            //        Size = new Vector(-50, 100),
            //        Location = new Vector(10, offset),
            //        BackColor = Color.White
            //    };
            //    info.InfoClick += ClickRequest;
            //    Components.Add(info);
            //    offset += 110;
            //}
            //for(int i = 0; i < activeGames.Length; i++)
            //{
            //    Game game = activeGames[i];
            //    GuiGameInfo info = new GuiGameInfo(game)
            //    {
            //        RWidth = 1,
            //        Size = new Vector(-50, 100),
            //        Location = new Vector(10, offset),
            //        BackColor = Color.White,
            //    };
            //    info.InfoClick += ActiveGameClick;
            //    Components.Add(info);
            //}
            //SearchForUpdates();
        }

        private void SearchForUpdates()
        {
            new Thread(() =>
            {
                while (true)
                {
                    //Task<Player[]> gameRequests = Globals.APIManager.GetDuelRequests();
                    //gameRequests.Wait();
                    //if(gameRequests.Result.Length != this.gameRequests.Length)
                    //{
                    //    fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                    //    return;
                    //}
                    //for(int i = 0; i < this.gameRequests.Length; i++)
                    //    if(this.gameRequests[i].Name != gameRequests.Result[i].Name)
                    //    {
                    //        fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                    //        return;
                    //    }
                    Task<Game[]> activeGames = Globals.APIManager.GetGames();
                    if(activeGames.Result.Length != this.activeGames.Length)
                    {
                        fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
                        return;
                    }
                    for(int i = 0; i < this.activeGames.Length; i++)
                        if(!this.activeGames[i].Equals(activeGames.Result[i]))
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
            activeGames = await Globals.APIManager.GetGames();
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
        }
    }
}
