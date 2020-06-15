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
    /// <summary>
    /// Liste von komponenten welche sich automatisch anpasst
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GuiList<T> : GuiComponent where T : GuiComponent
    {
        private List<T> components;

        public List<T> Components
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

        /// <summary>
        /// name und mögliche liste
        /// </summary>
        /// <param name="name"></param>
        /// <param name="components"></param>
        public GuiList(string name, List<T> components = null) : base()
        {
            Name = name;
            if (components != null)
                Components = components;
            else
                Components = new List<T>();
        }

        /// <summary>
        /// fügt eien komponenten hinzu und passt größe & position an
        /// </summary>
        /// <param name="t"></param>
        public void Add(T t)
        {
            t.Location = new Vector(Location.X, Location.Y + Size.Y + 30);
            Components.Add(t);
            Size = new Vector(Size.X, Size.Y + t.Size.Y + 10);
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnResize(object sender, Vector e)
        {
            components.ForEach(x => x.Component_OnResize(e));
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
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
            };
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
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
            };
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
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
            };
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
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
            };
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Panel_OnKeyPress(object sender, char e)
        {
            bool flag = false;
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if (e == 9 && x.Selected) //falls tab gedrückt wird, wird der nächste ausgewählt
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
            };
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
            OnLeave += GuiList_OnLeave;
            OnEnter += GuiList_OnEnter;
            SetLocationAndSize(this, Size);

            components.ForEach(x =>
            {
                x.Init();
                x.SetLocationAndSize(this, Size);
            });
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GuiList_OnEnter(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                if(x.OnHover(e))
                    x.Component_OnEnter(e);
            }
        }

        /// <summary>
        /// gibt events weiter zu den komponenten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void GuiList_OnLeave(object sender, Vector e)
        {
            for (int i = components.Count - 1; i >= 0; i--)
            {
                GuiComponent x = components[i];
                x.Component_OnLeave(e);
            }
        }

        /// <summary>
        /// gibt die komponente wieder welche den namen trägt
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetComponent(string name)
        {
            return components.Find(x => x.Name == name);
        }

        /// <summary>
        /// zeichnet alle komponenten, passt die größe and
        /// und zeichnet den namen
        /// </summary>
        public override void OnRender()
        {
            StateManager.SetFont(new Font("Arial", 15, FontStyle.Bold));
            float height = StateManager.GetStringHeight(Name);
            StateManager.SetColor(Color.Black);
            StateManager.FillRect(Location.X - 5 + 20, Location.Y, 2, height);
            StateManager.DrawString(Name, Location.X  + 20, Location.Y);
            int offset = 0;
            for(int i = 0; i < components.Count; i++)
            {
                T t = components[i];
                t.Location = new Vector(Location.X, Location.Y + offset + 30);
                t.OnRender();
                offset += (int)t.Size.Y + 10;
            }
            if (offset + 30 > Size.Y)
                Size = new Vector(Size.X, offset + 30);
        }
    }
}
