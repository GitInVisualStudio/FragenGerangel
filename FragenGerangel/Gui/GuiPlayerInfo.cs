using FragenGerangel.GameBase;
using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    public class GuiPlayerInfo : GuiButton
    {
        private Player player;
        private int mode;
        private string text;
        public event EventHandler<bool> InfoClick;


        public GuiPlayerInfo(Player player, string text, int mode) : base(player.Name)
        {
            Player = player;
            Mode = mode;
            Text = text;
            OnClick += GuiPlayerInfo_OnClick;
        }

        private void GuiPlayerInfo_OnClick(object sender, Vector e)
        {
            InfoClick?.Invoke(this, false);
        }

        public Player Player { get => player; set => player = value; }
        public int Mode { get => mode; set => mode = value; }
        public string Text { get => text; set => text = value; }

        public override void OnRender()
        {
            StateManager.SetFont(new Font("Arial", 12, FontStyle.Bold));
            StateManager.SetColor(CurrentColor);
            StateManager.FillRoundRect(Location, Size, 15);
            StateManager.SetColor(100, 0, 200, 150);
            StateManager.FillCircle(Location.X + 40, Location.Y + Size.Y / 2, 65);
            RenderUtils.DrawPlayer(player.Name, new Vector(Location.X + 40, Location.Y + Size.Y / 2), 50, false);
            StateManager.SetColor(Color.Black);
            float height = StateManager.GetStringHeight(player.Name);
            StateManager.DrawString(player.Name, Location.X + 90, Location.Y + Size.Y / 2 - height / 2);
            StateManager.DrawString(Text, Location.X + 90, Location.Y + Size.Y / 2+ height / 2);
            StateManager.FillRect(Location.X - 10 + 90, Location.Y + Size.Y / 2 - height / 2, 2, height * 2);

            float size = 12.5f;
            switch (mode)
            {
                case 0:
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, Color.Cyan, 45);
                    StateManager.SetColor(Color.White);
                    StateManager.DrawLine(Size.X - 150, Location.Y + Size.Y / 2 - size, Size.X - 150, Location.Y + Size.Y / 2 + size, 2);
                    StateManager.DrawLine(Size.X - 150 - size, Location.Y + Size.Y / 2, Size.X - 150 + size, Location.Y + Size.Y / 2, 2);
                    StateManager.SetColor(Color.Black);
                    StateManager.SetFont(new Font("Arial", 10, FontStyle.Bold));
                    height = StateManager.GetStringHeight("Anfrage Senden");
                    StateManager.DrawString("Anfrage Senden", Size.X - 110, Location.Y + Size.Y / 2 - height / 2);
                    StateManager.FillRect(Size.X - 115, Location.Y + Size.Y / 2 - height / 2, 2, height);
                    break;
                case 1:
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, Color.Cyan, 45);
                    StateManager.SetFont(new Font("Arial", 20));
                    StateManager.SetColor(Color.White);
                    StateManager.DrawCenteredString("" + (char)10004, Size.X - 150, Location.Y + Size.Y / 2 + 5);
                    //StateManager.DrawLine(Size.X - 150, Location.Y + Size.Y / 2 - size, Size.X - 150, Location.Y + Size.Y / 2 + size, 2);
                    //StateManager.DrawLine(Size.X - 150 - size, Location.Y + Size.Y / 2, Size.X - 150 + size, Location.Y + Size.Y / 2, 2);
                    StateManager.SetColor(Color.Black);
                    StateManager.SetFont(new Font("Arial", 10, FontStyle.Bold));
                    height = StateManager.GetStringHeight("Anfrage annehmen");
                    StateManager.DrawString("Anfrage annehmen", Size.X - 110, Location.Y + Size.Y / 2 - height / 2);
                    StateManager.FillRect(Size.X - 115, Location.Y + Size.Y / 2 - height / 2, 2, height);
                    break;
            }
        }
    }
}
