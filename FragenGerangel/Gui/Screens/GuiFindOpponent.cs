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
    /// <summary>
    /// Screen zum suchen von Gegnern
    /// </summary>
    public class GuiFindOpponent : GuiScreen
    {
        private FragenGerangel fragenGerangel;
        private Player[] players;
        //falls der Spieler was gesucht hat
        private bool updateList;
        //Unveränderte Arrays => damit beim Rendern der Array nicht verändert wird
        private Player[] newPlayer, friendList;
        private float timer;
        private bool searched = true;

        /// <summary>
        /// Spiel instanz für die Suche
        /// </summary>
        /// <param name="fragenGerangel"></param>
        public GuiFindOpponent(FragenGerangel fragenGerangel) : base()
        {
            this.fragenGerangel = fragenGerangel;
        }

        //wenn der spieler was in die suche eingibt
        private void GuiFindOpponent_OnTextChange(object sender, string e)
        {
            if (e == null || e.Length == 0)
                return;
            timer = 0;
            searched = false;
        }

        /// <summary>
        /// geht zurück wenn escape gedrückt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void Panel_OnKeyPress(object sender, char e)
        {
            if (e == 27)
                fragenGerangel.OpenScreen(new GuiMainScreen(fragenGerangel));
            base.Panel_OnKeyPress(sender, e);
        }

        /// <summary>
        /// erstellt die nötigen komponenten
        /// </summary>
        public override void Init()
        {
            //suche
            Components.Add(new GuiTextBox("Suche")
            {
                Location = new Vector(20, 75),
                RWidth = 1,
                Size = new Vector(-50, 50)
            });
            GetComponent<GuiTextBox>("Suche").OnTextChange += GuiFindOpponent_OnTextChange;

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

        /// <summary>
        /// fügt die gefundenen spieler dem screen hinzu
        /// </summary>
        private void AddComponents()
        {
            int offset = 170;
            if(players != null)
                foreach (Player p in players)
                {
                    //verschiedene komponenten wenn man befreundet ist und wenn nicht
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

        /// <summary>
        /// gibt zurück ob der spieler mit dem diesem spieler befreundet ist
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool IsFriend(Player p)
        {
            foreach(Player player in friendList)
            {
                if (player.Name == p.Name)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// wird aufgerufen wenn auf einen spieler geklickt wird
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoOnClick(object sender, bool e)
        {
            GuiPlayerInfo guiPlayerInfo = (GuiPlayerInfo)sender;
            if (!e)
            {
                //nur stats
                fragenGerangel.OpenScreen(new GuiStats(fragenGerangel, guiPlayerInfo.Player));
                return;
            }
            new Thread(() =>
            {
                //sendet anfrage
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

        /// <summary>
        /// Zeichnet die komponenten
        /// </summary>
        public override void OnRender()
        {
            timer += StateManager.delta;
            if(timer > 1f && !searched)
            {
                new Thread(() =>
                {
                    Task<Player[]> task = Globals.APIManager.Search(GetComponent<GuiTextBox>("Suche").Text);
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
                GuiTextBox search = GetComponent<GuiTextBox>("Suche");
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
