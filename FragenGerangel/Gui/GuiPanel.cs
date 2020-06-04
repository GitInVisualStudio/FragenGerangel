﻿using FragenGerangel.Utils.Math;
using FragenGerangel.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    public class GuiPanel : GuiComponent
    {
        private List<GuiComponent> components;
        private Bitmap buffer;

        public List<GuiComponent> Components
        {
            get
            {
                return components;
            }

            set
            {
                components = value;
            }
        }

        public GuiPanel() : base()
        {
            components = new List<GuiComponent>();
        }

        protected virtual void Panel_OnResize(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnResize(e));
        }

        protected virtual void Panel_OnRelease(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                    x.Component_OnRelease(e);
            };
        }

        protected virtual void Panel_OnMove(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                    x.Component_OnMove(e);
            };
        }

        protected virtual void Panel_OnClick(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                    x.Component_OnClick(e);
            };
        }

        protected virtual void Panel_OnKeyRelease(object sender, char e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.Selected)
                    x.Component_OnKeyRelease(e);
            };
        }

        protected virtual void Panel_OnKeyPress(object sender, char e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.Selected)
                    x.Component_OnKeyPress(e);
            };
        }

        public override void Init()
        {
            base.Init();
            OnResize += Panel_OnResize;
            OnClick += Panel_OnClick;
            OnMove += Panel_OnMove;
            OnRelease += Panel_OnRelease;
            OnKeyPress += Panel_OnKeyPress;
            OnKeyRelease += Panel_OnKeyRelease;
            SetLocationAndSize(this, Size);

            components.ForEach(x =>
            {
                x.Init();
                x.SetLocationAndSize(this, Size);
            });
        }

        public override void OnRender()
        {
            if (Size.X < 1 || Size.Y < 1)
                return;
            buffer = new Bitmap((int)Size.X, (int)Size.Y);
            StateManager.Push();
            StateManager.Translate(-Location);
            StateManager.SetGraphics(Graphics.FromImage(buffer));
            components.ForEach(x => x.OnRender());
            StateManager.Pop();
            StateManager.DrawImage(buffer, Location);
            buffer.Dispose();
        }
    }
}