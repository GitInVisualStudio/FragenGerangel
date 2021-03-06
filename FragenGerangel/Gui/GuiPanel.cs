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
    /// <summary>
    /// liste von komponenten
    /// </summary>
    public class GuiPanel : GuiComponent
    {
        private List<GuiComponent> components;

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

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnResize(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnResize(e));
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnRelease(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                    x.Component_OnRelease(e);
            }
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnMove(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                    x.Component_OnMove(e);
            }
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnClick(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.OnHover(e))
                {
                    x.Component_OnClick(e);
                    x.Selected = true;
                }
                else
                {
                    x.Selected = false;
                }
            }
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnKeyRelease(object sender, char e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (x.Selected)
                    x.Component_OnKeyRelease(e);
            }
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnKeyPress(object sender, char e)
        {
            bool flag = false;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if(e == 9 && x.Selected)
                {
                    flag = true;
                    x.Selected = false;
                    if (i < components.Count - 1)
                        components[i + 1].Selected = true;
                    else
                        components[0].Selected = true;
                    return;
                }
                if (x.Selected)
                {
                    x.Component_OnKeyPress(e);
                }
            }
            if (!flag && e == 9 && components.Count > 0)
                components[0].Selected = true;
        }

        /// <summary>
        /// fügt alle events hinzu
        /// </summary>
        public override void Init()
        {
            base.Init();
            OnResize += Panel_OnResize;
            OnClick += Panel_OnClick;
            OnMove += Panel_OnMove;
            OnRelease += Panel_OnRelease;
            OnKeyPress += Panel_OnKeyPress;
            OnKeyRelease += Panel_OnKeyRelease;
            OnLeave += GuiPanel_OnLeave;
            SetLocationAndSize(this, Size);

            components.ForEach(x =>
            {
                x.Init();
                x.SetLocationAndSize(this, Size);
            });
        }

        /// <summary>
        /// übergabe von events an die komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void GuiPanel_OnLeave(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                x.Component_OnLeave(e);
            }
        }

        /// <summary>
        /// gibt eine komponente des types T zurück mit dem namen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetComponent<T>(string name) where T : GuiComponent
        {
            return (T)components.Find(x => x.Name == name);
        }

        /// <summary>
        /// zeichnet alle komponenten
        /// </summary>
        public override void OnRender()
        {
            if (Size.X < 1 || Size.Y < 1)
                return;
            components.ForEach(x => x.OnRender());
        }
    }
}
