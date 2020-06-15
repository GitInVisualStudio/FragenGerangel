using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    /// <summary>
    /// bildschirm zum anzeigen von komponenten
    /// </summary>
    public class GuiScreen : GuiPanel
    {
        private bool opend = true;
        public Animation animation = new Animation(10);
        public event EventHandler OnClose;

        public virtual void OnSroll(int direction)
        {

        }

        /// <summary>
        /// ob der screen offen ist
        /// </summary>
        public bool Opend
        {
            get
            {
                return opend;
            }

            set
            {
                opend = value;
            }
        }

        /// <summary>
        /// starten der animationen zum öffnen
        /// </summary>
        public GuiScreen() : base()
        {
            RWidth = 1;
            RHeight = 1;
            animation.OnFinish += Animation_OnFinish;
        }

        /// <summary>
        /// setzen ob der screen offen ist oder nicht
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Animation_OnFinish(object sender, bool e)
        {
            opend = animation.Incremental;
        }

        /// <summary>
        /// öffnet den screen
        /// </summary>
        public virtual void Open()
        {
            animation.Reset();
            animation.Fire();
        }

        /// <summary>
        /// schließt dens screen
        /// </summary>
        public virtual void Close()
        {
            if (animation.Delta < 1 && animation.Incremental)
                animation._OnFinish();
            animation.Reverse();
            OnClose?.Invoke(this, null);
        }
    }
}
