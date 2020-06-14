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
        private float var3, var4, var5, var6;

        public GuiPlayerInfo(Player player, string text, int mode) : base(player.Name)
        {
            Player = player;
            Mode = mode;
            Text = text;
            OnClick += GuiPlayerInfo_OnClick;
            OnMove += GuiPlayerInfo_OnMove;
        }

        private void GuiPlayerInfo_OnMove(object sender, Vector e)
        {
            Vector position;
            switch (mode)
            {
                case 0:
                    position = new Vector(Size.X - 170, Location.Y + Size.Y / 2);
                    if((position - e).Length < 25)
                        var4 = 0.4f;
                    else
                        var4 = 0;
                    break;
                case 1:
                    position = new Vector(Size.X - 150 - 80, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                        var4 = 0.4f;
                    else
                        var4 = 0;
                    break;
                case 2:
                    position = new Vector(Size.X - 150, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                        var6 = 0.4f;
                    else
                        var6 = 0;
                    position = new Vector(Size.X - 150 - 60, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                        var4 = 0.4f;
                    else
                        var4 = 0;
                    break;
            }
        }

        private void GuiPlayerInfo_OnClick(object sender, Vector e)
        {
            Vector position;
            switch (mode)
            {
                case 0:
                    position = new Vector(Size.X - 170, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                    {
                        InfoClick?.Invoke(this, true);
                        return;
                    }
                    break;
                case 1:
                    position = new Vector(Size.X - 150 - 80, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                    {
                        InfoClick?.Invoke(this, true);
                        return;
                    }
                    break;
                case 2:
                    position = new Vector(Size.X - 150, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                    {
                        InfoClick?.Invoke(this, false);
                        return;
                    }
                    position = new Vector(Size.X - 150 - 60, Location.Y + Size.Y / 2);
                    if ((position - e).Length < 25)
                    {
                        InfoClick?.Invoke(this, true);
                        return;
                    }
                    break;

            }
            if(mode != 2)
                InfoClick?.Invoke(this, false);
        }

        public Player Player { get => player; set => player = value; }
        public int Mode { get => mode; set => mode = value; }
        public string Text { get => text; set => text = value; }

        public override void OnRender()
        {
            StateManager.SetFont(new Font("Arial", 12, FontStyle.Bold));
            var1 += (var2 - var1) * StateManager.delta * 10;
            int r = CurrentColor.R - (int)(CurrentColor.R * (var1));
            int g = CurrentColor.G - (int)(CurrentColor.G * (var1));
            int b = CurrentColor.B - (int)(CurrentColor.B * (var1));
            r = Math.Abs(r % 256);
            g = Math.Abs(g % 256);
            b = Math.Abs(b % 256);
            StateManager.SetColor(r, g, b);
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
            var3 += (var4 - var3) * StateManager.delta * 10;
            var5 += (var6 - var5) * StateManager.delta * 10;
            switch (mode)
            {
                case 0:
                    StateManager.Push();
                    StateManager.Translate(-20, 0);
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, GetColor(Color.Cyan, var3), 45);
                    StateManager.SetColor(Color.White);
                    StateManager.DrawLine(Size.X - 150, Location.Y + Size.Y / 2 - size, Size.X - 150, Location.Y + Size.Y / 2 + size, 2);
                    StateManager.DrawLine(Size.X - 150 - size, Location.Y + Size.Y / 2, Size.X - 150 + size, Location.Y + Size.Y / 2, 2);
                    StateManager.SetColor(Color.Black);
                    StateManager.SetFont(new Font("Arial", 10, FontStyle.Bold));
                    height = StateManager.GetStringHeight("Spielanfrage Senden");
                    StateManager.DrawString("Spielanfrage Senden", Size.X - 110, Location.Y + Size.Y / 2 - height / 2);
                    StateManager.FillRect(Size.X - 115, Location.Y + Size.Y / 2 - height / 2, 2, height);
                    StateManager.Pop();
                    break;
                case 1:
                    StateManager.Push();
                    StateManager.Translate(-80, 0);
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, GetColor(Color.Cyan, var3), 45);
                    StateManager.SetColor(Color.White);
                    StateManager.DrawLine(Size.X - 150, Location.Y + Size.Y / 2 - size, Size.X - 150, Location.Y + Size.Y / 2 + size, 2);
                    StateManager.DrawLine(Size.X - 150 - size, Location.Y + Size.Y / 2, Size.X - 150 + size, Location.Y + Size.Y / 2, 2);
                    StateManager.SetColor(Color.Black);
                    StateManager.SetFont(new Font("Arial", 10, FontStyle.Bold));
                    height = StateManager.GetStringHeight("Freundschaftsanfrage Senden");
                    StateManager.DrawString("Freundschaftsanfrage Senden", Size.X - 110, Location.Y + Size.Y / 2 - height / 2);
                    StateManager.FillRect(Size.X - 115, Location.Y + Size.Y / 2 - height / 2, 2, height);
                    StateManager.Pop();
                    break;
                case 2:
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, GetColor(Color.Cyan, var5), 45);
                    StateManager.SetFont(new Font("Arial", 20));
                    StateManager.SetColor(Color.White);
                    StateManager.DrawCenteredString("X", Size.X - 150, Location.Y + Size.Y / 2 + 5);

                    StateManager.Push();
                    StateManager.Translate(-60, 0);
                    StateManager.SetColor(0, 0, 0, 150);
                    StateManager.FillCircle(Size.X - 150, Location.Y + Size.Y / 2, 51);
                    StateManager.FillGradientCircle(Size.X - 150, Location.Y + Size.Y / 2, 50, Color.Blue, GetColor(Color.Cyan, var3), 45);
                    StateManager.SetFont(new Font("Arial", 20));
                    StateManager.SetColor(Color.White);
                    StateManager.DrawCenteredString("" + (char)10004, Size.X - 150, Location.Y + Size.Y / 2 + 5);
                    StateManager.Pop();

                    StateManager.SetColor(Color.Black);
                    StateManager.SetFont(new Font("Arial", 10, FontStyle.Bold));
                    height = StateManager.GetStringHeight("Anfrage annehmen");
                    StateManager.DrawString("Anfrage annehmen", Size.X - 110, Location.Y + Size.Y / 2 - height / 2);
                    StateManager.FillRect(Size.X - 115, Location.Y + Size.Y / 2 - height / 2, 2, height);
                    break;
            }
        }

        private Color GetColor(Color c1, float var1)
        {
            int r = c1.R - (int)(c1.R * var1);
            int g = c1.G - (int)(c1.G * var1);
            int b = c1.B - (int)(c1.B * var1);
            r = Math.Abs(r % 256);
            g = Math.Abs(g % 256);
            b = Math.Abs(b % 256);
            return Color.FromArgb(r, g, b);
        }
    }
}
