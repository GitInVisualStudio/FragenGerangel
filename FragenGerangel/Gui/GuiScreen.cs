using FragenGerangel.Utils;
using FragenGerangel.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Gui
{
    public class GuiScreen : GuiPanel
    {
        private bool opend = true;
        protected Animation animation = new Animation();
        public event EventHandler OnClose;
        private bool start = true;

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

        public GuiScreen() : base()
        {
            RWidth = 1;
            RHeight = 1;
            animation.Reverse();
            animation.OnFinish += Animation_OnFinish;
        }

        private void Animation_OnFinish(object sender, bool e)
        {
            opend = animation.Incremental;
        }

        public virtual void Open()
        {
            animation.Reset();
            animation.Reverse();
        }

        public virtual void Close()
        {
            animation.Reverse();
            OnClose?.Invoke(this, null);
        }
    }
}
